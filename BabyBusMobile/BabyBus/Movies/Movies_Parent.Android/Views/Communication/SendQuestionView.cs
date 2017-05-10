using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Control;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid.Views.Communication
{
	[Activity(Label = "@string/comm_label_qjly", Theme = "@style/CustomTheme")]
	public class SendQuestionView : ViewBase<SendQuestionViewModel>
	{
		private EditText content;
		private LinearLayout ll_askdate;
		private TextView beginAskDate;
		private TextView endAskDate;
		private DatePickerDialog begindateDlg;
		private DatePickerDialog enddateDlg;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			var titleId = GetResourceTitleId();
			SetCustomTitleWithBack(Resource.Layout.Page_Comm_SendQuestion, titleId);

			beginAskDate = FindViewById<Button>(Resource.Id.date_begin);
			endAskDate = FindViewById<Button>(Resource.Id.date_end);
			beginAskDate.Text = ViewModel.BeginDate.ToString("D");
			endAskDate.Text = ViewModel.EndDate.ToString("D");
			beginAskDate.Click += begindate_Click;
			endAskDate.Click += enddate_Click;
			ll_askdate = FindViewById<LinearLayout>(Resource.Id.ll_askdate);
			ll_askdate.Visibility = ViewStates.Gone;
			var segment = FindViewById<SegmentedGroup>(Resource.Id.segmented2);
			segment.SetTintColor(Resource.Color.bb_blue);
			segment.CheckedChange += segment_CheckedChange;

			if (ViewModel.SendToWho == RoleType.HeadMaster
			    || ViewModel.SendToWho == RoleType.Parent) {
				segment.Visibility = ViewStates.Gone;
			} else {
				segment.Visibility = ViewStates.Visible;

			}


			Button btnSelectChild = FindViewById<Button>(Resource.Id.btn_select_child);

			if (ViewModel.SendToWho == RoleType.Parent) {
				btnSelectChild.Visibility = ViewStates.Visible;
			} else {
				btnSelectChild.Visibility = ViewStates.Gone;
			}
			btnSelectChild.Click += (sender, e) => ViewModel.ShowSelectChildrenCommand.Execute();
			content = FindViewById<EditText>(Resource.Id.question_content_text);
			content.Hint = ViewModel.ContentHolder;

			RadioButton radCasualLeave = FindViewById<RadioButton>(Resource.Id.RadioButton_CasualLeave);
			RadioButton radSickLeave = FindViewById<RadioButton>(Resource.Id.RadioButton_SickLeave);
			RadioButton radGrippeLeave = FindViewById<RadioButton>(Resource.Id.RadioButton_GrippeLeave);
			RadioButton radMeaslesLeave = FindViewById<RadioButton>(Resource.Id.RadioButton_MeaslesLeave);
			RadioButton radMumpsLeave = FindViewById<RadioButton>(Resource.Id.RadioButton_MumpsLeave);
			RadioButton radDysenteryLeave = FindViewById<RadioButton>(Resource.Id.RadioButton_DysenteryLeave);
			RadioButton radHFMDLeave = FindViewById<RadioButton>(Resource.Id.RadioButton_HFMDLeave);
			RadioGroup radgropLeave11 = FindViewById<RadioGroup>(Resource.Id.radiogrop_leave_11);
			RadioGroup radgropLeave21 = FindViewById<RadioGroup>(Resource.Id.radiogrop_leave_21);
			RadioGroup radgropLeave22 = FindViewById<RadioGroup>(Resource.Id.radiogrop_leave_22);

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			radCasualLeave.Click += (object sender, EventArgs e) => {
				content.Text = "";
				content.Hint = "请输入请假理由以及请假时间……";
				radgropLeave21.Check(-1);
				radgropLeave22.Check(-1);
			};
			radSickLeave.Click += (object sender, EventArgs e) => {
				content.Text = "我家宝宝生病了，需要请假！病因：待确诊";
				radgropLeave11.Check(-1);
				radgropLeave22.Check(-1);
			};
			radGrippeLeave.Click += (object sender, EventArgs e) => {
				content.Text = "我家宝宝生病了，需要请假！病因：流感";
				radgropLeave11.Check(-1);
				radgropLeave22.Check(-1);
			};
			radMeaslesLeave.Click += (object sender, EventArgs e) => {
				content.Text = "我家宝宝生病了，需要请假！病因：麻疹";
				radgropLeave11.Check(-1);
				radgropLeave22.Check(-1);

			};
			radMumpsLeave.Click += (object sender, EventArgs e) => {
				content.Text = "我家宝宝生病了，需要请假！病因：腮腺炎";
				radgropLeave11.Check(-1);
				radgropLeave21.Check(-1);

			};
			radDysenteryLeave.Click += (object sender, EventArgs e) => {
				content.Text = "我家宝宝生病了，需要请假！病因：痢疾";
				radgropLeave11.Check(-1);
				radgropLeave21.Check(-1);
			};
			radHFMDLeave.Click += (object sender, EventArgs e) => {
				content.Text = "我家宝宝生病了，需要请假！病因：手足口";
				radgropLeave11.Check(-1);
				radgropLeave21.Check(-1);
			};
		}

		private void leave_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e) {
			/*switch (e.CheckedId) {
			case Resource.Id.RadioButton_CasualLeave:
				
			}*/

		}

		protected override void OnDestroy() {
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}

		void begindate_Click(object sender, EventArgs e) {
			var date = ViewModel.BeginDate;
			if (begindateDlg == null) {
				begindateDlg = new DatePickerDialog(
					this, OnBeginDatePickerSelect, date.Year, date.Month - 1,
					date.Day); 
			}
			begindateDlg.Show();
		}

		void enddate_Click(object sender, EventArgs e) {
			var date = ViewModel.EndDate;
			if (enddateDlg == null) {
				enddateDlg = new DatePickerDialog(
					this, OnEndDatePickerSelect, date.Year, date.Month - 1,
					date.Day); 
			}
			enddateDlg.Show();
		}

		private void OnBeginDatePickerSelect(object sender, DatePickerDialog.DateSetEventArgs e) {
			if (e.Date < DateTime.Now.Date) {
				this.ShowInfo("不要给以前请假偶");
			} else {
				beginAskDate.Text = e.Date.ToLongDateString();
				ViewModel.BeginDate = e.Date;
				if (ViewModel.EndDate < e.Date) {
					endAskDate.Text = e.Date.ToLongDateString();
					ViewModel.EndDate = e.Date;
				}
			}
		}

		private void OnEndDatePickerSelect(object sender, DatePickerDialog.DateSetEventArgs e) {
			if (e.Date < ViewModel.BeginDate) {
				this.ShowInfo("请假的结束时间不能早于开始时间");
			} else {
				endAskDate.Text = e.Date.ToLongDateString();
				ViewModel.EndDate = e.Date;
			}
		}

		private void segment_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e) {
			
			var rad_RadioGrop_SickLeave = FindViewById<RadioGroup>(Resource.Id.RadioGrop_SickLeave);
			switch (e.CheckedId) {
				case Resource.Id.button21:
					ViewModel.SelectMemoType = new MemoType { Type = QuestionType.NormalMessage };
					content.Hint = ViewModel.ContentHolder;
					rad_RadioGrop_SickLeave.Visibility = ViewStates.Gone;
					ll_askdate.Visibility = ViewStates.Gone;
					rad_RadioGrop_SickLeave.Check(-1);
					break;
				case Resource.Id.button22:
					ViewModel.SelectMemoType = new MemoType { Type = QuestionType.AskforLeave };
					rad_RadioGrop_SickLeave.Visibility = ViewStates.Visible;
					ll_askdate.Visibility = ViewStates.Visible;
					content.Hint = ViewModel.ContentHolder;
					break;
			}
		}

		int GetResourceTitleId() {
			int titleId;
			if (ViewModel.SendToWho == RoleType.HeadMaster) {
				titleId = Resource.String.comm_label_mastermail;
			} else if (ViewModel.SendToWho == RoleType.Parent) {
				titleId = Resource.String.comm_label_grly;
			} else {
				titleId = Resource.String.comm_label_qjly;
			}
			return titleId;
		}
	}
}