using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Object = Java.Lang.Object;
using BabyBus.Droid.Views.Communication;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid.Adapters
{
	public class QuestionIndexAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<QuestionModel> list;
		protected QuestionIndexViewModel indexVM;

		public QuestionIndexAdapter(MvxActivity activity, List<QuestionModel> list)
		{
			this.activity = activity;
			this.list = list;
		}

		public override int Count {
			get { return list.Count; }
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
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_Question, parent, false);
				holder = new ViewHolder();
				holder.Content = view.FindViewById<TextView>(Resource.Id.item_question_content);
				holder.AnswerContent = view.FindViewById<TextView>(Resource.Id.item_answer_content);
				holder.ChildName = view.FindViewById<TextView>(Resource.Id.item_question_childname);
				holder.CreateTime = view.FindViewById<TextView>(Resource.Id.item_question_createtime);
				holder.LabelIcon = view.FindViewById<ImageView>(Resource.Id.image_icon);
				holder.TypeName = view.FindViewById<TextView>(Resource.Id.label_typename);
				holder.Answered = view.FindViewById<TextView>(Resource.Id.label_question_answer);
				holder.UnAnswer = view.FindViewById<TextView>(Resource.Id.label_question_unanswer);
				holder.LeaveTime = view.FindViewById<TextView>(Resource.Id.label_question_askforleave);
				holder.QuestionType = view.FindViewById<TextView>(Resource.Id.label_question_type);
				//holder.BtnAnswer = view.FindViewById<Button>(Resource.Id.button_question_answer);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];
			holder.Content.Text = item.ContentAbstract;
			holder.AnswerContent.Text = item.AnswerAbstract;
			holder.AnswerContent.Visibility = item.IsHaveAnswers 
				? ViewStates.Visible : ViewStates.Gone;
			holder.ChildName.Text = item.Userinfo;
			holder.CreateTime.Text = BabyBus.Logic.Shared.Utils.DateTimeString(item.CreateTime);
			holder.LeaveTime.Visibility = ViewStates.Gone;

			//holder.LabelIcon.Visibility = ViewStates.Gone;
		
			var _qustiontype = item.GetQustionType();
			switch (_qustiontype) {
				case (int)QuestionType.AskforLeave:
					holder.LabelIcon.SetImageResource(Resource.Drawable.icon_askforleave);
					holder.TypeName.SetText(Resource.String.comm_label_sakleave);
					holder.QuestionType.SetText(Resource.String.comm_label_sakleave);
					holder.QuestionType.SetBackgroundColor(Color.XFPurple1);
					holder.LeaveTime.Visibility = ViewStates.Visible;
					holder.LeaveTime.Text = item.BeginDateToEndDate;
					break;
				case (int)QuestionType.MasterMessage:
					holder.LabelIcon.SetImageResource(Resource.Drawable.icon_tomaster);
					holder.TypeName.SetText(Resource.String.comm_label_tomaster);
					holder.QuestionType.SetText(Resource.String.comm_label_tomaster);
					holder.QuestionType.SetBackgroundColor(Color.XFLake1);
					break;
				case (int)QuestionType.NormalMessage:
					holder.LabelIcon.SetImageResource(Resource.Drawable.icon_normal);
					holder.TypeName.SetText(Resource.String.comm_label_normal);
					holder.QuestionType.SetText(Resource.String.comm_label_normal);
					holder.QuestionType.SetBackgroundColor(Color.XFBlue1);
					break;
				case (int)QuestionType.PersonalMessage:
					holder.LabelIcon.SetImageResource(Resource.Drawable.icon_toparent);
					holder.TypeName.SetText(Resource.String.comm_label_pasonalmessage);
					holder.QuestionType.SetText(Resource.String.comm_label_pasonalmessage);
					holder.QuestionType.SetBackgroundColor(Color.XFGreen1);
					break;
				default:
					break;

			}
			//holder.Answered.Visibility = (item.DisplayType == DisplayType.AnsweredQuestion) ? ViewStates.Visible : ViewStates.Gone;
			holder.Answered.Visibility = ViewStates.Gone;
			holder.UnAnswer.Visibility = (item.DisplayType == DisplayType.UnanswerQuestion) ? ViewStates.Visible : ViewStates.Gone;

//            holder.BtnAnswer.Visibility = (CustomConfig.ApkType == AppType.Teacher)
//                ? ViewStates.Visible
//                : ViewStates.Gone;

			var ll = view.FindViewById<LinearLayout>(Resource.Id.item_question_show);
			ll.Tag = position;
			ll.Click -= ll_Click;
			ll.Click += ll_Click;

//            holder.BtnAnswer.Tag = position;
//            holder.BtnAnswer.Click -= BtnAnswerOnClick;
//            holder.BtnAnswer.Click += BtnAnswerOnClick;
			return view;
		}

		protected void ll_Click(object sender, EventArgs e) {
			indexVM = indexVM ?? ((QuestionIndexView)activity).ViewModel;
			var positon = (int)((LinearLayout)sender).Tag;
			var item = list[positon];
			indexVM.ShowDetailCommand(item.QuestionId);
		}

		private void BtnAnswerOnClick(object sender, EventArgs eventArgs) {
			var position = (int)((Button)sender).Tag;
//            var item = list[position];
//            new AlertDialog.Builder(activity)
//                .SetTitle("º¢×ÓÐÕÃû")
//                .SetView((Button)sender)
//                .SetPositiveButton((int)Resource.String.mine_label_save, (o, args) => {
//                    
//                })
//                .Show();
//            var tab = (MvxTabActivity) activity.Parent.Parent;
//            tab.TabHost.Visibility = ViewStates.Gone;
//            var layout_answer = tab.LayoutInflater.Inflate(Resource.Layout.Item_SendAnswer, null);
//            tab.TabHost.AddView(layout_answer);

		}

		protected class ViewHolder : Object
		{
			public TextView Content { get; set; }

			public TextView AnswerContent { get; set; }

			public TextView ChildName { get; set; }

			public TextView CreateTime { get; set; }

			public TextView UnAnswer { get; set; }

			public TextView Answered { get; set; }

			public TextView LeaveTime { get; set; }

			public Button BtnAnswer { get; set; }

			public ImageView LabelIcon{ get; set; }

			public TextView TypeName{ get; set; }

			public TextView QuestionType { get; set; }

		}
	}
}