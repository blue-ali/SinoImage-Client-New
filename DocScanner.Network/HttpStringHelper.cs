using System;
using System.IO;
using System.Net;
using System.Text;

namespace DocScanner.Network
{
	public static class HttpStringHelper
	{
		public static string HttpGetString(string url)
		{
			bool flag = !url.StartsWith("http://");
			if (flag)
			{
				url = "http://" + url;
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Method = "GET";
			string result;
			using (WebResponse response = httpWebRequest.GetResponse())
			{
				using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
				{
					char[] array = new char[512];
					StringBuilder stringBuilder = new StringBuilder();
					streamReader.ReadBlock(array, 0, array.Length);
					result = new string(array);
				}
			}
			return result;
		}
	}
}
