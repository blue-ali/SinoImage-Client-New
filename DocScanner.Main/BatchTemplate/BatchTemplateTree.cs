using DocScanner.Bean;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DocScanner.Main.BatchTemplate
{
    //继承RadTreeView的话样式就变了 待解决
    public class BatchTemplateTree{

        private const string ROOT_NODE_NAME = "批次节点";
        //private static readonly BatchTemplateTree instance = new BatchTemplateTree();
        private RadTreeView batchTemplateTree = new RadTreeView();
        private RadContextMenu childContextMenu = new RadContextMenu();
        private RadContextMenu rootNodeContextMenu = new RadContextMenu();
        private RadMenuItem addMenuItem = new RadMenuItem();
        private RadMenuItem addMenuItem1 = new RadMenuItem();   //批次节点菜单项
        private RadMenuItem delMenuItem = new RadMenuItem();
        private RadMenuItem modifyMenuItem = new RadMenuItem();
        //private delegate void EventHandler(object sender, EventArgs args);
        //private EventHandler Clicked;

        //private static RadMenuItem radMenuItem4 = new RadMenuItem();
 
        public BatchTemplateTree():base()
        {
            batchTemplateTree.ExpandAnimation = ExpandAnimation.Opacity;
            batchTemplateTree.Dock = DockStyle.Fill;
            batchTemplateTree.AllowEdit = true;
            batchTemplateTree.ShowLines = true;
            batchTemplateTree.LineStyle = TreeLineStyle.Dot;
            batchTemplateTree.TreeIndent = 20;
            batchTemplateTree.TreeViewElement.ExpandImage = Properties.Resources.expand.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            batchTemplateTree.TreeViewElement.HoveredExpandImage = Properties.Resources.expand.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            batchTemplateTree.TreeViewElement.CollapseImage = Properties.Resources.collapse.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            batchTemplateTree.TreeViewElement.HoveredCollapseImage = Properties.Resources.collapse.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            batchTemplateTree.AllowShowFocusCues = true;
            batchTemplateTree.BackgroundImageLayout = ImageLayout.Stretch;
            batchTemplateTree.BackColor = SystemColors.ControlLightLight;
            batchTemplateTree.SpacingBetweenNodes = 12;
            batchTemplateTree.AllowArbitraryItemHeight = true;
            batchTemplateTree.ShowItemToolTips = true;

            childContextMenu.Items.Add(addMenuItem);
            childContextMenu.Items.Add(delMenuItem);
            childContextMenu.Items.Add(modifyMenuItem);
            rootNodeContextMenu.Items.Add(addMenuItem1);
            addMenuItem.Text = "新增";
            addMenuItem1.Text = "新增";
            delMenuItem.Text = "删除";
            modifyMenuItem.Text = "修改";
            addMenuItem.Click += AddMenuItem_Click;
            delMenuItem.Click += DelMenuItem_Click;
            modifyMenuItem.Click += ModifyMenuItem_Click;
            addMenuItem1.Click += AddMenuItem_Click;
        }

        internal void ExpandAll()
        {
            batchTemplateTree.ExpandAll();
        }

        internal void Clear()
        {
            batchTemplateTree.Nodes.Clear();
        }

        public RadTreeView GetRadTreeView()
        {
            return batchTemplateTree;
        }

        public static BatchTemplateTree CreateTreeFromTemplate(BatchTemplateDef template)
        {
            BatchTemplateTree templateTree = new BatchTemplateTree();
            if (template == null)
            {
                templateTree.CreateBatchNode();
            }
            else
            {
                templateTree.CreateNodesFromTemplate(null, template.RootNode);
            }
            return templateTree;
        }

        private void CreateBatchNode()
        {
            RadTreeNode batchNode = batchTemplateTree.Nodes.Add(ROOT_NODE_NAME);
            batchNode.ContextMenu = rootNodeContextMenu;
            batchNode.Selected = true;
        }

        private void CreateNodesFromTemplate(RadTreeNode parentNode, TemplateNode templateNode)
        {
            RadTreeNode currenTreeNode = null;
            if (parentNode == null)
            {
                currenTreeNode = batchTemplateTree.Nodes.Add(templateNode.Name);
                currenTreeNode.ContextMenu = rootNodeContextMenu;
            }else
            {
                currenTreeNode = parentNode.Nodes.Add(templateNode.Name);
                currenTreeNode.ContextMenu = childContextMenu;
            }
            if (templateNode.HasChildren())
            {
                foreach(TemplateNode child in templateNode.Children)
                {
                    CreateNodesFromTemplate(currenTreeNode, child);
                }
            }
        }

        //public void ClearNodes()
        //{
        //    batchTemplateTree.Nodes.Clear();
        //}

        private void ModifyMenuItem_Click(object sender, EventArgs e)
        {
            //batchTemplateTree.BeginEdit();
            batchTemplateTree.BeginEdit();
        }

        private void DelMenuItem_Click(object sender, EventArgs e)
        {
            //batchTemplateTree.SelectedNode.Remove();
            batchTemplateTree.SelectedNode.Remove();
        }

        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            //RadTreeNode categoryNode = batchTemplateTree.SelectedNode.Nodes.Add("新分类");
            ////tegoryNode.ExpandAll();
            //batchTemplateTree.ExpandAll();
            //categoryNode.ContextMenu = childContextMenu;
            RadTreeNode categoryNode = batchTemplateTree.SelectedNode.Nodes.Add("新分类");
            //tegoryNode.ExpandAll();
            batchTemplateTree.ExpandAll();
            categoryNode.ContextMenu = childContextMenu;
        }

    }
}
