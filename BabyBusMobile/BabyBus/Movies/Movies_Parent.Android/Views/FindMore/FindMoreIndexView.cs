
using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.ViewPagerIndicator.Interfaces;
using Android.Support.V4.View;
using BabyBus.Droid.ViewPagerIndicator;
using Java.Lang;
using Com.Handmark.Pulltorefresh.Library;


namespace BabyBus.Droid
{
	[Activity(Label = "Discovery", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class FindMoreIndexView : FragmentViewBase<FindMoreIndexViewModel>
	{

		private const string ImageResourceKey = "ItemImage";
		private const string TextResourceKey = "TextImage";
		private PullToRefreshListView mPullRefreshListView;
		private Handler mHandler;

		private readonly int[] images = {
			Resource.Drawable.discovery_curriculum,
			Resource.Drawable.discovery_activities,
			Resource.Drawable.discovery_forum,
			Resource.Drawable.discovery_more,
		};

		private ImageSlideAdapter _adapter;
		private IPageIndicator _indicator;
		private ViewPager _pager;
		private RecommendationAdpter _radapter;
		private readonly string[] _texts = {
			"课程",
			"活动",
			"论坛",
			"更多",
		};
		private List<ECPostInfo> eCPostInfo = new List<ECPostInfo>();

		public List<ECPostInfo> ECPostInfo {
			get { 
				return eCPostInfo ?? new List<ECPostInfo>();
			}
			set { 
				eCPostInfo = value;
			}
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			// Create your application here
			SetContentView(Resource.Layout.Page_EC_DiscoveryIndex);
			InitGroupButton();
			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			var classnotice = FindViewById<LinearLayout>(Resource.Id.parent_classnotice);
			var kgnotice = FindViewById<LinearLayout>(Resource.Id.parent_kgnotice);

			mPullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.recommendation_list);
			mPullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.Disabled;
			var actualListView = (ListView)mPullRefreshListView.RefreshableView;

			mHandler = new Handler();
			_radapter = new RecommendationAdpter(this, eCPostInfo);
			actualListView.Adapter = _radapter;

			ViewModel.FirstLoadedEventHandler += (s, list) => mHandler.PostDelayed(new Runnable(() => {
				
				ECPostInfo = (List<ECPostInfo>)list;
				_radapter.List = ECPostInfo;
				_radapter.NotifyDataSetChanged();
				InitChildInfo();
			}), 1000);
		}


		private void InitGroupButton()
		{

			var gridview = FindViewById<GridView>(Resource.Id.index_grid);

			var list = images.Select((t, i) => new JavaDictionary<string, object> { //必须是JavaDictionary。Dictionary会出错
				{ ImageResourceKey, t }, { TextResourceKey, _texts[i] }
			}).Cast<IDictionary<string, object>>().ToList();

			var saGroupButton = new SimpleAdapter(this,
				                    list,
				                    Resource.Layout.Item_EC_ButtonGroup,
				                    new[] { ImageResourceKey, TextResourceKey },
				                    new[] {
					Resource.Id.MainActivityImage,
					Resource.Id.MainActivityText
				});

			gridview.Adapter = saGroupButton;

			gridview.ItemClick += (sender, args) => {
				//Intent intent;

				switch (args.Position) {
					case 0:
						ViewModel.ShowActivityCourseIndexViewModel(ECColumnType.Course);
						break;
					case 1: 
						ViewModel.ShowActivityCourseIndexViewModel(ECColumnType.Activity);
						break;
					case 2:
						ViewModel.ShowForumIndexViewModel();
						break;
					case 3:
					
						break;
					default:
						break;
				}
			};
		}

		private void InitChildInfo()
		{
			_adapter = new ImageSlideAdapter(SupportFragmentManager) { ImageList = ViewModel.AdvertisementStrList ?? new List<string>() };
			_pager = FindViewById<ViewPager>(Resource.Id.pager);
			_pager.Adapter = _adapter;
			_indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
			_indicator.SetViewPager(_pager);
		}


		protected override void OnDestroy()
		{
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}

		private void SetListViewHeightBasedOnChildren(ListView listView)
		{
			var listAdapter = listView.Adapter;
			if (listAdapter == null) {
				// pre-condition
				return;
			}

			int totalHeight = 0;
			for (int i = 0; i < listAdapter.Count; i++) {
				var listItem = listAdapter.GetView(i, null, listView);
				// listItem.measure(0, 0);
				listItem.Measure(
					View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
					View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
				totalHeight += listItem.MeasuredHeight;
			}


			ViewGroup.LayoutParams test = listView.LayoutParameters;
			test.Height = totalHeight
			+ (listView.DividerHeight * (listAdapter.Count - 1));
			listView.LayoutParameters = test;
		}
	}
}

