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
    public class FormSysSetting : FormBase
    {
        private IContainer components = null;

        private Label label1;

        private Label label2;

        private Label label3;

        private Label label4;

        private Button btn_Parse;

        private Button btn_GenBatchNo;

        private TextBox txt_url;

        private TextBox txt_ImageType;

        private TextBox txt_Depart;

        private TextBox txt_Usr;

        private Button btn_OK;

        private Button btn_Cancel;

        private Label label5;

        private TextBox textBox2;

        private Label label6;

        private TextBox textBox5;

        private Label label7;

        private TextBox txt_BatchNO;

        public FormSysSetting()
        {
            this.InitializeComponent();
            this.Text += "(测试)";
            this.txt_Depart.DataBindings.Add("text", LibCommon.AppContext.GetInstance().GetVal<AccountSetting>(typeof(AccountSetting)), "AccountOrgID").DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            this.txt_Usr.DataBindings.Add("text", LibCommon.AppContext.GetInstance().GetVal<AccountSetting>(typeof(AccountSetting)), "AccountName").DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void btn_Parse_Click(object sender, EventArgs e)
        {
            string text = this.txt_url.Text;
        }

        private void btn_GenBatchNo_Click(object sender, EventArgs e)
        {
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.btn_Parse = new Button();
            this.btn_GenBatchNo = new Button();
            this.txt_url = new TextBox();
            this.txt_ImageType = new TextBox();
            this.txt_Depart = new TextBox();
            this.txt_Usr = new TextBox();
            this.btn_OK = new Button();
            this.btn_Cancel = new Button();
            this.label5 = new Label();
            this.textBox2 = new TextBox();
            this.label6 = new Label();
            this.textBox5 = new TextBox();
            this.label7 = new Label();
            this.txt_BatchNO = new TextBox();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(53, 98);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(64, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "影像类型";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(69, 151);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(50, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "机构号";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(69, 204);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(50, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "柜员号";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(37, 46);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(78, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "输入字符串";
            this.btn_Parse.Location = new Point(679, 38);
            this.btn_Parse.Margin = new Padding(4, 4, 4, 4);
            this.btn_Parse.Name = "btn_Parse";
            this.btn_Parse.Size = new Size(100, 28);
            this.btn_Parse.TabIndex = 4;
            this.btn_Parse.Text = "解析";
            this.btn_Parse.UseVisualStyleBackColor = true;
            this.btn_Parse.Click += new EventHandler(this.btn_Parse_Click);
            this.btn_GenBatchNo.Location = new Point(26, 363);
            this.btn_GenBatchNo.Margin = new Padding(4, 4, 4, 4);
            this.btn_GenBatchNo.Name = "btn_GenBatchNo";
            this.btn_GenBatchNo.Size = new Size(100, 28);
            this.btn_GenBatchNo.TabIndex = 5;
            this.btn_GenBatchNo.Text = "生成批次号（测试)";
            this.btn_GenBatchNo.UseVisualStyleBackColor = true;
            this.btn_GenBatchNo.Click += new EventHandler(this.btn_GenBatchNo_Click);
            this.txt_url.Location = new Point(153, 41);
            this.txt_url.Margin = new Padding(4, 4, 4, 4);
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new Size(508, 22);
            this.txt_url.TabIndex = 6;
            this.txt_ImageType.Location = new Point(153, 94);
            this.txt_ImageType.Margin = new Padding(4, 4, 4, 4);
            this.txt_ImageType.Name = "txt_ImageType";
            this.txt_ImageType.Size = new Size(132, 22);
            this.txt_ImageType.TabIndex = 7;
            this.txt_ImageType.Text = "TEST";
            this.txt_Depart.Location = new Point(153, 146);
            this.txt_Depart.Margin = new Padding(4, 4, 4, 4);
            this.txt_Depart.Name = "txt_Depart";
            this.txt_Depart.Size = new Size(132, 22);
            this.txt_Depart.TabIndex = 8;
            this.txt_Usr.Location = new Point(153, 199);
            this.txt_Usr.Margin = new Padding(4, 4, 4, 4);
            this.txt_Usr.Name = "txt_Usr";
            this.txt_Usr.Size = new Size(132, 22);
            this.txt_Usr.TabIndex = 9;
            this.btn_OK.Location = new Point(314, 457);
            this.btn_OK.Margin = new Padding(4, 4, 4, 4);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new Size(100, 28);
            this.btn_OK.TabIndex = 10;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
            this.btn_Cancel.Location = new Point(532, 468);
            this.btn_Cancel.Margin = new Padding(4, 4, 4, 4);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new Size(100, 28);
            this.btn_Cancel.TabIndex = 11;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(53, 257);
            this.label5.Margin = new Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(64, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "同步类型";
            this.textBox2.Location = new Point(153, 252);
            this.textBox2.Margin = new Padding(4, 4, 4, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(132, 22);
            this.textBox2.TabIndex = 13;
            this.textBox2.Text = "sync";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(45, 310);
            this.label6.Margin = new Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(80, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "EncryptKey";
            this.textBox5.Location = new Point(153, 305);
            this.textBox5.Margin = new Padding(4, 4, 4, 4);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Size(132, 22);
            this.textBox5.TabIndex = 15;
            this.textBox5.Text = "00000000";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(353, 310);
            this.label7.Margin = new Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new Size(221, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "参数[EncryptKey]长度为0位或8位\"";
            this.txt_BatchNO.Location = new Point(153, 364);
            this.txt_BatchNO.Margin = new Padding(4, 4, 4, 4);
            this.txt_BatchNO.Name = "txt_BatchNO";
            this.txt_BatchNO.Size = new Size(527, 22);
            this.txt_BatchNO.TabIndex = 17;
            base.AutoScaleDimensions = new SizeF(8f, 16f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(795, 531);
            base.Controls.Add(this.txt_BatchNO);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.textBox5);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.textBox2);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.btn_Cancel);
            base.Controls.Add(this.btn_OK);
            base.Controls.Add(this.txt_Usr);
            base.Controls.Add(this.txt_Depart);
            base.Controls.Add(this.txt_ImageType);
            base.Controls.Add(this.txt_url);
            base.Controls.Add(this.btn_GenBatchNo);
            base.Controls.Add(this.btn_Parse);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Margin = new Padding(4, 4, 4, 4);
            base.Name = "FormSysSetting";
            this.Text = "FormSysSetting";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
