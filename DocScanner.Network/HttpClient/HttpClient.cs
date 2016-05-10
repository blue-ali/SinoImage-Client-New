using DocScanner.Bean;
using DocScanner.Bean.pb;
using DocScanner.LibCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DocScanner.Network.Http
{
    class HttpClientManager
    {
        private static async Task<byte[]> PostWithContent(string url, byte[] fileBytes, string batchNo)
        {
            try
            {
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                content.Add(new StreamContent(new MemoryStream(fileBytes)), "upload", batchNo + ".pb");
                client.Timeout = TimeSpan.FromSeconds(600);

                HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsByteArrayAsync().Result;
            }
            catch (HttpRequestException e)
            {
                string exceptionMsg = "";
                if (e.Message.IndexOf("404") != -1)
                {
                    exceptionMsg = "NOT FOUND 404：没有找到对应服务";
                }
                else
                {
                    exceptionMsg = ExceptionHelper.GetFirstException(e).Message;
                }
                throw new WebException(exceptionMsg);
            }
            catch (TaskCanceledException) //超时异常
            {
                throw new WebException("与服务器连接超时");
            }
            catch (Exception e)
            {
                throw new WebException(ExceptionHelper.GetFirstException(e).Message);
            }

        }

        private static async Task<byte[]> Post(string url, string batchNo)
        {
            try
            {
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //content.Add(new StreamContent(new MemoryStream(fileBytes)), "upload", batchNo + ".pb");
                client.Timeout = TimeSpan.FromSeconds(600);

                //string url = HttpUtil.GetHttpUploadURL();
                HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                //.ConfigureAwait(false);
                //byte[] resultBytes = response.Content.ReadAsByteArrayAsync().Result;
                //NResultInfo resultInfo = NResultInfo.FromNetMsg(MsgResultInfo.ParseFrom(resultBytes));

                return response.Content.ReadAsByteArrayAsync().Result;
            }
            catch (HttpRequestException e)
            {
                string exceptionMsg = "";
                if (e.Message.IndexOf("404") != -1)
                {
                    exceptionMsg = "NOT FOUND 404：没有找到对应服务";
                }
                else
                {
                    exceptionMsg = e.Message;
                }
                throw new WebException(exceptionMsg);
            }
            catch (TaskCanceledException e) //超时异常
            {
                throw new WebException("与服务器连接超时");
            }
            catch (Exception e)
            {
                throw new WebException(ExceptionHelper.GetFirstException(e).Message);
            }

        }
        //return !string.IsNullOrWhiteSpace(input) ? Regex.Match(input, @"http://\w*\.directupload\.net/images/\d*/\w*\.[a-z]{3}").Value : null;

        /// <summary>
        /// 断点续传方式提交批次，通信两次，第一次提交批次和文件信息，第二次提交文件数据
        /// </summary>
        /// <param name="batchInfo"></param>
        /// <returns></returns>
        public static void BroekUpload(NBatchInfo batchInfo)
        {
            batchInfo.Status = EBatchStatus.NEW;
            string url = HttpUtil.GetHttpBrokeUploadBatchURL();
            byte[] batchBytes = batchInfo.ToPbMsgWithoutData().ToByteArray();
            //提交批次信息
            byte[] resultBytes = PostWithContent(url, batchBytes, batchInfo.BatchNO).Result;
            NResultInfo resultInfo = NResultInfo.FromNetMsg(MsgResultInfo.ParseFrom(resultBytes));
            resultInfo.EnsureResultSuccess();

            IList<string> processingFileIds = resultInfo.ProcessingFileIds;
            //List<NFileInfo> processingFileInfos = batchInfo.FileInfos.Where(x => processingFileIds.Contains(x.FileNO)).ToList();   //选出处理中的文件继续提交
            batchInfo.FileInfos = batchInfo.FileInfos.Where(x => processingFileIds.Contains(x.FileNO)).ToList();   //选出处理中的文件继续提交

            foreach (NFileInfo fileInfo in batchInfo.FileInfos) //提交剩余文件
            {
                batchInfo.Status = EBatchStatus.PROCESSING;
                //url = HttpUtil.GetHttpBrokeUploadFileURL();
                batchBytes = batchInfo.ToPbMsgWithData().ToByteArray();
                resultBytes = PostWithContent(url, batchBytes, batchInfo.BatchNO).Result;
                resultInfo = NResultInfo.FromNetMsg(MsgResultInfo.ParseFrom(resultBytes));
                resultInfo.EnsureResultSuccess();
            }
            //发送批次完成请求
            url = HttpUtil.GetHttpFinishBrokeBatchURL(batchInfo.BatchNO);
            resultBytes = Post(url, batchInfo.BatchNO).Result;
            resultInfo = NResultInfo.FromNetMsg(MsgResultInfo.ParseFrom(resultBytes));
            resultInfo.EnsureResultSuccess();
        }

        /// <summary>
        /// 全批次上传，只通信一次，批次信息+批次数据
        /// </summary>
        /// <param name="batchInfo"></param>
        public static void FullUpload(NBatchInfo batchInfo)
        {
            string url = HttpUtil.GetHttpFullUploadBatchURL();
            byte[] batchBytes = batchInfo.ToPbMsgWithData().ToByteArray();
            //提交批次信息
            byte[] resultBytes = PostWithContent(url, batchBytes, batchInfo.BatchNO).Result;
            NResultInfo resultInfo = NResultInfo.FromNetMsg(MsgResultInfo.ParseFrom(resultBytes));
        }

        public static NBatchInfo GetBatch(string batchNo)
        {
            string url = HttpUtil.GetHttpGetBatchURL(batchNo);
            //提交批次信息
            byte[] resultBytes = Post(url, batchNo).Result;

            NResultInfo resultInfo = NResultInfo.FromNetMsg(MsgResultInfo.ParseFrom(resultBytes));

            resultInfo.EnsureResultSuccess();

            NBatchInfo batchInfo = resultInfo.BatchInfo;
            IList<NFileInfo> fileInfos = batchInfo.FileInfos;

            string tmpDir = AppContext.GetInstance().Config.GetConfigParamValue("AppSetting", "TmpFileDir");
            string batchDir = Path.Combine(tmpDir, batchInfo.BatchNO);
            Directory.CreateDirectory(batchDir);
            TmpDirMgr.AddTmpDir(batchDir);

            foreach (NFileInfo fileInfo in fileInfos)
            {
                string filePath = Path.Combine(batchDir, fileInfo.FileNO);
                fileInfo.LocalPath = filePath;
                File.WriteAllBytes(filePath, fileInfo.Data);
                TmpFileMgr.AddTmpFile(filePath);
            }
            return batchInfo;
        }


    }

}
