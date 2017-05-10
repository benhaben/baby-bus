using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Services;
using Cirrious.CrossCore;
using Newtonsoft.Json;
using Cirrious.CrossCore.Platform;
using Aliyun.OpenServices.OpenStorageService;

namespace BabyBus.Utilities {
    public class UploadImageData {

        public string FileName {
            get;
            set;
        }


        public byte[] ImageBytes {
            get;
            set;
        }


        public string Reason {
            get;
            set;
        }


        public bool IsSuccess {
            get;
            set;
        }

        public UploadImageData(bool result, string fileName, string reason, byte[] imageBytes) {
            FileName = fileName;
            ImageBytes = imageBytes;
            Reason = reason;
            IsSuccess = result;
        }
    }

    public class ImageSendHelper {

        /// <summary>
        /// Occurs when send one image finished.
        /// </summary>
        public event SendImageProgressResultCallBack SendImageProgressResultEventHandler;

        /// <summary>
        /// Send image progress result call back.
        /// </summary>
        public delegate void SendImageProgressResultCallBack(UploadImageData uploadImageData);

        //        private HttpClient _baseClient;

        //        public static HttpClient BaseClient {
        //            get { return _baseClient ?? (_baseClient = new HttpClient { BaseAddress = new Uri(Constants.BaseUrl) }); }
        //        }


        public UploadImageData UploadImage(UploadImageData uploadImageData) {
            var uri = new Uri(Constants.OSSEndPoint, UriKind.Absolute);
            var pictureService = Mvx.Resolve<IPictureService>();
            UploadImageData ret;
            try {
                OssClient client = new OssClient(uri, Constants.OSSKey, Constants.OSSSecret);
                ObjectMetadata meta = new ObjectMetadata();
                var hex2 = pictureService.GetMd5String(uploadImageData.ImageBytes);

                #if DEBUG
                //模拟随机失败的情况
                Random random = new Random();
                int r = random.Next(0, 1);
                if (r == 1)
                    hex2 += "1";
                #endif

                using (var ms = new MemoryStream(uploadImageData.ImageBytes)) {
                    meta.ContentLength = ms.Length;
                    //the 0th send image
                    var result = client.PutObject(Constants.BucketName, uploadImageData.FileName, ms, meta);
                    int snedTime = 1;
                    //this condition(ETag different) is very rare, but still keep logic here. 
                    //if wifi error occur, en exception will raise, user can re-try, so re-try here is not very useful
                    for (; ((result.ETag.ToUpper() != hex2.ToUpper()) && snedTime < Constants.MaxRetrySendImageTime); snedTime++) {
                        result = client.PutObject(Constants.BucketName, uploadImageData.FileName, ms, meta); //Re Sent Image
                    }
                    if (snedTime >= Constants.MaxRetrySendImageTime) {
                        //bigger than MaxRetrySendImageTime, sned failure
                        uploadImageData.Reason = string.Format("发送 图片<{1}> 超过{0}次失败", Constants.MaxRetrySendImageTime, uploadImageData.FileName);
                        uploadImageData.IsSuccess = false;
                        ret = uploadImageData;

                        Mvx.Trace(MvxTraceLevel.Diagnostic, uploadImageData.Reason);

                        if (SendImageProgressResultEventHandler != null) {
                            SendImageProgressResultEventHandler(uploadImageData);
                        }
                    } else {
                        uploadImageData.IsSuccess = true;
                        uploadImageData.Reason = string.Format("发送 图片<{1}> {0}次成功", snedTime, uploadImageData.FileName);
                        ret = uploadImageData;

                        Mvx.Trace(MvxTraceLevel.Diagnostic, uploadImageData.Reason);

                        if (SendImageProgressResultEventHandler != null) {
                            SendImageProgressResultEventHandler(uploadImageData);
                        }
                    }
                }
               
            } catch (Exception ex) {
                //TODO: use Xamarin insight to report exception
                uploadImageData.Reason = string.Format("发送 图片<{1}> 发生异常：{0}", ex.Message, uploadImageData.FileName);
                uploadImageData.IsSuccess = false;
                ret = uploadImageData;

                Mvx.Trace(MvxTraceLevel.Diagnostic, uploadImageData.Reason);

                if (SendImageProgressResultEventHandler != null) {
                    SendImageProgressResultEventHandler(uploadImageData);
                }
            } finally {
                #if DEBUG_ANDROID
                var e = Mvx.Resolve<IEnvironmentService>();
                e.Log(filename);
                #endif
            }
            return ret;
        }
    }
}
