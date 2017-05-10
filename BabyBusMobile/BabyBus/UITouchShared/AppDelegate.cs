using Cirrious.CrossCore;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.ViewModels;
using Foundation;
using UIKit;

using JPushBinding;
using System.Diagnostics;
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using BigTed;
using UITouchShared;
using CoreGraphics;
using BabyBus.Logic.Shared;
using WeixinPayBinding.iOS;
using System.Runtime.Remoting.Messaging;

namespace BabyBus.iOS
{
   
	public class WXApiDelegateImplement: WXApiDelegate
	{
		private static WXApiDelegateImplement _instance;

		public static WXApiDelegateImplement GetWXApiDelegateInstance()
		{
			//do not support multi-thread 
			if (_instance == null) {
				_instance = new WXApiDelegateImplement();
			}
			return _instance;
		}

		public IWxApiDelegateAdapter IWxApiDelegate;

		public override void OnReq(BaseReq req)
		{
			Mvx.Trace(req.DebugDescription);
		}

		public override void OnResp(BaseResp resp)
		{
			if (IWxApiDelegate != null) {
				var sendAuthResp = (SendAuthResp)resp;
				if (sendAuthResp != null) {
					var respOfBabyBus = new SendAuthRespOfBabyBus();
					respOfBabyBus.Code = sendAuthResp.Code;
					respOfBabyBus.Country = sendAuthResp.Country;
					respOfBabyBus.ErrCode = sendAuthResp.ErrCode;
					respOfBabyBus.ErrStr = sendAuthResp.ErrStr;
					respOfBabyBus.Lang = sendAuthResp.Lang;
					respOfBabyBus.State = sendAuthResp.State;
					respOfBabyBus.Type = sendAuthResp.Type;
					IWxApiDelegate.OnResp(respOfBabyBus);
				}
			}
		}
	}

	[Register("AppDelegate")]
	public partial class AppDelegate : MvxApplicationDelegate
	{
		static UIWindow _window;

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{

			_window = new UIWindow(UIScreen.MainScreen.Bounds);

//						_window.RootViewController = new TestAppController ();

			var setup = new Setup(this, _window);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
			startup.Start();

			// make the window visible
			_window.MakeKeyAndVisible();

			// set Appearance
//            UITabBar.Appearance.SelectedImageTintColor = MvxTouchColor.Green;
			UIPageControl.Appearance.PageIndicatorTintColor = MvxTouchColor.Blue;
			UIPageControl.Appearance.CurrentPageIndicatorTintColor = MvxTouchColor.BrightGreen;
           
			//设置statusbar得前景色
			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

			var textAttr = new UITextAttributes();
			textAttr.TextColor = MvxTouchColor.White;
			UINavigationBar.Appearance.SetTitleTextAttributes(textAttr);
			UINavigationBar.Appearance.TintColor = MvxTouchColor.White;
			UINavigationBar.Appearance.SetBackgroundImage(UIImage.FromBundle("bar.png"), UIBarMetrics.Default);
			var shadow = new UIImage();
			UINavigationBar.Appearance.ShadowImage = shadow.ImageWithColor(UIColor.Clear, new CGSize(320, 1));
			//这个会导致bar透明
//            UITabBar.Appearance.BarTintColor = MvxTouchColor.White; 
			UITabBar.Appearance.BackgroundColor = MvxTouchColor.White;

//            Cell separators are table cells used to separate the table. The properties are set on the Table.
//            UITableView.Appearance.SeparatorColor = UIColor.FromPatternImage(UIImage.FromBundle("line-px2.png"));
//            var effect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Dark);
//            UITableView.Appearance.SeparatorEffect = UIVibrancyEffect.FromBlurEffect(effect);

			ProgressHUD.Shared.HudForegroundColor = UIColor.White;
			ProgressHUD.Shared.HudToastBackgroundColor = MvxTouchColor.Gray2;
			ProgressHUD.Shared.HudBackgroundColour = MvxTouchColor.Gray2;
			ProgressHUD.Shared.HudStatusShadowColor = MvxTouchColor.Gray2;


			APService.RegisterForRemoteNotificationTypes(
				UIRemoteNotificationType.None
				| UIRemoteNotificationType.Sound
				| UIRemoteNotificationType.Alert
				| UIRemoteNotificationType.Badge
                , null);

			//          [APService setupWithOption:launchOptions];
			APService.SetupWithOption(options);

//            if (options != null)
//            {
//                NSDictionary userInfo = options.ObjectForKey(UIApplication.LaunchOptionsRemoteNotificationKey) as NSDictionary;
//
//                    
//                if (userInfo != null)
//                {
//                    GetMessage(userInfo);
//                }
//            }
           

			WXApi.RegisterApp(BabyBus.Logic.Shared.Constants.APPID, "babybus weixin");
			return true;
		}

