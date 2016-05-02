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
    public class UCAccountNew : UserControl
    {
        private IContainer components = null;

        private Button btnCreateAccount;

        private Label label1;

        private Label label2;

        private Label label3;

        private Label label4;

        private TextBox textBox_Account;

        private TextBox textBox_Pwd;

        private TextBox textBox_PwdC;

        private ComboBox comboBox_OrgID;

        private Button btnCancel;

        public string Title
        {
            get
            {
                return "新建账户";
            }
        }

        public UCAccountNew()
        {
            this.InitializeComponent();
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            bool flag = string.IsNullOrEmpty(this.textBox_Account.Text.Trim());
            if (flag)
            {
                this.textBox_Account.Focus();
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
            this.btnCreateAccount = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.textBox_Account = new TextBox();
            this.textBox_Pwd = new TextBox();
            this.textBox_PwdC = new TextBox();
            this.comboBox_OrgID = new ComboBox();
            this.btnCancel = new Button();
            base.SuspendLayout();
            this.btnCreateAccount.Location = new Point(252, 435);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new Size(170, 42);
            this.btnCreateAccount.TabIndex = 0;
            this.btnCreateAccount.Text = "创建&C";
            this.btnCreateAccount.UseVisualStyleBackColor = true;
            this.btnCreateAccount.Click += new EventHandler(this.btnCreateAccount_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(194, 70);
            this.label1.Name = "label1";
            this.label1.Size = new Size(52, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "账户名";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(179, 146);
            this.label2.Name = "label2";
            this.label2.Size = new Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "所属机构";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(209, 222);
            this.label3.Name = "label3";
            this.label3.Size = new Size(37, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "密码";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(149, 298);
            this.label4.Name = "label4";
            this.label4.Size = new Size(97, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "再次输入密码";
            this.textBox_Account.Location = new Point(343, 70);
            this.textBox_Account.Name = "textBox_Account";
            this.textBox_Account.Size = new Size(182, 25);
            this.textBox_Account.TabIndex = 6;
            this.textBox_Pwd.Location = new Point(343, 214);
            this.textBox_Pwd.Name = "textBox_Pwd";
            this.textBox_Pwd.Size = new Size(182, 25);
            this.textBox_Pwd.TabIndex = 8;
            this.textBox_PwdC.Location = new Point(343, 287);
            this.textBox_PwdC.Name = "textBox_PwdC";
            this.textBox_PwdC.Size = new Size(182, 25);
            this.textBox_PwdC.TabIndex = 9;
            this.comboBox_OrgID.FormattingEnabled = true;
            this.comboBox_OrgID.Location = new Point(343, 143);
            this.comboBox_OrgID.Name = "comboBox_OrgID";
            this.comboBox_OrgID.Size = new Size(182, 23);
            this.comboBox_OrgID.TabIndex = 10;
            this.btnCancel.Location = new Point(537, 435);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(170, 42);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.comboBox_OrgID);
            base.Controls.Add(this.textBox_PwdC);
            base.Controls.Add(this.textBox_Pwd);
            base.Controls.Add(this.textBox_Account);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnCreateAccount);
            base.Name = "UCAccountNew";
            base.Size = new Size(807, 568);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
