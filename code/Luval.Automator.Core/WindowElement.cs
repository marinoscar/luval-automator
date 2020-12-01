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
        public WindowElement()
        {

        }

        public WindowElement(AutomationElement element) : base(element)
        {

        }
    }
}
