using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public class TmpDirMgr
    {
        // Fields
        private static List<string> _dirslist = new List<string>();

        // Methods
        public static void AddTmpDir(string dirname)
        {
            _dirslist.Add(dirname);
        }

        public static void DeleteDirectory()
        {
            foreach (string str in _dirslist)
            {
                try
                {
                    if (Directory.Exists(str))
                    {
                        RecycleBin.DeleteDir(str);
                    }
                }
                catch
                {
                }
            }
            _dirslist.Clear();
        }
    }

}
