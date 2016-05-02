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
    public class UCAccountMgr : UserControl
    {
        private IContainer components = null;

        public string Title
        {
            get
            {
                return "账户管理";
            }
        }

        public UCAccountMgr()
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
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "UCAccountMgr";
            base.Size = new Size(980, 626);
            base.ResumeLayout(false);
        }
    }
}
