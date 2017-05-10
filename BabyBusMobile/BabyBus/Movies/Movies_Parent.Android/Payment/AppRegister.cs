using System;
using Android.Content;
using Com.Tencent.MM.Sdk.Openapi;

namespace BabyBus.Droid.Payment
{
	public class AppRegister : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent) {
			IWXAPI api = WXAPIFactory.CreateWXAPI(context, null);

			api.RegisterApp(WXConfig.APPID);
		}
	}
}

