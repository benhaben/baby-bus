using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Views;
using Android.Webkit;
using Com.Handmark.Pulltorefresh.Library;
using System.Collections.Generic;
using Java.Lang;
using System;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]	
	public class ForumIndexView : ViewBase<ForumIndexViewModel>, PullToRefreshBase.IOnRefreshListener2
	{
		PullToRefreshListView _pullRefreshListView;
		Handler _handler;
		ForumIndexAdapter _adapter;
		private bool _isFirst = true;

		private List<ECPostInfo> _postinfos = new List<ECPostInfo>();

		public List<ECPostInfo> PostInfos {
			get { 
				return _postinfos ?? new List<ECPostInfo>();
			}
			set { 
				_postinfos = value;
			}
		}

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

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			// Create your application here
			//SetContentView(Resource.Layout.Page_EC_ForumIndex);

			bool requestWindowFeature = false;
			requestWindowFeature = RequestWindowFeature(WindowFeatures.CustomTitle);
			SetContentView(Resource.Layout.Page_EC_ForumIndex);
			if (requestWindowFeature) {
				Window.SetFeatureInt(WindowFeatures.CustomTitle, Resource.Layout.Bar_Default_WithBackAndAction);
				var btnBack = FindViewById<LinearLayout>(Resource.Id.ll_back);
				btnBack.Click += (object sender1, EventArgs arg) => CloseView();
				var topTitle = FindViewById<TextView>(Resource.Id.title);
				topTitle.Text = "论坛";
				var labelAction = FindViewById<TextView>(Resource.Id.label_action);
				labelAction.Text = "发帖";
				labelAction.Click += (s, cw) => ViewModel.SendForumViewModelCommand.Execute();
			} 

//			NetContectStatus.registerReceiver(this);
//			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
//			NetContectStatus.NetStatus(this, netStatus);

			_pullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.pull_refresh_list);
			_pullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.Both;
			var actualListView = (ListView)_pullRefreshListView.RefreshableView;

			_handler = new Handler();
			_adapter = new ForumIndexAdapter(this, PostInfos);
			actualListView.Adapter = _adapter;
			Button[] button = new Button[] {
				FindViewById<Button>(Resource.Id.button_1),
				FindViewById<Button>(Resource.Id.button_2),
				FindViewById<Button>(Resource.Id.button_3),
				FindViewById<Button>(Resource.Id.button_4),
				FindViewById<Button>(Resource.Id.button_5),
				FindViewById<Button>(Resource.Id.button_6),
			};

			ViewModel.DataLoadedMore += (sender1, addList) => _handler.PostDelayed(new Runnable(() => {
				foreach (var item in addList) {
					PostInfos.Add(item);
				}
				RefreshAdapter();
			}), 1000);

			ViewModel.DataRefreshed += (sender1, addList) => _handler.PostDelayed(new Runnable(() => {
				PostInfos.InsertRange(0, addList);
				RefreshAdapter();
			}), 1000);

			ViewModel.FirstLoadedEventHandler += (sender1, listObject) => _handler.Post(new Runnable(() => {
				var count = ViewModel.CategoryList == null ? 0 : ViewModel.CategoryList.Count;
				var Category = ViewModel.CategoryList;
				if (_isFirst) {
					_isFirst = false;
					for (var buttonId = 0; buttonId < count; buttonId++) {
						button[buttonId].Text = Category[buttonId].Name;
						button[buttonId].Visibility = ViewStates.Visible;
						button[buttonId].Tag = buttonId;
						button[buttonId].Click += (se, ee) => {
							var category = (Button)se;
							var num = (int)category.Tag;
							for (var categoryId = 0; categoryId < Category.Count; categoryId++) {
								if (categoryId == num) {
									button[categoryId].SetTextColor(Color.Blue);
								} else {
									button[categoryId].SetTextColor(Color.DarkGray);
								}
							}
							ViewModel.MaxId = 0;
							ViewModel.MinId = 0;
							ViewModel.CategoryId = Category[num].Id;
							ViewModel.FirstLoad();
						};
					}
				}
				PostInfos = (List<ECPostInfo>)listObject;
				_adapter.list = PostInfos;
				_adapter.NotifyDataSetChanged();
				_pullRefreshListView.SetOnRefreshListener(this);
			}));
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
			_adapter.NotifyDataSetChanged();
			_pullRefreshListView.OnRefreshComplete();
		}

	}
}

