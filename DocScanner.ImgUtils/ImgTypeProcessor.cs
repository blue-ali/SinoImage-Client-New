using DocScaner.Common;
using DocScanner.LibCommon;
using Logos.DocScaner.Common;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DocScanner.ImgUtils
{
    public class ImgTypeProcessor : IImgProcessor
	{
		public static EMostImageType DestType
		{
			get
			{
				string configParamValue = IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "DestType");
				bool flag = string.IsNullOrEmpty(configParamValue);
				EMostImageType result;
				if (flag)
				{
					result = EMostImageType.None;
				}
				else
				{
					result = (EMostImageType)Enum.Parse(typeof(EMostImageType), configParamValue);
				}
				return result;
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "DestType", value.ToString());
			}
		}

		public static int Quality
		{
			get
			{
				int num = IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "DestTypeQuality").ToInt();
				return (num == 0) ? 100 : num;
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "DestTypeQuality", value.ToString());
			}
		}

		public bool Enabled
		{
			get
			{
				return IniConfigSetting.Cur.GetConfigParamValue("ImageProcessSetting", "ImageTypeProcss").ToBool();
			}
			set
			{
				IniConfigSetting.Cur.SetConfigParamValue("ImageProcessSetting", "ImageTypeProcss", value.ToString());
			}
		}

		public string Name
		{
			get
			{
				return "ImageTypeProcss";
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
				bool flag2 = ImgTypeProcessor.DestType == EMostImageType.None || EMostImageTypeHelper.GetImageType(fname) == ImgTypeProcessor.DestType;
				if (flag2)
				{
					result = fname;
				}
				else
				{
					string imageExt = EMostImageTypeHelper.GetImageExt(ImgTypeProcessor.DestType);
					string text = FileHelper.GetFileDir(fname) + FileHelper.GetFileNameNoExt(fname) + imageExt;
					AppContext.Cur.MS.LogInfo("图像格式转换:" + fname + "=>" + text);
					ImgTypeProcessor.SaveImageType(ImgTypeProcessor.DestType, fname, text);
					result = text;
				}
			}
			return result;
		}

		private static void SaveImageTypeByPictureBox(EMostImageType imgtype, string fname, string nfilename)
		{
			PictureBox pictureBox = new PictureBox();
			pictureBox.Image = ImageHelper.LoadLocalImage(fname, true);
			pictureBox.Image.Save(nfilename, EMostImageTypeHelper.GetSysImgFmt(ImgTypeProcessor.DestType));
			pictureBox.Dispose();
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

		private static void SaveImageType(EMostImageType imgtype, string fname, string nfilename)
		{
			ImageCodecInfo encoderInfo = ImgTypeProcessor.GetEncoderInfo("image/" + imgtype.ToString().ToLower());
			EncoderParameters encoderParameters = new EncoderParameters(1);
			Encoder quality = Encoder.Quality;
			EncoderParameter encoderParameter = new EncoderParameter(quality, (long)ImgTypeProcessor.Quality);
			encoderParameters.Param[0] = encoderParameter;
			Bitmap bitmap = new Bitmap(fname);
			bitmap.Save(nfilename, encoderInfo, encoderParameters);
			bitmap.Dispose();
		}
	}
}
