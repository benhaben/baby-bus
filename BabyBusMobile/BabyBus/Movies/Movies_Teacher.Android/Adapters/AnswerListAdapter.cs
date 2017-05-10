using System.Collections.Generic;

using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Xamarin;
using Java.Lang;
using System.IO;
using Android.Graphics;
using Cirrious.CrossCore;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid
{
	public class AnswerListAdapter : BaseAdapter
	{
		private readonly MvxActivity _activity;
		private IPictureService pic;

		public IList<AnswerModel> List { get; set; }

		public AnswerListAdapter(MvxActivity activity, IList<AnswerModel> list)
		{
			_activity = activity;
			List = list;
		}

		public override Object GetItem(int position) {
			return null;
		}

		public override long GetItemId(int position) {
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
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
				holder.CreatTime.Visibility = IsShorTime ? ViewStates.Gone : ViewStates.Visible;
					 
				holder.Name.Text = item.UserName;
				holder.Content.Text = item.Content;
				holder.CreatTime.Text = BabyBus.Logic.Shared.Utils.DateTimeString(item.CreateTime);

				if (item.UserHead != null && item.UserHead != "") { 
					pic = Mvx.Resolve<IPictureService>();
					pic.LoadIamgeFromSource(item.UserHead, stream => {
						var ms = stream as MemoryStream;
						if (ms != null) {
							var bytes = ms.ToArray();
							var options = new BitmapFactory.Options() { InPurgeable = true };
							var bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
							holder.UserHeah.SetImageBitmap(bmp);
						}
					}, Constants.ThumbServerPath);
				}
			} catch (Exception ex) {
				Insights.Report(ex);
			}
			return view;
		}

		public override int Count {
			get { return List.Count; }
		}

		public class ViewHolder:Object
		{
			public TextView Name{ get; set; }

			public TextView Content{ get; set; }

			public TextView CreatTime{ get; set; }

			public ImageView UserHeah{ get; set; }
		}

		private bool ShorTime(int position) {
			if (position >= 1) { 
				if (List[position].CreateTime.DayOfYear == List[position - 1].CreateTime.DayOfYear) {
					if (List[position].CreateTime.Hour == List[position - 1].CreateTime.Hour) {
						return Math.Abs(List[position].CreateTime.Minute - List[position - 1].CreateTime.Minute) < 10;
					} else if (Math.Abs(List[position].CreateTime.Hour - List[position - 1].CreateTime.Hour) == 1) {
						return Math.Abs(List[position].CreateTime.Minute + 60 - List[position - 1].CreateTime.Minute) < 10;
					} else {
						return false;
					}
				} else if (Math.Abs(List[position].CreateTime.DayOfYear - List[position - 1].CreateTime.DayOfYear) == 1
				           && Math.Abs(List[position].CreateTime.Hour + 24 - List[position - 1].CreateTime.Hour) == 1) {
					return Math.Abs(List[position].CreateTime.Minute + 60 - List[position - 1].CreateTime.Minute) < 10;
				} else {
					return false;
				}
				
			} else {
				return false;
			}
		}
	}
}

