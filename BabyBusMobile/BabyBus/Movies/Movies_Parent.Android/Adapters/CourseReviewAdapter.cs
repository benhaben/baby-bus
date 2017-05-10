using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Android.Views;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using Object = Java.Lang.Object;
using Com.Squareup.Picasso;

namespace BabyBus.Droid
{
	public class CourseReviewAdapter : BaseAdapter
	{
		readonly MvxActivity activity;
		public List<ECReview> list;

		public string PicassoTag{ get; private set; }

		public CourseReviewAdapter(MvxActivity activity, List<ECReview> list)
		{
			this.activity = activity;
			this.list = list;
			PicassoTag = "CourseReviewAdapterTag";
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
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_EC_Review, parent, false);
				holder = new ViewHolder();
				holder.UserName = view.FindViewById<TextView>(Resource.Id.label_username);
				holder.Rating = view.FindViewById<RatingBar>(Resource.Id.user_recommendationindex);
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
			holder.Rating.Rating = item.Rating;
			holder.CreateDate.Text = BabyBus.Logic.Shared.Utils.DateTimeString(item.CreateDate);
			holder.Content.Text = item.Content;
			if (!string.IsNullOrEmpty(item.ImageName)) {
				Picasso.With(activity).Load(Constants.ThumbServerPath + item.ImageName).Tag(PicassoTag).CenterCrop()
					.Resize(80, 80).Placeholder(Resource.Drawable.icon_course).Into(holder.UserHead);//.Resize(320, 125)
			}
			return view;
		}

		protected class ViewHolder : Object
		{
			public TextView UserName { get; set; }

			public RatingBar Rating{ get; set; }

			public TextView CreateDate{ get; set; }

			public TextView Content{ get; set; }

			public ImageView UserHead{ get; set; }
		}
	}
}

