using System;
using System.IO;
using System.Text;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Aliyun.OpenServices.OpenStorageService;
using System.Text.RegularExpressions;
using System.Net;
using Aliyun.OpenServices;

namespace BabyBus.Logic.Shared
{
	public class UploadImageData
	{

		public string RemoteImageName {
			get;
			set;
		}


		public string LocalImageName {
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

		public UploadImageData(bool result, string fileName, string reason, string localImageName)
		{
			RemoteImageName = fileName;
			LocalImageName = localImageName;
			Reason = reason;
			IsSuccess = result;
		}

		public UploadImageData(bool result, string fileName, string reason, byte[] imageBytes)
		{
			RemoteImageName = fileName;
			ImageBytes = imageBytes;
			Reason = reason;
			IsSuccess = result;
			LocalImageName = null;
		}
	}

	public class ImageSendHelper
	{

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

		/// <summary>
		/// 以GET 形式获取数据
		/// </summary>
		/// <param name="RequestPara"></param>
		/// <param name="Url"></param>
		/// <returns></returns>

		public static  string GetData(string url)
		{
			string ReturnVal = null;
			WebRequest hr = WebRequest.Create(url);
			hr.Timeout = 2000;
			hr.Method = "GET";
			System.Net.WebResponse response = hr.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
			ReturnVal = reader.ReadToEnd();
			reader.Close();
			response.Close();
			return ReturnVal;
		}

		public static DateTime GetNowTime()
		{
			try {

//                添加一个授时服务器，香港天文台的
//                http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=随机种子
//                Random random = new Random();

				string data = GetData("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr");
				if (data != null) {
					Regex regex = new Regex(@"0=(?<timestamp>\d{10})\d+");
					Match match = regex.Match(data);
					if (match.Success) {
						return  GetTime(match.Groups["timestamp"].Value);
					} else {
						return DateTime.UtcNow;
					}
				} else {
					return DateTime.UtcNow;
				}
			} catch (Exception ex) {
				MvxTrace.Trace("获得网络时间失败" + ex.Message);
				return DateTime.UtcNow;
			}
		}

       

		/// <summary>
		/// 时间戳转为C#格式时间
		/// </summary>
		/// <param name=”timeStamp”></param>
		/// <returns></returns>
		private static DateTime GetTime(string timeStamp)
		{
			DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			long lTime = long.Parse(timeStamp + "0000000");
			TimeSpan toNow = new TimeSpan(lTime);
			return dtStart.Add(toNow);
		}

		public void InitOssClient()
		{
			var date = GetNowTime();
			var epoch = (date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
			var conf = new ClientConfiguration();
			conf.SetCustomEpochTicks(epoch);
			var uri = new Uri(Constants.OSSEndPoint, UriKind.Absolute);
			_client = new OssClient(uri, Constants.OSSKey, Constants.OSSSecret, conf);
		}

		OssClient _client;

		public UploadImageData UploadImage(UploadImageData uploadImageData)
		{
			UploadImageData ret;
			if (_client == null) {
				Mvx.Trace("InitOssClient shuold be called!");
				return uploadImageData;
			}

			try {
				var pictureService = Mvx.Resolve<IPictureService>();
               
				ObjectMetadata meta = new ObjectMetadata();
				byte[] imageBytes;
				if (uploadImageData.LocalImageName != null) {
					imageBytes = pictureService.GetImageBytesFromFile(uploadImageData.LocalImageName).GetAwaiter().GetResult();
				} else {
					imageBytes = uploadImageData.ImageBytes;
				}
                    
				var hex2 = pictureService.GetMd5String(imageBytes);

				#if DEBUG
				//模拟随机失败的情况
				Random random = new Random();
				int r = random.Next(0, 1);
				if (r == 1)
					hex2 += "1";
				#endif

				using (var ms = new MemoryStream(imageBytes)) {
					meta.ContentLength = ms.Length;
					//the 0th send image
					var result = _client.PutObject(Constants.BucketName, uploadImageData.RemoteImageName, ms, meta);
					int snedTime = 1;
					//this condition(ETag different) is very rare, but still keep logic here. 
					//if wifi error occur, en exception will raise, user can re-try, so re-try here is not very useful
					for (; ((result.ETag.ToUpper() != hex2.ToUpper()) && snedTime < Constants.MaxRetrySendImageTime); snedTime++) {
						result = _client.PutObject(Constants.BucketName, uploadImageData.RemoteImageName, ms, meta); //Re Sent Image
					}
					if (snedTime >= Constants.MaxRetrySendImageTime) {
						//bigger than MaxRetrySendImageTime, sned failure
						uploadImageData.Reason = string.Format("发送 图片<{1}> 超过{0}次失败", Constants.MaxRetrySendImageTime, uploadImageData.RemoteImageName);
						uploadImageData.IsSuccess = false;
						ret = uploadImageData;

						Mvx.Trace(MvxTraceLevel.Diagnostic, uploadImageData.Reason);

						if (SendImageProgressResultEventHandler != null) {
							SendImageProgressResultEventHandler(uploadImageData);
						}
					} else {
						uploadImageData.IsSuccess = true;
						uploadImageData.Reason = string.Format("发送 图片<{1}> {0}次成功", snedTime, uploadImageData.RemoteImageName);
						ret = uploadImageData;

						Mvx.Trace(MvxTraceLevel.Diagnostic, uploadImageData.Reason);

						if (SendImageProgressResultEventHandler != null) {
							SendImageProgressResultEventHandler(uploadImageData);
						}
					}
				}//clear stream
			} catch (Exception ex) {
				//TODO: use Xamarin insight to report exception
				uploadImageData.Reason = string.Format("发送 图片<{1}> 发生异常：{0}", ex.Message, uploadImageData.RemoteImageName);
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
