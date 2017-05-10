
using Android.Support.V4.App;
using Android.Graphics.Drawables;
using Android.Graphics;
using Com.Squareup.Picasso;
using System.Threading.Tasks;
using Android.Provider;
using System;
using Android.App;
using System.Text;
using System.IO;

namespace BabyBus.Droid.Fragments
{
	public class PicassoTarget : Java.Lang.Object,ITarget
	{
		//private readonly Picasso picasso;
		private readonly Activity context;
		private readonly string imagename;
		private Bitmap ImageBitmap;
		private byte[] ByteImage;

		public PicassoTarget(Activity context, string imagename)
		{
			//	this.picasso = picasso;
			this.context = context;
			this.imagename = imagename;
		}

		public PicassoTarget(Activity context, string imagename, byte[] tobitmap)
		{
			this.context = context;
			this.imagename = imagename;
			ByteImage = tobitmap;
		}

		public void OnBitmapFailed(Drawable p0)
		{

		}

		public void  OnBitmapLoaded(Bitmap bitmap, Picasso.LoadedFrom from)
		{
			
			Task task = Task.Factory.StartNew(() => {
				ImageBitmap = bitmap;
				if (ByteImage != null) {
					using (MemoryStream stream = new MemoryStream()) {
						bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
						ByteImage = stream.ToArray();
					}
						
				}
				string uri = MediaStore.Images.Media.InsertImage(context.ContentResolver, bitmap, "", "");

				//				var path = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDcim).Path + "/" + imagename;
				//				var filepath = GetFilePathByContentResolver(context, Uri.Parse(uri));
				//				SaveBitmapInFile(imagename, bitmap);
				//				try {
				//					var file = File.Create(filepath);
				//					bitmap.Compress(Bitmap.CompressFormat.Png, 75, file);
				//					file.Close();
				//				} catch (FileNotFoundException e) {
				//					e.PrintStackTrace();
				//				}
			});


		}

		public void OnPrepareLoad(Drawable p0)
		{

		}


		//		public string SaveBitmapInFile(string fileName, Bitmap bmp)
		//		{
		//			var ms = new MemoryStream();
		//			bmp.Compress(Bitmap.CompressFormat.Png, 100, ms);
		//			var result = SaveStreamInFile(fileName, ms);
		//			ms.Flush();
		//			ms.Close();
		//			return result;
		//		}

		//		public string SaveStreamInFile(string fileName, MemoryStream ms)
		//		{
		//			string filePath = string.Empty;
		//			try {
		//
		//				var bytes = new byte[ms.Length];
		//				bytes = ms.ToArray();
		//
		//				filePath = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDcim).Path + fileName;
		//				FileMode fileMode = File.Exists(filePath) ? FileMode.Truncate : FileMode.Create;
		//				var outStream = new FileStream(filePath, fileMode, FileAccess.Write,
		//					                FileShare.Delete);
		//				outStream.Write(bytes, 0, bytes.Length);
		//				outStream.Flush(true);
		//				outStream.Close();
		//			} catch (FileNotFoundException e) {
		//				e.PrintStackTrace();
		//			} catch (IOException e) {
		//				e.PrintStackTrace();
		//			} catch (Exception e) {
		//				MvxTrace.Trace(MvxTraceLevel.Error, "save image", e.Message);
		//			}
		//			return filePath;
		//		}
		//
		//		private string GetFilePathByContentResolver(Android.Content.Context context, Uri uri)
		//		{
		//			if (null == uri) {
		//				return null;
		//			}
		//			ICursor c = context.ContentResolver.Query(uri, null, null, null, null);
		//			string filePath = null;
		//			if (null == c) {
		//				throw new IllegalArgumentException(
		//					"Query on " + uri + " returns null result.");
		//			}
		//			try {
		//				if ((c.Count != 1) || !c.MoveToFirst()) {
		//				} else {
		//					filePath = c.GetString(
		//						c.GetColumnIndexOrThrow(MediaStore.MediaColumns.Data));
		//				}
		//			} finally {
		//				c.Close();
		//			}
		//			return filePath;
		//		}
		public Bitmap GetImageBitmap()
		{
			return	ImageBitmap;
		}

	}


}

