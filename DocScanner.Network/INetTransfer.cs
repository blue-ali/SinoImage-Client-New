using DocScanner.Bean;
using System;

namespace DocScaner.Network
{
    public interface INetTransfer : IDisposable
	{
		event EventHandler<TEventArg<NetTransferNotifyMsg>> OnNotify;

		NBatchInfo DownloadBatch(NQueryBatchInfo queryinfo);

		NBatchInfo GetDownloadBatchAysncResult();

		NResultInfo UploadBatch(NBatchInfo info);

		NResultInfo GetUploadBatchAsyncResult();

		NBatchHisRsp GetBatchHis(NBatchHisQry qry);

		NBatchHisRsp GetBatchHisAsyncResult();
	}
}
