using DocScanner.LibCommon;
using System.Drawing;

namespace DocScanner.ImgUtils
{
    public class ImgBWProcessor : IImgProcessor
	{
		public string Name
		{
			get
			{
				return "Black White Process";
			}
		}

		public bool Enabled
		{
			get
			{
				return IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "AutoBlackWhite").ToBool();
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "AutoBlackWhite", value.ToString());
			}
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
				Bitmap bitmap = ImageHelper.LoadCorectedImage(fname).OtsuThreshold();
				bitmap.Save(fname);
				result = fname;
			}
			return result;
		}
	}
}
