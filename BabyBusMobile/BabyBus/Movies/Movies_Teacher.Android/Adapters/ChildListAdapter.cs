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
using Com.Squareup.Picasso;

namespace BabyBus.Droid.Adapters
{
	public class ChildListAdapter : BaseAdapter
	{
		private readonly MvxActivity activity;
		public IList<ChildModel> list;
		private IPictureService pic;
		private Handler handler;

		public ChildListAdapter(MvxActivity activity, IList<ChildModel> list)
		{
			this.activity = activity;
			this.list = list;
			pic = Mvx.Resolve<IPictureService>();
			handler = new Handler();
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
			ViewHolder holder;
			View view;
			if (convertView == null) {
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_Child, parent, false);
				holder = new ViewHolder();
				holder.Name = view.FindViewById<TextView>(Resource.Id.child_name);
				holder.HeadImage = view.FindViewById<ImageView>(Resource.Id.head_image);
				holder.LabelPaid = view.FindViewById<TextView>(Resource.Id.label_paid);
				holder.LabelUnpay = view.FindViewById<TextView>(Resource.Id.label_unpay);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			ChildModel child = list[position];
			//Child Name
			holder.Name.Text = child.ChildName;
			//Payment Label
			holder.LabelPaid.Visibility = child.IsMember ? ViewStates.Visible : ViewStates.Gone;
			holder.LabelUnpay.Visibility = !child.IsMember ? ViewStates.Visible : ViewStates.Gone;

			//Child Text
			if (!string.IsNullOrEmpty(child.ImageName)) {
				Picasso.With(activity).Load(Constants.ThumbServerPath + child.ImageName).Placeholder(Resource.Drawable.baby_icon).CenterCrop()
					.Resize(80, 80).Tag(PicassoTag).Into(holder.HeadImage);
			} else {
				handler.Post(() => holder.HeadImage.SetImageResource(Resource.Drawable.baby_icon));
			}
			return view;
		}

		public string PicassoTag{ get; set; }

		public override int Count {
			get { return list.Count; }
		}

		public class ViewHolder : Object
		{
			public TextView Name { get; set; }

			public ImageView HeadImage { get; set; }

			public TextView LabelPaid { get; set; }

			public TextView LabelUnpay{ get; set; }
		}
	}
}