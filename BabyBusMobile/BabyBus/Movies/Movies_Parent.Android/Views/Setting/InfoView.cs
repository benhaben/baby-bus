using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Views.Frags.Dialog;
using BabyBus.Logic.Shared;
using Android.Graphics;
using Android.Content;
using System.IO;
using Android.App;


namespace BabyBus.Droid.Views.Mine
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class InfoView : FragmentViewBase<InfoViewModel>
	{
		private LinearLayout parentname;
		private LinearLayout childname;
		private LinearLayout llGenderDisplay;
		private TextView birthday;
		private DatePickerDialog dateDlg;

		public new InfoViewModel ViewModel {
			get { return (InfoViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					Finish();
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);
	
			SetCustomTitleWithBack(Resource.Layout.Page_Mine_InfoView, Resource.String.mine_label_setting);
            
			//Init Update User Event
			parentname = FindViewById<LinearLayout>(Resource.Id.setting_info_parentname);
			parentname.Click += parentname_Click;

			childname = FindViewById<LinearLayout>(Resource.Id.setting_info_childname);
			childname.Click += childname_Click;

			birthday = FindViewById<TextView>(Resource.Id.setting_info_birthday);
			birthday.Click += birthday_Click;
			//birthday.Text = ViewModel.Birthday.ToLongDateString();

			//Linear Layout Gender
			llGenderDisplay = FindViewById<LinearLayout>(Resource.Id.setting_info_gender);

			llGenderDisplay.Click += llGenderDisplay_Click;

			var ChildHead = FindViewById<ImageView>(Resource.Id.child_head);
			ChildHead.Click += (senders, es) => CropImage();  
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

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			using (MemoryStream stream = new MemoryStream())
				if (data != null) {
					Bitmap bitmap = (Bitmap)data.GetParcelableExtra("data");
					var ChildHead = FindViewById<ImageView>(Resource.Id.child_head);
					//ChildHead.SetImageBitmap(bitmap);
					bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
					var bytes = stream.ToArray();
					ChildHead.SetImageBitmap(bitmap);
					ViewModel.UpdatePicture(bytes);
					stream.Close();
				}
		}


		private void llGenderDisplay_Click(object sender, EventArgs e)
		{
			string[] genders = new string[]{ "男孩", "女孩" };
			new AlertDialog.Builder(this)
				.SetSingleChoiceItems(genders, ViewModel.Gender.Id - 1, (o, arge) => {
				ViewModel.Gender = (arge.Which == 0) ? GenderModel.CreateMan() : GenderModel.CreateWoman();
			})
				.SetNegativeButton("取消", (o, args) => {
			})
				.SetTitle("孩子性别：")
				.SetPositiveButton("确定", (o, args) => ViewModel.UpdateGenderCommand.Execute()).Show();
		}


		private void birthday_Click(object sender, EventArgs e)
		{
			if (dateDlg == null) {
				dateDlg = new DatePickerDialog(
					this, OnDatePickerSelect, ViewModel.Birthday.Year, ViewModel.Birthday.Month,
					ViewModel.Birthday.Day);  
			}

			dateDlg.Show();
		}

		private void OnDatePickerSelect(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			
			if (e.Date > DateTime.Now) {
				this.ShowInfo(Resources.GetString(Resource.String.mine_info_unbirth));
				return;
			}

			//日期选择之后的操作方法
			birthday.Text = e.Date.ToLongDateString();
			ViewModel.Birthday = e.Date;
			ViewModel.UpdateBirthdayCommand.Execute();
		}

		private void childname_Click(object sender, EventArgs e)
		{
			var text = new EditText(this);
			text.Text = ViewModel.ChildName;
			new AlertDialog.Builder(this)
				.SetTitle(Resources.GetString(Resource.String.register_label_childname))
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
				.SetTitle(Resources.GetString(Resource.String.register_label_parentname))
	            .SetView(text)
	            .SetPositiveButton((int)Resource.String.mine_label_save, (o, args) => {
				ViewModel.RealName = text.Text;
				ViewModel.UpdateParentNameCommand.Execute();
			})
	            .Show();
		}
	}
}