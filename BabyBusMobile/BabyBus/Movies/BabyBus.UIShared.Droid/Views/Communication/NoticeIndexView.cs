using System.Collections.Generic;
using Android.App;
using Android.OS;
using BabyBus.Droid.Adapters;
using Java.Lang;
using Com.Handmark.Pulltorefresh.Library;
using Android.Widget;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using Android.Content.PM;

namespace BabyBus.Droid.Views.Communication
{
	/// <summary>
	/// Notice Index View
	/// </summary>
	#if __PARENT__
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	#else
	[Activity]
	#endif
	public class NoticeIndexView : ViewBase<NoticeIndexViewModel>, PullToRefreshBase.IOnRefreshListener2
	{
		private PullToRefreshListView mPullRefreshListView;
		private Handler mHandler;
		private BaseNoticeIndexAdapter mAdapter;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

		}

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
			SetCustomTitleWithBack(Resource.Layout.Page_Comm_NoticeIndex, "信息通知");
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
			mAdapter = new NoticeIndexAdapter(this, Notices);

			actualListView.Adapter = mAdapter;
			ViewModel.DataRefreshed += (sender1, addList) => {
				mHandler.PostDelayed(new Runnable(() => {
					Notices.InsertRange(0, addList);
					RefreshAdapter();
				}), 1000);
			};
			ViewModel.DataLoadedMore += (sender1, addList) => mHandler.PostDelayed(new Runnable(() => {
				foreach (var item in addList) {
					Notices.Add(item);
//					ViewModel.Notices.Add(item);
				}
				RefreshAdapter();
			}), 1000);
			ViewModel.FirstLoadedEventHandler += (sender1, listObject) => mHandler.Post(new Runnable(() => {
				//this.HideInfo();
				Notices = (List<NoticeModel>)listObject;
				mAdapter.list = Notices;
//				mAdapter.list = ViewModel.Notices;
				mAdapter.NotifyDataSetChanged();
			}));


			mPullRefreshListView.SetOnRefreshListener(this);
		}

		protected override void OnResume()
		{
			base.OnResume();
			if (ViewModel != null)
				ViewModel.RefreshCommand.Execute();
			else {
				//Refresh Readed Status
				mAdapter.NotifyDataSetChanged();
			}
			//Stats Page Report
			StatsUtils.LogPageReport(PageReportType.NoticeIndex);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}

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
}