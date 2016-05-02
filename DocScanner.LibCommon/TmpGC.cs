using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.LibCommon
{
    public class TmpGC
    {
        // Methods
        public static void EmptyRubbish()
        {
            TmpFileMgr.DeleteFileList();
            TmpDirMgr.DeleteDirectory();
        }
    }

}
