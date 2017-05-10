using System;
using System.Linq;
using BabyBus.Models;
using BabyBus.ViewModels.Login;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Dialog.Touch;
using CrossUI.Touch.Dialog.Elements;
using Foundation;
using ObjCRuntime;
using UIKit;
using Views.BindableElements;
using CoreGraphics;
using CrossUI.Touch.Dialog.Utilities;
using BigTed;
using Cirrious.MvvmCross.Plugins.Color.Touch;

namespace BabyBus.iOS {


    public partial class RegisterDetailView : MvxDialogViewController {

        private RegisterDetailViewModel _baseViewModel;

        public RegisterDetailView()
            : base(UITableViewStyle.Grouped,
                   null,
                   true) {

        }

        public override void DidReceiveMemoryWarning() {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            if (this.NavigationController != null)
                this.NavigationController.SetNavigationBarHidden(false, false);
        }

        public override void ViewWillDisappear(bool animated) {
            base.ViewWillDisappear(animated);
            //            if (this.NavigationController != null)
            //                this.NavigationController.SetNavigationBarHidden(true, false);
            //fix crash when keyboard is show
            View.EndEditing(true);
        }

        public override void ViewDidAppear(bool animated) {
            base.ViewDidAppear(animated);
            _baseViewModel.RaiseAllPropertiesChanged();
        }


        void HideKeyBoardWhenTap() {
            //TODO: test tap in the uitextview
            var g = new UITapGestureRecognizer(() => {
                var firstResponder = View.FindFirstResponder();
                if (firstResponder != null) {
                    firstResponder.ResignFirstResponder();
                }
            });
            View.AddGestureRecognizer(g);
        }

