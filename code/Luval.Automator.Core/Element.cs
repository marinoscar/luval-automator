using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Luval.Automator.Core
{
    public class Element
    {
        public Element()
        {

        }

        public Element(AutomationElement automationElement)
        {
            Item = automationElement;
            Properties = ElementProperty.FromElement(automationElement);
        }

        private XElement _xml;
        private List<Element> _children;

        public AutomationElement Item { get; set; }
        public IEnumerable<ElementProperty> Properties { get; set; }
        public IEnumerable<Element> Children
        {
            get
            {
                if (_children == null)
                    LoadChildren();
                return _children;
            }
        }
        public Image Picture { get; set; }
        public XElement Xml
        {
            get
            {
                if (_xml == null)
                    LoadXml();
                return _xml;
            }
        }


        public ElementActionResult SetText(string value)
        {
            if (value == null)
                return ElementActionResult.CreateError("Text value cannot be null");
            if (Item == null)
                return ElementActionResult.CreateError("Automation element not provided");
            if (!Item.Current.IsEnabled)
                return ElementActionResult.CreateError("Automation element {0} is not enabled", Item.Current.AutomationId);
            if (!Item.Current.IsKeyboardFocusable)
                return ElementActionResult.CreateError("Automation element {0} is read-only", Item.Current.AutomationId);

            object valuePattern = null;
            try
            {
                if (Item.TryGetCurrentPattern(
                    ValuePattern.Pattern, out valuePattern))
                {
                    //Try to set the value
                    Item.SetFocus();
                    ((ValuePattern)valuePattern).SetValue(value);

                }
                else
                {
                    //If value property is not supported, then uses the keyboard
                    Item.SetFocus();
                    Thread.Sleep(300);
                    SendKeys.SendWait("^{HOME}");   // Move to start of control
                    SendKeys.SendWait("^+{END}");   // Select everything
                    SendKeys.SendWait("{DEL}");     // Delete selection
                    SendKeys.SendWait(value);
                }
            }
            catch (Exception ex)
            {
                return ElementActionResult.CreateError(ex, "Automation element {0} text operation failed with external error {1}", Item.Current.AutomationId, ex.Message);
            }

            return ElementActionResult.CreateSuccess();
        }

        public ElementActionResult GetText()
        {
            if (Item == null)
                return ElementActionResult.CreateError("Automation element not provided");

            object valuePattern = null;
            try
            {
                if (Item.TryGetCurrentPattern(
                    ValuePattern.Pattern, out valuePattern))
                {
                    //Try to set the value
                    Item.SetFocus();
                    return ElementActionResult.CreateSuccess(((ValuePattern)valuePattern).Current.Value);

                }
                else
                {
                    //If value property is not supported, then uses the keyboard
                    Item.SetFocus();
                    Thread.Sleep(300);
                    SendKeys.SendWait("^{HOME}");   // Move to start of control
                    SendKeys.SendWait("^+{END}");   // Select everything
                    SendKeys.SendWait("^(c)");
                    Thread.Sleep(100);
                    if (Clipboard.ContainsText(TextDataFormat.Text))
                        return ElementActionResult.CreateSuccess(Clipboard.GetText(TextDataFormat.Text));

                }
            }
            catch (Exception ex)
            {
                return ElementActionResult.CreateError(ex, "Automation element {0} text operation failed with external error {1}", Item.Current.AutomationId, ex.Message);
            }

            return ElementActionResult.CreateError("Unable to perform the text operation on element {0}", Item.Current.AutomationId);
        }

        public ElementActionResult Click()
        {
            if (Item == null)
                return ElementActionResult.CreateError("Automation element not provided");

            object invokePattern = null;

            try
            {
                if (Item.TryGetCurrentPattern(
                    InvokePattern.Pattern, out invokePattern))
                {
                    Item.SetFocus();
                    ((InvokePattern)invokePattern).Invoke();
                    Thread.Sleep(100);
                }
                else
                {
                    Item.SetFocus();
                    Thread.Sleep(300);
                    var p = default(System.Windows.Point);
                    if (Item.TryGetClickablePoint(out p))
                        WindowsApiFunctions.MouseClick(MouseClickType.Left, (int)p.X, (int)p.Y);
                    else
                    {
                        var calculatedPoint = new Point(
                            Convert.ToInt32(Item.Current.BoundingRectangle.X + (Item.Current.BoundingRectangle.Width / 2)),
                            Convert.ToInt32(Item.Current.BoundingRectangle.Y + (Item.Current.BoundingRectangle.Height / 2)));
                        WindowsApiFunctions.MouseClick(MouseClickType.Left, calculatedPoint.X, calculatedPoint.Y);
                    }

                }
            }
            catch (Exception ex)
            {
                return ElementActionResult.CreateError(ex, "Automation element {0} text operation failed with external error {1}", Item.Current.AutomationId, ex.Message);
            }

            return ElementActionResult.CreateSuccess();
        }

        protected virtual void LoadChildren()
        {
            _children = new List<Element>();
            foreach (var el in Item.FindAll(TreeScope.Children, Condition.TrueCondition).Cast<AutomationElement>())
                _children.Add(Element.FromAutomationElement(el));
        }

        protected virtual void LoadXml()
        {
            _xml = new XElement("Element");
            foreach (var prop in Properties)
            {
                var child = new XElement(prop.Name, prop.Value);
                child.SetAttributeValue("FullName", string.Format("{0}.{1}", prop.Root, prop.Name));
                _xml.Add(child);
            }
            _xml.SetAttributeValue("AutomationId", Item.Current.AutomationId);
            _xml.SetAttributeValue("Name", Item.Current.Name);
            _xml.SetAttributeValue("ClassName", Item.Current.ClassName);
            _xml.SetAttributeValue("Type", Item.Current.LocalizedControlType);
            _xml.SetAttributeValue("NativeWindowHandle", Item.Current.NativeWindowHandle);
            _xml.SetAttributeValue("BoundingRectangle", Item.Current.BoundingRectangle);
        }

        public static Element FromAutomationElement(AutomationElement element)
        {
            return new Element(element);
        }
    }
}
