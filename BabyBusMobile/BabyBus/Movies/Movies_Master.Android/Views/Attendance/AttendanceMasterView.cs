using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Droid.Adapters;
using Java.Lang;
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.CrossCore;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Attendance
{
	[Activity(Theme = "@style/CustomTheme")]
	public class AttendanceMasterView : FragmentViewBase<AttendanceMasterViewModel>
	{
		private readonly Handler mHandler = new Handler();
		private IMvxMessenger _messenger;
		private MvxSubscriptionToken _token;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);


			SetCalendarTitleWithBack(Resource.Layout.Page_Attendance_Master, Resource.String.att_title_teacherAttendance);

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			var list = FindViewById<ListView>(Resource.Id.list_attences);
			var adapter = new AttMasterListAdapter(this, ViewModel.Attendances);
			list.Adapter = adapter;

			list.ItemClick += (sender, args) => {
				var item = ViewModel.Attendances[args.Position];
				ViewModel.ClassId = item.ClassId;
				ViewModel.ShowDetailCommand.Execute();
			};
				
			ViewModel.FirstLoadedEventHandler += (sender, args) => mHandler.PostDelayed(new Runnable(() => {
				adapter.list = ViewModel.Attendances;
				adapter.NotifyDataSetChanged();
			}), 1000);

			var calendar = FindViewById<ImageView>(Resource.Id.icon_calendar);
			calendar.Click += Calendar_Click;			
			var AttendanceDate = FindViewById<TextView>(Resource.Id.attendance_date);
			AttendanceDate.Text = ViewModel.Date.ToString("D") + "考勤详情";
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

		protected override void OnPause() {
			base.OnPause();
			this.HideInfo();
		}
	}
}