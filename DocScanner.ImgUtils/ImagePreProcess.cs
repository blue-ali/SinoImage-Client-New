using System;
using System.Drawing;

namespace DocScanner.ImgUtils
{
	internal class ImagePreProcess
	{
		private void Gray(Bitmap bitmap, Rectangle rect, double Rrate = 0.3, double Grate = 0.59, double Brate = 0.11)
		{
			int y = rect.Y;
			int height = rect.Height;
			int x = rect.X;
			int width = rect.Width;
			for (int i = y; i < height; i++)
			{
				for (int j = x; j < width; j++)
				{
					Color pixel = bitmap.GetPixel(j, i);
					int num = (int)(Rrate * (double)pixel.R + Grate * (double)pixel.G + Brate * (double)pixel.B);
					Color color = Color.FromArgb(num, num, num);
					bitmap.SetPixel(j, i, color);
				}
			}
		}

		private void ReverseGray(Bitmap bitmap, Rectangle rect)
		{
			int y = rect.Y;
			int height = rect.Height;
			int x = rect.X;
			int width = rect.Width;
			for (int i = y; i < height; i++)
			{
				for (int j = x; j < width; j++)
				{
					Color pixel = bitmap.GetPixel(j, i);
					Color color = Color.FromArgb((int)(255 - pixel.R), (int)(255 - pixel.G), (int)(255 - pixel.B));
					bitmap.SetPixel(j, i, color);
				}
			}
		}

		private void ToBlackWihte(Bitmap bitmap, Rectangle rect, int average)
		{
			int y = rect.Y;
			int height = rect.Height;
			int x = rect.X;
			int width = rect.Width;
			for (int i = y; i < height; i++)
			{
				for (int j = x; j < width; j++)
				{
					int num = (int)(255 - bitmap.GetPixel(j, i).B);
					bool flag = num > average;
					if (flag)
					{
						Color color = Color.FromArgb(0, 0, 0);
						bitmap.SetPixel(j, i, color);
					}
					else
					{
						Color color2 = Color.FromArgb(255, 255, 255);
						bitmap.SetPixel(j, i, color2);
					}
				}
			}
		}
	}
}
