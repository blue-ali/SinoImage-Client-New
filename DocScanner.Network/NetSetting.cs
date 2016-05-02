using DocScanner.Bean;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms.Design;

namespace DocScaner.Network
{
    public class NetSetting : AbstractSetting<NetSetting>, IPropertiesSetting
	{
		[Category("网络设置"), Description("传输直接包含文件数据")]
		public bool IncludeFileData
		{
			get
			{
				return AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "IncludeFileData").ToBool();
			}
			set
			{
				AppContext.Cur.Cfg.SetConfigParamValue("NetSetting", "IncludeFileData", value.ToString());
			}
		}

		[Category("网络设置"), Description("传输方式"), TypeConverter(typeof(SupportTransfType))]
		public string TransferType
		{
			get
			{
				return AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "NetTransferType");
			}
			set
			{
				AppContext.Cur.Cfg.SetConfigParamValue("NetSetting", "NetTransferType", value.ToString());
			}
		}

		[Category("网络设置"), Description("本地模式服务器存储文件路径"), Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
		public string LocalmodeServerDir
		{
			get
			{
				return AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "LocalmodeServerDir");
			}
			set
			{
				AppContext.Cur.Cfg.SetConfigParamValue("NetSetting", "LocalmodeServerDir", value.ToString());
			}
		}

		[Category("网络设置"), Description("Http服务器地址列表(以;分割,不带http://前缀，带端口号)")]
		public string HttpServerHosts
		{
			get
			{
				return AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "HttpServerHosts");
			}
			set
			{
				AppContext.Cur.Cfg.SetConfigParamValue("NetSetting", "HttpServerHosts", value.ToString());
			}
		}

		[Category("网络设置"), Description("启用负载均衡")]
		public bool AllowServerBalance
		{
			get
			{
				return AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "NetAllowServerBalance").ToBool();
			}
			set
			{
				AppContext.Cur.Cfg.SetConfigParamValue("NetSetting", "NetAllowServerBalance", value.ToString());
			}
		}

		[Category("网络设置"), Description("允许更新服务器地址")]
		public bool AllowUpdateServerAddress
		{
			get
			{
				return AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "AllowUpdateServerAddress").ToBool();
			}
			set
			{
				AppContext.Cur.Cfg.SetConfigParamValue("NetSetting", "AllowUpdateServerAddress", value.ToString());
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