        void CreateRoot() {
            var bindings = this.CreateInlineBindingTarget<RegisterDetailViewModel>();
            var birthday = new DateTime(2011, 1, 1);
            StringElement ele = new StringElement("完成绑定", delegate {
                Console.WriteLine("RegisterCommand Execute start ");
                var vm = this.ViewModel as RegisterDetailViewModel;
                vm.RegisterCommand.Execute();
                Console.WriteLine("RegisterCommand Execute finish");
            });

            var photoElement = 
                new PhotoElement(this)
                    .Bind(bindings, e => e.Name, vm => vm.ChildName)
                    .Bind(bindings, e => e.ImageUri, vm => vm.Filename, "StringToUriThumb")
                //                      .Bind(bindings, e => e.DescriptionOfChild, vm => vm.Phone)
                    .Bind(bindings, e => e.ChoosePictureWithCropCommand, vm => vm.ChoosePictureWithCropCommand)
                    .Bind(bindings, e => e.TakePictureWithCropCommand, vm => vm.TakePictureWithCropCommand);


            // Perform any additional setup after loading the view, typically from a nib.
            Root = new RootElement("注册与绑定") {
                new Section("绑定信息") {
                    new EntryElement("密保卡", "免费用户不需要输入").Bind(this, "Value Mibaoka"),
                    new RadioRootElement<CityModel>("选择城市", new RadioGroup("选择城市", 0)) {
                        new BindableSection<CityBindableRadioElement, CityModel>()
                            .Bind(bindings, element => element.ItemsSource, vm => vm.Cities)
                        //                            .Bind(bindings, e => e, vm => vm.City) ,
                    }.Bind(bindings, e => e.EnhanceRadioSelected, vm => vm.City) 
                        .Bind(bindings, element => element.Items, vm => vm.Cities),

                    new RadioRootElement<KindergartenModel>("选择幼儿园", new RadioGroup("选择幼儿园", 0)) {
                        new BindableSection<KindergartenBindableRadioElement, KindergartenModel>().Bind(bindings, element => element.ItemsSource, vm => vm.Kindergartens),
                    }.Bind(bindings, e => e.EnhanceRadioSelected, vm => vm.Kindergarten)
                        .Bind(bindings, element => element.Items, vm => vm.Kindergartens),

                    new RadioRootElement<KindergartenClassModel>("选择班级", new RadioGroup("选择班级", 0)) {
                        new BindableSection<KindergartenBindableRadioElement, KindergartenClassModel>().Bind(bindings, element => element.ItemsSource, vm => vm.KindergartenClasses),
                    }.Bind(bindings, e => e.EnhanceRadioSelected, vm => vm.KindergartenClass)
                        .Bind(bindings, element => element.Items, vm => vm.KindergartenClasses),
                },
                new Section("注册信息") {
                    new EntryElement("姓名").Bind(bindings, e => e.Value, vm => vm.ChildName),
                    //                    new EntryElement("简介").Bind(bindings, e => e.Value, vm => vm.Phone),
                    new RadioRootElement<GenderModel>("选择性别", new RadioGroup("选择性别", 0)) {
                        new BindableSection<GenderBindableRadioElement, GenderModel>().Bind(bindings, element => element.ItemsSource, vm => vm.Genders),
                    }.Bind(bindings, e => e.EnhanceRadioSelected, vm => vm.Gender)
                        .Bind(bindings, element => element.Items, vm => vm.Genders),

                    new DateElement("生日", birthday).Bind(bindings, e => e.Value, vm => vm.Birthday),
                    //Oneway : This binding mode transfers values from the ViewModel to the View
                    photoElement,
                }
            };
            //
            //            var set = photoElement.CreateBindingSet<PhotoElement, RegisterDetailViewModel>();
            //            set.Bind().For(me => me.Name).To(p => p.ChildName);
            //            set.Bind().For(me => me.Photo).To(p => p.Bytes).TwoWay().WithConversion("InMemoryImage");
            //            set.Bind().For(me => me.ChoosePictureWithCropCommand).To(p => p.ChoosePictureWithCropCommand);
            //            set.Bind().For(me => me.TakePictureWithCropCommand).To(p => p.TakePictureWithCropCommand);
            //            set.Apply();
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            //            HideKeyBoardWhenTap();
            //          TableView.BackgroundView = null;
            //          TableView.BackgroundColor = UIColor.Green;
            //
            //          TableView.BackgroundView = null;
            //          TableView.BackgroundColor = new UIColor (255.0f, 255.0f, 255.0f, 0.03f);

            //          TableView.BackgroundColor = UIColor.Clear;
            //          ParentViewController.View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("sea.jpg"));
            if (this.NavigationController != null)
                this.NavigationController.SetNavigationBarHidden(false, false);

            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;
            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) => {
                    //                  SetEditing (true, true);
                    Console.WriteLine("RegisterCommand Execute start ");
                    var vm = this.ViewModel as RegisterDetailViewModel;
                    vm.RegisterCommand.Execute();
                    var firstResponder = View.FindFirstResponder();
                    if (firstResponder != null) {
                        firstResponder.ResignFirstResponder();
                    }
                })
                , true);

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;
            _baseViewModel = ViewModel as RegisterDetailViewModel;

            _baseViewModel.InfoChanged += (object sender, EventArgs e) => {
                ShowHUD();
            };

            CreateRoot();
        }

        public virtual void ShowHUD() {
            if (_baseViewModel != null && !string.IsNullOrEmpty(_baseViewModel.Information)) {
                //                BTProgressHUD.Dismiss();
                ProgressHUD.Shared.HudForegroundColor = UIColor.White;
                ProgressHUD.Shared.HudToastBackgroundColor = MvxTouchColor.Gray1;
                ProgressHUD.Shared.HudBackgroundColour = MvxTouchColor.Gray1;

                if (_baseViewModel.IsSuccessStatus) {
                    BTProgressHUD.ShowSuccessWithStatus(_baseViewModel.Information, Constants.RefreshTime);
                } else if (_baseViewModel.IsErrorStatus) {
                    BTProgressHUD.ShowErrorWithStatus(_baseViewModel.Information, Constants.RefreshTime);
                } else if (_baseViewModel.IsRunning) {
                    ProgressHUD.Shared.Show(
                        "取消"
                        ,
                        () => ProgressHUD.Shared.ShowErrorWithStatus("取消操作!")
                        ,
                        _baseViewModel.Information,
                        -1,
                        ProgressHUD.MaskType.None,
                        Constants.ProgressLongTime); 
                } else {
                    BTProgressHUD.ShowToast(
                        _baseViewModel.Information,
                        ProgressHUD.MaskType.None,
                        true,
                        Constants.RefreshTime);
                }

            } else {
                BTProgressHUD.Dismiss();
            }
        }
    }
}

