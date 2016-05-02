using AxFoxitReaderSDKProLib;
using DocScanner.Bean;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DocScanner.View.PDF
{
    public class UCFoxitPDFView : UserControl, IUCView
	{
		private NFileInfo _curfileinfo;

		private string[] _supportexts = new string[]
		{
			".pdf",
			".fdf"
		};

		private IContainer components = null;

		private AxFoxitReaderSDK axFoxitReaderSDK1;

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
					string file_path = (this._curfileinfo == null) ? string.Empty : (this._curfileinfo.LocalPath ?? string.Empty);
					this.axFoxitReaderSDK1.OpenFile(file_path, "");
				}
			}
		}

		public UCFoxitPDFView()
		{
			this.InitializeComponent();
			this.axFoxitReaderSDK1.Dock = DockStyle.Fill;
			this.axFoxitReaderSDK1.ShowTitleBar(false);
			this.axFoxitReaderSDK1.ShowToolbarButton(0, false);
			this.axFoxitReaderSDK1.ShowToolbarButton(1, false);
			this.axFoxitReaderSDK1.ShowToolbarButton(2, false);
			this.axFoxitReaderSDK1.ShowToolBar(false);
			this.axFoxitReaderSDK1.ShowStatusBar(false);
		}

		public bool ProcessCMD(string cmd)
		{
			bool flag = cmd == "DoPrint";
			if (flag)
			{
				this.axFoxitReaderSDK1.PrintWithDialog();
			}
			else
			{
				bool flag2 = cmd == "DoFitViewWidth";
				if (!flag2)
				{
					bool flag3 = cmd == "DoFitViewHeight";
					if (flag3)
					{
					}
				}
			}
			return true;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCFoxitPDFView));
            this.axFoxitReaderSDK1 = new AxFoxitReaderSDKProLib.AxFoxitReaderSDK();
            ((System.ComponentModel.ISupportInitialize)(this.axFoxitReaderSDK1)).BeginInit();
            this.SuspendLayout();
            // 
            // axFoxitReaderSDK1
            // 
            this.axFoxitReaderSDK1.Enabled = true;
            this.axFoxitReaderSDK1.Location = new System.Drawing.Point(35, 89);
            this.axFoxitReaderSDK1.Name = "axFoxitReaderSDK1";
            this.axFoxitReaderSDK1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axFoxitReaderSDK1.OcxState")));
            this.axFoxitReaderSDK1.Size = new System.Drawing.Size(800, 600);
            this.axFoxitReaderSDK1.TabIndex = 0;
            // 
            // UCFoxitPDFView
            // 
            this.Controls.Add(this.axFoxitReaderSDK1);
            this.Name = "UCFoxitPDFView";
            this.Size = new System.Drawing.Size(614, 435);
            ((System.ComponentModel.ISupportInitialize)(this.axFoxitReaderSDK1)).EndInit();
            this.ResumeLayout(false);

		}
	}
}
