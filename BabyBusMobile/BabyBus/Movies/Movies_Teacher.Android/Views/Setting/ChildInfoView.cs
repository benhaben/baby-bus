using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Uri = Android.Net.Uri;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Setting
{
	[Activity(Label = "幼儿信息")]
	public class ChildInfoView : ViewBase<ChildInfoViewModel>
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
			SetContentView(Resource.Layout.Page_Setting_ChildInfo);

			var phone = FindViewById<TextView>(Resource.Id.setting_info_phone);
			phone.Click += (sender, args) => {
				Uri uri = Uri.Parse("tel:" + phone.Text);
				var intent = new Intent(Intent.ActionCall, uri);
				StartActivity(intent);
			};
		}
	}
}