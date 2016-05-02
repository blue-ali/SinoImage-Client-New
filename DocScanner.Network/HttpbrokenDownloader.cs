using DocScaner.CodeUtils;
using DocScanner.CodeUtils;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace DocScaner.Network
{
    public class HttpbrokenDownloader
	{
		private static object _filelocker = new object();

		public static bool NormalDownloadFile(string LocalFileName, string RemoteFileURL)
		{
			bool result;
			try
			{
				WebClient webClient = new WebClient();
				webClient.DownloadFile(RemoteFileURL, LocalFileName);
				result = true;
			}
			catch (Exception ex)
			{
				throw new Exception("HTTP下载文件出错，" + ex.Message);
			}
			return result;
		}

		private static bool DownloadRange(string strURL, string fname, int startindex, int endindex)
		{
			bool result;
			try
			{
				Uri requestUri = new Uri(strURL);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				httpWebRequest.AddRange(startindex, endindex);
				using (Stream responseStream = httpWebRequest.GetResponse().GetResponseStream())
				{
					int num = endindex - startindex;
					byte[] array = new byte[num];
					int datalen = responseStream.Read(array, 0, num);
					HttpbrokenDownloader.WriteDataToFile(fname, startindex, array, datalen);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		private static bool WriteDataToFile(string strFileName, int SPosition, byte[] btContent, int datalen)
		{
			object filelocker = HttpbrokenDownloader._filelocker;
			bool result;
			lock (filelocker)
			{
				bool flag2 = File.Exists(strFileName);
				FileStream fileStream;
				if (flag2)
				{
					fileStream = File.OpenWrite(strFileName);
				}
				else
				{
					fileStream = new FileStream(strFileName, FileMode.Create);
				}
				fileStream.Seek((long)SPosition, SeekOrigin.Current);
				fileStream.Write(btContent, 0, datalen);
				fileStream.Close();
				result = true;
			}
			return result;
		}

		public static bool DownloadInThreads(string strURL, int threads)
		{
			return false;
		}

		public static bool SafetyDownloadFile(string strFileName, string strUrl, int trytimes = 5, string fileServerMD5 = "")
		{
			int i = 0;
			bool result;
			while (i < trytimes)
			{
				i++;
				bool flag = HttpbrokenDownloader.BrokenDownloadFile(strFileName, strUrl);
				bool flag2 = flag;
				if (flag2)
				{
					bool flag3 = !string.IsNullOrEmpty(fileServerMD5);
					if (flag3)
					{
						bool flag4 = MD5Helper.GetFileMD5(strFileName) != fileServerMD5;
						if (flag4)
						{
							File.Delete(strFileName);
							continue;
						}
					}
					result = true;
					return result;
				}
				Thread.Sleep(1000);
			}
			result = true;
			return result;
		}

		public static bool BrokenDownloadFile(string strFileName, string strUrl)
		{
			bool flag = File.Exists(strFileName);
			FileStream fileStream;
			long num;
			if (flag)
			{
				fileStream = File.OpenWrite(strFileName);
				num = fileStream.Length;
				fileStream.Seek(num, SeekOrigin.Current);
			}
			else
			{
				fileStream = new FileStream(strFileName, FileMode.Create);
				num = 0L;
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(strUrl);
			bool flag2 = num > 0L;
			if (flag2)
			{
				httpWebRequest.AddRange((int)num);
			}
			Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
			byte[] buffer = new byte[512];
			for (int i = responseStream.Read(buffer, 0, 512); i > 0; i = responseStream.Read(buffer, 0, 512))
			{
				fileStream.Write(buffer, 0, i);
			}
			fileStream.Close();
			responseStream.Close();
			return true;
		}
	}
}
