using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Color = Android.Graphics.Color;
using BabyBus.Logic.Shared;
using System;

namespace BabyBus.Droid.Views
{

	public abstract class TabbedViewBase<TViewModel> 
		: MvxTabActivity,ICustomTitleBar where TViewModel:BaseViewModel
	{
		private long mExitTime;

		public new TViewModel ViewModel {
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void OnCreate(Android.OS.Bundle bundle)
		{
			//RequestWindowFeature(WindowFeatures.NoTitle);
//            RequestedOrientation = ScreenOrientation.Portrait;
			base.OnCreate(bundle);

		}

		protected override void OnResume()
		{
			base.OnResume();
		}

		protected override void OnDestroy()
		{

			base.OnDestroy();
		}

		public void SetCustomTitle(int layoutId, string title)
		{
			bool requestWindowFeature = false;
			//RequestWindowFeature (WindowFeatures.NoTitle);
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle);
			SetContentView(Resource.Layout.MainPrtView);
			if (requestWindowFeature) {
				this.Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Default);
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = title;
				   

			}	

		}

		public void SetCustomTitleWithBack(int layoutId, string title)
		{
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			// Create your application here
			SetContentView(layoutId);

			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Default_WithBack);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender, EventArgs e) => {
					CloseView();
				};
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = title;
			} 
		}

		public virtual void CloseView()
		{
			Finish();
		}

		public override void OnBackPressed()
		{
			if ((Utils.Utils.CurrentMillis - mExitTime) > 2000) {
				Toast.MakeText(this, "再按一次退出程序", ToastLength.Short).Show();
				//						AndHUD.Shared.ShowToast (this, "TTT", MaskType.None, TimeSpan.FromMilliseconds (1000), false);
				mExitTime = Utils.Utils.CurrentMillis;
			} else {
				Finish();
				Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
			}
		}


	}
}
