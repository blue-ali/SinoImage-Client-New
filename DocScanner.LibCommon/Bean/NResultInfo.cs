using DocScanner.Bean.pb;
using System;
using System.Collections.Generic;

namespace DocScanner.Bean
{
    public class NResultInfo
	{
		public string Msg
		{
			get;
			set;
		}

		public EResultStatus Status
		{
			get;
			set;
		}

		public string BatchNO
		{
			get;
			set;
		}

		public int CurFileIndex
		{
			get;
			set;
		}

		public string CurFileName
		{
			get;
			set;
		}

        public NBatchInfo BatchInfo
        {
            get;
            set;
        }

        private IList<string> _processingFileIds = new List<string>();
   
        public IList<string> ProcessingFileIds
        {
            get
            {
                return _processingFileIds;
            }
            set
            {
                this._processingFileIds = value;
            }
        }

        public static NResultInfo FromNetMsg(MsgResultInfo info)
		{
            NResultInfo resultInfo = new NResultInfo();
            resultInfo.Msg = info.Msg;
            resultInfo.Status = info.Status;
            resultInfo.BatchNO = info.BatchNO;
            resultInfo.CurFileIndex = resultInfo.CurFileIndex;
            resultInfo.CurFileName = resultInfo.CurFileName;
            resultInfo.ProcessingFileIds = info.ProcessingFileIdsList;
            resultInfo.BatchInfo = NBatchInfo.FromPBMsg(info.BatchInfo);
            return resultInfo;
		}

		public MsgResultInfo ToPBMsg()
		{
            MsgResultInfo.Builder builder = new MsgResultInfo.Builder();
            builder.Msg = this.Msg;
            builder.Status = this.Status;
            builder.BatchNO = this.BatchNO;
            builder.CurFileIndex = this.CurFileIndex;
            builder.CurFileName = this.CurFileName;
            return builder.BuildParsed();
		}

        public void EnsureResultSuccess()
        {
            if (this.Status == EResultStatus.eFailed)
                throw new Exception(this.Msg);
        }
    }
}
