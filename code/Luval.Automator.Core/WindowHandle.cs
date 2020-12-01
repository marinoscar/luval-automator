using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Automator.Core
{
    public class WindowHandle
    {
        public IntPtr Handle { get; set; }
        public string Title { get; set; }

        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
    }
}
