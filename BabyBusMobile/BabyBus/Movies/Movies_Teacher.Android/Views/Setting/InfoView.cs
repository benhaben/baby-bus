using System;

using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Logic.Shared;
using Android.Content;
using Android.Graphics;
using System.IO;

namespace BabyBus.Droid.Views.Mine
{
	[Activity(Theme = "@style/CustomTheme")]
	public class InfoView : ViewBase<InfoViewModel>
	{
		private LinearLayout parentname;
		private LinearLayout childname;
		private LinearLayout gender;
		private TextView birthday;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetCustomTitleWithBack(Resource.Layout.Page_Mine_InfoView, Resource.String.mine_label_setting);

			//Init Update User Event
			parentname = FindViewById<LinearLayout>(Resource.Id.setting_info_parentname);
			parentname.Click += parentname_Click;

			var ChildHead = FindViewById<ImageView>(Resource.Id.UserHead);
			ChildHead.Click += (sender, e) => CropImage();

			gender = FindViewById<LinearLayout>(Resource.Id.setting_info_gender);
		}

		private void birthday_Click(object sender, EventArgs e)
		{
			new DatePickerDialog(
				this, OnDatePickerSelect, DateTime.Today.Year, DateTime.Today.Month - 1,
				DateTime.Today.Day).Show();
		}

		private void OnDatePickerSelect(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			RunOnUiThread(() => {
				birthday.Text = e.Date.ToLongDateString();
				ViewModel.Birthday = e.Date;
				ViewModel.UpdateBirthdayCommand.Execute();
			});
		}

		private void childname_Click(object sender, EventArgs e)
		{
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

		private void parentname_Click(object sender, EventArgs e)
		{
			var text = new EditText(this);
			text.Text = ViewModel.RealName;
			new AlertDialog.Builder(this)
	            .SetTitle("教师称谓")
	            .SetView(text)
	            .SetPositiveButton((int)Resource.String.mine_label_save, (o, args) => {
				ViewModel.RealName = text.Text;
				ViewModel.UpdateParentNameCommand.Execute();
			})
	            .Show();
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			using (MemoryStream stream = new MemoryStream())
				if (data != null) {
					Bitmap bitmap = (Bitmap)data.GetParcelableExtra("data");
					var ChildHead = FindViewById<ImageView>(Resource.Id.UserHead);
					//ChildHead.SetImageBitmap(bitmap);
					bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
					var bytes = stream.ToArray();
					ChildHead.SetImageBitmap(bitmap);
					ViewModel.UpdatePicture(bytes);
					stream.Close();


				}
		}

		private void CropImage()
		{
			using (Intent intent = new Intent(Intent.ActionGetContent, null)) {
				intent.SetType("image/*");
				intent.PutExtra("crop", "true");
				intent.PutExtra("aspectX", 1);
				intent.PutExtra("aspectY", 1);
				intent.PutExtra("outputX", 80);
				intent.PutExtra("outputY", 80);
				intent.PutExtra("scale", true);
				intent.PutExtra("return-data", true);
				intent.PutExtra("outputFormat", Bitmap.CompressFormat.Png.ToString());
				intent.SetAction(Intent.ActionPick);
				StartActivityForResult(intent, 3003);


			}
		}
	}
}