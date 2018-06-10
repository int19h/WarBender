using System;
using System.Runtime.InteropServices;

namespace WarBender.UI {
    internal static class NativeMethods {
        public const uint TPM_LEFTALIGN = 0x0000;
        public const uint TPM_RETURNCMD = 0x0100;

        public const uint SC_MAXIMIZE = 0xF030;
        public const uint SC_RESTORE = 0xF120;

        public const int WM_SYSCOMMAND = 0x112;

        public const uint ECM_FIRST = 0x1500;
        public const uint EM_SETCUEBANNER = ECM_FIRST + 1;

        [DllImport("user32", SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32", SetLastError = true)]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        [DllImport("user32", SetLastError = true)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, string lParam);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool AllocConsole();
    }
}
