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

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme")]		
	public class ForumIndexView : ViewBase<ForumIndexViewModel>
	{
		private PullToRefreshListView mPullRefreshListView;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
			SetCustomTitleWithBack(Resource.Layout.Page_EC_Forum, "论坛");
			var Button1 = FindViewById<TextView>(Resource.Id.buttom_1);
			var Button2 = FindViewById<TextView>(Resource.Id.buttom_2);
			var Button3 = FindViewById<TextView>(Resource.Id.buttom_3);
			var Button4 = FindViewById<TextView>(Resource.Id.buttom_4);

			mPullRefreshListView = FindViewById<PullToRefreshListView>(Resource.Id.pull_refresh_list);
			mPullRefreshListView.Mode = PullToRefreshBase.PullToRefreshMode.Both;







		}

	}
}

