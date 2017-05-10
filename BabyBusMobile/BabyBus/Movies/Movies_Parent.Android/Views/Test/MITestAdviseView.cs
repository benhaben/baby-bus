using Android.App;
using Android.OS;

namespace BabyBus.Droid.Views.Test
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]		
	public class MITestAdviseView : ActivityBase
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetCustomTitleWithBack(Resource.Layout.Page_Comm_Picture, "建议");
		}

	}

}

