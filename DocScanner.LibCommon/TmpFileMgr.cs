using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public class TmpFileMgr
    {
        // Fields
        private static List<string> _fnamelist = new List<string>();

        // Methods
        public static void AddTmpFile(string fpath)
        {
            if (File.Exists(fpath))
            {
                _fnamelist.Add(fpath);
            }
        }

        public static void DeleteFile(string fpath)
        {
            RecycleBin.DeleteFile(fpath);
            _fnamelist.Remove(fpath);
        }

        public static void DeleteFileList()
        {
            foreach (string str in _fnamelist)
            {
                try
                {
                    RecycleBin.DeleteFile(str);
                }
                catch
                {
                }
            }
            _fnamelist.Clear();
        }
    }

}
