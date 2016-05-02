using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DocScanner.View.RTF_TXT
{
	public class RichTextBoxPrint
	{
		private struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;
		}

		private struct CHARRANGE
		{
			public int cpMin;

			public int cpMax;
		}

		private struct FORMATRANGE
		{
			public IntPtr hdc;

			public IntPtr hdcTarget;

			public RichTextBoxPrint.RECT rc;

			public RichTextBoxPrint.RECT rcPage;

			public RichTextBoxPrint.CHARRANGE chrg;
		}

		private const double anInch = 14.4;

		private const int WM_USER = 1024;

		private const int EM_FORMATRANGE = 1081;

		private int checkPrint = 0;

		private RichTextBox _box;

		[DllImport("USER32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		private int Print(RichTextBox box, int charFrom, int charTo, PrintPageEventArgs e)
		{
			RichTextBoxPrint.RECT rc;
			rc.Top = (int)((double)e.MarginBounds.Top * 14.4);
			rc.Bottom = (int)((double)e.MarginBounds.Bottom * 14.4);
			rc.Left = (int)((double)e.MarginBounds.Left * 14.4);
			rc.Right = (int)((double)e.MarginBounds.Right * 14.4);
			RichTextBoxPrint.RECT rcPage;
			rcPage.Top = (int)((double)e.PageBounds.Top * 14.4);
			rcPage.Bottom = (int)((double)e.PageBounds.Bottom * 14.4);
			rcPage.Left = (int)((double)e.PageBounds.Left * 14.4);
			rcPage.Right = (int)((double)e.PageBounds.Right * 14.4);
			IntPtr hdc = e.Graphics.GetHdc();
			RichTextBoxPrint.FORMATRANGE fORMATRANGE;
			fORMATRANGE.chrg.cpMax = charTo;
			fORMATRANGE.chrg.cpMin = charFrom;
			fORMATRANGE.hdc = hdc;
			fORMATRANGE.hdcTarget = hdc;
			fORMATRANGE.rc = rc;
			fORMATRANGE.rcPage = rcPage;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			zero = new IntPtr(1);
			IntPtr intPtr2 = IntPtr.Zero;
			intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(fORMATRANGE));
			Marshal.StructureToPtr(fORMATRANGE, intPtr2, false);
			intPtr = RichTextBoxPrint.SendMessage(box.Handle, 1081, zero, intPtr2);
			Marshal.FreeCoTaskMem(intPtr2);
			e.Graphics.ReleaseHdc(hdc);
			return intPtr.ToInt32();
		}

		public void DoPrint(RichTextBox box)
		{
			this._box = box;
			PrintDocument printDocument = new PrintDocument();
			printDocument.BeginPrint += new PrintEventHandler(this.Doc_BeginPrint);
			printDocument.PrintPage += new PrintPageEventHandler(this.Doc_PrintPage);
			PrintDialog printDialog = new PrintDialog();
			bool flag = printDialog.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				printDocument.Print();
			}
		}

		private void Doc_PrintPage(object sender, PrintPageEventArgs e)
		{
			this.checkPrint = this.Print(this._box, this.checkPrint, this._box.TextLength, e);
			bool flag = this.checkPrint < this._box.TextLength;
			if (flag)
			{
				e.HasMorePages = true;
			}
			else
			{
				e.HasMorePages = false;
			}
		}

		private void Doc_BeginPrint(object sender, PrintEventArgs e)
		{
			this.checkPrint = 0;
		}
	}
}
