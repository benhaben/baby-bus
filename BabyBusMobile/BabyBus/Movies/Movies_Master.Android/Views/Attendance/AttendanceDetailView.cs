using Android.App;
using Android.OS;
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

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

//var bt = FindViewById<Button>(Resource.Id.childgrid_bt_finish);


			var grid = FindViewById<GridView>(Resource.Id.childgrid);
			var adapter = new ChildGridAdapter(this, ViewModel.Children);
			grid.Adapter = adapter;

//            grid.ItemClick += (sender, args) => {
//                View view = args.View;
//                var isselect = view.FindViewById<ImageView>(Resource.Id.isselected);
//                var text = view.FindViewById<TextView>(Resource.Id.item_grid_image_text);
//
//                ChildModel child = ViewModel.Children[args.Position];
//                child.IsSelect = !child.IsSelect;
//                if (child.IsSelect) {
//                    isselect.SetImageResource(Resource.Drawable.icon_data_select);
//                    text.SetBackgroundResource(Resource.Drawable.bgd_relatly_line);
//                }
//                else {
//                    isselect.SetImageBitmap(null);
//                    text.SetBackgroundColor(new Color(0x00000000));
//                }
//                bt.Text = string.Format("Íê³É({0}/{1})", ViewModel.Attence, ViewModel.Total);
//            };

			ViewModel.FirstLoadedEventHandler += (sender, args) => RunOnUiThread(() => {
				adapter.list = ViewModel.Children;
				//adapter.NotifyDataSetChanged();
				grid.Adapter = adapter;
			});
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}
	}
}