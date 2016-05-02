using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    internal class StringHelper
    {
        // Methods
        public Dictionary<string, string> ParseURL(string url, char startchar = '?', char splitchar = '&', char equalchar = '=')
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            int index = url.IndexOf(startchar);
            if (index != -1)
            {
                url = url.Substring(index);
            }
            char[] separator = new char[] { splitchar };
            string[] strArray = url.Split(separator);
            if (strArray != null)
            {
                foreach (string str in strArray)
                {
                    char[] chArray2 = new char[] { equalchar };
                    string[] strArray3 = str.Split(chArray2);
                    if ((strArray3 != null) && (strArray3.Length == 2))
                    {
                        dictionary[strArray3[0]] = strArray3[1];
                    }
                }
            }
            return dictionary;
        }
    }

}
