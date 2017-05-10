using System;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using Android.Views;
using Object = Java.Lang.Object;
using Android.OS;
using BabyBus.Droid.Utils;


namespace BabyBus.Droid
{
	public class MITestDetailAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<MIAssessIndex> list;
		protected MITestView indexVM;
		private Handler handler;
		private readonly float dp;

		public MITestDetailAdapter(MvxActivity activity, List<MIAssessIndex> list, float dp)
		{
			this.activity = activity;
			this.list = list;
			this.dp = dp;
			handler = new Handler();
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

				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_MI_AssessIndex, parent, false);
				holder = new ViewHolder();
				holder.MIAssessName = view.FindViewById<TextView>(Resource.Id.AssessName);
				holder.QuestionList = view.FindViewById<ListView>(Resource.Id.testquestion);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];
			holder.MIAssessName.Text = item.Name;
			if (item.MITestList != null) {
				const int DefaultGridHeight = CustomConfig.DefaultThumbQuestionListHeight;
				ViewGroup.LayoutParams layoutParams = holder.QuestionList.LayoutParameters;
				//layoutParams.Width = Convert.ToInt32 (BabyBusContext.WidthInDp * dp);
				var n = 0;
				foreach (var m in item.MITestList) {
					if (m.Name.Length >= 17 && m.Name.Length < 34) {
						n += 1;
					} else if (m.Name.Length >= 34 && m.Name.Length < 51) {
						n += 2;
					} else {
						;
					}
				}
				layoutParams.Height = Convert.ToInt32((DefaultGridHeight * item.MITestList.Count + n * 20) * dp);
				holder.QuestionList.LayoutParameters = layoutParams;
				holder.QuestionList.Adapter = new  MITestQuestionAdapter(activity, item.MITestList, position, dp);
			}

			return view;
		}

		protected class ViewHolder : Object
		{

			public TextView MIAssessName{ get; set; }

			public ListView QuestionList{ get; set; }

		}
	}
}


