using DocScanner.CodeUtils;
using DocScanner.Bean;
using DocScanner.LibCommon;
using DocScanner.Main;
using System;
using System.IO;
using DocScanner.LibCommon.Util;

namespace DocScanner.Common
{
    public static class BatchExt
    {
        public static void SetupBatchInfo(this NBatchInfo batch)
        {
            batch.UpadateAllDate(DateTime.Now);
            batch.SourceIP = NetHelper.GetHostIP4Address();
            batch.MachineID = NetHelper.GetFirstMacAddress();
            batch.TellerNO = AccountSetting.GetInstance().AccountName;
            batch.OrgID = AccountSetting.GetInstance().AccountOrgID;
            batch.BusiSysId = BusinessSetting.GetInstance().bustype;
            batch.BusiTypeId = BusinessSetting.GetInstance().busno;
        }

        public static void SetupFileInfo(this NFileInfo finfo)  
        {
            finfo.FileMD5 = MD5Helper.GetFileMD5(finfo.LocalPath);
            finfo.FileSize = (int)new FileInfo(finfo.LocalPath).Length;
            //finfo.FileName = FileHelper.GetFileName(finfo.LocalPath);
        }
    }
}
