using Android.App;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using com.alliance.calendar;
using System;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid
{
	[Activity(Label = "CalendarFragment")]
	public class CalendarFragment : MvxDialogFragment
	{
		private DateTime _date;

		public CalendarFragment()
		{
			_date = DateTime.Now;
		}

		public CalendarFragment(DateTime date)
		{
			_date = date;
		}

		public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			base.EnsureBindingContextSet(savedInstanceState);

			var view = this.BindingInflate(Resource.Layout.Dialog_Calendar, null);
			var builder = new AlertDialog.Builder(Activity);
			AlertDialog dialog;



			builder.SetTitle(Resources.GetString(Resource.String.att_title_selectDate));
			builder.SetView(view);
//			dialog.SetNegativeButton(Resources.GetString(Resource.String.common_label_return), (s, a) => {
//			});
//			dialog.SetPositiveButton(Resources.GetString(Resource.String.common_label_enter), (s, a) => {
//			});
			dialog = builder.Create();

			var _calendarControl = view.FindViewById<CustomCalendar>(Resource.Id.CalendarControl);
			_calendarControl.NextButtonText = ">";
			_calendarControl.PreviousButtonText = "<";

			_calendarControl.NextButtonVisibility = ViewStates.Visible;
			_calendarControl.PreviousButtonVisibility = ViewStates.Visible;

			_calendarControl.ShowOnlyCurrentMonth = true;
			var list = new System.Collections.Generic.List<CustomCalendarData> {
				new CustomCalendarData(_date),
			};
			_calendarControl.CustomDataAdapter = list;

			_calendarControl.OnCalendarMonthChange += (sender, e) => {
				var date = new DateTime(e.Year, e.Month, 1);
				list.Clear();
				list.Add(new CustomCalendarData(date));
			};

			_calendarControl.OnCalendarSelectedDate += (sender, e) => {
				if (e.SelectedDate.Date > DateTime.Now.Date) {
					Toast.MakeText(this.Activity, "不能选择将来的日子", ToastLength.Short).Show();
				} else {
					list.Clear();
					list.Add(new CustomCalendarData(e.SelectedDate));
					_calendarControl.CustomDataAdapter = list;
					var message = new DateTimeMessage(this, e.SelectedDate);
					var messenger = Mvx.Resolve<IMvxMessenger>();
					messenger.Publish(message);
					dialog.Dismiss();	
				}
			};
			return dialog;
		}
	}
}

