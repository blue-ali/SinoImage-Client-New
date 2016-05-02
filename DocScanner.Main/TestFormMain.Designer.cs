using System.Drawing;
using System.Windows.Forms;

namespace DocScanner.Main
{
    partial class TestFormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private UCBench ucBench1;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ucBench1 = new UCBench();
            base.SuspendLayout();
            this.ucBench1.Dock = DockStyle.Fill;
            this.ucBench1.Location = new Point(0, 0);
            this.ucBench1.Name = "ucBench1";
            this.ucBench1.Size = new Size(0x380, 0x273);
            this.ucBench1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x380, 0x273);
            base.Controls.Add(this.ucBench1);
            base.Name = "TestFormMain";
            this.Text = "Form1";
            base.ResumeLayout(false);

        }

        #endregion
    }
}