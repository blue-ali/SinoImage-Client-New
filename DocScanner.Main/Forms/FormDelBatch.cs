using DocScaner.Network;
using DocScanner.Bean;
using DocScanner.Bean.pb;
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
    public class FormDelBatch : FormBase
    {
        private UCNavigatorBar _naparent;

        private INetTransfer _transfer;

        private IContainer components = null;

        private Button btn_Cancel;

        private TextBox txtBox_BatchNO;

        private Label label1;

        private Button btn_DelBatch;

        private Label label3;

        private RadWaitingBar radWaitingBar1;

        private TableLayoutPanel tableLayoutPanel1;

        private Panel panel1;

        private Panel panel2;

        private Panel panel3;

        private TabControl tabControl1;

        private TabPage tabPage_Local;

        private ListView listView1;

        private ColumnHeader columnHeader1;

        public string CurrentBatchNo
        {
            get
            {
                return this.txtBox_BatchNO.Text;
            }
            set
            {
                this.txtBox_BatchNO.Text = value;
            }
        }

        public UCNavigatorBar Naparent
        {
            get
            {
                return this._naparent;
            }
            set
            {
                this._naparent = value;
            }
        }

        public FormDelBatch(UCNavigatorBar p)
        {
            this.InitializeComponent();
            this.Text = "删除批次";
            this._naparent = p;
            this.label3.Text = "";
            UploadedBatchLogger localUploaded = UploadedBatchLogger.GetLocalUploaded();
            bool flag = localUploaded != null;
            if (flag)
            {
                foreach (UploadedBatchInfo current in localUploaded.BatchNos)
                {
                    this.listView1.Items.Add(current.BatchNo);
                }
            }
        }

        private void DeleteItemFromListView(string itemname)
        {
            foreach (ListViewItem listViewItem in this.listView1.Items)
            {
                bool flag = listViewItem.Text == itemname;
                if (flag)
                {
                    this.listView1.Items.Remove(listViewItem);
                }
            }
        }

        public void OnReportDelete(object sender, TEventArg<NetTransferNotifyMsg> arg)
        {
            bool invokeRequired = base.InvokeRequired;
            if (invokeRequired)
            {
                base.Invoke(new Action<object, TEventArg<NetTransferNotifyMsg>>(this.OnReportDelete), new object[]
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
                    this.label3.Text = arg.Arg.Msg;
                    this.radWaitingBar1.StartWaiting();
                    this.SetUIStatus(false);
                }
                bool flag2 = arg.Arg.Status == ENetTransferStatus.OnProgress;
                if (flag2)
                {
                    this.label3.Text = arg.Arg.Msg;
                }
                bool flag3 = arg.Arg.Status == ENetTransferStatus.Error;
                if (flag3)
                {
                    this.radWaitingBar1.WaitingIndicatorWidth = 0;
                    this.label3.Text = arg.Arg.Msg;
                    this.label3.ForeColor = Color.Red;
                    this.radWaitingBar1.EndWaiting();
                    this.SetUIStatus(true);
                }
                bool flag4 = arg.Arg.Status == ENetTransferStatus.Success;
                if (flag4)
                {
                    this.radWaitingBar1.WaitingIndicatorWidth = 0;
                    this.radWaitingBar1.EndWaiting();
                    this.label3.Text = "删除成功" + arg.Arg.Msg;
                    UploadedBatchLogger.Del_Old(this.txtBox_BatchNO.Text);
                    this.DeleteItemFromListView(this.txtBox_BatchNO.Text);
                    this.txtBox_BatchNO.Text = "";
                    this.SetUIStatus(true);
                }
            }
        }

        private void SetUIStatus(bool able)
        {
            foreach (Control control in base.Controls)
            {
                control.Enabled = able;
            }
            this.radWaitingBar1.Enabled = true;
        }

        public void btn_Cancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void btn_DelBatch_Click(object sender, EventArgs e)
        {
            bool flag = string.IsNullOrEmpty(this.txtBox_BatchNO.Text);
            if (!flag)
            {
                NBatchInfo nBatchInfo = new NBatchInfo();
                nBatchInfo.BatchNO = this.txtBox_BatchNO.Text;
                nBatchInfo.Operation = EOperType.eDEL;
                this._transfer = INetTransferFactory.GetNetTransfer();
                this._transfer.OnNotify -= this.OnReportDelete;
                this._transfer.OnNotify += this.OnReportDelete;
                this._transfer.UploadBatch(nBatchInfo);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bool flag = this.listView1.SelectedItems.Count > 0;
            if (flag)
            {
                this.txtBox_BatchNO.Text = this.listView1.SelectedItems[0].SubItems[0].Text;
            }
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
            this.btn_Cancel = new Button();
            this.txtBox_BatchNO = new TextBox();
            this.label1 = new Label();
            this.btn_DelBatch = new Button();
            this.label3 = new Label();
            this.radWaitingBar1 = new RadWaitingBar();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.panel3 = new Panel();
            this.tabControl1 = new TabControl();
            this.tabPage_Local = new TabPage();
            this.listView1 = new ListView();
            this.columnHeader1 = new ColumnHeader();
            ((ISupportInitialize)this.radWaitingBar1).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_Local.SuspendLayout();
            base.SuspendLayout();
            this.btn_Cancel.Location = new Point(516, 127);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(136, 38);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "关闭";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.txtBox_BatchNO.Location = new Point(92, 26);
            this.txtBox_BatchNO.Name = "txtBox_BatchNO";
            this.txtBox_BatchNO.Size = new Size(560, 25);
            this.txtBox_BatchNO.TabIndex = 2;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(29, 31);
            this.label1.Name = "label1";
            this.label1.Size = new Size(52, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "批次号";
            this.btn_DelBatch.Location = new Point(92, 127);
            this.btn_DelBatch.Name = "btn_DelBatch";
            this.btn_DelBatch.Size = new Size(136, 38);
            this.btn_DelBatch.TabIndex = 5;
            this.btn_DelBatch.Text = "删除批次";
            this.btn_DelBatch.UseVisualStyleBackColor = true;
            this.btn_DelBatch.Click += new EventHandler(this.btn_DelBatch_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(9, 154);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(55, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "label3";
            this.radWaitingBar1.BackColor = SystemColors.ControlLightLight;
            this.radWaitingBar1.Dock = DockStyle.Bottom;
            this.radWaitingBar1.Location = new Point(0, 391);
            this.radWaitingBar1.Margin = new Padding(4);
            this.radWaitingBar1.Name = "radWaitingBar1";
            this.radWaitingBar1.RootElement.ControlBounds = new Rectangle(0, 391, 150, 30);
            this.radWaitingBar1.RootElement.ToolTipText = null;
            this.radWaitingBar1.Size = new Size(734, 33);
            this.radWaitingBar1.TabIndex = 3;
            this.radWaitingBar1.Text = "radWaitingBar1";
            this.radWaitingBar1.WaitingIndicatorWidth = 0;
            this.radWaitingBar1.WaitingSpeed = 10;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 203f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 190f));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 91f));
            this.tableLayoutPanel1.Size = new Size(734, 391);
            this.tableLayoutPanel1.TabIndex = 8;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(3, 396);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(728, 85);
            this.panel1.TabIndex = 0;
            this.panel1.Visible = false;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtBox_BatchNO);
            this.panel2.Controls.Add(this.btn_Cancel);
            this.panel2.Controls.Add(this.btn_DelBatch);
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(3, 206);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(728, 184);
            this.panel2.TabIndex = 1;
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Dock = DockStyle.Fill;
            this.panel3.Location = new Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(728, 197);
            this.panel3.TabIndex = 2;
            this.tabControl1.Controls.Add(this.tabPage_Local);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(728, 197);
            this.tabControl1.TabIndex = 5;
            this.tabPage_Local.Controls.Add(this.listView1);
            this.tabPage_Local.Location = new Point(4, 25);
            this.tabPage_Local.Name = "tabPage_Local";
            this.tabPage_Local.Padding = new Padding(3);
            this.tabPage_Local.Size = new Size(720, 168);
            this.tabPage_Local.TabIndex = 0;
            this.tabPage_Local.Text = "本地已提交批次";
            this.tabPage_Local.UseVisualStyleBackColor = true;
            this.listView1.BorderStyle = BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new ColumnHeader[]
            {
                this.columnHeader1
            });
            this.listView1.Dock = DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(714, 162);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.listView1.MouseDoubleClick += new MouseEventHandler(this.listView1_MouseDoubleClick);
            this.columnHeader1.Text = "批次号";
            this.columnHeader1.Width = 265;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(734, 424);
            base.Controls.Add(this.tableLayoutPanel1);
            base.Controls.Add(this.radWaitingBar1);
            base.Margin = new Padding(3);
            base.Name = "FormDelBatch";
            this.Text = "批次查询";
            ((ISupportInitialize)this.radWaitingBar1).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Local.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
