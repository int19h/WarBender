using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Microsoft.Win32;
using WarBender.Modules;
using WarBender.UI.Design;
using static WarBender.UI.NativeMethods;

namespace WarBender.UI {
    public partial class MainForm : FormBase {
        public static MainForm Instance;

        private string _initialText;
        private readonly ModelGetters _modelGetters = new ModelGetters();

        public MainForm() {
            if (Instance != null) {
                throw new InvalidOperationException($"There can only be one {nameof(MainForm)}");
            }
            Instance = this;

            InitializeComponent();
            _initialText = Text;

            Trace.Listeners.Add(new TextBoxTraceListener(richTextBoxLog));

            SendMessage(textBoxSearch.Handle, EM_SETCUEBANNER, 0, "Search (Ctrl+,)");

            openFileDialog.InitialDirectory = saveFileDialog.InitialDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Mount&Blade Warband Savegames");

            treeListView.SmallImageList = modelImageList.ModelImageList;
            treeListView.EmptyListMsgFont = Font;
            treeListView.CanExpandGetter = _modelGetters.CanExpand;
            treeListView.ChildrenGetter = _modelGetters.GetChildren;
            treeListViewNameColumn.AspectGetter = _modelGetters.GetName;
            treeListViewNameColumn.ImageGetter = _modelGetters.GetImage;

            var treeRenderer = treeListView.TreeColumnRenderer;
            treeRenderer.UseTriangles = true;
            treeRenderer.UseGdiTextRendering = true;
            treeRenderer.IsShowLines = false;

            if (WarbandPath != null) {
                openFileDialogModule.InitialDirectory = Path.Combine(WarbandPath, "Modules");
            }
        }

        internal GameDesignerHost DesignerHost { get; private set; }

        public string FileName { get; private set; }

        public bool IsDirty { get; private set; }

        public Game Game {
            get => DesignerHost?.Game;
            set {
                if (DesignerHost != null) {
                    DesignerHost.ComponentAdded -= DesignerHost_ComponentAdded;
                    DesignerHost.ComponentChanged -= DesignerHost_ComponentChanged;
                    DesignerHost.ComponentRemoved -= DesignerHost_ComponentRemoved;
                }

                DesignerHost = value == null ? null : new GameDesignerHost(value);

                if (DesignerHost != null) {
                    DesignerHost.ComponentAdded += DesignerHost_ComponentAdded;
                    DesignerHost.ComponentChanged += DesignerHost_ComponentChanged;
                    DesignerHost.ComponentRemoved += DesignerHost_ComponentRemoved;
                }

                _modelGetters.Game = value;
            }
        }

        private string WarbandPath {
            get {
                try {
                    var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                    var warbandKey = hklm?.OpenSubKey(@"SOFTWARE\mount&blade warband");
                    return warbandKey?.GetValue("install_path", null)?.ToString();
                } catch (UnauthorizedAccessException) {
                    return null;
                } catch (IOException) {
                    return null;
                }
            }
        }

        private void UpdateFileDialogs(string fileName) {
            openFileDialog.FileName = fileName;
            saveFileDialog.FileName = fileName;

            string path = null;
            try {
                path = Path.GetDirectoryName(fileName);
            } catch (Exception ex) {
                Trace.WriteLine(ex, nameof(MainForm));
                return;
            }

            openFileDialog.InitialDirectory = path;
            saveFileDialog.InitialDirectory = path;
        }

        private const string ModuleLinkFileName = "Module.lnk";

