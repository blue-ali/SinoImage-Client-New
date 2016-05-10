using DocScanner.LibCommon;

namespace DocScanner.ImgUtils
{
    public class ImgBlankProcessor : IImgProcessor
	{
		public bool Enabled
		{
			get
			{
				return IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "ImageBlankProcess").ToBool();
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "ImageBlankProcess", value.ToString());
			}
		}

		public string Name
		{
			get
			{
				return "ImageBlankProcess";
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
				AppContext.GetInstance().MS.LogInfo("空白图过滤:" + fname);
				result = fname;
			}
			return result;
		}
	}
}
