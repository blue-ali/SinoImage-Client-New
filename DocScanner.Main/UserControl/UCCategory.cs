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
    public class UCCategory : UserControl
    {
        private IContainer components = null;

        private Button button1;

        private Label label1;

        private TextBox textBox_Category;

        public string Title
        {
            get
            {
                return "分类名称";
            }
        }

        public string CategoryName
        {
            get
            {
                return this.textBox_Category.Text.Trim();
            }
        }

        public UCCategory()
        {
            this.InitializeComponent();
            this.textBox_Category.KeyDown += new KeyEventHandler(this.TextBox_Category_KeyDown);
            this.textBox_Category.Focus();
        }

        private void TextBox_Category_KeyDown(object sender, KeyEventArgs e)
        {
            bool flag = e.KeyCode == Keys.Return;
            if (flag)
            {
                this.btnOk_Click(this, EventArgs.Empty);
            }
            bool flag2 = e.KeyCode == Keys.Escape;
            if (flag2)
            {
                (base.Parent as Form).DialogResult = DialogResult.Cancel;
                (base.Parent as Form).Close();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool flag = string.IsNullOrEmpty(this.textBox_Category.Text.Trim());
            if (!flag)
            {
                (base.Parent as Form).DialogResult = DialogResult.OK;
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
            this.button1 = new Button();
            this.label1 = new Label();
            this.textBox_Category = new TextBox();
            base.SuspendLayout();
            this.button1.Location = new Point(197, 193);
            this.button1.Name = "button1";
            this.button1.Size = new Size(87, 37);
            this.button1.TabIndex = 0;
            this.button1.Text = "确定&C";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.btnOk_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(54, 104);
            this.label1.Name = "label1";
            this.label1.Size = new Size(37, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "名称";
            this.textBox_Category.Location = new Point(121, 99);
            this.textBox_Category.Name = "textBox_Category";
            this.textBox_Category.Size = new Size(300, 25);
            this.textBox_Category.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.button1);
            base.Controls.Add(this.textBox_Category);
            base.Controls.Add(this.label1);
            base.Name = "UCCategory";
            base.Size = new Size(482, 267);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
