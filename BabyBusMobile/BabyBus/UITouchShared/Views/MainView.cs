using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using Foundation;
using JPushBinding;
using ObjCRuntime;
using UIKit;
using Xamarin;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Touch.Platform;
using UITouchShared;
using Cirrious.MvvmCross.Plugins.Messenger;
using System;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using Utilities.Touch;
using RestSharp;
using BabyBusSSApi.ServiceModel.DTO.Reponse;
using System.Net;

namespace BabyBus.iOS
{
	public sealed class MainView : MvxTabBarViewController
	{
		public MainView()
		{
			this.Request = new Cirrious.MvvmCross.ViewModels.MvxViewModelRequest(
				typeof(MainViewModel), null, null, null
			);

			var tags = new List<string>();
			tags.Add(string.Format("Kindergarten_{0}", BabyBusContext.KindergartenId));
			tags.Add(string.Format("Class_{0}", BabyBusContext.ClassId));
			NSSet set = new NSSet(tags.ToArray());
			APService.SetTagsAndAlias(set, "User_" + BabyBusContext.UserAllInfo.UserId, new Selector("tagsAliasCallback:tags:alias:"), this);

			//Insights User
			var infos = new Dictionary<string, string> {
				{ "Name", BabyBusContext.UserAllInfo.RealName },
				{ "Phone", BabyBusContext.UserAllInfo.LoginName }
			};
			var unique = BabyBusContext.Kindergarten.KindergartenName + "_"
			             + BabyBusContext.Class.ClassName + "_" + BabyBusContext.UserAllInfo.RealName;
			Insights.Identify(unique, infos);

			// need this additional call to ViewDidLoad because UIkit creates the view before the C# hierarchy has been constructed
			ViewDidLoad();

			UpdateStatus();
			Reachability.ReachabilityChanged += ReachabilityChanged;

			RegisterNotifications();
			var messenger = Mvx.Resolve<IMvxMessenger>();
			messenger.Subscribe<JPushNotificationMessage>(HandleNoticeMessage, MvxReference.Strong);
			messenger.Subscribe<RedirectMessage>(HandleRedirectMessage, MvxReference.Strong);

			//Init Html Auth 
			InitHtmlAuth();

		}

		static void InitHtmlAuth()
		{
			var iServiceClient = new RestClient("http://115.28.2.41:8888/");
			var request = new RestRequest("/authenticate/credentials", Method.GET);
			request.RequestFormat = DataFormat.Json;
			request.AddQueryParameter("Username", "18502992708");
			request.AddQueryParameter("Password", "123456");
			request.AddQueryParameter("Remeberme", "1");
			var authResponse = iServiceClient.Get<AuthenticateResponse>(request);
			if (authResponse != null && authResponse.Cookies.Count > 0) {
				//set cookies
				List<NSHttpCookie> cookies = new List<NSHttpCookie>();
				foreach (var sessionCookie in authResponse.Cookies) {
					var cookie = new Cookie(sessionCookie.Name, sessionCookie.Value, sessionCookie.Path, sessionCookie.Domain);
					//load url from remote service
					NSHttpCookieStorage.SharedStorage.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;
					var cookieDict = new NSMutableDictionary();
					cookieDict.Add(NSHttpCookie.KeyName, new NSString(sessionCookie.Name));
					cookieDict.Add(NSHttpCookie.KeyValue, new NSString(sessionCookie.Value));
					cookieDict.Add(NSHttpCookie.KeyPath, new NSString(sessionCookie.Path));
					cookieDict.Add(NSHttpCookie.KeyDomain, new NSString(sessionCookie.Domain));
					cookies.Add(new NSHttpCookie(cookieDict));
				}
				NSHttpCookieStorage.SharedStorage.SetCookies(cookies.ToArray(), new NSUrl("http://115.28.2.41:8888/"), new NSUrl("http://115.28.2.41:8888/"));
			}
		}

