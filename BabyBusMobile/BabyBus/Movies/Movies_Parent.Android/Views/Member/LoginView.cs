using Android.App;
using Android.OS;
using Android.Widget;
using Uri = Android.Net.Uri;
using Android.Content;
using BabyBus.Logic.Shared;
using Com.Tencent.MM.Sdk.Openapi;
using Com.Tencent.MM.Sdk.Modelmsg;
using Android.Content.PM;


namespace BabyBus.Droid.Views.Member
{
	[Activity(Label = "LoginView", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
	public class LoginView : ViewBase<LoginViewModel>
	{
		private readonly IWXAPI msgApi;

		public LoginView()
		{
			this.msgApi = WXAPIFactory.CreateWXAPI(this, WXConfig.APPID, true);
		}

		private bool registerApp()//微信注册，注册后再微信的APP里面就可以看到第三方APP的图的图标
		{
			return msgApi.RegisterApp(WXConfig.APPID);
		}
		// send oauth request
		private bool SendAuthReq()//想微信发送请求
		{

			var req = new SendAuth.Req();  
			req.Scope = "snsapi_userinfo";  
			req.State = "wechat_sdk_demo_test";  
			return msgApi.SendReq(req);
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here

			SetContentView(Resource.Layout.LoginView);


//			var call = FindViewById<TextView>(Resource.Id.user_call);
//			call.Click += (sender, args) => {
//				Uri uri = Uri.Parse("tel:4009922586");
//				var intent = new Intent(Intent.ActionCall, uri);
//				StartActivity(intent);
//			};

			var WxLoginButton = FindViewById<ImageView>(Resource.Id.image_wx_button);
			WxLoginButton.Click += (sender, e) => SendAuthReq();

			//            ViewModel.ReCheckout += (sender, s) => this.ShowConfirm("", s, () => ViewModel.GotoRegisterDetailCommand.Execute());
		}

		protected override void OnStart()
		{
			base.OnStart();

			//Hide ActionBar
			ActionBar.Hide();
		}
	}
}