        private async Task WithModuleFor(Stopwatch sw, string fileName, Func<Module, Task> action) {
            Trace.WriteLine($"Determining module path for {fileName}", nameof(MainForm));
            using (var linkDeleter = new ScopeGuard()) {
                var savesPath = Path.GetDirectoryName(fileName);
                var linkPath = Path.Combine(savesPath, ModuleLinkFileName);
                if (!File.Exists(linkPath)) {
                    try {
                        File.WriteAllBytes(linkPath, Array.Empty<byte>());
                        linkDeleter.Add(() => File.Delete(linkPath));
                    } catch {
                    }
                }

                Shell32.ShellLinkObject link = null;
                try {
                    // Can't instantiate Shell32.Shell directly, because it will require IShellDispatch6
                    // if the app is built on Win8+, and then it'll fail at runtime on Win7. On the other
                    // hand, IShellDispatch5 is available on Win7.
                    var shellType = Type.GetTypeFromProgID("Shell.Application");
                    var shell = (Shell32.IShellDispatch5)Activator.CreateInstance(shellType);
                    var folder = shell.NameSpace(savesPath);
                    var folderItem = folder?.ParseName(ModuleLinkFileName);
                    link = folderItem?.IsLink == true ? folderItem.GetLink : null;
                } catch (Exception ex) {
                    // Shell APIs are brittle - if anything here fails, just ignore the link.
                    Trace.WriteLine(ex, nameof(MainForm));
                }

                string moduleName = null;
                var modulePath = link?.Path;
                if (string.IsNullOrEmpty(modulePath) && WarbandPath != null) {
                    moduleName = Path.GetFileName(savesPath);
                    Trace.WriteLine($"No module link found; guessing module name as '{moduleName}'", nameof(MainForm));
                    modulePath = Path.Combine(WarbandPath, "Modules", moduleName);
                    if (File.Exists(Path.Combine(modulePath, "module.ini"))) {
                        Trace.WriteLine($"Guessed module found at {modulePath}", nameof(MainForm));
                    } else {
                        Trace.WriteLine($"Guessed module does not exist at {modulePath}", nameof(MainForm));
                        moduleName = null;
                        modulePath = null;
                    }
                }

                var writeLink = false;
                if (string.IsNullOrEmpty(modulePath) || !File.Exists(Path.Combine(modulePath, "module.ini"))) {
                    if (!string.IsNullOrEmpty(modulePath)) {
                        Trace.WriteLine($"Found invalid module link to {modulePath}, replacing it", nameof(MainForm));
                    }

                    if (!string.IsNullOrEmpty(modulePath)) {
                        openFileDialogModule.InitialDirectory = modulePath;
                    }

                    sw.Stop();
                    try {
                        if (openFileDialogModule.ShowDialog() != DialogResult.OK) {
                            return;
                        }
                    } finally {
                        sw.Start();
                    }

                    modulePath = Path.GetDirectoryName(openFileDialogModule.FileName);
                    writeLink = true;
                } else {
                    Trace.WriteLine($"Found existing module link to {modulePath}", nameof(MainForm));
                }

                var wmmxFileName = Directory.GetFiles(modulePath, "*.wmmx").SingleOrDefault();
                if (wmmxFileName != null) {
                    Trace.WriteLine($"Module has an associated metadata file: {wmmxFileName}", nameof(MainForm));
                    moduleName = null;
                } else {
                    moduleName = moduleName ?? Path.GetFileName(modulePath);
                    Trace.WriteLine($"Module has no associated metadata file; assuming name '{moduleName}'", nameof(MainForm));
                    wmmxFileName = Path.Combine(typeof(Program).Assembly.Location, $@"..\Modules\{moduleName}.wmmx");
                    Trace.WriteLine($"Guessing metadata location: {wmmxFileName}", nameof(MainForm));
                }

                ModuleMetadata metadata = null;
                if (File.Exists(wmmxFileName)) {
                    Trace.WriteLine("Loading module metadata", nameof(MainForm));

                    loadMetadata: try {
                        metadata = new ModuleMetadata(wmmxFileName);
                    } catch (Exception ex) {
                        Trace.WriteLine(ex, nameof(MainForm));
                        var dr = MessageBox.Show(this,
                            $"Error loading module metadata from {wmmxFileName}:\r\n\r\n{ex.Message}",
                            null, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Abort) {
                            return;
                        } else if (dr == DialogResult.Retry) {
                            goto loadMetadata;
                        }
                    }

                    if (metadata != null) {
                        moduleName = moduleName ?? metadata.ModuleName;
                        Trace.WriteLine($"Loaded metadata for module '{moduleName}'", nameof(MainForm));
                    }
                } 

                if (metadata == null) { 
                    Trace.WriteLine("No module metadata found for this module", nameof(MainForm));

                    var dr = MessageBox.Show(this,
                        $"No module metadata found for module '{moduleName}'. Would you like to use Native module metadata instead?\r\n\r\n" +
                        $"Using Native metadata may provide wrong slot names or incorrect slot types for some objects.\r\n\r\n" +
                        $"Not using Native metadata means that all slots will be nameless and typeless.",
                        null, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Cancel) {
                        return;
                    } else if (dr == DialogResult.Yes) {
                        metadata = ModuleMetadata.Native;
                    } else {
                        metadata = null;
                    }
                }

                Module module = null;
                loadModule: try {
                    Trace.WriteLine("Loading module", nameof(MainForm));
                    module = new Module(modulePath, metadata);
                } catch (Exception ex) {
                    Trace.WriteLine(ex, nameof(MainForm));
                    var dr = MessageBox.Show(this,
                        $"Error loading module data for {moduleName} from {modulePath}:\r\n\r\n{ex.Message}",
                        null, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (dr == DialogResult.Retry) {
                        goto loadModule;
                    } else {
                        return;
                    }
                }

                await action(module);

                if (writeLink && link != null) {
                    Trace.WriteLine($"Creating link to module: {linkPath} -> {modulePath}", nameof(MainForm));
                    try {
                        link.Path = modulePath;
                        link.Description = "Mount & Blade module used by saved games in this folder.";
                        link.Save();
                        linkDeleter.Disarm();
                    } catch (Exception ex) {
                        Trace.WriteLine(ex, nameof(MainForm));
                    }
                }
            }
        }

        public async Task OpenAsync(string fileName) {
            if (!ConfirmDiscardingChanges()) {
                return;
            }

            UpdateFileDialogs(fileName);
            UseWaitCursor = true;
            openToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            var oldMsg = treeListView.EmptyListMsg;
            treeListView.EmptyListMsg = "Loading ...";

            load: try {
                var sw = Stopwatch.StartNew();
                Trace.WriteLine($"Opening {fileName}", nameof(MainForm));

                treeListView.Objects = Array.Empty<object>();
                foreach (var mdiChild in MdiChildren) {
                    mdiChild.Close();
                }
                Game = null;
                IsDirty = false;
                Text = _initialText;

                Game game = null;
                Exception roundtripError = null;
                await WithModuleFor(sw, fileName, async module => {
                    await Task.Run(() => {
                        using (var stream = File.OpenRead(fileName)) {
                            game = Game.Load(stream, module);
                            stream.Position = 0;
                            try {
                                game.Save(new VerifyingStream(stream));
                            } catch (Exception ex) {
                                Trace.WriteLine(ex, nameof(MainForm));
                                roundtripError = ex;
                            }
                        }
                    });
                });
                if (game == null) {
                    return;
                }

                Game = game;
                treeListView.Objects = new[] { Game.Data };
                treeListView.ExpandAll();
                treeListView.CollapseAll();
                treeListView.Expand(Game.Data);
                ShowPropertyGrid(treeListView.Objects);

                FileName = fileName;
                Text = Path.GetFileName(fileName) + " - " + _initialText;

                if (roundtripError != null) {
                    var dr = MessageBox.Show(this,
                        "Loaded game cannot be round-tripped correctly by WarBender:\r\n\r\n" +
                        roundtripError.Message +
                        "\r\n\r\nData corruption is likely if this game is saved!",
                        null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                sw.Stop();
                Trace.WriteLine($"Opened in {sw.Elapsed}", nameof(MainForm));

            } catch (Exception ex) {
                Trace.WriteLine(ex, nameof(MainForm));
                var dr = MessageBox.Show(this, $"Error loading {fileName}:\r\n\r\n{ex.Message}", null, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (dr == DialogResult.Retry) {
                    goto load;
                }
            } finally {
                treeListView.EmptyListMsg = oldMsg;
                openToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = Game != null;
                UseWaitCursor = false;
            }
        }

        public async Task SaveAsync(string fileName) {
            UpdateFileDialogs(fileName);
            UseWaitCursor = true;
            openToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;

            save: try {
                var sw = Stopwatch.StartNew();
                Trace.WriteLine($"Saving as {fileName}", nameof(MainForm));

                var tempFileName = Path.GetTempFileName();
                Trace.WriteLine($"Temporary file: {tempFileName}", nameof(MainForm));

                Exception saveError = null;
                await Task.Run(() => {
                    try {
                        Game.Save(tempFileName);
                    } catch (Exception ex) {
                        Trace.WriteLine(ex, nameof(MainForm));
                        saveError = ex;
                    }
                });

                if (saveError != null) {
                    var dr = MessageBox.Show(this,
                        $"Error writing to temporary file {tempFileName}:\r\n\r\n" +
                        $"{saveError.Message}",
                        null, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (dr == DialogResult.Retry) {
                        goto save;
                    }
                    return;
                }

                string backupFileName = null;
                backup: if (File.Exists(fileName)) {
                    Trace.WriteLine($"{fileName} already exists!", nameof(MainForm));
                    backupFileName = fileName + "~" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    Trace.WriteLine($"Backing up existing file to: {backupFileName}", nameof(MainForm));

                    try {
                        File.Move(fileName, backupFileName);
                    } catch (Exception ex) {
                        Trace.WriteLine(ex, nameof(MainForm));
                        var dr = MessageBox.Show(this,
                            $"Couldn't back up {fileName}:\r\n\r\n{ex.Message}\r\n\r\n" +
                            $"Try again, or ignore the problem and overwrite it?",
                            null, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                        switch (dr) {
                            case DialogResult.Abort:
                                return;
                            case DialogResult.Retry:
                                goto backup;
                        }
                    }
                }

                Trace.WriteLine($"Moving {tempFileName} to {fileName}", nameof(MainForm));
                move: try {
                    File.Move(tempFileName, fileName);
                } catch (Exception ex) {
                    Trace.WriteLine(ex, nameof(MainForm));
                    var msg = $"Error writing to {fileName}:\r\n\r\n{ex.Message}";
                    if (backupFileName != null) {
                        msg += $"\r\n\r\nYour original save was backed up to {backupFileName}";
                    }
                    var dr = MessageBox.Show(this, msg, null, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (dr == DialogResult.Retry) {
                        goto move;
                    }
                }

                IsDirty = false;
                FileName = fileName;
                Text = Path.GetFileName(fileName) + " - " + _initialText;

                sw.Stop();
                Trace.WriteLine($"Saved in {sw.Elapsed}", nameof(MainForm));
            } catch (Exception ex) {
                Trace.WriteLine(ex, nameof(MainForm));
                var dr = MessageBox.Show(this,
                    $"Error saving to {fileName}:\r\n\r\n{ex.Message}",
                    null, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (dr == DialogResult.Retry) {
                    goto save;
                }
            } finally {
                openToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = Game != null;
                UseWaitCursor = false;
            }
        }

        public void HighlightObjects(IList objs) {
            if (objs.Count > 0) {
                treeListView.SelectObjects(objs);
                treeListView.EnsureModelVisible(objs[0]);
            }
        }

        public void ShowPropertyGrid(IReadOnlyCollection<object> objects) {
            PropertyGridForm gridForm = null;
            foreach (var mdiChild in MdiChildren) {
                gridForm = mdiChild as PropertyGridForm;
                if (gridForm?.Objects.SequenceEqual(objects) == true) {
                    break;
                }
                gridForm = null;
            }

            gridForm = gridForm ?? new PropertyGridForm();
            gridForm.DesignerHost = DesignerHost;
            gridForm.Objects = objects;
            gridForm.MdiParent = this;

            if (ActiveMdiChild == null) {
                gridForm.WindowState = FormWindowState.Maximized;
            }

            gridForm.Show();
            gridForm.Activate();
        }

        private void ShowPropertyGrid(IEnumerable objects) => ShowPropertyGrid(objects.Cast<object>().ToArray());

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == (Keys.Control | Keys.Oemcomma)) {
                textBoxSearch.Focus();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ProcessComponentChange(object component) {
            if (component is IDataObjectChild) {
                IsDirty = true;
            }
        }

        private bool ConfirmDiscardingChanges() {
            if (!IsDirty) {
                return true;
            }
            var dr = MessageBox.Show(this,
                "You have unsaved changes. If you continue with this operation, " +
                "they will be lost. Are you sure you want to proceed?",
                null, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return dr == DialogResult.Yes;
        }

        private void menuStrip_ItemAdded(object sender, ToolStripItemEventArgs e) {
            // Hide MDI child icon.
            if (e.Item.GetType().ToString() == "System.Windows.Forms.MdiControlStrip+SystemMenuItem") {
                e.Item.Visible = false;
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
            //var console = GetConsoleWindow();
            //if (console != IntPtr.Zero) {
            //    var mdiClient = Controls.OfType<MdiClient>().Single();
            //    SetParent(console, mdiClient.Handle);
            //    SetWindowLong(console, GWL_STYLE, WS_CHILD | WS_MAXIMIZE | WS_HSCROLL | WS_VSCROLL | WS_VISIBLE);
            //    showConsoleToolStripMenuItem.Visible = true;
            //    mdiClient.Resize += delegate {
            //        SetWindowPos(console, IntPtr.Zero, 0, 0, mdiClient.ClientRectangle.Width, mdiClient.ClientRectangle.Height, 0);
            //    };
            //}

            var args = Environment.GetCommandLineArgs();
            if (args.Length == 2) {
                OpenAsync(args[1]).GetAwaiter();
            } else if (args.Length == 1) {
                openToolStripMenuItem.PerformClick();
            } else {
                MessageBox.Show(
                    this,
                    "Incorrect number of command line arguments (expected zero or one):\r\n\r\n" + Environment.CommandLine,
                    null,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                Show();
                Activate();
                OpenAsync(openFileDialog.FileName).GetAwaiter();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                SaveAsync(saveFileDialog.FileName).GetAwaiter();
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e) {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void searchTimer_Tick(object sender, EventArgs e) {
            searchTimer.Stop();
            if (string.IsNullOrWhiteSpace(textBoxSearch.Text)) {
                treeListView.ModelFilter = null;
                treeListView.DefaultRenderer = null;
            } else {
                var textFilter = TextMatchFilter.Contains(treeListView, textBoxSearch.Text);
                var treeFilter = new TreeFilter(treeListView, textFilter);
                var filter = new CompositeAnyFilter(new List<IModelFilter> { textFilter, treeFilter });
                treeListView.DefaultRenderer = new HighlightTextRenderer();
                treeListView.ModelFilter = filter;
            }
            treeListView.Refresh();
        }

        private void treeListView_CellToolTipShowing(object sender, ToolTipShowingEventArgs e) {
            e.Text = _modelGetters.GetToolTip(e.Model);
            e.Handled = true;
        }

        private void treeListView_ItemActivate(object sender, EventArgs e) {
            ShowPropertyGrid(treeListView.SelectedObjects);
        }

        private void openContextMenuItem_Click(object sender, EventArgs e) {
            ShowPropertyGrid(treeListView.SelectedObjects);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            new AboutBox().ShowDialog(this);
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowPropertyGrid(Enumerable.Empty<object>());
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (var mdiChild in MdiChildren.ToArray()) {
                mdiChild.Close();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            new SettingsForm().ShowDialog(this);
        }

        private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (var mdiChild in MdiChildren) {
                mdiChild.WindowState = FormWindowState.Minimized;
            }
        }

        private void DesignerHost_ComponentRemoved(object sender, ComponentEventArgs e) {
            ProcessComponentChange(e.Component);
        }

        private void DesignerHost_ComponentChanged(object sender, ComponentChangedEventArgs e) {
            ProcessComponentChange(e.Component);
        }

        private void DesignerHost_ComponentAdded(object sender, ComponentEventArgs e) {
            ProcessComponentChange(e.Component);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (!ConfirmDiscardingChanges()) {
                e.Cancel = true;
            }
        }
    }
}
