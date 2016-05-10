using DocScanner.Bean.pb;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocScanner.Network
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

		public static string GetHttpGetBatchURL(string batchNo)
        {
            string url = AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "ServerHosts") + AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "GetBatchUrl") + "?batchNo=" + batchNo;
			if (!url.StartsWith("http://"))
			{
                url = "http://" + url;
			}
			return url;
		}
        
		public static string GetHttpBrokeUploadBatchURL()
		{
            string host = AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "ServerHosts");
            string url = host + AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "SubmitBrokeUrl");
			if (!url.StartsWith("http://"))
			{
                url = "http://" + url;
			}
			return url;
		}

        public static string GetHttpBrokeUploadFileURL()
        {
            string host = AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "ServerHosts");
            string url = host + AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "SubmitBrokeFileUrl");
            if (!url.StartsWith("http://"))
            {
                url = "http://" + url;
            }
            return url;
        }

        public static string GetHttpFinishBrokeBatchURL(string batchNo)
        {
            string host = AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "ServerHosts");
            string url = host + AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "FinishBrokeBatchUrl") + "?batchNo=" + batchNo;
            if (!url.StartsWith("http://"))
            {
                url = "http://" + url;
            }
            return url;
        }

        public static string GetHttpFullUploadURL()
        {
            string host = AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "ServerHosts");
            string url = host + AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "SubmitFullUrl");
            if (!url.StartsWith("http://"))
            {
                url = "http://" + url;
            }
            return url;
        }

        public static string GetFastestServer()
		{
			string result;
			if (!string.IsNullOrEmpty(_fastestserver))
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
