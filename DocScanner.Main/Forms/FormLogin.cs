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
    public class FormLogin : Form
    {
        private IContainer components = null;

        private TextBox textBox_CustNO;

        private Label label3;

        private Label label2;

        private Label label1;

        private Button btn_Confirm;

        private ComboBox comboBox_Dep;

        private ComboBox comboBox_BusiType;

        private Label label4;

        private TextBox textBox1;

        public FormLogin()
        {
            this.InitializeComponent();
            this.Text = "登录";
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
            this.label4 = new Label();
            this.textBox1 = new TextBox();
            base.SuspendLayout();
            this.textBox_CustNO.Location = new Point(179, 187);
            this.textBox_CustNO.Margin = new Padding(4);
            this.textBox_CustNO.Name = "textBox_CustNO";
            this.textBox_CustNO.Size = new Size(276, 22);
            this.textBox_CustNO.TabIndex = 20;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(63, 187);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(50, 17);
            this.label3.TabIndex = 19;
            this.label3.Text = "客户号";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(77, 129);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(36, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "网点";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(63, 80);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(64, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "业务系统";
            this.btn_Confirm.Location = new Point(206, 300);
            this.btn_Confirm.Margin = new Padding(4);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new Size(135, 53);
            this.btn_Confirm.TabIndex = 16;
            this.btn_Confirm.Text = "确定";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            this.comboBox_Dep.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_Dep.FormattingEnabled = true;
            this.comboBox_Dep.Location = new Point(179, 129);
            this.comboBox_Dep.Margin = new Padding(4);
            this.comboBox_Dep.Name = "comboBox_Dep";
            this.comboBox_Dep.Size = new Size(276, 24);
            this.comboBox_Dep.TabIndex = 15;
            this.comboBox_BusiType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_BusiType.FormattingEnabled = true;
            this.comboBox_BusiType.Location = new Point(179, 80);
            this.comboBox_BusiType.Margin = new Padding(4);
            this.comboBox_BusiType.Name = "comboBox_BusiType";
            this.comboBox_BusiType.Size = new Size(276, 24);
            this.comboBox_BusiType.TabIndex = 14;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(77, 238);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(36, 17);
            this.label4.TabIndex = 21;
            this.label4.Text = "密码";
            this.textBox1.Location = new Point(179, 233);
            this.textBox1.Margin = new Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(276, 22);
            this.textBox1.TabIndex = 22;
            base.AutoScaleDimensions = new SizeF(8f, 16f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(518, 379);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.textBox_CustNO);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btn_Confirm);
            base.Controls.Add(this.comboBox_Dep);
            base.Controls.Add(this.comboBox_BusiType);
            base.Name = "FormLogin";
            this.Text = "FormLogin";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
