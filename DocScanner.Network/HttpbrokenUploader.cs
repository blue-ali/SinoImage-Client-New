using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace DocScanner.Network
{
	internal class HttpbrokenUploader
	{
		
		public static string serverPath = "";

		public const string JspFileUpload = "FileUpload";

		public const string JspFileGetTransferedLength = "FileGetTransferedLength";

		public const string JspFileDownload = "FileDownload";

		public static bool SupportResume
		{
			get;
			set;
		}

		public static bool UpLoadFile(string fileName, int byteCount, out string msg)
		{
			msg = "";
			bool result = true;
			long num = HttpbrokenUploader.FileGetTransferedLength(fileName);
			FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			long length = fileStream.Length;
			fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
			try
			{
				bool flag = num > 0L;
				if (flag)
				{
					fileStream.Seek(num, SeekOrigin.Current);
				}
				while (num <= length)
				{
					bool flag2 = num + (long)byteCount > length;
					byte[] array;
					if (flag2)
					{
						array = new byte[Convert.ToInt64(length - num)];
						binaryReader.Read(array, 0, Convert.ToInt32(length - num));
					}
					else
					{
						array = new byte[byteCount];
						binaryReader.Read(array, 0, byteCount);
					}
					try
					{
						Hashtable hashtable = new Hashtable();
						hashtable.Add("fileName", fileName);
						hashtable.Add("npos", num.ToString());
						byte[] array2 = HttpbrokenUploader.PostData(HttpbrokenUploader.serverPath + "FileUpload", array, hashtable);
					}
					catch (Exception ex)
					{
						msg = ex.ToString();
						result = false;
						break;
					}
					num += (long)byteCount;
				}
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
			finally
			{
				binaryReader.Close();
				fileStream.Close();
			}
			GC.Collect();
			return result;
		}

		private static long FileGetTransferedLength(string fileName)
		{
			bool flag = !HttpbrokenUploader.SupportResume;
			long result;
			if (flag)
			{
				result = 0L;
			}
			else
			{
				fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
				Hashtable hashtable = new Hashtable();
				hashtable.Add("fileName", fileName);
				byte[] data = new byte[0];
				try
				{
					byte[] bytes = HttpbrokenUploader.PostData(HttpbrokenUploader.serverPath + "FileGetTransferedLength", data, hashtable);
					string @string = Encoding.Default.GetString(bytes);
					long num = Convert.ToInt64(@string);
					result = num;
				}
				catch
				{
					result = 0L;
				}
			}
			return result;
		}

		private static byte[] PostData(string serverURL, byte[] data, Hashtable parms)
    {
        WebClient webClient = new WebClient();
        webClient.Headers.Add("Content-Type", "multipart/form-data;");
        if (parms != null)
        {
            string str = string.Join("&", parms.Cast<DictionaryEntry>().Select<DictionaryEntry, string>(o => (o.Key + "=" + o.Value)).ToArray<string>());
            serverURL = serverURL + "?" + str;
        }

        return webClient.UploadData(serverURL, "POST", data);
		}

		public static bool TestUpLoadFile(string fileName, int byteCount, out string msg)
		{
			msg = "";
			bool result = true;
			long num = 0L;
			FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			long num2 = 100L;
			fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
			try
			{
				while (num <= num2)
				{
					bool flag = num + (long)byteCount > num2;
					byte[] array;
					if (flag)
					{
						array = new byte[Convert.ToInt64(num2 - num)];
						binaryReader.Read(array, 0, Convert.ToInt32(num2 - num));
					}
					else
					{
						array = new byte[byteCount];
						binaryReader.Read(array, 0, byteCount);
					}
					try
					{
						Hashtable hashtable = new Hashtable();
						hashtable.Add("fileName", fileName);
						hashtable.Add("npos", num.ToString());
						byte[] array2 = HttpbrokenUploader.PostData(HttpbrokenUploader.serverPath + "UpLoadSer100.aspx", array, hashtable);
					}
					catch (Exception ex)
					{
						msg = ex.ToString();
						result = false;
						break;
					}
					num += (long)byteCount;
				}
			}
			catch (Exception ex2)
			{
				msg = ex2.ToString();
				result = false;
			}
			finally
			{
				binaryReader.Close();
				fileStream.Close();
			}
			GC.Collect();
			return result;
		}
	}
}
