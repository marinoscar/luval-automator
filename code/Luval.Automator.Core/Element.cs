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
using System.Xml.XPath;

namespace Luval.Automator.Core
{
    public class Element
    {
        public Element()
        {
            InternalId = Guid.NewGuid().ToString().Replace("-", "").ToLowerInvariant();
        }

        public Element(AutomationElement automationElement) : this()
        {
            Item = automationElement;
            Properties = ElementProperty.FromElement(automationElement);
        }

        private XElement _xml;
        private List<Element> _children;
        private List<Element> _allChildren;

        public string InternalId { get; private set; }
        public AutomationElement Item { get; set; }
        public IEnumerable<ElementProperty> Properties { get; set; }
        public bool AreAllChildrenLoaded { get; private set; }
        public IEnumerable<Element> Children
        {
            get
            {
                if (_children == null)
                    LazyLoadChildren();
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

        public string ToXml()
        {
            LoadAllChildren();
            return Xml.ToString();
        }

        public IEnumerable<Element> Query(string xpath)
        {
            //sample query => '//Children/Element[@Type="button"]/NameProperty[text()="Close"]/..' => Gets a button with the name of Close
            LoadAllChildren();
            //return Xml.XPathSelectElements(xpath).Select(i => LookForElementNode(i)) //makes sure that there is an element node as a result
            //    .Where(i => i.Attribute("_InternalHashId") != null && !string.IsNullOrEmpty(i.Attribute("_InternalHashId").Value))
            //    .Select(i => _allChildren.FirstOrDefault(c => c.GetHashCode() == Convert.ToInt32(i.Attribute("_InternalHashId").Value)));
            var nodes = Xml.XPathSelectElements(xpath).ToList();
            return nodes.Select(i => LookForElementNode(i)) //makes sure that there is an element node as a result
                .Where(i => i.Attribute("_InternalId") != null && !string.IsNullOrEmpty(i.Attribute("_InternalId").Value))
                .Select(i => _allChildren.FirstOrDefault(c => c.GetHashCode() == Convert.ToInt32(i.Attribute("_InternalId").Value)));
        }

        private XElement LookForElementNode(XElement el)
        {
            if (el.Name == "Element") return el;
            return LookForElementNode(el.Parent);
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

        protected virtual void LazyLoadChildren()
        {
            _children = new List<Element>();
            foreach (var el in Item.FindAll(TreeScope.Children, Condition.TrueCondition).Cast<AutomationElement>())
                _children.Add(Element.FromAutomationElement(el));
        }

        protected virtual void LoadAllChildren()
        {
            if (AreAllChildrenLoaded) return;
            _allChildren = new List<Element>(Children);
            foreach (var child in Children)
            {
                LoadAllChildren(child);
            }
            AreAllChildrenLoaded = true;
        }

        private void LoadAllChildren(Element element)
        {
            _allChildren.AddRange(element.Children);
            foreach (var el in element.Children)
            {
                el.LazyLoadChildren();
                LoadAllChildren(el);
            }
        }

        protected virtual void LoadXml()
        {
            _xml = new XElement("Element");
            foreach (var prop in Properties)
            {
                var pNode = new XElement(prop.Name, prop.Value);
                pNode.SetAttributeValue("FullName", string.Format("{0}.{1}", prop.Root, prop.Name));
                _xml.Add(pNode);
            }
            _xml.SetAttributeValue("_InternalId", GetHashCode());
            _xml.SetAttributeValue("AutomationId", Item.Current.AutomationId);
            _xml.SetAttributeValue("Name", Item.Current.Name);
            _xml.SetAttributeValue("ClassName", Item.Current.ClassName);
            _xml.SetAttributeValue("Type", Item.Current.LocalizedControlType);
            _xml.SetAttributeValue("NativeWindowHandle", Item.Current.NativeWindowHandle);
            _xml.SetAttributeValue("BoundingRectangle", Item.Current.BoundingRectangle);

            var children = new XElement("Children");
            foreach (var child in Children)
            {
                child.LoadXml();
                children.Add(child.Xml);
            }
            _xml.Add(children);
        }

        public static Element FromAutomationElement(AutomationElement element)
        {
            return new Element(element);
        }
    }
}
