using DocScanner.Bean;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DocScanner.View.IE
{
    public class UCWebView : UserControl, IUCView
	{
		private string[] _supportexts = new string[]
		{
			".html",
			".htm",
			".shtml",
			".xhtml",
			".mhtml",
			".mht",
			".xml"
		};

		private NFileInfo _curfileinfo;

		private IContainer components = null;

		private WebBrowser webBrowser1;

		public NFileInfo CurFileInfo
		{
			get
			{
				return this._curfileinfo;
			}
			set
			{
				bool flag = this._curfileinfo != value;
				if (flag)
				{
					this._curfileinfo = value;
					string urlString = (this._curfileinfo == null) ? string.Empty : (this._curfileinfo.LocalPath ?? string.Empty);
					this.webBrowser1.Navigate(urlString);
				}
			}
		}

		public UCWebView()
		{
			this.InitializeComponent();
		}

		public bool ProcessCMD(string cmd)
		{
			bool flag = cmd == "DoPrint";
			if (flag)
			{
				this.DoPrint();
			}
			return true;
		}

		private void DoPrint()
		{
			this.webBrowser1.ShowPrintDialog();
		}

		public string[] GetSupportTypeExt()
		{
			return this._supportexts;
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
			this.webBrowser1 = new WebBrowser();
			base.SuspendLayout();
			this.webBrowser1.Dock = DockStyle.Fill;
			this.webBrowser1.Location = new Point(0, 0);
			this.webBrowser1.MinimumSize = new Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new Size(739, 543);
			this.webBrowser1.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.webBrowser1);
			base.Name = "UCWebViewer";
			base.Size = new Size(739, 543);
			base.ResumeLayout(false);
		}
	}
}
