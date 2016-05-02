using DocScanner.ImgUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace DocScanner.OCR
{
    public class TestClass
    {
        public void Main()
        {
            //C:\workspace\space_imageclient\SinoImage-Client\DocScanner.Main\bin\Debug\Tesseract
            using (var engine = new TesseractEngine("C:\\workspace\\space_imageclient\\SinoImage-Client\\DocScanner.Main\\bin\\Debug\\Tesseract\\tessdata", "eng", EngineMode.Default))
            //using (var engine = new TesseractEngine(@"C:\\Users\\Administrator\\Desktop\\tesseract-master\\src\\Tesseract.Tests\\tessdata", "eng", EngineMode.Default))
            {
                 using (var img = new Bitmap("C:\\Users\\Administrator\\Desktop\\tesseract-master\\src\\Tesseract.Tests\\phototest.tif"))
                // using (var img = new Bitmap("f:\\testimage\\1111.jpg"))
                //using (var img = PixConverter.ToPix(new Bitmap("f:\\testimage\\1111.jpg")))
                {
                    //using (var page = engine.Process(img, Rect.FromCoords(0, 0, img.Width, img.Height)))

                    using (var page = engine.Process(img, Rect.FromCoords(0, 0, img.Width, 188)))
                    {
                        var region1Text = page.GetText();
                        Console.WriteLine("text: " + region1Text);
                        const string expectedTextRegion1 =
                            "This is a lot of 12 point text to test the\ncor code and see if it works on all types\nof file format.\n\n";

                        // Assert.That(region1Text, Is.EqualTo(expectedTextRegion1));
                        page.RegionOfInterest = Rect.FromCoords(0, 188, img.Width, img.Height-188);

                        var region2Text = page.GetText();
                        Console.WriteLine("text: " + region2Text);
                        const string expectedTextRegion2 =
                            "The quick brown dog jumped over the\nlazy fox. The quick brown dog jumped\nover the lazy fox. The quick brown dog\njumped over the lazy fox. The quick\nbrown dog jumped over the lazy fox.\n\n";

                        // Assert.That(region2Text, Is.EqualTo(expectedTextRegion2));
                    }
                }
            }
        }

        public void test()
        {
            TesseractEngine tesseractEngine = new TesseractEngine("C:\\workspace\\space_imageclient\\SinoImage-Client\\DocScanner.Main\\bin\\Debug\\Tesseract\\tessdata", "eng", EngineMode.Default);
            Bitmap bitmap = new Bitmap("f:\\testimage\\number.jpg");
            Page page = tesseractEngine.Process(bitmap);
            string a = page.GetText();
        }
    }
}
