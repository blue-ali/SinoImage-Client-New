using DocScanner.Bean;
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
	public class BatchTemplatedef
	{
		public string Name
		{
			get;
			set;
		}

		public NodeDef RootNode
		{
			get;
			set;
		}

		public void FromRadTree(RadTreeNode treerootnode)
		{
			this.RootNode = null;
			bool flag = treerootnode == null;
			if (!flag)
			{
				LinkedList<RadTreeNode> linkedList = new LinkedList<RadTreeNode>();
				NodeDef nodeDef = new NodeDef();
				nodeDef.Name = treerootnode.Text;
				treerootnode.Tag = nodeDef;
				linkedList.AddLast(treerootnode);
				while (linkedList.Count > 0)
				{
					LinkedListNode<RadTreeNode> first = linkedList.First;
					bool flag2 = first.Value.Nodes.Count > 0;
					if (flag2)
					{
						foreach (RadTreeNode current in first.Value.Nodes)
						{
							NodeDef nodeDef2 = new NodeDef();
							nodeDef2.Name = current.Text;
							current.Tag = nodeDef2;
							(first.Value.Tag as NodeDef).AddChild(nodeDef2);
							linkedList.AddLast(current);
						}
					}
					linkedList.RemoveFirst();
				}
				this.RootNode = nodeDef;
			}
		}

		public static RadTreeNode CreateRadTreeFromTemplate(RadTreeView tree, BatchTemplatedef bat, string rootnodetext)
		{
			bool flag = bat == null;
			RadTreeNode result;
			if (flag)
			{
				RadTreeNode radTreeNode = tree.Nodes.Add(rootnodetext);
				tree.Refresh();
				radTreeNode.Selected = true;
				result = radTreeNode;
			}
			else
			{
				LinkedList<NodeDef> linkedList = new LinkedList<NodeDef>();
				linkedList.AddLast(bat.RootNode);
				RadTreeNode radTreeNode2 = tree.Nodes.Add(rootnodetext);
				tree.Refresh();
				bat.RootNode.Tag = radTreeNode2;
				while (linkedList.Count > 0)
				{
					NodeDef value = linkedList.First.Value;
					bool flag2 = value.Children != null && value.Children.Count > 0;
					if (flag2)
					{
						RadTreeNode radTreeNode3 = value.Tag as RadTreeNode;
						foreach (NodeDef current in value.Children)
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
				radTreeNode2.ExpandAll();
				radTreeNode2.Selected = true;
				result = radTreeNode2;
			}
			return result;
		}
	}
}
