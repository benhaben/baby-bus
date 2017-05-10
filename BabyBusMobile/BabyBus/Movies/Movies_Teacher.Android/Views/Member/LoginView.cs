using Android.App;
using Android.OS;
using Android.Widget;
using Uri = Android.Net.Uri;
using Android.Content;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Member
{
	[Activity(Label = "LoginView"
        , NoHistory = true, ClearTaskOnLaunch = true)]
	public class LoginView : ViewBase<LoginViewModel>
	{
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Create your application here
			SetContentView(Resource.Layout.LoginView);

			var call = FindViewById<TextView>(Resource.Id.user_call);
			call.Click += (sender, args) => {
				Uri uri = Uri.Parse("tel:4009922586");
				var intent = new Intent(Intent.ActionCall, uri);
				StartActivity(intent);
			};
		}

		protected override void OnStart() {
			base.OnStart();

			//Hide ActionBar
			ActionBar.Hide();
		}
	}
}