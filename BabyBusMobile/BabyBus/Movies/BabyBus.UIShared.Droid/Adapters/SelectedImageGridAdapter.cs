using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Utils;
using BabyBus.Droid.Views.Common.Album;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Java.Lang;
using Exception = System.Exception;

namespace BabyBus.Droid.Adapters
{
	/// <summary>
	/// Selected Image GridView Adapter
	/// </summary>
	public class SelectedImageGridAdapter : BaseAdapter
	{
		private readonly MvxActivity activity;

		public SelectedImageGridAdapter (MvxActivity activity)
		{
			this.activity = activity;
		}

		public override int Count {
			get { return ImageCollection.BmpList.Count + 1; }
		}

		public override Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view = convertView ??
			            activity.LayoutInflater.Inflate (Resource.Layout.Item_Published_Grid, parent, false);

			var iv = view.FindViewById<ImageView> (Resource.Id.item_grid_image);
			if (position == ImageCollection.BmpList.Count) {  //Add Pic
				iv.SetImageResource (Resource.Drawable.icon_addpic_unfocused);

				if (position == CustomConfig.ImageMaxNumber) {
					iv.Visibility = ViewStates.Gone;
				}
			} else {  //Image Pic
				iv.SetImageBitmap (ImageCollection.BmpList [position]);
			}
			return view;
		}

		/// <summary>
		/// Update Selected Image Collection, Load bitmap List.
		/// </summary>
		public void Update ()
		{
			if (ImageCollection.Max < ImageCollection.PthList.Count) {
				for (int i = ImageCollection.Max; i < ImageCollection.PthList.Count; i++) {
					try {
						string path = ImageCollection.PthList [i];
						Bitmap bmp = ImageCollection.RevitionImageSize (path);
						if (bmp != null) {
							ImageCollection.BmpList.Add (bmp);
							ImageCollection.Max++;
						}
					} catch (Exception ex) {
						Mvx.Trace ("Image Collection: " + ex.Message);
					}
				}
			}
		}
	}
}