using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static WarBender.UI.NativeMethods;

namespace WarBender.UI {
    public class MdiTabStrip : ToolStrip {
        public Form MdiParent { get; private set; }

        private readonly Dictionary<Form, ToolStripButton> _mdiChildren =
            new Dictionary<Form, ToolStripButton>();

        public MdiTabStrip() {
            Visible = false;
        }

        private void UpdateButton(ToolStripButton button) {
            var mdiChild = (Form)button.Tag;
            button.Text = mdiChild.Text;
            button.Image = mdiChild.Icon.ToBitmap();
        }

        private void CreateButtonsFor(IEnumerable<Form> mdiChidren) {
            foreach (var mdiChild in mdiChidren) {
                mdiChild.FormClosed += MdiChild_FormClosed;
                mdiChild.Activated += MdiChild_Activated;
                mdiChild.Deactivate += MdiChild_Deactivate;
                mdiChild.Resize += MdiChild_Resize;
                mdiChild.TextChanged += MdiChild_TextChanged;

                var button = new ToolStripButton() {
                    Tag = mdiChild,
                    DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                    ImageScaling = ToolStripItemImageScaling.None,
                };
                UpdateButton(button);
                button.Click += Button_Click;
                button.MouseDown += Button_MouseDown;

                _mdiChildren[mdiChild] = button;
                Items.Add(button);
            }
        }

        protected override void OnParentChanged(EventArgs e) {
            if (MdiParent != null) {
                MdiParent.MdiChildActivate -= MdiParent_MdiChildActivate;
            }

            foreach (var button in _mdiChildren.Values) {
                Items.Remove(button);
            }
            _mdiChildren.Clear();

            base.OnParentChanged(e);

            MdiParent = FindForm();
            if (MdiParent != null) {
                MdiParent.MdiChildActivate += MdiParent_MdiChildActivate;
                CreateButtonsFor(MdiParent.MdiChildren);
            }

            Visible = _mdiChildren.Any();
        }

        private void MdiParent_MdiChildActivate(object sender, EventArgs e) {
            var newChildren = MdiParent.MdiChildren.Except(_mdiChildren.Keys).Where(form => !form.Disposing);
            CreateButtonsFor(newChildren);
            Visible = _mdiChildren.Any();
        }

        private void MdiChild_Resize(object sender, EventArgs e) {
            var mdiChild = (Form)sender;
            if (mdiChild.WindowState == FormWindowState.Minimized) {
                mdiChild.Hide();
            }
        }

        private void MdiChild_TextChanged(object sender, EventArgs e) {
            var mdiChild = (Form)sender;
            var button = _mdiChildren[mdiChild];
            UpdateButton(button);
        }

        private void MdiChild_FormClosed(object sender, FormClosedEventArgs e) {
            var mdiChild = (Form)sender;

            mdiChild.FormClosed -= MdiChild_FormClosed;
            mdiChild.Activated -= MdiChild_Activated;
            mdiChild.Deactivate -= MdiChild_Deactivate;
            mdiChild.Resize -= MdiChild_Resize;
            mdiChild.TextChanged -= MdiChild_TextChanged;

            var button = Items.OfType<ToolStripButton>().Single(b => b.Tag == mdiChild);
            Items.Remove(button);
            _mdiChildren.Remove(mdiChild);

            Visible = _mdiChildren.Any();
        }

        private void MdiChild_Activated(object sender, EventArgs e) {
            _mdiChildren[(Form)sender].Checked = true;
        }

        private void MdiChild_Deactivate(object sender, EventArgs e) {
            _mdiChildren[(Form)sender].Checked = false;
        }

        private void Button_Click(object sender, EventArgs e) {
            var mdiChild = (Form)((ToolStripButton)sender).Tag;

            if (MdiParent.ActiveMdiChild?.WindowState == FormWindowState.Maximized) {
                mdiChild.WindowState = FormWindowState.Maximized;
            } else if (mdiChild.WindowState == FormWindowState.Minimized) {
                mdiChild.WindowState = FormWindowState.Normal;
            }

            mdiChild.Show();
            mdiChild.Activate();
        }

        private void Button_MouseDown(object sender, MouseEventArgs e) {
            var mdiChild = (Form)((ToolStripButton)sender).Tag;
            if (e.Button == MouseButtons.Middle) {
                mdiChild.Close();
            } else if (e.Button == MouseButtons.Right) {
                var hSysMenu = GetSystemMenu(mdiChild.Handle, false);
                var command = TrackPopupMenuEx(
                    hSysMenu,
                    TPM_LEFTALIGN | TPM_RETURNCMD,
                    Cursor.Position.X,
                    Cursor.Position.Y,
                    MdiParent.Handle,
                    IntPtr.Zero);
                if (command != 0) {
                    PostMessage(mdiChild.Handle, WM_SYSCOMMAND, (IntPtr)command, IntPtr.Zero);
                }
            }
        }
    }
}
