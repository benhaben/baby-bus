using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Provider;
using Android.Support.V4.Util;
using BabyBus.Droid.Utils;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using Java.Lang;
using Exception = System.Exception;
using FileNotFoundException = Java.IO.FileNotFoundException;
using IOException = Java.IO.IOException;
using Math = System.Math;
using Uri = Android.Net.Uri;
using BabyBus.Droid.Utils.Image;
using Xamarin;
using BabyBus.Logic.Shared;
using System.Threading;

// Analysis disable once CheckNamespace


namespace BabyBus.Droid.Services
{
	public class PictureService : IDroidPictureService
	{
		public static LruCache ImageCahce;
		public static IEnvironmentService Env;
      
		TaskFactory factory;
		CancellationTokenSource cts = new CancellationTokenSource();

		public PictureService()
		{  
			// Create a scheduler that uses two threads.
			LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(2);
			// Create a TaskFactory and pass it our custom scheduler.
			factory = new TaskFactory(lcts);

			var maxMemory = (int)(Runtime.GetRuntime().MaxMemory() / 1024);
			ImageCahce = new LruCache(maxMemory / 8);
			Env = Mvx.Resolve<IEnvironmentService>();
		}

		public string GetMd5String(byte[] bytes)
		{
			System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] source = md5.ComputeHash(bytes);
			var sb1 = new StringBuilder();
			for (int i = 0; i < source.Length; i++) {
				sb1.Append(source[i].ToString("x2"));
			}

			return sb1.ToString();
		}

		public Task<byte[]> GetImageBytesFromFile(string fileName)
		{
			var tcs = new TaskCompletionSource<byte[]>();
			var bmp = ImageRevition.RevitionImageSize(fileName, 360, 360);
			var ms = new MemoryStream();
			bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
			tcs.TrySetResult(ms.ToArray());

			return tcs.Task;
		}

		public string SaveImageInFile(string fileName, byte[] bytes)
		{
			string filePath = string.Empty;
			try {
				filePath = Env.GetPicturePathAndName(fileName);
				FileMode fileMode = Env.FileExists(filePath) ? FileMode.Truncate : FileMode.Create;
				var outStream = new FileStream(filePath, fileMode, FileAccess.Write,
					                FileShare.Delete);
				outStream.Write(bytes, 0, bytes.Length);
				outStream.Flush(true);
				outStream.Close();
			} catch (FileNotFoundException e) {
				e.PrintStackTrace();
			} catch (IOException e) {
				e.PrintStackTrace();
			} catch (Exception e) {
				MvxTrace.Trace(MvxTraceLevel.Error, "save image", e.Message);
			}
			return filePath;
		}

		public MemoryStream GetStreamFromFile(string filePath)
		{
			var ret = new MemoryStream();

			//DecodeByteArray
			Bitmap bmp = BitmapFactory.DecodeFile(filePath);
			if (bmp == null) {
				MvxTrace.Trace(MvxTraceLevel.Diagnostic, "decode file", "return null");
				return null;
			}
			try {
				bmp.Compress(Bitmap.CompressFormat.Png, 100, ret);
			} catch (FileNotFoundException e) {
				e.PrintStackTrace();
			} catch (IOException e) {
				e.PrintStackTrace();
			} catch (Exception e) {
				MvxTrace.Trace(MvxTraceLevel.Error, "compress image", e.Message);
			}
			return ret;
		}



		public void LoadIamgeFromSource(string fileName, Action<Stream> imageAvailable,
		                                string path = Constants.ImageServerPath)
		{
			if (string.IsNullOrEmpty(fileName))
				return;
			factory.StartNew(() => {
				var stream = new MemoryStream();
				//Lode From Cache
				var value = (ObjectWrapper<MemoryStream>)ImageCahce.Get(fileName);
				if (value != null) {
					stream = value.Value;
					imageAvailable(stream);
				} else {
					//Load From Local
					var e = new EnvironmentService();
					string fullFileName = e.GetPicturePathAndName(fileName);

					if (e.FileExists(fullFileName)) {
						stream = GetStreamFromFile(fullFileName);
						var wrapper = new ObjectWrapper<MemoryStream>(stream);
						ImageCahce.Put(fileName, wrapper);
						imageAvailable(stream);
					} else {
						var download = Mvx.Resolve<IMvxHttpFileDownloader>();
						string serverpath = path + fileName;
						download.RequestDownload(serverpath, fullFileName,
							() => {
								stream = GetStreamFromFile(fullFileName);
								var wrapper = new ObjectWrapper<MemoryStream>(stream);
								ImageCahce.Put(fileName, wrapper);
								imageAvailable(stream);
							},
							ex =>
                                Insights.Report(ex));
					}
				}
			}, cts.Token
			);
		}

