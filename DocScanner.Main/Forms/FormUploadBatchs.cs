using DocScaner.Network;
using DocScanner.Bean;
using DocScanner.Common;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class FormUploadBatchs : FormBase
    {
        private NBatchInfoGroup _group;

        private List<string> _successedBatchs = new List<string>();

        private List<string> _uploadSuccessedbatchs = new List<string>();

        private INetTransfer _transfer;

        private IContainer components = null;

        private Button btn_Upload;

        private Button btn_Close;

        private RadWaitingBar radWaitingBar1;

        private ListView listView1;

        private ColumnHeader columnHeader1;

        private Label labelStatus;

        private ColumnHeader columnHeader2;

        private ColumnHeader columnHeader3;

        private Button btnShowEx;

        public NBatchInfoGroup Group
        {
            get
            {
                return this._group;
            }
            set
            {
                this._group = value;
                this.ShowIntoList(this._group);
            }
        }

        public List<string> SuccessedUploadBatchs
        {
            get
            {
                return this._successedBatchs;
            }
        }

        public List<string> UploadSuccessedbatchs
        {
            get
            {
                return this._uploadSuccessedbatchs;
            }
        }

        public FormUploadBatchs()
        {
            this.InitializeComponent();
            base.DialogResult = DialogResult.Cancel;
            this.labelStatus.Text = "";
        }

        private void ShowIntoList(NBatchInfoGroup group)
        {
            this.listView1.Items.Clear();
            bool flag = group != null;
            if (flag)
            {
                foreach (NBatchInfo current in group.Batchs)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Tag = current;
                    listViewItem.Checked = true;
                    listViewItem.Text = current.DisplayName;
                    listViewItem.SubItems.Add("");
                    this.listView1.Items.Add(listViewItem);
                }
            }
        }

        private void SetUIStatus(bool abled)
        {
            foreach (Control control in base.Controls)
            {
                control.Enabled = abled;
            }
            this.radWaitingBar1.Enabled = true;
        }

        public void OnTransferNotify(object sender, TEventArg<NetTransferNotifyMsg> arg)
        {
            bool invokeRequired = base.InvokeRequired;
            if (invokeRequired)
            {
                base.Invoke(new Action<object, TEventArg<NetTransferNotifyMsg>>(this.OnTransferNotify), new object[]
                {
                    sender,
                    arg
                });
            }
            else
            {
                bool flag = arg.Arg.Status == ENetTransferStatus.Start;
                if (flag)
                {
                    this.radWaitingBar1.WaitingIndicatorWidth = 10;
                    this.labelStatus.Text = arg.Arg.Msg;
                    this.radWaitingBar1.StartWaiting();
                    this.SetUIStatus(false);
                    ListViewItem listViewItem = this.FindItem(arg.Arg.Msg);
                    bool flag2 = listViewItem != null;
                    if (flag2)
                    {
                        listViewItem.SubItems[1].Text = arg.Arg.Msg;
                    }
                }
                bool flag3 = arg.Arg.Status == ENetTransferStatus.Error;
                if (flag3)
                {
                    this.radWaitingBar1.WaitingIndicatorWidth = 0;
                    this.labelStatus.Text = arg.Arg.Msg;
                    this.labelStatus.ForeColor = Color.Red;
                    ListViewItem listViewItem2 = this.FindItem(arg.Arg.CurBatchNO);
                    bool flag4 = listViewItem2 != null;
                    if (flag4)
                    {
                        listViewItem2.SubItems[1].Text = "失败" + arg.Arg.Msg;
                    }
                    this.radWaitingBar1.EndWaiting();
                    this.SetUIStatus(true);
                }
                bool flag5 = arg.Arg.Status == ENetTransferStatus.OnProgress;
                if (flag5)
                {
                    this.labelStatus.Text = arg.Arg.Msg;
                }
                bool flag6 = arg.Arg.Status == ENetTransferStatus.Success;
                if (flag6)
                {
                    this.radWaitingBar1.WaitingIndicatorWidth = 0;
                    this.radWaitingBar1.EndWaiting();
                    this.labelStatus.Text = "提交批次成功" + arg.Arg.CurBatchNO;
                    this.SetUIStatus(true);
                    this._uploadSuccessedbatchs.Add(arg.Arg.CurBatchNO);
                    UploadedBatchLogger.Add_New(new UploadedBatchInfo(arg.Arg.CurBatchNO));
                    ListViewItem listViewItem3 = this.FindItem(arg.Arg.Msg);
                    bool flag7 = listViewItem3 != null;
                    if (flag7)
                    {
                        listViewItem3.SubItems[1].Text = "成功";
                    }
                }
            }
        }

        private ListViewItem FindItem(string batchno)
        {
            ListViewItem result;
            foreach (ListViewItem listViewItem in this.listView1.Items)
            {
                bool flag = (listViewItem.Tag as NBatchInfo).BatchNO == batchno;
                if (flag)
                {
                    result = listViewItem;
                    return result;
                }
            }
            result = null;
            return result;
        }

        private void btn_UploadBatch_Click(object sender, EventArgs e)
        {
            bool flag = false;
            NBatchInfoGroup nBatchInfoGroup = new NBatchInfoGroup();
            foreach (ListViewItem listViewItem in this.listView1.Items)
            {
                bool flag2 = listViewItem.Checked && listViewItem.SubItems[1].Text != "成功";
                if (flag2)
                {
                    NBatchInfo nBatchInfo = listViewItem.Tag as NBatchInfo;
                    nBatchInfo.SetupBatchInfo();
                    nBatchInfoGroup.Batchs.Add(nBatchInfo);
                    flag = true;
                }
            }
            bool flag3 = !flag;
            if (flag3)
            {
                MessageBox.Show("请勾选影像批次");
            }
            else
            {
                foreach (NBatchInfo current in nBatchInfoGroup.Batchs)
                {
                    this._transfer = INetTransferFactory.GetNetTransfer();
                    this._transfer.OnNotify -= new EventHandler<TEventArg<NetTransferNotifyMsg>>(this.OnTransferNotify);
                    this._transfer.OnNotify += new EventHandler<TEventArg<NetTransferNotifyMsg>>(this.OnTransferNotify);
                    NResultInfo nResultInfo = this._transfer.UploadBatch(current);
                }
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btn_Upload = new Button();
            this.btn_Close = new Button();
            this.radWaitingBar1 = new RadWaitingBar();
            this.listView1 = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.labelStatus = new Label();
            this.btnShowEx = new Button();
            ((ISupportInitialize)this.radWaitingBar1).BeginInit();
            base.SuspendLayout();
            this.btn_Upload.Location = new Point(248, 318);
            this.btn_Upload.Margin = new Padding(2);
            this.btn_Upload.Name = "btn_Upload";
            this.btn_Upload.Size = new Size(87, 32);
            this.btn_Upload.TabIndex = 0;
            this.btn_Upload.Text = "提交&S";
            this.btn_Upload.UseVisualStyleBackColor = true;
            this.btn_Upload.Click += new EventHandler(this.btn_UploadBatch_Click);
            this.btn_Close.Location = new Point(413, 318);
            this.btn_Close.Margin = new Padding(2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new Size(87, 32);
            this.btn_Close.TabIndex = 1;
            this.btn_Close.Text = "关闭&C";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new EventHandler(this.btn_Close_Click);
            this.radWaitingBar1.BackColor = SystemColors.ControlLightLight;
            this.radWaitingBar1.Dock = DockStyle.Bottom;
            this.radWaitingBar1.Location = new Point(0, 458);
            this.radWaitingBar1.Margin = new Padding(2);
            this.radWaitingBar1.Name = "progressBar1";
            this.radWaitingBar1.RootElement.ControlBounds = new Rectangle(0, 458, 150, 30);
            this.radWaitingBar1.RootElement.ToolTipText = null;
            this.radWaitingBar1.Size = new Size(695, 28);
            this.radWaitingBar1.TabIndex = 3;
            this.radWaitingBar1.Text = "radWaitingBar1";
            this.radWaitingBar1.WaitingIndicatorWidth = 0;
            this.radWaitingBar1.WaitingSpeed = 10;
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new ColumnHeader[]
            {
                this.columnHeader1,
                this.columnHeader2,
                this.columnHeader3
            });
            this.listView1.Dock = DockStyle.Top;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(695, 235);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.columnHeader1.Text = "批次";
            this.columnHeader1.Width = 248;
            this.columnHeader2.Text = "状态";
            this.columnHeader2.Width = 220;
            this.columnHeader3.Text = "时间";
            this.columnHeader3.Width = 220;
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new Point(12, 373);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new Size(55, 15);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "label1";
            this.btnShowEx.Location = new Point(651, 318);
            this.btnShowEx.Name = "btnShowEx";
            this.btnShowEx.Size = new Size(32, 23);
            this.btnShowEx.TabIndex = 6;
            this.btnShowEx.Text = ">>";
            this.btnShowEx.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(695, 486);
            base.Controls.Add(this.btnShowEx);
            base.Controls.Add(this.labelStatus);
            base.Controls.Add(this.listView1);
            base.Controls.Add(this.radWaitingBar1);
            base.Controls.Add(this.btn_Close);
            base.Controls.Add(this.btn_Upload);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Margin = new Padding(3, 2, 3, 2);
            base.Name = "FormUploadBatchs";
            this.Text = "批次提交";
            ((ISupportInitialize)this.radWaitingBar1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
