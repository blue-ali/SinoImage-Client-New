using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocScaner.Network
{
    public class HttpUtil
	{

		private static bool _inited = false;

		private static string _fastestserver;

		public static List<string> GetServerHostsFromWeb()
		{
			string configParamValue = IniConfigSetting.Cur.GetConfigParamValue("NetSetting", "ServerGetServerListURL");
			List<string> serverHostsFromProfile = AbstractSetting<NetSetting>.CurSetting.GetServerHostsFromProfile();
			List<string> result;
			if (!AbstractSetting<NetSetting>.CurSetting.AllowUpdateServerAddress)
			{
				result = serverHostsFromProfile;
			}
			else
			{
				foreach (string current in serverHostsFromProfile)
				{
					string url = current + configParamValue;
					try
					{
						string text = HttpStringHelper.HttpGetString(url);
                  
                    if (text.Contains(":"))
                    {
                        char[] separator = new char[] { ';' };
                        result = text.Split(separator).Select<string, string>(x => x.Trim()).ToList<string>();
                    }

                }
                catch
					{
					}
				}
				result = serverHostsFromProfile;
			}
			return result;
		}

		public static string GetHttpDownloadURL()
		{
			string text = GetFastestServer() + AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "HttpDownloadURL");
			Console.WriteLine("TigEraHttp.GetFastestServer():" + GetFastestServer() + ":");
			Console.WriteLine("NetHttpDownloadURL:" + AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "HttpDownloadURL") + ":");
			bool flag = !text.StartsWith("http://");
			if (flag)
			{
				text = "http://" + text;
			}
			return text;
		}

		public static string GetHttpUploadURL()
		{
			string text = GetFastestServer() + AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "HttpUploadURL");
			bool flag = !text.StartsWith("http://");
			if (flag)
			{
				text = "http://" + text;
			}
			return text;
		}

		public static string GetFastestServer()
		{
			bool flag = !string.IsNullOrEmpty(_fastestserver);
			string result;
			if (flag)
			{
				result = _fastestserver;
			}
			else
			{
				List<string> serverHostsFromWeb = GetServerHostsFromWeb();
				result = serverHostsFromWeb[0];
			}
			return result;
		}
	}
}
