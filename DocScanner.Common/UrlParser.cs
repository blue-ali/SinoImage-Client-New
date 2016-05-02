using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Common
{
    public class UrlParser
    {
        private Dictionary<string, string> _dic = new Dictionary<string, string>();

        public static UrlParser ParseURL(string url)
        {
            return new UrlParser();
        }

        public bool Parse(string url, char splittoken = '&')
        {
            this._dic.Clear();
            string[] array = url.Split(new char[]
            {
                splittoken
            });
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                bool flag = text.Contains("=");
                if (flag)
                {
                    string[] array3 = text.Split(new char[]
                    {
                        '='
                    });
                    Debug.Assert(array3.Length == 2);
                    this._dic[array3[0].ToLower()] = array3[1].ToLower();
                }
            }
            return true;
        }

        public string GetKey(string key)
        {
            bool flag = this._dic.ContainsKey(key.ToLower());
            string result;
            if (flag)
            {
                result = this._dic[key.ToLower()];
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }
    }
}
