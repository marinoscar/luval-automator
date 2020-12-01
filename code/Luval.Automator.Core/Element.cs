using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Luval.Automator.Core
{
    public class Element
    {
        public AutomationElement Item { get; set; }
        public IEnumerable<ElementSelectionProperty> Properties { get; set; }

        public Image Picture { get; set; }

        public static Element FromAutomationElement(AutomationElement element)
        {
            return new Element() {
                Item = element,
                Properties = ElementSelectionProperty.FromElement(element)
            };
        }
    }
}
