using DocScanner.Bean;
using DocScanner.Bean.pb;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public static class NavigateTreeHelper
    {

        /// <summary>
        /// 根据节点类型删除节点。
        /// </summary>
        /// <param name="node"></param>
        public static void RemoveNode(this RadTreeNode node)
        {
            if (node.Tag is NBatchInfo)
            {
                node.Remove();
            }
            else if (node.Tag is NCategoryInfo)
            {
                RemoveCategoryNode(node);
            }
            else if (node.Tag is NFileInfo)
            {
                RemovceFileNode(node);
            }
        }

        /// <summary>
        /// 删除分类节点。遍历分类节点下的子节点，如果是文件节点，调用删除文件节点的方法进行删除，如果是分类节点，递归调用
        /// </summary>
        /// <param name="node">要删除的节点</param>
        public static void RemoveCategoryNode(this RadTreeNode node)
        {
            //所属批次是从本地添加，不是来自服务器，可以直接删除分类节点
            if ((node.GetBatchNode().Tag as NBatchInfo).Operation == EOperType.eADD)
            {
                node.Remove();
            }
            else   //所属批次来自服务器，删除从本地添加的文件节点，其他文件节点设置删除标示
            {
                node.Visible = false;
                List<RadTreeNode> toDelNodes = GetFileNodesWithAddOperation(node);
                foreach (RadTreeNode toDelNode in toDelNodes)
                {
                    toDelNode.Remove();
                }
            }
        }

        private static List<RadTreeNode> GetFileNodesWithAddOperation(RadTreeNode node)
        {
            List<RadTreeNode> addFileNodes = new List<RadTreeNode>();
            foreach (RadTreeNode childNode in node.Nodes)
            {
                if (childNode.Tag is NFileInfo)  //文件节点
                {
                    if ((childNode.Tag as NFileInfo).Operation == EOperType.eADD)    //从本地添加的文件节点，后续做删除操作
                    {
                        addFileNodes.Add(childNode);
                    }
                    else          //其他来自服务器的文件，设置删除标识
                    {
                        (childNode.Tag as NFileInfo).Operation = EOperType.eDEL;
                    }
                }
                else      //分类节点,递归调用继续找出文件节点
                {
                    addFileNodes.AddRange(GetFileNodesWithAddOperation(childNode));
                }
            }
            return addFileNodes;

        }

        public static void RemovceFileNode(this RadTreeNode node)
        {
            NFileInfo fileInfo = node.Tag as NFileInfo;
            if (fileInfo.Operation == EOperType.eADD)    //从本地添加的文件，直接删除
            {
                node.Remove();
            }//else if(fileInfo.Operation == EOperType.eFROM_SERVER_NOTCHANGE || fileInfo.Operation == EOperType..eUPD) //来自服务器（包括更新状态）的文件设置删除标识
            else
            {
                node.Visible = false;
                fileInfo.Operation = EOperType.eDEL;
            }
        }

        public static RadTreeNode GetBatchNode(this RadTreeNode node)
        {
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }

        public static bool IsBatchNode(this RadTreeNode node)
        {
            return node.Tag is NBatchInfo;
        }

        public static bool IsFromServer(this RadTreeNode node)
        {
            bool result;
            while (node != null)
            {
                if (node.Tag is NBatchInfo)
                {
                    result = ((node.Tag as NBatchInfo).Operation == EOperType.eFROM_SERVER_NOTCHANGE);
                    return result;
                }
                node = node.Parent;
            }
            result = false;
            return result;
        }

        public static bool IsCategoryNode(this RadTreeNode node)
        {
            return node.Tag is NCategoryInfo;
        }

        public static bool IsFileNode(this RadTreeNode node)
        {
            return node.Tag is NFileInfo;
        }

        public static bool UpdateNodeNInfo(this RadTreeNode node)
        {
            if (!(node.Tag is NBatchInfo))
            {
                if (node.Tag is NFileInfo)
                {
                    node.UpdateFileNodeCatInfo();
                }
                else
                {
                    if (node.Tag is NCategoryInfo)
                    {
                    }
                }
            }
            return true;
        }

        public static int GetNextCategoryIndex(this RadTreeNode parentNode)
        {
            return parentNode.Nodes.Where(x => x.Tag is NCategoryInfo).ToList().Count + 1;
        }
        

        /// <summary>
        /// 根据FileInfo更新节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fileInfo"></param>
        public static RadTreeNode UpdateFileNode(this RadTreeNode node, NFileInfo fileInfo)
        {
            node.ToolTipText = fileInfo.ToUITipString();
            node.Text = fileInfo.DisplayName;
            node.SetImageIcon(fileInfo.LocalPath, NavigateTree.ThumbImgSize, NavigateTree.ThumbImgSize);
            if ((node.Tag as NFileInfo).Operation != EOperType.eADD)
            {
                fileInfo.Operation = EOperType.eUPD;
            }
            fileInfo.FileNO = (node.Tag as NFileInfo).FileNO;
            node.Tag = fileInfo;
            return node;
        }

        /// <summary>
        /// 更新文件节点的目录信息，提交批次时调用
        /// </summary>
        /// <param name="filenode"></param>
        /// <returns></returns>
        public static string UpdateFileNodeCatInfo(this RadTreeNode filenode)
        {
            NFileInfo nFileInfo = filenode.Tag as NFileInfo;
            string text = "";
            RadTreeNode parent = filenode.Parent;
            while (parent.Tag is NCategoryInfo)
            {
                text = (parent.Tag as NCategoryInfo).CategoryName + "." + text;
                parent = parent.Parent;
            }
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Substring(0, text.Length - 1);
            }
            nFileInfo.Category = text;
            return text;
        }

        public static List<RadTreeNode> SelectNFileNode(this List<RadTreeNode> list)
        {
            List<RadTreeNode> list2 = new List<RadTreeNode>();
            foreach (RadTreeNode current in list)
            {
                if (current.Tag is NFileInfo)
                {
                    list2.Add(current);
                }
            }
            return list2;
        }

        public static NBatchInfo CreateTmpNBatchInfo(this RadTreeNode node)
        {
            NBatchInfo info = new NBatchInfo
            {
                BatchNO = (node.Tag as NBatchInfo).BatchNO,
                Author = AccountSetting.GetInstance().AccountName
            };
            info.FileInfos.AddRange(node.GetChildren().SelectNFileNode().Select<RadTreeNode, NFileInfo>(o => o.Tag as NFileInfo));
            return info;

        }

        public static List<RadTreeNode> GetAllChildFileNodes(RadTreeNode rootnode)
        {
            List<RadTreeNode> list = new List<RadTreeNode>();
            foreach (RadTreeNode node in rootnode.Nodes)
            {
                if (node.Tag is NFileInfo)
                {
                    list.Add(node);
                }
                else
                {
                    List<RadTreeNode> allChildFileNodes = GetAllChildFileNodes(node);
                    list.AddRange(allChildFileNodes);
                }
            }
            return list;
        }
    }
}
