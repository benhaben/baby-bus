using Android.App;
using Android.Content.PM;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using CN.Jpush.Android.Api;

namespace BabyBus.Droid
{
	[Activity(
		Label = "贝贝巴士家长版"
		, ScreenOrientation = ScreenOrientation.Portrait
        , MainLauncher = true
        , Icon = "@drawable/Icon"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true)]
	public class SplashScreen : MvxSplashScreenActivity
	{
		public SplashScreen()
			: base(Resource.Layout.SplashScreen)
		{
			
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			//Xamarin.Insights
			//Insights.Initialize("a106ce8a29cf5c0aa929086ebd741f7d72730bd2", this);
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
			if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait) {
				SetContentView(Resource.Layout.SplashScreen);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			//Task.Run(() => UpdateManager.UpdateAll());
		}

		protected override void OnPause()
		{
			base.OnPause();

			JPushInterface.OnPause(this);
		}

		protected override void OnResume()
		{
			base.OnResume();

			JPushInterface.OnResume(this);
		}
	}
}