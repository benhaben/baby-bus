using System;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Android.Views;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using Object = Java.Lang.Object;
using Cirrious.CrossCore;

namespace BabyBus.Droid
{
	public class ForumCommentAdapter : BaseAdapter
	{
		MvxActivity activity;
		public List<ECComment> list;
		IPictureService pic;

		public ForumCommentAdapter(MvxActivity activity, List<ECComment> list)
		{
			this.activity = activity;
			this.list = list;
			pic = Mvx.Resolve<IPictureService>();
		}

		public override int Count {
			get { return list.Count; }
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
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_EC_Comment, parent, false);
				holder = new ViewHolder();
				holder.UserName = view.FindViewById<TextView>(Resource.Id.label_username);
				holder.CreateDate = view.FindViewById<TextView>(Resource.Id.label_createdate);
				holder.Content = view.FindViewById<TextView>(Resource.Id.text_content);
				holder.UserHead = view.FindViewById<ImageView>(Resource.Id.send_headimage);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];
			holder.UserName.Text = item.RealName;
			holder.CreateDate.Text = BabyBus.Logic.Shared.Utils.DateTimeString(item.CreateDate);
			holder.Content.Text = item.Content;
			return view;
		}

		protected class ViewHolder : Object
		{
			public TextView UserName { get; set; }

			public TextView CreateDate{ get; set; }

			public TextView Content{ get; set; }

			public ImageView UserHead{ get; set; }
		}
	}
}

