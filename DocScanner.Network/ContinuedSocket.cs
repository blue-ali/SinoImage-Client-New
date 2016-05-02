using System;
using System.Net;
using System.Net.Sockets;

namespace DocScaner.Network
{
	internal class ContinuedSocket
	{
		private int _trycount = 5;

		private int _tryinterval = 500;

		private Socket _socket;

		private string _socketid = default(Guid).ToString();

		public int TryCount
		{
			get
			{
				return this._trycount;
			}
			set
			{
				this._trycount = value;
			}
		}

		public int TryInterVal
		{
			get
			{
				return this._tryinterval;
			}
			set
			{
				this._tryinterval = value;
			}
		}

		public string RemotAddress
		{
			get;
			set;
		}

		public int RemotePort
		{
			get;
			set;
		}

		public byte[] SentData
		{
			get;
			set;
		}

		public string SocketID
		{
			get
			{
				return this._socketid;
			}
		}

		public bool Send(byte[] data)
		{
			int num = 0;
			bool result;
			while (true)
			{
				try
				{
					bool flag = this._socket != null;
					if (flag)
					{
						this._socket.Close();
						this._socket = null;
					}
					this._socket = this.connectSocket();
					this._socket.Send(data);
				}
				catch (Exception)
				{
					num++;
					bool flag2 = num > this.TryCount;
					if (flag2)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		public bool Send(ContinuedSocketMsgBody data)
		{
			return this.Send(data.ToSocketBytes());
		}

		public bool Recive()
		{
			return false;
		}

		public void Close()
		{
		}

		private Socket connectSocket()
		{
			bool flag = this._socket != null && this._socket.Connected;
			Socket socket;
			if (flag)
			{
				socket = this._socket;
			}
			else
			{
				this._socket.Close();
				this._socket = null;
				IPHostEntry hostEntry = Dns.GetHostEntry(this.RemotAddress);
				IPAddress[] addressList = hostEntry.AddressList;
				for (int i = 0; i < addressList.Length; i++)
				{
					IPAddress address = addressList[i];
					IPEndPoint iPEndPoint = new IPEndPoint(address, this.RemotePort);
					Socket socket2 = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					socket2.Connect(iPEndPoint);
					bool connected = socket2.Connected;
					if (connected)
					{
						this._socket = socket2;
						break;
					}
				}
				socket = this._socket;
			}
			return socket;
		}

		private int QueryPostion()
		{
			this.Send(new ContinuedSocketMsgBody(this.SocketID)
			{
				SocketCMD = ESOCKETCMD.QUOERYPOS
			});
			return 0;
		}
	}
}
