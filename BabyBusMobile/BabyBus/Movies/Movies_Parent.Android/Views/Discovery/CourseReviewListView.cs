using System;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Views;
using Android.App;
using Android.OS;
using Com.Handmark.Pulltorefresh.Library;
using System.Collections.Generic;
using Android.Widget;
using Java.Lang;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme")]
	public class CourseReviewListView : ViewBase<CourseReviewListViewModel>, PullToRefreshBase.IOnRefreshListener2
	{
		PullToRefreshListView _pullRefreshListView;
		Handler _handler;
		CourseReviewAdapter _adapter;

		List<ECReview> _reviews;

		public List<ECReview> Reviews {
			get { 
				return _reviews ?? new List<ECReview>();
			}
			set { 
				_reviews = value;
			}
		}

		public CourseReviewListView()
		{
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e) {
			base.OnCreateCalled(sender, e);

			// Create your application here
			SetCustomTitleWithBack(Resource.Layout.Page_EC_CourseReviewList, Resource.String.ec_title_review);

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			_pullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.pull_refresh_list);
			_pullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.PullUpToRefresh;
			var actualListView = (ListView)_pullRefreshListView.RefreshableView;

			_handler = new Handler();
			_adapter = new CourseReviewAdapter(this, Reviews);
			actualListView.Adapter = _adapter;

			ViewModel.DataLoadedMore += (sender1, addList) => _handler.PostDelayed(new Runnable(() => {
				foreach (var item in addList) {
					Reviews.Add(item);
				}
				RefreshAdapter();
			}), 1000);

			ViewModel.FirstLoadedEventHandler += (sender1, listObject) => _handler.Post(new Runnable(() => {
				Reviews = (List<ECReview>)listObject;
				_adapter.list = Reviews;
				_adapter.NotifyDataSetChanged();
			}));

			_pullRefreshListView.SetOnRefreshListener(this);
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}

		#region pullrefresh ListView Listener

		public void OnPullDownToRefresh(PullToRefreshBase p0) {
			
		}

		public void OnPullUpToRefresh(PullToRefreshBase p0) {
			ViewModel.LoadMoreCommand.Execute();
		}

		#endregion

		private void RefreshAdapter() {
			_adapter.NotifyDataSetChanged();
			_pullRefreshListView.OnRefreshComplete();
		}
	}
}

