using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using DocScanner.Bean;
using DocScanner.LibCommon;
using Saraff.Twain;

namespace DocScanner.Adapter.SharpTwain
{
    public class SharpTwainAcquirer : IFileAcquirer, IHasIPropertiesSetting, IDisposable
	{
		public class NestSetting : IPropertiesSetting
		{
			private SharpTwainAcquirer _acq;

			[Browsable(false)]
			public string Name
			{
				get
				{
					return "扫描仪设置";
				}
			}

			[Category("设置"), Description("保存影像类型"), DisplayName("保存影像类型")]
			public EImgType FType
			{
				get
				{
					string configParamValue = AppContext.GetInstance().Config.GetConfigParamValue("SharpTwainSetting", "SaveImageType");
					bool flag = string.IsNullOrEmpty(configParamValue);
					EImgType result;
					if (flag)
					{
						result = EImgType.jpeg;
					}
					else
					{
						try
						{
							EImgType eImgType = (EImgType)Enum.Parse(typeof(EImgType), configParamValue);
							result = eImgType;
						}
						catch
						{
							result = EImgType.jpeg;
						}
					}
					return result;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpTwainSetting", "SaveImageType", value.ToString());
				}
			}

			[Category("设置"), Description("保存影像类型"), DisplayName("重新选择扫描仪")]
			public bool ResetSelectScaner
			{
				get
				{
					return IniConfigSetting.Cur.GetConfigParamValue("SharpTwainSetting", "ResetSelectScaner").ToBool();
				}
				set
				{
					IniConfigSetting.Cur.SetConfigParamValue("SharpTwainSetting", "ResetSelectScaner", value.ToString());
				}
			}

			[Browsable(false), Category("设置"), Description("保存影像路径"), DisplayName("保存影像路径")]
			public string ImageDir
			{
				get
				{
					string configParamValue = AppContext.GetInstance().Config.GetConfigParamValue("SharpTwainSetting", "ImageDir");
					bool flag = string.IsNullOrEmpty(configParamValue);
					if (flag)
					{
						configParamValue = AppContext.GetInstance().Config.GetConfigParamValue("AppSetting", "TmpFileDir");
					}
					return configParamValue;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpTwainSetting", "ImageDir", value.ToString());
				}
			}

			[Category("设置"), DisplayName("保存压缩比例")]
			public long ImgRatio
			{
				get
				{
					long num = (long)AppContext.GetInstance().Config.GetConfigParamValue("SharpTwainSetting", "ImgRatio").ToInt();
					bool flag = num == 0L;
					if (flag)
					{
						num = 30L;
					}
					return num;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpTwainSetting", "ImgRatio", value.ToString());
				}
			}

			public NestSetting(SharpTwainAcquirer acq)
			{
				this._acq = acq;
			}
		}

		private IAcquirerParam _param;

		private Twain32 _twain;

		private bool _initialized = false;

		private SharpTwainAcquirer.NestSetting _setting;

		public event EventHandler<TEventArg<string>> OnAcquired;

		public event EventHandler<TEventArg<string>> OnError;

		public bool Initialized
		{
			get
			{
				return this._initialized;
			}
		}

		public string Name
		{
			get
			{
				return "SharpTwainAcquirer";
			}
		}

		public string CnName
		{
			get
			{
				return "扫描仪";
			}
		}

		public bool Initialize(IAcquirerParam initparam)
		{
			this._param = initparam;
			this.GetSetting().ImageDir = AppContext.GetInstance().Config.GetConfigParamValue("AppSetting", "TmpFileDir");
			return true;
		}

		public bool Acquire()
		{
			bool flag = this._twain != null;
			if (flag)
			{
				this._twain.Dispose();
				this._twain = null;
			}
			this._twain = new Twain32();
			this._twain.Parent = Control.FromHandle(this._param.HostWnd);
			this._twain.OpenDSM();
			this._twain.ShowUI = true;
			this._twain.AcquireError -= new EventHandler<Twain32.AcquireErrorEventArgs>(this._twain_AcquireError);
			this._twain.AcquireError += new EventHandler<Twain32.AcquireErrorEventArgs>(this._twain_AcquireError);
			this._twain.CloseDataSource();
			this._twain.SelectSource();
			this.GetSetting().ResetSelectScaner = false;
			this._twain.EndXfer -= new EventHandler<Twain32.EndXferEventArgs>(this._twain_EndXfer);
			this._twain.EndXfer += new EventHandler<Twain32.EndXferEventArgs>(this._twain_EndXfer);
			this._twain.Acquire();
			this._twain.Dispose();
			this._twain = null;
			return true;
		}

		private void _twain_EndXfer(object sender, Twain32.EndXferEventArgs e)
		{
			try
			{
				bool flag = e.Image != null;
				if (flag)
				{
					string text = this.GetSetting().ImageDir + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + this.GetSetting().FType.ToString();
					EncoderParameters encoderParameters = new EncoderParameters(1);
					EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, this.GetSetting().ImgRatio);
					encoderParameters.Param[0] = encoderParameter;
					e.Image.Save(text, this.GetSetting().FType.ToImageCodecInfo(), encoderParameters);
					bool flag2 = File.Exists(text);
					if (flag2)
					{
						TmpFileMgr.AddTmpFile(text);
					}
					bool flag3 = this.OnAcquired != null;
					if (flag3)
					{
						this.OnAcquired(this, new TEventArg<string>(text));
					}
				}
			}
			catch (Exception ex)
			{
				AppContext.GetInstance().MS.LogError(ex.ToString());
				MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void _twain_AcquireCompleted(object sender, EventArgs e)
		{
			bool flag = this.OnAcquired != null;
			if (flag)
			{
				this.OnAcquired(this, new TEventArg<string>(""));
			}
		}

		private void _twain_AcquireError(object sender, Twain32.AcquireErrorEventArgs e)
		{
			bool flag = this.OnError != null;
			if (flag)
			{
				this.OnError(this, new TEventArg<string>(e.Exception.ToString()));
			}
		}

		public void UnInitialize()
		{
			this.ClearEvents();
			bool flag = this._twain != null;
			if (flag)
			{
				this._twain.Dispose();
				this._twain = null;
				this._initialized = false;
			}
		}

		public void Dispose()
		{
			this.UnInitialize();
		}

		public SharpTwainAcquirer.NestSetting GetSetting()
		{
			bool flag = this._setting == null;
			if (flag)
			{
				this._setting = new SharpTwainAcquirer.NestSetting(this);
			}
			return this._setting;
		}

		IPropertiesSetting IHasIPropertiesSetting.GetSetting()
		{
			return this.GetSetting();
		}
	}
}
