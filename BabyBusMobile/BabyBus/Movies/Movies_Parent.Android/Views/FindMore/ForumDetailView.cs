using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using Android.Webkit;
using Java.Lang;
using BabyBus.Droid.Views.Member;
using Android.Content;
using BabyBus.Droid.Views.Communication;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class ForumDetailView : ViewBase<ForumDetailViewModel>
	{
		private readonly Handler mhandler = new Handler();

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			// Create your application here
			SetShareTitleWithBack(Resource.Layout.Page_EC_ForumDetail, Resource.String.notice_detial);

			var webview = FindViewById<WebView>(Resource.Id.webview);
			//webview.LoadUrl("file:///android_asset/Content/" + "FindMore/Talent gene detection.htm");
			ViewModel.FirstLoadedEventHandler += (s, ec) => mhandler.Post(new Runnable(() => {

				var SendPraise = FindViewById<Button>(Resource.Id.sendpraise);
				var SendPraised = FindViewById<Button>(Resource.Id.sendpraised);
				SendPraise.Visibility = ViewModel.IsPraised ? ViewStates.Gone : ViewStates.Visible;
				SendPraised.Visibility = ViewModel.IsPraised ? ViewStates.Visible : ViewStates.Gone;
				SendPraise.Click += (senders, es) => {
					SendPraise.Visibility = ViewStates.Gone;
					SendPraised.Visibility = ViewStates.Visible;
					ViewModel.SendPraiseCommand.Execute();
				};
				webview.LoadData(ViewModel.Description, "text/html; charset=UTF-8", null);
				InitCommentInfo();

			}));
			ViewModel.AddedCommentEventHandler += (senders, es) => mhandler.Post(() => {
				ViewModel.UserComment = string.Format("圈子点评：{0}", ViewModel.Comment);
				InitCommentInfo();
			});
		}

		public override void ShareMessage()
		{
			base.ShareMessage();
			var intent = new Intent(this, typeof(OneKeyShare));
			intent.PutExtra("Description", ViewModel.PostInfo.Abstract);
			intent.PutExtra("ImageName", ViewModel.PostInfo.FirstImage);
			intent.PutExtra("Title", ViewModel.PostInfoTiTle);
			intent.PutExtra("WebpageUrl", string.Format("http://115.28.88.175:8099/api/sharehtml?ContentType={0}&Id={1}", 1, ViewModel.PostInfo.PostInfoId));
			StartActivity(intent);
		}

		private void InitCommentInfo()
		{
			var commentlist = FindViewById<ListView>(Resource.Id.review_list);
			var adapter = new ForumCommentAdapter(this, ViewModel.ECCommentList);
			commentlist.Adapter = adapter;
			adapter.list = ViewModel.ECCommentList;
			commentlist.Adapter = adapter;
			ListViewHeightBasedOnChildren.SetListView(commentlist);
			commentlist.Adapter = adapter;
			adapter.NotifyDataSetChanged();
		}


	}
}

