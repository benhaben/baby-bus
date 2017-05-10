using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Widget;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.ViewPagerIndicator;
using BabyBus.Droid.ViewPagerIndicator.Interfaces;
using BabyBus.Droid.Views.Content;
using Android.Graphics;
using System;
using Android.Views;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using System.IO;
using Android.Content.PM;

namespace BabyBus.Droid.Views.Main
{
	[Activity(Label = "ParentHomeView", ScreenOrientation = ScreenOrientation.Portrait)]
	public class ParentHomeView : FragmentViewBase<ParentHomeViewModel>
	{
		private const string ImageResourceKey = "ItemImage";
		private const string TextResourceKey = "TextImage";
		private readonly int[] images = {
			Resource.Drawable.Menu_SchoolPaper,
			Resource.Drawable.Menu_Paradise,
			Resource.Drawable.Menu_FindMore,
			Resource.Drawable.Menu_MultipleIntelligence,
			Resource.Drawable.Menu_friendCricle,
			Resource.Drawable.Menu_add,
		};

		private ImageSlideAdapter _adapter;
		private Handler _handler;
		private IPageIndicator _indicator;
		private ViewPager _pager;
		private MainView _parentTab;
		private IPictureService pic;

		private readonly string[] _texts = {

			"校讯",
			"体智",
			"发现",
			"圈子",
			"乐园",
			"添加",
		};

		public ParentHomeView()
		{
			pic = Mvx.Resolve<IPictureService>();
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);
			SetContentView(Resource.Layout.Page_HoneMain);
			_handler = new Handler();
			_parentTab = Parent as MainView;

			InitGroupButton();
			//InitChildInfo();

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			var childhead = FindViewById<ImageView>(Resource.Id.child_head);
			if (BabyBusContext.UserAllInfo.Child.HasHeadImage) {
				var fileName = BabyBusContext.UserAllInfo.Child.ImageName + Constants.ThumbRule;
				pic.LoadIamgeFromSource(fileName, stream => {
					var ms = stream as MemoryStream;
					if (ms != null) {
						var bytes = ms.ToArray();
						var options = new BitmapFactory.Options() { InPurgeable = true };
						var bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
						childhead.SetImageBitmap(bmp);
					}
				}, Constants.ThumbServerPath);
			}
			var mhandler = new Handler();
			ViewModel.FirstLoadedEventHandler += (sender1, e1) => mhandler.Post(delegate {
				_adapter = new ImageSlideAdapter(SupportFragmentManager) {
					ImageList = ViewModel.AdvertisementStrList
				};
				_pager = FindViewById<ViewPager>(Resource.Id.pager);
				_pager.Adapter = _adapter;
				_indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
				_indicator.SetViewPager(_pager);
			});

		}

		private void InitGroupButton()
		{
			var gridview = FindViewById<GridView>(Resource.Id.parent_grid);

			var list = images.Select((t, i) => new JavaDictionary<string, object> { //必须是JavaDictionary。Dictionary会出错
				{ ImageResourceKey, t }, { TextResourceKey, _texts[i] }
			}).Cast<IDictionary<string, object>>().ToList();

			var saGroupButton = new SimpleAdapter(this,
				                    list,
				                    Resource.Layout.Item_ButtonGroup,
				                    new[] { ImageResourceKey, TextResourceKey },
				                    new[] {
					Resource.Id.MainActivityImage,
					Resource.Id.MainActivityText
				});

			gridview.Adapter = saGroupButton;

			gridview.ItemClick += (sender, args) => {
				switch (args.Position) {
					case 0:
						if (_parentTab != null)
							_parentTab.TabHost.SetCurrentTabByTag("SchoolPaper");
						break;
					case 1: 
						if (_parentTab != null)
							_parentTab.TabHost.SetCurrentTabByTag("TestHome");
						break;
					case 2:
						if (_parentTab != null)
							_parentTab.TabHost.SetCurrentTabByTag("Discovery");
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