using System;
using System.Diagnostics;
using System.Windows.Forms;
using WarBender.UI.Design;
using WarBender.UI.Properties;

namespace WarBender.UI {
    public static class Program {
        [STAThread]
        public static void Main() {
            if (Settings.Default.DebugConsole && !Debugger.IsAttached) {
                NativeMethods.AllocConsole();
                Console.OpenStandardOutput();
                Trace.Listeners.Add(new ConsoleTraceListener());
            }

            Trace.WriteLine($"WarBender v{typeof(Program).Assembly.GetName().Version}", nameof(Program));

            GameTypeDescriptionProvider.Register();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            Settings.Default.Save();
        }
    }
}
