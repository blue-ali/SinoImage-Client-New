using DocScaner.Common;
using DocScanner.Bean;
using DocScanner.Bean.pb;
using DocScanner.LibCommon;
using Logos.DocScaner.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

namespace DocScaner.Network
{
    internal class HttpNetTransfer : INetTransfer, IDisposable
	{
		private NBatchInfo _downloadresult;

		private NResultInfo _uploadresult;

		private NBatchHisRsp _batchhisrspresult;

		public event EventHandler<TEventArg<NetTransferNotifyMsg>> OnNotify;

		public string Localdownloaddir
		{
			get
			{
				return AppContext.Cur.Cfg.GetConfigParamValue("AppSetting", "TmpFileDir");
			}
		}

		public NBatchInfo DownloadBatch(NQueryBatchInfo queryinfo)
		{
			NBatchInfo nBatchInfo = new NBatchInfo();
			nBatchInfo.BatchNO = queryinfo.BatchNO;
			NResultInfo resultInfo = new NResultInfo();
			this._downloadresult = null;
			string text = HttpUtil.GetHttpDownloadURL();
			this.ReportMsg(ENetTransferStatus.Start, queryinfo.BatchNO, "", 0.0, 0.0);
			try
			{
				text = string.Concat(new object[]
				{
					text,
					"?batchno=",
					queryinfo.BatchNO,
					"&version=",
					queryinfo.Version
				});
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
				httpWebRequest.Method = "POST";
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				MsgBatchInfo input = MsgBatchInfo.ParseFrom(responseStream);
				NBatchInfo nBatchInfo2 = NBatchInfo.FromPBMsg(input);
				if (!nBatchInfo2.HasError())
				{
					if (nBatchInfo2.FileInfos != null)
					{
						foreach (NFileInfo current in nBatchInfo2.FileInfos)
						{
							string text2 = Path.Combine(this.Localdownloaddir, nBatchInfo2.BatchNO);
							Directory.CreateDirectory(text2);
							string text3 = Path.Combine(text2, FileHelper.GetFileName(current.FileName));
							bool flag3 = string.IsNullOrEmpty(current.FileURL);
							if (flag3)
							{
								current.FileURL = HttpUtil.GetHttpDownloadURL() + nBatchInfo2.BatchNO + "/" + current.FileName;
							}
							bool flag4 = (current.Data == null || current.Data.Length == 0) && current.FileURL.IsHttpURL();
							if (flag4)
							{
								bool flag5 = HttpbrokenDownloader.NormalDownloadFile(text3, current.FileURL);
							}
							else
							{
								bool flag6 = current.Data != null;
								if (flag6)
								{
									current.ExPortDataToFile(text3);
								}
							}
							current.FileName = FileHelper.GetFileName(text3);
							current.LocalPath = text3;
							TmpFileMgr.AddTmpFile(text3);
							TmpDirMgr.AddTmpDir(text2);
						}
					}
					nBatchInfo = nBatchInfo2;
					this._downloadresult = nBatchInfo;
					this.ReportMsg(ENetTransferStatus.Success, queryinfo.BatchNO, "", 0.0, 0.0);
				}
				else
				{
					this.ReportMsg(ENetTransferStatus.Error, queryinfo.BatchNO, nBatchInfo2.ResultInfo.Msg, 0.0, 0.0);
				}
			}
			catch (Exception ex)
			{
				this.ReportMsg(ENetTransferStatus.Error, queryinfo.BatchNO, ex.Message, 0.0, 0.0);
				nBatchInfo.ResultInfo = new NResultInfo();
				nBatchInfo.ResultInfo.Status = EResultStatus.eFailed;
				nBatchInfo.ResultInfo.Msg = ex.Message;
				bool flag7 = ex.Message == "Unable to connect to the remote server";
				if (flag7)
				{
					nBatchInfo.ResultInfo.Msg = "远程服务器未启动";
				}
				nBatchInfo.ResultInfo = resultInfo;
			}
			return nBatchInfo;
		}

