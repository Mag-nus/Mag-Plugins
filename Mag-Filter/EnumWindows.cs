using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinUtil
{
    public class WinEnum
    {
        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn,
            IntPtr lParam);

        public static IEnumerable<IntPtr> EnumerateProcessWindowHandles(int processId)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
                EnumThreadWindows(thread.Id,
                    (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles;
        }

        public const uint WM_GETTEXT = 0x000D;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam,
            StringBuilder lParam);

        [DllImport("user32.dll")]
        public static extern bool SetWindowText(IntPtr hWnd, string text);

    }
}
