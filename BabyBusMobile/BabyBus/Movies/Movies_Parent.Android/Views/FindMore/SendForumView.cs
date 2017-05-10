using Android.App;
using Android.OS;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid
{

	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class SendForumView : ViewBase<SendForumViewModel>
	{

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
			SetCustomTitleWithBack(Resource.Layout.Page_EC_SendForum, "发帖");

		}

	}
}

