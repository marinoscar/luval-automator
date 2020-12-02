using Luval.Automator.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Luval.Automator.UI
{
    public partial class WindowInspect : BaseForm
    {
        public WindowInspect()
        {
            InitializeComponent();
        }

        private void WindowInspect_Load(object sender, EventArgs e)
        {
            LoadDropdown();
        }

        private void LoadDropdown()
        {
            cboWindows.Items.Clear();
            var windows = WindowsApiFunctions.GetWindowHandles();
            cboWindows.Tag = windows.OrderBy(i => i.ProcessName).ToArray();
            foreach (var window in (WindowHandle[])cboWindows.Tag)
            {
                cboWindows.Items.Add(string.Format("{0} - {1}", window.ProcessName, window.Title));
            }
        }

        private WindowHandle GetSelectedWindowHandle()
        {
            if (cboWindows.SelectedIndex <= 0) return null;
            return ((WindowHandle[])cboWindows.Tag)[cboWindows.SelectedIndex];
        }

        private void cboWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedWindow = GetSelectedWindowHandle();
            var el = Element.FromAutomationElement(AutomationElement.FromHandle(selectedWindow.Handle));
            LoadTree(el);
        }

        private void LoadTree(Element el)
        {
            elementTree.Nodes.Clear();
            var root = new TreeNode() { Name = "Root", Text = string.Format("{0} - {1} - {2}", el.Item.Current.LocalizedControlType, el.Item.Current.AutomationId, el.Item.Current.ClassName) };
            root.Tag = el;
            root.Nodes.Add(CreateDummyNode());
            elementTree.Nodes.Add(root);
            root.Collapse();
        }

        private void LoadNode(TreeNode root, Element el)
        {
            var node = new TreeNode()
            {
                Name = string.Format("{0} - {1} - {2}", el.Item.Current.LocalizedControlType, el.Item.Current.AutomationId, el.Item.Current.ClassName),
                Text = string.IsNullOrWhiteSpace(el.Item.Current.Name) ? el.Item.Current.ClassName : el.Item.Current.Name,
                Tag = el
            };

            root.Nodes.Add(node);
            node.Nodes.Add(CreateDummyNode());
            node.Collapse();
        }

        private TreeNode CreateDummyNode()
        {
            return new TreeNode() { Name = string.Format("Dummy - {0}", Guid.NewGuid()), Text = "Empty" };
        }

        private void elementTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null) return;
            elementDS.DataSource = ((Element)e.Node.Tag).Properties.ToList();
            elementDS.ResetBindings(false);
        }

        private void elementTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag == null) return;
            var dummy = e.Node.Nodes.Cast<TreeNode>().FirstOrDefault(i => i.Name.StartsWith("Dummy"));
            if (dummy == null) return;
            e.Node.Nodes.Remove(dummy);

            var el = ((Element)e.Node.Tag);
            var children = el.Item.FindAll(TreeScope.Children, Condition.TrueCondition).Cast<AutomationElement>().ToList();
            foreach (var child in children)
            {
                LoadNode(e.Node, Element.FromAutomationElement(child));
            }
        }

        private void elementTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null) return;
            var element = ((Element)e.Node.Tag);
            if (element.Picture == null)
            {
                element.Picture = WindowsApiFunctions.CaptureElement(GetSelectedWindowHandle().Handle, element);
                elementPicture.Image = null;
                elementPicture.Refresh();
                elementPicture.Image = element.Picture;
                elementPicture.Refresh();
            }
        }

        private void mnuSaveImageAs_Click(object sender, EventArgs e)
        {
            if (elementPicture.Image == null) return;
            var saveDlg = new SaveFileDialog()
            {
                Title = "Save Image",
                RestoreDirectory = true,
                Filter = "PNG (*.png) | *.png"
            };
            saveDlg.ShowDialog();
            elementPicture.Image.Save(saveDlg.FileName, ImageFormat.Png);
        }

        private void btnExportXML_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedWindowHandle();
            var windowEl = new WindowElement(AutomationElement.FromHandle(selected.Handle));
            var saveDlg = new SaveFileDialog()
            {
                Title = "Save XML",
                RestoreDirectory = true,
                Filter = "xml (*.xml) | *.xml"
            };
            saveDlg.ShowDialog();
            File.WriteAllText(saveDlg.FileName, windowEl.ToXml());
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var selector = new WindowSelector();
            var window = selector.Find("SampleMessage", "Main Sample Window");
            //Children/Element[@Type="button"]/NameProperty[text()="Close"]/.. button
            //Element[@AutomationId="textBox1"]
            var textBox = window.Query(@"//Element[@AutomationId=""textBox1""]").FirstOrDefault();
            var button = window.Query(@"//Children/Element[@Type=""button""]/NameProperty[text()=""Close""]/..").FirstOrDefault();
            textBox.SetText("My name is Oscar Marin");
            button.Click();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDropdown();
        }
    }
}
