﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Luval.Automator.Core
{
    public class WindowSelector
    {
        public WindowElement Find(int processId, string titleRegEx)
        {
            var handle = WindowsApiFunctions.GetWindowHandles().FirstOrDefault(i => i.ProcessId == processId && Regex.IsMatch(i.Title, titleRegEx, RegexOptions.IgnoreCase));
            if (handle == null) throw new ElementException(string.Format("Unable to find window for process Id: {0} and title regex {1}", processId, titleRegEx));
            return new WindowElement(AutomationElement.FromHandle(handle.Handle));
        }

        public WindowElement Find(IntPtr windowHandle)
        {
            var handle = WindowsApiFunctions.GetWindowHandles().FirstOrDefault(i => i.Handle == windowHandle);
            if (handle == null) throw new ElementException(string.Format("Unable to find window for handle: {0}", windowHandle));
            return new WindowElement(AutomationElement.FromHandle(handle.Handle));
        }

        public WindowElement Find(string processName, string titleRegEx)
        {
            var handle = WindowsApiFunctions.GetWindowHandles().FirstOrDefault(i => i.ProcessName == processName && Regex.IsMatch(i.Title, titleRegEx, RegexOptions.IgnoreCase));
            if (handle == null) throw new ElementException(string.Format("Unable to find window for process name: {0} and title regex {1}", processName, titleRegEx));
            return new WindowElement(AutomationElement.FromHandle(handle.Handle));
        }

        public WindowElement Find(string titleRegEx)
        {
            var handle = WindowsApiFunctions.GetWindowHandles().FirstOrDefault(i => Regex.IsMatch(i.Title, titleRegEx, RegexOptions.IgnoreCase));
            if (handle == null) throw new ElementException(string.Format("Unable to find window for title regex {0}", titleRegEx));
            return new WindowElement(AutomationElement.FromHandle(handle.Handle));
        }

        public IEnumerable<WindowElement> FindAllForProcess(string processName)
        {
            return WindowsApiFunctions.GetWindowHandles().Where(i => i.ProcessName == processName).Select(i => new WindowElement(AutomationElement.FromHandle(i.Handle)));
        }

        public IEnumerable<WindowElement> FindAllForProcess(int processId)
        {
            return WindowsApiFunctions.GetWindowHandles().Where(i => i.ProcessId == processId).Select(i => new WindowElement(AutomationElement.FromHandle(i.Handle)));
        }
    }
}
