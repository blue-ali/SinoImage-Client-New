using DocScanner.Bean;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DocScanner.View.RTF_TXT
{
    public class UCRTFTEXTView : UserControl, IUCView
	{
		private NFileInfo _curfileinfo;

		private string[] _supportexts = new string[]
		{
			".rtf",
			".txt"
		};

		private IContainer components = null;

		private RichTextBox richTextBox1;

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
					string fname = (this._curfileinfo == null) ? null : this._curfileinfo.LocalPath;
					this.OpenFile(fname);
				}
			}
		}

		private void OpenFile(string fname)
		{
			bool flag = string.IsNullOrEmpty(fname) || !File.Exists(fname);
			if (flag)
			{
				this.richTextBox1.Text = "";
			}
			else
			{
				bool flag2 = fname.ToLower().EndsWith(".rtf");
				if (flag2)
				{
					this.richTextBox1.LoadFile(fname);
				}
				else
				{
					bool flag3 = fname.ToLower().EndsWith(".txt");
					if (flag3)
					{
						this.richTextBox1.Text = File.ReadAllText(fname, new UTF8Encoding());
					}
				}
			}
		}

		public UCRTFTEXTView()
		{
			this.InitializeComponent();
			Color backColor = this.richTextBox1.BackColor;
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.BackColor = backColor;
		}

		public bool ProcessCMD(string cmd)
		{
			bool flag = cmd == "DoPrint";
			if (flag)
			{
				RichTextBoxPrint richTextBoxPrint = new RichTextBoxPrint();
				richTextBoxPrint.DoPrint(this.richTextBox1);
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
			this.richTextBox1 = new RichTextBox();
			base.SuspendLayout();
			this.richTextBox1.Dock = DockStyle.Fill;
			this.richTextBox1.Location = new Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new Size(763, 580);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.richTextBox1);
			base.Name = "UCWordView";
			base.Size = new Size(763, 580);
			base.ResumeLayout(false);
		}
	}
}
