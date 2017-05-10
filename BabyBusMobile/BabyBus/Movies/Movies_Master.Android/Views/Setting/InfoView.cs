using System;

using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Mine
{
	[Activity(Theme = "@style/CustomTheme")]
	public class InfoView : ViewBase<InfoViewModel>
	{
		private LinearLayout parentname;
		private LinearLayout childname;
		private LinearLayout gender;
		private TextView birthday;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			SetCustomTitleWithBack(Resource.Layout.Page_Mine_InfoView, Resource.String.mine_label_setting);

			//Init Update User Event
			parentname = FindViewById<LinearLayout>(Resource.Id.setting_info_parentname);
			parentname.Click += parentname_Click;

		}

		private void birthday_Click(object sender, EventArgs e) {
			new DatePickerDialog(
				this, OnDatePickerSelect, DateTime.Today.Year, DateTime.Today.Month - 1,
				DateTime.Today.Day).Show();
		}

		private void OnDatePickerSelect(object sender, DatePickerDialog.DateSetEventArgs e) {
			RunOnUiThread(() => {
				//ÈÕÆÚÑ¡ÔñÖ®ºóµÄ²Ù×÷·½·¨
				birthday.Text = e.Date.ToLongDateString();
				ViewModel.Birthday = e.Date;
				ViewModel.UpdateBirthdayCommand.Execute();
			});
		}

		private void childname_Click(object sender, EventArgs e) {
			var text = new EditText(this);
			text.Text = ViewModel.ChildName;
			new AlertDialog.Builder(this)
                .SetTitle("孩子姓名")
                .SetView(text)
                .SetPositiveButton((int)Resource.String.mine_label_save, (o, args) => {
				ViewModel.ChildName = text.Text;
				ViewModel.UpdateChildNameCommand.Execute();
			})
                .Show();
		}

		private void parentname_Click(object sender, EventArgs e) {
			var text = new EditText(this);
			text.Text = ViewModel.RealName;
			new AlertDialog.Builder(this)
	            .SetTitle("园长称谓")
	            .SetView(text)
	            .SetPositiveButton((int)Resource.String.mine_label_save, (o, args) => {
				ViewModel.RealName = text.Text;
				ViewModel.UpdateParentNameCommand.Execute();
			})
	            .Show();
		}
	}
}