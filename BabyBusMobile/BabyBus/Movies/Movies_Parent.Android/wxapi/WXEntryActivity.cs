using System;
using Android.App;
using Android.Content;
using Android.OS;
using Com.Tencent.MM.Sdk.Openapi;
using Com.Tencent.MM.Sdk.Modelbase;
using Com.Tencent.MM.Sdk.Modelmsg;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using BabyBusSSApi.ServiceModel.DTO.Reponse;
using BabyBusSSApi.ServiceModel.Enumeration;
using BabyBus.Droid.Views.Main;
using System.Threading.Tasks;
using Java.Lang;
using Android.Content.PM;
using Com.Squareup.Picasso;
using BabyBus.Droid.Fragments;

namespace BabyBus.Droid.wxapi
{
	[Activity(Label = "WXEntryActivity", Exported = true, Name = "com.rnt.babybus.parent.wxapi.WXEntryActivity",
		Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]	
	public class WXEntryActivity : Activity,IWXAPIEventHandler
	{
		private  IWXAPI msgApi;
		private readonly IRemoteService _remoteService;
		private readonly Handler mhandler;

		public WXEntryActivity()
		{
			_remoteService = Mvx.Resolve<IRemoteService>();
			mhandler = new Handler();
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Page_WXEntryActivity);
			msgApi = WXAPIFactory.CreateWXAPI(this, WXConfig.APPID, false); 
			var b = msgApi.HandleIntent(Intent, this);
		}

		protected override void OnNewIntent(Intent intent)
		{
			base.OnNewIntent(intent);
			Intent = intent;
			var b = msgApi.HandleIntent(intent, this);
		}

		public  void  OnReq(BaseReq p0)
		{  
			// TODO Auto-generated method stub  
			Finish();
		}

		public  void OnResp(BaseResp resp)
		{   
			switch (resp.WXErrCode) {  
				case BaseResp.ErrCode.ErrOk: 
					var bundle = new Bundle();
					resp.ToBundle(bundle);
					var s1 = new SendAuth.Resp(bundle);
					System.String code = s1.Code; 
					Task task = Task.Factory.StartNew(() => {
						SenCodetoWeixin(code);
					});

					break; 
				default:  
					break;  
			} 
			mhandler.PostDelayed(new Runnable(Finish), 3000);   
			
		}

		private void SenCodetoWeixin(string code)
		{
			if (string.IsNullOrEmpty(code)) {
				
				return;
			}
			try {
				var authResponse = _remoteService.LoginWithWechat(code).Result;
				StoreAuthResponse(authResponse);
			} catch (BabyBusWebServiceException ex) {
				
			}
		}

		void StoreAuthResponse(AuthenticateResponse authResponse)
		{
			var user = new UserModel();
			if (authResponse.Meta.ContainsKey("UserId"))
				user.UserId = Convert.ToInt64(authResponse.Meta["UserId"]);
			if (authResponse.Meta.ContainsKey("ChildId"))
				user.ChildId = Convert.ToInt64(authResponse.Meta["ChildId"]);
			if (authResponse.Meta.ContainsKey("KindergartenId"))
				user.KindergartenId = Convert.ToInt64(authResponse.Meta["KindergartenId"]);
			if (authResponse.Meta.ContainsKey("ClassId"))
				user.ClassId = Convert.ToInt64(authResponse.Meta["ClassId"]);
			if (authResponse.Meta.ContainsKey("RoleType"))
				user.RoleType = (RoleType)Convert.ToInt64(authResponse.Meta["RoleType"]);
			if (authResponse.Meta.ContainsKey("LoginName"))
				user.LoginName = Convert.ToString(authResponse.Meta["LoginName"]);
			if (authResponse.Meta.ContainsKey("RealName"))
				user.RealName = Convert.ToString(authResponse.Meta["RealName"]);
			if (authResponse.Meta.ContainsKey("HeadImage"))
				user.ImageName = Convert.ToString(authResponse.Meta["HeadImage"]);
			user.Kindergarten = new KindergartenModel();
			user.Kindergarten.KindergartenId = user.KindergartenId;
			if (authResponse.Meta.ContainsKey("KindergartenName")) {
				user.Kindergarten.KindergartenName = authResponse.Meta["KindergartenName"];
			} else {
				user.Kindergarten.KindergartenName = Constants.Anonymous;
			}
			user.Class = new KindergartenClassModel();
			user.Class.ClassId = user.ClassId;
			user.Class.KindergartenId = user.KindergartenId;
			if (authResponse.Meta.ContainsKey("ClassName")) {
				user.Class.ClassName = authResponse.Meta["ClassName"];
			} else {
				user.Class.ClassName = Constants.Anonymous;
			}
			user.Child = new ChildModel();
			user.Child.ClassId = user.ClassId;
			user.Child.KindergartenId = user.KindergartenId;
			if (authResponse.Meta.ContainsKey("ChildName")) {
				user.Child.ChildName = authResponse.Meta["ChildName"];
			} else {
				user.Child.ChildName = Constants.Anonymous;
			}
			if (authResponse.Meta.ContainsKey("ChildId"))
				user.Child.ChildId = Convert.ToInt64(authResponse.Meta["ChildId"]);
			if (authResponse.Meta.ContainsKey("Birthday"))
				user.Child.Birthday = Convert.ToDateTime(authResponse.Meta["Birthday"]);
			if (authResponse.Meta.ContainsKey("Gender"))
				user.Child.Gender = Convert.ToInt32(authResponse.Meta["Gender"]);
			if (authResponse.Meta.ContainsKey("HeadImage")) {
				user.Child.ImageName = Convert.ToString(authResponse.Meta["HeadImage"]);
				Task task = Task.Factory.StartNew(() => {
					var targt = new PicassoTarget(this, user.Child.ImageName, user.Child.Image);
					Picasso.With(this).Load(Constants.ThumbServerPath + user.Child.ImageName).Into(targt);
				});
			}
				
			user.Cookie = _remoteService.CookiesJsonString;
			//store in local DB
			BabyBusContext.UserAllInfo = user;
			Intent mintent = new Intent(this, typeof(MainView));
			StartActivity(mintent);
		}

	}
}

