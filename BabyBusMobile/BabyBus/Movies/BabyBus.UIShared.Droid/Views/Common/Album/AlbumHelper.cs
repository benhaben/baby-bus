using System.Collections.Generic;
using System.IO;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Provider;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Java.Lang;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Common.Album
{
	/// <summary>
	/// Album Helper, Load Thumb & Source Image From Local File Syste.
	/// </summary>
	public class AlbumHelper
	{
		private static AlbumHelper instance;
		private Context context;
		private ContentResolver cr;
		private IEnvironmentService eService;

		private Dictionary<string, string> thumbnailList;

		private AlbumHelper()
		{
			eService = Mvx.Resolve<IEnvironmentService>();
		}

		public static AlbumHelper Instance {
			get { return instance ?? (instance = new AlbumHelper()); }
		}

		public List<ImageItem> ImageList { get; private set; }

		public void Init(Context c) {
			if (context != null)
				return;
			context = c;
			cr = c.ContentResolver;
		}

		//Build Image List, Including Thumb & Source List
		public void BuildImageList() {
			thumbnailList = new Dictionary<string, string>();
			ImageList = new List<ImageItem>();
			GetThumbnail();
			GetAlbum();
		}

		private void GetThumbnail() {
			string[] projection = {
				MediaStore.Images.ImageColumns.Id, MediaStore.Images.Thumbnails.ImageId,
				MediaStore.Images.Thumbnails.Data
			};
			ICursor cur = cr.Query(MediaStore.Images.Thumbnails.ExternalContentUri, projection, null, null, null);
			GetThumbnailColumnData(cur);
		}

		private void GetThumbnailColumnData(ICursor cur) {
			if (cur.MoveToFirst()) {
				int imageIdColumn = cur.GetColumnIndex(MediaStore.Images.Thumbnails.ImageId);
				int dataColumn = cur.GetColumnIndex(MediaStore.Images.Thumbnails.Data);
				do {
					int imageId = cur.GetInt(imageIdColumn);
					string imagePath = cur.GetString(dataColumn);
					if (eService.FileExists(imagePath)) {
						if (!thumbnailList.ContainsKey("" + imageId))
							thumbnailList.Add("" + imageId, imagePath);
					}
				} while (cur.MoveToNext());
			}
		}

		private void GetAlbum() {
			string[] columns = {
				MediaStore.Images.ImageColumns.Id, MediaStore.Images.ImageColumns.Data
			};
			ICursor cur = cr.Query(MediaStore.Images.Media.ExternalContentUri, columns, null, null, null);
			GetAlbumColumnData(cur);
		}

		private void GetAlbumColumnData(ICursor cur) {
			if (cur.MoveToFirst()) {
				int photoIdIndex = cur.GetColumnIndex(MediaStore.Images.ImageColumns.Id);
				int photoPathIndex = cur.GetColumnIndex(MediaStore.Images.ImageColumns.Data);
				do {
					string id = cur.GetString(photoIdIndex);
					string imagePath = cur.GetString(photoPathIndex);
					if (eService.FileExists(imagePath)) {
						var imageItem = new ImageItem { ImageId = id, ImagePath = imagePath };
						if (thumbnailList.ContainsKey(id))
							imageItem.ThumbnailPath = thumbnailList[id];
						ImageList.Insert(0, imageItem);
					}
					MvxTrace.Trace("Path:" + imagePath);
				} while (cur.MoveToNext());
			}
		}

		public void GetSelectedImageList() {
			var selectList = new List<ImageItem>();
			foreach (ImageItem item in ImageList) {
				if (item.IsSelect) {
					selectList.Add(item);
				}
			}
			foreach (ImageItem item in selectList) {
				ImageCollection.PthList.Add(item.ImagePath);
			}
		}

		public void GetTakedImage(Stream picStream) {
//            var memoryStream = new MemoryStream();
//            picStream.CopyTo(memoryStream);
//            var bytes = memoryStream.ToArray();
			try {
				var bmp = BitmapFactory.DecodeStream(picStream);
				ImageCollection.PthList.Add("");
				ImageCollection.BmpList.Add(bmp);
				ImageCollection.Max++;
			} catch (Exception ex) {
                
			}

		}
	}
}