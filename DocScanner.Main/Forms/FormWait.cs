using DocScanner.LibCommon;
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
    public class FormWait : FormBase
    {
        private IContainer components = null;

        private RadProgressBar progressBar1;

        public string ProgessBarText
        {
            get
            {
                return this.progressBar1.Text;
            }
            set
            {
                this.progressBar1.Text = value;
            }
        }

        public FormWait()
        {
            this.InitializeComponent();
            this.progressBar1.SeparatorColor1 = Color.FromArgb(239, 239, 239);
            this.progressBar1.SeparatorColor2 = Color.White;
            this.progressBar1.SeparatorWidth = 0;
            this.progressBar1.ShowProgressIndicators = true;
            this.progressBar1.SweepAngle = 120;
            this.progressBar1.TextAlignment = ContentAlignment.MiddleCenter;
            this.progressBar1.Value1 = 60;
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
            this.progressBar1 = new RadProgressBar();
            ((ISupportInitialize)this.progressBar1).BeginInit();
            base.SuspendLayout();
            this.progressBar1.BackColor = SystemColors.ControlLightLight;
            this.progressBar1.Dash = false;
            this.progressBar1.Dock = DockStyle.Fill;
            this.progressBar1.Location = new Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RootElement.ControlBounds = new Rectangle(140, 27, 150, 30);
            this.progressBar1.SeparatorColor1 = Color.FromArgb(239, 239, 239);
            this.progressBar1.SeparatorColor2 = Color.White;
            this.progressBar1.SeparatorWidth = 4;
            this.progressBar1.Size = new Size(503, 72);
            this.progressBar1.StepWidth = 13;
            this.progressBar1.TabIndex = 0;
            this.progressBar1.TextAlignment = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(503, 72);
            base.Controls.Add(this.progressBar1);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "FormWait";
            this.Text = "FormWait";
            ((ISupportInitialize)this.progressBar1).EndInit();
            base.ResumeLayout(false);
        }
    }
}
