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
    public class UCNavigatorBar : UserControl
    {
        private static readonly UCNavigatorBar instance = new UCNavigatorBar();

        public static UCNavigatorBar GetInstance()
        {
            return instance;
        }

        // Fields
        private IFileAcquirer _acq;
        public ScanOpe _lastScanOpeType;
        
        private IContainer components = null;
        private NavigateTree navigateTree;
        private RadTreeView radTreeView;

        // Events
        //public event EventHandler<TEventArg<RadTreeNode>> OnItemSelectChanged;

        

        // Methods
        private UCNavigatorBar()
        {
            this.InitializeComponent();
            this.navigateTree = NavigateTree.GetInstance();
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 27);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "NavigatorBar";
            this.Size = new System.Drawing.Size(235, 495);
            this.TabIndex = 1;
            //navigateTree.CheckBoxes = false;

            //navigateTree.ExpandAnimation = ExpandAnimation.Opacity;
            //navigateTree.AllowDragDrop = true;
            //navigateTree.AllowEdit = true;
            //navigateTree.TreeIndent = 35;
            //navigateTree.TreeViewElement.ExpandImage = Properties.Resources.expand.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            //navigateTree.TreeViewElement.HoveredExpandImage = Properties.Resources.expand.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            //navigateTree.TreeViewElement.CollapseImage = Properties.Resources.collapse.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            //navigateTree.TreeViewElement.HoveredCollapseImage = Properties.Resources.collapse.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            //navigateTree.LineStyle = TreeLineStyle.Dot;
            //navigateTree.LineColor = Color.DeepSkyBlue;
            //navigateTree.ItemHeight = this.GetSetting().ThumbImgSize;
            //navigateTree.ImageScalingSize = new Size(this.GetSetting().ThumbImgSize, this.GetSetting().ThumbImgSize);

            //navigateTree.AllowArbitraryItemHeight = true;
            //navigateTree.ShowItemToolTips = true;
            //navigateTree.SelectedNodeChanged += radTreeView1_SelectedNodeChanged;
            //navigateTree.Nodes.PropertyChanged += this.Nodes_PropertyChanged;
            //this.SetupBatchNodeMenu();
            //this.SetupCategoryNodeMenu();
            //this.SetupFileNodeMenu();
            //base.Load += new EventHandler(this.UCNavigatorBar_Load);
        }

        public NavigateTree GetNavigateTree()
        {
            return navigateTree;
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
            AppContext.GetInstance().MS.LogError(e.Arg);
        }


        /*public RadTreeNode AddFileNode(RadTreeNode batchnode, NFileInfo info)
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
        }*/

        /*private void BatchNode_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
        }*/

        

        

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
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
                NBatchInfo batchInfo = new NBatchInfo
                {
                    Operation = EOperType.eADD,
                    BatchNO = str
                };
                //RadTreeNode node = BatchTemplateDef.CreateRadTreeFromTemplate(navigateTree, BatchNoMaker.Cur.SelectedTemplate, str);
                navigateTree.FromBatchTemplate(BatchNoMaker.Cur.SelectedTemplate, batchInfo);
                //this.UpdateAllNodeMenu(node);
            }
        }

        public void DoScanBatchDoc()
        {
            this._acq = AppContext.GetInstance().GetVal<SharpAcquirerFactory>(typeof(SharpAcquirerFactory)).GetAdapter("");
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
            /*
            if (((navigateTree.Nodes.Count > 0) && (navigateTree.Nodes[navigateTree.Nodes.Count - 1].Nodes.Count > 0)) && (this._lastScanOpeType != ScanOpe.ReplaceCurrent))
            {
                RadTreeNode node = navigateTree.Nodes[navigateTree.Nodes.Count - 1];
                RadTreeNode input = node.Nodes[node.Nodes.Count - 1];
                if (this.OnItemSelectChanged != null)
                {
                    this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(input));
                }
            }*/
        }

        /*public void DoSelectNode(object info)
        {
            Tuple<string, string> data = info as Tuple<string, string>;
            if (data != null)
            {
                IEnumerable<bool> source = from o in navigateTree.Nodes select (o.Tag as NBatchInfo).BatchNO == data.Item1;
                if ((source != null) && (source.Count<bool>() > 0))
                {
                }
            }
        }*/

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
                //this.BatchToTreeView(group);
                navigateTree.FromBatch(group);
            }
        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            navigateTree = NavigateTree.GetInstance();
            radTreeView = navigateTree.GetRadTree();
            ((System.ComponentModel.ISupportInitialize)(radTreeView)).BeginInit();
            this.SuspendLayout();
  
            // 
            // UCNavigatorBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(navigateTree.GetRadTree());
            this.Name = "UCNavigatorBar";
            this.Size = new System.Drawing.Size(233, 634);
            ((System.ComponentModel.ISupportInitialize)(radTreeView)).EndInit();
            this.ResumeLayout(false);

        }
               

        protected virtual void OnFileAcquied(string FilePath)
        {
            NFileInfo fileInfo = new NFileInfo();
            fileInfo.SetDate(DateTime.Now);
            fileInfo.Author = AccountSetting.GetInstance().AccountName;
            fileInfo.LocalPath = FilePath;
            fileInfo.FileName = FileHelper.GetFileName(FilePath);
            fileInfo.Operation = EOperType.eADD;

            NBatchInfo batchInfo = null;
            RadTreeNode batchNode;
            if (this._lastScanOpeType == ScanOpe.Add)
            {

                if (navigateTree.GetRadTree().Nodes.Count == 0)
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
                    batchNode = navigateTree.CreateBatchNode(batchInfo);
}
                else if (navigateTree.GetRadTree().SelectedNode != null)
                {
                    batchNode = navigateTree.GetRadTree().SelectedNode.GetBatchNode();
                    batchInfo = batchNode.Tag as NBatchInfo;
                }
                else
                {
                    batchNode = navigateTree.GetRadTree().Nodes[navigateTree.GetRadTree().Nodes.Count - 1];
                    batchInfo = batchNode.Tag as NBatchInfo;
                }
                RadTreeNode fileNode = navigateTree.CreateFileNodeFromLocal(batchNode, fileInfo, batchInfo);
 
                batchNode.ExpandAll();
                navigateTree.UpdateBatchNodeTitle(batchNode);
                Application.DoEvents();
            }
            else if (this._lastScanOpeType == ScanOpe.ReplaceCurrent)
            {
                RadTreeNode selectedNode = navigateTree.SelectedNode.UpdateFileNode(fileInfo);
                //this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(selectedNode));
            }
            else if (this._lastScanOpeType == ScanOpe.AddToCur)
            {
                //TODO AddToCur?
                /*RadTreeNode node = navigateTree.SelectedNode;
                if (node.Tag is NFileInfo)
                {
                    node = node.Parent;
                }*/
                batchNode = navigateTree.SelectedNode;
                batchInfo = batchNode.Tag as NBatchInfo;
                //NFileInfo info4 = new NFileInfo();
                //info4.SetDate(DateTime.Now);
                //info4.LocalPath = FilePath;
                //info4.FileName = FileHelper.GetFileName(FilePath);
                //info4.Operation = EOperType.eADD;
                //RadTreeNode node5 = node.Nodes.Add(info4.DisplayName);
                //node.TextAlignment = ContentAlignment.MiddleCenter;
                //node5.SetImageIcon(info4.LocalPath, this._viewfileinfoicon);
                //node5.Tag = info4;
                //this.SetFileNodeDefualtProperty(node5);
                //node5.Selected = true;
                //node5.ToolTipText = info4.ToUITipString();
                //node.ExpandAll();

                RadTreeNode fileNode = navigateTree.CreateFileNodeFromLocal(batchNode, fileInfo, batchInfo);

                navigateTree.UpdateBatchNodeTitle(batchNode);
                Application.DoEvents();
            }
        }

        public void ScanBatchDoc(object param)
        {
            this._lastScanOpeType = ScanOpe.Add;
            this.DoScanBatchDoc();
        }

        /*
        private RadTreeNode SetBatchNodeDefaultProperty(RadTreeNode node)
        {
            if (node != null)
            {
                node.AllowDrop = false;
                node.Checked = true;
                node.Checked = true;
                node.ItemHeight = UISetting.GetInstance().BatchNodeHeight;
                node.Font = new Font(this.Font.Name, navigateTree.Lev1NodeFontSize);
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
            node.ItemHeight = UISetting.GetInstance().ThumbImgSize;
            node.Checked = true;
            node.TextAlignment = ContentAlignment.MiddleCenter;
            node.PropertyChanged += new PropertyChangedEventHandler(this.FileNode_PropertyChanged);
            return node;
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
        }*/

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
                navigateTree.FromBatch(group);
            }
        }

        /*
        private void UpdateAllNodeMenu(RadTreeNode node)
        {
            this.SetBatchNodeDefaultProperty(node);
            List<RadTreeNode> children = node.GetChildren();
            foreach (RadTreeNode node2 in children)
            {
                this.SetupNodeDefaultProperty(node2);
            }
        }
        */
        /*
        public void UpdateThumbNail(object param = null)
        {
            RadTreeNode selectedNode = navigateTree.SelectedNode;
            if (((selectedNode != null) && (selectedNode.Tag != null)) && (selectedNode.Tag is NFileInfo))
            {
                NFileInfo tag = selectedNode.Tag as NFileInfo;
                selectedNode.SetImageIcon(tag.LocalPath, this._viewfileinfoicon);
            }
        }*/

        public void UploadBatch(object param = null)
        {
            NBatchInfoGroup group = navigateTree.ToBatch();
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
                if ((batchs.ShowDialog() == DialogResult.OK) && !FunctionSetting.GetInstance().KeepSuccessedUploadNodeInTree)
               // if ((batchs.ShowDialog() == DialogResult.OK) && !FunctionSetting.KeepSuccessedUploadNodeInTree)
                {
                    List<string> uploadSuccessedbatchs = batchs.UploadSuccessedbatchs;
                    //for (int i = navigateTree.Nodes.Count - 1; i >= 0; i--)
                    //{
                    //    if (uploadSuccessedbatchs.Contains((navigateTree.Nodes[i].Tag as NBatchInfo).BatchNO))
                    //    {
                    //        navigateTree.Nodes.Remove(navigateTree.Nodes[i]);
                    //    }
                    //}
                    foreach(RadTreeNode batchNode in navigateTree.GetRadTree().Nodes)
                    {
                        if (uploadSuccessedbatchs.Contains((batchNode.Tag as NBatchInfo).BatchNO))
                        {
                            navigateTree.GetRadTree().Nodes.Remove(batchNode);
                        }
                    }
                }
                if (navigateTree.GetRadTree().Nodes.Count > 0)
                {
                    navigateTree.GetRadTree().Nodes[0].Selected = true;
                    navigateTree.NavigateFirstItem(null);
                }
                else
                {
                    //this.OnItemSelectChanged(this, new TEventArg<RadTreeNode>(null));
                }
                navigateTree.GetRadTree().Focus();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Nested Types

    }

    public enum ScanOpe
    {
        None,
        Add,
        AddToCur,
        ReplaceCurrent
    }
}

