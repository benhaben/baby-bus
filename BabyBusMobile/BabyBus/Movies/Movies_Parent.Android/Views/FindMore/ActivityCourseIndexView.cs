using Android.App;
using Android.OS;
using Java.Lang;
using Com.Handmark.Pulltorefresh.Library;
using Android.Widget;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Views;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class ActivityCourseIndexView : ViewBase<ActivityCourseIndexViewModel>, PullToRefreshBase.IOnRefreshListener2
	{
		private ActivityCourseIndexAdapter _adapter;
		private Handler mHandler;
		private PullToRefreshListView mPullRefreshListView;

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
			switch (ViewModel.ECCoulumnType) {
				case ECColumnType.Activity:
					SetCustomTitleWithBack(Resource.Layout.Page_EC_ActivityCourseIndex, "活动");
					break;
				case ECColumnType.Course:
					SetCustomTitleWithBack(Resource.Layout.Page_EC_ActivityCourseIndex, "课程");
					break;
				default:
					SetCustomTitleWithBack(Resource.Layout.Page_EC_ActivityCourseIndex, "更多");
					break;
			}


			NetContectStatus.registerReceiver(this);
			var netStatu = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatu);

			mPullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.pull_refresh_list);
			mPullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.Both;
			var actualListView = (ListView)mPullRefreshListView.RefreshableView;

			mHandler = new Handler();
			_adapter = new ActivityCourseIndexAdapter(this, new List<ECPostInfo>());
			actualListView.Adapter = _adapter;


			ViewModel.FirstLoadedEventHandler += (sender1, listObject) => mHandler.PostDelayed(new Runnable(() => {
				ECPostInfo = (List<ECPostInfo>)listObject;
				_adapter.list = ECPostInfo;
				_adapter.NotifyDataSetChanged();
				mPullRefreshListView.SetOnRefreshListener(this);
			}), 1000);

			ViewModel.DataRefreshed += (sender1, addList) => mHandler.PostDelayed(new Runnable(() => {
				ECPostInfo.InsertRange(0, addList);
				RefreshAdapter();
			}), 1000);
			ViewModel.DataLoadedMore += (sender1, addList) => mHandler.PostDelayed(new Runnable(() => {
				foreach (var item in addList) {
					ECPostInfo.Add(item);
				}
				RefreshAdapter();
			}), 1000);
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

		protected override void OnResume()
		{
			base.OnResume();
			if (ViewModel != null)
				ViewModel.RefreshCommand.Execute();
			else {
				//Refresh Readed Status
				_adapter.NotifyDataSetChanged();
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			CloseView();
		}

		private void RefreshAdapter()
		{
			_adapter.list = ECPostInfo;
			_adapter.NotifyDataSetChanged();
			mPullRefreshListView.OnRefreshComplete();
			mPullRefreshListView.OnRefreshComplete();
		}
	}


			
}

