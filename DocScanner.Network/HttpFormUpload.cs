using DocScaner.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DocScaner.Network
{
    public static class HttpFormUpload
	{
		public class FileParameter
		{
			public byte[] File
			{
				get;
				set;
			}

			public string FileName
			{
				get;
				set;
			}

			public string ContentType
			{
				get;
				set;
			}

			public FileParameter(byte[] file) : this(file, null)
			{
			}

			public FileParameter(byte[] file, string filename) : this(file, filename, null)
			{
			}

			public FileParameter(byte[] file, string filename, string contenttype)
			{
				this.File = file;
				this.FileName = filename;
				this.ContentType = contenttype;
			}
		}

		private static readonly Encoding encoding = Encoding.UTF8;

		private static string _useragent = "tigera";

		public static string UserAgent
		{
			get
			{
				return HttpFormUpload._useragent;
			}
			set
			{
				HttpFormUpload._useragent = value;
			}
		}

		public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
		{
			string text = string.Format("----------{0:N}", Guid.NewGuid());
			string contentType = "multipart/form-data; boundary=" + text;
			byte[] multipartFormData = HttpFormUpload.GetMultipartFormData(postParameters, text);
			return HttpFormUpload.PostForm(postUrl, userAgent, contentType, multipartFormData);
		}

		private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
		{
			HttpWebRequest httpWebRequest = WebRequest.Create(postUrl) as HttpWebRequest;
			bool flag = httpWebRequest == null;
			if (flag)
			{
				throw new NullReferenceException("request is not a http request");
			}
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = contentType;
			httpWebRequest.UserAgent = userAgent;
			httpWebRequest.CookieContainer = new CookieContainer();
			httpWebRequest.ContentLength = (long)formData.Length;
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				requestStream.Write(formData, 0, formData.Length);
				requestStream.Close();
			}
			return httpWebRequest.GetResponse() as HttpWebResponse;
		}

		private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
		{
			Stream stream = new MemoryStream();
			bool flag = false;
			foreach (KeyValuePair<string, object> current in postParameters)
			{
				bool flag2 = flag;
				if (flag2)
				{
					stream.Write(HttpFormUpload.encoding.GetBytes("\r\n"), 0, HttpFormUpload.encoding.GetByteCount("\r\n"));
				}
				flag = true;
				bool flag3 = current.Value is HttpFormUpload.FileParameter;
				if (flag3)
				{
					HttpFormUpload.FileParameter fileParameter = (HttpFormUpload.FileParameter)current.Value;
					string s = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n", new object[]
					{
						boundary,
						current.Key,
						fileParameter.FileName ?? current.Key,
						fileParameter.ContentType ?? "application/octet-stream"
					});
					stream.Write(HttpFormUpload.encoding.GetBytes(s), 0, HttpFormUpload.encoding.GetByteCount(s));
					stream.Write(fileParameter.File, 0, fileParameter.File.Length);
				}
				else
				{
					string s2 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}", boundary, current.Key, current.Value);
					stream.Write(HttpFormUpload.encoding.GetBytes(s2), 0, HttpFormUpload.encoding.GetByteCount(s2));
				}
			}
			string s3 = "\r\n--" + boundary + "--\r\n";
			stream.Write(HttpFormUpload.encoding.GetBytes(s3), 0, HttpFormUpload.encoding.GetByteCount(s3));
			stream.Position = 0L;
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			stream.Close();
			return array;
		}

		public static HttpWebResponse UploadFile(string fname, string postURL)
		{
			FileStream fileStream = new FileStream(fname, FileMode.Open, FileAccess.Read);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("filename", FileHelper.GetFileName(fname));
			dictionary.Add("fileformat", FileHelper.GetFileExtNoIncDot(fname));
			dictionary.Add("file", new HttpFormUpload.FileParameter(array, FileHelper.GetFileName(fname), ""));
			HttpWebResponse result;
			try
			{
				HttpWebResponse httpWebResponse = HttpFormUpload.MultipartFormDataPost(postURL, HttpFormUpload.UserAgent, dictionary);
				result = httpWebResponse;
			}
			catch (WebException ex)
			{
				throw ex;
			}
			return result;
		}
	}
}