		void HandleRedirectMessage(RedirectMessage msg)
		{
			if (MainViewModel != null) {
				if (BabyBusContext.RoleType == RoleType.Teacher) {
					if (msg.PageTag.ToLower() == "GrowMemory".ToLower()) {
						this.SelectedIndex = 1;
					} else if (msg.PageTag.ToLower() == "Notice".ToLower()) {
						this.SelectedIndex = 2;
					} else if (msg.PageTag.ToLower() == "Question".ToLower()) {
						this.SelectedIndex = 3;
					}
				} else if (BabyBusContext.RoleType == RoleType.Parent) {
					if (msg.PageTag.ToLower() == "GrowMemory".ToLower()) {
						this.SelectedIndex = 1;
					} else if (msg.PageTag.ToLower() == "Notice".ToLower()) {
						this.SelectedIndex = 2;
					} else if (msg.PageTag.ToLower() == "Question".ToLower()) {
						this.SelectedIndex = 3;
					}
				} else if (BabyBusContext.RoleType == RoleType.HeadMaster) {
					if (msg.PageTag.ToLower() == "GrowMemory".ToLower()) {
						this.SelectedIndex = 0;
					} else if (msg.PageTag.ToLower() == "Notice".ToLower()) {
						this.SelectedIndex = 1;
					} else if (msg.PageTag.ToLower() == "Question".ToLower()) {
						this.SelectedIndex = 2;
					}
				}
			}
		}

		void HandleNoticeMessage(JPushNotificationMessage msg)
		{
			if (MainViewModel != null) {
				if (UIApplication.SharedApplication.ApplicationState != UIApplicationState.Active) {
					if (msg.Tag.ToLower() == "Notice".ToLower()) {
						MainViewModel.ShowNoticeDetailViewModel(msg.Id, msg.IsHtml);
					} else if (msg.Tag.ToLower() == "GrowMemory".ToLower()) {
						MainViewModel.ShowMemoryIndexViewModel();
					} else if (msg.Tag.ToLower() == "Question".ToLower()) {
						MainViewModel.ShowQuestionDetailCommand(msg.Id);
					} else if (msg.Tag.ToLower() == "Course".ToLower()) {
						MainViewModel.ShowCourseDetailCommand(msg.Id);
					} else if (msg.Tag.ToLower() == "Forum".ToLower()) {
						MainViewModel.ShowForumDetailCommand(msg.Id);
					} else {
						
					}
				} else {
					if (msg.Tag.ToLower() == "Notice".ToLower() && this.VCCommunication != null) {
						VCCommunication.TabBarItem.SetBadge();
					} else if (msg.Tag.ToLower() == "GrowMemory".ToLower() && this.VCGrown != null) {
						VCGrown.TabBarItem.SetBadge();
					} else if (msg.Tag.ToLower() == "Question".ToLower() && this.VCQuestion != null) {
						VCQuestion.TabBarItem.SetBadge();
					} else {
					}
				}
			}

		}

		void NoticeNotification(NSNotification notification)
		{
			if (MainViewModel != null) {
				Mvx.Trace("JPushNotificationMessagennn success");
				var dict = (NSDictionary)notification.Object;
				var noticeIdStr = dict.ValueForKey(new NSString("noticeId")).ToString();
				var isHtmlStr = dict.ValueForKey(new NSString("IsHtml"));
				var noticeId = Convert.ToInt32(noticeIdStr);
				var isHtml = Convert.ToBoolean(isHtmlStr);
				MainViewModel.ShowNoticeDetailViewModel(noticeId, isHtml);
			}
		}

		List<NSObject> _eventListeners;

		void RegisterNotifications()
		{
			if (_eventListeners == null) {
				_eventListeners = new List<NSObject>();
			}
			// Listen for photo loading notifications
			_eventListeners.Add(NSNotificationCenter.DefaultCenter
                .AddObserver(new NSString("BABYBUS_NOTICE_NOTIFICATION"), NoticeNotification));
		}

		void UnRegisterNotifications()
		{
			if (_eventListeners != null) {
				NSNotificationCenter.DefaultCenter.RemoveObservers(_eventListeners);
				_eventListeners.Clear();
				_eventListeners = null;
			}
		}

		void UpdateStatus()
		{
			remoteHostStatus = Reachability.RemoteHostStatus();
			internetStatus = Reachability.InternetConnectionStatus();
			localWifiStatus = Reachability.LocalWifiConnectionStatus();
		}

		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;

