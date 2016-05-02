using PdfSharp.Pdf;
using System;
using System.Diagnostics;

namespace DocScaner.PDF.Utils
{
	internal class Program
	{
		internal static PdfDocument s_document;

		private static void Main()
		{
			string text = string.Format("{0}_tempfile.pdf", Guid.NewGuid().ToString("D").ToUpper());
			Program.s_document = new PdfDocument();
			Program.s_document.Info.Title = "PDFsharp XGraphic Sample";
			Program.s_document.Info.Author = "Stefan Lange";
			Program.s_document.Info.Subject = "Created with code snippets that show the use of graphical functions";
			Program.s_document.Info.Keywords = "PDFsharp, XGraphics";
			new LinesAndCurves().DrawPage(Program.s_document.AddPage());
			new Shapes().DrawPage(Program.s_document.AddPage());
			new Paths().DrawPage(Program.s_document.AddPage());
			new Text().DrawPage(Program.s_document.AddPage());
			new Images().DrawPage(Program.s_document.AddPage());
			Program.s_document.Save(text);
			Process.Start(text);
		}
	}
}
