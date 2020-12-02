using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Luval.Automator.Core
{
    public class WindowElement : Element
    {

        public WindowElement(AutomationElement element) : base(element)
        {
            Handle = new IntPtr(Item.Current.NativeWindowHandle);
        }

        public IntPtr Handle { get; private set; }

        public void Close()
        {
            WindowsApiFunctions.CloseWindow(new IntPtr(Item.Current.NativeWindowHandle));
        }

        public void Activate()
        {
            WindowsApiFunctions.SetWindowState(Handle, WindowState.SW_SHOWNORMAL);
            WindowsApiFunctions.SetForegroundWindow(Handle);
        }

        public void Minimize()
        {
            WindowsApiFunctions.SetWindowState(Handle, WindowState.SW_FORCEMINIMIZE);
        }

        public void Maximize()
        {
            WindowsApiFunctions.SetWindowState(Handle, WindowState.SW_SHOWMAXIMIZED);
        }

    }
}
