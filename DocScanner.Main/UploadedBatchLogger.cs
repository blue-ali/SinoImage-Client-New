using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    [Serializable]
    public class UploadedBatchLogger
    {
        public List<UploadedBatchInfo> BatchNos = new List<UploadedBatchInfo>();

        private static string _localuploadebatchsfname = "_localUploadedBatch.dat";

        public static UploadedBatchLogger GetLocalUploaded()
        {
            string text = SystemHelper.GetAssemblesDirectory() + UploadedBatchLogger._localuploadebatchsfname;
            bool flag = File.Exists(text);
            UploadedBatchLogger result;
            if (flag)
            {
                try
                {
                    UploadedBatchLogger uploadedBatchLogger = SerializeHelper.DeSerializeFromXML<UploadedBatchLogger>(text);
                    result = uploadedBatchLogger;
                    return result;
                }
                catch
                {
                    File.Delete(text);
                }
            }
            UploadedBatchLogger uploadedBatchLogger2 = new UploadedBatchLogger();
            result = uploadedBatchLogger2;
            return result;
        }

        public static void Add_New(UploadedBatchInfo newBatchNos)
        {
            bool flag = !AbstractSetting<FunctionSetting>.CurSetting.AllowLogUploaded;
            if (!flag)
            {
                UploadedBatchLogger localUploaded = UploadedBatchLogger.GetLocalUploaded();
                bool flag2 = localUploaded.BatchNos.Find((UploadedBatchInfo o) => o.BatchNo == newBatchNos.BatchNo) == null;
                if (flag2)
                {
                    localUploaded.BatchNos.Insert(0, newBatchNos);
                }
                string fname = SystemHelper.GetAssemblesDirectory() + UploadedBatchLogger._localuploadebatchsfname;
                try
                {
                    SerializeHelper.SerializeToXML<UploadedBatchLogger>(localUploaded, fname);
                }
                catch
                {
                }
            }
        }

        public static void Del_Old(string batchno)
        {
            if (AbstractSetting<FunctionSetting>.CurSetting.AllowLogUploaded)
            {
                UploadedBatchLogger localUploaded = UploadedBatchLogger.GetLocalUploaded();
                foreach (UploadedBatchInfo info in localUploaded.BatchNos)
                {
                    if (info.BatchNo == batchno)
                    {
                        localUploaded.BatchNos.RemoveAll(o => o.BatchNo == batchno);
                        string fname = SystemHelper.GetAssemblesDirectory() + _localuploadebatchsfname;
                        SerializeHelper.SerializeToXML<UploadedBatchLogger>(localUploaded, fname);
                        break;
                    }
                }

            }
        }
    }
}
