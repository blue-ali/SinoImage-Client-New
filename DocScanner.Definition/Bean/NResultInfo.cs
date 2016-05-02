using DocScanner.Bean.pb;

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

		public static NResultInfo FromNetMsg(MsgResultInfo info)
		{
			return new NResultInfo
			{
				Msg = info.Msg,
				Status = info.Status,
				BatchNO = info.BatchNO,
				CurFileIndex = info.CurFileIndex,
				CurFileName = info.CurFileName
			};
		}

		public MsgResultInfo ToPBMsg()
		{
			return new MsgResultInfo.Builder
			{
				Msg = this.Msg,
				Status = this.Status,
				BatchNO = this.BatchNO,
				CurFileIndex = this.CurFileIndex,
				CurFileName = this.CurFileName
			}.BuildParsed();
		}
	}
}
