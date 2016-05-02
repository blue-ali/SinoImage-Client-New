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
    public class UCBatchTempalteName : UserControl
    {
        private IContainer components = null;

        private Button btnSave;

        private Label label1;

        private TextBox textBox1;

        public string Title
        {
            get
            {
                return "新建模版";
            }
        }

        public string TemplateName
        {
            get;
            set;
        }

        public UCBatchTempalteName()
        {
            this.InitializeComponent();
            this.textBox1.Focus();
            base.KeyDown += new KeyEventHandler(this.UCBatchTempalteName_KeyDown);
        }

        private void UCBatchTempalteName_KeyDown(object sender, KeyEventArgs e)
        {
            bool flag = e.KeyCode == Keys.Return;
            if (flag)
            {
                this.btnSave_Click(this, EventArgs.Empty);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool flag = string.IsNullOrEmpty(this.textBox1.Text.Trim());
            if (!flag)
            {
                bool flag2 = BatchTemplateMgr.ContainTemplate(this.textBox1.Text);
                if (flag2)
                {
                    MessageBox.Show("已存在同名批次");
                }
                else
                {
                    this.TemplateName = this.textBox1.Text;
                    (base.Parent as Form).DialogResult = DialogResult.OK;
                    (base.Parent as Form).Close();
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
            this.btnSave = new Button();
            this.label1 = new Label();
            this.textBox1 = new TextBox();
            base.SuspendLayout();
            this.btnSave.Location = new Point(157, 133);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(132, 46);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存&S";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(53, 61);
            this.label1.Name = "label1";
            this.label1.Size = new Size(67, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "模版名称";
            this.textBox1.Location = new Point(173, 56);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(212, 25);
            this.textBox1.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnSave);
            base.Name = "UCBatchTempalteName";
            base.Size = new Size(452, 206);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
