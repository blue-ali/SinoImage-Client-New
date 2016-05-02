using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Common
{
    public sealed class EncryptUtils
    {
        // Methods
        public static string Base64Decrypt(string input)
        {
            return Base64Decrypt(input, new UTF8Encoding());
        }

        public static string Base64Decrypt(string input, Encoding encode)
        {
            return encode.GetString(Convert.FromBase64String(input));
        }

        public static string Base64Encrypt(string input)
        {
            return Base64Encrypt(input, new UTF8Encoding());
        }

        public static string Base64Encrypt(string input, Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }

        public static string DES3Decrypt(string data, string key)
        {
            ICryptoTransform transform = new TripleDESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(key), Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 }.CreateDecryptor();
            string str = "";
            try
            {
                byte[] inputBuffer = Convert.FromBase64String(data);
                str = Encoding.ASCII.GetString(transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch (Exception)
            {
            }
            return str;
        }

        public static string DES3Encrypt(string data, string key)
        {
            ICryptoTransform transform = new TripleDESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes(key), Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 }.CreateEncryptor();
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            return Convert.ToBase64String(transform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        public static string DESDecrypt(string data, string key, string iv)
        {
            byte[] buffer3;
            byte[] bytes = Encoding.ASCII.GetBytes(key);
            byte[] rgbIV = Encoding.ASCII.GetBytes(iv);
            try
            {
                buffer3 = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream(buffer3);
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(stream2);
            return reader.ReadToEnd();
        }

        public static string DESEncrypt(string data, string key, string iv)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(key);
            byte[] rgbIV = Encoding.ASCII.GetBytes(iv);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            int keySize = provider.KeySize;
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(stream2);
            writer.Write(data);
            writer.Flush();
            stream2.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
        }

        public static string MD5Encrypt(Stream stream)
        {
            byte[] buffer = MD5.Create().ComputeHash(stream);
            StringBuilder builder = new StringBuilder();
            foreach (byte num2 in buffer)
            {
                builder.Append(num2.ToString("x2"));
            }
            return builder.ToString();
        }

        public static string MD5Encrypt(string input)
        {
            return MD5Encrypt(input, new UTF8Encoding());
        }

        public static string MD5Encrypt(string input, Encoding encode)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(encode.GetBytes(input));
            StringBuilder builder = new StringBuilder(0x20);
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
            }
            return builder.ToString();
        }

        public static string MD5Encrypt16(string input, Encoding encode)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(provider.ComputeHash(encode.GetBytes(input)), 4, 8).Replace("-", "");
        }
    }

}
