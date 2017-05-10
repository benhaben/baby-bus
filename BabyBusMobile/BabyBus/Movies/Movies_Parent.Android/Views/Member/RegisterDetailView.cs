using Uri = Android.Net.Uri;
using Android.App;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using Android.Widget;
using Android.OS;
using System;

namespace BabyBus.Droid
{
	[Activity(Label = "RegisterDetailView", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class RegisterDetailView : ViewBase<RegisterDetailViewModel>
	{
		private TextView etBirthday;
		private ImageView ivBaby;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
			SetContentView(Resource.Layout.RegisterDetailView);
			//Datapicker
			etBirthday = FindViewById<TextView>(Resource.Id.edit_birthday);
			etBirthday.Text = ViewModel.Birthday.ToLongDateString();
			etBirthday.Click += (sender, e) => new DatePickerDialog(
				this, OnDatePickerSelect, DateTime.Today.Year, DateTime.Today.Month - 1,
				DateTime.Today.Day).Show();

			//BabyImage
			ivBaby = FindViewById<ImageView>(Resource.Id.image_baby);
			ivBaby.Click += ivBaby_Click;
		}

		private void ivBaby_Click(object sender, EventArgs e)
		{
			ViewModel.ChoosePictureWithCropCommand.Execute(null);


			PopupMenu menu = new PopupMenu(this, ivBaby);
			menu.Inflate(Resource.Menu.media_menu);
			menu.Show();

			menu.MenuItemClick += (s1, arg1) => {
				if (arg1.Item.ItemId == Resource.Id.item_select_photo) {
					ViewModel.ChoosePictureWithCropCommand.Execute(null);
				}
				if (arg1.Item.ItemId == Resource.Id.item_take_photo) {
					ViewModel.TakePictureWithCropCommand.Execute(null);
				}
				if (arg1.Item.ItemId == Resource.Id.item_crop_photo) {
					ViewModel.ChoosePictureWithCropCommand.Execute(null);
				}
			};

		}

		void OnDatePickerSelect(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			RunOnUiThread(() => {
				//日期选择之后的操作方法
				if (etBirthday != null) {
					etBirthday.Text = e.Date.ToLongDateString();
					ViewModel.Birthday = e.Date;
				}
			});
		}

		protected override void OnStart()
		{
			base.OnStart();

			//Hide ActionBar
			ActionBar.Hide();
		}
	}
}