		void ReachabilityChanged(object o, System.EventArgs e)
		{
			UpdateStatus();
			const string noNet = "(未连接)";
			if (remoteHostStatus == NetworkStatus.NotReachable
			    && internetStatus == NetworkStatus.NotReachable
			    && localWifiStatus == NetworkStatus.NotReachable) {
				foreach (var vc in ViewControllers) {
					var nav = vc as UINavigationController;
					if (nav != null) {
						var label = nav.TopViewController.NavigationItem.TitleView as UILabel;
						if (label != null && !label.Text.Contains(noNet)) {
							label.Text = label.Text + noNet;
							label.Frame = new CoreGraphics.CGRect(label.Frame.X, label.Frame.Y, label.Frame.Width * 2, label.Frame.Height);
        
						}
					}
				}
			} else {
				foreach (var vc in ViewControllers) {
					var nav = vc as UINavigationController;
					if (nav != null) {
						var label = nav.TopViewController.NavigationItem.TitleView as UILabel;
						if (label != null && !label.Text.IsEmpty()) {
							var start = label.Text.IndexOf(noNet, new System.StringComparison());
							if (start >= 0 && start < label.Text.Length) {
								label.Text = label.Text.Substring(0, start);
								label.Frame = new CoreGraphics.CGRect(label.Frame.X, label.Frame.Y, label.Frame.Width / 2, label.Frame.Height);
							}
						}
					}
				}
			}
		}

		// Extensions.cs
		//        public partial class Button {
		//            public void SetTarget(System.Action callback) {
		//                SetTarget(new ActionDispatcher(callback), ActionDispatcher.Selector);
		//            }
		//        }
		[Export("tagsAliasCallback:tags:alias:")]
		void TagsAliasCallback(int iResCode, NSSet tags, NSString alias)
		{
			Mvx.Trace(string.Format("iResCode is {0}, alias is {1}, tags count is {2}", iResCode, alias, tags.Count));
		}

		public MainViewModel MainViewModel
		{ get { return base.ViewModel as MainViewModel; } }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			if (this.NavigationController != null)
				this.NavigationController.SetNavigationBarHidden(true, false);

			//Enter Log
			var iMvxTouchSystem = Mvx.Resolve<IMvxTouchSystem>();
			string build = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
			string version = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
			MainViewModel.VerCode = 1;
			MainViewModel.VerName = version + "." + build;
			MainViewModel.PhoneMode = string.Format("{0}.{1}", iMvxTouchSystem.Version.Major, iMvxTouchSystem.Version.Minor); 
			MainViewModel.OSType = "iOS";
			MainViewModel.EnterLog();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			if (this.NavigationController != null)
				this.NavigationController.SetNavigationBarHidden(false, false);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			if (ViewModel == null)
				return;
			#if __TEACHER__
			var viewControllers = new UIViewController[] {
				CreateTabFor("首页", "home", MainViewModel.TeacherHomeViewModel),
				CreateTabFor("成长记忆", "grow", MainViewModel.MemoryIndexViewModel),
				CreateTabFor("信息通知", "homework", MainViewModel.NoticeIndexViewModel),
				CreateTabFor("家园共育", "communication", MainViewModel.QuestionIndexViewModel),
				CreateTabFor("我", "me", MainViewModel.SettingViewModel)
			};
			VCHome = viewControllers[0];
			VCGrown = viewControllers[1];
			VCCommunication = viewControllers[2];
			VCQuestion = viewControllers[3];
			#elif __PARENT__
			var viewControllers = new UIViewController[] {
//				CreateTabFor("首页", "home", MainViewModel.ParentHomeViewModel),
				CreateTabFor("体智", "test", MainViewModel.TestHomeViewModel),
				CreateTabFor("发现", "findmore", MainViewModel.FindMoreIndeViewModel),
				CreateTabFor("校讯", "school", MainViewModel.ParentSchoolOnlineViewModel),
				CreateTabFor("我", "me", MainViewModel.SettingMainViewModel)
			};
			VCHome = viewControllers[0];
			VCGrown = viewControllers[1];
			VCCommunication = viewControllers[2];
			VCQuestion = viewControllers[3];

