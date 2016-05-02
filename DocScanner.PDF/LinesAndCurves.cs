using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;

namespace DocScaner.PDF.Utils
{
	public class LinesAndCurves : Base
	{
		public void DrawPage(PdfPage page)
		{
			XGraphics gfx = XGraphics.FromPdfPage(page);
			base.DrawTitle(page, gfx, "Lines & Curves");
			this.DrawLine(gfx, 1);
			this.DrawLines(gfx, 2);
			this.DrawBezier(gfx, 3);
			this.DrawBeziers(gfx, 4);
			this.DrawCurve(gfx, 5);
			this.DrawArc(gfx, 6);
		}

		private void DrawLine(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawLine");
			gfx.DrawLine(XPens.DarkGreen, 0, 0, 250, 0);
			gfx.DrawLine(XPens.Gold, 15, 7, 230, 15);
			XPen xPen = new XPen(XColors.Navy, 4.0);
			gfx.DrawLine(xPen, 0, 20, 250, 20);
			xPen = new XPen(XColors.Firebrick, 6.0);
			xPen.DashStyle = XDashStyle.Dash;
			gfx.DrawLine(xPen, 0, 40, 250, 40);
			xPen.Width = 7.3;
			xPen.DashStyle = XDashStyle.DashDotDot;
			gfx.DrawLine(xPen, 0, 60, 250, 60);
			gfx.DrawLine(new XPen(XColors.Goldenrod, 10.0)
			{
				LineCap = XLineCap.Flat
			}, 10, 90, 240, 90);
			gfx.DrawLine(XPens.Black, 10, 90, 240, 90);
			gfx.DrawLine(new XPen(XColors.Goldenrod, 10.0)
			{
				LineCap = XLineCap.Square
			}, 10, 110, 240, 110);
			gfx.DrawLine(XPens.Black, 10, 110, 240, 110);
			gfx.DrawLine(new XPen(XColors.Goldenrod, 10.0)
			{
				LineCap = XLineCap.Round
			}, 10, 130, 240, 130);
			gfx.DrawLine(XPens.Black, 10, 130, 240, 130);
			base.EndBox(gfx);
		}

		private void DrawLines(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawLines");
			XPen xPen = new XPen(XColors.DarkSeaGreen, 6.0);
			xPen.LineCap = XLineCap.Round;
			xPen.LineJoin = XLineJoin.Bevel;
			XPoint[] points = new XPoint[]
			{
				new XPoint(20.0, 30.0),
				new XPoint(60.0, 120.0),
				new XPoint(90.0, 20.0),
				new XPoint(170.0, 90.0),
				new XPoint(230.0, 40.0)
			};
			gfx.DrawLines(xPen, points);
			base.EndBox(gfx);
		}

		private void DrawBezier(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawBezier");
			gfx.DrawBezier(new XPen(XColors.DarkRed, 5.0), 20.0, 110.0, 40.0, 10.0, 170.0, 90.0, 230.0, 20.0);
			base.EndBox(gfx);
		}

		private void DrawBeziers(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawBeziers");
			XPoint[] points = new XPoint[]
			{
				new XPoint(20.0, 30.0),
				new XPoint(40.0, 120.0),
				new XPoint(80.0, 20.0),
				new XPoint(110.0, 90.0),
				new XPoint(180.0, 40.0),
				new XPoint(210.0, 40.0),
				new XPoint(220.0, 80.0)
			};
			XPen pen = new XPen(XColors.Firebrick, 4.0);
			gfx.DrawBeziers(pen, points);
			base.EndBox(gfx);
		}

		private void DrawCurve(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawCurve");
			XPoint[] points = new XPoint[]
			{
				new XPoint(20.0, 30.0),
				new XPoint(60.0, 120.0),
				new XPoint(90.0, 20.0),
				new XPoint(170.0, 90.0),
				new XPoint(230.0, 40.0)
			};
			XPen pen = new XPen(XColors.RoyalBlue, 3.5);
			gfx.DrawCurve(pen, points, 1.0);
			base.EndBox(gfx);
		}

		private void DrawArc(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawArc");
			XPen pen = new XPen(XColors.Plum, 4.7);
			gfx.DrawArc(pen, 0, 0, 250, 140, 190, 200);
			base.EndBox(gfx);
		}
	}
}
