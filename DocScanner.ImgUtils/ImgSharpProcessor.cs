using System;

namespace DocScanner.ImgUtils
{
	public class ImgSharpProcessor : IImgProcessor
	{
		public bool Enabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public string Name
		{
			get
			{
				return "Auto Sharp";
			}
		}

		public string Process(string fname)
		{
			return fname;
		}
	}
}
