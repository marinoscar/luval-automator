using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Xml.Linq;

namespace Luval.Automator.Core
{
    public class ElementProperty
    {
        public string Root { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public string Value { get; set; }

        public string GetPropertyFullName()
        {
            return !string.IsNullOrWhiteSpace(Root) ? string.Format("{0}.{1}", Root, Name) : Name;
        }


        public static ElementProperty FromElementProperty(AutomationProperty property, AutomationElement element)
        {
            var nameParts = property.ProgrammaticName.Split('.');
            return new ElementProperty()
            {
                Value = Convert.ToString(element.GetCurrentPropertyValue(property)),
                Root = nameParts.Length > 1 ? nameParts[0] : string.Empty,
                Name = nameParts.Length <= 1 ? nameParts[0] : nameParts[1]
            };
        }

        public static IEnumerable<ElementProperty> FromElement(AutomationElement element)
        {
            var res = new List<ElementProperty>();
            foreach (var prop in element.GetSupportedProperties())
            {
                res.Add(FromElementProperty(prop, element));
            }
            return res;
        }
    }
}
