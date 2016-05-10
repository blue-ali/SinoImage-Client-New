using DocScanner.LibCommon;
using DocScanner.LibCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocScanner.ImgUtils
{
    public class ImageChainProcessor : IImgProcessor
	{
		[ThreadStatic]
		private static ImageChainProcessor _processer;

		private List<IImgProcessor> _filters = new List<IImgProcessor>();

		public static ImageChainProcessor Cur
		{
			get
			{
				bool flag = ImageChainProcessor._processer == null;
				if (flag)
				{
					ImageChainProcessor._processer = new ImageChainProcessor();
					ImageChainProcessor._processer._filters.Add(new ImgTypeProcessor());
					ImageChainProcessor._processer._filters.Add(new ImgBlankProcessor());
					ImageChainProcessor._processer._filters.Add(new ImgRectifyProcessor());
					ImageChainProcessor._processer._filters.Add(new ImgSharpProcessor());
					ImageChainProcessor._processer._filters.Add(new ImgRmBlackEdgeProcessor());
					ImageChainProcessor._processer._filters.Add(new ImgBWProcessor());
					ImageChainProcessor._processer._filters.Add(new ImgGrayProcessor());
					ImageChainProcessor._processer._filters.Add(new ImgSizeProcessor());
				}
				return ImageChainProcessor._processer;
			}
		}

		public bool Enabled
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public bool CreateBackUp
		{
			get
			{
				return IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "AutoCreateBackup").ToBool();
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "AutoCreateBackup", value.ToString());
			}
		}

		public string Name
		{
			get
			{
				return "ImageChainProcess";
			}
		}

		public IImgProcessor GetProcessor(Type type)
		{
			return (from o in this._filters
			where o.GetType() == type
			select o).First<IImgProcessor>();
		}

		public string Process(string fname)
		{
            //FileExtUtil.IsImageExt(FileHelper.GetFileExt(fname));
			if (FileHelper.IsImageExt(fname))
			{
				foreach (IImgProcessor current in this._filters)
				{
					fname = current.Process(fname);
				}
			}
			return fname;
		}
	}
}