		public void LoadIamgeFromSource(string fileName, Action<Stream> imageAvailable, Action<string> loadfailure, string path = "http://babybus.emolbase.com/")
		{
			if (string.IsNullOrEmpty(fileName))
				return;
			factory.StartNew(() => {
				var stream = new MemoryStream();
				//Lode From Cache
				var value = (ObjectWrapper<MemoryStream>)ImageCahce.Get(fileName);
				if (value != null) {
					stream = value.Value;
					imageAvailable(stream);
				} else {
					//Load From Local
					var e = new EnvironmentService();
					string fullFileName = e.GetPicturePathAndName(fileName);

					if (e.FileExists(fullFileName)) {
						stream = GetStreamFromFile(fullFileName);
						var wrapper = new ObjectWrapper<MemoryStream>(stream);
						ImageCahce.Put(fileName, wrapper);
						imageAvailable(stream);
					} else {
						var download = Mvx.Resolve<IMvxHttpFileDownloader>();
						string serverpath = path + fileName;
						download.RequestDownload(serverpath, fullFileName,
							() => {
								stream = GetStreamFromFile(fullFileName);
								var wrapper = new ObjectWrapper<MemoryStream>(stream);
								ImageCahce.Put(fileName, wrapper);
								imageAvailable(stream);
							},
							ex =>
                                loadfailure("图片加载失败，请检查网络"));
					}
				}
			}, cts.Token
			);
		}

		public MemoryStream GetStreamByFileName(string fileName)
		{
			var filePath = Env.GetPicturePathAndName(fileName);
			if (Env.FileExists(filePath)) {
				var stream = GetStreamFromFile(filePath);
				return stream;
			}
			return null;
		}

		public string SaveBitmapInFile(string fileName, Bitmap bmp)
		{
			var ms = new MemoryStream();
			bmp.Compress(Bitmap.CompressFormat.Png, 100, ms);
			var result = SaveStreamInFile(fileName, ms);
			ms.Flush();
			ms.Close();
			return result;
		}

		public string SaveStreamInFile(string fileName, MemoryStream ms)
		{
			string filePath = string.Empty;
			try {

				var bytes = new byte[ms.Length];
				bytes = ms.ToArray();

				filePath = Env.GetPicturePathAndName(fileName);
				FileMode fileMode = Env.FileExists(filePath) ? FileMode.Truncate : FileMode.Create;
				var outStream = new FileStream(filePath, fileMode, FileAccess.Write,
					                FileShare.Delete);
				outStream.Write(bytes, 0, bytes.Length);
				outStream.Flush(true);
				outStream.Close();
			} catch (FileNotFoundException e) {
				e.PrintStackTrace();
			} catch (IOException e) {
				e.PrintStackTrace();
			} catch (Exception e) {
				MvxTrace.Trace(MvxTraceLevel.Error, "save image", e.Message);
			}
			return filePath;
		}

