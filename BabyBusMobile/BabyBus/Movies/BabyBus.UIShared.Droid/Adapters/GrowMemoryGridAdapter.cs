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
using Com.Squareup.Picasso;

namespace BabyBus.Droid.Adapters
{
	public class GrowMemoryGridAdapter : BaseAdapter
	{
		private readonly MvxActivity _activity;
		private List<string> _list;
		private Handler handler;

		public string PicassoTag{ get; private set; }

		public GrowMemoryGridAdapter(MvxActivity activity, List<string> list)
		{
			_activity = activity;
			_list = list;
			PicassoTag = "GrowMemoryGridAdapterTag";
		}

		public override int Count {
			get {
				if (_list != null)
					return _list.Count;
				else
					return 0;
			}
		}

		public override Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int position)
		{
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ??
			           _activity.LayoutInflater.Inflate(Resource.Layout.Item_Published_Grid, parent, false);
			var iv = view.FindViewById<ImageView>(Resource.Id.item_grid_image);
			Picasso.With(_activity).Load(Constants.ThumbServerPath + _list[position]).Placeholder(Resource.Drawable.icon_imageloading)
				.Resize(80, 80).Tag(PicassoTag).Into(iv);
			var fileName = _list[position] + Constants.ThumbRule;
			return view;
		}

	}
}
