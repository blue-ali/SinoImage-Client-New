using DocScanner.Bean;
using DocScanner.LibCommon;
using DocScanner.Network.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms.Design;

namespace DocScanner.Network
{
    public class NetSetting : AbstractSetting<NetSetting>, IPropertiesSetting
	{

        private static readonly NetSetting instance = new NetSetting();

        //private NetSetting()
        //{

        //}

        public static NetSetting GetInstance()
        {
            return instance;
        }

		[Category("网络设置"), Description("传输直接包含文件数据")]
		public bool IncludeFileData
		{
			get
			{
				return AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "IncludeFileData").ToBool();
			}
			set
			{
				AppContext.GetInstance().Config.SetConfigParamValue("NetSetting", "IncludeFileData", value.ToString());
			}
		}

		[Category("网络设置"), Description("传输方式"), TypeConverter(typeof(SupportTransfType))]
		public string TransferType
		{
			get
			{
				return AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "NetTransferType");
			}
			set
			{
				AppContext.GetInstance().Config.SetConfigParamValue("NetSetting", "NetTransferType", value.ToString());
			}
		}

		[Category("网络设置"), Description("本地模式服务器存储文件路径"), Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
		public string LocalmodeServerDir
		{
			get
			{
				return AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "LocalmodeServerDir");
			}
			set
			{
				AppContext.GetInstance().Config.SetConfigParamValue("NetSetting", "LocalmodeServerDir", value.ToString());
			}
		}

		[Category("网络设置"), Description("Http服务器地址列表(以;分割,不带http://前缀，带端口号)")]
		public string HttpServerHosts
		{
			get
			{
				return AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "HttpServerHosts");
			}
			set
			{
				AppContext.GetInstance().Config.SetConfigParamValue("NetSetting", "HttpServerHosts", value.ToString());
			}
		}

		[Category("网络设置"), Description("启用负载均衡")]
		public bool AllowServerBalance
		{
			get
			{
				return AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "NetAllowServerBalance").ToBool();
			}
			set
			{
				AppContext.GetInstance().Config.SetConfigParamValue("NetSetting", "NetAllowServerBalance", value.ToString());
			}
		}

		[Category("网络设置"), Description("允许更新服务器地址")]
		public bool AllowUpdateServerAddress
		{
			get
			{
				return AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "AllowUpdateServerAddress").ToBool();
			}
			set
			{
				AppContext.GetInstance().Config.SetConfigParamValue("NetSetting", "AllowUpdateServerAddress", value.ToString());
			}
		}

        [Category("网络设置"), Description("传输模式")]
        public ETransMode TransMode
        {
            get
            {
                string configValue = AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "TransMode");
                if (string.IsNullOrEmpty(configValue))
                {
                    return ETransMode.FULL;
                }else
                {
                    return (ETransMode)Enum.Parse(typeof(ETransMode), configValue);
                }
                //return AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "TransMode");
            }
            set
            {
                AppContext.GetInstance().Config.SetConfigParamValue("NetSetting", "TransMode", value.ToString());
            }
        }

        [Category("网络设置"), Description("超时时间")]
        public string Timeout
        {
            get
            {
                return AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "Timeout");
            }
            set
            {
                AppContext.GetInstance().Config.SetConfigParamValue("NetSetting", "Timeout", value);
            }
        }

        [Browsable(false)]
		public override string Name
		{
			get
			{
				return "网络设置";
			}
		}

		public override bool Equals(NetSetting other)
		{
			throw new NotImplementedException();
		}

		public List<string> GetServerHostsFromProfile()
		{
			return IniConfigSetting.Cur.GetConfigParamValue("NetSetting", "HttpServerHosts").Split(new char[]
			{
				';'
			}).ToList<string>();
		}

		public void SaveServerHosts2file(List<string> servers)
		{
			string value = string.Join(";", servers.ToArray());
			IniConfigSetting.Cur.SetConfigParamValue("NetSetting", "HttpServerHosts", value);
		}
	}
}
