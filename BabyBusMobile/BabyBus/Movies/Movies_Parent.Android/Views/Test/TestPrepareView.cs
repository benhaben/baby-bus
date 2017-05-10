using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using Android.Views;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class TestPrepareView : ViewBase<TestPrepareViewModel>
	{
		private DatePickerDialog begindateDlg;
		private Handler handler = new Handler();

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			// Create your application here
			SetCustomTitleWithBack(Resource.Layout.Page_Test_Prepare, "我的宝宝");


			InitGender();
			Initchildbirthday();
//			InitHeightWeight();


			ViewModel.FirstLoadedEventHandler += (sender1, e1) => 
				handler.Post(() => InitGenderDisplay((GenderType)ViewModel.Gender));
			var next = FindViewById<TextView>(Resource.Id.next);
			next.Click += (sender1, args1) => {
				UpdateChildInfo();
				TestInfo();
			};
		}

		private void UpdateChildInfo()
		{
			var ChildWeight = FindViewById<EditText>(Resource.Id.child_weight);
			var ChildHeight = FindViewById<EditText>(Resource.Id.child_height);
			var ChildBirthday = FindViewById<TextView>(Resource.Id.child_birthday);
			var ChildName = FindViewById<TextView>(Resource.Id.child_name);
			ViewModel.Weight = float.Parse(ChildWeight.Text);
			ViewModel.Height = float.Parse(ChildHeight.Text);
			ViewModel.ChildName = ChildName.Text;
			ViewModel.UpdateInfo();
		}

		private void InitGenderDisplay(GenderType gender)
		{
			var TextBoy = FindViewById<TextView>(Resource.Id.text_boy);
			var TextGirl = FindViewById<TextView>(Resource.Id.text_girl);
			var IconBoy = FindViewById<ImageView>(Resource.Id.icon_boy);
			var IconGirl = FindViewById<ImageView>(Resource.Id.icon_girl);
			var LineBoy = FindViewById<View>(Resource.Id.line_boy);
			var LineGirl = FindViewById<View>(Resource.Id.line_girl);
			if (gender == GenderType.Female) {
				TextBoy.SetTextColor(Color.XFBlack1);
				IconBoy.SetImageResource(Resource.Drawable.icon_test_boy_unselected);
				LineBoy.SetBackgroundColor(Color.XFBlack1);
				TextGirl.SetTextColor(Color.XFGreen1);
				IconGirl.SetImageResource(Resource.Drawable.icon_test_girl_selected);
				LineGirl.SetBackgroundColor(Color.XFGreen1);
			} else {
				TextBoy.SetTextColor(Color.XFGreen1);
				IconBoy.SetImageResource(Resource.Drawable.icon_test_boy_selected);
				LineBoy.SetBackgroundColor(Color.XFGreen1);
				TextGirl.SetTextColor(Color.XFBlack1);
				IconGirl.SetImageResource(Resource.Drawable.icon_test_girl_unselected);
				LineGirl.SetBackgroundColor(Color.XFBlack1);
			}
		}

		private void InitGender()
		{
			var Selectboy = FindViewById<LinearLayout>(Resource.Id.selected_boy);
			var SelectGirl = FindViewById<LinearLayout>(Resource.Id.selected_girl);
			Selectboy.Click += (sender, e) => {
				ViewModel.Gender = (int)GenderType.Male;
				InitGenderDisplay(GenderType.Male);
			};
			SelectGirl.Click += (sender, e) => {
				ViewModel.Gender = (int)GenderType.Female;
				InitGenderDisplay(GenderType.Female);
			};
		}

		private void Initchildbirthday()
		{
			var ChildBirthday = FindViewById<TextView>(Resource.Id.child_birthday);
			ChildBirthday.Click += (sender, e) => {
				var date = ViewModel.Birthday;

				if (begindateDlg == null) {
					begindateDlg = new DatePickerDialog(
						this, OnBeginDatePickerSelect, date.Year, date.Month - 1, date.Day); 
				}
				begindateDlg.Show();
			};
		}

		//		private void InitHeightWeight()
		//		{
		//			var ChildWeight = FindViewById<EditText>(Resource.Id.child_weight);
		//			var ChildHeight = FindViewById<EditText>(Resource.Id.child_height);
		//			ChildWeight.Click += (sender, e) => {
		//				ChildWeight.Text = "";
		//			};
		//			ChildHeight.Click += (sender, e) => {
		//				ChildHeight.Text = "";
		//			};
		//		}

		private void OnBeginDatePickerSelect(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			var ChildBirthday = FindViewById<TextView>(Resource.Id.child_birthday);
			ChildBirthday.Text = e.Date.ToString("D");
			ViewModel.Birthday = e.Date;
		}

		private void TestInfo()
		{
			var intent = new Intent(this, typeof(ContentDetailView));
			var url = string.Format("http://115.28.2.41:8888/Pages/PhysicalExaminationReport_Mobile.html?ChildId={0}", BabyBusContext.ChildId);
			intent.PutExtra("FileName", url);
			intent.PutExtra("Title", "宝宝测评");
			StartActivity(intent);
		}
	}
}

