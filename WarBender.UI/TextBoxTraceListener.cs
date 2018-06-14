using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WarBender.UI {
    internal class TextBoxTraceListener : TraceListener {
        private readonly TextBoxBase _textBox;

        public TextBoxTraceListener(TextBoxBase textBox) {
            _textBox = textBox;
        }

        public override void Write(string message) {
            message = message.Replace("\n", "\r\n");
            _textBox.BeginInvoke((Action)delegate {
                _textBox.AppendText(message);
                _textBox.ScrollToCaret();
            });
        }

        public override void WriteLine(string message) => Write(message + "\n");
    }
}
