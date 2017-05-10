using Android.App;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;
using CN.Jpush.Android.Api;

namespace BabyBus.Droid
{
	[Activity(
		Label = "贝贝巴士管理版"
		, MainLauncher = true
        , Icon = "@drawable/Icon"
		, Theme = "@style/Theme.Splash"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashScreen : MvxSplashScreenActivity
	{
		public SplashScreen()
			: base(Resource.Layout.SplashScreen)
		{
		}

		protected override void OnStart() {
			base.OnStart();
			//Task.Run(()=>UpdateManager.UpdateAll());
		}

		protected override void OnPause() {
			base.OnPause();

			JPushInterface.OnPause(this);
		}

		protected override void OnResume() {
			base.OnResume();

			JPushInterface.OnResume(this);
		}
	}
}