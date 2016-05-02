using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.LibCommon
{
    public class FormBase : Form
    {
        // Fields
        private IContainer components = null;

        // Methods
        public FormBase()
        {
            this.InitializeComponent();
            base.ShowInTaskbar = false;
            base.ShowIcon = false;
            base.StartPosition = FormStartPosition.CenterParent;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x148, 0x135);
            base.Name = "FormBase";
            this.Text = "FormBase";
            base.ResumeLayout(false);
        }
    }

}
