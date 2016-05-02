using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public static class RecycleBin
    {
        // Fields
        private const int SHERB_NOCONFIRMATION = 1;
        private const int SHERB_NOPROGRESSUI = 2;
        private const int SHERB_NOSOUND = 4;

        // Methods
        public static bool CleanRecycleBin()
        {
            SHEmptyRecycleBin(IntPtr.Zero, "", 7);
            return true;
        }

        public static bool DeleteDir(string path)
        {
            FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            return true;
        }

        public static bool DeleteFile(string fullname)
        {
            try
            {
                FileSystem.DeleteFile(fullname, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("shell32.dll")]
        private static extern int SHEmptyRecycleBin(IntPtr handle, string root, int falgs);
    }

}
