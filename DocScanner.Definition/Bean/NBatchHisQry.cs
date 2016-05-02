using DocScanner.Bean.pb;

namespace DocScanner.Bean
{
    public class NBatchHisQry
	{
		public string User
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public string BatchNO
		{
			get;
			set;
		}

		public MsgBatchHisQry ToPBMsg()
		{
			return new MsgBatchHisQry.Builder
			{
				User1 = this.User,
				Password2 = this.Password,
				BatchNO3 = this.BatchNO
			}.BuildParsed();
		}
	}
}
