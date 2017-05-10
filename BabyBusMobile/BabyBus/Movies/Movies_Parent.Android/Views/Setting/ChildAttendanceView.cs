using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using com.alliance.calendar;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid.Views.Setting
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class ChildAttendanceView : ViewBase<ChildAttendanceViewModel>
	{
		private CustomCalendar _calendarControl;

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			SetCustomTitleWithBack(Resource.Layout.Page_Setting_ChildAttendance, Resource.String.mine_label_attendance);

			_calendarControl = FindViewById<CustomCalendar>(Resource.Id.CalendarControl);
			_calendarControl.NextButtonText = ">";
			_calendarControl.PreviousButtonText = "<";

			_calendarControl.NextButtonVisibility = ViewStates.Visible;
			_calendarControl.PreviousButtonVisibility = ViewStates.Visible;

			_calendarControl.ShowOnlyCurrentMonth = true;
			_calendarControl.ShowFromDate = new DateTime();


			var customData = ViewModel.AttenceDates.Select(date => new CustomCalendarData(date)).ToList();

			_calendarControl.CustomDataAdapter = customData;

//			//CalendarControl.OnCalendarSelectedDate += CalendarControl_CalendarDateSelected;
//			ViewModel.FirstLoadedEventHandler += (sender1, args) => RunOnUiThread(() => {
//				customData.AddRange(ViewModel.AttenceDates.Select(date => new CustomCalendarData(date)));
//				_calendarControl.CustomDataAdapter = customData;
//				//this.HideInfo();
//				//ViewModel.IsLoading = false;
//			});
			var handler = new Handler();
			ViewModel.FirstLoadedEventHandler += (sender1, args1) => handler.Post(() => {
				customData.AddRange(ViewModel.AttenceDates.Select(date => new CustomCalendarData(date)));
				_calendarControl.CustomDataAdapter = customData;
				//this.HideInfo();
				//ViewModel.IsLoading = false;
			});
			_calendarControl.OnCalendarMonthChange += 
				 (object sender2, CalendarNavigationEventArgs date) => {
				var datetime = new DateTime(date.Year, date.Month, 1);
				ViewModel.TheDateTime = datetime;
				ViewModel.ReloadData();
				customData = ViewModel.AttenceDates.Select(x => new CustomCalendarData(x)).ToList();

				_calendarControl.CustomDataAdapter = customData;
			};

//            if (ViewModel.IsLoading) {
//                this.ShowUpdatingInfo();
//            }
			//Stats Page Report
			StatsUtils.LogPageReport(PageReportType.Attendance);
		}

		//        private void CalendarControl_CalendarDateSelected(object sender, CalendarDateSelectionEventArgs e) {
		//            Toast.MakeText(this, e.SelectedDate.ToString(), ToastLength.Short).Show();
		//        }
	}
}