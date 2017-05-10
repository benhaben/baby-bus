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
	public class ForumCommentListView 
		: ViewBase<ForumCommentListViewModel>, PullToRefreshBase.IOnRefreshListener2
	{
		PullToRefreshListView _pullRefreshListView;
		Handler _handler;
		ForumCommentAdapter _adapter;

		List<ECComment> _comments;

		public List<ECComment> Comments {
			get { 
				return _comments ?? new List<ECComment>();
			}
			set { 
				_comments = value;
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
			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle); 

			SetContentView(Resource.Layout.Page_EC_CourseReviewList);
			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Default_WithBackAndAction);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender1, EventArgs arg) => {
					CloseView();
				};
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = Resources.GetString(Resource.String.ec_title_review);
				var labelAction = FindViewById<TextView>(Resource.Id.label_action);
				labelAction.Text = "评论";
				labelAction.Click += (s, cw) => {
					ViewModel.SendCommentCommand.Execute();
				};
			} 

			RequestedOrientation = ScreenOrientation.Nosensor;


			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			_pullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.pull_refresh_list);
			_pullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.PullUpToRefresh;
			var actualListView = (ListView)_pullRefreshListView.RefreshableView;

			_handler = new Handler();
			_adapter = new ForumCommentAdapter(this, Comments);
			actualListView.Adapter = _adapter;

			ViewModel.DataLoadedMore += (sender1, addList) => _handler.PostDelayed(new Runnable(() => {
				foreach (var item in addList) {
					Comments.Add(item);
				}
				RefreshAdapter();
			}), 1000);

			ViewModel.FirstLoadedEventHandler += (sender1, listObject) => _handler.Post(new Runnable(() => {
				Comments = (List<ECComment>)listObject;
				_adapter.list = Comments;
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

