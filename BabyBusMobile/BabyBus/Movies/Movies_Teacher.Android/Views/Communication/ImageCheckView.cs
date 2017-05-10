using Android.App;
using Android.OS;
using Android.Views;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.Views.Common.Album;
using BabyBus.Droid.ViewPagerIndicator.Interfaces;
using Android.Support.V4.View;
using BabyBus.Logic.Shared;
using BabyBus.Droid.ViewPagerIndicator;

namespace BabyBus.Droid.Views.Common
{
	/// <summary>
	/// Image Detail for Send.
	/// </summary>
	[Activity(Label = "ImageGridView")]
	public class ImageCheckView : FragmentViewBase<ImageCheckViewModel>
	{
		private IPageIndicator _indicator;
		private ViewPager _pager;
		private PhotoPagerAdapter _adapter;


		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e) {
			base.OnCreateCalled(sender, e);

			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.Page_Comm_MemoryDetail);
			Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			ViewModel.FirstLoadedEventHandler += (sender1, args) => RunOnUiThread(() => {
				//View Pager Indicator
				_adapter = new PhotoPagerAdapter(SupportFragmentManager) { PhotoNameList = ImageCollection.PthList };
				_pager = FindViewById<ViewPager>(Resource.Id.pager);
				_pager.Adapter = _adapter;
				_indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
				_indicator.SetViewPager(_pager);
				_indicator.CurrentItem = ImageCollection.Selected;

				//Stats Page Report
				//StatsUtils.LogPageReport (PageReportType.GrowMomeryDetail);

			});
		}
	}
}