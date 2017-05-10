using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using JPushBinding;

namespace TestJPush2
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			// If you have defined a root view controller, set it here:
			window.RootViewController =  new MyRootView();
			
			// make the window visible
			window.MakeKeyAndVisible ();

			APService.RegisterForRemoteNotificationTypes (
				UIRemoteNotificationType.None
				| UIRemoteNotificationType.Sound
				| UIRemoteNotificationType.Alert
				, null);

//			[APService setupWithOption:launchOptions];
			APService.SetupWithOption (options);
			return true;
		}
		//		didRegisterForRemoteNotificationsWithDeviceToken

		public override void FailedToRegisterForRemoteNotifications (UIApplication application, NSError error)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
//			throw new NotImplementedException ();
			Console.WriteLine ("FailedToRegisterForRemoteNotifications : " + error.ToString());


		}

		public override void DidRegisterUserNotificationSettings (UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
//			throw new NotImplementedException ();
			Console.WriteLine ("DidRegisterUserNotificationSettings : " + notificationSettings.ToString());
		}

		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
//			throw new NotImplementedException ();
//			[APService registerDeviceToken:deviceToken];
			APService.RegisterDeviceToken (deviceToken);
		}

		public override void ReceivedLocalNotification (UIApplication application, UILocalNotification notification)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
//			throw new NotImplementedException ();
			Console.WriteLine ("ReceivedLocalNotification : " + notification.ToString());

		}

		public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
//			throw new NotImplementedException ();
			APService.HandleRemoteNotification (userInfo);

		}

		public override void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
//			throw new NotImplementedException ();
//			    [APService handleRemoteNotification:userInfo];
			// IOS 7 Support Required
//			  [APService handleRemoteNotification:userInfo];
//			  completionHandler(UIBackgroundFetchResultNewData);

			APService.HandleRemoteNotification (userInfo);
			completionHandler (UIBackgroundFetchResult.NewData);

		}
	}
}

