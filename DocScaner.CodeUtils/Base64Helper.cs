using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScaner.CodeUtils
{
    public static class Base64Helper
    {
        // Methods
        public static byte[] BytesFrom64(string data)
        {
            return Convert.FromBase64String(data);
        }

        public static string BytesTo64(byte[] data)
        {
            string str = string.Empty;
            try
            {
                str = Convert.ToBase64String(data, 0, data.Length);
            }
            catch
            {
                throw;
            }
            return str;
        }

        public static bool FileFrom64(string data, string fname)
        {
            byte[] bytes = BytesFrom64(data);
            File.WriteAllBytes(fname, bytes);
            return true;
        }

        public static string FileTo64(string fpath)
        {
            return BytesTo64(File.ReadAllBytes(fpath));
        }
    }

}
