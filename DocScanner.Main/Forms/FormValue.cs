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
    public class FormValue : FormBase
    {
        public static int ScrValue = 0;

        private IContainer components = null;

        private TrackBar ValueBar;

        private Button BtCancel;

        private Button BtSure;

        private Label lab;

        public int ResultValue
        {
            get
            {
                return this.ValueBar.Value;
            }
        }

        public FormValue()
        {
            this.InitializeComponent();
        }

        public static bool FShowValue(string name)
        {
            return new FormValue
            {
                Text = name
            }.ShowDialog() == DialogResult.OK;
        }

        private void BtSure_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void ValueBar_Scroll(object sender, EventArgs e)
        {
            this.lab.Text = this.ValueBar.Value.ToString();
            FormValue.ScrValue = this.ValueBar.Value;
        }

        private void ValueBar_MouseUp(object sender, MouseEventArgs e)
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
            this.ValueBar = new TrackBar();
            this.BtCancel = new Button();
            this.BtSure = new Button();
            this.lab = new Label();
            ((ISupportInitialize)this.ValueBar).BeginInit();
            base.SuspendLayout();
            this.ValueBar.Location = new Point(29, 5);
            this.ValueBar.Maximum = 100;
            this.ValueBar.Minimum = -100;
            this.ValueBar.Name = "ValueBar";
            this.ValueBar.Size = new Size(343, 56);
            this.ValueBar.TabIndex = 7;
            this.ValueBar.TickFrequency = 5;
            this.ValueBar.Scroll += new EventHandler(this.ValueBar_Scroll);
            this.ValueBar.MouseUp += new MouseEventHandler(this.ValueBar_MouseUp);
            this.BtCancel.Location = new Point(293, 57);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new Size(75, 27);
            this.BtCancel.TabIndex = 6;
            this.BtCancel.Text = "取 消";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new EventHandler(this.BtCancel_Click);
            this.BtSure.Location = new Point(212, 57);
            this.BtSure.Name = "BtSure";
            this.BtSure.Size = new Size(75, 27);
            this.BtSure.TabIndex = 5;
            this.BtSure.Text = "确 定";
            this.BtSure.UseVisualStyleBackColor = true;
            this.BtSure.Click += new EventHandler(this.BtSure_Click);
            this.lab.Location = new Point(29, 36);
            this.lab.Name = "lab";
            this.lab.Size = new Size(343, 17);
            this.lab.TabIndex = 8;
            this.lab.Text = "0";
            this.lab.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(8f, 15f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(400, 88);
            base.Controls.Add(this.lab);
            base.Controls.Add(this.ValueBar);
            base.Controls.Add(this.BtCancel);
            base.Controls.Add(this.BtSure);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Margin = new Padding(5, 5, 5, 5);
            base.Name = "FormValue";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "亮度";
            ((ISupportInitialize)this.ValueBar).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
