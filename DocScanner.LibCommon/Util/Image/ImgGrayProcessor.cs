using DocScanner.LibCommon;
using System.Drawing;

namespace DocScanner.ImgUtils
{
    public class ImgGrayProcessor : IImgProcessor
	{
		public string Name
		{
			get
			{
				return "Gray Process";
			}
		}

		public bool Enabled
		{
			get
			{
				return IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "AutoGray").ToBool();
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "AutoGray", value.ToString());
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
				Bitmap bitmap = ImageHelper.LoadCorectedImage(fname).ToBitmap().ToGrayBitmap();
				bitmap.Save(fname);
				result = fname;
			}
			return result;
		}
	}
}
