using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.CodeUtils
{
    public static class LanguagyCodeHelper
    {
        // Methods
        public static string FormatZXingString(string input)
        {
            byte[] bytes = new byte[input.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)input[i];
            }
            char[] chars = new char[input.Length + 1];
            Encoding.GetEncoding("gb2312").GetChars(bytes, 0, bytes.Length, chars, 0);
            return new string(chars);
        }
    }

}