			#elif __MASTER__
			var viewControllers = new UIViewController[] {
				CreateTabFor("首页", "home", MainViewModel.MasterHomeViewModel),
				CreateTabFor("信息通知", "homework", MainViewModel.NoticeIndexViewModel),
				CreateTabFor("家园共育", "communication", MainViewModel.QuestionIndexViewModel),
				CreateTabFor("我的信息", "me", MainViewModel.SettingViewModel)
			};
			VCHome = viewControllers[0];
			VCCommunication = viewControllers[1];

			#endif
			ViewControllers = viewControllers;
			CustomizableViewControllers = new UIViewController[] { };
			SelectedViewController = ViewControllers[0];

			if (BabyBusContext.RoleType == RoleType.HeadMaster) {
				UITabBar.Appearance.SelectedImageTintColor = MvxTouchColor.LightRed;
//                UITabBar.Appearance.BarTintColor = MvxTouchColor.LightRed;
			} else if (BabyBusContext.RoleType == RoleType.Parent) {
				UITabBar.Appearance.SelectedImageTintColor = MvxTouchColor.BrightGreen;
//                UITabBar.Appearance.BarTintColor = MvxTouchColor.BrightGreen;
			} else if (BabyBusContext.RoleType == RoleType.Teacher) {
				UITabBar.Appearance.SelectedImageTintColor = MvxTouchColor.Blue;
//                UITabBar.Appearance.SelectionIndicatorImage = UIImage.FromBundle("meSelected.png").ImageByScalingToMaxSize(EasyLayout.HomePageNoticeTabBarHeightAndWidth, 2);
			}
		}

		public UIViewController VCHome {
			get;
			set;
		}

		public UIViewController VCGrown {
			get;
			set;
		}

		public UIViewController VCCommunication {
			get;
			set;
		}

		public UIViewController VCQuestion {
			get;
			set;
		}

		private int _createdSoFarCount = 0;

		private UIViewController CreateTabFor(string title, string imageName, IMvxViewModel viewModel)
		{
			var controller = new UINavigationController();
			var screen = this.CreateViewControllerFor(viewModel) as UIViewController;
			SetTitleAndTabBarItem(screen, title, imageName);
			controller.PushViewController(screen, false);
			return controller;
		}

		private void SetTitleAndTabBarItem(UIViewController screen, string title, string imageName)
		{
			screen.Title = title;
			#if __PARENT__
			var image = UIImage.FromBundle("images/school_home_view/" + imageName + "-1.png");
			if (image != null) {
				image = image.ImageByScalingToMaxSize(EasyLayout.HomePageNoticeTabBarHeightAndWidth, 2);
			}

			var imageSelected = UIImage.FromBundle("images/school_home_view/" + imageName + "-1.png");
			if (imageSelected != null) {
				imageSelected = imageSelected.ImageByScalingToMaxSize(EasyLayout.HomePageNoticeTabBarHeightAndWidth, 2);
			}
			#else
			var image = UIImage.FromBundle(imageName + ".png");
			if (image != null) {
				image = image.ImageByScalingToMaxSize(EasyLayout.HomePageNoticeTabBarHeightAndWidth, 2);
			}

			var imageSelected = UIImage.FromBundle(imageName + "Selected.png");
			if (imageSelected != null) {
				imageSelected = imageSelected.ImageByScalingToMaxSize(EasyLayout.HomePageNoticeTabBarHeightAndWidth, 2);
			}
			#endif

			var barItem = new UITabBarItem(title, image, imageSelected);

			UITextAttributes attributes = new UITextAttributes();
			attributes.Font = UIFont.SystemFontOfSize(10);
			barItem.SetTitleTextAttributes(attributes, UIControlState.Normal);

			UITextAttributes attributes1 = new UITextAttributes();
			attributes.Font = UIFont.BoldSystemFontOfSize(11);
			barItem.SetTitleTextAttributes(attributes1, UIControlState.Selected);

			screen.TabBarItem = barItem;
			_createdSoFarCount++;
		}
	}
}