using DocScanner.CodeUtils;
using DocScaner.Common;
using DocScanner.Bean;
using DocScanner.LibCommon;
using DocScanner.Main;
using Logos.DocScaner.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Common
{
    public static class BatchExt
    {
        public static void SetupBatchInfo(this NBatchInfo batch)
        {
            batch.UpadateAllDate(DateTime.Now);
            batch.SourceIP = NetHelper.GetHostIP4Address();
            batch.MachineID = NetHelper.GetFirstMacAddress();
            batch.TellerNO = AbstractSetting<AccountSetting>.CurSetting.AccountName;
            batch.OrgID = AbstractSetting<AccountSetting>.CurSetting.AccountOrgID;
            batch.BusiSysId = AbstractSetting<BusinessSetting>.CurSetting.bustype;
            batch.BusiTypeId = AbstractSetting<BusinessSetting>.CurSetting.busno;
        }

        public static void SetupFileInfo(this NFileInfo finfo)  
        {
            finfo.FileMD5 = MD5Helper.GetFileMD5(finfo.LocalPath);
            finfo.FileSize = (int)new FileInfo(finfo.LocalPath).Length;
            finfo.FileName = FileHelper.GetFileName(finfo.LocalPath);
        }
    }
}
