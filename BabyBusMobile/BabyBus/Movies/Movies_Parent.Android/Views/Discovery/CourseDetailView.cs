
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Views;
using Android.Webkit;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme")]		
	public class CourseDetailView : ViewBase<CourseDetailViewModel>
	{
		ECColumnType _type = ECColumnType.Course;

		void Init(ECColumnType type) {
			_type = type;
		}

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Create your application here
			int title;
			if (_type == ECColumnType.Course) {
				title = Resource.String.ec_title_courseDetail;
			} else {
				title = Resource.String.ec_title_activityDetail;
			}
			SetCustomTitleWithBack(Resource.Layout.Page_EC_CourseDetail, title);

			//more review
			var moreReview = FindViewById<LinearLayout>(Resource.Id.more_review);
			moreReview.Click += (sender, e) => {
				ViewModel.ShowReviewListCommand.Execute();
			};

			var webview = FindViewById<WebView>(Resource.Id.webview);
			webview.LoadUrl("file:///android_asset/Content/" + "FindMore/Talent gene detection.htm");
			var rating = FindViewById<RatingBar>(Resource.Id.user_recommendationindex);
			rating.Rating = 5;

			var reviewlist = FindViewById<ListView>(Resource.Id.review_list);
			var testlist = new List<ECReview>();
			testlist.Add(new ECReview());
			testlist.Add(new ECReview());
			var adapter = new CourseReviewAdapter(this, testlist);
			reviewlist.Adapter = adapter;
			SetListViewHeightBasedOnChildren(reviewlist);
		}

		void SetListViewHeightBasedOnChildren(ListView listView) {
			var listAdapter = listView.Adapter;
			if (listAdapter == null) {
				// pre-condition
				return;
			}

			int totalHeight = 0;
			for (int i = 0; i < listAdapter.Count; i++) {
				var listItem = listAdapter.GetView(i, null, listView);
				// listItem.measure(0, 0);
				listItem.Measure(
					View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
					View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
				totalHeight += (listItem.MeasuredHeight + 55);
			}


			ViewGroup.LayoutParams test = listView.LayoutParameters;
			test.Height = totalHeight
			+ (listView.DividerHeight * (listAdapter.Count - 1));
			listView.LayoutParameters = test;
		}
	}
}

