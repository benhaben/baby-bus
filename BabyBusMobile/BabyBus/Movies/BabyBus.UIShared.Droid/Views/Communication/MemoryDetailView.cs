

using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.ViewPagerIndicator;
using BabyBus.Droid.ViewPagerIndicator.Interfaces;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using Android.Content.PM;

namespace BabyBus.Droid.Views.Communication
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MemoryDetailView : FragmentViewBase<MemoryDetailViewModel>
	{
		private IPageIndicator _indicator;
		private ViewPager _pager;
		private PhotoPagerAdapter _adapter;



		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.Page_Comm_MemoryDetail);
			Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			ViewModel.FirstLoadedEventHandler += (sender1, args) => RunOnUiThread(() => {
				//View Pager Indicator
				_adapter = new PhotoPagerAdapter(SupportFragmentManager) { PhotoNameList = ViewModel.Notice.ImageList };
				_pager = FindViewById<ViewPager>(Resource.Id.pager);
				_pager.Adapter = _adapter;
				_indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
				_indicator.SetViewPager(_pager);
				_indicator.CurrentItem = ViewModel.Position;

				//Stats Page Report
				StatsUtils.LogPageReport(PageReportType.GrowMomeryDetail);

			});
		}
	}
}