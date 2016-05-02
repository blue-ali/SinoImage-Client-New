using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DocScanner.Bean;
using AxDJVUCONTROLLib;
using DocScaner.Common;

namespace DocScanner.View.DJVU
{
    public class UCDJVUView : UserControl, IUCView
	{
		private NFileInfo _curfileinfo;

		private string[] _supportexts = new string[]
		{
			".djvu"
		};

		private IContainer components = null;
        private AxDjVuCtl axDjVuCtl1;

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
					string fname = (this._curfileinfo == null) ? string.Empty : (this._curfileinfo.LocalPath ?? string.Empty);
					this.OpenFile(fname);
				}
			}
		}

		private void OpenFile(string fname)
		{
			this.axDjVuCtl1.SRC = FileHelper.LocalFile2URL(fname);
		}

		public UCDJVUView()
		{
			this.InitializeComponent();
		}

		public bool ProcessCMD(string cmd)
		{
			bool flag = cmd == "DoPrint";
			if (flag)
			{
				this.axDjVuCtl1.CmdPrint();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDJVUView));
            this.axDjVuCtl1 = new AxDJVUCONTROLLib.AxDjVuCtl();
            ((System.ComponentModel.ISupportInitialize)(this.axDjVuCtl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axDjVuCtl1
            // 
            this.axDjVuCtl1.Enabled = true;
            this.axDjVuCtl1.Location = new System.Drawing.Point(0, 0);
            this.axDjVuCtl1.Name = "axDjVuCtl1";
            this.axDjVuCtl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axDjVuCtl1.OcxState")));
            this.axDjVuCtl1.Size = new System.Drawing.Size(192, 192);
            this.axDjVuCtl1.TabIndex = 0;
            // 
            // UCDJVUView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UCDJVUView";
            this.Size = new System.Drawing.Size(688, 464);
            ((System.ComponentModel.ISupportInitialize)(this.axDjVuCtl1)).EndInit();
            this.ResumeLayout(false);

		}
	}
}
