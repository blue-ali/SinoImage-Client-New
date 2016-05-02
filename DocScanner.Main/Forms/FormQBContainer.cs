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
    public class FormQBContainer : FormBase
    {
        private IContainer components = null;

        private TabControl tabControl1;

        public FormQBContainer()
        {
            this.InitializeComponent();
            this.Text = "查询";
        }

        public FormQBContainer AddControl(Type ctltype)
        {
            object obj = ReflectHelper.Construct(ctltype);
            Control ctrl = (Control)obj;
            this.AddControl(ctrl);
            return this;
        }

        public FormQBContainer AddControl(Control ctrl)
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = FormHelper.GetCtrlTitle(ctrl);
            tabPage.Controls.Add(ctrl);
            ctrl.Dock = DockStyle.Fill;
            this.tabControl1.TabPages.Add(tabPage);
            return this;
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
            this.tabControl1 = new TabControl();
            base.SuspendLayout();
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(840, 516);
            this.tabControl1.TabIndex = 5;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(840, 516);
            base.Controls.Add(this.tabControl1);
            base.Name = "FormQBContainer";
            this.Text = "FormQBContainer";
            base.ResumeLayout(false);
        }
    }
}
