using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using CrossUI.Touch.Dialog.Elements;
using Foundation;
using UIKit;
using BigTed;
using CoreGraphics;
using System.Collections.Generic;
using JPushBinding;
using ObjCRuntime;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.iOS
{
    /// <summary>
    /// setting for app
    /// </summary>
    public sealed class SettingView : MvxBabybusDialogViewController
    {

        public SettingView()
            : base(UITableViewStyle.Plain,
                   null,
                   false)
        {
            var label = new UILabel(new CGRect(0, 0, 40, 35));
            label.Text = "我";
            label.TextAlignment = UITextAlignment.Center;
            this.NavigationItem.TitleView = label;
            label.TextColor = MvxTouchColor.White;

        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            //判断并接收返回的参数
            base.ViewWillAppear(animated);
            //want to reload data here 1. return from infoview 2.discard view viewmodel life flow
            _baseViewModel.LoadData();

            //TODO: picture can not show because of element can not get current cell, workaround here
            NSTimer.CreateScheduledTimer(new TimeSpan(3), (t) =>
                {
                    _baseViewModel.RaiseAllPropertiesChanged();
                    if (t != null)
                    {
                        t.Invalidate();
                    }
                });
        }



        SettingViewModel _baseViewModel = null;

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();

            _baseViewModel = this.ViewModel as SettingViewModel;
            var bindings = this.CreateInlineBindingTarget<SettingViewModel>();

            Section section = new Section("");
            var nameCardPhoto = new NameCardPhotoElement()
                .Bind(bindings, e => e.Name, vm => vm.KindergartenName)
                .Bind(bindings, e => e.DescriptionOfChild, vm => vm.ClassNameAndLoginName)
                .Bind(bindings, e => e.ImageUri, vm => vm.ImageName, "StringToUriThumb");
            section.Add(nameCardPhoto);

            #if __TEACHER__
            var children = new StyledStringElement("查看本班幼儿列表", "", UITableViewCellStyle.Value1);
            children.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            children.Tapped += () =>
            {
                _baseViewModel.ShowChildrenCommand.Execute();
            };
            children.Image = UIImage.FromBundle("");
            children.Font = EasyLayout.TitleFont;
            children.SubtitleFont = EasyLayout.SubTitleFont;
            children.TextColor = MvxTouchColor.Black1;
            children.DetailColor = MvxTouchColor.Black1;
            children.Image = UIImage.FromBundle("images/setting_view/attendance.png");

            section.Add(children);

            #endif

            #if __PARENT__
            var childrenAttendance = new StyledStringElement("查看孩子考勤记录", "", UITableViewCellStyle.Value1);
            childrenAttendance.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            childrenAttendance.Tapped += () =>
            {
                _baseViewModel.ShowAttendanceCommand.Execute();
            };
            childrenAttendance.Font = EasyLayout.TitleFont;
            childrenAttendance.SubtitleFont = EasyLayout.SubTitleFont;
            childrenAttendance.TextColor = MvxTouchColor.Black1;
            childrenAttendance.DetailColor = MvxTouchColor.Black1;
            childrenAttendance.Image = UIImage.FromBundle("images/setting_view/attendance.png");

            section.Add(childrenAttendance);
            #endif

            var clearCache = new StyledStringElement("清除缓存", "", UITableViewCellStyle.Value1);
            clearCache.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            clearCache.Tapped += () =>
            { 
                _baseViewModel.ClearCacheCommand.Execute();
                SDWebImage.SDImageCache.SharedImageCache.ClearMemory();
                SDWebImage.SDImageCache.SharedImageCache.ClearDisk();
                SDWebImage.SDImageCache.SharedImageCache.CleanDisk();
                BTProgressHUD.ShowSuccessWithStatus("已经清空图片", BabyBus.Logic.Shared.Constants.RefreshTime);
            };
            clearCache.Font = EasyLayout.TitleFont;
            clearCache.SubtitleFont = EasyLayout.SubTitleFont;
            clearCache.TextColor = MvxTouchColor.Black1;
            clearCache.DetailColor = MvxTouchColor.Black1;
            clearCache.Image = UIImage.FromBundle("images/setting_view/clear.png");

//            NSObject ver = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
//            string version = ver.ToString();
//            var checkVersion = new StyledStringElement("当前版本", version, UITableViewCellStyle.Value1);
//            checkVersion.Accessory = UITableViewCellAccessory.DisclosureIndicator;
//            checkVersion.Tapped += () => {
//                //check version at first
////                https://itunes.apple.com/lookup?id=956099526
//                if (BabyBusContext.UserAllInfo.RoleType == RoleType.Teacher) {
//                    UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/cn/app/bei-bei-ba-shi-lao-shi-ban/id956099526?l=en&mt=8"));
//                } else if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
//                    UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/cn/app/bei-bei-ba-shi-jia-zhang-ban/id955666963?l=en&mt=8"));
//                } else {
//                    UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/cn/app/bei-bei-ba-shi-lao-shi-ban/id956099526?l=en&mt=8"));
//                }
//            };

            var feedback = new StyledStringElement("意见反馈", "", UITableViewCellStyle.Value1);
            feedback.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            feedback.Tapped += () =>
            {
                _baseViewModel.ShowFeedbackViewCommand.Execute();
            };
            feedback.Font = EasyLayout.TitleFont;
            feedback.SubtitleFont = EasyLayout.SubTitleFont;
            feedback.TextColor = MvxTouchColor.Black1;
            feedback.DetailColor = MvxTouchColor.Black1;
            feedback.Image = UIImage.FromBundle("images/setting_view/feedback.png");


            var payment = new StyledStringElement("增值服务", "", UITableViewCellStyle.Value1);
            payment.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            payment.Tapped += () =>
            {
                _baseViewModel.ShowPaymentViewCommand.Execute();
            };
            payment.Font = EasyLayout.TitleFont;
            payment.SubtitleFont = EasyLayout.SubTitleFont;
            payment.TextColor = MvxTouchColor.Black1;
            payment.DetailColor = MvxTouchColor.Black1;
            payment.Image = UIImage.FromBundle("images/setting_view/sev-1.png");
//            var about = new StyledStringElement("关于贝贝巴士", "", UITableViewCellStyle.Default);
//            about.Accessory = UITableViewCellAccessory.DisclosureIndicator;
//            about.Tapped += () => {
//                if (BabyBusContext.UserAllInfo.RoleType == RoleType.Teacher) {
//                    UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/cn/app/bei-bei-ba-shi-lao-shi-ban/id956099526?l=en&mt=8"));
//                } else if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
//                    UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/cn/app/bei-bei-ba-shi-jia-zhang-ban/id955666963?l=en&mt=8"));
//                } else {
//                    UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/cn/app/bei-bei-ba-shi-lao-shi-ban/id956099526?l=en&mt=8"));
//                }
//            };

            var logout = new StyledStringElement("退出登录", "", UITableViewCellStyle.Value1)
				.Bind(bindings, e => e.SelectedCommand, vm => vm.LogoutCommand);
            logout.Font = EasyLayout.TitleFont;
            logout.SubtitleFont = EasyLayout.SubTitleFont;
            logout.TextColor = MvxTouchColor.Black1;
            logout.DetailColor = MvxTouchColor.Black1;
            logout.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            logout.Image = UIImage.FromBundle("images/setting_view/reload.png");

            logout.Tapped += () =>
            {
                var tags = new List<string>();
                tags.Add(string.Format("Kindergarten_{0}", 0));
                tags.Add(string.Format("Class_{0}", 0));
                NSSet set = new NSSet(tags.ToArray());
                APService.SetTagsAndAlias(set, "User_Null", new Selector("tagsAliasCallback:tags:alias:"), this);

                BTProgressHUD.ShowSuccessWithStatus("成功退出", 1000);
            };

            StyledStringElement changePassword = new StyledStringElement("修改密码", "", UITableViewCellStyle.Value1);
            changePassword.Tapped += () => _baseViewModel.RePasswordCommand.Execute();
            changePassword.Alignment = UITextAlignment.Left;
            changePassword.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            changePassword.Font = EasyLayout.TitleFont;
            changePassword.SubtitleFont = EasyLayout.SubTitleFont;
            changePassword.TextColor = MvxTouchColor.Black1;
            changePassword.DetailColor = MvxTouchColor.Black1;
            changePassword.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            changePassword.Image = UIImage.FromBundle("images/setting_view/password.png");

            section.Add(changePassword);
           
//            section.Add(checkVersion);
            section.Add(clearCache);
            section.Add(feedback);
            if (BabyBusContext.RoleType == RoleType.Parent)
                section.Add(payment);
//            section.Add(about);
            section.Add(logout);
            // Perform any additional setup after loading the view, typically from a nib.
            Root = new RootElement(null)
            {
                section
            };
            if (this.NavigationController != null)
                this.NavigationController.SetNavigationBarHidden(false, false);


//            section.FooterView = new UIView(new CGRect(0, 0, 320, 480));
        }
    }
    //class
}

	