using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Automator.Core
{
    public static class WindowsApiFunctions
    {
        #region Private API Calls

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr handle, ref Rectangle rect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

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


        #endregion


        private static Rectangle GetRelativeRec(Rectangle windowRec, Rectangle elementRec)
        {
            return new Rectangle(
                (elementRec.X - windowRec.X), //relative X
                (elementRec.Y - windowRec.Y), //relative Y
                elementRec.Width, 
                elementRec.Height);
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

        public static Image CaptureWindow(IntPtr handle)
        {
            // Get the size of the window to capture
            Rectangle rect = new Rectangle();
            GetWindowRect(handle, ref rect);

            // GetWindowRect returns Top/Left and Bottom/Right, so fix it
            rect.Width = rect.Width - rect.X;
            rect.Height = rect.Height - rect.Y;

            var bitmap = new Bitmap(rect.Width, rect.Height);
            // Use PrintWindow to draw the window into our bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                IntPtr hdc = g.GetHdc();
                if (!PrintWindow(handle, hdc, 0))
                {
                    int error = Marshal.GetLastWin32Error();
                    var exception = new System.ComponentModel.Win32Exception(error);
                    Debug.WriteLine("ERROR: " + error + ": " + exception.Message);
                }
                g.ReleaseHdc(hdc);
            }
            return bitmap;
        }

        public static Image CaptureElement(IntPtr handle, Element element)
        {
            var windowRec = default(Rectangle);
            GetWindowRect(handle, ref windowRec);

            var elementRec = new Rectangle((int)element.Item.Current.BoundingRectangle.X, (int)element.Item.Current.BoundingRectangle.Y, (int)element.Item.Current.BoundingRectangle.Width, (int)element.Item.Current.BoundingRectangle.Height);
            var cropImg = new Bitmap(elementRec.Width, elementRec.Height);
            using (var windowImg = new Bitmap(CaptureWindow(handle)))
            {
                if (elementRec.Width == windowImg.Width && elementRec.Height == windowImg.Height) return (Bitmap)windowImg.Clone();
                return windowImg.Clone(GetRelativeRec(windowRec, elementRec), windowImg.PixelFormat);
            }
        }
        
    }
}
