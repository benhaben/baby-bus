using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Views;
using Android.Webkit;
using Java.Lang;
using BabyBus.Droid.Views.Member;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class CourseDetailView : ViewBase<CourseDetailViewModel>
	{
		ECColumnType _type = ECColumnType.Course;

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);

			_type = ViewModel.ECColumnType;
			int title;
			if (_type == ECColumnType.Course) {
				title = Resource.String.ec_title_courseDetail;
			} else {
				title = Resource.String.ec_title_activityDetail;
			}
			SetCustomTitleWithBack(Resource.Layout.Page_EC_CourseDetail, title);

			//more review
			var moreReview = FindViewById<LinearLayout>(Resource.Id.more_review);
			moreReview.Click += (s, ce) => ViewModel.ShowReviewListCommand.Execute();

			var webview = FindViewById<WebView>(Resource.Id.webview);
			//webview.LoadUrl("file:///android_asset/Content/" + "FindMore/Talent gene detection.htm");
		
			var rating = FindViewById<RatingBar>(Resource.Id.user_recommendationindex);
			rating.Rating = (float)ViewModel.PostInfo.Rating;

			var reviewlist = FindViewById<ListView>(Resource.Id.review_list);


			var adapter = new CourseReviewAdapter(this, ViewModel.ECReviewList);
			reviewlist.Adapter = adapter;
			ListViewHeightBasedOnChildren.SetListView(reviewlist);
			var mHandler = new Handler();

			ViewModel.FirstLoadedEventHandler += (se, ee) => mHandler.Post(new Runnable(() => {
				webview.LoadData(ViewModel.Description, "text/html; charset=UTF-8", null);

				adapter.list = ViewModel.ECReviewList;
				reviewlist.Adapter = adapter;
				ListViewHeightBasedOnChildren.SetListView(reviewlist);
				adapter.NotifyDataSetChanged();

			}));
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			CloseView();
		}
	}
}

