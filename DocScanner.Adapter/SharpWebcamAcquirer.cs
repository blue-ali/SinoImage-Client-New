using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using DocScanner.Bean;
using DocScanner.LibCommon;

namespace DocScanner.Adapter.SharpWebcam
{
    public class SharpWebcamAcquirer : IFileAcquirer, IHasIPropertiesSetting, IDisposable
	{
		public class NestSetting : IPropertiesSetting
		{
			private SharpWebcamAcquirer _acq;

			private UCPreviewer _parent;

			[Browsable(false)]
			public string Name
			{
				get
				{
					return "";
				}
			}

			[Category("摄像头设置"), DisplayName("分辨率设置"), TypeConverter(typeof(UCPreviewer.CapsConverter))]
			public string Resolutions
			{
				get
				{
					return AppContext.GetInstance().Config.GetConfigParamValue("SharpWebcamSetting", "Resolutions");
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpWebcamSetting", "Resolutions", value.ToString());
				}
			}

			[Category("摄像头设置"), DisplayName("保存影像宽度")]
			public int ImageWidth
			{
				get
				{
					int num = AppContext.GetInstance().Config.GetConfigParamValue("SharpWebcamSetting", "ImageWidth").ToInt();
					bool flag = num == 0;
					if (flag)
					{
						num = 800;
					}
					return num;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpWebcamSetting", "ImageWidth", value.ToString());
				}
			}

			[Category("摄像头设置"), DisplayName("保存影像高度")]
			public int ImageHeight
			{
				get
				{
					int num = AppContext.GetInstance().Config.GetConfigParamValue("SharpWebcamSetting", "ImageHeight").ToInt();
					bool flag = num == 0;
					if (flag)
					{
						num = 600;
					}
					return num;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpWebcamSetting", "ImageHeight", value.ToString());
				}
			}

			[Category("摄像头设置"), DisplayName("缩略图高度")]
			public int ThumbHeight
			{
				get
				{
					int num = AppContext.GetInstance().Config.GetConfigParamValue("SharpWebcamSetting", "ThumbHeight").ToInt();
					bool flag = num == 0;
					if (flag)
					{
						num = 80;
					}
					return num;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpWebcamSetting", "ThumbHeight", value.ToString());
				}
			}

			[Browsable(false), Category("设置"), Description("保存影像路径"), DisplayName("保存影像路径")]
			public string ImageDir
			{
				get
				{
					string configParamValue = AppContext.GetInstance().Config.GetConfigParamValue("SharpWebcamSetting", "ImageDir");
					bool flag = string.IsNullOrEmpty(configParamValue);
					if (flag)
					{
						configParamValue = AppContext.GetInstance().Config.GetConfigParamValue("AppSetting", "TmpFileDir");
					}
					return configParamValue;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpWebcamSetting", "ImageDir", value.ToString());
				}
			}

			[Category("摄像头设置"), DisplayName("缩略图宽度")]
			public int ThumbWidth
			{
				get
				{
					int num = AppContext.GetInstance().Config.GetConfigParamValue("SharpWebcamSetting", "ThumbWidth").ToInt();
					bool flag = num == 0;
					if (flag)
					{
						num = 80;
					}
					return num;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpWebcamSetting", "ThumbWidth", value.ToString());
				}
			}

			[Category("摄像头设置"), DisplayName("保存压缩比例")]
			public long ImgRatio
			{
				get
				{
					long num = (long)AppContext.GetInstance().Config.GetConfigParamValue("SharpWebcamSetting", "ImgRatio").ToInt();
					bool flag = num == 0L;
					if (flag)
					{
						num = 30L;
					}
					return num;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpWebcamSetting", "ImgRatio", value.ToString());
				}
			}

			[Category("摄像头设置"), DisplayName("照片格式")]
			public EImgType FType
			{
				get
				{
					string value = AppContext.GetInstance().Config.GetConfigParamValue("SharpWebcamSetting", "ImageType");
					bool flag = string.IsNullOrEmpty(value);
					if (flag)
					{
						value = EImgType.jpeg.ToString();
					}
					return (EImgType)Enum.Parse(typeof(EImgType), value);
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpWebcamSetting", "ImageType", value.ToString());
				}
			}

			public NestSetting(SharpWebcamAcquirer acq)
			{
				this._acq = acq;
			}

			public NestSetting(UCPreviewer parent)
			{
				this._parent = parent;
			}
		}

		private IAcquirerParam _param;

		private SharpWebcamAcquirer.NestSetting _setting;

		public event EventHandler<TEventArg<string>> OnAcquired;

		public event EventHandler<TEventArg<string>> OnError;

		public bool Initialized
		{
			get
			{
				return true;
			}
		}

		public string Name
		{
			get
			{
				return "SharpWebcamAcquirer";
			}
		}

		public Control Parent
		{
			get;
			set;
		}

		public string CnName
		{
			get
			{
				return "摄像头";
			}
		}

		public bool Initialize(IAcquirerParam initparam = null)
		{
			this._param = initparam;
			return true;
		}

		public bool Acquire()
		{
			bool result;
			try
			{
				FormContainer formContainer = new FormContainer();
				UCPreviewer uCPreviewer = new UCPreviewer(this);
				uCPreviewer.Start();
				formContainer.SetControl(uCPreviewer);
				formContainer.SetKeyEscCloseForm(true);
				formContainer.Parent = this.Parent;
				bool flag = formContainer.ShowDialog() == DialogResult.OK;
				if (flag)
				{
					List<string> images = uCPreviewer.GetImages();
					bool flag2 = this.OnAcquired != null;
					if (flag2)
					{
						foreach (string current in images)
						{
							this.OnAcquired(this, new TEventArg<string>(current));
						}
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				bool flag3 = this.OnError != null;
				if (flag3)
				{
					this.OnError(this, new TEventArg<string>(ex.ToString()));
				}
				result = false;
			}
			return result;
		}

		public void UnInitialize()
		{
			this.ClearEvents();
		}

		public void Dispose()
		{
			this.UnInitialize();
		}

		public SharpWebcamAcquirer.NestSetting GetSetting()
		{
			bool flag = this._setting == null;
			if (flag)
			{
				this._setting = new SharpWebcamAcquirer.NestSetting(this);
			}
			return this._setting;
		}

		IPropertiesSetting IHasIPropertiesSetting.GetSetting()
		{
			return this.GetSetting();
		}
	}
}
