using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Adapters;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Attendance
{
	[Activity(Theme = "@style/CustomTheme")]
	public class AttendanceDetailView : ViewBase<AttendanceDetailViewModel>
	{
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			SetCustomTitleWithBack(Resource.Layout.Page_Attendance_Detail, Resource.String.att_title_teacherAttendance);

			var bt = FindViewById<Button>(Resource.Id.childgrid_bt_finish);

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			var grid = FindViewById<GridView>(Resource.Id.childgrid);
			var adapter = new ChildGridAdapter(this, ViewModel.Children);
			grid.Adapter = adapter;

			grid.ItemClick += (sender, args) => {
				View view = args.View;
				var isselect = view.FindViewById<ImageView>(Resource.Id.signed);

				ChildModel child = ViewModel.Children[args.Position];
				child.IsSelect = !child.IsSelect;
				isselect.Visibility = child.IsSelect ? ViewStates.Visible : ViewStates.Gone;
				bt.Text = string.Format("完成({0}/{1})", ViewModel.Attence, ViewModel.Total);
			};

			ViewModel.FirstLoadedEventHandler += (sender, args) => RunOnUiThread(() => {
				adapter.list = ViewModel.Children;
				grid.Adapter = adapter;
			});
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}
	}
}