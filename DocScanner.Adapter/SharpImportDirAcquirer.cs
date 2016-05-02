using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace DocScanner.Adapter.SharpImportDir
{
    public class SharpImportDirAcquirer : IFileAcquirer, IHasIPropertiesSetting, IDisposable
	{
		public class NestSetting : IPropertiesSetting
		{
			private SharpImportDirAcquirer _acq;

			[Browsable(false), Category("设置"), DisplayName("初始目录")]
			public string InitDir
			{
				get
				{
					return AppContext.Cur.Cfg.GetConfigParamValue("SharpImportDir", "InitDir");
				}
				set
				{
					AppContext.Cur.Cfg.SetConfigParamValue("SharpImportDir", "InitDir", value.ToString());
				}
			}

			[Browsable(false)]
			public string Name
			{
				get
				{
					return "";
				}
			}

			[Category("设置"), DisplayName("支持文件类型")]
			public string MatchedFileExtensions
			{
				get
				{
					string text = AppContext.Cur.Cfg.GetConfigParamValue("SharpImportDir", "MatchedFileExtensions");
					bool flag = string.IsNullOrEmpty(text);
					if (flag)
					{
						text = ".jpg;.bmp";
					}
					return text;
				}
				set
				{
					AppContext.Cur.Cfg.SetConfigParamValue("SharpImportDir", "MatchedFileExtensions", value.ToString());
				}
			}

			public NestSetting(SharpImportDirAcquirer acq)
			{
				this._acq = acq;
			}
		}

		private IAcquirerParam _param;

		private SharpImportDirAcquirer.NestSetting _setting;


		public event EventHandler<TEventArg<string>> OnAcquired = null;

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
				return "SharpImportDirAcquirer";
			}
		}

		public string CnName
		{
			get
			{
				return "目录采集器";
			}
		}

		public bool Initialize(IAcquirerParam intparam = null)
		{
			this._param = intparam;
			return true;
		}

		public bool Acquire()
		{
			FormDirPicker formDirPicker = new FormDirPicker(this);
			bool flag = formDirPicker.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				bool flag2 = this.OnAcquired != null;
				if (flag2)
				{
					foreach (string current in formDirPicker.SelectedFiles)
					{
						this.OnAcquired(this, new TEventArg<string>(current));
					}
				}
			}
			return true;
		}

		public void UnInitialize()
		{
			this.ClearEvents();
		}

		public void Dispose()
		{
			this.UnInitialize();
		}

		public SharpImportDirAcquirer.NestSetting GetSetting()
		{
			bool flag = this._setting == null;
			if (flag)
			{
				this._setting = new SharpImportDirAcquirer.NestSetting(this);
			}
			return this._setting;
		}

		IPropertiesSetting IHasIPropertiesSetting.GetSetting()
		{
			return this.GetSetting();
		}
	}
}
