using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Support.V4.Util;
using Android.Widget;
using BabyBus.Droid.Services;
using Cirrious.CrossCore.Platform;
using Java.Lang;
using Exception = System.Exception;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Utils.Image
{
	/// <summary>
	///     Bitmap Cache, cache album images(thumb) in Memory Cache or Local File System,
	///     avoid re-load image from Source File.
	/// </summary>
	public class BitmapCache
	{
		private readonly PictureService _pic;
		private readonly BitmapLruCache imageCache;
		private int test_cache;
		private int test_thumb;
		private int test_total;

		public BitmapCache()
		{
			var maxMemory = (int)(Runtime.GetRuntime().MaxMemory() / 1024);
			imageCache = new BitmapLruCache(maxMemory / 8);

			_pic = new PictureService();
		}

		/// <summary>
		///     Display Bitmap Image
		/// </summary>
		public void DisplayBmp(ImageView iv, string thumPath, string sourcePath, string imageId, IImageCallback callback) {
			string path;
			bool isThumbPath;

			//test
			test_total++;

			if (!string.IsNullOrEmpty(thumPath)) {
				path = thumPath;
				isThumbPath = true;
				//test
				test_thumb++;
			} else if (!string.IsNullOrEmpty(sourcePath)) {
				path = sourcePath;
				isThumbPath = false;
			} else {
				return;
			}
			//Image is Cached, Load from Cache
			ObjectWrapper<Bitmap> wrapper = null;
			try {
				//wrapper = (ObjectWrapper<Bitmap>) imageCache.Get(path);
			} catch (Exception ex) {
			}
			if (wrapper != null && wrapper.Value != null) {
				//test
				test_cache++;
				Bitmap bmp = wrapper.Value;

				if (callback != null && (iv.Tag == null || iv.Tag.ToString() == sourcePath)) {
					callback.ImageLoad(iv, bmp);
				} else {
					iv.SetImageResource(Resource.Color.black);
				}
			} else {
				//Image is Not Cache, Load from File
				Task.Run(() => {
					Bitmap thumb = null;
					try {
						//Load From Local File
						string imageFileName = imageId + Constants.PNGSuffix;
						MemoryStream ms = _pic.GetStreamByFileName(imageFileName);
						if (ms != null) {
							byte[] bytes = ms.ToArray();
							var options = new BitmapFactory.Options { InPurgeable = true };
							thumb = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
							ms.Flush();
							ms.Close();
						} else {
							//Load and Revition From Source File
							thumb = RevitionImageSize(sourcePath);
							_pic.SaveBitmapInFile(imageFileName, thumb);
						}
					} catch (Exception ex) {
					}
					wrapper = new ObjectWrapper<Bitmap>(thumb);
					imageCache.Put(path, wrapper);
					if ((iv.Tag == null || iv.Tag.ToString() == sourcePath))
						callback.ImageLoad(iv, thumb);
				});
			}
			MvxTrace.Trace(string.Format("Total:{0},Thumb:{1},Cache:{2}", test_total, test_thumb, test_cache));
		}

		private Bitmap RevitionImageSize(string sourcePath) {
			return ImageRevition.RevitionImageSize(sourcePath, 120, 80);
		}

		public void ClearCache() {
			imageCache.TrimToSize(-1);
		}
	}
}