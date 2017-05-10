using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using CN.Jpush.Android.Api;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.JPush
{
	[BroadcastReceiver]
	[IntentFilter(new string[] {"cn.jpush.android.intent.REGISTRATION",
		"cn.jpush.android.intent.UNREGISTRATION",
		"cn.jpush.android.intent.MESSAGE_RECEIVED",
		"cn.jpush.android.intent.NOTIFICATION_RECEIVED",
		"cn.jpush.android.intent.NOTIFICATION_OPENED",
		"cn.jpush.android.intent.ACTION_RICHPUSH_CALLBACK"
	}, Categories = new string[] { "com.rnt.babybus.parent" })]
	public class JPushReceiver : BroadcastReceiver
	{
		private const String TAG = "MyReceiver";

		public override void OnReceive(Context context, Intent intent)
		{
			Bundle bundle = intent.Extras;
			Log.Debug(TAG, "onReceive - " + intent.Action + ", extras: " + printBundle(bundle));

			if (JPushInterface.ActionRegistrationId.Equals(intent.Action)) {
				String regId = bundle.GetString(JPushInterface.ActionRegistrationId); 
				Log.Debug(TAG, "接收Registration Id : " + regId);
				//send the Registration Id to your server...
			} else if (JPushInterface.ActionUnregister.Equals(intent.Action)) {
				String regId = bundle.GetString(JPushInterface.ActionRegistrationId);
				Log.Debug(TAG, "接收UnRegistration Id : " + regId);
				//send the UnRegistration Id to your server...
			} else if (JPushInterface.ActionMessageReceived.Equals(intent.Action)) {
				Log.Debug(TAG, "接收到推送下来的自定义消息: " + bundle.GetString(JPushInterface.ExtraMessage));
				processCustomMessage(context, bundle);

			} else if (JPushInterface.ActionNotificationReceived.Equals(intent.Action)) {
				Log.Debug(TAG, "接收到推送下来的通知");
				int notifactionId = bundle.GetInt(JPushInterface.ExtraNotificationId);
				Log.Debug(TAG, "接收到推送下来的通知的ID: " + notifactionId);

			} else if (JPushInterface.ActionNotificationOpened.Equals(intent.Action)) {
				Log.Debug(TAG, "用户点击打开了通知");

				//打开自定义的Activity
				var vm = new MainViewModel();
				Intent i = Mvx.Resolve<IMvxAndroidViewModelRequestTranslator>().GetIntentWithKeyFor(vm).Item1;
				i.PutExtras(bundle);
				i.AddFlags(ActivityFlags.ClearTop);
				i.AddFlags(ActivityFlags.SingleTop);
				context.StartActivity(i);
			} else if (JPushInterface.ActionRichpushCallback.Equals(intent.Action)) {
				Log.Debug(TAG, "用户收到到RICH PUSH CALLBACK: " + bundle.GetString(JPushInterface.ExtraExtra));
				//在这里根据 JPushInterface.EXTRA_EXTRA 的内容处理代码，比如打开新的Activity， 打开一个网页等..

			} else {
				Log.Debug(TAG, "Unhandled intent - " + intent.Action);
			}
		}

		// 打印所有的 intent extra 数据
		private static String printBundle(Bundle bundle)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var key in bundle.KeySet()) {
				if (key.Equals(JPushInterface.ExtraNotificationId)) {
					sb.Append("\nkey:" + key + ", value:" + bundle.GetInt(key));
				} else {
					sb.Append("\nkey:" + key + ", value:" + bundle.GetString(key));
				}
			}
			return sb.ToString();
		}

		//send msg to MainActivity
		private void processCustomMessage(Context context, Bundle bundle)
		{
			/*
             * 这一段是处理自定义消息，比如可以在消息里定义一个动作指令，接收后，做出相应的动作行为，就不继续翻译了。
            if (MainActivity.isForeground) {
                String message = bundle.GetString(JPushInterface.ExtraMessage);
                String extras = bundle.GetString(JPushInterface.ExtraExtra);
                Intent msgIntent = new Intent(MainActivity.MESSAGE_RECEIVED_ACTION);
                msgIntent.putExtra(MainActivity.KEY_MESSAGE, message);
                if (!ExampleUtil.isEmpty(extras)) {
                    try {
                        JSONObject extraJson = new JSONObject(extras);
                        if (null != extraJson && extraJson.length() > 0) {
                            msgIntent.putExtra(MainActivity.KEY_EXTRAS, extras);
                        }
                    } catch (JSONException e) {

                    }

                }
                context.sendBroadcast(msgIntent);
            }*/

		}
	}
}