		public NResultInfo UploadBatch(NBatchInfo batch)
		{
			NResultInfo nResultInfo = new NResultInfo();
			nResultInfo.Status = EResultStatus.eSuccess;
			NResultInfo result;
			try
			{
				string text = Path.Combine(this.Localdownloaddir, string.Concat(new object[]
				{
					batch.BatchNO,
					"[",
					batch.Version,
					"].pbope"
				}));
				bool flag = AppContext.Cur.Cfg.GetConfigParamValue("NetSetting", "IncludeFileData").ToBool();
				batch.ToPBFile(text, flag);
				TmpFileMgr.AddTmpFile(text);
				this.ReportMsg(ENetTransferStatus.Start, batch.BatchNO, "", 0.0, 0.0);
				string httpUploadURL = HttpUtil.GetHttpUploadURL();
				Console.WriteLine("upload url" + httpUploadURL);
				HttpWebResponse resp = HttpFormUpload.UploadFile(text, httpUploadURL);
				this._uploadresult = HttpNetTransfer.ParseWebResponse(resp);
				if (this._uploadresult.Status > EResultStatus.eSuccess)
				{
					this.ReportMsg(ENetTransferStatus.Error, batch.BatchNO, this._uploadresult.Msg, 0.0, 0.0);
					result = this._uploadresult;
					return result;
				}
				if (!flag)
				{
					foreach (NFileInfo current in batch.FileInfos)
					{
						if (current.Operation == EOperType.eADD || current.Operation == EOperType.eUPD)
						{
							current.AttatchFileData(current.LocalPath);
							string text2 = string.Concat(new object[]
							{
								FileHelper.GetFileName(current.LocalPath),
								"[",
								current.Version,
								"].pbdata"
							});
							current.ToPBFile(text2, true);
							this.ReportMsg(ENetTransferStatus.OnProgress, batch.BatchNO, "上传" + current.LocalPath, 0.0, 0.0);
							resp = HttpFormUpload.UploadFile(text2, httpUploadURL);
							this._uploadresult = HttpNetTransfer.ParseWebResponse(resp);
							bool flag5 = this._uploadresult.Status == EResultStatus.eSuccess;
							if (!flag5)
							{
								this.ReportMsg(ENetTransferStatus.Error, batch.BatchNO, "上传" + current.LocalPath + this._uploadresult.Msg, 0.0, 0.0);
								result = this._uploadresult;
								return result;
							}
							this.ReportMsg(ENetTransferStatus.Success, batch.BatchNO, "上传" + current.LocalPath, 0.0, 0.0);
							TmpFileMgr.AddTmpFile(text2);
						}
					}
				}
				this.ReportMsg(ENetTransferStatus.Success, batch.BatchNO, "", 0.0, 0.0);
			}
			catch (WebException ex)
			{
				bool flag6 = ex.Status == WebExceptionStatus.ConnectFailure;
				if (flag6)
				{
					this.ReportMsg(ENetTransferStatus.Error, batch.BatchNO, "远程服务器无法连接", 0.0, 0.0);
					nResultInfo.Status = EResultStatus.eFailed;
					nResultInfo.Msg = "远程服务器无法连接";
				}
				bool flag7 = ex.Status == WebExceptionStatus.ProtocolError;
				if (flag7)
				{
					this.ReportMsg(ENetTransferStatus.Error, batch.BatchNO, "在服务器无法找到对应服务", 0.0, 0.0);
					nResultInfo.Status = EResultStatus.eFailed;
					nResultInfo.Msg = "在服务器无法找到对应服务";
				}
				else
				{
					nResultInfo.Status = EResultStatus.eFailed;
					nResultInfo.Msg = ex.Message;
				}
			}
			this._uploadresult = nResultInfo;
			result = nResultInfo;
			return result;
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
			return this._uploadresult;
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
