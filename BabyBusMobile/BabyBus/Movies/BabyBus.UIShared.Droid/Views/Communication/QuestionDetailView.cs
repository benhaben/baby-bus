using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using BabyBus.Droid.Views.Member;
using Android.Content.PM;

namespace BabyBus.Droid.Views.Communication
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class QuestionDetailView : ViewBase<QuestionDetailViewModel>
	{
		//private LinearLayout ll;
		private AnswerListAdapter _adapter;

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			SetCustomTitleWithBack(Resource.Layout.Page_Comm_QuestionDetail, "详细");

			//Role
			var layout_sendAnswer = FindViewById<LinearLayout>(Resource.Id.layout_send_answer);

			/*var leavetime = FindViewById<TextView>(Resource.Id.leavetime);
			if (ViewModel.QuestionType == QuestionType.AskforLeave) {
				leavetime.Visibility = ViewStates.Visible;
			} else {
				leavetime.Visibility = ViewStates.Gone;
			}*/
//			layout_sendAnswer.Visibility = BabyBusContext.RoleType == RoleType.Teacher
//				? ViewStates.Visible
//				: ViewStates.Gone;
			/*var titlebar = FindViewById<LinearLayout> (Resource.Id.question_titlebar);
			switch(BabyBusContext.RoleType){
			case RoleType.Master:
				titlebar.SetBackgroundColor (Resources.GetColor (Resource.Color.bb_orange));
					break;
			case RoleType.Parent:
				titlebar.SetBackgroundColor (Resources.GetColor (Resource.Color.bb_green));
				break;
			case RoleType.Teacher:
				titlebar.SetBackgroundColor (Resources.GetColor (Resource.Color.bb_blue));
				break;
			};*/
			var list = FindViewById<ListView>(Resource.Id.answer_list);
			_adapter = new AnswerListAdapter(this, new List<AnswerModel>());
			list.Adapter = _adapter;
			ViewModel.RefreshAnswers += (sender1, args) => RunOnUiThread(() => {
//				if (ll != null)
//				{
//					ll.RemoveAllViews();
//					foreach (var answer in ViewModel.Question.Answers) {
//						var text = new TextView(this);
//						text.Text = answer.UserName + " : " + answer.Content;
//						ll.AddView(text);
//					}
//				}
				_adapter.List = ViewModel.Question.Answers;
				list.Adapter = _adapter;
				ListViewHeightBasedOnChildren.SetListView(list);
				_adapter.NotifyDataSetChanged();

			});
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			ViewModel.FirstLoadedEventHandler += (sender1, args) => RunOnUiThread(() => {
				_adapter = new AnswerListAdapter(this, ViewModel.Question.Answers ?? new List<AnswerModel>());
				list.Adapter = _adapter;
				ListViewHeightBasedOnChildren.SetListView(list);
			});

		}

		private int GetTitleResourceId()
		{
			if (ViewModel == null)
				return -1;
			switch (ViewModel.QuestionType) {
				case QuestionType.NormalMessage:
					return Resource.String.comm_label_question;
				case QuestionType.AskforLeave:
					return Resource.String.comm_label_askforleave;
				default:
					return -1;
			}
		}
	}
}