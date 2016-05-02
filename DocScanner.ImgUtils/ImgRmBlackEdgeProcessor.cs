using System;
using System.Drawing;

namespace DocScanner.ImgUtils
{
	public class ImgRmBlackEdgeProcessor : IImgProcessor
	{
		public string Name
		{
			get
			{
				return "Remove Black Edge Processor";
			}
		}

		public bool Enabled
		{
			get;
			set;
		}

		public string Process(string fname)
		{
			bool flag = !this.Enabled;
			string result;
			if (flag)
			{
				result = fname;
			}
			else
			{
				Bitmap bitmap = ImageHelper.LoadCorectedImage(fname).RemoveBlackEdge();
				bitmap.Save(fname);
				result = fname;
			}
			return result;
		}
	}
}
