using DocScanner.ImgUtils;
using DocScanner.LibCommon;
using System.Drawing;
using Tesseract;

namespace DocScaner.OCR.Tesseract
{
    public static class TSOCRHelper
	{

        static Page page = null;

        public static string Parse(string img, string language)
		{
			string result;
			using (Bitmap bitmap = new Bitmap(img))
			{
				Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
				result = TSOCRHelper.Parse(bitmap, language, rect);
			}
			return result;
		}

		public static string Parse(string img, string language, Rectangle rect)
		{
			string result;
			using (Bitmap bitmap = new Bitmap(img))
			{
				result = TSOCRHelper.Parse(bitmap, language, rect);
			}
			return result;
		}

		public static string Parse(Bitmap bitmap, string language, Rectangle rect)
		{

            //TesseractEngine tesseractEngine = new TesseractEngine("C:\\workspace\\space_imageclient\\SinoImage-Client\\DocScanner.Main\\bin\\Debug\\Tesseract\\tessdata", "eng", EngineMode.Default);
            //bitmap = new Bitmap("f:\\testimage\\number.jpg");
            //Rect region = new Rect(rect.X, rect.Y, rect.Width, rect.Height);
            string datapath = SystemHelper.GetAssemblesDirectory() + "Tesseract\\tessdata";
            TesseractEngine tesseractEngine = new TesseractEngine(datapath, language.ToString(), EngineMode.Default);
            page = tesseractEngine.Process(bitmap);
            //Page page = tesseractEngine.Process(bitmap, Rect.FromCoords(rect.X, rect.Y, rect.Width, rect.Height));
            page.RegionOfInterest = Rect.FromCoords(rect.X, rect.Y, rect.X+rect.Width, rect.Y+rect.Height);
            //string a = page.GetText();
            //         Bitmap bitmap1 = ImageHelper.LoadCorectedImage("f:\\testimage\\number.jpg").ToBitmap();
            //         Page page = tesseractEngine.Process(bitmap);
            //Page page = tesseractEngine.Process(bitmap, Rect.FromCoords(rect.X, rect.Y, rect.Width, rect.Height));
            return page.GetText();
		}
	}
}
