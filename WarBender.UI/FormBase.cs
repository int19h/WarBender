using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WarBender.UI
{
    public class FormBase : Form, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private WindowRestoreStateInfo _windowRestoreState;

        [Browsable(false)]
        [SettingsBindable(true)]
        public WindowRestoreStateInfo WindowRestoreState {
            get { return _windowRestoreState; }
            set {
                _windowRestoreState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WindowRestoreState"));
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            WindowRestoreState = new WindowRestoreStateInfo();
            WindowRestoreState.Bounds = WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;
            WindowRestoreState.WindowState = WindowState;
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            if (WindowRestoreState != null) {
                Bounds = ConstrainToScreen(WindowRestoreState.Bounds);
                WindowState = WindowRestoreState.WindowState;
            }
        }

        private Rectangle ConstrainToScreen(Rectangle bounds) {
            var screen = Screen.FromRectangle(WindowRestoreState.Bounds);
            var workingArea = screen.WorkingArea;
            var width = Math.Min(bounds.Width, workingArea.Width);
            var height = Math.Min(bounds.Height, workingArea.Height);
            var left = Math.Min(workingArea.Right - width, Math.Max(bounds.Left, workingArea.Left));
            var top = Math.Min(workingArea.Bottom - height, Math.Max(bounds.Top, workingArea.Top));
            return new Rectangle(left, top, width, height);
        }
    }

    public class WindowRestoreStateInfo {
        public Rectangle Bounds { get; set; }
        public FormWindowState WindowState { get; set; }
    }
}
