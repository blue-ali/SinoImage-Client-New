using System;
using System.Drawing.Imaging;

namespace DocScanner.Bean
{
	public static class EImgType2ImageFormat
	{
		public static ImageFormat ToImageFormat(this EImgType type)
		{
			bool flag = type == EImgType.bmp;
			ImageFormat result;
			if (flag)
			{
				result = ImageFormat.Bmp;
			}
			else
			{
				bool flag2 = type == EImgType.jpeg;
				if (flag2)
				{
					result = ImageFormat.Jpeg;
				}
				else
				{
					bool flag3 = type == EImgType.tiff;
					if (flag3)
					{
						result = ImageFormat.Tiff;
					}
					else
					{
						bool flag4 = type == EImgType.png;
						if (!flag4)
						{
							throw new Exception("not support file types");
						}
						result = ImageFormat.Png;
					}
				}
			}
			return result;
		}

		public static ImageCodecInfo ToImageCodecInfo(this EImgType type)
		{
			string text = string.Empty;
			bool flag = type == EImgType.bmp;
			if (flag)
			{
				text = "image/bmp";
			}
			else
			{
				bool flag2 = type == EImgType.jpeg;
				if (flag2)
				{
					text = "image/jpeg";
				}
				else
				{
					bool flag3 = type == EImgType.gif;
					if (flag3)
					{
						text = "image/gif";
					}
					else
					{
						bool flag4 = type == EImgType.tiff;
						if (flag4)
						{
							text = "image/tiff";
						}
						else
						{
							bool flag5 = type == EImgType.png;
							if (flag5)
							{
								text = "image/png";
							}
						}
					}
				}
			}
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo[] array = imageEncoders;
			ImageCodecInfo result;
			for (int i = 0; i < array.Length; i++)
			{
				ImageCodecInfo imageCodecInfo = array[i];
				bool flag6 = imageCodecInfo.MimeType.ToLower().Equals(text.ToLower());
				if (flag6)
				{
					result = imageCodecInfo;
					return result;
				}
			}
			result = null;
			return result;
		}

		public static ImageCodecInfo ToImageCodecInfo(this string Extension)
		{
			string value = string.Empty;
			bool flag = Extension == ".bmp";
			if (flag)
			{
				value = "image/bmp";
			}
			else
			{
				bool flag2 = Extension == ".jpeg" || Extension == ".jpg" || Extension == ".JPG";
				if (flag2)
				{
					value = "image/jpeg";
				}
				else
				{
					bool flag3 = Extension == ".gif";
					if (flag3)
					{
						value = "image/gif";
					}
					else
					{
						bool flag4 = Extension == ".tiff";
						if (flag4)
						{
							value = "image/tiff";
						}
						else
						{
							bool flag5 = Extension == ".png";
							if (flag5)
							{
								value = "image/png";
							}
						}
					}
				}
			}
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo[] array = imageEncoders;
			ImageCodecInfo result;
			for (int i = 0; i < array.Length; i++)
			{
				ImageCodecInfo imageCodecInfo = array[i];
				bool flag6 = imageCodecInfo.MimeType.Equals(value);
				if (flag6)
				{
					result = imageCodecInfo;
					return result;
				}
			}
			result = null;
			return result;
		}
	}
}
