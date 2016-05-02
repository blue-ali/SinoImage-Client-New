using System;
using System.IO;
using DocScanner.Bean;
using DocScanner.LibCommon;
using DocScanner.Bean.pb;

namespace DocScaner.Network
{
    public class LocalFileTransfer : INetTransfer, IDisposable
	{
		private NBatchInfo _downloadresult;

		private NResultInfo _uploadresult;

		public event EventHandler<TEventArg<NetTransferNotifyMsg>> OnNotify;

		public string LocalmodeServerDir
		{
			get
			{
				return AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "LocalmodeServerDir");
			}
		}

		NBatchInfo INetTransfer.DownloadBatch(NQueryBatchInfo queryinfo)
		{
			this._downloadresult = null;
			string fileName = this.GetFileName(queryinfo.BatchNO);
			NBatchInfo nBatchInfo = new NBatchInfo();
			this.ReportMsg(ENetTransferStatus.Start, queryinfo.BatchNO, "", 0.0, 0.0);
			bool flag = File.Exists(fileName);
			if (flag)
			{
				nBatchInfo = NBatchInfo.FromPBFile(fileName);
				nBatchInfo.Operation = EOperType.eFROM_SERVER_NOTCHANGE;
				foreach (NFileInfo current in nBatchInfo.FileInfos)
				{
					current.Operation = EOperType.eFROM_SERVER_NOTCHANGE;
				}
				this._downloadresult = nBatchInfo;
				this.ReportMsg(ENetTransferStatus.Success, queryinfo.BatchNO, "完成", 0.0, 0.0);
			}
			else
			{
				this.ReportMsg(ENetTransferStatus.Error, queryinfo.BatchNO, "服务器不存在此批次信息", 0.0, 0.0);
				nBatchInfo.ResultInfo = new NResultInfo
				{
					Status = EResultStatus.eFailed,
					Msg = "服务器不存在此批次信息"
				};
			}
			return nBatchInfo;
		}

		NResultInfo INetTransfer.UploadBatch(NBatchInfo info)
		{
			this.ReportMsg(ENetTransferStatus.Start, info.BatchNO, "", 0.0, 0.0);
			string fileName = this.GetFileName(info.BatchNO);
			info.ToPBFile(fileName, true);
			AppContext.Cur.MS.LogDebug("localserver mode save to " + fileName);
			this.ReportMsg(ENetTransferStatus.Success, info.BatchNO, "", 0.0, 0.0);
			NResultInfo nResultInfo = new NResultInfo();
			nResultInfo.Status = EResultStatus.eSuccess;
			this._uploadresult = nResultInfo;
			return nResultInfo;
		}

		public string GetFileName(string batchno)
		{
			return Path.Combine(this.LocalmodeServerDir, batchno) + ".pbope";
		}

		public void ReportMsg(ENetTransferStatus status, string batchno, string msg, double speed = 0.0, double percent = 0.0)
		{
			bool flag = this.OnNotify != null;
			if (flag)
			{
				this.OnNotify(this, new TEventArg<NetTransferNotifyMsg>(new NetTransferNotifyMsg(status, batchno, msg, speed, percent)));
			}
		}

		public void Dispose()
		{
			this.ClearEvents();
		}

		public NBatchInfo GetDownloadBatchAysncResult()
		{
			return this._downloadresult;
		}

		public NResultInfo GetUploadBatchAsyncResult()
		{
			return this._uploadresult;
		}

		public NBatchHisRsp GetBatchHis(NBatchHisQry qry)
		{
			throw new NotImplementedException();
		}

		public NBatchHisRsp GetBatchHisAsyncResult()
		{
			throw new NotImplementedException();
		}
	}
}
