using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DocScanner.Bean;
using DocScanner.LibCommon;
using DocScanner.LibCommon.Util;

namespace DocScanner.Adapter.SharpImportFile
{
    public class SharpImportFileAcquirer : IFileAcquirer, IHasIPropertiesSetting, IDisposable
	{
		public class NestPropertySetting : IPropertiesSetting
		{
			private SharpImportFileAcquirer _parent;

			[Browsable(false), Category("设置"), DisplayName("初始目录")]
			public string InitDir
			{
				get
				{
					return AppContext.GetInstance().Config.GetConfigParamValue("SharpImportFileAcquirer", "InitDir");
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpImportFileAcquirer", "InitDir", value.ToString());
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
					string text = AppContext.GetInstance().Config.GetConfigParamValue("SharpImportFileAcquirer", "MatchedFileExtensions");
					bool flag = string.IsNullOrEmpty(text);
					if (flag)
					{
						text = ".jpg;.bmp";
					}
					return text;
				}
				set
				{
					AppContext.GetInstance().Config.SetConfigParamValue("SharpImportFileAcquirer", "MatchedFileExtensions", value.ToString());
				}
			}

			public NestPropertySetting(SharpImportFileAcquirer parent)
			{
				this._parent = parent;
			}
		}

		private IAcquirerParam _param;

		private SharpImportFileAcquirer.NestPropertySetting _setting;

		public event EventHandler<TEventArg<string>> OnAcquired;

		public event EventHandler<TEventArg<string>> OnError;

		public bool Initialized
		{
			get
			{
				return true;
			}
		}

		public EImgType Filter
		{
			get;
			set;
		}

		public string Name
		{
			get
			{
				return "SharpImportFileAcquirer";
			}
		}

		public string CnName
		{
			get
			{
				return "导入文件";
			}
		}

		public bool Initialize(IAcquirerParam intparam = null)
		{
			this._param = intparam;
			return true;
		}

		public bool Acquire()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = AppContext.GetInstance().Config.GetConfigParamValue("UISetting", "LastAccessDir");
			List<string> list = this.GetSetting().MatchedFileExtensions.ToLower().Split(new char[]
			{
				';'
			}).ToList<string>();
			string text = "";
			foreach (string current in list)
			{
				text = string.Concat(new string[]
				{
					text,
					"(*",
					current.ToString(),
					")|*",
					current.ToString(),
					"|"
				});
			}
			text += "All files(*.*)|*.*";
			openFileDialog.Filter = text;
			openFileDialog.Multiselect = true;
			openFileDialog.InitialDirectory = AppContext.GetInstance().Config.GetConfigParamValue("UISetting", "LastAccessDir");
			bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				AppContext.GetInstance().Config.SetConfigParamValue("UISetting", "LastAccessDir", FileHelper.GetFileDir(openFileDialog.FileNames[0]));
				string[] fileNames = openFileDialog.FileNames;
				bool flag2 = this.OnAcquired != null;
				if (flag2)
				{
					string[] fileNames2 = openFileDialog.FileNames;
					for (int i = 0; i < fileNames2.Length; i++)
					{
						string input = fileNames2[i];
						this.OnAcquired(this, new TEventArg<string>(input));
					}
				}
				bool flag3 = fileNames.Length != 0;
				if (flag3)
				{
					this.GetSetting().InitDir = Path.GetDirectoryName(fileNames[0]);
				}
			}
			return true;
		}

		public static bool IsImgExt(string fname)
		{
			bool flag = string.IsNullOrEmpty(fname);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				fname = fname.ToLower();
				string[] array = new string[]
				{
					"ani",
					"anm",
					"bmp",
					"dib",
					"rle",
					"cdr",
					"cur",
					"dcm",
					"emf",
					"gif",
					"hdr",
					"ico",
					"ics",
					"jpg",
					"jpeg",
					"pcd",
					"png",
					"tif",
					"tiff"
				};
				for (int i = 0; i < array.Length; i++)
				{
					bool flag2 = fname.EndsWith(array[i]);
					if (flag2)
					{
						result = true;
						return result;
					}
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

		public SharpImportFileAcquirer.NestPropertySetting GetSetting()
		{
			bool flag = this._setting == null;
			if (flag)
			{
				this._setting = new SharpImportFileAcquirer.NestPropertySetting(this);
			}
			return this._setting;
		}

		IPropertiesSetting IHasIPropertiesSetting.GetSetting()
		{
			return this.GetSetting();
		}
	}
}
