using System;
using System.Collections.Generic;
using System.IO;

using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Object = Java.Lang.Object;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Adapters
{
	#if false
	public class CheckoutListAdapter : BaseAdapter
	{
		private readonly MvxActivity activity;
		public List<CheckoutModel> list;
		private IPictureService pic;
		private Handler handler;

		public CheckoutListAdapter(MvxActivity activity, List<CheckoutModel> list)
		{
			this.activity = activity;
			this.list = list;
			pic = Mvx.Resolve<IPictureService>();
			handler = new Handler();
		}

		public override Object GetItem(int position) {
			return null;
		}

		public override long GetItemId(int position) {
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder;
			View view;
			if (convertView == null) {
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_Checkout, parent, false);
				holder = new ViewHolder();
				holder.Name = view.FindViewById<TextView>(Resource.Id.child_name);
				holder.HeadImage = view.FindViewById<ImageView>(Resource.Id.head_image);
				holder.Status = view.FindViewById<TextView>(Resource.Id.checkout_status);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			CheckoutModel child = list[position];
			//Child Name
			holder.Name.Text = child.ChildName;
			//Child Status
			holder.Status.Text = child.Status;
			//Child Text
			if (!string.IsNullOrEmpty(child.ImageName)) {
				var fileName = child.ImageName + Constants.ThumbRule;
				pic.LoadIamgeFromSource(fileName, stream => {
					var ms = stream as MemoryStream;
					if (ms != null) {
						var bytes = ms.ToArray();
						var options = new BitmapFactory.Options() { InPurgeable = true };
						var bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
						handler.Post(() => holder.HeadImage.SetImageBitmap(bmp));
					} else {
						handler.Post(() => holder.HeadImage.SetImageResource(Resource.Drawable.baby_icon));
					}
				}, Constants.ThumbServerPath);
			} else {
				handler.Post(() => holder.HeadImage.SetImageResource(Resource.Drawable.baby_icon));
			}
			return view;
		}

		public override int Count {
			get { return list.Count; }
		}

		public class ViewHolder : Object
		{
			public TextView Name { get; set; }

			public ImageView HeadImage { get; set; }

			public TextView Status { get; set; }
		}
	}
	#endif
}