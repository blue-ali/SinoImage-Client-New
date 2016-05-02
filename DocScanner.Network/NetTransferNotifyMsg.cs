using System;

namespace DocScaner.Network
{
	public class NetTransferNotifyMsg
	{
		public string Msg
		{
			get;
			set;
		}

		public string CurBatchNO
		{
			get;
			set;
		}

		public ENetTransferStatus Status
		{
			get;
			set;
		}

		public double Speed
		{
			get;
			set;
		}

		public double Percent
		{
			get;
			set;
		}

		public NetTransferNotifyMsg(ENetTransferStatus status, string Batchno, string msg, double speed = 0.0, double percent = 0.0)
		{
			this.Msg = msg;
			this.Status = status;
			this.CurBatchNO = Batchno;
			this.Speed = speed;
			this.Percent = percent;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.CurBatchNO,
				":",
				this.Status,
				":",
				this.Msg
			});
		}
	}
}
