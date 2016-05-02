using DocScanner.LibCommon;
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
    public class UCAccountSetting : UserControl
    {
        private IContainer components = null;

        private Button btnOK;

        private Label label1;

        private Label label2;

        private TextBox textBoxOrgID;

        private TextBox textBoxAccount;

        public string Title
        {
            get
            {
                return "帐户设置";
            }
        }

        public UCAccountSetting()
        {
            this.InitializeComponent();
            this.textBoxOrgID.Text = AbstractSetting<AccountSetting>.CurSetting.AccountOrgID;
            this.textBoxAccount.Text = AbstractSetting<AccountSetting>.CurSetting.AccountName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AbstractSetting<AccountSetting>.CurSetting.AccountOrgID = this.textBoxOrgID.Text;
            AbstractSetting<AccountSetting>.CurSetting.AccountName = this.textBoxAccount.Text;
            (base.Parent as Form).Close();
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
            this.btnOK = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.textBoxOrgID = new TextBox();
            this.textBoxAccount = new TextBox();
            base.SuspendLayout();
            this.btnOK.Location = new Point(115, 199);
            this.btnOK.Margin = new Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(100, 29);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定&O";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(48, 51);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(53, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "机构ID";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(48, 116);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(53, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "用户ID";
            this.textBoxOrgID.Location = new Point(175, 48);
            this.textBoxOrgID.Margin = new Padding(4, 4, 4, 4);
            this.textBoxOrgID.Name = "textBoxOrgID";
            this.textBoxOrgID.Size = new Size(132, 25);
            this.textBoxOrgID.TabIndex = 3;
            this.textBoxAccount.Location = new Point(175, 105);
            this.textBoxAccount.Margin = new Padding(4, 4, 4, 4);
            this.textBoxAccount.Name = "textBoxAccount";
            this.textBoxAccount.Size = new Size(132, 25);
            this.textBoxAccount.TabIndex = 4;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.textBoxAccount);
            base.Controls.Add(this.textBoxOrgID);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnOK);
            base.Margin = new Padding(4, 4, 4, 4);
            base.Name = "UCAccountSetting";
            base.Size = new Size(335, 246);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
