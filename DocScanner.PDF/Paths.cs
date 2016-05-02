using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;

namespace DocScaner.PDF.Utils
{
	public class Paths : Base
	{
		public void DrawPage(PdfPage page)
		{
			XGraphics gfx = XGraphics.FromPdfPage(page);
			base.DrawTitle(page, gfx, "Paths");
			this.DrawPathOpen(gfx, 1);
			this.DrawPathClosed(gfx, 2);
			this.DrawPathAlternateAndWinding(gfx, 3);
			this.DrawGlyphs(gfx, 5);
			this.DrawClipPath(gfx, 6);
		}

		private void DrawPathOpen(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawPath (open)");
			XPen xPen = new XPen(XColors.Navy, 3.1415926535897931);
			xPen.DashStyle = XDashStyle.Dash;
			XGraphicsPath xGraphicsPath = new XGraphicsPath();
			xGraphicsPath.AddLine(10, 120, 50, 60);
			xGraphicsPath.AddArc(50, 20, 110, 80, 180, 180);
			xGraphicsPath.AddLine(160, 60, 220, 100);
			gfx.DrawPath(xPen, xGraphicsPath);
			base.EndBox(gfx);
		}

		private void DrawPathClosed(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawPath (closed)");
			XPen xPen = new XPen(XColors.Navy, 3.1415926535897931);
			xPen.DashStyle = XDashStyle.Dash;
			XGraphicsPath xGraphicsPath = new XGraphicsPath();
			xGraphicsPath.AddLine(10, 120, 50, 60);
			xGraphicsPath.AddArc(50, 20, 110, 80, 180, 180);
			xGraphicsPath.AddLine(160, 60, 220, 100);
			xGraphicsPath.CloseFigure();
			gfx.DrawPath(xPen, xGraphicsPath);
			base.EndBox(gfx);
		}

		private void DrawPathAlternateAndWinding(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawPath (alternate / winding)");
			XPen pen = new XPen(XColors.Navy, 2.5);
			XGraphicsPath xGraphicsPath = new XGraphicsPath();
			xGraphicsPath.FillMode = XFillMode.Alternate;
			xGraphicsPath.AddLine(10, 130, 10, 40);
			xGraphicsPath.AddBeziers(new XPoint[]
			{
				new XPoint(10.0, 40.0),
				new XPoint(30.0, 0.0),
				new XPoint(40.0, 20.0),
				new XPoint(60.0, 40.0),
				new XPoint(80.0, 60.0),
				new XPoint(100.0, 60.0),
				new XPoint(120.0, 40.0)
			});
			xGraphicsPath.AddLine(120, 40, 120, 130);
			xGraphicsPath.CloseFigure();
			xGraphicsPath.AddEllipse(40, 80, 50, 40);
			gfx.DrawPath(pen, XBrushes.DarkOrange, xGraphicsPath);
			xGraphicsPath = new XGraphicsPath();
			xGraphicsPath.FillMode = XFillMode.Winding;
			xGraphicsPath.AddLine(130, 130, 130, 40);
			xGraphicsPath.AddBeziers(new XPoint[]
			{
				new XPoint(130.0, 40.0),
				new XPoint(150.0, 0.0),
				new XPoint(160.0, 20.0),
				new XPoint(180.0, 40.0),
				new XPoint(200.0, 60.0),
				new XPoint(220.0, 60.0),
				new XPoint(240.0, 40.0)
			});
			xGraphicsPath.AddLine(240, 40, 240, 130);
			xGraphicsPath.CloseFigure();
			xGraphicsPath.AddEllipse(160, 80, 50, 40);
			gfx.DrawPath(pen, XBrushes.DarkOrange, xGraphicsPath);
			base.EndBox(gfx);
		}

		private void DrawGlyphs(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "Draw Glyphs");
			XGraphicsPath xGraphicsPath = new XGraphicsPath();
			xGraphicsPath.AddString("Hello!", new XFontFamily("Times New Roman"), XFontStyle.BoldItalic, 100.0, new XRect(0.0, 0.0, 250.0, 140.0), XStringFormats.Center);
			gfx.DrawPath(new XPen(XColors.Purple, 2.3), XBrushes.DarkOrchid, xGraphicsPath);
			base.EndBox(gfx);
		}

		private void DrawClipPath(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "Clip through Path");
			XGraphicsPath xGraphicsPath = new XGraphicsPath();
			xGraphicsPath.AddString("Clip!", new XFontFamily("Verdana"), XFontStyle.Bold, 90.0, new XRect(0.0, 0.0, 250.0, 140.0), XStringFormats.Center);
			gfx.IntersectClip(xGraphicsPath);
			XPen xPen = XPens.DarkRed.Clone();
			xPen.DashStyle = XDashStyle.Dot;
			for (double num = 0.0; num <= 90.0; num += 0.5)
			{
				gfx.DrawLine(xPen, 0.0, 0.0, 250.0 * Math.Cos(num / 90.0 * 3.1415926535897931), 250.0 * Math.Sin(num / 90.0 * 3.1415926535897931));
			}
			base.EndBox(gfx);
		}
	}
}
