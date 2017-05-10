using System;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Views;
using Android.App;
using Android.OS;
using Com.Handmark.Pulltorefresh.Library;
using System.Collections.Generic;
using Android.Widget;
using Java.Lang;
using Android.Views;
using Android.Content.PM;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class SettingCommentView : ViewBase<SettingCommentViewModel>, PullToRefreshBase.IOnRefreshListener2
	{
		PullToRefreshListView _pullRefreshListView;
		Handler _handler;
		PostAdapter _adapter;

		List<ECPostInfo> _postInfo;

		public List<ECPostInfo> PostInfo {
			get { 
				return _postInfo ?? new List<ECPostInfo>();
			}
			set { 
				_postInfo = value;
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			if (ViewModel != null)
				ViewModel.FirstLoad();
			else {
				//Refresh Readed Status
				_adapter.NotifyDataSetChanged();
			}
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			// Create your application here
			SetCustomTitleWithBack(Resource.Layout.Page_Setting_PostCommentList, "我的帖子");


			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			_pullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.pull_refresh_list);
			_pullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.PullUpToRefresh;
			var actualListView = (ListView)_pullRefreshListView.RefreshableView;

			_handler = new Handler();
			_adapter = new PostAdapter(this, PostInfo);
			actualListView.Adapter = _adapter;

			ViewModel.DataLoadedMore += (sender1, addList) => _handler.PostDelayed(new Runnable(() => {
				foreach (var item in addList) {
					PostInfo.Add(item);
				}
				RefreshAdapter();
			}), 1000);

			ViewModel.FirstLoadedEventHandler += (sender1, listObject) => _handler.Post(new Runnable(() => {
				PostInfo = (List<ECPostInfo>)listObject;
				_adapter.list = PostInfo;
				_adapter.NotifyDataSetChanged();
			}));

			_pullRefreshListView.SetOnRefreshListener(this);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}

		#region pullrefresh ListView Listener

		public void OnPullDownToRefresh(PullToRefreshBase p0)
		{

		}

		public void OnPullUpToRefresh(PullToRefreshBase p0)
		{
			ViewModel.LoadMoreCommand.Execute();
		}

		#endregion

		private void RefreshAdapter()
		{
			_adapter.NotifyDataSetChanged();
			_pullRefreshListView.OnRefreshComplete();
		}
	}
}




