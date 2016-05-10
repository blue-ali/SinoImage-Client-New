using DocScanner.Bean;
using DocScanner.Bean.pb;
using DocScanner.LibCommon;
using DocScanner.Network.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace DocScanner.Network
{
    internal class HttpNetTransfer : INetTransfer, IDisposable
	{
		private NBatchInfo _downloadresult;

		//private NResultInfo _uploadresult;

		private NBatchHisRsp _batchhisrspresult;

		public event EventHandler<TEventArg<NetTransferNotifyMsg>> OnNotify;

		public string Localdownloaddir
		{
			get
			{
				return AppContext.GetInstance().Config.GetConfigParamValue("AppSetting", "TmpFileDir");
			}
		}

		public NBatchInfo DownloadBatch(NQueryBatchInfo queryinfo)
		{
			//NBatchInfo nBatchInfo = new NBatchInfo();
			//nBatchInfo.BatchNO = queryinfo.BatchNO;
			NResultInfo resultInfo = new NResultInfo();
			this._downloadresult = null;
			string url = HttpUtil.GetHttpGetBatchURL(queryinfo.BatchNO);
			this.ReportMsg(ENetTransferStatus.Start, queryinfo.BatchNO, "", 0.0, 0.0);
			try
			{
                NBatchInfo batchInfo = HttpClientManager.GetBatch(queryinfo.BatchNO);   //HTTP请求获取批次信息
                this._downloadresult = batchInfo;
				this.ReportMsg(ENetTransferStatus.Success, queryinfo.BatchNO, "", 0.0, 0.0);
			}
			catch (Exception ex)
			{
				//this.ReportMsg(ENetTransferStatus.Error, queryinfo.BatchNO, ex.Message, 0.0, 0.0);
				//nBatchInfo.ResultInfo = new NResultInfo();
				//nBatchInfo.ResultInfo.Status = EResultStatus.eFailed;
				//nBatchInfo.ResultInfo.Msg = ex.Message;
				//if (ex.Message == "Unable to connect to the remote server")
				//{
				//	nBatchInfo.ResultInfo.Msg = "远程服务器未启动";
				//}
				//nBatchInfo.ResultInfo = resultInfo;

                this.ReportMsg(ENetTransferStatus.Error, queryinfo.BatchNO, ExceptionHelper.GetFirstException(ex).Message, 0.0, 0.0);
            }
			return null;
		}

		public void UploadBatch(NBatchInfo batch)
		{
			NResultInfo nResultInfo = new NResultInfo();
			nResultInfo.Status = EResultStatus.eSuccess;
			NResultInfo result;
			try
			{
				string transMode = AppContext.GetInstance().Config.GetConfigParamValue("NetSetting", "TransMode");
                //batch.TransMode = (ETransMode)Enum.Parse(typeof(ETransMode), transMode);
                if (transMode.Equals(ConstString.TRANSMODE_FULL))
                {
                    HttpClientManager.FullUpload(batch);   //完全方式提交
                }
                else if (transMode.Equals(ConstString.TRANSMODE_BROKE))
                {
                    HttpClientManager.BroekUpload(batch);  //断点方式提交
                }
				this.ReportMsg(ENetTransferStatus.Success, batch.BatchNO, "", 0.0, 0.0);
			}
			//catch (WebException ex)
			//{
			//	this.ReportMsg(ENetTransferStatus.Error, batch.BatchNO, ExceptionHelper.GetFirstException(ex).Message, 0.0, 0.0);
			//}
            catch (Exception e)
            {
                this.ReportMsg(ENetTransferStatus.Error, batch.BatchNO, ExceptionHelper.GetFirstException(e).Message, 0.0, 0.0);
            }
			//this._uploadresult = nResultInfo;
			//result = nResultInfo;
			//return result;
		}

		public static NResultInfo ParseWebResponse(HttpWebResponse resp)
		{
			NResultInfo result;
			try
			{
				MsgResultInfo info = MsgResultInfo.ParseFrom(resp.GetResponseStream());
				NResultInfo nResultInfo = NResultInfo.FromNetMsg(info);
				result = nResultInfo;
			}
			catch (Exception ex)
			{
				result = new NResultInfo
				{
					Status = EResultStatus.eFailed,
					Msg = ex.ToString()
				};
			}
			return result;
		}

		public void ReportMsg(ENetTransferStatus status, string batchno, string msg, double speed = 0.0, double pencent = 0.0)
		{
			bool flag = this.OnNotify != null;
			if (flag)
			{
				this.OnNotify(this, new TEventArg<NetTransferNotifyMsg>(new NetTransferNotifyMsg(status, batchno, msg, speed, pencent)));
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
            //return this._uploadresult;
            return null;
		}

		public NBatchHisRsp GetBatchHis(NBatchHisQry qry)
		{
			return this._batchhisrspresult;
		}

		public NBatchHisRsp GetBatchHisAsyncResult()
		{
			return this._batchhisrspresult;
		}
	}
}
