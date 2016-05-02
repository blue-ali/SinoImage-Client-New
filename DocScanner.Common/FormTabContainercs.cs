using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocScanner.Common
{
    public class FormTabContainercs : Form
    {
        // Fields
        private TabControl _tab;
        private IContainer components = null;

        // Methods
        public FormTabContainercs()
        {
            this.InitializeComponent();
            this._tab = new TabControl();
            base.Controls.Add(this._tab);
            this._tab.Dock = DockStyle.Fill;
        }

        public void AddControl(UserControl uc)
        {
            TabPage page = new TabPage();
            this._tab.TabPages.Add(page);
            page.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
            page.Text = uc.Name;
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
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2c4, 580);
            base.Name = "FormTabContainercs";
            this.Text = "FormTabContainercs";
            base.ResumeLayout(false);
        }
    }

}
