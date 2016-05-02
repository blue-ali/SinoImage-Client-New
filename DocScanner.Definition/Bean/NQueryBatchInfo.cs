using DocScanner.Bean.pb;
using System.ComponentModel;
using System.IO;

namespace DocScanner.Bean
{
    public class NQueryBatchInfo
	{
		[Category("查询信息"), DisplayName("查询者")]
		public string User1
		{
			get;
			set;
		}

		[Category("查询信息"), DisplayName("所属机构")]
		public string Branch
		{
			get;
			set;
		}

		[Category("查询信息"), DisplayName("查询时间")]
		public int QueryTime
		{
			get;
			set;
		}

		[Category("查询信息"), DisplayName("查询日期")]
		public int QueryDate
		{
			get;
			set;
		}

		[Category("查询信息"), DisplayName("查询批次编号")]
		public string BatchNO
		{
			get;
			set;
		}

		[Browsable(false), Category("查询信息"), DisplayName("查询密码")]
		public string Password
		{
			get;
			set;
		}

		[Browsable(false), Category("查询信息"), DisplayName("登录帐号IP")]
		public string SourceIP
		{
			get;
			set;
		}

		[Browsable(false), Category("查询信息"), DisplayName("登录帐号机器ID")]
		public string MachineID
		{
			get;
			set;
		}

		public int Version
		{
			get;
			set;
		}

		public NQueryBatchInfo()
		{
			this.User1 = "";
			this.BatchNO = "";
			this.SourceIP = "";
			this.MachineID = "";
			this.Password = "";
			this.Version = 1;
		}

		public MsgQueryBatchInfo ToPBMsg()
		{
			return new MsgQueryBatchInfo.Builder
			{
				User1 = this.User1,
				Password2 = this.Password,
				QueryDate4 = this.QueryDate,
				QueryTime5 = this.QueryTime,
				BatchNO6 = this.BatchNO,
				SourceIP42 = this.SourceIP,
				MachineID44 = this.MachineID,
				Version7 = this.Version
			}.BuildParsed();
		}

		public void ToPBFile(string fname)
		{
			MsgQueryBatchInfo msgQueryBatchInfo = this.ToPBMsg();
			File.WriteAllBytes(fname, msgQueryBatchInfo.ToByteArray());
		}
	}
}
