using AForge.Imaging.Filters;
using DocScaner.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DocScanner.ImgUtils
{
    public static class ImageHelper
	{
		private static PixelFormat[] indexedPixelFormats = new PixelFormat[]
		{
			PixelFormat.Undefined,
			PixelFormat.Undefined,
			PixelFormat.Format16bppArgb1555,
			PixelFormat.Format1bppIndexed,
			PixelFormat.Format4bppIndexed,
			PixelFormat.Format8bppIndexed
		};

		private static byte[] rgbValues;

		private static Brush whitebrush = new SolidBrush(Color.Transparent);

		public static Image LoadCorectedImage(string fname)
		{
			Image image = ImageHelper.LoadLocalImage(fname, false);
			float num = (float)image.Width / (float)image.Height;
			bool flag = (double)num < 0.01 || num > 100f;
			if (flag)
			{
				throw new Exception("您加载的图像高宽比例严重失调");
			}
			return image;
		}

		public static Bitmap ToBitmap(this Image img)
		{
			return new Bitmap(img);
		}

		public static Image ScaleByPercent(this Image imgPhoto, float nPercent)
		{
			bool flag = nPercent == 1f;
			Image result;
			if (flag)
			{
				result = imgPhoto;
			}
			else
			{
				int width = imgPhoto.Width;
				int height = imgPhoto.Height;
				int x = 0;
				int y = 0;
				int x2 = 0;
				int y2 = 0;
				int width2 = (int)((float)width * nPercent);
				int height2 = (int)((float)height * nPercent);
				Bitmap bitmap = new Bitmap(width2, height2, imgPhoto.PixelFormat);
				bitmap.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
				Graphics graphics = null;
				try
				{
					graphics = Graphics.FromImage(bitmap);
				}
				catch
				{
					result = imgPhoto;
					return result;
				}
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(imgPhoto, new Rectangle(x2, y2, width2, height2), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
				graphics.Dispose();
				result = bitmap;
			}
			return result;
		}

		public static Image LoadSizedImage(string strSourceFileName, int intWidth, int intHeight, string AddExtString = "")
		{
			bool flag = !File.Exists(strSourceFileName);
			Image result;
			if (flag)
			{
				result = null;
			}
			else
			{
				using (Stream stream = new FileStream(strSourceFileName, FileMode.Open, FileAccess.Read))
				{
					using (Image image = Image.FromStream(stream))
					{
						Image image2 = image.Change2Size(intWidth, intHeight);
						result = image2;
					}
				}
			}
			return result;
		}

		public static bool IsImgExt(string fname)
		{
            String fileExt = FileHelper.GetFileExtension(fname);

            FileExtension.

            bool result;
			if (string.IsNullOrEmpty(fname))
			{
				result = false;
			}
			else
			{
				fname = fname.ToLower();
				string[] array = new string[]
				{
					"ani",
					"anm",
					"bmp",
					"dib",
					"rle",
					"cdr",
					"cur",
					"dcm",
					"emf",
					"gif",
					"hdr",
					"ico",
					"ics",
					"jpg",
					"jpeg",
					"pcd",
					"png",
					"tif",
					"tiff"
				};
				for (int i = 0; i < array.Length; i++)
				{
					bool flag2 = fname.EndsWith(array[i]);
					if (flag2)
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}

		public static Image LoadLocalImage(string fname, bool IntoMemory = true)
		{
			Image result = null;
			FileStream fileStream = null;
			try
			{
				fileStream = File.OpenRead(fname);
				MemoryStream memoryStream = new MemoryStream();
				byte[] array = new byte[10000];
				while (true)
				{
					int num = fileStream.Read(array, 0, array.Length);
					bool flag = num == 0;
					if (flag)
					{
						break;
					}
					memoryStream.Write(array, 0, num);
				}
				result = Image.FromStream(memoryStream);
			}
			finally
			{
				bool flag2 = fileStream != null;
				if (flag2)
				{
					fileStream.Close();
					fileStream.Dispose();
				}
			}
			return result;
		}

		public static Image ImgCut(this Image img, Rectangle Rect)
		{
			Bitmap bitmap = new Bitmap(img);
			Rect = Rect.ToNormalRectangle();
			bool flag = Rect.Width < 1;
			if (flag)
			{
				Rect.Width = 1;
			}
			bool flag2 = Rect.Height < 1;
			if (flag2)
			{
				Rect.Height = 1;
			}
			Bitmap bitmap2 = new Bitmap(Rect.Width, Rect.Height);
			Graphics graphics = Graphics.FromImage(bitmap2);
			try
			{
				graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), Rect, GraphicsUnit.Pixel);
			}
			finally
			{
				graphics.Dispose();
			}
			bitmap.Dispose();
			return bitmap2;
		}

		public static Rectangle ToNormalRectangle(this Rectangle _selection)
		{
			return new Rectangle
			{
				X = ((_selection.Width > 0) ? _selection.X : (_selection.X + _selection.Width)),
				Y = ((_selection.Height > 0) ? _selection.Y : (_selection.Y + _selection.Height)),
				Width = Math.Abs(_selection.Width),
				Height = Math.Abs(_selection.Height)
			};
		}

		public static bool IsBlankImge(this Image image)
		{
			return false;
		}

		public static bool IsBlankImage(string path)
		{
			Image image = ImageHelper.LoadCorectedImage(path);
			bool flag = image != null;
			bool result;
			if (flag)
			{
				bool flag2 = image.IsBlankImge();
				result = flag2;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public static Image Fit2PictureBox(this Image image, PictureBox picBox)
		{
			double num = (double)image.Width / (double)picBox.Width;
			double num2 = (double)image.Height / (double)picBox.Height;
			double num3 = (num < num2) ? num2 : num;
			Bitmap bitmap = new Bitmap((int)((double)image.Width / num3), (int)((double)image.Height / num3));
			bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
			graphics.Dispose();
			image.Dispose();
			return bitmap;
		}

		public static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
		{
			PixelFormat[] array = ImageHelper.indexedPixelFormats;
			bool result;
			for (int i = 0; i < array.Length; i++)
			{
				PixelFormat pixelFormat = array[i];
				bool flag = pixelFormat.Equals(imgPixelFormat);
				if (flag)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static Image Change2Size(this Image img, int Width, int Height)
		{
			return img.GetThumbnailImage(Width, Height, null, IntPtr.Zero);
		}

		public unsafe static Bitmap ToGrayBitmap(this Image img)
		{
			Bitmap bitmap = new Bitmap(img);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			int stride = bitmapData.Stride;
			IntPtr scan = bitmapData.Scan0;
			byte* ptr = (byte*)((void*)scan);
			int num = stride - bitmap.Width * 3;
			for (int i = 0; i < bitmap.Height; i++)
			{
				for (int j = 0; j < bitmap.Width; j++)
				{
					byte b = *ptr;
					byte b2 = ptr[1];
					byte b3 = ptr[2];
					*ptr = (ptr[1] = (ptr[2] = (byte)(0.299 * (double)b3 + 0.587 * (double)b2 + 0.114 * (double)b)));
					ptr += 3;
				}
				ptr += num;
			}
			bitmap.UnlockBits(bitmapData);
			return bitmap;
		}

		public unsafe static Bitmap Threshoding(Bitmap b, byte Threshold)
		{
			int width = b.Width;
			int height = b.Height;
			BitmapData bitmapData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			byte* ptr = (byte*)((void*)bitmapData.Scan0);
			int num = bitmapData.Stride - width * 4;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					byte b2 = ptr[2];
					byte b3 = ptr[1];
					byte b4 = *ptr;
					byte b5 = (byte)((int)b2 * 19595 + (int)b3 * 38469 + (int)b4 * 7472 >> 16);
					bool flag = b5 >= Threshold;
					if (flag)
					{
						*ptr = (ptr[1] = (ptr[2] = 255));
					}
					else
					{
						*ptr = (ptr[1] = (ptr[2] = 0));
					}
					ptr += 4;
				}
				ptr += num;
			}
			b.UnlockBits(bitmapData);
			return b;
		}

		public unsafe static Bitmap OtsuThreshold(this Image img)
		{
			Bitmap bitmap = new Bitmap(img);
			int width = bitmap.Width;
			int height = bitmap.Height;
			byte b = 0;
			int[] array = new int[256];
			int num = 0;
			int num2 = 0;
			double num3 = 0.0;
			double num4 = 0.0;
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			byte* ptr = (byte*)((void*)bitmapData.Scan0);
			int num5 = bitmapData.Stride - width * 4;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					array[(int)(*ptr)]++;
					ptr += 4;
				}
				ptr += num5;
			}
			bitmap.UnlockBits(bitmapData);
			for (int k = 0; k < 256; k++)
			{
				num3 += (double)(k * array[k]);
				num += array[k];
			}
			double num6 = -1.0;
			for (int l = 0; l < 256; l++)
			{
				num2 += array[l];
				int num7 = num - num2;
				bool flag = num7 == 0;
				if (flag)
				{
					break;
				}
				num4 += (double)(l * array[l]);
				double num8 = num3 - num4;
				double num9 = num4 / (double)num2;
				double num10 = num8 / (double)num7;
				double num11 = (double)num2 * num9 * num9 + (double)num7 * num10 * num10;
				bool flag2 = num11 > num6;
				if (flag2)
				{
					num6 = num11;
					b = (byte)l;
				}
			}
			bool flag3 = b == 0;
			if (flag3)
			{
				b = 127;
			}
			return ImageHelper.Threshoding(bitmap, b);
		}

		public unsafe static Bitmap KiContrast(this Image img, int Degree)
		{
			Bitmap bitmap = new Bitmap(img);
			bool flag = bitmap == null;
			Bitmap result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = Degree < -100;
				if (flag2)
				{
					Degree = -100;
				}
				bool flag3 = Degree > 100;
				if (flag3)
				{
					Degree = 100;
				}
				try
				{
					double num = (100.0 + (double)Degree) / 100.0;
					num *= num;
					int width = bitmap.Width;
					int height = bitmap.Height;
					BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
					byte* ptr = (byte*)((void*)bitmapData.Scan0);
					int num2 = bitmapData.Stride - width * 3;
					for (int i = 0; i < height; i++)
					{
						for (int j = 0; j < width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								double num3 = (((double)ptr[k] / 255.0 - 0.5) * num + 0.5) * 255.0;
								bool flag4 = num3 < 0.0;
								if (flag4)
								{
									num3 = 0.0;
								}
								bool flag5 = num3 > 255.0;
								if (flag5)
								{
									num3 = 255.0;
								}
								ptr[k] = (byte)num3;
							}
							ptr += 3;
						}
						ptr += num2;
					}
					bitmap.UnlockBits(bitmapData);
					result = bitmap;
				}
				catch
				{
					result = null;
				}
			}
			return result;
		}

		public unsafe static Image Lightness(this Image img, int Degree)
		{
			Bitmap bitmap = new Bitmap(img);
			bool flag = bitmap == null;
			Image result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = Degree < -255;
				if (flag2)
				{
					Degree = -255;
				}
				bool flag3 = Degree > 255;
				if (flag3)
				{
					Degree = 255;
				}
				int width = bitmap.Width;
				int height = bitmap.Height;
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
				try
				{
					byte* ptr = (byte*)((void*)bitmapData.Scan0);
					int num = bitmapData.Stride - width * 3;
					for (int i = 0; i < height; i++)
					{
						for (int j = 0; j < width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								int val = (int)ptr[k] + Degree;
								bool flag4 = Degree < 0;
								if (flag4)
								{
									ptr[k] = (byte)Math.Max(0, val);
								}
								bool flag5 = Degree > 0;
								if (flag5)
								{
									ptr[k] = (byte)Math.Min(255, val);
								}
							}
							ptr += 3;
						}
						ptr += num;
					}
					bitmap.UnlockBits(bitmapData);
					result = bitmap;
				}
				catch
				{
					result = bitmap;
				}
			}
			return result;
		}

		public static Bitmap AddMosaic(Bitmap bitmap, int effectWidth, Rectangle region)
		{
			for (int i = region.Top; i < region.Bottom; i += effectWidth)
			{
				for (int j = region.Left; j < region.Right; j += effectWidth)
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = j;
					while (num5 < j + effectWidth && num5 < region.Width)
					{
						int num6 = i;
						while (num6 < i + effectWidth && num6 < region.Height)
						{
							Color pixel = bitmap.GetPixel(num5, num6);
							num += (int)pixel.R;
							num2 += (int)pixel.G;
							num3 += (int)pixel.B;
							num4++;
							num6++;
						}
						num5++;
					}
					bool flag = num4 == 0 || num4 == 0 || num4 == 0;
					if (flag)
					{
						break;
					}
					num /= num4;
					num2 /= num4;
					num3 /= num4;
					int num7 = j;
					while (num7 < j + effectWidth && num7 < region.Width)
					{
						int num8 = i;
						while (num8 < i + effectWidth && num8 < region.Height)
						{
							Color color = Color.FromArgb(num, num2, num3);
							bitmap.SetPixel(num7, num8, color);
							num8++;
						}
						num7++;
					}
				}
			}
			return bitmap;
		}

		public unsafe static Bitmap KiMosaic(Bitmap b, int val, Rectangle region)
		{
			bool flag = b.Equals(null);
			Bitmap result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int width = b.Width;
				int height = b.Height;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				BitmapData bitmapData = b.LockBits(new Rectangle(region.Left, region.Top, region.Width, region.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
				byte* ptr = (byte*)bitmapData.Scan0.ToPointer();
				for (int i = 0; i < height; i++)
				{
					for (int j = 0; j < width; j++)
					{
						bool flag2 = i % val == 0;
						if (flag2)
						{
							bool flag3 = j % val == 0;
							if (flag3)
							{
								num = (int)ptr[2];
								num2 = (int)ptr[1];
								num3 = (int)(*ptr);
							}
							else
							{
								*ptr = (byte)num3;
								ptr[1] = (byte)num2;
								ptr[2] = (byte)num;
							}
						}
						else
						{
							byte* ptr2 = ptr - bitmapData.Stride;
							*ptr = *ptr2;
							ptr[1] = ptr2[1];
							ptr[2] = ptr2[2];
						}
						ptr += 3;
					}
					ptr += bitmapData.Stride - width * 3;
				}
				b.UnlockBits(bitmapData);
				result = b;
			}
			return result;
		}

		public static Bitmap SoftImage(this Image img)
		{
			int height = img.Height;
			int width = img.Width;
			Bitmap bitmap = new Bitmap(width, height);
			Bitmap bitmap2 = (Bitmap)img;
			int[] array = new int[]
			{
				1,
				2,
				1,
				2,
				4,
				2,
				1,
				2,
				1
			};
			for (int i = 1; i < width - 1; i++)
			{
				for (int j = 1; j < height - 1; j++)
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					for (int k = -1; k <= 1; k++)
					{
						for (int l = -1; l <= 1; l++)
						{
							Color pixel = bitmap2.GetPixel(i + l, j + k);
							num += (int)pixel.R * array[num4];
							num2 += (int)pixel.G * array[num4];
							num3 += (int)pixel.B * array[num4];
							num4++;
						}
					}
					num /= 16;
					num2 /= 16;
					num3 /= 16;
					num = ((num > 255) ? 255 : num);
					num = ((num < 0) ? 0 : num);
					num2 = ((num2 > 255) ? 255 : num2);
					num2 = ((num2 < 0) ? 0 : num2);
					num3 = ((num3 > 255) ? 255 : num3);
					num3 = ((num3 < 0) ? 0 : num3);
					bitmap.SetPixel(i - 1, j - 1, Color.FromArgb(num, num2, num3));
				}
			}
			return bitmap;
		}

		public static Bitmap SharpImage(this Image img)
		{
			int height = img.Height;
			int width = img.Width;
			Bitmap bitmap = new Bitmap(width, height);
			Bitmap bitmap2 = (Bitmap)img;
			int[] array = new int[]
			{
				-1,
				-1,
				-1,
				-1,
				9,
				-1,
				-1,
				-1,
				-1
			};
			for (int i = 1; i < width - 1; i++)
			{
				for (int j = 1; j < height - 1; j++)
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					for (int k = -1; k <= 1; k++)
					{
						for (int l = -1; l <= 1; l++)
						{
							Color pixel = bitmap2.GetPixel(i + l, j + k);
							num += (int)pixel.R * array[num4];
							num2 += (int)pixel.G * array[num4];
							num3 += (int)pixel.B * array[num4];
							num4++;
						}
					}
					num = ((num > 255) ? 255 : num);
					num = ((num < 0) ? 0 : num);
					num2 = ((num2 > 255) ? 255 : num2);
					num2 = ((num2 < 0) ? 0 : num2);
					num3 = ((num3 > 255) ? 255 : num3);
					num3 = ((num3 < 0) ? 0 : num3);
					bitmap.SetPixel(i - 1, j - 1, Color.FromArgb(num, num2, num3));
				}
			}
			return bitmap;
		}

		public static Bitmap Rotate(this Image im, int angle)
		{
			return ImageHelper.Rotate(new Bitmap(im), angle);
		}

		public static Bitmap Rotate(Bitmap b, int angle)
		{
			angle %= 360;
			double num = (double)angle * 3.1415926535897931 / 180.0;
			double num2 = Math.Cos(num);
			double num3 = Math.Sin(num);
			int width = b.Width;
			int height = b.Height;
			int num4 = (int)Math.Max(Math.Abs((double)width * num2 - (double)height * num3), Math.Abs((double)width * num2 + (double)height * num3));
			int num5 = (int)Math.Max(Math.Abs((double)width * num3 - (double)height * num2), Math.Abs((double)width * num3 + (double)height * num2));
			Bitmap bitmap = new Bitmap(num4, num5);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.InterpolationMode = InterpolationMode.Bilinear;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			Point point = new Point((num4 - width) / 2, (num5 - height) / 2);
			Rectangle rect = new Rectangle(point.X, point.Y, width, height);
			Point point2 = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
			graphics.TranslateTransform((float)point2.X, (float)point2.Y);
			graphics.RotateTransform((float)(360 - angle));
			graphics.TranslateTransform((float)(-(float)point2.X), (float)(-(float)point2.Y));
			graphics.DrawImage(b, rect);
			graphics.ResetTransform();
			graphics.Save();
			graphics.Dispose();
			return bitmap;
		}

		public static Bitmap RemoveBlackEdge(this Image img)
		{
			Bitmap bitmap = new Bitmap(img);
			Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
			int width = bitmapData.Width;
			int height = bitmapData.Height;
			int stride = bitmapData.Stride;
			double picByteSize = ImageHelper.GetPicByteSize(bitmap.PixelFormat);
			int num = (int)Math.Ceiling(picByteSize * (double)width);
			int num2 = stride - num;
			IntPtr scan = bitmapData.Scan0;
			int num3 = stride * height;
			int num4 = 0;
			ImageHelper.rgbValues = new byte[num3];
			Marshal.Copy(scan, ImageHelper.rgbValues, 0, num3);
			int num5 = (int)((double)num * 0.02);
			int num6 = (int)((double)height * 0.02);
			int num7 = (int)picByteSize;
			for (int i = 0; i < height; i++)
			{
				int j;
				for (j = 0; j < num; j += 4)
				{
					bool flag = true;
					bool flag2 = ImageHelper.rgbValues[num4] <= 20;
					if (flag2)
					{
						for (int k = 0; k < 4; k++)
						{
							bool flag3 = num4 + k >= num3;
							if (flag3)
							{
								break;
							}
							bool flag4 = k < 3;
							if (flag4)
							{
								ImageHelper.rgbValues[num4 + k] = 255;
							}
							else
							{
								ImageHelper.rgbValues[num4 + k] = 0;
							}
						}
					}
					bool flag5 = ImageHelper.rgbValues[num4] > 20;
					if (flag5)
					{
						for (int l = 1; l <= num7; l++)
						{
							bool flag6 = num4 + l >= num3;
							if (flag6)
							{
								break;
							}
							bool flag7 = ImageHelper.rgbValues[num4 + l] <= 15;
							if (flag7)
							{
								flag = false;
							}
						}
					}
					bool flag8 = ImageHelper.rgbValues[num4] <= 20 || j >= num / 2;
					if (flag8)
					{
						flag = false;
					}
					ImageHelper.recCheck(ref ImageHelper.rgbValues, num4, height, stride, true);
					bool flag9 = num4 + 4 < num3;
					if (!flag9)
					{
						break;
					}
					num4 += 4;
					bool flag10 = j >= num5 & flag;
					if (flag10)
					{
						break;
					}
				}
				bool flag11 = j == num;
				if (flag11)
				{
					num4 += num2;
				}
				else
				{
					bool flag12 = num4 + num2 + num - j - 1 >= num3;
					if (flag12)
					{
						break;
					}
					num4 += (num2 + num - j - 1) / 4 * 4;
				}
			}
			num4 = (num3 - 1) / 4 * 4;
			for (int i = height - 1; i >= 0; i--)
			{
				num4 -= num2;
				int j;
				for (j = (num - 1) / 4 * 4; j >= 0; j -= 4)
				{
					bool flag = true;
					bool flag13 = ImageHelper.rgbValues[num4] <= 20;
					if (flag13)
					{
						for (int m = 0; m < 4; m++)
						{
							bool flag14 = num4 + m >= num3;
							if (flag14)
							{
								break;
							}
							bool flag15 = m < 3;
							if (flag15)
							{
								ImageHelper.rgbValues[num4 + m] = 255;
							}
							else
							{
								ImageHelper.rgbValues[num4 + m] = 0;
							}
						}
					}
					bool flag16 = ImageHelper.rgbValues[num4] > 20;
					if (flag16)
					{
						for (int n = 1; n <= num7; n++)
						{
							bool flag17 = num4 - n <= 0;
							if (flag17)
							{
								break;
							}
							bool flag18 = ImageHelper.rgbValues[num4 - n] <= 20;
							if (flag18)
							{
								flag = false;
							}
						}
					}
					bool flag19 = ImageHelper.rgbValues[num4] <= 20 || num / 2 > j;
					if (flag19)
					{
						flag = false;
					}
					ImageHelper.recCheck(ref ImageHelper.rgbValues, num4, height, stride, false);
					bool flag20 = num4 - 4 > 0;
					if (!flag20)
					{
						break;
					}
					num4 -= 4;
					bool flag21 = num6 < height - i;
					if (flag21)
					{
						bool flag22 = j < num - num5 & flag;
						if (flag22)
						{
							break;
						}
					}
				}
				bool flag23 = j != -1;
				if (flag23)
				{
					num4 -= j;
				}
			}
			Marshal.Copy(ImageHelper.rgbValues, 0, scan, num3);
			bitmap.UnlockBits(bitmapData);
			return bitmap;
		}

		private static double GetPicByteSize(PixelFormat pixelformat)
		{
			return 4.0;
		}

		private static void recCheck(ref byte[] rgbValues, int posScan, int h, int stride, bool islLeft)
		{
			int num = h * stride;
			int num2 = (int)((double)h * 0.02);
			for (int i = 1; i <= num2; i++)
			{
				int num3 = 0;
				bool flag = islLeft && posScan - stride * i > 0;
				if (flag)
				{
					num3 = posScan - stride * i / 4 * 4;
				}
				else
				{
					bool flag2 = !islLeft && posScan + stride * i < num;
					if (flag2)
					{
						num3 = posScan + stride * i / 4 * 4;
					}
				}
				bool flag3 = rgbValues[num3] <= 20;
				if (!flag3)
				{
					break;
				}
				for (int j = 0; j < 4; j++)
				{
					bool flag4 = num3 + j >= num;
					if (flag4)
					{
						break;
					}
					bool flag5 = j < 3;
					if (flag5)
					{
						rgbValues[num3 + j] = 255;
					}
					else
					{
						rgbValues[num3 + j] = 0;
					}
				}
			}
		}

		private static bool ArgbEqual(Color cr1, Color cr2)
		{
			return cr1.A == cr2.A && cr1.R == cr2.R && cr1.G == cr2.G && cr1.B == cr2.B;
		}

		private static Dictionary<string, Point> GetBlock(Bitmap bm, int x, int y)
		{
			Dictionary<string, Point> dictionary = new Dictionary<string, Point>();
			Stack<Point> stack = new Stack<Point>();
			Color pixel = bm.GetPixel(x, y);
			bool flag = ImageHelper.ArgbEqual(pixel, Color.White);
			Dictionary<string, Point> result;
			if (flag)
			{
				result = dictionary;
			}
			else
			{
				stack.Push(new Point(x, y));
				while (stack.Count != 0)
				{
					Point point = stack.Pop();
					string key = point.X + "#" + point.Y;
					dictionary[key] = new Point(point.X, point.Y);
					List<Point> list = new List<Point>();
					Point item = new Point(point.X + 1, point.Y);
					bool flag2 = item.X < bm.Width;
					if (flag2)
					{
						Color pixel2 = bm.GetPixel(item.X, item.Y);
						bool flag3 = ImageHelper.ArgbEqual(pixel2, Color.Black);
						if (flag3)
						{
							list.Add(item);
						}
					}
					item = new Point(point.X - 1, point.Y);
					bool flag4 = item.X >= 0;
					if (flag4)
					{
						Color pixel3 = bm.GetPixel(item.X, item.Y);
						bool flag5 = ImageHelper.ArgbEqual(pixel3, Color.Black);
						if (flag5)
						{
							list.Add(item);
						}
					}
					item = new Point(point.X, point.Y + 1);
					bool flag6 = item.Y < bm.Height;
					if (flag6)
					{
						Color pixel4 = bm.GetPixel(item.X, item.Y);
						bool flag7 = ImageHelper.ArgbEqual(pixel4, Color.Black);
						if (flag7)
						{
							list.Add(item);
						}
					}
					item = new Point(point.X, point.Y - 1);
					bool flag8 = item.Y >= 0;
					if (flag8)
					{
						Color pixel5 = bm.GetPixel(item.X, item.Y);
						bool flag9 = ImageHelper.ArgbEqual(pixel5, Color.Black);
						if (flag9)
						{
							list.Add(item);
						}
					}
					for (int i = 0; i < list.Count; i++)
					{
						Point item2 = list[i];
						key = item2.X + "#" + item2.Y;
						bool flag10 = !dictionary.ContainsKey(key);
						if (flag10)
						{
							stack.Push(item2);
						}
					}
				}
				result = dictionary;
			}
			return result;
		}

		public static Bitmap RemoveBlock(this Image img, int nBlockSize)
		{
			Bitmap result;
			using (Bitmap bitmap = new Bitmap(img))
			{
				result = ImageHelper.RemoveBlock(bitmap, nBlockSize);
			}
			return result;
		}

		public static Bitmap RemoveBlock(Bitmap bm, int nBlockSize)
		{
			Dictionary<string, Point> dictionary = new Dictionary<string, Point>();
			for (int i = 0; i < bm.Width; i++)
			{
				for (int j = 0; j < bm.Height; j++)
				{
					bool flag = dictionary.ContainsKey(i + "#" + j);
					if (!flag)
					{
						Dictionary<string, Point> block = ImageHelper.GetBlock(bm, i, j);
						foreach (string current in block.Keys)
						{
							bool flag2 = dictionary.ContainsKey(current);
							if (flag2)
							{
								break;
							}
							dictionary.Add(current, block[current]);
						}
						bool flag3 = block.Count < nBlockSize;
						if (flag3)
						{
							foreach (KeyValuePair<string, Point> current2 in block)
							{
								Point value = current2.Value;
								bm.SetPixel(value.X, value.Y, Color.White);
							}
						}
					}
				}
			}
			return bm;
		}

		public static Bitmap ConvertAforgeSupportImg(Bitmap b)
		{
			return new Bitmap(b.Width, b.Height, PixelFormat.Format24bppRgb);
		}

		public static Bitmap ToGrayBitmapConvertGray(Bitmap b)
		{
			b = new Grayscale(0.2125, 0.7154, 0.0721).Apply(b);
			return b;
		}

		public static Bitmap ConvertDual(Bitmap b)
		{
			b = new Threshold(50).Apply(b);
			return b;
		}

		public static Bitmap ConvertRemoveChaos(Bitmap b)
		{
			return new BlobsFiltering(1, 1, b.Width, b.Height).Apply(b);
		}

		public static Bitmap AutoDeskew(this Image In)
		{
			Bitmap bitmap = new Bitmap(In);
			ImgRectify imgRectify = new ImgRectify(bitmap);
			double skewAngle = imgRectify.GetSkewAngle();
			Bitmap bitmap2 = ImageHelper.Rotate(bitmap, (int)skewAngle);
			Bitmap image = (Bitmap)bitmap2.Clone();
			Graphics graphics = Graphics.FromImage(bitmap2);
			graphics.FillRectangle(ImageHelper.whitebrush, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height));
			graphics.DrawImage(image, new Point(0, 0));
			return bitmap2;
		}

		public static Bitmap GetScreenBitmap()
		{
			Screen primaryScreen = Screen.PrimaryScreen;
			int width = primaryScreen.Bounds.Width;
			int height = primaryScreen.Bounds.Height;
			Bitmap bitmap = new Bitmap(width, height);
			Graphics graphics = Graphics.FromImage(bitmap);
			Bitmap result;
			try
			{
				graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(width, height));
				result = bitmap;
			}
			finally
			{
				graphics.Dispose();
			}
			return result;
		}
	}
}
