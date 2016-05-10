using PdfSharp.Drawing;
using PdfSharp.Pdf;
using DocScanner.Bean;
using DocScanner.LibCommon.Util;

namespace DocScaner.PDF.Utils
{
    public class PDFHelper
	{
		public static bool Img2PDF(string imgfname, string pdffname)
		{
			PdfDocument pdfDocument = new PdfDocument();
			bool flag = FileHelper.IsImageExt(imgfname);
			if (flag)
			{
				PdfPage page = pdfDocument.AddPage();
				XGraphics xGraphics = XGraphics.FromPdfPage(page);
				XImage xImage = XImage.FromFile(imgfname);
				xGraphics.DrawImage(xImage, 0.0, 0.0, xImage.Width, xImage.Height);
			}
			pdfDocument.Save(pdffname);
			return true;
		}

		public static bool Batch2PDF(NBatchInfo batch, string pdffname)
		{
			PdfDocument pdfDocument = new PdfDocument();
			pdfDocument.Info.Title = batch.BatchNO;
			pdfDocument.Info.Author = batch.Author;
			pdfDocument.Info.Subject = batch.Title;
			foreach (NFileInfo current in batch.FileInfos)
			{
				bool flag = FileHelper.IsImageExt(current.LocalPath);
				if (flag)
				{
					PdfPage page = pdfDocument.AddPage();
					XGraphics xGraphics = XGraphics.FromPdfPage(page);
					XImage xImage = XImage.FromFile(current.LocalPath);
					xGraphics.DrawImage(xImage, 0.0, 0.0, xImage.Width, xImage.Height);
				}
			}
			pdfDocument.Save(pdffname);
			return true;
		}
	}
}
