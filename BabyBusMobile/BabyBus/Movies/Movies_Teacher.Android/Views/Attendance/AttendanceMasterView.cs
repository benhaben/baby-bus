using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.CrossCore;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Attendance
{
	[Activity(Theme = "@style/CustomTheme")]
	public class AttendanceMasterView : FragmentViewBase<AttendanceMasterViewModel>
	{
		private TextView label_attence;
		private TextView label_unattence;
		private ListView mListView;
		private AttendanceChildListAdapter mAdapter;
		private List<ChildModel> _child;
		private IMvxMessenger _messenger;
		private MvxSubscriptionToken _token;

		public List<ChildModel> child {
			get {
				return _child ?? new List<ChildModel>();
			}
			set {
				_child = value;
			}
		}

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			SetCalendarTitleWithBack(Resource.Layout.Page_Attendance_Master, Resource.String.att_title_teacherAttendance);
			label_attence = FindViewById<TextView>(Resource.Id.label_attence);
			label_unattence = FindViewById<TextView>(Resource.Id.label_unattence);

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);
				
			//mPullRefreshListView = FindViewById<PullToRefreshListView> (Resource.Id.unattendance_child_list);
			//mPullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.Both;
			mListView = FindViewById<ListView>(Resource.Id.unattendance_child_list);
			mAdapter = new AttendanceChildListAdapter(this, child);
			mListView.Adapter = mAdapter;

			var calendar = FindViewById<ImageView>(Resource.Id.icon_calendar);
			calendar.Click += Calendar_Click;

			ViewModel.FirstLoadedEventHandler += (sender, args) => RunOnUiThread(() => {
				if (ViewModel.IsAttence) {
					label_attence.Text = ViewModel.Attence.ToString();
					label_unattence.Text = ViewModel.UnAttence.ToString();
				} else {
					const string str = "-";
					label_attence.Text = str;
					label_unattence.Text = str;
				}
				mAdapter.list = ViewModel.Children;
				mAdapter.NotifyDataSetChanged();

			});

			_messenger = Mvx.Resolve<IMvxMessenger>();
			_token = _messenger.Subscribe<DateTimeMessage>((message) => {
				ViewModel.Date = message.Date;
				ViewModel.FirstLoad();
			});
		}

		void Calendar_Click(object sender, EventArgs e) {
			var dialog = new CalendarFragment(ViewModel.Date);
			dialog.Show(SupportFragmentManager, "Dialog");
		}


		protected override void OnResume() {
			base.OnResume();
			if (!IsOnCreate) {
				ViewModel.FirstLoad();
			} else {
				IsOnCreate = false;
			}
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}
	}
}