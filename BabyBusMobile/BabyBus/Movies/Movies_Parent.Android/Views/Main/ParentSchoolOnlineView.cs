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
using Android.Views;
using BabyBus.Logic.Shared;
using Android.Content.PM;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid.Views.Main
{
	[Activity(Label = "SchoolPaper", ScreenOrientation = ScreenOrientation.Portrait)]
	public class ParentSchoolOnlineView : FragmentViewBase<ParentSchoolOnlineViewModel>
	{
		private const string ImageResourceKey = "ItemImage";
		private const string TextResourceKey = "TextImage";

		private readonly int[] images = {
			Resource.Drawable.Menu_Memory,
			Resource.Drawable.Menu_Notice,
			Resource.Drawable.Menu_Share,
			Resource.Drawable.Menu_Memo,
			Resource.Drawable.Menu_Mail,
			Resource.Drawable.Menu_ParentsSchool
		};

		private ImageSlideAdapter _adapter;
		private Handler _handler;
		private IPageIndicator _indicator;
		private ViewPager _pager;
		private MainView _parentTab;

		private readonly string[] _texts = {

			"成长记忆",
			"信息通知",
			"家园共育",
			"请假留言",
			"园长信箱",
			"家长学堂"
		};

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			// Create your application here
			SetContentView(Resource.Layout.Page_Home_ParentNew);
			_handler = new Handler();
			_parentTab = Parent as MainView;
			InitGroupButton();


			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			var classnotice = FindViewById<LinearLayout>(Resource.Id.parent_classnotice);
			var kgnotice = FindViewById<LinearLayout>(Resource.Id.parent_kgnotice);

			classnotice.Click += (senders, es) => {
				if (ViewModel.ClassNoticeId != 0)
					ViewModel.ShowNoticeDetailViewModel(ViewModel.ClassNotice.NoticeId, ViewModel.ClassNotice.IsHtml);
			}; 
			kgnotice.Click += (senders, es) => {
				if (ViewModel.KindergartenNoticeId != 0) {
					ViewModel.ShowNoticeDetailViewModel(ViewModel.KindergartenNotice.NoticeId, ViewModel.KindergartenNotice.IsHtml);
				}	
			};

			ViewModel.FirstLoadedEventHandler += (senders, argss) => _handler.Post(() => {
				classnotice.Visibility = (ViewModel.ClassNoticeId == 0) 
					? ViewStates.Gone : ViewStates.Visible;
				kgnotice.Visibility = (ViewModel.KindergartenNoticeId == 0)
					? ViewStates.Gone : ViewStates.Visible; 
				InitADInfo();
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
						ViewModel.ShowMemoryIndexViewCommand.Execute();
						break;
					case 1: 
						ViewModel.NoticeIndexViewCommand.Execute();
						break;
					case 2:
						ViewModel.QuestionIndexViewCommand.Execute();
						break;
					case 3:
						ViewModel.QuestionCommand.Execute();
						break;
					case 4:
						ViewModel.SendToMasterCommand.Execute();
						break;
					case 5:
						ViewModel.ShowLearningMaterialsViewCommand();
						break;
					default:
						break;
				}
			};
		}

		private void InitADInfo()
		{

			_adapter = new ImageSlideAdapter(SupportFragmentManager) { ImageList = ViewModel.AdvertisementStrList ?? new List<string>() };
			_pager = FindViewById<ViewPager>(Resource.Id.pager);
			_pager.Adapter = _adapter;
			_indicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
			_indicator.SetViewPager(_pager);

			var ll_cn = FindViewById<LinearLayout>(Resource.Id.parent_classnotice);
			ll_cn.Click += (sender, args) => {
				if (_parentTab != null)
					_parentTab.TabHost.SetCurrentTabByTag("Notice");
			};
			var ll_kn = FindViewById<LinearLayout>(Resource.Id.parent_kgnotice);
			ll_kn.Click += (sender, args) => {
				if (_parentTab != null)
					_parentTab.TabHost.SetCurrentTabByTag("Notice");
			};
		}

		protected override void OnResume()
		{
			base.OnResume();

			//ViewModel.InitData ();
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
