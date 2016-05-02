using DocScaner.OCR.Tesseract;
using DocScanner.CodeUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using ZXing;

namespace DocScanner.OCR
{
    public class OCRMgr
	{
		public enum Lang
		{
			eng,
			chi_sim,
			barcode
		}

		private static Dictionary<CnOCRType, Func<Bitmap, Rectangle, string>> _parsers;

		public static string Parse(CnOCRType type, Bitmap bitmap, Rectangle rc)
		{
			return OCRMgr._parsers[type](bitmap, rc);
		}

		static OCRMgr()
		{
			bool flag = OCRMgr._parsers == null;
			if (flag)
			{
				OCRMgr._parsers = new Dictionary<CnOCRType, Func<Bitmap, Rectangle, string>>();
			}
			OCRMgr._parsers[CnOCRType.None] = new Func<Bitmap, Rectangle, string>(OCRMgr.mapNoneParse);
			OCRMgr._parsers[CnOCRType.条形_二维码] = new Func<Bitmap, Rectangle, string>(OCRMgr.mapBarParse);
			OCRMgr._parsers[CnOCRType.数字] = new Func<Bitmap, Rectangle, string>(OCRMgr.mapNumParse);
			OCRMgr._parsers[CnOCRType.字母] = new Func<Bitmap, Rectangle, string>(OCRMgr.mapAlphetparse);
			OCRMgr._parsers[CnOCRType.中文金额] = new Func<Bitmap, Rectangle, string>(OCRMgr.mapCnCashParse);
			OCRMgr._parsers[CnOCRType.中文] = new Func<Bitmap, Rectangle, string>(OCRMgr.mapChineseParse);
		}

		private static string mapNoneParse(Bitmap bitmap, Rectangle re)
		{
			return string.Empty;
		}

		public static string mapBarParse(Bitmap bitmap, Rectangle rc)
		{
			return OCRMgr.Bar_Parse(bitmap);
		}

		public static string mapNumParse(Bitmap bitmap, Rectangle rc)
		{
			return OCRMgr.TSORC_Parse(bitmap, OCRMgr.Lang.eng, rc);
		}

		public static string mapAlphetparse(Bitmap bitmap, Rectangle rc)
		{
			return OCRMgr.TSORC_Parse(bitmap, OCRMgr.Lang.eng, rc);
		}

		public static string mapCnCashParse(Bitmap bitmap, Rectangle rc)
		{
			return OCRMgr.TSORC_Parse(bitmap, OCRMgr.Lang.chi_sim, rc);
		}

		public static string mapChineseParse(Bitmap bitmap, Rectangle rc)
		{
			return OCRMgr.TSORC_Parse(bitmap, OCRMgr.Lang.chi_sim, rc);
		}

		public static string TSORC_Parse(Bitmap bitmap, OCRMgr.Lang lan, Rectangle rc)
		{
			return TSOCRHelper.Parse(bitmap, lan.ToString(), rc);
		}

		public static string Bar_Parse(Bitmap bitmap)
		{
			Result result = ((IBarcodeReader)new BarcodeReader
			{
				Options = 
				{
					TryHarder = true
				}
			}).Decode(bitmap);
			bool flag = result != null;
			string result2;
			if (flag)
			{
				bool flag2 = result.Text.Length > 0 && result.Text[0] > '\u0080' && result.Text[0] < 'Ā';
				if (flag2)
				{
					string text = LanguagyCodeHelper.FormatZXingString(result.Text);
					result2 = text;
				}
				else
				{
					string text2 = result.Text;
					result2 = text2;
				}
			}
			else
			{
				result2 = string.Empty;
			}
			return result2;
		}
	}
}
