using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using BabyBus.Droid.Views.Communication;
using Com.Squareup.Picasso;
using Object = Java.Lang.Object;
using Cirrious.MvvmCross.Droid.Fragging;

namespace BabyBus.Droid.Adapters
{
	public class ShareAppAdapter  : BaseAdapter
	{

		public List<ShareConfig> list;
		protected readonly MvxFragmentActivity activity;
		private bool IsGridLayout;

		public ShareAppAdapter(MvxFragmentActivity activity, List<ShareConfig> list, bool IsGridLayout = false)
		{
			this.activity = activity;
			this.list = list;
			this.IsGridLayout = IsGridLayout;
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
			var item = list[position];
			if (convertView == null) {
				if (IsGridLayout) {
					view = activity.LayoutInflater.Inflate(Resource.Layout.Item_ShareApp_Grid, parent, false);
				} else {
					view = activity.LayoutInflater.Inflate(Resource.Layout.Item_ShareApp_List, parent, false);
				}

				holder = new ViewHolder();
				holder.AppLogo = view.FindViewById<ImageView>(Resource.Id.icon_shareapp_logo);
				holder.AppName = view.FindViewById<TextView>(Resource.Id.text_app_name);
				holder.Abstract = view.FindViewById<TextView>(Resource.Id.text_share_abstract);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			if (!string.IsNullOrEmpty(item.AppName)) {
				holder.AppName.Text = item.AppName;
			}
			if (!string.IsNullOrEmpty(item.Abstract)) {
				holder.Abstract.Text = item.Abstract;
			}
			if (item.ImageId != 0) {
				Picasso.With(activity).Load(item.ImageId).CenterCrop()
					.Resize(60, 60).Placeholder(Resource.Drawable.icon_course).Into(holder.AppLogo);
			} else {
				holder.AppLogo.Visibility = ViewStates.Gone;
			}
			return view;
		}



		protected class ViewHolder : Object
		{

			public ImageView AppLogo{ get; set; }

			public TextView AppName{ get; set; }

			public TextView Abstract{ get; set; }
		}
	}
}


