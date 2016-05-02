using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;

namespace DocScaner.PDF.Utils
{
	public class Images : Base
	{
		private const string jpegSamplePath = "../../../../../../dev/XGraphicsLab/images/Z3.jpg";

		private const string gifSamplePath = "../../../../../../dev/XGraphicsLab/images/Test.gif";

		private const string pngSamplePath = "../../../../../../dev/XGraphicsLab/images/Test.png";

		private const string tiffSamplePath = "../../../../../../dev/XGraphicsLab/images/Rose (RGB 8).tif";

		private const string pdfSamplePath = "../../../../../PDFs/SomeLayout.pdf";

		public void DrawPage(PdfPage page)
		{
			XGraphics gfx = XGraphics.FromPdfPage(page);
			base.DrawTitle(page, gfx, "Images");
			this.DrawImage(gfx, 1);
			this.DrawImageScaled(gfx, 2);
			this.DrawImageRotated(gfx, 3);
			this.DrawImageSheared(gfx, 4);
			this.DrawGif(gfx, 5);
			this.DrawPng(gfx, 6);
			this.DrawTiff(gfx, 7);
			this.DrawFormXObject(gfx, 8);
		}

		private void DrawImage(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawImage (original)");
			XImage xImage = XImage.FromFile("../../../../../../dev/XGraphicsLab/images/Z3.jpg");
			double x = (250.0 - (double)(xImage.PixelWidth * 72) / xImage.HorizontalResolution) / 2.0;
			gfx.DrawImage(xImage, x, 0.0);
			base.EndBox(gfx);
		}

		private void DrawImageScaled(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawImage (scaled)");
			XImage image = XImage.FromFile("../../../../../../dev/XGraphicsLab/images/Z3.jpg");
			gfx.DrawImage(image, 0, 0, 250, 140);
			base.EndBox(gfx);
		}

		private void DrawImageRotated(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawImage (rotated)");
			XImage xImage = XImage.FromFile("../../../../../../dev/XGraphicsLab/images/Z3.jpg");
			gfx.TranslateTransform(125.0, 70.0);
			gfx.ScaleTransform(0.7);
			gfx.RotateTransform(-25.0);
			gfx.TranslateTransform(-125.0, -70.0);
			double num = (double)(xImage.PixelWidth * 72) / xImage.HorizontalResolution;
			double height = (double)(xImage.PixelHeight * 72) / xImage.HorizontalResolution;
			gfx.DrawImage(xImage, (250.0 - num) / 2.0, 0.0, num, height);
			base.EndBox(gfx);
		}

		private void DrawImageSheared(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawImage (sheared)");
			XImage xImage = XImage.FromFile("../../../../../../dev/XGraphicsLab/images/Z3.jpg");
			gfx.TranslateTransform(125.0, 70.0);
			gfx.ScaleTransform(-0.7, 0.7);
			gfx.ShearTransform(-0.4, -0.3);
			gfx.TranslateTransform(-125.0, -70.0);
			double num = (double)(xImage.PixelWidth * 72) / xImage.HorizontalResolution;
			double height = (double)(xImage.PixelHeight * 72) / xImage.HorizontalResolution;
			gfx.DrawImage(xImage, (250.0 - num) / 2.0, 0.0, num, height);
			base.EndBox(gfx);
		}

		private void DrawGif(XGraphics gfx, int number)
		{
			this.backColor = XColors.LightGoldenrodYellow;
			this.borderPen = new XPen(XColor.FromArgb(202, 121, 74), this.borderWidth);
			base.BeginBox(gfx, number, "DrawImage (GIF)");
			XImage xImage = XImage.FromFile("../../../../../../dev/XGraphicsLab/images/Test.gif");
			double num = (double)(xImage.PixelWidth * 72) / xImage.HorizontalResolution;
			double num2 = (double)(xImage.PixelHeight * 72) / xImage.HorizontalResolution;
			gfx.DrawImage(xImage, (250.0 - num) / 2.0, (140.0 - num2) / 2.0, num, num2);
			base.EndBox(gfx);
		}

		private void DrawPng(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawImage (PNG)");
			XImage xImage = XImage.FromFile("../../../../../../dev/XGraphicsLab/images/Test.png");
			double num = (double)(xImage.PixelWidth * 72) / xImage.HorizontalResolution;
			double num2 = (double)(xImage.PixelHeight * 72) / xImage.HorizontalResolution;
			gfx.DrawImage(xImage, (250.0 - num) / 2.0, (140.0 - num2) / 2.0, num, num2);
			base.EndBox(gfx);
		}

		private void DrawTiff(XGraphics gfx, int number)
		{
			XColor backColor = this.backColor;
			this.backColor = XColors.LightGoldenrodYellow;
			base.BeginBox(gfx, number, "DrawImage (TIFF)");
			XImage xImage = XImage.FromFile("../../../../../../dev/XGraphicsLab/images/Rose (RGB 8).tif");
			double num = (double)(xImage.PixelWidth * 72) / xImage.HorizontalResolution;
			double num2 = (double)(xImage.PixelHeight * 72) / xImage.HorizontalResolution;
			gfx.DrawImage(xImage, (250.0 - num) / 2.0, (140.0 - num2) / 2.0, num, num2);
			base.EndBox(gfx);
			this.backColor = backColor;
		}

		private void DrawFormXObject(XGraphics gfx, int number)
		{
			base.BeginBox(gfx, number, "DrawImage (Form XObject)");
			XImage xImage = XImage.FromFile("../../../../../PDFs/SomeLayout.pdf");
			gfx.TranslateTransform(125.0, 70.0);
			gfx.ScaleTransform(0.35);
			gfx.TranslateTransform(-125.0, -70.0);
			double num = (double)(xImage.PixelWidth * 72) / xImage.HorizontalResolution;
			double num2 = (double)(xImage.PixelHeight * 72) / xImage.HorizontalResolution;
			gfx.DrawImage(xImage, (250.0 - num) / 2.0, (140.0 - num2) / 2.0, num, num2);
			base.EndBox(gfx);
		}
	}
}
