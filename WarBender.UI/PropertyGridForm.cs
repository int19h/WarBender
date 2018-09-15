using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using WarBender.GameData;
using WarBender.UI.Design;

namespace WarBender.UI {
    public partial class PropertyGridForm : Form {
        private readonly ModelGetters _modelGetters = new ModelGetters();
        private Icon _defaultIcon;
        private object[] _objects = { };
        private int _rowHeight;
        private bool _autoSize = true;

        public PropertyGridForm() {
            InitializeComponent();
            listView.EmptyListMsgFont = listView.Font;
            showInTreeToolStripMenuItem.Font = new Font(showInTreeToolStripMenuItem.Font, FontStyle.Bold);

            _defaultIcon = Icon;
            _rowHeight = listView.Height;

            listView.SmallImageList = sharedImageLists.ModelImageList;
            columnName.AspectGetter = _modelGetters.GetName;
            columnName.ImageGetter = _modelGetters.GetImage;
        }

        public Game Game => DesignerHost.Game;

        internal GameDesignerHost DesignerHost {
            get => (GameDesignerHost)propertyGrid.Site;
            set {
                _modelGetters.Game = value.Game;
                propertyGrid.Site = value;
            }
        }

        public IReadOnlyCollection<object> Objects {
            get => _objects;
            set {
                _objects = value.ToArray();
                propertyGrid.SelectedObjects = _objects;
                listView.Objects = _objects;
                ResizeListView();

                var images = _objects.Select(obj => _modelGetters.GetImage(obj)).Distinct().ToArray();
                Image image = null;
                if (images.Length == 1) {
                    image = sharedImageLists.ModelImageList.Images[images[0]];
                }
                if (image == null) {
                    image = sharedImageLists.ModelImageList.Images["Record"];
                }
                Icon = Icon.FromHandle(new Bitmap(image).GetHicon());

                if (_objects.Length == 1) {
                    var obj = _objects[0];
                    switch (obj) {
                        case SavedGame data:
                            Text = (string)_modelGetters.GetName(data);
                            break;
                        case IHasName hasName:
                            Text = hasName.Name;
                            break;
                        case IHasId hasId:
                            Text = hasId.Id;
                            break;
                        case IRecord record:
                            Text = record.Type.Name;
                            if (record.Index >= 0) {
                                Text += " #" + record.Index;
                            }
                            break;
                        case ICollection coll:
                            Text = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(FriendlyNames.Plural(coll.ItemType));
                            break;
                        default:
                            Text = obj.GetType().Name;
                            break;
                    }
                } else {
                    Text = FriendlyNames.Describe(_objects);
                }
            }
        }

        private bool _resizing;

        private void ResizeListView() {
            if (_resizing) {
                return;
            }

            _resizing = true;
            try {
                listView.Height = _rowHeight;
                for (int i = 0; i < 2; ++i) {
                    listView.AutoResizeColumns();
                    listView.ArrangeIcons(ListViewAlignment.Left);
                    bool done = true;
                    for (int j = 0; j < listView.Items.Count; ++j) {
                        Rectangle r;
                        try {
                            r = listView.GetItemRect(j, ItemBoundsPortion.Entire);
                        } catch {
                            // Can happen when form is being destroyed.
                            return;
                        }
                        if (!listView.ClientRectangle.Contains(r)) {
                            listView.Height += _rowHeight;
                            done = false;
                            break;
                        }
                    }
                    if (done) {
                        break;
                    }
                }
            } finally {
                _resizing = false;
            }
        }

        private void listView_CellToolTipShowing(object sender, ToolTipShowingEventArgs e) {
            e.Text = _modelGetters.GetToolTip(e.Model);
            e.Handled = true;
        }

        private void listView_ItemActivate(object sender, EventArgs e) {
            MainForm.Instance.HighlightObjects(listView.SelectedObjects);
        }

        private void listView_CellRightClick(object sender, CellRightClickEventArgs e) {
            if (e.Model != null) {
                e.MenuStrip = contextMenuStrip;
            }
        }

        private void showInTreeToolStripMenuItem_Click(object sender, EventArgs e) {
            MainForm.Instance.HighlightObjects(listView.SelectedObjects);
        }

        private void openInNewWindowToolStripMenuItem_Click(object sender, EventArgs e) {
            MainForm.Instance.ShowPropertyGrid(listView.SelectedObjects.Cast<object>().ToArray());
        }

        private void removeFromThisWindowToolStripMenuItem_Click(object sender, EventArgs e) {
            var removed = listView.SelectedObjects.Cast<object>();
            Objects = Objects.Except(removed).ToArray();
            if (Objects.Count == 0) {
                Close();
            }
        }

        private void listView_ModelCanDrop(object sender, ModelDropEventArgs e) {
            if (e.TargetModel == null && e.SourceModels.Cast<object>().All(obj => obj is IDataObject)) {
                e.Effect = DragDropEffects.Link;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listView_ModelDropped(object sender, ModelDropEventArgs e) {
            Objects = Objects.Union(e.SourceModels.Cast<object>()).ToArray();
        }

        private void listView_SizeChanged(object sender, EventArgs e) {
            if (_autoSize) {
                ResizeListView();
            }
        }

        private void splitter_SplitterMoving(object sender, SplitterEventArgs e) {
            _autoSize = false;
        }
    }
}
