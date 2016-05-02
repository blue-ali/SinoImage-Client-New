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
    public class FormClipViewer : FormBase
	{
		private IContainer components = null;

		private PictureBox picZoom;

		public Image CutBit
		{
			set
			{
				this.picZoom.Image = value;
			}
		}

		public FormClipViewer()
		{
			this.InitializeComponent();
			base.KeyDown += new KeyEventHandler(this.FormClipViewer_KeyDown);
		}

		private void FormClipViewer_KeyDown(object sender, KeyEventArgs e)
		{
			bool flag = e.KeyCode == Keys.Escape;
			if (flag)
			{
				base.Close();
			}
		}

		private void picZoom_DoubleClick(object sender, EventArgs e)
		{
			base.Close();
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
			this.picZoom = new PictureBox();
			((ISupportInitialize)this.picZoom).BeginInit();
			base.SuspendLayout();
			this.picZoom.Dock = DockStyle.Fill;
			this.picZoom.Location = new Point(0, 0);
			this.picZoom.Margin = new Padding(4);
			this.picZoom.Name = "picZoom";
			this.picZoom.Size = new Size(844, 552);
			this.picZoom.SizeMode = PictureBoxSizeMode.Zoom;
			this.picZoom.TabIndex = 0;
			this.picZoom.TabStop = false;
			this.picZoom.DoubleClick += new EventHandler(this.picZoom_DoubleClick);
			base.AutoScaleDimensions = new SizeF(8f, 16f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(844, 552);
			base.Controls.Add(this.picZoom);
			base.Margin = new Padding(4);
			base.Name = "FormClipViewer";
			this.Text = "框选图像查看-ESC退出";
			base.KeyDown += new KeyEventHandler(this.FormClipViewer_KeyDown);
			((ISupportInitialize)this.picZoom).EndInit();
			base.ResumeLayout(false);
		}
	}
}
