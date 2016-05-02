using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocScanner.Main
{
    [Serializable]
    public class UploadedBatchInfo
    {
        public string BatchNo
        {
            get;
            set;
        }

        public DateTime Time
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }

        public UploadedBatchInfo()
        {
            this.Time = DateTime.Now;
        }

        public UploadedBatchInfo(string batchno)
        {
            this.BatchNo = batchno;
            this.Time = DateTime.Now;
        }
    }
}
