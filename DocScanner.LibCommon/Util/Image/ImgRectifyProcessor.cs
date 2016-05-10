using DocScanner.LibCommon;

namespace DocScanner.ImgUtils
{
    public class ImgRectifyProcessor : IImgProcessor
	{
		public bool Enabled
		{
			get
			{
				return IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "ImageRectifyProcessEnable").ToBool();
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "ImageRectifyProcessEnable", value.ToString());
			}
		}

		public string Name
		{
			get
			{
				return "ImageRectifyProcess";
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
				AppContext.GetInstance().MS.LogInfo("图像矫正:" + fname);
				result = fname;
			}
			return result;
		}
	}
}
