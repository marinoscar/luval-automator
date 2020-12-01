using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Automator.Core
{
    public static class WindowsApiFunctions
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "EnumDesktopWindows", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        private delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        private static List<IntPtr> _windowHandles;
        private static List<string> _windowTitles;

        private static bool FilterCallback(IntPtr hWnd, int lParam)
        {
            // Get the window's title.
            StringBuilder sb_title = new StringBuilder(1024);
            int length = GetWindowText(hWnd, sb_title, sb_title.Capacity);
            string title = sb_title.ToString();

            // If the window is visible and has a title, save it.
            if (IsWindowVisible(hWnd) &&
                string.IsNullOrEmpty(title) == false)
            {
                _windowHandles.Add(hWnd);
                _windowTitles.Add(title);
            }

            // Return true to indicate that we
            // should continue enumerating windows.
            return true;
        }

        public static IEnumerable<WindowHandle> GetWindowHandles()
        {
            var res = new List<WindowHandle>();

            _windowHandles = new List<IntPtr>();
            _windowTitles = new List<string>();

            if (!EnumDesktopWindows(IntPtr.Zero, FilterCallback, IntPtr.Zero))
                return res;
            else
            {
                for (int i = 0; i < _windowHandles.Count; i++)
                {
                    uint pid = 0;
                    var processId = GetWindowThreadProcessId(_windowHandles[i], out pid);
                    var process = Process.GetProcessById(Convert.ToInt32(pid));
                    res.Add(new WindowHandle() { Handle = _windowHandles[i], Title = _windowTitles[i], ProcessId = process.Id, ProcessName = process.ProcessName });
                }
            }
            return res;
        }

        
    }
}
