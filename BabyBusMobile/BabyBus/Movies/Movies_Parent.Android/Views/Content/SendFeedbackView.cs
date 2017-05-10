
using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Communication
{
	/// <summary>
	/// Send FeedBack View, by Text and Phone
	/// </summary>
	[Activity(Label = "@string/home_title_feedback", Theme = "@style/CustomTheme",
		ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class SendFeedbackView : ViewBase<SendFeedbackViewModel>
	{
		private EditText content;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetCustomTitleWithBack(Resource.Layout.Page_Comm_SendFeedback, Resource.String.home_title_feedback);

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			content = FindViewById<EditText>(Resource.Id.question_content_text);
			content.Hint = "请简要描述您的问题和意见！";
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			NetContectStatus.unregisterReceiver(this);
		}
	}
}