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
    public class UCDelBatch : UserControl
    {
        private IContainer components = null;

        private Button btn_DelBatch;

        private TextBox textBox1;

        public string BathNOToDel
        {
            get;
            set;
        }

        public UCDelBatch()
        {
            this.InitializeComponent();
        }

        private void btn_DelBatch_Click(object sender, EventArgs e)
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
            this.btn_DelBatch = new Button();
            this.textBox1 = new TextBox();
            base.SuspendLayout();
            this.btn_DelBatch.Location = new Point(249, 258);
            this.btn_DelBatch.Name = "btn_DelBatch";
            this.btn_DelBatch.Size = new Size(115, 45);
            this.btn_DelBatch.TabIndex = 0;
            this.btn_DelBatch.Text = "删除批次";
            this.btn_DelBatch.UseVisualStyleBackColor = true;
            this.btn_DelBatch.Click += new EventHandler(this.btn_DelBatch_Click);
            this.textBox1.Location = new Point(129, 140);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(311, 22);
            this.textBox1.TabIndex = 1;
            base.AutoScaleDimensions = new SizeF(8f, 16f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.btn_DelBatch);
            base.Name = "UCDelBatch";
            base.Size = new Size(717, 345);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
