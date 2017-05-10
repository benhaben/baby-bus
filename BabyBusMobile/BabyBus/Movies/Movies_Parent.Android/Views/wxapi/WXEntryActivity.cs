
using System;
using Android.App;
using Android.OS;
using Com.Tencent.MM.Sdk.Openapi;
using Com.Tencent.MM.Sdk.Modelbase;
using Com.Tencent.MM.Sdk.Modelmsg;

namespace BabyBus.Droid.wxapi
{
	[Activity]			
	public class WXEntryActivity : Activity,IWXAPIEventHandler
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.LoginView);
		}

		public void  OnReq(BaseReq p0)
		{  
			// TODO Auto-generated method stub  

		}

		public void OnResp(BaseResp resp)
		{   
			switch (resp.WXErrCode) {  
				case BaseResp.ErrCode.ErrOk: 
					String code = ((SendAuth.Resp)resp).Code;  
					break; 
				default:  
					break;  
			} 
		}
	}
}

