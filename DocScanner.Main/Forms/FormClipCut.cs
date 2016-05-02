using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DocScanner.Main
{
    public class FormClipCut : RadForm
    {
        private IContainer components = null;

        private Button button1;

        private Button button2;

        private Label label2;

        public FormClipCut()
        {
            this.InitializeComponent();
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
            this.button2 = new Button();
            this.label2 = new Label();
            base.SuspendLayout();
            this.button1.DialogResult = DialogResult.Cancel;
            this.button1.Location = new Point(41, 76);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button2.DialogResult = DialogResult.OK;
            this.button2.Location = new Point(138, 76);
            this.button2.Name = "button2";
            this.button2.Size = new Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "剪裁";
            this.button2.UseVisualStyleBackColor = true;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(39, 37);
            this.label2.Name = "label2";
            this.label2.Size = new Size(137, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "剪裁您所选择的区域吗？";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(241, 133);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Location = new Point(200, 200);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FormClipCut";
            this.Text = "裁剪";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
