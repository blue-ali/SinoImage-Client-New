using System;
using System.Diagnostics;
using System.Drawing;

namespace DocScanner.ImgUtils
{
	public class ImgRectify
	{
		public class HougLine
		{
			public int Count;

			public int Index;

			public double Alpha;

			public double d;
		}

		private Bitmap cBmp;

		private double cAlphaStart;

		private double cAlphaStep;

		private int cSteps;

		private double[] cSinA;

		private double[] cCosA;

		private double cDMin;

		private double cDStep;

		private int cDCount;

		private int[] cHMatrix;

		public double GetSkewAngle()
		{
			this.Calc();
			ImgRectify.HougLine[] top = this.GetTop(20);
			int num = 0;
			double num2 = 0.0;
			int num3 = 0;
			do
			{
				num2 += top[num].Alpha;
				checked
				{
					num3++;
					num++;
				}
			}
			while (num <= 19);
			return num2 / (double)num3;
		}

		private ImgRectify.HougLine[] GetTop(int Count)
		{
			checked
			{
				ImgRectify.HougLine[] array = new ImgRectify.HougLine[Count + 1];
				int num = Count - 1;
				for (int i = 0; i <= num; i++)
				{
					array[i] = new ImgRectify.HougLine();
				}
				int num2 = this.cHMatrix.Length - 1;
				for (int j = 0; j <= num2; j++)
				{
					bool flag = this.cHMatrix[j] > array[Count - 1].Count;
					if (flag)
					{
						array[Count - 1].Count = this.cHMatrix[j];
						array[Count - 1].Index = j;
						int num3 = Count - 1;
						while (num3 > 0 && array[num3].Count > array[num3 - 1].Count)
						{
							ImgRectify.HougLine hougLine = array[num3];
							array[num3] = array[num3 - 1];
							array[num3 - 1] = hougLine;
							num3--;
						}
					}
				}
				int num4 = Count - 1;
				for (int k = 0; k <= num4; k++)
				{
					int num5 = array[k].Index / this.cSteps;
					int index = array[k].Index - num5 * this.cSteps;
					array[k].Alpha = this.GetAlpha(index);
					array[k].d = unchecked((double)num5 + this.cDMin);
				}
				return array;
			}
		}

		public ImgRectify(Bitmap bmp)
		{
			this.cAlphaStart = -20.0;
			this.cAlphaStep = 0.2;
			this.cSteps = 200;
			this.cDStep = 1.0;
			this.cBmp = bmp;
		}

		private void Calc()
		{
			checked
			{
				int num = (int)Math.Round((double)this.cBmp.Height / 4.0);
				int num2 = (int)Math.Round((double)(this.cBmp.Height * 3) / 4.0);
				this.Init();
				int num3 = num;
				int num4 = num2;
				for (int i = num3; i <= num4; i++)
				{
					int num5 = this.cBmp.Width - 2;
					for (int j = 1; j <= num5; j++)
					{
						bool flag = this.IsBlack(j, i);
						if (flag)
						{
							bool flag2 = !this.IsBlack(j, i + 1);
							if (flag2)
							{
								this.Calc(j, i);
							}
						}
					}
				}
			}
		}

		private void Calc(int x, int y)
		{
			checked
			{
				int num = this.cSteps - 1;
				for (int i = 0; i <= num; i++)
				{
					double d = unchecked((double)y * this.cCosA[i] - (double)x * this.cSinA[i]);
					int num2 = (int)Math.Round(this.CalcDIndex(d));
					int num3 = num2 * this.cSteps + i;
					try
					{
						this.cHMatrix[num3]++;
					}
					catch (Exception ex2)
					{
						Exception ex = ex2;
						Debug.WriteLine(ex.ToString());
					}
				}
			}
		}

		private double CalcDIndex(double d)
		{
			return (double)Convert.ToInt32(d - this.cDMin);
		}

		private bool IsBlack(int x, int y)
		{
			Color pixel = this.cBmp.GetPixel(x, y);
			double num = (double)pixel.R * 0.299 + (double)pixel.G * 0.587 + (double)pixel.B * 0.114;
			return num < 140.0;
		}

		private void Init()
		{
			checked
			{
				this.cSinA = new double[this.cSteps - 1 + 1];
				this.cCosA = new double[this.cSteps - 1 + 1];
				int num = this.cSteps - 1;
				for (int i = 0; i <= num; i++)
				{
					double num2 = unchecked(this.GetAlpha(i) * 3.1415926535897931) / 180.0;
					this.cSinA[i] = Math.Sin(num2);
					this.cCosA[i] = Math.Cos(num2);
				}
				this.cDMin = (double)(0 - this.cBmp.Width);
				this.cDCount = (int)Math.Round((double)(2 * (this.cBmp.Width + this.cBmp.Height)) / this.cDStep);
				this.cHMatrix = new int[this.cDCount * this.cSteps + 1];
			}
		}

		public double GetAlpha(int Index)
		{
			return this.cAlphaStart + (double)Index * this.cAlphaStep;
		}
	}
}
