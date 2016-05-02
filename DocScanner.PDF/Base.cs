using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;

namespace DocScaner.PDF.Utils
{
	public class Base
	{
		protected XColor backColor;

		protected XColor backColor2;

		protected XColor shadowColor;

		protected double borderWidth;

		protected XPen borderPen;

		private static XPoint[] pentagram;

		private XGraphicsState state;

		private static XPoint[] Pentagram
		{
			get
			{
				bool flag = Base.pentagram == null;
				if (flag)
				{
					int[] array = new int[]
					{
						0,
						3,
						1,
						4,
						2
					};
					Base.pentagram = new XPoint[5];
					for (int i = 0; i < 5; i++)
					{
						double num = (double)(array[i] * 2) * 3.1415926535897931 / 5.0 - 0.31415926535897931;
						Base.pentagram[i].X = Math.Cos(num);
						Base.pentagram[i].Y = Math.Sin(num);
					}
				}
				return Base.pentagram;
			}
		}

		protected Base()
		{
			this.backColor = XColors.Ivory;
			this.backColor2 = XColors.WhiteSmoke;
			this.backColor = XColor.FromArgb(212, 224, 240);
			this.backColor2 = XColor.FromArgb(253, 254, 254);
			this.shadowColor = XColors.Gainsboro;
			this.borderWidth = 4.5;
			this.borderPen = new XPen(XColor.FromArgb(94, 118, 151), this.borderWidth);
		}

		public void DrawTitle(PdfPage page, XGraphics gfx, string title)
		{
			XRect layoutRectangle = new XRect(default(XPoint), gfx.PageSize);
			layoutRectangle.Inflate(-10.0, -15.0);
			XFont font = new XFont("Verdana", 14.0, XFontStyle.Bold);
			gfx.DrawString(title, font, XBrushes.MidnightBlue, layoutRectangle, XStringFormats.TopCenter);
			layoutRectangle.Offset(0.0, 5.0);
			font = new XFont("Verdana", 8.0, XFontStyle.Italic);
			XStringFormat xStringFormat = new XStringFormat();
			xStringFormat.Alignment = XStringAlignment.Near;
			xStringFormat.LineAlignment = XLineAlignment.Far;
			gfx.DrawString("Created with PDFsharp 1.32.2608-g (www.pdfsharp.net)", font, XBrushes.DarkOrchid, layoutRectangle, xStringFormat);
			font = new XFont("Verdana", 8.0);
			xStringFormat.Alignment = XStringAlignment.Center;
			gfx.DrawString(Program.s_document.PageCount.ToString(), font, XBrushes.DarkOrchid, layoutRectangle, xStringFormat);
			Program.s_document.Outlines.Add(title, page, true);
		}

		public void BeginBox(XGraphics gfx, int number, string title)
		{
			XRect xRect = new XRect(0.0, 20.0, 300.0, 200.0);
			bool flag = number % 2 == 0;
			if (flag)
			{
				xRect.X = 295.0;
			}
			xRect.Y = (double)(40 + (number - 1) / 2 * 195);
			xRect.Inflate(-10.0, -10.0);
			XRect rect = xRect;
			rect.Offset(this.borderWidth, this.borderWidth);
			gfx.DrawRoundedRectangle(new XSolidBrush(this.shadowColor), rect, new XSize(23.0, 23.0));
			XLinearGradientBrush brush = new XLinearGradientBrush(xRect, this.backColor, this.backColor2, XLinearGradientMode.Vertical);
			gfx.DrawRoundedRectangle(this.borderPen, brush, xRect, new XSize(15.0, 15.0));
			xRect.Inflate(-5.0, -5.0);
			XFont font = new XFont("Verdana", 12.0, XFontStyle.Regular);
			gfx.DrawString(title, font, XBrushes.Navy, xRect, XStringFormats.TopCenter);
			xRect.Inflate(-10.0, -5.0);
			xRect.Y += 20.0;
			xRect.Height -= 20.0;
			this.state = gfx.Save();
			gfx.TranslateTransform(xRect.X, xRect.Y);
		}

		public void EndBox(XGraphics gfx)
		{
			gfx.Restore(this.state);
		}
	}
}
