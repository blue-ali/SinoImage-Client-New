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
    public class UCLocalQueryBatchNO : UserControl
    {
        private IContainer components = null;

        public UCLocalQueryBatchNO()
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
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(8f, 16f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "UCLocalQueryBatchNO";
            base.Size = new Size(784, 396);
            base.ResumeLayout(false);
        }
    }
}
