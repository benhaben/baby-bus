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
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class SettingPayOrderView : ViewBase<SettingPayOrderViewModel>, PullToRefreshBase.IOnRefreshListener2
	{
		PullToRefreshListView _pullRefreshListView;
		Handler _handler;
		PostPayOrderAdapter _adapter;

		List<ECPayOrder> _eCPayOrder;

		public List<ECPayOrder> ECPayOrder {
			get { 
				return _eCPayOrder ?? new List<ECPayOrder>();
			}
			set { 
				_eCPayOrder = value;
			}
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			// Create your application here
			SetCustomTitleWithBack(Resource.Layout.Page_SettingPayOrderList, "我的订单");


			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			_pullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.pull_refresh_list);
			_pullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.PullUpToRefresh;
			var actualListView = (ListView)_pullRefreshListView.RefreshableView;

			_handler = new Handler();
			_adapter = new PostPayOrderAdapter(this, ECPayOrder);
			actualListView.Adapter = _adapter;

			ViewModel.DataLoadedMore += (sender1, addList) => _handler.PostDelayed(new Runnable(() => {
				foreach (var item in addList) {
					ECPayOrder.Add(item);
				}
				RefreshAdapter();
			}), 1000);

			ViewModel.FirstLoadedEventHandler += (sender1, listObject) => _handler.Post(new Runnable(() => {
				ECPayOrder = (List<ECPayOrder>)listObject;
				_adapter.List = ECPayOrder;
				_adapter.NotifyDataSetChanged();
			}));

			_pullRefreshListView.SetOnRefreshListener(this);
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




