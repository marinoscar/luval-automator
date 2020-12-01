using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Luval.Automator.Core
{
    public class Element
    {
        public AutomationElement Item { get; set; }
        public IEnumerable<ElementSelectionProperty> Properties { get; set; }
        public Image Picture { get; set; }

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


        public static Element FromAutomationElement(AutomationElement element)
        {
            return new Element() {
                Item = element,
                Properties = ElementSelectionProperty.FromElement(element)
            };
        }
    }
}
