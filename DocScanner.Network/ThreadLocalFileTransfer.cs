using System;
using System.Threading;
using DocScanner.Bean;
using DocScanner.LibCommon;

namespace DocScaner.Network
{
    public class ThreadLocalFileTransfer : INetTransfer, IDisposable
	{
		private INetTransfer _realimp;

		public event EventHandler<TEventArg<NetTransferNotifyMsg>> OnNotify;

		public ThreadLocalFileTransfer()
		{
			this._realimp = new HttpNetTransfer();
			this._realimp.OnNotify += this._realimp_OnNotify;
		}

		private void _realimp_OnNotify(object sender, TEventArg<NetTransferNotifyMsg> e)
		{
			bool flag = this.OnNotify != null;
			if (flag)
			{
				this.OnNotify(sender, e);
			}
		}

		public void Dispose()
		{
			this._realimp.Dispose();
			this.ClearEvents();
		}

		public NBatchInfo DownloadBatch(NQueryBatchInfo queryinfo)
		{
			ParameterizedThreadStart start = new ParameterizedThreadStart(this.ThreadJobDownImp);
			Thread thread = new Thread(start);
			thread.Start(queryinfo);
			return null;
		}

		private void ThreadJobDownImp(object pa)
		{
			this._realimp.DownloadBatch(pa as NQueryBatchInfo);
		}

		public NResultInfo UploadBatch(NBatchInfo info)
		{
			ParameterizedThreadStart start = new ParameterizedThreadStart(this.ThreadJobUploadImp);
			Thread thread = new Thread(start);
			thread.Start(info);
			return null;
		}

		private void ThreadJobUploadImp(object pa)
		{
			this._realimp.UploadBatch(pa as NBatchInfo);
		}

		public NBatchInfo GetDownloadBatchAysncResult()
		{
			bool flag = this._realimp != null;
			NBatchInfo result;
			if (flag)
			{
				result = this._realimp.GetDownloadBatchAysncResult();
			}
			else
			{
				result = null;
			}
			return result;
		}

		public NResultInfo GetUploadBatchAsyncResult()
		{
			bool flag = this._realimp != null;
			NResultInfo result;
			if (flag)
			{
				result = this._realimp.GetUploadBatchAsyncResult();
			}
			else
			{
				result = null;
			}
			return result;
		}

		public NBatchHisRsp GetBatchHis(NBatchHisQry qry)
		{
			bool flag = this._realimp != null;
			NBatchHisRsp result;
			if (flag)
			{
				result = this._realimp.GetBatchHis(qry);
			}
			else
			{
				result = null;
			}
			return result;
		}

		public NBatchHisRsp GetBatchHisAsyncResult()
		{
			throw new NotImplementedException();
		}
	}
}
