using DocScanner.LibCommon;
using System;
using System.Drawing.Imaging;

namespace DocScanner.ImgUtils
{
    public static class EMostImageTypeHelper
	{
		public static EMostImageType GetImageType(string fname)
		{
			fname = fname.ToLower();
			bool flag = fname.EndsWith("bmp");
			EMostImageType result;
			if (flag)
			{
				result = EMostImageType.Bmp;
			}
			else
			{
				bool flag2 = fname.EndsWith("jpg") || fname.EndsWith("jpeg");
				if (flag2)
				{
					result = EMostImageType.Jpg;
				}
				else
				{
					bool flag3 = fname.EndsWith("tiff") || fname.EndsWith("tif");
					if (!flag3)
					{
						throw new Exception();
					}
					result = EMostImageType.Tiff;
				}
			}
			return result;
		}

		public static string GetImageExt(EMostImageType imagtype)
		{
			bool flag = imagtype == EMostImageType.Bmp;
			string result;
			if (flag)
			{
				result = ".bmp";
			}
			else
			{
				bool flag2 = imagtype == EMostImageType.Jpg;
				if (flag2)
				{
					result = ".jpg";
				}
				else
				{
					bool flag3 = imagtype == EMostImageType.Tiff;
					if (!flag3)
					{
						throw new Exception();
					}
					result = ".tiff";
				}
			}
			return result;
		}

		public static ImageFormat GetSysImgFmt(EMostImageType imagtype)
		{
			bool flag = imagtype == EMostImageType.Bmp;
			ImageFormat result;
			if (flag)
			{
				result = ImageFormat.Bmp;
			}
			else
			{
				bool flag2 = imagtype == EMostImageType.Jpg;
				if (flag2)
				{
					result = ImageFormat.Jpeg;
				}
				else
				{
					bool flag3 = imagtype == EMostImageType.Tiff;
					if (!flag3)
					{
						throw new Exception();
					}
					result = ImageFormat.Tiff;
				}
			}
			return result;
		}
	}
}
