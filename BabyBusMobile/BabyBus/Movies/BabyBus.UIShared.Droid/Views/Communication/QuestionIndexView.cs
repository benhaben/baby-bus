using Android.App;
using Android.OS;
using BabyBus.Droid.Adapters;
using Java.Lang;
using Com.Handmark.Pulltorefresh.Library;
using Android.Widget;
using System.Linq;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using BabyBus.Logic.Shared;
using Android.Content.PM;

namespace BabyBus.Droid.Views.Communication
{
	#if __PARENT__
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = ScreenOrientation.Portrait)]	


#else
	[Activity]
	#endif
	public class QuestionIndexView : ViewBase<QuestionIndexViewModel>, PullToRefreshBase.IOnRefreshListener2
	{
		private QuestionIndexAdapter _adapter;
		private Handler mHandler;
		private PullToRefreshListView mPullRefreshListView;

		private List<QuestionModel> _questions;

		public List<QuestionModel> Questions {
			get {
				return _questions ?? new List<QuestionModel>();
			}
			set {
				_questions = value;
			}
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			NetContectStatus.registerReceiver(this);
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			#if __PARENT__
			SetCustomTitleWithBack(Resource.Layout.Page_Comm_QuestionIndex, "家园共育");
			#else
			SetContentView(Resource.Layout.Page_Comm_QuestionIndex);
			#endif

			var netStatu = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatu);

			mPullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.pull_refresh_list);
			mPullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.Both;
			var actualListView = (ListView)mPullRefreshListView.RefreshableView;

			mHandler = new Handler();
			_adapter = new QuestionIndexAdapter(this, Questions);
			actualListView.Adapter = _adapter;

			ViewModel.DataRefreshed += (sender1, addlist) => mHandler.PostDelayed(new Runnable(() => {
				Questions = addlist;
				_adapter.list = Questions;
				RefreshAdapter();
			}), 1000);
			ViewModel.DataLoadedMore += (sender1, addlist) => mHandler.PostDelayed(new Runnable(() => {
				foreach (var item in addlist) {
					if (Questions.Count(x => x.QuestionId == item.QuestionId) == 0)
						Questions.Add(item);
				}
				RefreshAdapter();
			}), 1000);
			ViewModel.FirstLoadedEventHandler += (sender1, listObject) => mHandler.PostDelayed(new Runnable(() => {
				//                this.HideInfo();
				Questions = (List<QuestionModel>)listObject;
				_adapter.list = Questions;
				_adapter.NotifyDataSetChanged();
			}), 1000);

			mPullRefreshListView.SetOnRefreshListener(this);

			var messenger = Mvx.Resolve<IMvxMessenger>();
			
			messenger.Subscribe<QuestionMessage>(m => {
				var item = Questions.FirstOrDefault(x => x.QuestionId == m.Question.QuestionId);
				if (item != null) {
					item = m.Question;
				}
			});


		}

		protected override void OnResume()
		{
			base.OnResume();
			if (!IsOnCreate) {
				if (ViewModel != null)
					ViewModel.RefreshCommand.Execute();
			} else {
				IsOnCreate = false;
			}
		}

		void RefreshAdapter()
		{
			_adapter.NotifyDataSetChanged();
			mPullRefreshListView.OnRefreshComplete();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}

		#region Pull To Refresh

		public void OnPullDownToRefresh(PullToRefreshBase p0)
		{
			ViewModel.RefreshCommand.Execute();
		}

		public void OnPullUpToRefresh(PullToRefreshBase p0)
		{
			ViewModel.LoadMoreCommand.Execute();
		}

		#endregion
	}
}