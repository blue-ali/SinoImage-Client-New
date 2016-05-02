using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class UCInsuranceBatchNOMaker : UserControl
    {
        private IContainer components = null;

        private TextBox textBox_CustNO;

        private Label label3;

        private Label label2;

        private Label label1;

        private Button btn_Confirm;

        private ComboBox comboBox_Dep;

        private ComboBox comboBox_BusiType;

        public string BatchNO
        {
            get;
            set;
        }

        public string Title
        {
            get
            {
                return "新建批次号";
            }
        }

        public UCInsuranceBatchNOMaker()
        {
            this.InitializeComponent();
            this.comboBox_BusiType.Items.AddRange(InsuranceData.BusinessType.ToArray());
            this.comboBox_Dep.Items.AddRange(InsuranceData.Departs.ToArray());
            this.comboBox_BusiType.SelectedIndex = 0;
            this.comboBox_Dep.SelectedIndex = 0;
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            bool flag = this.comboBox_BusiType.SelectedIndex == -1 || this.comboBox_Dep.SelectedIndex == -1 || string.IsNullOrEmpty(this.textBox_CustNO.Text);
            if (!flag)
            {
                this.BatchNO = this.comboBox_BusiType.Text + this.comboBox_Dep.Text + this.textBox_CustNO.Text;
                Form form = base.Parent as Form;
                bool flag2 = form != null;
                if (flag2)
                {
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                }
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
            this.textBox_CustNO = new TextBox();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.btn_Confirm = new Button();
            this.comboBox_Dep = new ComboBox();
            this.comboBox_BusiType = new ComboBox();
            base.SuspendLayout();
            this.textBox_CustNO.Location = new Point(177, 208);
            this.textBox_CustNO.Margin = new Padding(4, 5, 4, 5);
            this.textBox_CustNO.Name = "textBox_CustNO";
            this.textBox_CustNO.Size = new Size(310, 26);
            this.textBox_CustNO.TabIndex = 13;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(46, 208);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(57, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "客户号";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(62, 135);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(41, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "网点";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(46, 74);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(73, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "业务系统";
            this.btn_Confirm.Location = new Point(201, 280);
            this.btn_Confirm.Margin = new Padding(4, 5, 4, 5);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new Size(152, 66);
            this.btn_Confirm.TabIndex = 9;
            this.btn_Confirm.Text = "确定";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            this.btn_Confirm.Click += new EventHandler(this.btn_Confirm_Click);
            this.comboBox_Dep.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_Dep.FormattingEnabled = true;
            this.comboBox_Dep.Location = new Point(177, 135);
            this.comboBox_Dep.Margin = new Padding(4, 5, 4, 5);
            this.comboBox_Dep.Name = "comboBox_Dep";
            this.comboBox_Dep.Size = new Size(310, 28);
            this.comboBox_Dep.TabIndex = 8;
            this.comboBox_BusiType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_BusiType.FormattingEnabled = true;
            this.comboBox_BusiType.Location = new Point(177, 74);
            this.comboBox_BusiType.Margin = new Padding(4, 5, 4, 5);
            this.comboBox_BusiType.Name = "comboBox_BusiType";
            this.comboBox_BusiType.Size = new Size(310, 28);
            this.comboBox_BusiType.TabIndex = 7;
            base.AutoScaleDimensions = new SizeF(9f, 20f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.textBox_CustNO);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btn_Confirm);
            base.Controls.Add(this.comboBox_Dep);
            base.Controls.Add(this.comboBox_BusiType);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "UCInsuranceBatchNOMaker";
            base.Size = new Size(569, 380);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
