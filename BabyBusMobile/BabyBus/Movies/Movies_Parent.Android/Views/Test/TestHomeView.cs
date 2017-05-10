using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.ViewPagerIndicator.Interfaces;
using Android.Support.V4.View;
using BabyBus.Droid.ViewPagerIndicator;
using Java.Lang;
using Android.Content.PM;
using BabyBus.Droid.Views.Test;

namespace BabyBus.Droid
{
	[Activity(Label = "TestHome", ScreenOrientation = ScreenOrientation.Portrait)]	  
	public class TestHomeView : FragmentViewBase<TestHomeViewModel>
	{
		private ImageSlideAdapter _adapter;
		private IPageIndicator _indicator;
		private ViewPager _pager;

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
			if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait) {
				SetContentView(Resource.Layout.Page_Test_Home);
			} else {
			
			}
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			SetContentView(Resource.Layout.Page_Test_Home);

			var ButtonMIMyBaby = FindViewById<TextView>(Resource.Id.button_1_1);
			ButtonMIMyBaby.Click += (sender1, e1) => ViewModel.ShowPhysicalTest();
			var ButtonMIAdvise = FindViewById<TextView>(Resource.Id.button_1_2);
			ButtonMIAdvise.Click += (sender1, args1) => {
				var intent = new Intent(this, typeof(ContentDetailView));
				var url = string.Format("http://115.28.2.41:8888/Pages/PhysicalExerciseGuide.html");
				intent.PutExtra("FileName", url);
				intent.PutExtra("Title", "相关建议");
				StartActivity(intent);
			};
			var ButtonTestMyBaby = FindViewById<TextView>(Resource.Id.button_2_1);
			ButtonTestMyBaby.Click += (sender1, e1) => ViewModel.ShowMITest();
			var ButtonTestAdvise = FindViewById<TextView>(Resource.Id.button_2_2);
			ButtonTestAdvise.Click += (senders, es) => {
				var intent = new Intent(this, typeof(ContentDetailView));
				var url = string.Format(Constants.BaseHtmlUrl + "/MI_Guilde_Mobile.html?ChildId={0}", BabyBusContext.ChildId);
				intent.PutExtra("FileName", url);
				intent.PutExtra("Title", "光谱方案");
				StartActivity(intent);
			};
			var ImageTestPrepare = FindViewById<ImageView>(Resource.Id.image_testprepare);
			ImageTestPrepare.Click += (sender1, args1) => {
				var intent = new Intent(this, typeof(ContentDetailView));
				var url = string.Format("http://mp.weixin.qq.com/s?__biz=MzAwNjM1NjUxNQ==&mid=403113213&idx=1&sn=6133f319e5b1d85d84a418219c7061a5#wechat_redirectl");
				intent.PutExtra("FileName", url);
				intent.PutExtra("Title", "体格测评");
				StartActivity(intent);
			};
			var ImageTestMultiple = FindViewById<ImageView>(Resource.Id.image_testmultiple);
			ImageTestMultiple.Click += (sender1, args1) => {
				var intent = new Intent(this, typeof(ContentDetailView));
				var url = string.Format("http://mp.weixin.qq.com/s?__biz=MzAwNjM1NjUxNQ==&mid=402428190&idx=1&sn=122cbf46e0874516f34eee596bc0426f&scene=18#wechat_redirect");
				intent.PutExtra("FileName", url);
				intent.PutExtra("Title", "多元智能简介");
				StartActivity(intent);
			};
			var mHandler = new Handler();
			ViewModel.FirstLoadedEventHandler += (s, list) => mHandler.PostDelayed(new Runnable(InitAdvertise), 1000);

			var ButtonTemperamentTestMyBaby = FindViewById<TextView>(Resource.Id.button_3_1);
			ButtonTemperamentTestMyBaby.Click += (sender1, e1) => ViewModel.ShowTemperamentTest();

			var ButtonTemperamentTestAdvise = FindViewById<TextView>(Resource.Id.button_3_2);
			ButtonTemperamentTestAdvise.Click += (senders, es) => ViewModel.ShowTemperamentTest();

			var ImageTestTemperament = FindViewById<ImageView>(Resource.Id.image_temperament);
			ImageTestTemperament.Click += (sender1, e1) => ViewModel.ShowTemperamentTest();
		}

		private void InitAdvertise()
		{
			//View Pager Indicator
//			var slideList = new List<int> {
//				Resource.Drawable.bar_slide_6,
//				Resource.Drawable.bar_slide_1,
//				Resource.Drawable.bar_slide_2,
//				Resource.Drawable.bar_slide_3,
//				Resource.Drawable.bar_slide_4,
//				Resource.Drawable.bar_slide_5,
//			};
			//			_adapter = new ImageSlideAdapter(SupportFragmentManager) { List = slideList };
			//			_pager = FindViewById<ViewPager>(Resource.Id.pager);
			//			_pager.Adapter = _adapter;
			//			_indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
			//			_indicator.SetViewPager(_pager);

			_adapter = new ImageSlideAdapter(SupportFragmentManager) { ImageList = ViewModel.AdvertisementStrList ?? new List<string>() };
			_pager = FindViewById<ViewPager>(Resource.Id.pager);
			_pager.Adapter = _adapter;
			_indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
			_indicator.SetViewPager(_pager);
		}
	}
}

