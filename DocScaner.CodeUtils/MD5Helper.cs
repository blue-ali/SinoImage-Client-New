using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.CodeUtils
{
    public class MD5Helper
    {
        // Methods
        public static string GetFileMD5(string fileName)
        {
            string str;
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(stream);
                    stream.Close();
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        builder.Append(buffer[i].ToString("x2"));
                    }
                    str = builder.ToString();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + exception.Message);
            }
            return str;
        }

        public static string GetMD5(byte[] buff)
        {
            using (MD5 md = MD5.Create())
            {
                byte[] buffer = md.ComputeHash(buff);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

}
