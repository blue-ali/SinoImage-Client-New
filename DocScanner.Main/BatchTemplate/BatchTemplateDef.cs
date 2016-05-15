using DocScanner.Bean;
using DocScanner.Main.BatchTemplate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    [Serializable]
	public class BatchTemplateDef
	{
        private const string ROOT_NODE_NAME = "批次节点";

        public BatchTemplateDef()
        {
            this.RootNode = new TemplateNode();
            RootNode.Name = ROOT_NODE_NAME;
        }

        public string Name
		{
			get;
			set;
		}

		public TemplateNode RootNode
		{
			get;
			set;
		}

		public static BatchTemplateDef FromBatchTemplateTree(BatchTemplateTree templateTree, string templateName)
		{
            BatchTemplateDef template = new BatchTemplateDef();
            template.Name = templateName;
            RadTreeNode treeRootNode = templateTree.GetRadTreeView().Nodes[0];

            if (treeRootNode != null)
			{
                template.RootNode = new TemplateNode() { Name = treeRootNode.Text};
                template.AddTemplateNodeFromTreeNode(template.RootNode, treeRootNode);
			}
            return template;
		}

        private void AddTemplateNodeFromTreeNode(TemplateNode templateNode, RadTreeNode radTreeNode)
        {
            if (radTreeNode.Nodes.Count > 0)
            {
                foreach(RadTreeNode radTreeChild in radTreeNode.Nodes)
                {
                    TemplateNode templateChild = new TemplateNode() { Name = radTreeChild.Text };
                    templateNode.AddChild(templateChild);
                    AddTemplateNodeFromTreeNode(templateChild, radTreeChild);
                }
            }
        }

        public void _FromRadTree(RadTreeNode treerootnode)
        {
            this.RootNode = null;
            if (treerootnode != null)
            {
                LinkedList<RadTreeNode> linkedList = new LinkedList<RadTreeNode>();
                TemplateNode templateNode = new TemplateNode();
                templateNode.Name = treerootnode.Text;
                treerootnode.Tag = templateNode;
                linkedList.AddLast(treerootnode);
                while (linkedList.Count > 0)
                {
                    LinkedListNode<RadTreeNode> first = linkedList.First;
                    bool flag2 = first.Value.Nodes.Count > 0;
                    if (flag2)
                    {
                        foreach (RadTreeNode current in first.Value.Nodes)
                        {
                            TemplateNode TemplateNode2 = new TemplateNode();
                            TemplateNode2.Name = current.Text;
                            current.Tag = TemplateNode2;
                            (first.Value.Tag as TemplateNode).AddChild(TemplateNode2);
                            linkedList.AddLast(current);
                        }
                    }
                    linkedList.RemoveFirst();
                }
                this.RootNode = templateNode;
            }
        }



        public static RadTreeNode _CreateTemplateTree(RadTreeView tree, BatchTemplateDef bat, String nodeText)
		{
			RadTreeNode batchNode;
			if (bat == null)
			{
                //RadTreeNode batchNode = NavigateTreeHelper.CreateBatchNode(tree, batchInfo, null);
                batchNode = tree.Nodes.Add(nodeText);
                //tree.Refresh();
                batchNode.Selected = true;
				//result = batchNode;
			}
			else
			{
				LinkedList<TemplateNode> linkedList = new LinkedList<TemplateNode>();
				linkedList.AddLast(bat.RootNode);
                batchNode = tree.Nodes.Add(nodeText);
                //RadTreeNode batchNode = NavigateTreeHelper.CreateBatchNode(tree, batchInfo, null);
                tree.Refresh();
				bat.RootNode.Tag = batchNode;
				while (linkedList.Count > 0)
				{
					TemplateNode value = linkedList.First.Value;
					if (value.Children != null && value.Children.Count > 0)
					{
						RadTreeNode radTreeNode3 = value.Tag as RadTreeNode;
						foreach (TemplateNode current in value.Children)
						{
							RadTreeNode radTreeNode4 = radTreeNode3.Nodes.Add(current.Name);
							radTreeNode4.ShowCheckBox = false;
							radTreeNode4.Tag = new NCategoryInfo(current.Name);
							current.Tag = radTreeNode4;
							linkedList.AddLast(current);
						}
					}
					linkedList.RemoveFirst();
				}
                batchNode.ExpandAll();
                batchNode.Selected = true;
				
			}
			return batchNode;
		}
	}
}
