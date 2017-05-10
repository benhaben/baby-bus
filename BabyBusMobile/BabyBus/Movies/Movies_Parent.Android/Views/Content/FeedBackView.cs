
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Uri = Android.Net.Uri;
using BabyBus.Droid.Views.Communication;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Content
{
	/// <summary>
	/// FeedBack View
	/// </summary>
	[Activity(Theme = "@style/CustomTheme")]
	public class FeedBackView : ViewBase<FeedBackViewModel>
	{
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			SetCustomTitleWithBack(Resource.Layout.Page_Content_FeedBack, Resource.String.home_title_feedback);

			var text1 = FindViewById<TextView>(Resource.Id.feedback_phone);
			text1.Click += (sender, args) => {
				Uri uri = Uri.Parse("tel:" + text1.Text);
				var intent = new Intent(Intent.ActionCall, uri);
				StartActivity(intent);
			};

			var text2 = FindViewById<TextView>(Resource.Id.feedback_detail);
			text2.Click += (sender, args) => {
				var intent = new Intent(this, typeof(SendFeedbackView));
				StartActivity(intent);
			};

		}
	}
}