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
    public class UCSignatureInfoRecord : UserControl
    {
        private IContainer components = null;

        private TabPage tabPage2;

        private TabPage tabPage1;

        private Button btn_Save;

        private PictureBox pictureBox1;

        private TabControl tabControl1;

        private Button btn_Cancel;

        private TextBox txtbox_Name;

        private TextBox txtBox_IDNO;

        private TextBox textBox1;

        private Label label4;

        private Label label3;

        private Label label1;

        private TextBox textBox_Comment;

        private Label label5;

        public Image ClipBitmap
        {
            set
            {
                this.pictureBox1.Image = value;
            }
        }

        public string Title
        {
            get
            {
                return "签名信息";
            }
        }

        public UCSignatureInfoRecord()
        {
            this.InitializeComponent();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            InsureUserInfo insureUserInfo = new InsureUserInfo();
            insureUserInfo.Name = this.txtbox_Name.Text;
            insureUserInfo.IDNO = this.txtBox_IDNO.Text;
            insureUserInfo.SignatureImgName = "sig" + DateTime.Now.ToString("HHmmssffff");
            this.pictureBox1.Image.Save(LibCommon.AppContext.Cur.GetVal<AppSetting>(typeof(AppSetting)).TmpFileDir + insureUserInfo.SignatureImgName);
            insureUserInfo.Comment = this.textBox_Comment.Text;
            InsureUserInfoMgr.Instance.AddUser(insureUserInfo);
            MessageBox.Show("保存成功");
            Form form = base.Parent as Form;
            bool flag = form != null;
            if (flag)
            {
                form.Close();
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Form form = base.Parent as Form;
            bool flag = form != null;
            if (flag)
            {
                form.Close();
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
            this.tabPage2 = new TabPage();
            this.tabPage1 = new TabPage();
            this.textBox_Comment = new TextBox();
            this.label5 = new Label();
            this.btn_Cancel = new Button();
            this.txtbox_Name = new TextBox();
            this.txtBox_IDNO = new TextBox();
            this.textBox1 = new TextBox();
            this.label4 = new Label();
            this.label3 = new Label();
            this.btn_Save = new Button();
            this.pictureBox1 = new PictureBox();
            this.label1 = new Label();
            this.tabControl1 = new TabControl();
            this.tabPage1.SuspendLayout();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            this.tabControl1.SuspendLayout();
            base.SuspendLayout();
            this.tabPage2.Location = new Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(875, 509);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage1.Controls.Add(this.textBox_Comment);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.btn_Cancel);
            this.tabPage1.Controls.Add(this.txtbox_Name);
            this.tabPage1.Controls.Add(this.txtBox_IDNO);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btn_Save);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3, 3, 3, 3);
            this.tabPage1.Size = new Size(1277, 697);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "信息录入";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.textBox_Comment.Location = new Point(153, 439);
            this.textBox_Comment.Multiline = true;
            this.textBox_Comment.Name = "textBox_Comment";
            this.textBox_Comment.Size = new Size(480, 118);
            this.textBox_Comment.TabIndex = 21;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(54, 450);
            this.label5.Margin = new Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(41, 20);
            this.label5.TabIndex = 20;
            this.label5.Text = "批注";
            this.btn_Cancel.Location = new Point(999, 621);
            this.btn_Cancel.Margin = new Padding(4, 5, 4, 5);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(129, 49);
            this.btn_Cancel.TabIndex = 17;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.txtbox_Name.Location = new Point(153, 198);
            this.txtbox_Name.Name = "txtbox_Name";
            this.txtbox_Name.Size = new Size(150, 26);
            this.txtbox_Name.TabIndex = 16;
            this.txtBox_IDNO.Location = new Point(153, 297);
            this.txtBox_IDNO.Name = "txtBox_IDNO";
            this.txtBox_IDNO.Size = new Size(364, 26);
            this.txtBox_IDNO.TabIndex = 15;
            this.textBox1.Location = new Point(153, 92);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(112, 26);
            this.textBox1.TabIndex = 10;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(39, 302);
            this.label4.Name = "label4";
            this.label4.Size = new Size(89, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "身份证编号";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(54, 202);
            this.label3.Name = "label3";
            this.label3.Size = new Size(73, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "客户姓名";
            this.btn_Save.Location = new Point(741, 621);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new Size(129, 49);
            this.btn_Save.TabIndex = 12;
            this.btn_Save.Text = "保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new EventHandler(this.btn_Save_Click);
            this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBox1.Location = new Point(724, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(524, 551);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(54, 97);
            this.label1.Name = "label1";
            this.label1.Size = new Size(73, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "签名编号";
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(1285, 730);
            this.tabControl1.TabIndex = 9;
            base.AutoScaleDimensions = new SizeF(9f, 20f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.tabControl1);
            base.Name = "UCSignatureInfoRecord";
            base.Size = new Size(1285, 730);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((ISupportInitialize)this.pictureBox1).EndInit();
            this.tabControl1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}
