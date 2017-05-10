using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Views;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class SendReviewView : ViewBase<SendReviewViewModel>
	{

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
			SetCustomTitleWithBack(Resource.Layout.Page_EC_SendReview, "点评");
			var Rating = FindViewById<RatingBar>(Resource.Id.user_recommendationindex);
			var InfoTitle = FindViewById<TextView>(Resource.Id.title);
			var Sendbutton = FindViewById<Button>(Resource.Id.button1);

//			Rating.Click += (sender, e) => {
//				ViewModel.Rating = Rating.Rating;
//			};
			Sendbutton.Click += (sender, e) => {
				ViewModel.Rating = (int)Rating.Rating;
				ViewModel.SendCommand.Execute();	
			};

     


		}

	}
}

