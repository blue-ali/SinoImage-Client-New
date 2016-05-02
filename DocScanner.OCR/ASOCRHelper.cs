using System;
using System.Runtime.InteropServices;

namespace DocScaner.OCR.Asprise
{
	public class ASOCRHelper
	{
		public static string ParseImage(string image, int startx, int starty, int width, int height)
		{
			IntPtr ptr = ASOCRHelper.OCRpart(image, -1, startx, starty, width, height);
			return Marshal.PtrToStringAnsi(ptr);
		}

		[DllImport("AspriseOCR.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr OCR(string file, int type);

		[DllImport("AspriseOCR.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr OCRpart(string file, int type, int startX, int startY, int width, int height);

		[DllImport("AspriseOCR.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr OCRBarCodes(string file, int type);

		[DllImport("AspriseOCR.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr OCRpartBarCodes(string file, int type, int startX, int startY, int width, int height);
	}
}
