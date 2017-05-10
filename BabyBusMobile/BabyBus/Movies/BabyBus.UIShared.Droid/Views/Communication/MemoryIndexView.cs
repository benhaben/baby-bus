using System.Collections.Generic;
using Android.App;
using Android.OS;
using BabyBus.Droid.Adapters;
using Java.Lang;
using Com.Handmark.Pulltorefresh.Library;
using Android.Widget;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using Com.Squareup.Picasso;
using Android.Content.PM;

namespace BabyBus.Droid.Views.Communication
{
	
	#if __PARENT__
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = ScreenOrientation.Portrait)]	
	



	




#else
	[Activity]
	#endif
	public class MemoryIndexView : ViewBase<MemoryIndexViewModel>, PullToRefreshBase.IOnRefreshListener2
	{
		private PullToRefreshListView mPullRefreshListView;
		private Handler mHandler;
		//private BaseNoticeIndexAdapter mAdapter;
		private MemoryIndexAdapter mAdapter;
		private bool _isFirst = true;
		private float dp;

		private List<NoticeModel> _notices;

		public List<NoticeModel> Notices {
			get {
				return _notices ?? new List<NoticeModel>();
			}
			set {
				_notices = value;
			}
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			#if __PARENT__
			SetCustomTitleWithBack(Resource.Layout.Page_Comm_NoticeIndex, "成长记忆");
			#else
			SetContentView(Resource.Layout.Page_Comm_NoticeIndex);
			#endif

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			mPullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.pull_refresh_list);
			mPullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.Both;
			var actualListView = (ListView)mPullRefreshListView.RefreshableView;


			mHandler = new Handler();
			dp = (float)Resources.DisplayMetrics.Density;
			mAdapter = new MemoryIndexAdapter(this, Notices, dp);
			actualListView.Adapter = mAdapter;
			ViewModel.DataRefreshed += (sender1, addList) => mHandler.PostDelayed(new Runnable(() => {
				Notices.InsertRange(0, addList);
				RefreshAdapter();
			}), 1000);
			ViewModel.DataLoadedMore += (sender1, addList) => mHandler.PostDelayed(new Runnable(() => {
				foreach (var item in addList) {
					Notices.Add(item);
					//					ViewModel.Notices.Add(item);
				}
				RefreshAdapter();
			}), 1500);
			ViewModel.FirstLoadedEventHandler += (sender1, listObject) => mHandler.Post(new Runnable(() => {
				//this.HideInfo();
				Notices = (List<NoticeModel>)listObject;
				mAdapter.list = Notices;
				//				mAdapter.list = ViewModel.Notices;
				mAdapter.NotifyDataSetChanged();
				_isFirst = false;
				//ViewModel.IsLoading = false;
			}));

			//			var soundListener = new SoundPullEventListener (this);
			//			soundListener.AddSoundEvent(PullToRefreshBase.PullToRefreshState.PullToRefresh, Resource.Raw.pull_event);
			//			soundListener.AddSoundEvent(PullToRefreshBase.PullToRefreshState.Reset, Resource.Raw.reset_sound);
			//			soundListener.AddSoundEvent(PullToRefreshBase.PullToRefreshState.Refreshing, Resource.Raw.refreshing_sound);
			//			mPullRefreshListView.SetOnPullEventListener (soundListener);
			mPullRefreshListView.SetOnRefreshListener(this);
		}

		protected override void OnResume()
		{
			base.OnResume();
			if (ViewModel != null)
				ViewModel.RefreshCommand.Execute();
			//Stats Page Report
			StatsUtils.LogPageReport(PageReportType.GrowMomeryIndex);
		}


		protected override void OnDestroy()
		{
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}


		#if false //ÃÂ¨Ã–ÂªÂºÃÃ—Ã·Ã’ÂµÂµÃ„ÃˆÃ«Â¿Ãš
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
		//             var m1 = menu.Add(0, 1, 0, "ÃÃ‚Ã”Ã¶");
		//             m1.SetShowAsActionFlags(ShowAsAction.IfRoom | ShowAsAction.WithText);

		MenuInflater.Inflate(Resource.Menu.media_menu, menu);


		return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item) {
		switch (item.ItemId) {
		case 1: {

		}
		break;
		}
		return base.OnOptionsItemSelected(item);
		}

		//        public override void OnWindowFocusChanged(bool hasFocus) {
		//            //ActivityÃ‰ÃºÃƒÃ¼Ã–ÃœÃ†ÃšÃ–ÃÂ£Â¬onStart, onResume, onCreateÂ¶Â¼Â²Â»ÃŠÃ‡Ã•Ã¦Ã•Ã½visibleÂµÃ„ÃŠÂ±Â¼Ã¤ÂµÃ£Â£Â¬
		//            //Ã•Ã¦Ã•Ã½ÂµÃ„visibleÃŠÂ±Â¼Ã¤Ã£ÃŠÃ‡onWindowFocusChanged()ÂºÂ¯ÃŠÃ½Â±Â»Ã–Â´ÃÃÃŠÂ±
		//            base.OnWindowFocusChanged(hasFocus);
		//            if (hasFocus) {
		//                mListView.autoRefresh();
		//            }
		//        }
		#endif

		#region pullrefresh ListView Listener

		public void OnPullDownToRefresh(PullToRefreshBase p0)
		{
			ViewModel.RefreshCommand.Execute();
		}

		public void OnPullUpToRefresh(PullToRefreshBase p0)
		{
			ViewModel.LoadMoreCommand.Execute();
		}

		#endregion

		private void RefreshAdapter()
		{
			mAdapter.NotifyDataSetChanged();
			mPullRefreshListView.OnRefreshComplete();
		}
	}

	#if false
	[Activity(Label = "NoticeIndexView")]
	public class NoticeIndexView : ViewBase<NoticeIndexViewModel>
	{
	private IPullToRefresharpView ptr_view;
	private Handler handler = new Handler();
	protected override void OnCreate(Bundle bundle) {
	base.OnCreate(bundle);

	// Create your application here
	SetContentView(Resource.Layout.Page_Comm_NoticeIndex);

	ptr_view = (IPullToRefresharpView)FindViewById(Resource.Id.noticeList);

	ptr_view.RefreshActivated += ptr_view_RefreshActivated;
	ViewModel.DataRefreshed += (sender, args) => {
	if (ptr_view != null)
	{
	// When you are done refreshing your content, let PullToRefresharp know you're done.
	ptr_view.OnRefreshCompleted();
	}
	};
	}

	private async void ptr_view_RefreshActivated(object sender, EventArgs args) {
	// LOOK HERE!
	// Refresh your content when PullToRefresharp informs you that a refresh is needed
	handler.PostDelayed(()=> ViewModel.RefreshCommand.Execute(),1000);
	//            handler.PostDelayed(() => {
	//                if (ptr_view != null)
	//                {
	//                    // When you are done refreshing your content, let PullToRefresharp know you're done.
	//                    ptr_view.OnRefreshCompleted();
	//                }
	//            }, 2000);
	}
	}
	#endif
}