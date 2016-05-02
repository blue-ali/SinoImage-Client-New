using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;

namespace DocScaner.PDF.Utils
{
	public class Text : Base
	{
		public void DrawPage(PdfPage page)
		{
			XGraphics gfx = XGraphics.FromPdfPage(page);
			base.DrawTitle(page, gfx, "Text");
			this.DrawText(gfx, 1);
			this.DrawTextAlignment(gfx, 2);
			this.MeasureText(gfx, 3);
		}

		private void DrawText(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "Text Styles");
			XPdfFontOptions pdfOptions = new XPdfFontOptions(PdfFontEncoding.WinAnsi, PdfFontEmbedding.Default);
			XFont font = new XFont("Times New Roman", 20.0, XFontStyle.Regular, pdfOptions);
			XFont font2 = new XFont("Times New Roman", 20.0, XFontStyle.Bold, pdfOptions);
			XFont font3 = new XFont("Times New Roman", 20.0, XFontStyle.Italic, pdfOptions);
			XFont font4 = new XFont("Times New Roman", 20.0, XFontStyle.BoldItalic, pdfOptions);
			gfx.DrawString("Times (regular)", font, XBrushes.DarkSlateGray, 0.0, 30.0);
			gfx.DrawString("Times (bold)", font2, XBrushes.DarkSlateGray, 0.0, 65.0);
			gfx.DrawString("Times (italic)", font3, XBrushes.DarkSlateGray, 0.0, 100.0);
			gfx.DrawString("Times (bold italic)", font4, XBrushes.DarkSlateGray, 0.0, 135.0);
			base.EndBox(gfx);
		}

		private void DrawTextAlignment(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "Text Alignment");
			XRect xRect = new XRect(0.0, 0.0, 250.0, 140.0);
			XFont font = new XFont("Verdana", 10.0);
			XBrush purple = XBrushes.Purple;
			XStringFormat xStringFormat = new XStringFormat();
			gfx.DrawRectangle(XPens.YellowGreen, xRect);
			gfx.DrawLine(XPens.YellowGreen, xRect.Width / 2.0, 0.0, xRect.Width / 2.0, xRect.Height);
			gfx.DrawLine(XPens.YellowGreen, 0.0, xRect.Height / 2.0, xRect.Width, xRect.Height / 2.0);
			gfx.DrawString("TopLeft", font, purple, xRect, xStringFormat);
			xStringFormat.Alignment = XStringAlignment.Center;
			gfx.DrawString("TopCenter", font, purple, xRect, xStringFormat);
			xStringFormat.Alignment = XStringAlignment.Far;
			gfx.DrawString("TopRight", font, purple, xRect, xStringFormat);
			xStringFormat.LineAlignment = XLineAlignment.Center;
			xStringFormat.Alignment = XStringAlignment.Near;
			gfx.DrawString("CenterLeft", font, purple, xRect, xStringFormat);
			xStringFormat.Alignment = XStringAlignment.Center;
			gfx.DrawString("Center", font, purple, xRect, xStringFormat);
			xStringFormat.Alignment = XStringAlignment.Far;
			gfx.DrawString("CenterRight", font, purple, xRect, xStringFormat);
			xStringFormat.LineAlignment = XLineAlignment.Far;
			xStringFormat.Alignment = XStringAlignment.Near;
			gfx.DrawString("BottomLeft", font, purple, xRect, xStringFormat);
			xStringFormat.Alignment = XStringAlignment.Center;
			gfx.DrawString("BottomCenter", font, purple, xRect, xStringFormat);
			xStringFormat.Alignment = XStringAlignment.Far;
			gfx.DrawString("BottomRight", font, purple, xRect, xStringFormat);
			base.EndBox(gfx);
		}

		private void MeasureText(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "Measure Text");
			XFont xFont = new XFont("Times New Roman", 95.0, XFontStyle.Regular);
			XSize xSize = gfx.MeasureString("Hallo", xFont);
			double height = xFont.GetHeight(gfx);
			int lineSpacing = xFont.FontFamily.GetLineSpacing(XFontStyle.Regular);
			int cellAscent = xFont.FontFamily.GetCellAscent(XFontStyle.Regular);
			int cellDescent = xFont.FontFamily.GetCellDescent(XFontStyle.Regular);
			int num = lineSpacing - cellAscent - cellDescent;
			double num2 = height * (double)cellAscent / (double)lineSpacing;
			gfx.DrawRectangle(XBrushes.Bisque, 20.0, 100.0 - num2, xSize.Width, num2);
			double num3 = height * (double)cellDescent / (double)lineSpacing;
			gfx.DrawRectangle(XBrushes.LightGreen, 20.0, 100.0, xSize.Width, num3);
			double height2 = height * (double)num / (double)lineSpacing;
			gfx.DrawRectangle(XBrushes.Yellow, 20.0, 100.0 + num3, xSize.Width, height2);
			XColor darkSlateBlue = XColors.DarkSlateBlue;
			darkSlateBlue.A = 0.6;
			gfx.DrawString("Hallo", xFont, new XSolidBrush(darkSlateBlue), 20.0, 100.0);
			base.EndBox(gfx);
		}
	}
}
