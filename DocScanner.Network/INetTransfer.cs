using DocScanner.Bean;
using System;

namespace DocScanner.Network
{
    public interface INetTransfer : IDisposable
	{
		event EventHandler<TEventArg<NetTransferNotifyMsg>> OnNotify;

		NBatchInfo DownloadBatch(NQueryBatchInfo queryinfo);

		NBatchInfo GetDownloadBatchAysncResult();

		void UploadBatch(NBatchInfo info);

		NResultInfo GetUploadBatchAsyncResult();

		NBatchHisRsp GetBatchHis(NBatchHisQry qry);

		NBatchHisRsp GetBatchHisAsyncResult();
	}
}
