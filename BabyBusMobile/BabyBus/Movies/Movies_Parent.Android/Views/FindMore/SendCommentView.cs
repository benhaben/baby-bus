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
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class SendCommentView : ViewBase<SendCommentViewModel>
	{

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
			SetCustomTitleWithBack(Resource.Layout.Page_EC_SendComment, "评价");

		}

	}
}

