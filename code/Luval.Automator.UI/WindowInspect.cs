using Luval.Automator.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            InitForm();
        }

        private void InitForm()
        {
            var windows = WindowsApiFunctions.GetWindowHandles();
            cboWindows.Tag = windows.OrderBy(i => i.ProcessName).ToArray();
            foreach (var window in (WindowHandle[])cboWindows.Tag)
            {
                cboWindows.Items.Add(string.Format("{0} - {1}", window.ProcessName, window.Title));
            }
        }

        private void cboWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedWindow = ((WindowHandle[])cboWindows.Tag)[cboWindows.SelectedIndex];
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
                Name = string.Format("{0} - {1} - {2}",el.Item.Current.LocalizedControlType,  el.Item.Current.AutomationId, el.Item.Current.ClassName),
                Text = string.IsNullOrWhiteSpace(el.Item.Current.Name) ? el.Item.Current.ClassName : el.Item.Current.Name,
                Tag = el
            };
            
            var dummy = root.Nodes.Cast<TreeNode>().FirstOrDefault(i => i.Name.StartsWith("Dummy"));
            if (dummy != null) root.Nodes.Remove(dummy);

            root.Nodes.Add(node);
            node.Nodes.Add(CreateDummyNode());
            node.Collapse();
        }

        private TreeNode CreateDummyNode()
        {
            return new TreeNode() { Name = string.Format("Dummy - {0}", Guid.NewGuid()), Text = "Empty"};
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
            ((Element)e.Node.Tag).Item.SetFocus();
        }
    }
}