		public override bool HandleOpenURL(UIApplication application, NSUrl url)
		{
			return  WXApi.HandleOpenURL(url, WXApiDelegateImplement.GetWXApiDelegateInstance());
		}

		public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			return  WXApi.HandleOpenURL(url, WXApiDelegateImplement.GetWXApiDelegateInstance());
		}

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			return  WXApi.HandleOpenURL(url, WXApiDelegateImplement.GetWXApiDelegateInstance());
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
			//          throw new NotImplementedException ();
			Debug.WriteLine("FailedToRegisterForRemoteNotifications : " + error.ToString());


		}

		//        public override bool DidFinishLaunchingWithOptions(UIApplication app, NSDictionary launchOptions) {
		//            base.WillFinishLaunching(app, launchOptions);
		//            NSDictionary remoteNotification = launchOptions.ObjectForKey(UIApplicationLaunchOptionsRemoteNotificationKey);
		//
		//        }

		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
			//          throw new NotImplementedException ();
			Debug.WriteLine("DidRegisterUserNotificationSettings : " + notificationSettings.ToString());
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
			//          throw new NotImplementedException ();
			//          [APService registerDeviceToken:deviceToken];
			APService.RegisterDeviceToken(deviceToken);
		}

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
			//          throw new NotImplementedException ();
			Debug.WriteLine("ReceivedLocalNotification : " + notification.ToString());

		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
			//          throw new NotImplementedException ();

			GetMessage(userInfo);
			APService.HandleRemoteNotification(userInfo);
		}

		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
			//          throw new NotImplementedException ();
			//              [APService handleRemoteNotification:userInfo];
			// IOS 7 Support Required
			//            [APService handleRemoteNotification:userInfo];
			//            completionHandler(UIBackgroundFetchResultNewData);

			GetMessage(userInfo);

			APService.HandleRemoteNotification(userInfo);
			completionHandler(UIBackgroundFetchResult.NewData);
		}

		public override void WillEnterForeground(UIApplication application)
		{
			APService.ResetBadge();
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 1;
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		static void GetMessage(NSDictionary userInfo)
		{
			// 取得 APNs 标准信息内容
			var aps = userInfo.ValueForKey(new NSString("aps"));
			var content = aps.ValueForKey(new NSString("alert"));
			var badge = aps.ValueForKey(new NSString("badge"));
			var sound = aps.ValueForKey(new NSString("sound"));
			var noticeId = userInfo.ValueForKey(new NSString("NoticeId"));
			var tag = userInfo.ValueForKey(new NSString("Tag"));
			var questionId = userInfo.ValueForKey(new NSString("QuestionId"));
			var isHtml = userInfo.ValueForKey(new NSString("IsHtml"));

			try {
				var notificationMessage = new JPushNotificationMessage(null);
				notificationMessage.Tag = tag.ToString();
				if (notificationMessage.Tag == "Notice") {
					notificationMessage.Id = Convert.ToInt32(noticeId.ToString());
					notificationMessage.IsHtml = Convert.ToBoolean(isHtml.ToString());
				} else if (notificationMessage.Tag == "GrowMemory") {
				} else if (notificationMessage.Tag == "Question") {
					//服务器这里都用了noticeId
					notificationMessage.Id = Convert.ToInt32(questionId.ToString());
				} else {
				}
           
				var messenger = Mvx.Resolve<IMvxMessenger>();
				messenger.Publish<JPushNotificationMessage>(notificationMessage);
			} catch (Exception ex) {
				Xamarin.Insights.Report(ex, Xamarin.Insights.Severity.Critical);
			}
		}
	}
}
