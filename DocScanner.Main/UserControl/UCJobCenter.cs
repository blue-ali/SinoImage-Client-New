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
    public class UCJobCenter : UserControl
    {
        private IContainer components = null;

        private DateTimePicker dateTimePicker1;

        public string Title
        {
            get
            {
                return "定时任务";
            }
        }

        public UCJobCenter()
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
            this.dateTimePicker1 = new DateTimePicker();
            base.SuspendLayout();
            this.dateTimePicker1.Location = new Point(131, 172);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new Size(253, 25);
            this.dateTimePicker1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.dateTimePicker1);
            base.Name = "UCJobCenter";
            base.Size = new Size(953, 536);
            base.ResumeLayout(false);
        }
    }
}
