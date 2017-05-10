using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Xamarin;
using Java.Lang;
using BabyBus.Logic.Shared;
using Com.Squareup.Picasso;
using System;


namespace BabyBus.Droid
{
	public class AnswerListAdapter : BaseAdapter
	{
		private readonly MvxActivity _activity;

		public IList<AnswerModel> List { get; set; }

		public string PicassoTag{ get; private set; }

		public AnswerListAdapter(MvxActivity activity, IList<AnswerModel> list)
		{
			_activity = activity;
			List = list;
			PicassoTag = "AnswerListAdapterTag";
		}

		public override Java.Lang.Object GetItem(int position)
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
			View view = null;
			try {
				var item = List[position];
				view = item.IsMyself ? _activity.LayoutInflater.Inflate(Resource.Layout.Item_Answer_owner, parent, false)
					: _activity.LayoutInflater.Inflate(Resource.Layout.Item_Answer_opponent, parent, false);
				holder = new ViewHolder();
				holder.Name = view.FindViewById<TextView>(Resource.Id.item_answer_name);
				holder.Content = view.FindViewById<TextView>(Resource.Id.item_answer_content);
				holder.CreatTime = view.FindViewById<TextView>(Resource.Id.creattime);
				holder.UserHeah = view.FindViewById<ImageView>(Resource.Id.user_head);
				view.Tag = holder;

				var IsShorTime = ShorTime(position);
				holder.CreatTime.Visibility = IsShorTime ? ViewStates.Visible : ViewStates.Gone;
				holder.CreatTime.Text = item.CreateTimeString; 
				holder.Name.Text = item.RealName;
				holder.Content.Text = item.Content;
				holder.CreatTime.Text = BabyBus.Logic.Shared.Utils.DateTimeString(item.CreateTime);
				if (!string.IsNullOrEmpty(item.ImageName)) {
					Picasso.With(_activity).Load(Constants.ThumbServerPath + item.ImageName).Placeholder(Resource.Drawable.Child_headImage)
						.Resize(40, 40).Tag(PicassoTag).Into(holder.UserHeah);
				} else {
					holder.UserHeah.SetImageResource(Resource.Drawable.Child_headImage);
				}
			} catch (Java.Lang.Exception ex) {
				Insights.Report(ex);
			}
			return view;
		}

		public override int Count {
			get { return List.Count; }
		}

		public class ViewHolder:Java.Lang.Object
		{
			public TextView Name{ get; set; }

			public TextView Content{ get; set; }

			public TextView CreatTime{ get; set; }

			public ImageView UserHeah{ get; set; }
		}

		private bool ShorTime(int position)
		{
			if (position >= 1) {
				var time = List[position].CreateTime.ToOADate() - List[position - 1].CreateTime.ToOADate();
				if (time > 0.5) {
					return true;
				} else {
					return false;
				}
			} else {
				return true;
			}
		}
	}
		

}

