using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DocScanner.Bean;
using DocScanner.LibCommon;

namespace DocScanner.AdapterFactory
{
    public class UCAdapterSetting : UserControl
	{

		private IContainer components = null;

		private ComboBox comboBox1;

		private Label label1;

		private Label label2;

		private PropertyGrid propertyGrid1;

		private Button btn_Ok;

		private Button btnCancel;

		public string Title
		{
			get
			{
				return "采集器参数设置";
			}
		}

		public SharpAcquirerFactory AdaptsFactory
		{
			get;
			set;
		}

		public UCAdapterSetting(SharpAcquirerFactory adpts)
		{
			this.InitializeComponent();
			this.AdaptsFactory = adpts;
            //this.comboBox1.DataSource = adpts.Acqs.Values.Select<IFileAcquirer>(o => new { o.Name, o.CnName }).ToList();
            this.comboBox1.DataSource = adpts.Acqs.Values.Select(o => new { o.Name, o.CnName }).ToList();

            this.comboBox1.DisplayMember = "CnName";
			this.comboBox1.ValueMember = "Name";
			string configParamValue = AppContext.GetInstance().Config.GetConfigParamValue("AdapterSetting", "DefaultAdapter");
			bool flag = !string.IsNullOrEmpty(configParamValue);
			if (flag)
			{
				this.comboBox1.SelectedValue = configParamValue;
			}
			bool flag2 = this.comboBox1.SelectedValue == null;
			if (flag2)
			{
				this.comboBox1.SelectedIndex = 0;
			}
			this.comboBox1.SelectedValueChanged += new EventHandler(this.comboBox1_SelectedValueChanged);
			this.comboBox1.Refresh();
			this.comboBox1_SelectedValueChanged(this, EventArgs.Empty);
		}

		private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
		{
			AppContext.GetInstance().Config.SetConfigParamValue("AdapterSetting", "DefaultAdapter", this.comboBox1.SelectedValue.ToString());
			bool flag = this.AdaptsFactory != null;
			if (flag)
			{
				IFileAcquirer adapter = this.AdaptsFactory.GetAdapter(this.comboBox1.SelectedValue.ToString());
				this.propertyGrid1.SelectedObject = adapter.GetSetting();
			}
		}

		private void btn_Ok_Click(object sender, EventArgs e)
		{
			bool flag = base.Parent is Form;
			if (flag)
			{
				(base.Parent as Form).DialogResult = DialogResult.OK;
				(base.Parent as Form).Close();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			bool flag = base.Parent is Form;
			if (flag)
			{
				(base.Parent as Form).DialogResult = DialogResult.Cancel;
				(base.Parent as Form).Close();
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
			this.comboBox1 = new ComboBox();
			this.label1 = new Label();
			this.label2 = new Label();
			this.propertyGrid1 = new PropertyGrid();
			this.btn_Ok = new Button();
			this.btnCancel = new Button();
			base.SuspendLayout();
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new Point(108, 16);
			this.comboBox1.Margin = new Padding(3, 2, 3, 2);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new Size(271, 23);
			this.comboBox1.TabIndex = 0;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(14, 20);
			this.label1.Name = "label1";
			this.label1.Size = new Size(82, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "当前采集器";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(14, 57);
			this.label2.Name = "label2";
			this.label2.Size = new Size(112, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "采集器参数设置";
			this.propertyGrid1.Location = new Point(14, 89);
			this.propertyGrid1.Margin = new Padding(3, 2, 3, 2);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new Size(521, 374);
			this.propertyGrid1.TabIndex = 3;
			this.btn_Ok.Location = new Point(290, 474);
			this.btn_Ok.Margin = new Padding(3, 2, 3, 2);
			this.btn_Ok.Name = "btn_Ok";
			this.btn_Ok.Size = new Size(81, 37);
			this.btn_Ok.TabIndex = 4;
			this.btn_Ok.Text = "确定";
			this.btn_Ok.UseVisualStyleBackColor = true;
			this.btn_Ok.Click += new EventHandler(this.btn_Ok_Click);
			this.btnCancel.Location = new Point(440, 474);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(81, 37);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			base.AutoScaleDimensions = new SizeF(8f, 15f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btn_Ok);
			base.Controls.Add(this.propertyGrid1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.comboBox1);
			base.Margin = new Padding(3, 2, 3, 2);
			base.Name = "UCAdapterSetting";
			base.Size = new Size(555, 514);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
