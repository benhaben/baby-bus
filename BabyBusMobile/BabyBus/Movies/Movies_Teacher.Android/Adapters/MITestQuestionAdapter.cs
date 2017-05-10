using System;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using Android.Views;
using Object = Java.Lang.Object;
using Cirrious.CrossCore;
using BabyBus.Droid.Utils;



namespace BabyBus.Droid
{
	public class MITestQuestionAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<MITestQuestion> list;
		protected MITestView indexVM;
		private IPictureService pic;
		private int assess;
		private readonly float dp;

		public MITestQuestionAdapter(MvxActivity activity, List<MITestQuestion> list, int assesee, float dp)
		{
			this.activity = activity;
			this.list = list;
			this.assess = assesee;
			pic = Mvx.Resolve<IPictureService>();
			this.dp = dp;
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

				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_MI_Testqustion, parent, false);
				holder = new ViewHolder();
				holder.MI_QuestionName = view.FindViewById<TextView>(Resource.Id.testquestion);
				holder.Score = view.FindViewById<RatingBar>(Resource.Id.answer);
				holder.Item = view.FindViewById<LinearLayout>(Resource.Id.Item_TestQuestion);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];
			holder.MI_QuestionName.Text = item.Name;

			var h = holder.MI_QuestionName.GetY();
			holder.Score.Rating = item.Score;
			var n = 0;
			var DefaultGridHeight = CustomConfig.DefaultThumbQuestionListHeight;
			ViewGroup.LayoutParams layoutParams = holder.Item.LayoutParameters;
			layoutParams.Width = Convert.ToInt32(BabyBusContext.WidthInDp * dp);
			if (item.Name.Length >= 17 && item.Name.Length < 34) {
				n = 1;
			} else if (item.Name.Length >= 34 && item.Name.Length < 51) {
				n = 2;
			} else {
				n = 0;
			}
			layoutParams.Height = Convert.ToInt32((DefaultGridHeight + n * 20) * dp);
			holder.Score.RatingBarChange += (o, e) => {
				var tem = list[position];
				list[position].Score = (int)holder.Score.Rating;
				tem.Score = (int)holder.Score.Rating;

			};
			return view;
		}

		protected class ViewHolder : Object
		{

			public TextView MI_QuestionName{ get; set; }

			public RatingBar Score{ get; set; }

			public LinearLayout Item{ get; set; }

		}
	}
}


