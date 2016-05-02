using System;

namespace DocScanner.ImgUtils
{
	public interface IImgProcessor
	{
		string Name
		{
			get;
		}

		bool Enabled
		{
			get;
			set;
		}

		string Process(string fname);
	}
}
