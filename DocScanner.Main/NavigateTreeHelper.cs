using DocScanner.Bean;
using DocScanner.Bean.pb;
using DocScanner.LibCommon;
using DocScanner.LibCommon.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public static class NavigateTreeHelper
    {

        [Category("用户界面定义"), Description("树节点字体大小")]
        public static float Lev1NodeFontSize
        {
            get
            {
                string configParamValue = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "Lev1NodeFontSize");
                if (string.IsNullOrEmpty(configParamValue))
                {
                    return 14f;
                }
                return float.Parse(configParamValue);
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "Lev1NodeFontSize", value.ToString());
            }
        }

        [Category("用户界面定义"), Description("树节点字体"), Browsable(false)]
        public static string Lev1NodeFont
        {
            get
            {
                string configParamValue = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "Lev1NodeFont");
                if (string.IsNullOrEmpty(configParamValue))
                {
                    return "宋体";
                }
                return configParamValue;
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "Lev1NodeFont", value.ToString());
            }
        }

        private static int ThumbImgSize
        {
            get
            {
                string configParamValue = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "ThumbImgSize");
                bool flag = string.IsNullOrEmpty(configParamValue);
                int result;
                if (flag)
                {
                    result = 96;
                }
                else
                {
                    result = int.Parse(configParamValue);
                }
                return result;
            }
        }

        public static RadTreeNode CreateBatchNode(RadTreeView tree, NBatchInfo batchInfo, RadContextMenu contextMenu)
        {
            RadTreeNode batchNode = tree.Nodes.Add(batchInfo.DisplayName);
            batchNode.Tag = batchInfo;
            batchNode.AllowDrop = false;
            batchNode.ItemHeight = 50;
            batchNode.TextAlignment = ContentAlignment.MiddleCenter;
            batchNode.Image = Properties.Resources.BatchIcno.GetThumbnailImage(40, 40 , null, IntPtr.Zero);
            batchNode.Font = new Font(Lev1NodeFont, Lev1NodeFontSize);
            batchNode.ContextMenu = contextMenu;
            tree.Refresh();
            batchNode.Selected = true;
            return batchNode;
        }

        public static RadTreeNode CreateCategoryNode(RadTreeView tree, NCategoryInfo categoryInfo)
        {
            RadTreeNode node = tree.SelectedNode.Nodes.Add(categoryInfo.CategoryName);
            tree.SelectedNode.Nodes.Move(tree.SelectedNode.Nodes.Count - 1, 0);
            node.Tag = categoryInfo;
            node.Image = Properties.Resources.CatalogIcon.GetThumbnailImage(40, 40, null, IntPtr.Zero);
            node.ItemHeight = 50;
            node.Font = new Font(Lev1NodeFont, 12);
            node.ShowCheckBox = false;
            node.Selected = true;
            tree.Refresh();
            //this.radTreeView1.SelectedNode.ExpandAll();
            return node;
        }

        public static RadTreeNode UpdateCategoryNode(RadTreeNode categoryNode, RadContextMenu contextMenu)
        {
            //categoryNode.Tag = categoryInfo;
            NCategoryInfo categoryInfo = new NCategoryInfo();
            categoryInfo.CategoryName = categoryNode.Text;
            categoryNode.Tag = categoryInfo;
            categoryNode.Image = Properties.Resources.CatalogIcon.GetThumbnailImage(40, 40, null, IntPtr.Zero);
            categoryNode.ItemHeight = 50;
            categoryNode.Font = new Font(Lev1NodeFont, 12);
            categoryNode.ShowCheckBox = false;
            categoryNode.Selected = true;
            categoryNode.TextAlignment = ContentAlignment.MiddleCenter;
            categoryNode.ContextMenu = contextMenu;
            return categoryNode;
        }


        public static void UpdateBatchNodeTitle(this RadTreeView radTreeView, RadTreeNode node)
        {
            if (node == null)
            {
                node = radTreeView.SelectedNode;
            }
            while (!(node.Tag is NBatchInfo))
            {
                node = node.Parent;
            }
            if (node != null)
            {
                node.Text = GetChildNodesCount(node).ToCommentString() + (node.Tag as NBatchInfo).BatchNO;
            }
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

        /// <summary>
        /// 创建来自本地采集文件的节点，根据BatchInfo的LastNo生成FileNo
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="fileInfo"></param>
        /// <param name="contextMenu"></param>
        /// <param name="batchInfo"></param>
        /// <returns></returns>
        public static RadTreeNode CreateFileNodeFromLocal(RadTreeNode parentNode, NFileInfo fileInfo, RadContextMenu contextMenu, NBatchInfo batchInfo)
        {
            fileInfo.BatchNO = batchInfo.BatchNO;
            batchInfo.LastNO++;
            fileInfo.FileNO = batchInfo.LastNO.ToString();

            RadTreeNode fileNode = parentNode.Nodes.Add(fileInfo.DisplayName);
            fileNode.SetImageIcon(fileInfo.LocalPath, true);
            //node2.Image = ImageHelper.LoadSizedImage(info.LocalPath, this.GetSetting().ThumbImgSize, this.GetSetting().ThumbImgSize);
            fileNode.Tag = fileInfo;
            fileNode.Selected = true;
            fileNode.ToolTipText = fileInfo.ToUITipString();
            fileNode.ContextMenu = contextMenu;
            fileNode.ItemHeight = ThumbImgSize;
            fileNode.Checked = true;
            fileNode.TextAlignment = ContentAlignment.MiddleCenter;

            return fileNode;
        }

        /// <summary>
        /// 创建来自服务器文件的节点，根据FileNo更新BatchInfo的LastNo
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="fileInfo"></param>
        /// <param name="contextMenu"></param>
        /// <param name="batchInfo"></param>
        /// <returns></returns>
        public static RadTreeNode CreateFileNodeFromServer(RadTreeNode parentNode, NFileInfo fileInfo, RadContextMenu contextMenu, NBatchInfo batchInfo)
        {
            RadTreeNode fileNode = parentNode.Nodes.Add(fileInfo.DisplayName);
            fileNode.SetImageIcon(fileInfo.LocalPath, true);
            //node2.Image = ImageHelper.LoadSizedImage(info.LocalPath, this.GetSetting().ThumbImgSize, this.GetSetting().ThumbImgSize);
            fileNode.Tag = fileInfo;
            fileNode.Selected = true;
            fileNode.ToolTipText = fileInfo.ToUITipString();
            fileNode.ContextMenu = contextMenu;
            fileNode.ItemHeight = ThumbImgSize;
            fileNode.Checked = true;
            fileNode.TextAlignment = ContentAlignment.MiddleCenter;

            int fileNo = fileInfo.FileNO.ToInt();
            if (batchInfo.LastNO < fileNo)
            {
                batchInfo.LastNO = fileNo;
            }
            return fileNode;
        }

        public static int GetChildNodesCount(RadTreeNode node)
        {
            int num = 0;
            if (((node == null) || (node.Nodes == null)) || (node.Nodes.Count == 0))
            {
                return num;
            }
            foreach (RadTreeNode node2 in node.Nodes)
            {
                if (node2.Tag is NCategoryInfo)
                {
                    num += GetChildNodesCount(node2);
                }
                else if ((node2.Tag is NFileInfo) && ((node2.Tag as NFileInfo).Operation != EOperType.eDEL))
                {
                    num++;
                }
            }
            return num;
        }

        private static void FileNode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Checked")
            {
                RadTreeNode node = sender as RadTreeNode;
                if (node.Checked)
                {
                    node.Parent.Checked = true;
                }
                if (!node.Checked && node.Parent.Nodes.All<RadTreeNode>(o => !o.Checked))
                {
                    node.Parent.Checked = false;
                }

            }
        }

        public static RadTreeNode GetNode(RadTreeNode rootNode, string name)
        {
            foreach (RadTreeNode node in rootNode.Nodes)
            {
                if (node.Name.Equals(name))
                    return node;
                RadTreeNode next = GetNode(node, name);
                if (next != null)
                    return next;
            }
            return null;
        }

        public static NBatchInfo CreateTmpNBatchInfo(this RadTreeNode node)
        {
            NBatchInfo info = new NBatchInfo
            {
                BatchNO = (node.Tag as NBatchInfo).BatchNO,
                Author = AbstractSetting<AccountSetting>.CurSetting.AccountName
            };
            info.FileInfos.AddRange(node.GetChildren().SelectNFileNode().Select<RadTreeNode, NFileInfo>(o => o.Tag as NFileInfo));
            return info;

        }

        public static void SetImageIcon(this RadTreeNode node, string fname, bool showimage)
        {
            if (showimage)
            {
                if (ImgUtils.ImageHelper.IsImgExt(fname))
                {
                    node.Image = ImgUtils.ImageHelper.LoadSizedImage(fname, NavigateTreeHelper.ThumbImgSize, NavigateTreeHelper.ThumbImgSize, "");
                }
                else
                {
                    node.Image = FileHelper.GetFilesIcon(fname);
                }
            }
            else
            {
                node.Image = null;
            }
        }

        /// <summary>
        /// 删除分类节点。遍历分类节点下的子节点，如果是文件节点，调用删除文件节点的方法进行删除，如果是分类节点，递归调用
        /// </summary>
        /// <param name="node">要删除的节点</param>
        public static void RemoveCategoryNode(this RadTreeNode node)
        {
            //所属批次是从本地添加，不是来自服务器，可以直接删除分类节点
            if((node.GetBatchNode().Tag as NBatchInfo).Operation == EOperType.eADD)
            {
                node.Remove();
            }
            else   //所属批次来自服务器，删除从本地添加的文件节点，其他文件节点设置删除标示
            {
                node.Visible = false;
                List<RadTreeNode> toDelNodes = GetFileNodesWithAddOperation(node);
                foreach(RadTreeNode toDelNode in toDelNodes)
                {
                    toDelNode.Remove();
                }
            }
        }

        private static List<RadTreeNode> GetFileNodesWithAddOperation(RadTreeNode node)
        {
            List<RadTreeNode> addFileNodes = new List<RadTreeNode>();
            foreach(RadTreeNode childNode in node.Nodes)
            {
                if(childNode.Tag is NFileInfo )  //文件节点
                {
                    if((childNode.Tag as NFileInfo).Operation == EOperType.eADD)    //从本地添加的文件节点，后续做删除操作
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

        /// <summary>
        /// 根据节点类型删除节点。
        /// </summary>
        /// <param name="node"></param>
        public static void RemoveNode(this RadTreeNode node)
        {
            if(node.Tag is NBatchInfo)
            {
                node.Remove();
            }else if(node.Tag is NCategoryInfo)
            {
                RemoveCategoryNode(node);   
            }else if(node.Tag is NFileInfo)
            {
                RemovceFileNode(node);
            }
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
            node.SetImageIcon(fileInfo.LocalPath, true);
            if((node.Tag as NFileInfo).Operation != EOperType.eADD)
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

        public static List<RadTreeNode> GetChildren(this RadTreeNode node)
        {
            List<RadTreeNode> list = new List<RadTreeNode>();
            bool flag = node.Nodes != null && node.Nodes.Count > 0;
            if (flag)
            {
                foreach (RadTreeNode current in node.Nodes)
                {
                    list.Add(current);
                    list.AddRange(current.GetChildren());
                }
            }
            return list;
        }

        public static List<RadTreeNode> SelectNFileNode(this List<RadTreeNode> list)
        {
            List<RadTreeNode> list2 = new List<RadTreeNode>();
            foreach (RadTreeNode current in list)
            {
                bool flag = current.Tag is NFileInfo;
                if (flag)
                {
                    list2.Add(current);
                }
            }
            return list2;
        }

        public static void FinalizeDrop(RadTreeNode targetNode, RadTreeNode draggedNode, ArrowDirection arrowDirection, RadTreeView endTree)
        {
            bool showCheckBox = draggedNode.ShowCheckBox;
            bool flag = targetNode.Tag is NFileInfo;
            if (!flag)
            {
                bool flag2 = draggedNode.Tag is NBatchInfo;
                if (!flag2)
                {
                    bool flag3 = arrowDirection == ArrowDirection.Right;
                    if (flag3)
                    {
                        RadTreeNodeCollection radTreeNodeCollection = targetNode.Nodes;
                        bool flag4 = targetNode != draggedNode.Parent;
                        if (flag4)
                        {
                            radTreeNodeCollection.Add(draggedNode);
                            targetNode.Expand();
                        }
                        else
                        {
                            radTreeNodeCollection.Move(draggedNode.Index, radTreeNodeCollection.Count - 1);
                        }
                        draggedNode.ShowCheckBox = showCheckBox;
                    }
                    else
                    {
                        RadTreeNodeCollection radTreeNodeCollection = (targetNode.Parent != null) ? targetNode.Parent.Nodes : endTree.Nodes;
                        int num = targetNode.Index;
                        bool flag5 = radTreeNodeCollection.Contains(draggedNode);
                        bool flag6 = flag5 && draggedNode.Index < targetNode.Index;
                        if (flag6)
                        {
                            num--;
                        }
                        bool flag7 = arrowDirection == ArrowDirection.Down;
                        if (flag7)
                        {
                            num++;
                        }
                        bool flag8 = flag5;
                        if (flag8)
                        {
                            radTreeNodeCollection.Move(draggedNode.Index, num);
                        }
                        else
                        {
                            radTreeNodeCollection.Insert(num, draggedNode);
                        }
                        bool flag9 = targetNode.Parent != null;
                        if (flag9)
                        {
                            targetNode.Parent.Expand();
                        }
                        draggedNode.ShowCheckBox = showCheckBox;
                    }
                }
            }
        }
    }
}