		public MemoryStream GetSmallStreamFromFile(string filePath, int reqWidth, int reqHeight, ref string newFilePath)
		{
			var ret = new MemoryStream();

			var options = new BitmapFactory.Options();
			options.InJustDecodeBounds = true;


			BitmapFactory.DecodeFile(filePath, options);

			// Calculate inSampleSize  
			options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

			// Decode bitmap with inSampleSize set  
			options.InJustDecodeBounds = false;


			//DecodeByteArray
			Bitmap bmp = BitmapFactory.DecodeFile(filePath, options);
			if (bmp == null) {
				MvxTrace.Trace(MvxTraceLevel.Diagnostic, "decode file", "return null");
				return null;
			}
			try {
				//bmp.CopyPixelsToBuffer(ms, ImageFormat.Jpeg);
				if (bmp.Compress(Bitmap.CompressFormat.Jpeg, 100, ret)) {
					var e = new EnvironmentService();
					newFilePath = e.GetTempPicturePathAndName();
					var fileMode = FileMode.OpenOrCreate;
					fileMode = e.FileExists(newFilePath) ? FileMode.Truncate : FileMode.Create;
					var outStream = new FileStream(newFilePath, fileMode, FileAccess.Write,
						                FileShare.Delete);
					outStream.Write(ret.ToArray(), 0, (int)ret.Length);
					outStream.Flush(true);
					outStream.Close();
				}
			} catch (FileNotFoundException e) {
				e.PrintStackTrace();
			} catch (IOException e) {
				e.PrintStackTrace();
			} catch (Exception e) {
				MvxTrace.Trace(MvxTraceLevel.Error, "compress image", e.Message);
			}
			return ret;
		}

		private int CalculateInSampleSize(BitmapFactory.Options options,
		                                  int reqWidth, int reqHeight)
		{
			// Raw height and width of image
			int height = options.OutHeight;
			int width = options.OutWidth;
			int inSampleSize = 1;

			if (height > reqHeight || width > reqWidth) {
				// Calculate ratios of height and width to requested height and
				// width
				double heightRatio = Math.Round(height
				                     / (float)reqHeight);
				double widthRatio = Math.Round(width / (float)reqWidth);

				// Choose the smallest ratio as inSampleSize value, this will
				// guarantee
				// a final image with both dimensions larger than or equal to the
				// requested height and width.
				inSampleSize = (int)(heightRatio < widthRatio ? widthRatio : heightRatio);
			}

			return inSampleSize;
		}

		public bool SaveImageInAlbum(Context context, Bitmap bmp)
		{
			try {
				//Insert Image To Album
				string uri = MediaStore.Images.Media.InsertImage(context.ContentResolver, bmp, "", "");
				if (uri == null) {
					return false;
				}
				string picPath = GetFilePathByContentResolver(context, Uri.Parse(uri));
				if (picPath == null) {
					return false;
				}

//                ContentResolver cr = context.ContentResolver;
//                ContentValues values = new ContentValues(4);
//                values.Put(MediaStore.Images.Media.InterfaceConsts.DateTaken,DateTime.Now.Millisecond);
//                values.Put(MediaStore.Images.Media.InterfaceConsts.MimeType,"image/png");
//                values.Put(MediaStore.Images.Media.InterfaceConsts.Orientation, 0);
//                values.Put(MediaStore.Images.Media.InterfaceConsts.Data, picPath);
//
//                cr.Insert(MediaStore.Images.Media.ExternalContentUri, values);
//				var path = "file://" + Android.OS.Environment.ExternalStorageDirectory;

				context.SendBroadcast(new Intent(Intent.ActionMediaMounted, 
					Uri.Parse(picPath)));
				
				return true;
			} catch (Exception ex) {
				return false;
			}
		}

		private string GetFilePathByContentResolver(Context context, Uri uri)
		{
			if (null == uri) {
				return null;
			}
			ICursor c = context.ContentResolver.Query(uri, null, null, null, null);
			string filePath = null;
			if (null == c) {
				throw new IllegalArgumentException(
					"Query on " + uri + " returns null result.");
			}
			try {
				if ((c.Count != 1) || !c.MoveToFirst()) {
				} else {
					filePath = c.GetString(
						c.GetColumnIndexOrThrow(MediaStore.MediaColumns.Data));
				}
			} finally {
				c.Close();
			}
			return filePath;
		}
	}

	public static class PictureServiceExtension
	{
		public static bool CanLoadFromLocal(this IPictureService pic, string fileName)
		{
			try {
				// Is In Cache?
				var cache = (ObjectWrapper<MemoryStream>)PictureService.ImageCahce.Get(fileName);
				if (cache != null) {
					return true;
				}
				var fullFileName = PictureService.Env.GetPicturePathAndName(fileName);
				if (PictureService.Env.FileExists(fullFileName)) {
					return true;
				}
			} catch (Exception ex) {
				MvxTrace.Trace(ex.ToString());
			}
			return false;
		}
	}
}