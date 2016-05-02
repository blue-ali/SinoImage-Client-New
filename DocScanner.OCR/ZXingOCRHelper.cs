using DocScaner.Common;
using Logos.DocScaner.Common;
using System;
using System.Drawing;
using System.IO;
using ZXing;

namespace DocScaner.OCR.Zxing
{
    public class ZXingOCRHelper
	{
		public static Result ParseImgBarCode(string fpath)
		{
			bool flag = !File.Exists(fpath);
			if (flag)
			{
				throw new Exception("文件不存在" + fpath);
			}
			bool flag2 = !FileHelper.IsImageExt(fpath);
			if (flag2)
			{
				throw new Exception("非图片文件" + fpath);
			}
			Result result2;
			using (Bitmap bitmap = (Bitmap)Image.FromFile(fpath))
			{
				Result result = ZXingOCRHelper.ParseImgBarCode(bitmap);
				result2 = result;
			}
			return result2;
		}

		public static Result ParseImgBarCode(Bitmap barcodeBitmap)
		{
			return ((IBarcodeReader)new BarcodeReader
			{
				Options = 
				{
					TryHarder = true,
					PossibleFormats = 
					{
						BarcodeFormat.EAN_13
					}
				}
			}).Decode(barcodeBitmap);
		}
	}
}
