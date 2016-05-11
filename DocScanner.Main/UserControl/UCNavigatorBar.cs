using DocScaner.PDF.Utils;
using DocScanner.AdapterFactory;
using DocScanner.Bean;
using DocScanner.Bean.pb;
using DocScanner.Common;
using DocScanner.ImgUtils;
using DocScanner.LibCommon;
using DocScanner.LibCommon.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class UCNavigatorBar : UserControl, IHasIPropertiesSetting
    {
        // Fields
        private IFileAcquirer _acq;
        private ScanOpe _lastScanOpeType;
        private RadContextMenu _menuCategoryNode = new RadContextMenu();
        private RadContextMenu _menudefaultbatchnode = new RadContextMenu();
        private RadContextMenu _menudownloadedbatchnode = new RadContextMenu();
        private RadContextMenu _menuFileNode = new RadContextMenu();
        private RadContextMenu _menuFileNodenoDel = new RadContextMenu();
        private NestIPropertiesSetting _setting;
        private bool _viewfileinfoicon = true;
        private IContainer components = null;
        private RadTreeView radTreeView1;

        // Events
        public event EventHandler<TEventArg<RadTreeNode>> OnItemSelectChanged;

        public delegate void NodePropertyEventHandler(Object sender, NodePropertyEventArgs e);
        public event NodePropertyEventHandler nodeProperty;

        public class NodePropertyEventArgs: EventArgs
        {
            private RadTreeNode node;

            public NodePropertyEventArgs(RadTreeNode node)
            {
                this.node = node;
            }
        }

        // Methods
        public UCNavigatorBar()
        {
            this.InitializeComponent();
            this.radTreeView1.CheckBoxes = false;

            this.radTreeView1.ExpandAnimation = ExpandAnimation.Opacity;
            this.radTreeView1.AllowDragDrop = true;
            this.radTreeView1.AllowEdit = true;
            this.radTreeView1.TreeIndent = 35;
            this.radTreeView1.TreeViewElement.ExpandImage = Properties.Resources.expand.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            this.radTreeView1.TreeViewElement.HoveredExpandImage = Properties.Resources.expand.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            this.radTreeView1.TreeViewElement.CollapseImage = Properties.Resources.collapse.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            this.radTreeView1.TreeViewElement.HoveredCollapseImage = Properties.Resources.collapse.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            this.radTreeView1.LineStyle = TreeLineStyle.Dot;
            this.radTreeView1.LineColor = Color.DeepSkyBlue;
            //this.radTreeView1.ItemHeight = this.GetSetting().ThumbImgSize;
            //this.radTreeView1.ImageScalingSize = new Size(this.GetSetting().ThumbImgSize, this.GetSetting().ThumbImgSize);

            this.radTreeView1.AllowArbitraryItemHeight = true;
            this.radTreeView1.ShowItemToolTips = true;
            this.radTreeView1.SelectedNodeChanged += radTreeView1_SelectedNodeChanged;
            this.radTreeView1.Nodes.PropertyChanged += this.Nodes_PropertyChanged;
            this.SetupBatchNodeMenu();
            this.SetupCategoryNodeMenu();
            this.SetupFileNodeMenu();
            //base.Load += new EventHandler(this.UCNavigatorBar_Load);
        }

        private void _acq_OnAcquired(object sender, TEventArg<string> e)
        {
            if (base.InvokeRequired)
            {
                object[] args = new object[] { sender, e };
                base.Invoke(new Action<object, TEventArg<string>>(this._acq_OnAcquired), args);
            }
            else
            {
                e.Arg = ImageChainProcessor.Cur.Process(e.Arg);
                this.OnFileAcquied(e.Arg);
            }
        }

        private void _acq_OnError(object sender, TEventArg<string> e)
        {
            LibCommon.AppContext.GetInstance().MS.LogError(e.Arg);
        }

        public RadTreeNode AddFileNode(RadTreeNode batchnode, NFileInfo info)
        {
            if (string.IsNullOrEmpty(info.Category))
            {
                RadTreeNode node = batchnode.Nodes.Add(info.DisplayName);
                node.Tag = info;
                return node;
            }
            RadTreeNode node3 = batchnode;
            char[] separator = new char[] { '.' };
            string[] strArray2 = info.Category.Split(separator);
            for (int i = 0; i < strArray2.Length; i++)
            {
                string key = strArray2[i];
                IEnumerable<RadTreeNode> source = from o in node3.Nodes
                                                  where (o.Text == key) && (o.Tag is NCategoryInfo)
                                                  select o;
                if ((source != null) && (source.Count<RadTreeNode>() > 0))
                {
                    node3 = source.First<RadTreeNode>();
                }
                else
                {
                    RadTreeNode node5 = node3.Nodes.Add(key);
                    node5.Tag = new NCategoryInfo(key);
                    this.SetCategoryNodeDefaultProperty(node5);
                    node3 = node5;
                    node3.Expand();
                }
            }
            RadTreeNode node4 = node3.Nodes.Add(info.DisplayName);
            node4.Tag = info;
            return node4;
        }

        private void BatchAddCategory_Click(object sender, EventArgs e)
        {
            UCCategory ctrl = new UCCategory();
            FormContainer container = new FormContainer();
            container.SetControl(ctrl);
            if ((container.ShowDialog() == DialogResult.OK) && !(this.radTreeView1.SelectedNode.Tag is NFileInfo))
            {
                NCategoryInfo categoryInfo = new NCategoryInfo(ctrl.CategoryName);
                RadTreeNode categoryNode = NavigateTreeHelper.CreateCategoryNode(this.radTreeView1, categoryInfo);
                categoryNode.ContextMenu = this._menuCategoryNode;
                this.radTreeView1.Refresh();
                this.radTreeView1.SelectedNode.ExpandAll();
                Application.DoEvents();
                categoryNode.Selected = true;
                //categoryNode.ExpandAll();
                Application.DoEvents();
            }
        }

        private void BatchExportPdf_Click(object sender, EventArgs e)
        {
            if ((this.radTreeView1.SelectedNode.Nodes.Count != 0) && (this.radTreeView1.SelectedNode.GetChildren().SelectNFileNode().Count != 0))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                string str = (this.radTreeView1.SelectedNode.Tag as NBatchInfo).BatchNO + ".pdf";
                dialog.FileName = str;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    PDFHelper.Batch2PDF(this.radTreeView1.SelectedNode.CreateTmpNBatchInfo(), dialog.FileName);
                    SystemHelper.ExplorerFile(dialog.FileName);
                }
            }
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
                RadTreeNode selectedNode = this.radTreeView1.SelectedNode;
                //string str = selectedNode.FullPath.ToString().Substring(selectedNode.FullPath.ToString().LastIndexOf("]") + 1);
                NBatchInfo batchInfo = selectedNode.Tag as NBatchInfo;
                //新增文件转为NFileInfo对象
                List<NFileInfo> fileInfos = BeanUtil.FileDialog2FileInfo(dialog, batchInfo.BatchNO);
                //批次节点下增加文件节点
                AddNodeWithFileInfo(selectedNode, fileInfos, batchInfo);

                radTreeView1.UpdateBatchNodeTitle(selectedNode);
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
                node.SetImageIcon(fileInfo.LocalPath, true);
                node.Tag = fileInfo;
                if (fileInfo.Operation == EOperType.eFROM_SERVER_NOTCHANGE)
                {
                    node.ContextMenu = this._menuFileNode;
                }
                else
                {
                    node.ContextMenu = this._menuFileNode;
                }
                node.ItemHeight = this.GetSetting().ThumbImgSize;
                node.Checked = true;
                node.TextAlignment = ContentAlignment.MiddleCenter;
                node.PropertyChanged += new PropertyChangedEventHandler(this.FileNode_PropertyChanged);
                Application.DoEvents();
            }
            //(parentNode.Tag as NBatchInfo).FileInfos.AddRange(fileInfos);
            //batchInfo.FileInfos.AddRange(fileInfos);
        }

        private void BatchNode_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Checked")
            {
                RadTreeNode node = sender as RadTreeNode;
                if (!node.Checked)
                {
                    node.Nodes.ForEach<RadTreeNode>(o => o.Checked = false);
                }
                else if (node.Nodes.Any<RadTreeNode>(o => o.Checked))
                {
                    foreach (RadTreeNode node2 in node.Nodes)
                    {
                        node2.Checked = true;
                    }
                }
            }
        }

        public void BatchToTreeView(NBatchInfoGroup group)
        {
            if (((group != null) && (group.Batchs != null)) && (group.Batchs.Count > 0))
            {
                NBatchInfo batchInfo = group.Batchs[0];
                RadTreeNode batchNode = NavigateTreeHelper.CreateBatchNode(this.radTreeView1, batchInfo, this._menudefaultbatchnode);
                List<NFileInfo> fileInfos = batchInfo.FileInfos;

                //fileInfos.ForEach(x => x.FileName = x.FileNO);
                //HashSet<String> categorys = new HashSet<string>();

                foreach (NFileInfo fileInfo in fileInfos)
                {
                    string category = fileInfo.Category;
                    if (String.IsNullOrEmpty(category)) //没有分类的文件
                    {
                        NavigateTreeHelper.CreateFileNodeFromServer(batchNode, fileInfo, this._menuFileNode, batchInfo);
                    }
                    else
                    {
                        String path = batchNode.Text + "." + category;
                        RadTreeNode categoryNode = this.radTreeView1.GetNodeByPath(path, ".");
                        if (categoryNode == null)
                        {
                            categoryNode = this.radTreeView1.AddNodeByPath(path, ".");
                            NavigateTreeHelper.UpdateCategoryNode(categoryNode, this._menuCategoryNode);
                            categoryNode.Parent.Nodes.Move(categoryNode.Index, 0);  //将分类节点移到父节点最前面的位置
                        }
                        NavigateTreeHelper.CreateFileNodeFromServer(categoryNode, fileInfo, this._menuFileNode, batchInfo);
                    }
                }
                batchNode.ExpandAll();
                radTreeView1.UpdateBatchNodeTitle(batchNode);
            }
        }

        public void ChangeView(object param)
        {
            foreach (RadTreeNode node in this.radTreeView1.Nodes)
            {
                List<RadTreeNode> children = node.GetChildren();
                foreach (RadTreeNode node2 in children)
                {
                    if (node2.Tag is NFileInfo)
                    {
                        node2.SetImageIcon((node2.Tag as NFileInfo).LocalPath, this._viewfileinfoicon);
                    }
                }
            }
            this._viewfileinfoicon = !this._viewfileinfoicon;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DoClearBatchs(object param = null)
        {
            this.radTreeView1.Nodes.Clear();
            this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(null));
            this.radTreeView1.Refresh();
            TmpGC.EmptyRubbish();
        }

        public void DoFilterImg(object param = null)
        {
            new UCFilterImg().ShowInContainer();
        }

        public void DoNewBatch(object param = null)
        {
            string str = BatchNoMaker.Cur.FromInputDialog("");
            if (!string.IsNullOrEmpty(str))
            {
                NBatchInfo info = new NBatchInfo
                {
                    Operation = EOperType.eADD,
                    BatchNO = str
                };
                RadTreeNode node = BatchTemplatedef.CreateRadTreeFromTemplate(this.radTreeView1, BatchNoMaker.Cur.SelectedTemplate, info.DisplayName);
                node.Text = info.DisplayName;
                node.Tag = info;
                this.UpdateAllNodeMenu(node);
            }
        }

        public void DoScanBatchDoc()
        {
            this._acq = LibCommon.AppContext.GetInstance().GetVal<SharpAcquirerFactory>(typeof(SharpAcquirerFactory)).GetAdapter("");
            this._acq.OnAcquired -= new EventHandler<TEventArg<string>>(this._acq_OnAcquired);
            this._acq.OnError -= new EventHandler<TEventArg<string>>(this._acq_OnError);
            this._acq.OnAcquired += new EventHandler<TEventArg<string>>(this._acq_OnAcquired);
            this._acq.OnError += new EventHandler<TEventArg<string>>(this._acq_OnError);
            IAcquirerParam initparam = new IAcquirerParam
            {
                HostWnd = base.Handle
            };
            this._acq.Initialize(initparam);
            using (new DurTimeJob("采集中.."))
            {
                this._acq.Acquire();
            }
            if (((this.radTreeView1.Nodes.Count > 0) && (this.radTreeView1.Nodes[this.radTreeView1.Nodes.Count - 1].Nodes.Count > 0)) && (this._lastScanOpeType != ScanOpe.ReplaceCurrent))
            {
                RadTreeNode node = this.radTreeView1.Nodes[this.radTreeView1.Nodes.Count - 1];
                RadTreeNode input = node.Nodes[node.Nodes.Count - 1];
                if (this.OnItemSelectChanged != null)
                {
                    this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(input));
                }
            }
        }

        public void DoSelectNode(object info)
        {
            Tuple<string, string> data = info as Tuple<string, string>;
            if (data != null)
            {
                IEnumerable<bool> source = from o in this.radTreeView1.Nodes select (o.Tag as NBatchInfo).BatchNO == data.Item1;
                if ((source != null) && (source.Count<bool>() > 0))
                {
                }
            }
        }

        public void DownloadBatch(object param = null)
        {
            UCQueryBatch ctrl = new UCQueryBatch();
            FormQBContainer container = new FormQBContainer();
            container.AddControl(ctrl);
            container.AddControl(typeof(UCQueryNBatchHis));
            if (container.ShowDialog() == DialogResult.OK)
            {
                ctrl.DownloadGroup.Update2NoneMode();   //更新所有状态为from server not change
                NBatchInfoGroup group = ctrl.DownloadGroup.MyClone();
                this.BatchToTreeView(group);
            }
        }

        private void FileNode_Property_Click(object sender, EventArgs e)
        {
            ShowNodePropertyCMD cmd = new ShowNodePropertyCMD
            {
                node = this.radTreeView1.SelectedNode
            };
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(cmd);
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

        private void Fnodeexportpdf_Click(object sender, EventArgs e)
        {
            NFileInfo tag = this.radTreeView1.SelectedNode.Tag as NFileInfo;
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

        public NBatchInfoGroup FromTree()
        {
            NBatchInfoGroup group = new NBatchInfoGroup();
            foreach (RadTreeNode node in this.radTreeView1.Nodes)
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


        public NestIPropertiesSetting GetSetting()
        {
            if (this._setting == null)
            {
                this._setting = new NestIPropertiesSetting(this);
            }
            return this._setting;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.radTreeView1 = new Telerik.WinControls.UI.RadTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).BeginInit();
            this.SuspendLayout();
            // 
            // radTreeView1
            // 
            this.radTreeView1.AllowDragDrop = true;
            this.radTreeView1.AllowDragDropBetweenTreeViews = false;
            this.radTreeView1.AllowDrop = true;
            this.radTreeView1.AllowPlusMinusAnimation = true;
            this.radTreeView1.AllowShowFocusCues = true;
            this.radTreeView1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radTreeView1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.radTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTreeView1.DropFeedbackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.radTreeView1.Location = new System.Drawing.Point(0, 0);
            this.radTreeView1.Name = "treeView1";
            // 
            // 
            // 
            this.radTreeView1.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 150, 250);
            this.radTreeView1.ShowLines = true;
            this.radTreeView1.ShowRootLines = false;
            this.radTreeView1.Size = new System.Drawing.Size(233, 634);
            this.radTreeView1.SpacingBetweenNodes = 12;
            this.radTreeView1.TabIndex = 0;
            // 
            // UCNavigatorBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radTreeView1);
            this.Name = "UCNavigatorBar";
            this.Size = new System.Drawing.Size(233, 634);
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).EndInit();
            this.ResumeLayout(false);

        }

        private void menuBatchNode_AddScanF_Click(object sender, EventArgs e)
        {
            this._lastScanOpeType = ScanOpe.AddToCur;
            this.DoScanBatchDoc();
        }

        private void menuBatchNode_DEL_Click(object sender, EventArgs e)
        {
            this.radTreeView1.Nodes.Remove(this.radTreeView1.SelectedNode);
            this.radTreeView1.Refresh();
        }

        private void menuBatchNode_Property_Click(object sender, EventArgs e)
        {
            ShowNodePropertyCMD cmd = new ShowNodePropertyCMD
            {
                node = this.radTreeView1.SelectedNode
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
                RadTreeNode selectedNode = this.radTreeView1.SelectedNode;
                RadTreeNode batchNode = selectedNode.GetBatchNode();
                NBatchInfo batchInfo = batchNode.Tag as NBatchInfo;
                List<NFileInfo> fileInfos = BeanUtil.FileDialog2FileInfo(dialog, batchInfo.BatchNO);
                AddNodeWithFileInfo(selectedNode, fileInfos, batchInfo);
                radTreeView1.UpdateBatchNodeTitle(batchNode);
                batchNode.ExpandAll();
            }
        }

        private void Menucategoryaddsub_Click(object sender, EventArgs e)
        {
            this.BatchAddCategory_Click(sender, e);
        }

        private void Menucategorydel_Click(object sender, EventArgs e)
        {
            this.radTreeView1.SelectedNode.RemoveCategoryNode();
        }

        private void Menucategorymodiname_Click(object sender, EventArgs e)
        {
            UCCategory ctrl = new UCCategory();
            FormContainer container = new FormContainer();
            container.SetControl(ctrl);
            if (container.ShowDialog() == DialogResult.OK)
            {
                this.radTreeView1.SelectedNode.Text = ctrl.CategoryName;
                (this.radTreeView1.SelectedNode.Tag as NCategoryInfo).CategoryName = ctrl.CategoryName;
                if ((radTreeView1.SelectedNode.GetBatchNode().Tag as NBatchInfo).Operation == EOperType.eFROM_SERVER_NOTCHANGE)
                {

                }
            }
        }

        private void Menucategoryproperty_Click(object sender, EventArgs e)
        {
            ShowNodePropertyCMD cmd = new ShowNodePropertyCMD
            {
                node = this.radTreeView1.SelectedNode
            };
            LibCommon.AppContext.GetInstance().GetVal<CmdDispatcher>(typeof(CmdDispatcher)).ProcessCMD(cmd);
        }

        private void menuExplorerFile_Click(object sender, EventArgs e)
        {
            NFileInfo tag = this.radTreeView1.SelectedNode.Tag as NFileInfo;
            SystemHelper.ExplorerFile(tag.LocalPath);
        }

        private void menuFileNode_REMOVE_Click(object sender, EventArgs e)
        {
            this.radTreeView1.SelectedNode.RemovceFileNode();
            radTreeView1.UpdateBatchNodeTitle(null);
            this.radTreeView1.Refresh();
        }

        private void menuFileNodeSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "另存为",
                FileName = (this.radTreeView1.SelectedNode.Tag as NFileInfo).LocalPath
            };
            if ((dialog.ShowDialog() == DialogResult.OK) && (dialog.FileName != (this.radTreeView1.SelectedNode.Tag as NFileInfo).LocalPath))
            {
                File.Copy((this.radTreeView1.SelectedNode.Tag as NFileInfo).LocalPath, dialog.FileName);
            }
        }

        private void menuReplaceWithScan_Click(object sender, EventArgs e)
        {
            this._lastScanOpeType = ScanOpe.ReplaceCurrent;
            this.DoScanBatchDoc();
        }

        public void NavigateFirstItem(object param = null)
        {
            if (this.radTreeView1.SelectedNode != null)
            {
                if (this.radTreeView1.SelectedNode.Tag is NFileInfo)
                {
                    this.radTreeView1.SelectedNode.Parent.Nodes[0].Selected = true;
                }
                else if (this.radTreeView1.SelectedNode.Nodes.Count > 0)
                {
                    this.radTreeView1.SelectedNode.Nodes[0].Selected = true;
                }
                this.radTreeView1.BringIntoView(this.radTreeView1.SelectedNode);
            }
            else if (this.radTreeView1.Nodes.Count > 0)
            {
                if (this.radTreeView1.Nodes[0].Nodes.Count > 0)
                {
                    this.radTreeView1.Nodes[0].Nodes[0].Selected = true;
                }
                this.radTreeView1.BringIntoView(this.radTreeView1.SelectedNode);
            }
        }

        public void NavigateLastItem(object param = null)
        {
            if (this.radTreeView1.SelectedNode != null)
            {
                if (this.radTreeView1.SelectedNode.Tag is NFileInfo)
                {
                    this.radTreeView1.SelectedNode.Parent.Nodes[this.radTreeView1.SelectedNode.Parent.Nodes.Count - 1].Selected = true;
                }
                else if (this.radTreeView1.SelectedNode.Nodes.Count > 0)
                {
                    this.radTreeView1.SelectedNode.Nodes[this.radTreeView1.SelectedNode.Nodes.Count - 1].Selected = true;
                }
                this.radTreeView1.BringIntoView(this.radTreeView1.SelectedNode);
            }
            else if (this.radTreeView1.Nodes.Count > 0)
            {
                RadTreeNode node = this.radTreeView1.Nodes[this.radTreeView1.Nodes.Count - 1];
                if (node.Nodes.Count > 0)
                {
                    node.Nodes[node.Nodes.Count - 1].Selected = true;
                }
                this.radTreeView1.BringIntoView(this.radTreeView1.SelectedNode);
            }
        }

        public void NavigateNextItem(object param = null)
        {
            if (this.radTreeView1.SelectedNode != null)
            {
                if (this.radTreeView1.SelectedNode == this.radTreeView1.SelectedNode.Parent.Nodes[this.radTreeView1.SelectedNode.Parent.Nodes.Count - 1])
                {
                    LibCommon.AppContext.GetInstance().MS.LogWarning("已经是最后一张");
                }
                else if ((this.radTreeView1.SelectedNode != null) && (this.radTreeView1.SelectedNode.NextSiblingNode != null))
                {
                    this.radTreeView1.SelectedNode.NextSiblingNode.Selected = true;
                    this.radTreeView1.BringIntoView(this.radTreeView1.SelectedNode);
                }
            }
        }

        public void NavigatePrevItem(object param = null)
        {
            if (this.radTreeView1.SelectedNode != null)
            {
                if (this.radTreeView1.SelectedNode.Index == 0)
                {
                    LibCommon.AppContext.GetInstance().MS.LogWarning("已经是第一张");
                }
                if ((this.radTreeView1.SelectedNode != null) && (this.radTreeView1.SelectedNode.PrevSiblingNode != null))
                {
                    this.radTreeView1.SelectedNode.PrevSiblingNode.Selected = true;
                    this.radTreeView1.BringIntoView(this.radTreeView1.SelectedNode);
                }
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
                TabPage parent = base.Parent as TabPage;
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

        protected virtual void OnFileAcquied(string FilePath)
        {
            NFileInfo fileInfo = new NFileInfo();
            fileInfo.SetDate(DateTime.Now);
            fileInfo.Author = AbstractSetting<AccountSetting>.CurSetting.AccountName;
            fileInfo.LocalPath = FilePath;
            fileInfo.FileName = FileHelper.GetFileName(FilePath);
            fileInfo.Operation = EOperType.eADD;

            if (this._lastScanOpeType == ScanOpe.Add)
            {
                RadTreeNode batchNode;
                NBatchInfo batchInfo = null;

                if (this.radTreeView1.Nodes.Count == 0)
                {
                    string batchNo = BatchNoMaker.Cur.FromInputDialog(FilePath);
                    if (string.IsNullOrEmpty(batchNo))
                    {
                        return;
                    }
                    batchInfo = new NBatchInfo
                    {
                        BatchNO = batchNo
                    };
                    batchNode = NavigateTreeHelper.CreateBatchNode(this.radTreeView1, batchInfo, _menudefaultbatchnode);
}
                else if (this.radTreeView1.SelectedNode != null)
                {
                    batchNode = this.radTreeView1.SelectedNode.GetBatchNode();
                    batchInfo = batchNode.Tag as NBatchInfo;
                }
                else
                {
                    batchNode = this.radTreeView1.Nodes[this.radTreeView1.Nodes.Count - 1];
                    batchInfo = batchNode.Tag as NBatchInfo;
                }
                RadTreeNode fileNode = NavigateTreeHelper.CreateFileNodeFromLocal(batchNode, fileInfo, this._menuFileNode, batchInfo);
 
                batchNode.ExpandAll();
                radTreeView1.UpdateBatchNodeTitle(batchNode);
                Application.DoEvents();
            }
            else if (this._lastScanOpeType == ScanOpe.ReplaceCurrent)
            {
                RadTreeNode selectedNode = this.radTreeView1.SelectedNode.UpdateFileNode(fileInfo);
                this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(selectedNode));
            }
            else if (this._lastScanOpeType == ScanOpe.AddToCur)
            {
                //TODO AddToCur?
                RadTreeNode node = this.radTreeView1.SelectedNode;
                if (node.Tag is NFileInfo)
                {
                    node = node.Parent;
                }
                NFileInfo info4 = new NFileInfo();
                info4.SetDate(DateTime.Now);
                info4.LocalPath = FilePath;
                info4.FileName = FileHelper.GetFileName(FilePath);
                info4.Operation = EOperType.eADD;
                RadTreeNode node5 = node.Nodes.Add(info4.DisplayName);
                node.TextAlignment = ContentAlignment.MiddleCenter;
                node5.SetImageIcon(info4.LocalPath, this._viewfileinfoicon);
                node5.Tag = info4;
                this.SetFileNodeDefualtProperty(node5);
                node5.Selected = true;
                node5.ToolTipText = info4.ToUITipString();
                node.ExpandAll();
                radTreeView1.UpdateBatchNodeTitle(node.GetBatchNode());
                Application.DoEvents();
            }
        }

        private void radTreeView1_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            RadTreeNode input = e.Node;
            if ((input != null) && (this.OnItemSelectChanged != null))
            {
                this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(input));
            }
        }

        public void ScanBatchDoc(object param)
        {
            this._lastScanOpeType = ScanOpe.Add;
            this.DoScanBatchDoc();
        }

        private RadTreeNode SetBatchNodeDefaultProperty(RadTreeNode node)
        {
            if (node != null)
            {
                node.AllowDrop = false;
                node.Checked = true;
                node.Checked = true;
                node.ItemHeight = this.GetSetting().BatchNodeHeight;
                node.Font = new Font(this.Font.Name, NavigateTreeHelper.Lev1NodeFontSize);
                node.ContextMenu = this._menudefaultbatchnode;
                node.PropertyChanged += new PropertyChangedEventHandler(this.BatchNode_PropertyChanged);
            }
            return node;
        }

        private void SetCategoryNodeDefaultProperty(RadTreeNode node)
        {
            node.ContextMenu = this._menuCategoryNode;
            node.ShowCheckBox = false;
        }

        public RadTreeNode SetFileNodeDefualtProperty(RadTreeNode node)
        {
            if ((node.Tag as NFileInfo).Operation == EOperType.eFROM_SERVER_NOTCHANGE)
            {
                node.ContextMenu = this._menuFileNode;
            }
            else
            {
                node.ContextMenu = this._menuFileNode;
            }
            node.ItemHeight = this.GetSetting().ThumbImgSize;
            node.Checked = true;
            node.TextAlignment = ContentAlignment.MiddleCenter;
            node.PropertyChanged += new PropertyChangedEventHandler(this.FileNode_PropertyChanged);
            return node;
        }

        private void SetupBatchNodeMenu()
        {
            RadMenuItem item = new RadMenuItem
            {
                Text = "属性"
            };
            item.Click += new EventHandler(this.FileNode_Property_Click);
            this._menudefaultbatchnode.Items.Add(item);
            if (this.GetSetting().AllowDelMenu)
            {
                RadMenuItem item6 = new RadMenuItem
                {
                    Text = "移除"
                };
                item6.Click += new EventHandler(this.menuBatchNode_DEL_Click);
                this._menudefaultbatchnode.Items.Add(item6);
            }
            RadMenuItem item2 = new RadMenuItem
            {
                Text = "从本地添加文件到批次中"
            };
            item2.Click += new EventHandler(this.BatchNode_AddLocalF_Click);
            this._menudefaultbatchnode.Items.Add(item2);
            RadMenuItem item3 = new RadMenuItem
            {
                Text = "从扫描设备补扫并添加到批次中"
            };
            item3.Click += new EventHandler(this.menuBatchNode_AddScanF_Click);
            this._menudefaultbatchnode.Items.Add(item3);
            RadMenuItem item4 = new RadMenuItem
            {
                Text = "新建分类"
            };
            item4.Click += new EventHandler(this.BatchAddCategory_Click);
            this._menudefaultbatchnode.Items.Add(item4);
            RadMenuItem item5 = new RadMenuItem
            {
                Text = "导出到pdf"
            };
            item5.Click += new EventHandler(this.BatchExportPdf_Click);
            this._menudefaultbatchnode.Items.Add(item5);
        }

        private void SetupCategoryNodeMenu()
        {
            RadMenuItem item = new RadMenuItem
            {
                Text = "修改分类名称"
            };
            item.Click += new EventHandler(this.Menucategorymodiname_Click);
            this._menuCategoryNode.Items.Add(item);
            RadMenuItem item2 = new RadMenuItem
            {
                Text = "新建子分类"
            };
            item2.Click += new EventHandler(this.Menucategoryaddsub_Click);
            this._menuCategoryNode.Items.Add(item2);
            RadMenuItem item3 = new RadMenuItem();
            RadMenuItem item4 = new RadMenuItem();
            item3.Text = "删除分类";
            item3.Click += new EventHandler(this.Menucategorydel_Click);
            this._menuCategoryNode.Items.Add(item3);
            RadMenuItem item5 = new RadMenuItem
            {
                Text = "从本地添加文件到批次分类中"
            };
            item5.Click += new EventHandler(this.Menucategoryadd_Click);
            this._menuCategoryNode.Items.Add(item5);
            RadMenuItem item6 = new RadMenuItem
            {
                Text = "分类属性"
            };
            item6.Click += new EventHandler(this.Menucategoryproperty_Click);
            this._menuCategoryNode.Items.Add(item6);
        }

        private void SetupFileNodeMenu()
        {
            RadMenuItem item = new RadMenuItem
            {
                Text = "属性"
            };
            item.Click += new EventHandler(this.FileNode_Property_Click);
            this._menuFileNode.Items.Add(item);
            RadMenuItem item2 = new RadMenuItem
            {
                Text = "移除"
            };
            item2.Click += new EventHandler(this.menuFileNode_REMOVE_Click);
            this._menuFileNode.Items.Add(item2);
            RadMenuItem item3 = new RadMenuItem
            {
                Text = "另存为..."
            };
            item3.Click += new EventHandler(this.menuFileNodeSaveAs_Click);
            this._menuFileNode.Items.Add(item3);
            RadMenuItem item4 = new RadMenuItem
            {
                Text = "使用本地应用程序打开"
            };
            item4.Click += new EventHandler(this.menuExplorerFile_Click);
            this._menuFileNode.Items.Add(item4);
            RadMenuItem item5 = new RadMenuItem
            {
                Text = "重新采集替换此图..."
            };
            item5.Click += new EventHandler(this.menuReplaceWithScan_Click);
            this._menuFileNode.Items.Add(item5);
            RadMenuItem item6 = new RadMenuItem
            {
                Text = "导出到pdf"
            };
            item6.Click += new EventHandler(this.Fnodeexportpdf_Click);
            this._menuFileNode.Items.Add(item6);
        }

        private void SetupNodeDefaultProperty(RadTreeNode node)
        {
            if ((node.Tag != null) && (node.Tag is NCategoryInfo))
            {
                this.SetCategoryNodeDefaultProperty(node);
            }
            if ((node.Tag != null) && (node.Tag is NFileInfo))
            {
                this.SetFileNodeDefualtProperty(node);
            }
            if ((node.Tag != null) && (node.Tag is NBatchInfo))
            {
                this.SetBatchNodeDefaultProperty(node);
            }
        }

        public void SetupTreeMenu()
        {
            this.radTreeView1.AllowDefaultContextMenu = false;
            RadContextMenuManager manager = new RadContextMenuManager();
            RadContextMenu menu = new RadContextMenu();
            RadMenuItem item = new RadMenuItem
            {
                Text = "新建批次"
            };
            item.Click += (sender, e) => this.DoNewBatch(null);
            menu.Items.Add(item);
            RadMenuItem item2 = new RadMenuItem
            {
                Text = "清空所有批次"
            };
            item2.Click += (sender, e) => this.DoClearBatchs(null);
            menu.Items.Add(item2);
            manager.SetRadContextMenu(this.radTreeView1, menu);
        }

        public void TestFromFileToTree(object param = null)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                NBatchInfo info = NBatchInfo.FromPBFile(dialog.FileName);
                info.ExtractFileData(LibCommon.AppContext.GetInstance().GetVal<AppSetting>(typeof(AppSetting)).TmpFileDir);
                NBatchInfoGroup group = new NBatchInfoGroup
                {
                    Batchs = { info }
                };
                this.BatchToTreeView(group);
            }
        }

        IPropertiesSetting IHasIPropertiesSetting.GetSetting()
        {
            return this.GetSetting();
        }

        private void UCNavigatorBar_Load(object sender, EventArgs e)
        {
            //this.components = new Container();
            //this.radTreeView1 = new RadTreeView();
            ((ISupportInitialize)this.radTreeView1).BeginInit();
            base.SuspendLayout();
            this.radTreeView1.AllowDragDrop = true;
            this.radTreeView1.AllowDragDropBetweenTreeViews = false;
            this.radTreeView1.AllowDrop = true;
            this.radTreeView1.AllowPlusMinusAnimation = true;
            this.radTreeView1.AllowShowFocusCues = true;
            this.radTreeView1.BackColor = SystemColors.ControlLightLight;
            this.radTreeView1.BackgroundImageLayout = ImageLayout.Stretch;
            this.radTreeView1.Dock = DockStyle.Fill;
            this.radTreeView1.DropFeedbackColor = Color.FromArgb(255, 128, 0);
            this.radTreeView1.Location = new Point(0, 0);
            this.radTreeView1.Name = "treeView1";
            this.radTreeView1.RootElement.ControlBounds = new Rectangle(0, 0, 150, 250);
            this.radTreeView1.ShowLines = true;
            this.radTreeView1.ShowRootLines = false;
            this.radTreeView1.Size = new Size(233, 687);
            this.radTreeView1.SpacingBetweenNodes = 12;
            this.radTreeView1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.radTreeView1);
            base.Name = "UCNavigatorBar";
            base.Size = new Size(233, 687);
            ((ISupportInitialize)this.radTreeView1).EndInit();
            base.ResumeLayout(false);
        }

        private void UpdateAllNodeMenu(RadTreeNode node)
        {
            this.SetBatchNodeDefaultProperty(node);
            List<RadTreeNode> children = node.GetChildren();
            foreach (RadTreeNode node2 in children)
            {
                this.SetupNodeDefaultProperty(node2);
            }
        }

        public void UpdateThumbNail(object param = null)
        {
            RadTreeNode selectedNode = this.radTreeView1.SelectedNode;
            if (((selectedNode != null) && (selectedNode.Tag != null)) && (selectedNode.Tag is NFileInfo))
            {
                NFileInfo tag = selectedNode.Tag as NFileInfo;
                selectedNode.SetImageIcon(tag.LocalPath, this._viewfileinfoicon);
            }
        }

        public void UploadBatch(object param = null)
        {
            NBatchInfoGroup group = this.FromTree();
            if ((group == null) || (group.Batchs.Count == 0))
            {
                LibCommon.AppContext.GetInstance().MS.LogWarning("没有可提交的数据");
            }
            else
            {
                FormUploadBatchs batchs = new FormUploadBatchs
                {
                    Group = group
                };
                if ((batchs.ShowDialog() == DialogResult.OK) && !AbstractSetting<FunctionSetting>.CurSetting.KeepSuccessedUploadNodeInTree)
                {
                    List<string> uploadSuccessedbatchs = batchs.UploadSuccessedbatchs;
                    for (int i = this.radTreeView1.Nodes.Count - 1; i >= 0; i--)
                    {
                        if (uploadSuccessedbatchs.Contains((this.radTreeView1.Nodes[i].Tag as NBatchInfo).BatchNO))
                        {
                            this.radTreeView1.Nodes.Remove(this.radTreeView1.Nodes[i]);
                        }
                    }
                }
                if (this.radTreeView1.Nodes.Count > 0)
                {
                    this.radTreeView1.Nodes[0].Selected = true;
                    this.NavigateFirstItem(null);
                }
                else
                {
                    this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(null));
                }
                this.radTreeView1.Focus();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Nested Types

    }

    public class NestIPropertiesSetting : IPropertiesSetting
    {
        // Fields
        private UCNavigatorBar _bar;

        // Methods
        public NestIPropertiesSetting(UCNavigatorBar bar)
        {
            this._bar = bar;
        }

        // Properties
        [Category("用户界面定义"), Description("允许自定义层次子节点")]
        public bool AllowCreateCategory
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "AllowCreateCategory").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "AllowCreateCategory", value.ToString());
            }
        }

        [Category("用户界面定义"), Description("允许移除菜单")]
        public bool AllowDelMenu
        {
            get
            {
                return LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "AllowDelMenu").ToBool();
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "AllowDelMenu", value.ToString());
            }
        }

        [Category("用户界面定义"), Description("批次号节点高度")]
        public int BatchNodeHeight
        {
            get
            {
                string configParamValue = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "BatchNodeHeight");
                if (string.IsNullOrEmpty(configParamValue))
                {
                    return 0x20;
                }
                return int.Parse(configParamValue);
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "BatchNodeHeight", value.ToString());
            }
        }

        [Category("用户界面定义"), Description("文件节点标题类型")]
        public ENFileNodeTitleType FileNodeTitleType
        {
            get
            {
                return NodeTitleTypeSetting.FileNodeTitleType;
            }
            set
            {
                NodeTitleTypeSetting.FileNodeTitleType = value;
            }
        }

        /*
        [Category("用户界面定义"), Description("节点字体大小")]
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
        }*/

        [Browsable(false)]
        public string Name
        {
            get
            {
                return "界面设置-左边文件管理导航栏";
            }
        }

        [Category("用户界面定义"), Description("缩略图尺寸")]
        public int ThumbImgSize
        {
            get
            {
                string configParamValue = LibCommon.AppContext.GetInstance().Config.GetConfigParamValue("LeftPaneSetting", "ThumbImgSize");
                if (string.IsNullOrEmpty(configParamValue))
                {
                    return 0x60;
                }
                return int.Parse(configParamValue);
            }
            set
            {
                LibCommon.AppContext.GetInstance().Config.SetConfigParamValue("LeftPaneSetting", "ThumbImgSize", value.ToString());
            }
        }
    }

    public enum ScanOpe
    {
        None,
        Add,
        AddToCur,
        ReplaceCurrent
    }
}

