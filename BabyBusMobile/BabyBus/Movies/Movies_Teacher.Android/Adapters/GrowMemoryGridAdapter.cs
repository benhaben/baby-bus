
using System.Collections.Generic;
using System.IO;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Object = Java.Lang.Object;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Adapters
{
	public class GrowMemoryGridAdapter : BaseAdapter
	{
		private readonly MvxActivity _activity;
		private List<string> _list;
		private IPictureService pic;
		private Handler handler;

		public GrowMemoryGridAdapter(MvxActivity activity, List<string> list)
		{
			_activity = activity;
			_list = list;
			pic = Mvx.Resolve<IPictureService>();
			handler = new Handler();
		}

		public override int Count {
			get {
				if (_list != null)
					return _list.Count;
				else
					return 0;
			}
		}

		public override Object GetItem(int position) {
			return null;
		}

		public override long GetItemId(int position) {
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			var view = convertView ??
			           _activity.LayoutInflater.Inflate(Resource.Layout.Item_Published_Grid, parent, false);
			var iv = view.FindViewById<ImageView>(Resource.Id.item_grid_image);

			var fileName = _list[position] + Constants.ThumbRule;

			pic.LoadIamgeFromSource(fileName, stream => {
				var ms = stream as MemoryStream;
				if (ms != null) {
					var bytes = ms.ToArray();
					var options = new BitmapFactory.Options() { InPurgeable = true };
					var bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
					handler.Post(() => {
						iv.SetImageBitmap(bmp);
					});
				}
			}, Constants.ThumbServerPath);

			return view;
		}

	}
}
