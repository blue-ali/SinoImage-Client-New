using DocScanner.Network;
using DocScanner.Bean;
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
    public class UCQueryNBatchHis : UserControl
    {
        private INetTransfer _transfer;

        private IContainer components = null;

        private TableLayoutPanel tableLayoutPanel1;

        private RadTreeView treeView1;

        private TableLayoutPanel tableLayoutPanel2;

        private Panel panel1;

        private TextBox textBox_BatchNO;

        private Label label1;

        private Button btnQryBatchHis;

        public List<NBatchInfo> BatchInfos
        {
            get;
            set;
        }

        public string Title
        {
            get
            {
                return "批次历史信息";
            }
        }

        public UCQueryNBatchHis()
        {
            this.InitializeComponent();
        }

        private void btnQryBatchHis_Click(object sender, EventArgs e)
        {
            bool flag = string.IsNullOrEmpty(this.textBox_BatchNO.Text);
            if (!flag)
            {
                NBatchHisQry nBatchHisQry = new NBatchHisQry();
                nBatchHisQry.BatchNO = this.textBox_BatchNO.Text;
                this._transfer = INetTransferFactory.GetNetTransfer();
                this._transfer.OnNotify -= new EventHandler<TEventArg<NetTransferNotifyMsg>>(this._transfer_OnNotify);
                this._transfer.OnNotify -= new EventHandler<TEventArg<NetTransferNotifyMsg>>(this._transfer_OnNotify);
                this._transfer.GetBatchHis(nBatchHisQry);
            }
        }

        private void _transfer_OnNotify(object sender, TEventArg<NetTransferNotifyMsg> e)
        {
            bool flag = e.Arg.Status == ENetTransferStatus.AllDone;
            if (flag)
            {
                NBatchHisRsp batchHisAsyncResult = this._transfer.GetBatchHisAsyncResult();
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
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.treeView1 = new RadTreeView();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.panel1 = new Panel();
            this.btnQryBatchHis = new Button();
            this.textBox_BatchNO = new TextBox();
            this.label1 = new Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200f));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 850f));
            this.tableLayoutPanel1.Controls.Add(this.treeView1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel1.Size = new Size(1053, 676);
            this.tableLayoutPanel1.TabIndex = 0;
            this.treeView1.Dock = DockStyle.Fill;
            this.treeView1.Location = new Point(203, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new Size(847, 670);
            this.treeView1.TabIndex = 1;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Dock = DockStyle.Fill;
            this.tableLayoutPanel2.Location = new Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 200f));
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.Size = new Size(194, 670);
            this.tableLayoutPanel2.TabIndex = 0;
            this.panel1.Controls.Add(this.btnQryBatchHis);
            this.panel1.Controls.Add(this.textBox_BatchNO);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(188, 194);
            this.panel1.TabIndex = 0;
            this.btnQryBatchHis.Location = new Point(44, 140);
            this.btnQryBatchHis.Name = "btnQryBatchHis";
            this.btnQryBatchHis.Size = new Size(75, 23);
            this.btnQryBatchHis.TabIndex = 2;
            this.btnQryBatchHis.Text = "查询&Q";
            this.btnQryBatchHis.UseVisualStyleBackColor = true;
            this.btnQryBatchHis.Click += new EventHandler(this.btnQryBatchHis_Click);
            this.textBox_BatchNO.Location = new Point(16, 89);
            this.textBox_BatchNO.Name = "textBox1";
            this.textBox_BatchNO.Size = new Size(149, 25);
            this.textBox_BatchNO.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(3, 62);
            this.label1.Name = "label1";
            this.label1.Size = new Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "批次号";
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tableLayoutPanel1);
            base.Name = "UCQueryNBatchHis";
            base.Size = new Size(1053, 676);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}
