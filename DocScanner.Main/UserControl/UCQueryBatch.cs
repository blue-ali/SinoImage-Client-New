using DocScaner.Network;
using DocScanner.Bean;
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
    public class UCQueryBatch : UserControl
	{
		private NBatchInfoGroup _downloadgroup = new NBatchInfoGroup();

		private INetTransfer _transfer;

		private IContainer components = null;

		private Button btn_Download;

		private Button btn_Cancel;

		private TextBox textBox1;

		private Label label1;

		private Label label3;

		private RadWaitingBar radWaitingBar1;

		private TableLayoutPanel tableLayoutPanel1;

		private Panel panel1;

		private Panel panel2;

		private Panel panel3;

		private ListView listView1;

		private ColumnHeader columnHeader1;

		private ColumnHeader columnHeader2;

		public string Title
		{
			get
			{
				return "批次查询";
			}
		}

		public string CurrentBatchNo
		{
			get
			{
				return this.textBox1.Text;
			}
			set
			{
				this.textBox1.Text = value;
			}
		}

		public NBatchInfoGroup DownloadGroup
		{
			get
			{
				return this._downloadgroup;
			}
		}

		public UCQueryBatch()
		{
			this.InitializeComponent();
			this.Text = "查询批次";
			this.label3.Text = "";
			this.textBox1.Focus();
			UploadedBatchLogger localUploaded = UploadedBatchLogger.GetLocalUploaded();
			bool flag = localUploaded != null;
			if (flag)
			{
				foreach (UploadedBatchInfo current in localUploaded.BatchNos)
				{
					ListViewItem listViewItem = this.listView1.Items.Add(current.BatchNo);
					listViewItem.SubItems.Add(current.Time.ToLongDateString());
				}
			}
			this.listView1.MouseClick += new MouseEventHandler(this.ListView1_MouseClick);
		}

		private void ListView1_MouseClick(object sender, MouseEventArgs e)
		{
			bool flag = e.Button == MouseButtons.Right && this.listView1.SelectedItems.Count > 0;
			if (flag)
			{
				ContextMenu contextMenu = new ContextMenu();
				MenuItem menuItem = new MenuItem();
				menuItem.Text = "删除记录";
				menuItem.Click += delegate(object senderx, EventArgs ee)
				{
					UploadedBatchLogger.Del_Old(this.listView1.SelectedItems[0].Text);
					this.listView1.Items.RemoveAt(this.listView1.SelectedIndices[0]);
				};
				contextMenu.MenuItems.Add(menuItem);
				contextMenu.Show(this.listView1, e.Location);
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
					this.label3.Text = arg.Arg.Msg;
					this.radWaitingBar1.StartWaiting();
					this.SetUIStatus(false);
				}
				bool flag2 = arg.Arg.Status == ENetTransferStatus.Error;
				if (flag2)
				{
					this.radWaitingBar1.WaitingIndicatorWidth = 0;
					this.label3.Text = arg.Arg.Msg;
					this.label3.ForeColor = Color.Red;
					this.radWaitingBar1.EndWaiting();
					LibCommon.AppContext.Cur.MS.LogError("下载失败" + arg.Arg.Msg);
					this.SetUIStatus(true);
				}
				bool flag3 = arg.Arg.Status == ENetTransferStatus.OnProgress;
				if (flag3)
				{
					this.label3.Text = arg.Arg.Msg;
				}
				bool flag4 = arg.Arg.Status == ENetTransferStatus.Success;
				if (flag4)
				{
					this.radWaitingBar1.WaitingIndicatorWidth = 0;
					this.radWaitingBar1.EndWaiting();
					this.label3.Text = "下载成功" + arg.Arg.Msg;
					LibCommon.AppContext.Cur.MS.LogSuccess("下载成功");
					this._downloadgroup.Batchs.Add(this._transfer.GetDownloadBatchAysncResult());
					this.SetUIStatus(true);
				}
			}
		}

		public void btn_Download_Click(object sender, EventArgs e)
		{
			bool flag = string.IsNullOrEmpty(this.textBox1.Text);
			if (!flag)
			{
				this._transfer = INetTransferFactory.GetNetTransfer();
				this._transfer.OnNotify -= new EventHandler<TEventArg<NetTransferNotifyMsg>>(this.OnTransferNotify);
				this._transfer.OnNotify += new EventHandler<TEventArg<NetTransferNotifyMsg>>(this.OnTransferNotify);
				NQueryBatchInfo nQueryBatchInfo = new NQueryBatchInfo();
				nQueryBatchInfo.BatchNO = this.textBox1.Text;
				this._transfer.DownloadBatch(nQueryBatchInfo);
			}
		}

		public void btn_Cancel_Click(object sender, EventArgs e)
		{
			this.GetHostForm().DialogResult = DialogResult.OK;
			this.GetHostForm().Close();
		}

		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			bool flag = this.listView1.SelectedItems.Count > 0;
			if (flag)
			{
				this.textBox1.Text = this.listView1.SelectedItems[0].SubItems[0].Text;
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
			this.btn_Download = new Button();
			this.btn_Cancel = new Button();
			this.textBox1 = new TextBox();
			this.label1 = new Label();
			this.label3 = new Label();
			this.radWaitingBar1 = new RadWaitingBar();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.panel1 = new Panel();
			this.panel2 = new Panel();
			this.panel3 = new Panel();
			this.listView1 = new ListView();
			this.columnHeader1 = new ColumnHeader();
			this.columnHeader2 = new ColumnHeader();
			((ISupportInitialize)this.radWaitingBar1).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.btn_Download.Location = new Point(152, 146);
			this.btn_Download.Name = "btn_Download";
			this.btn_Download.Size = new Size(127, 38);
			this.btn_Download.TabIndex = 0;
			this.btn_Download.Text = "下载&D";
			this.btn_Download.UseVisualStyleBackColor = true;
			this.btn_Download.Click += new EventHandler(this.btn_Download_Click);
			this.btn_Cancel.Location = new Point(516, 146);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new Size(136, 38);
			this.btn_Cancel.TabIndex = 1;
			this.btn_Cancel.Text = "关闭&C";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
			this.textBox1.Location = new Point(92, 72);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(560, 25);
			this.textBox1.TabIndex = 2;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(34, 75);
			this.label1.Name = "label1";
			this.label1.Size = new Size(52, 15);
			this.label1.TabIndex = 3;
			this.label1.Text = "批次号";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(9, 154);
			this.label3.Margin = new Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new Size(55, 15);
			this.label3.TabIndex = 7;
			this.label3.Text = "label3";
			this.radWaitingBar1.BackColor = SystemColors.ControlLightLight;
			this.radWaitingBar1.Dock = DockStyle.Bottom;
			this.radWaitingBar1.Location = new Point(0, 433);
			this.radWaitingBar1.Margin = new Padding(4);
			this.radWaitingBar1.Name = "radWaitingBar1";
			this.radWaitingBar1.RootElement.ControlBounds = new Rectangle(0, 433, 150, 30);
			this.radWaitingBar1.RootElement.ToolTipText = null;
			this.radWaitingBar1.Size = new Size(812, 33);
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
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 229f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 205f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));
			this.tableLayoutPanel1.Size = new Size(812, 433);
			this.tableLayoutPanel1.TabIndex = 8;
			this.panel1.Controls.Add(this.label3);
			this.panel1.Dock = DockStyle.Fill;
			this.panel1.Location = new Point(3, 437);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(806, 44);
			this.panel1.TabIndex = 0;
			this.panel1.Visible = false;
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.textBox1);
			this.panel2.Controls.Add(this.btn_Cancel);
			this.panel2.Controls.Add(this.btn_Download);
			this.panel2.Dock = DockStyle.Fill;
			this.panel2.Location = new Point(3, 232);
			this.panel2.Name = "panel2";
			this.panel2.Size = new Size(806, 199);
			this.panel2.TabIndex = 1;
			this.panel3.Dock = DockStyle.Fill;
			this.panel3.Location = new Point(3, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new Size(806, 223);
			this.panel3.TabIndex = 2;
			this.listView1.BorderStyle = BorderStyle.FixedSingle;
			this.listView1.Columns.AddRange(new ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2
			});
			this.listView1.Dock = DockStyle.Top;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.Location = new Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new Size(812, 226);
			this.listView1.TabIndex = 4;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = View.Details;
			this.listView1.MouseDoubleClick += new MouseEventHandler(this.listView1_MouseDoubleClick);
			this.columnHeader1.Text = "批次号";
			this.columnHeader1.Width = 265;
			this.columnHeader2.Text = "时间";
			base.AutoScaleDimensions = new SizeF(8f, 15f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.listView1);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.radWaitingBar1);
			base.Name = "UCQueryBatch";
			base.Size = new Size(812, 466);
			((ISupportInitialize)this.radWaitingBar1).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
