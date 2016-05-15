using DocScaner.PDF.Utils;
using DocScanner.Bean;
using DocScanner.Bean.pb;
using DocScanner.Common;
using DocScanner.LibCommon;
using DocScanner.LibCommon.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class NavigateTree
    {

        private static readonly NavigateTree instance = new NavigateTree();

        private RadTreeView navigateTree = new RadTreeView();
        private RadContextMenu categoryContextMenu = new RadContextMenu();
        private RadContextMenu batchContextMenu = new RadContextMenu();
        private RadContextMenu _menudownloadedbatchnode = new RadContextMenu();
        private RadContextMenu fileContextMenu = new RadContextMenu();
        private RadContextMenu fileContextMenunoDel = new RadContextMenu();

        private bool showFileImage = true;

        // Events
        public event EventHandler<TEventArg<RadTreeNode>> OnItemSelectChanged;

        public delegate void NodePropertyEventHandler(Object sender, NodePropertyEventArgs e);
        public event NodePropertyEventHandler nodeProperty;



        public class NodePropertyEventArgs : EventArgs
        {
            private RadTreeNode node;

            public NodePropertyEventArgs(RadTreeNode node)
            {
                this.node = node;
            }
        }

        private NavigateTree()
        {
            navigateTree.CheckBoxes = false;

            navigateTree.ExpandAnimation = ExpandAnimation.Opacity;
            navigateTree.AllowDragDrop = true;
            navigateTree.AllowEdit = true;
            navigateTree.TreeIndent = 35;
            navigateTree.TreeViewElement.ExpandImage = Properties.Resources.expand.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            navigateTree.TreeViewElement.HoveredExpandImage = Properties.Resources.expand.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            navigateTree.TreeViewElement.CollapseImage = Properties.Resources.collapse.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            navigateTree.TreeViewElement.HoveredCollapseImage = Properties.Resources.collapse.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            navigateTree.LineStyle = TreeLineStyle.Dot;
            navigateTree.LineColor = Color.DeepSkyBlue;

            navigateTree.AllowDragDrop = true;
            navigateTree.AllowDragDropBetweenTreeViews = false;
            navigateTree.AllowDrop = true;
            navigateTree.AllowPlusMinusAnimation = true;
            navigateTree.AllowShowFocusCues = true;
            navigateTree.BackColor = System.Drawing.SystemColors.ControlLightLight;
            navigateTree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            navigateTree.Dock = System.Windows.Forms.DockStyle.Fill;
            navigateTree.DropFeedbackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            navigateTree.Location = new System.Drawing.Point(0, 0);
            navigateTree.Name = "NavigateTree";
            // 
            // 
            // 
            navigateTree.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 150, 250);
            navigateTree.ShowLines = true;
            navigateTree.ShowRootLines = false;
            navigateTree.Size = new System.Drawing.Size(233, 634);
            navigateTree.SpacingBetweenNodes = 12;
            navigateTree.TabIndex = 0;
            //navigateTree.ItemHeight = this.GetSetting().ThumbImgSize;
            //navigateTree.ImageScalingSize = new Size(this.GetSetting().ThumbImgSize, this.GetSetting().ThumbImgSize);

            navigateTree.AllowArbitraryItemHeight = true;
            navigateTree.ShowItemToolTips = true;
            //navigateTree.SelectedNodeChanged += NavigateTree_SelectedNodeChanged;
            navigateTree.SelectedNodeChanged += navigateTree_SelectedNodeChanged;
            navigateTree.Nodes.PropertyChanged += this.Nodes_PropertyChanged;
            this.OnItemSelectChanged += this.NavigateTreeOnItemSelectChanged;
        }

        public RadTreeView GetRadTree()
        {
            return navigateTree;
        }

        public RadTreeNode SelectedNode
        {
            get
            {
                return navigateTree.SelectedNode;
            }
        }

        private void SetupBatchNodeMenu()
        {
            RadMenuItem item = new RadMenuItem
            {
                Text = "属性"
            };
            item.Click += new EventHandler(this.FileNode_Property_Click);
            this.batchContextMenu.Items.Add(item);
            if (UISetting.GetInstance().AllowDelMenu)
            {
                RadMenuItem item6 = new RadMenuItem
                {
                    Text = "移除"
                };
                item6.Click += new EventHandler(this.menuBatchNode_DEL_Click);
                this.batchContextMenu.Items.Add(item6);
            }
            RadMenuItem item2 = new RadMenuItem
            {
                Text = "从本地添加文件到批次中"
            };
            item2.Click += new EventHandler(this.BatchNode_AddLocalF_Click);
            this.batchContextMenu.Items.Add(item2);
            RadMenuItem item3 = new RadMenuItem
            {
                Text = "从扫描设备补扫并添加到批次中"
            };
            item3.Click += new EventHandler(this.menuBatchNode_AddScanF_Click);
            this.batchContextMenu.Items.Add(item3);
            RadMenuItem item4 = new RadMenuItem
            {
                Text = "新建分类"
            };
            item4.Click += new EventHandler(this.BatchAddCategory_Click);
            this.batchContextMenu.Items.Add(item4);
            RadMenuItem item5 = new RadMenuItem
            {
                Text = "导出到pdf"
            };
            item5.Click += new EventHandler(this.BatchExportPdf_Click);
            this.batchContextMenu.Items.Add(item5);
        }

        private void SetupCategoryNodeMenu()
        {
            RadMenuItem item = new RadMenuItem
            {
                Text = "修改分类名称"
            };
            item.Click += new EventHandler(this.Menucategorymodiname_Click);
            this.categoryContextMenu.Items.Add(item);
            RadMenuItem item2 = new RadMenuItem
            {
                Text = "新建子分类"
            };
            item2.Click += new EventHandler(this.Menucategoryaddsub_Click);
            this.categoryContextMenu.Items.Add(item2);
            RadMenuItem item3 = new RadMenuItem();
            RadMenuItem item4 = new RadMenuItem();
            item3.Text = "删除分类";
            item3.Click += new EventHandler(this.Menucategorydel_Click);
            this.categoryContextMenu.Items.Add(item3);
            RadMenuItem item5 = new RadMenuItem
            {
                Text = "从本地添加文件到批次分类中"
            };
            item5.Click += new EventHandler(this.Menucategoryadd_Click);
            this.categoryContextMenu.Items.Add(item5);
            RadMenuItem item6 = new RadMenuItem
            {
                Text = "分类属性"
            };
            item6.Click += new EventHandler(this.Menucategoryproperty_Click);
            this.categoryContextMenu.Items.Add(item6);
        }

        private void SetupFileNodeMenu()
        {
            RadMenuItem item = new RadMenuItem
            {
                Text = "属性"
            };
            item.Click += new EventHandler(this.FileNode_Property_Click);
            this.fileContextMenu.Items.Add(item);
            RadMenuItem item2 = new RadMenuItem
            {
                Text = "移除"
            };
            item2.Click += new EventHandler(this.menuFileNode_REMOVE_Click);
            this.fileContextMenu.Items.Add(item2);
            RadMenuItem item3 = new RadMenuItem
            {
                Text = "另存为..."
            };
            item3.Click += new EventHandler(this.menuFileNodeSaveAs_Click);
            this.fileContextMenu.Items.Add(item3);
            RadMenuItem item4 = new RadMenuItem
            {
                Text = "使用本地应用程序打开"
            };
            item4.Click += new EventHandler(this.menuExplorerFile_Click);
            this.fileContextMenu.Items.Add(item4);
            RadMenuItem item5 = new RadMenuItem
            {
                Text = "重新采集替换此图..."
            };
            item5.Click += new EventHandler(this.menuReplaceWithScan_Click);
            this.fileContextMenu.Items.Add(item5);
            RadMenuItem item6 = new RadMenuItem
            {
                Text = "导出到pdf"
            };
            item6.Click += new EventHandler(this.Fnodeexportpdf_Click);
            this.fileContextMenu.Items.Add(item6);
        }

        private void FileNode_Property_Click(object sender, EventArgs e)
        {
            ShowNodePropertyCMD cmd = new ShowNodePropertyCMD
            {
                node = navigateTree.SelectedNode
            };
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(cmd);
        }


        public static NavigateTree GetInstance()
        {
            return instance;
        }

        [Category("用户界面定义"), Description("树节点字体大小")]
        public float Lev1NodeFontSize
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
        public string Lev1NodeFont
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

        public static int ThumbImgSize
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

        public RadTreeNode CreateBatchNode(NBatchInfo batchInfo)
        {
            RadTreeNode batchNode = navigateTree.Nodes.Add(batchInfo.DisplayName);
            batchNode.Tag = batchInfo;
            batchNode.AllowDrop = false;
            batchNode.ItemHeight = 50;
            batchNode.TextAlignment = ContentAlignment.MiddleCenter;
            batchNode.Image = Properties.Resources.BatchIcno.GetThumbnailImage(40, 40 , null, IntPtr.Zero);
            batchNode.Font = new Font(Lev1NodeFont, Lev1NodeFontSize);
            batchNode.ContextMenu = batchContextMenu;
            navigateTree.Refresh();
            batchNode.Selected = true;
            return batchNode;
        }

        public RadTreeNode CreateCategoryNode(RadTreeNode parentNode, NCategoryInfo categoryInfo)
        {
            //RadTreeNode node = parentNode.Nodes.Add(categoryInfo.CategoryName);
            int index = parentNode.GetNextCategoryIndex();
            RadTreeNode node = new RadTreeNode() {Text=categoryInfo.CategoryName};
            node.Tag = categoryInfo;
            node.Image = Properties.Resources.CatalogIcon.GetThumbnailImage(40, 40, null, IntPtr.Zero);
            node.ItemHeight = 50;
            node.Font = new Font(Lev1NodeFont, 12);
            node.ShowCheckBox = false;
            node.Selected = true;

            parentNode.Nodes.Insert(index, node);
            //parentNode.Nodes.Move(navigateTree.SelectedNode.Nodes.Count - 1, 0);
            
            navigateTree.Refresh();
            //navigateTree.SelectedNode.ExpandAll();
            return node;
        }

        public RadTreeNode UpdateCategoryNode(RadTreeNode categoryNode)
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
            categoryNode.ContextMenu = categoryContextMenu;
            return categoryNode;
        }


        /// <summary>
        /// 创建来自本地采集文件的节点，根据BatchInfo的LastNo生成FileNo
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="fileInfo"></param>
        /// <param name="contextMenu"></param>
        /// <param name="batchInfo"></param>
        /// <returns></returns>
        public RadTreeNode CreateFileNodeFromLocal(RadTreeNode parentNode, NFileInfo fileInfo, NBatchInfo batchInfo)
        {
            fileInfo.BatchNO = batchInfo.BatchNO;
            batchInfo.LastNO++;
            fileInfo.FileNO = batchInfo.LastNO.ToString();

            RadTreeNode fileNode = parentNode.Nodes.Add(fileInfo.DisplayName);
            fileNode.SetImageIcon(fileInfo.LocalPath, ThumbImgSize, ThumbImgSize);
            //node2.Image = ImageHelper.LoadSizedImage(info.LocalPath, this.GetSetting().ThumbImgSize, this.GetSetting().ThumbImgSize);
            fileNode.Tag = fileInfo;
            fileNode.Selected = true;
            fileNode.ToolTipText = fileInfo.ToUITipString();
            fileNode.ContextMenu = fileContextMenu;
            fileNode.ItemHeight = ThumbImgSize;
            fileNode.Checked = true;
            fileNode.TextAlignment = ContentAlignment.MiddleCenter;

            return fileNode;
        }

        public static RadTreeNode CreateRadTreeFromTemplate(RadTreeView tree, BatchTemplateDef template)
        {
            return null;
        }

        /// <summary>
        /// 创建来自服务器文件的节点，根据FileNo更新BatchInfo的LastNo
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="fileInfo"></param>
        /// <param name="contextMenu"></param>
        /// <param name="batchInfo"></param>
        /// <returns></returns>
        public RadTreeNode CreateFileNodeFromServer(RadTreeNode parentNode, NFileInfo fileInfo, NBatchInfo batchInfo)
        {
            RadTreeNode fileNode = parentNode.Nodes.Add(fileInfo.DisplayName);
            fileNode.SetImageIcon(fileInfo.LocalPath, ThumbImgSize, ThumbImgSize);
            //node2.Image = ImageHelper.LoadSizedImage(info.LocalPath, this.GetSetting().ThumbImgSize, this.GetSetting().ThumbImgSize);
            fileNode.Tag = fileInfo;
            fileNode.Selected = true;
            fileNode.ToolTipText = fileInfo.ToUITipString();
            fileNode.ContextMenu = fileContextMenu;
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

        private void FileNode_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
             

        private void navigateTree_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            RadTreeNode input = e.Node;
            if ((input != null) && (this.OnItemSelectChanged != null))
            {
                this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(input));
            }
        }

        private void Nodes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Count")
            {
                RadTreeNodeCollection source = sender as RadTreeNodeCollection;
                int count = source.Count;
                if (source.Select<RadTreeNode, string>(o => o.Text).Distinct<string>().Count<string>() != count)
                {
                    LibCommon.AppContext.GetInstance().MS.LogError("请勿输入相同批次编号");
                }
                TabPage parent = UCNavigatorBar.GetInstance().Parent as TabPage;
                if (parent != null)
                {
                    if (count == 0)
                    {
                        parent.Text = "文件管理";
                    }
                    else
                    {
                        parent.Text = "文件管理 [" + count.ToString() + "]";
                    }
                }
            }
        }

        

        private void Fnodeexportpdf_Click(object sender, EventArgs e)
        {
            NFileInfo tag = navigateTree.SelectedNode.Tag as NFileInfo;
            if ((tag != null) && FileHelper.IsImageExt(tag.FileName))
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    FileName = tag.LocalPath + ".pdf"
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    PDFHelper.Img2PDF(tag.LocalPath, dialog.FileName);
                    SystemHelper.ExplorerFile(dialog.FileName);
                }
            }
        }

        private void BatchAddCategory_Click(object sender, EventArgs e)
        {
            RadTreeNode parentNode = sender as RadTreeNode;
            UCCategory ctrl = new UCCategory();
            FormContainer container = new FormContainer();
            container.SetControl(ctrl);
            if ((container.ShowDialog() == DialogResult.OK) && !(navigateTree.SelectedNode.Tag is NFileInfo))
            {
                NCategoryInfo categoryInfo = new NCategoryInfo(ctrl.CategoryName);
                RadTreeNode categoryNode = CreateCategoryNode(parentNode, categoryInfo);
                categoryNode.ContextMenu = this.categoryContextMenu;
                navigateTree.Refresh();
                navigateTree.SelectedNode.ExpandAll();
                Application.DoEvents();
                categoryNode.Selected = true;
                //categoryNode.ExpandAll();
                Application.DoEvents();
            }
        }

        private void menuBatchNode_AddScanF_Click(object sender, EventArgs e)
        {
            UCNavigatorBar.GetInstance()._lastScanOpeType = ScanOpe.AddToCur;
            UCNavigatorBar.GetInstance().DoScanBatchDoc();
        }

        private void menuBatchNode_DEL_Click(object sender, EventArgs e)
        {
            navigateTree.Nodes.Remove(navigateTree.SelectedNode);
            navigateTree.Refresh();
        }

        private void menuBatchNode_Property_Click(object sender, EventArgs e)
        {
            ShowNodePropertyCMD cmd = new ShowNodePropertyCMD
            {
                node = navigateTree.SelectedNode
            };
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(cmd);
        }

        private void Menucategoryadd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("UISetting", "LastAccessDir"),
                Multiselect = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("UISetting", "LastAccessDir", FileHelper.GetFileDir(dialog.FileNames[0]));
                RadTreeNode selectedNode = navigateTree.SelectedNode;
                RadTreeNode batchNode = selectedNode.GetBatchNode();
                NBatchInfo batchInfo = batchNode.Tag as NBatchInfo;
                List<NFileInfo> fileInfos = BeanUtil.FileDialog2FileInfo(dialog, batchInfo.BatchNO);
                AddNodeWithFileInfo(selectedNode, fileInfos, batchInfo);
                UpdateBatchNodeTitle(batchNode);
                batchNode.ExpandAll();
            }
        }

        private void Menucategoryaddsub_Click(object sender, EventArgs e)
        {
            this.BatchAddCategory_Click(sender, e);
        }

        private void Menucategorydel_Click(object sender, EventArgs e)
        {
            navigateTree.SelectedNode.RemoveCategoryNode();
        }

        private void Menucategorymodiname_Click(object sender, EventArgs e)
        {
            UCCategory ctrl = new UCCategory();
            FormContainer container = new FormContainer();
            container.SetControl(ctrl);
            if (container.ShowDialog() == DialogResult.OK)
            {
                navigateTree.SelectedNode.Text = ctrl.CategoryName;
                (navigateTree.SelectedNode.Tag as NCategoryInfo).CategoryName = ctrl.CategoryName;
                if ((navigateTree.SelectedNode.GetBatchNode().Tag as NBatchInfo).Operation == EOperType.eFROM_SERVER_NOTCHANGE)
                {

                }
            }
        }

        private void Menucategoryproperty_Click(object sender, EventArgs e)
        {
            ShowNodePropertyCMD cmd = new ShowNodePropertyCMD
            {
                node = navigateTree.SelectedNode
            };
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(cmd);
        }

        private void menuExplorerFile_Click(object sender, EventArgs e)
        {
            NFileInfo tag = navigateTree.SelectedNode.Tag as NFileInfo;
            SystemHelper.ExplorerFile(tag.LocalPath);
        }

        private void menuFileNode_REMOVE_Click(object sender, EventArgs e)
        {
            navigateTree.SelectedNode.RemovceFileNode();
            UpdateBatchNodeTitle(null);
            navigateTree.Refresh();
        }

        private void menuFileNodeSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "另存为",
                FileName = (navigateTree.SelectedNode.Tag as NFileInfo).LocalPath
            };
            if ((dialog.ShowDialog() == DialogResult.OK) && (dialog.FileName != (navigateTree.SelectedNode.Tag as NFileInfo).LocalPath))
            {
                File.Copy((navigateTree.SelectedNode.Tag as NFileInfo).LocalPath, dialog.FileName);
            }
        }

        private void menuReplaceWithScan_Click(object sender, EventArgs e)
        {
            UCNavigatorBar.GetInstance()._lastScanOpeType = ScanOpe.ReplaceCurrent;
            UCNavigatorBar.GetInstance().DoScanBatchDoc();
        }

        private void BatchNode_AddLocalF_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("UISetting", "LastAccessDir"),
                Multiselect = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("UISetting", "LastAccessDir", FileHelper.GetFileDir(dialog.FileNames[0]));
                RadTreeNode selectedNode = navigateTree.SelectedNode;
                //string str = selectedNode.FullPath.ToString().Substring(selectedNode.FullPath.ToString().LastIndexOf("]") + 1);
                NBatchInfo batchInfo = selectedNode.Tag as NBatchInfo;
                //新增文件转为NFileInfo对象
                List<NFileInfo> fileInfos = BeanUtil.FileDialog2FileInfo(dialog, batchInfo.BatchNO);
                //批次节点下增加文件节点
                AddNodeWithFileInfo(selectedNode, fileInfos, batchInfo);

                UpdateBatchNodeTitle(selectedNode);
                selectedNode.ExpandAll();
            }
        }

        private void AddNodeWithFileInfo(RadTreeNode parentNode, List<NFileInfo> fileInfos, NBatchInfo batchInfo)
        {
            foreach (NFileInfo fileInfo in fileInfos)
            {
                batchInfo.LastNO++;
                fileInfo.FileNO = batchInfo.LastNO.ToString();
                RadTreeNode node = parentNode.Nodes.Add(fileInfo.DisplayName);
                node.TextAlignment = ContentAlignment.MiddleCenter;
                node.SetImageIcon(fileInfo.LocalPath, ThumbImgSize, ThumbImgSize);
                node.Tag = fileInfo;
                if (fileInfo.Operation == EOperType.eFROM_SERVER_NOTCHANGE)
                {
                    node.ContextMenu = this.fileContextMenu;
                }
                else
                {
                    node.ContextMenu = this.fileContextMenu;
                }
                node.ItemHeight = UISetting.GetInstance().ThumbImgSize;
                node.Checked = true;
                node.TextAlignment = ContentAlignment.MiddleCenter;
                node.PropertyChanged += new PropertyChangedEventHandler(this.FileNode_PropertyChanged);
                Application.DoEvents();
            }
        }

        private void BatchExportPdf_Click(object sender, EventArgs e)
        {
            if ((this.navigateTree.SelectedNode.Nodes.Count != 0) && (this.navigateTree.SelectedNode.GetChildren().SelectNFileNode().Count != 0))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                string str = (this.navigateTree.SelectedNode.Tag as NBatchInfo).BatchNO + ".pdf";
                dialog.FileName = str;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    PDFHelper.Batch2PDF(this.navigateTree.SelectedNode.CreateTmpNBatchInfo(), dialog.FileName);
                    SystemHelper.ExplorerFile(dialog.FileName);
                }
            }
        }

        public RadTreeNode FromBatchTemplate(BatchTemplateDef template, NBatchInfo batchInfo)
        {
            RadTreeNode batchNode = null;
            TemplateNode templateRoot = template.RootNode;
            if(templateRoot != null)
            {
                batchNode = CreateBatchNode(batchInfo);
                if (templateRoot.HasChildren())
                {
                    foreach (TemplateNode templateChild in templateRoot.Children)
                    {
                        AddCategoryNodeByTemplateNode(batchNode, templateChild);
                    }
                }
            }
            return batchNode;
        }

        private void AddCategoryNodeByTemplateNode(RadTreeNode parentNode, TemplateNode templateNode)
        {
            RadTreeNode categoryNode = CreateCategoryNode(parentNode, new NCategoryInfo() { CategoryName = templateNode.Name });
            if (templateNode.HasChildren())
            {
                foreach(TemplateNode templateChild in templateNode.Children)
                {
                    AddCategoryNodeByTemplateNode(categoryNode, templateChild);
                }
            }
        }

        public void FromBatch(NBatchInfoGroup group)
        {
            if (((group != null) && (group.Batchs != null)) && (group.Batchs.Count > 0))
            {
                NBatchInfo batchInfo = group.Batchs[0];
                RadTreeNode batchNode = CreateBatchNode(batchInfo);
                List<NFileInfo> fileInfos = batchInfo.FileInfos;

                //fileInfos.ForEach(x => x.FileName = x.FileNO);
                //HashSet<String> categorys = new HashSet<string>();

                foreach (NFileInfo fileInfo in fileInfos)
                {
                    string category = fileInfo.Category;
                    if (String.IsNullOrEmpty(category)) //没有分类的文件
                    {
                        CreateFileNodeFromServer(batchNode, fileInfo, batchInfo);
                    }
                    else
                    {
                        String path = batchNode.Text + "." + category;
                        RadTreeNode categoryNode = navigateTree.GetNodeByPath(path, ".");
                        if (categoryNode == null)
                        {
                            categoryNode = navigateTree.AddNodeByPath(path, ".");
                            UpdateCategoryNode(categoryNode);
                            categoryNode.Parent.Nodes.Move(categoryNode.Index, 0);  //将分类节点移到父节点最前面的位置
                        }
                        CreateFileNodeFromServer(categoryNode, fileInfo, batchInfo);
                    }
                }
                batchNode.ExpandAll();
                UpdateBatchNodeTitle(batchNode);
            }
        }

        public NBatchInfoGroup ToBatch()
        {
            NBatchInfoGroup group = new NBatchInfoGroup();
            foreach (RadTreeNode node in navigateTree.Nodes)
            {
                NBatchInfo batchInfo = node.Tag as NBatchInfo;
                batchInfo.FileInfos.Clear();
                //List<String> categoryNames = node.Nodes.Where<RadTreeNode>(x => x.Tag is NCategoryInfo).ToList().Select<RadTreeNode, String>(x => (x.Tag as NCategoryInfo).CategoryName).ToList();
                //batchInfo.Categorys = categoryNames;
                int count = node.Nodes.Count;
                if (batchInfo.Operation == EOperType.eADD)
                {
                    //获取所有文件节点
                    List<RadTreeNode> fileNodes = NavigateTreeHelper.GetAllChildFileNodes(node);
                    foreach (RadTreeNode fileNode in fileNodes)
                    {
                        fileNode.UpdateFileNodeCatInfo();
                        batchInfo.FileInfos.Add(fileNode.Tag as NFileInfo);
                    }
                    group.Batchs.Add(batchInfo);
                }
                else
                {
                    if (batchInfo.Operation == EOperType.eFROM_SERVER_NOTCHANGE || batchInfo.Operation == EOperType.eUPD)
                    {
                        int version;
                        int num3 = 1;
                        batchInfo.FileInfos.Clear();
                        List<RadTreeNode> list2 = NavigateTreeHelper.GetAllChildFileNodes(node);
                        foreach (RadTreeNode node3 in list2)
                        {
                            node3.UpdateNodeNInfo();
                            NFileInfo info3 = node3.Tag as NFileInfo;
                            info3.SetupFileInfo();
                            if (info3.OrigData == null) //本地添加文件
                            {
                                info3.Operation = EOperType.eADD;
                                batchInfo.Operation = EOperType.eUPD;
                                batchInfo.FileInfos.Add(node3.Tag as NFileInfo);
                            }
                            else if (info3.Operation == EOperType.eDEL)  //要删除的文件, 清空数据，减少数据传输
                            {
                                info3.Data = null;
                                batchInfo.Operation = EOperType.eUPD;
                                batchInfo.FileInfos.Add(node3.Tag as NFileInfo);
                            }
                            else if (info3.FileMD5 != info3.OrigData.FileMD5)   //判断文件内容是否变化，有问题，操作节点时不操作Info对象
                            {
                                info3.Operation = EOperType.eUPDATEFILE;
                                NFileInfo origData = info3.OrigData;
                                version = origData.Version;
                                origData.Version = version + 1;
                                info3.Version = version;
                                batchInfo.Operation = EOperType.eUPD;
                                batchInfo.FileInfos.Add(node3.Tag as NFileInfo);
                            }
                            else if (!info3.MyEqual(info3.OrigData))    //判断文件属性是否变化
                            {
                                info3.Operation = EOperType.eUPDATEBASIC;
                                info3.Version = (info3.OrigData.Version++);
                                info3.Data = null;
                                batchInfo.Operation = EOperType.eUPD;
                                batchInfo.FileInfos.Add(node3.Tag as NFileInfo);
                            }
                            //没有任何变化不传输
                        }

                    }
                    if (batchInfo.Operation == EOperType.eUPD)
                    {
                        batchInfo.Version++;
                        group.Batchs.Add(batchInfo);
                    }
                }
            }
            return group;
        }

        public void DoClearBatchs(object param = null)
        {
            navigateTree.Nodes.Clear();
            this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(null));
            navigateTree.Refresh();
            TmpGC.EmptyRubbish();
        }

        /*public void ChangeView(object param)
        {
            foreach (RadTreeNode node in navigateTree.Nodes)
            {
                List<RadTreeNode> children = node.GetChildren();
                foreach (RadTreeNode node2 in children)
                {
                    if (node2.Tag is NFileInfo)
                    {
                        node2.SetImageIcon((node2.Tag as NFileInfo).LocalPath, this.showFileImage);
                    }
                }
            }
            this.showFileImage = !this.showFileImage;
        }*/

        public void NavigateFirstItem(object param = null)
        {
            if (navigateTree.SelectedNode != null)
            {
                if (navigateTree.SelectedNode.Tag is NFileInfo)
                {
                    navigateTree.SelectedNode.Parent.Nodes[0].Selected = true;
                }
                else if (navigateTree.SelectedNode.Nodes.Count > 0)
                {
                    navigateTree.SelectedNode.Nodes[0].Selected = true;
                }
                navigateTree.BringIntoView(navigateTree.SelectedNode);
            }
            else if (navigateTree.Nodes.Count > 0)
            {
                if (navigateTree.Nodes[0].Nodes.Count > 0)
                {
                    navigateTree.Nodes[0].Nodes[0].Selected = true;
                }
                navigateTree.BringIntoView(navigateTree.SelectedNode);
            }
        }

        public void NavigateLastItem(object param = null)
        {
            if (navigateTree.SelectedNode != null)
            {
                if (navigateTree.SelectedNode.Tag is NFileInfo)
                {
                    navigateTree.SelectedNode.Parent.Nodes[navigateTree.SelectedNode.Parent.Nodes.Count - 1].Selected = true;
                }
                else if (navigateTree.SelectedNode.Nodes.Count > 0)
                {
                    navigateTree.SelectedNode.Nodes[navigateTree.SelectedNode.Nodes.Count - 1].Selected = true;
                }
                navigateTree.BringIntoView(navigateTree.SelectedNode);
            }
            else if (navigateTree.Nodes.Count > 0)
            {
                RadTreeNode node = navigateTree.Nodes[navigateTree.Nodes.Count - 1];
                if (node.Nodes.Count > 0)
                {
                    node.Nodes[node.Nodes.Count - 1].Selected = true;
                }
                navigateTree.BringIntoView(navigateTree.SelectedNode);
            }
        }

        public void NavigateNextItem(object param = null)
        {
            if (navigateTree.SelectedNode != null)
            {
                if (navigateTree.SelectedNode == navigateTree.SelectedNode.Parent.Nodes[navigateTree.SelectedNode.Parent.Nodes.Count - 1])
                {
                    LibCommon.AppContext.GetInstance().MS.LogWarning("已经是最后一张");
                }
                else if ((navigateTree.SelectedNode != null) && (navigateTree.SelectedNode.NextSiblingNode != null))
                {
                    navigateTree.SelectedNode.NextSiblingNode.Selected = true;
                    navigateTree.BringIntoView(navigateTree.SelectedNode);
                }
            }
        }

        public void NavigatePrevItem(object param = null)
        {
            if (navigateTree.SelectedNode != null)
            {
                if (navigateTree.SelectedNode.Index == 0)
                {
                    LibCommon.AppContext.GetInstance().MS.LogWarning("已经是第一张");
                }
                if ((navigateTree.SelectedNode != null) && (navigateTree.SelectedNode.PrevSiblingNode != null))
                {
                    navigateTree.SelectedNode.PrevSiblingNode.Selected = true;
                    navigateTree.BringIntoView(navigateTree.SelectedNode);
                }
            }
        }

        public void UpdateBatchNodeTitle(RadTreeNode node)
        {
            if (node == null)
            {
                node = navigateTree.SelectedNode;
            }
            while (!(node.Tag is NBatchInfo))
            {
                node = node.Parent;
            }
            if (node != null)
            {
                node.Text = GetFileNodesCount(node).ToCommentString() + (node.Tag as NBatchInfo).BatchNO;
            }
        }

        public int GetFileNodesCount(RadTreeNode node)
        {
            if (((node == null) || (node.Nodes == null)) || (node.Nodes.Count == 0))
            {
                return 0;
            }
            int num = 0;
            foreach (RadTreeNode node2 in node.Nodes)
            {
                if (node2.Tag is NCategoryInfo)
                {
                    num += GetFileNodesCount(node2);
                }
                else if ((node2.Tag is NFileInfo) && ((node2.Tag as NFileInfo).Operation != EOperType.eDEL))
                {
                    num++;
                }
            }
            return num;
        }

        private void NavigateTreeOnItemSelectChanged(object sender, TEventArg<RadTreeNode> e)
        {
            NFileInfo curFileInfo = null;
            bool flag = e == null || e.Arg == null || e.Arg.Tag == null;
            if (!flag)
            {
                curFileInfo = (e.Arg.Tag as NFileInfo);
            }
            UCCenterView.GetInstance().CurFileInfo = curFileInfo;
            UCRightPane.GetIntstance().OnNodeChanged(e.Arg, false);
        }
    }
}
