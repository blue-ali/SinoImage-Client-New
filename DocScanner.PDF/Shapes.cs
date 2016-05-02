using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;

namespace DocScaner.PDF.Utils
{
	public class Shapes : Base
	{
		public void DrawPage(PdfPage page)
		{
			XGraphics gfx = XGraphics.FromPdfPage(page);
			base.DrawTitle(page, gfx, "Shapes");
			this.DrawRectangle(gfx, 1);
			this.DrawRoundedRectangle(gfx, 2);
			this.DrawEllipse(gfx, 3);
			this.DrawPolygon(gfx, 4);
			this.DrawPie(gfx, 5);
			this.DrawClosedCurve(gfx, 6);
		}

		private void DrawRectangle(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawRectangle");
			XPen pen = new XPen(XColors.Navy, 3.1415926535897931);
			gfx.DrawRectangle(pen, 10, 0, 100, 60);
			gfx.DrawRectangle(XBrushes.DarkOrange, 130, 0, 100, 60);
			gfx.DrawRectangle(pen, XBrushes.DarkOrange, 10, 80, 100, 60);
			gfx.DrawRectangle(pen, XBrushes.DarkOrange, 150, 80, 60, 60);
			base.EndBox(gfx);
		}

		private void DrawRoundedRectangle(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawRoundedRectangle");
			XPen pen = new XPen(XColors.RoyalBlue, 3.1415926535897931);
			gfx.DrawRoundedRectangle(pen, 10, 0, 100, 60, 30, 20);
			gfx.DrawRoundedRectangle(XBrushes.Orange, 130, 0, 100, 60, 30, 20);
			gfx.DrawRoundedRectangle(pen, XBrushes.Orange, 10, 80, 100, 60, 30, 20);
			gfx.DrawRoundedRectangle(pen, XBrushes.Orange, 150, 80, 60, 60, 20, 20);
			base.EndBox(gfx);
		}

		private void DrawEllipse(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawEllipse");
			XPen pen = new XPen(XColors.DarkBlue, 2.5);
			gfx.DrawEllipse(pen, 10, 0, 100, 60);
			gfx.DrawEllipse(XBrushes.Goldenrod, 130, 0, 100, 60);
			gfx.DrawEllipse(pen, XBrushes.Goldenrod, 10, 80, 100, 60);
			gfx.DrawEllipse(pen, XBrushes.Goldenrod, 150, 80, 60, 60);
			base.EndBox(gfx);
		}

		private void DrawPolygon(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawPolygon");
			XPen xPen = new XPen(XColors.DarkBlue, 2.5);
			base.EndBox(gfx);
		}

		private void DrawPie(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawPie");
			XPen pen = new XPen(XColors.DarkBlue, 2.5);
			gfx.DrawPie(pen, 10, 0, 100, 90, -120, 75);
			gfx.DrawPie(XBrushes.Gold, 130, 0, 100, 90, -160, 150);
			gfx.DrawPie(pen, XBrushes.Gold, 10, 50, 100, 90, 80, 70);
			gfx.DrawPie(pen, XBrushes.Gold, 150, 80, 60, 60, 35, 290);
			base.EndBox(gfx);
		}

		private void DrawClosedCurve(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawClosedCurve");
			XPen pen = new XPen(XColors.DarkBlue, 2.5);
			gfx.DrawClosedCurve(pen, XBrushes.SkyBlue, new XPoint[]
			{
				new XPoint(10.0, 120.0),
				new XPoint(80.0, 30.0),
				new XPoint(220.0, 20.0),
				new XPoint(170.0, 110.0),
				new XPoint(100.0, 90.0)
			}, XFillMode.Winding, 0.7);
			base.EndBox(gfx);
		}
	}
}
