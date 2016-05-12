using DocScanner.Network;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Main
{
    public class UCSetupWizard : UserControl
    {
        private IContainer components = null;

        private CheckBox checkBoxUseCluster;

        private Label label1;

        private TextBox textBoxTmpFileDir;

        private Button btnTmpFileDir;

        private Button btnOK;

        private Label label2;

        private TextBox textBoxServerAdds;

        public string ServerAddrs
        {
            get
            {
                return AbstractSetting<NetSetting>.CurSetting.HttpServerHosts;
            }
            set
            {
                AbstractSetting<NetSetting>.CurSetting.HttpServerHosts = value;
            }
        }

        public bool UseCluster
        {
            get
            {
                return AbstractSetting<NetSetting>.CurSetting.AllowServerBalance;
            }
            set
            {
                AbstractSetting<NetSetting>.CurSetting.AllowServerBalance = value;
            }
        }

        public string TmpFileDir
        {
            get
            {
                return AppSetting.GetInstance().TmpFileDir;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && Directory.Exists(value))
                {
                    AppSetting.GetInstance().TmpFileDir = value;
                }
            }
        }

        public string Title
        {
            get
            {
                return "系统设置向导";
            }
        }

        public UCSetupWizard()
        {
            this.InitializeComponent();
            this.textBoxTmpFileDir.Text = this.TmpFileDir;
            this.checkBoxUseCluster.Checked = this.UseCluster;
            this.textBoxServerAdds.Text = this.ServerAddrs;
        }

        private void btnTmpFileDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            bool flag = folderBrowserDialog.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBoxTmpFileDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.TmpFileDir = this.textBoxTmpFileDir.Text;
            this.UseCluster = this.checkBoxUseCluster.Checked;
            this.ServerAddrs = this.textBoxServerAdds.Text;
            this.GetHostForm().DialogResult = DialogResult.OK;
            this.GetHostForm().Close();
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
            this.checkBoxUseCluster = new CheckBox();
            this.label1 = new Label();
            this.textBoxTmpFileDir = new TextBox();
            this.btnTmpFileDir = new Button();
            this.btnOK = new Button();
            this.label2 = new Label();
            this.textBoxServerAdds = new TextBox();
            base.SuspendLayout();
            this.checkBoxUseCluster.AutoSize = true;
            this.checkBoxUseCluster.Location = new Point(72, 62);
            this.checkBoxUseCluster.Name = "checkBoxUseCluster";
            this.checkBoxUseCluster.Size = new Size(164, 19);
            this.checkBoxUseCluster.TabIndex = 0;
            this.checkBoxUseCluster.Text = "后台使用服务器集群";
            this.checkBoxUseCluster.UseVisualStyleBackColor = true;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(72, 147);
            this.label1.Name = "label1";
            this.label1.Size = new Size(97, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "临时文件目录";
            this.textBoxTmpFileDir.Location = new Point(191, 143);
            this.textBoxTmpFileDir.Name = "textBoxTmpFileDir";
            this.textBoxTmpFileDir.Size = new Size(263, 25);
            this.textBoxTmpFileDir.TabIndex = 2;
            this.btnTmpFileDir.Location = new Point(484, 143);
            this.btnTmpFileDir.Name = "btnTmpFileDir";
            this.btnTmpFileDir.Size = new Size(75, 27);
            this.btnTmpFileDir.TabIndex = 3;
            this.btnTmpFileDir.Text = "...";
            this.btnTmpFileDir.UseVisualStyleBackColor = true;
            this.btnTmpFileDir.Click += new EventHandler(this.btnTmpFileDir_Click);
            this.btnOK.Location = new Point(533, 498);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(142, 62);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定&O";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(88, 212);
            this.label2.Name = "label2";
            this.label2.Size = new Size(90, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "服务器地址:";
            this.textBoxServerAdds.Location = new Point(199, 215);
            this.textBoxServerAdds.Name = "textBoxServerAdds";
            this.textBoxServerAdds.Size = new Size(285, 25);
            this.textBoxServerAdds.TabIndex = 6;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.textBoxServerAdds);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnTmpFileDir);
            base.Controls.Add(this.textBoxTmpFileDir);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.checkBoxUseCluster);
            base.Name = "UCSetupWizard";
            base.Size = new Size(802, 608);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
