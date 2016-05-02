using System;

namespace DocScaner.Network
{
	public class ContinuedSocketMsgBody
	{
		public int Length
		{
			get;
			set;
		}

		public byte[] Data
		{
			get;
			set;
		}

		public string ClientID
		{
			get;
			set;
		}

		public ESOCKETCMD SocketCMD
		{
			get;
			set;
		}

		public ContinuedSocketMsgBody(string id)
		{
			this.ClientID = id;
		}

		public byte[] ToSocketBytes()
		{
			return null;
		}

		public ContinuedSocketMsgBody FromSocketBytes(byte[] data)
		{
			return null;
		}
	}
}
