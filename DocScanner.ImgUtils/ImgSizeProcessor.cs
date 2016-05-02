using DocScaner.Common;
using DocScanner.LibCommon;
using Logos.DocScaner.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DocScanner.ImgUtils
{
    public class ImgSizeProcessor : IImgProcessor
	{
		public static int MaxSize
		{
			get
			{
				int num = IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "DestImageSize").ToInt();
				bool flag = num == 0;
				int result;
				if (flag)
				{
					result = 10000;
				}
				else
				{
					result = num;
				}
				return result;
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "DestImageSize", value.ToString());
			}
		}

		public bool Enabled
		{
			get
			{
				return IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "ImageSizeProcess").ToBool();
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "ImageSizeProcess", value.ToString());
			}
		}

		public string Name
		{
			get
			{
				return "ImageSizeProcess";
			}
		}

		private static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo result;
			for (int i = 0; i < imageEncoders.Length; i++)
			{
				bool flag = imageEncoders[i].MimeType == mimeType;
				if (flag)
				{
					result = imageEncoders[i];
					return result;
				}
			}
			result = null;
			return result;
		}

		public void processImp(string fname, int Quality)
		{
			string text = FileHelper.GetFileExtNoIncDot(fname).ToLower();
			bool flag = text == "jpg";
			if (flag)
			{
				text = "jpeg";
			}
			ImageCodecInfo encoderInfo = ImgSizeProcessor.GetEncoderInfo("image/" + text);
			EncoderParameters encoderParameters = new EncoderParameters(1);
			Encoder quality = Encoder.Quality;
			EncoderParameter encoderParameter = new EncoderParameter(quality, (long)Quality);
			encoderParameters.Param[0] = encoderParameter;
			Bitmap bitmap = new Bitmap(fname);
			bitmap.Save(fname, encoderInfo, encoderParameters);
			bitmap.Dispose();
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
				bool flag2 = ImgSizeProcessor.MaxSize == 0;
				if (flag2)
				{
					result = fname;
				}
				else
				{
					int num = ImgSizeProcessor.MaxSize * 1024;
					FileInfo fileInfo = new FileInfo(fname);
					bool flag3 = fileInfo.Length <= (long)num;
					if (flag3)
					{
						result = fname;
					}
					else
					{
						int quality = (int)((long)(num * 100) / fileInfo.Length);
						this.processImp(fname, quality);
						AppContext.Cur.MS.LogInfo("图像大小转换:" + fname);
						result = fname;
					}
				}
			}
			return result;
		}
	}
}
