
using System;
using CoreGraphics;
using BabyBus.ViewModels.Login;
using UIKit;
using Cirrious.MvvmCross.Binding.BindingContext;
using ObjCRuntime;
using CoreGraphics;
using CoreAnimation;
using Cirrious.MvvmCross.Plugins.Color.Touch;
using Cirrious.CrossCore.UI;

namespace BabyBus.iOS {
    public class RegisterView : MvxBabyBusBaseViewController {
        private RegisterViewModel _baseViewModel;

        public RegisterView() {
            AddGestureWhenTap = true;
        }

        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
//            this.NavigationController.SetNavigationBarHidden(true, animated);
        }

        public override void ViewWillDisappear(bool animated) {
            base.ViewWillDisappear(animated);
//            this.NavigationController.SetNavigationBarHidden(false, animated);
        }

        public override void DidReceiveMemoryWarning() {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
            ReleaseDesignerOutlets();
        }

        void ReleaseDesignerOutlets() {
            if (Password != null) {
                Password.Dispose();
            }

            if (UserName != null) {
                UserName.Dispose();
            }
        }

        UIImageView _imageView = null;

        public UIImageView ImageAndTitle {
            get {
                if (_imageView == null) {   
                    _imageView = 
                        new UIImageView(UIImage.FromFile("loginPagePicture.png"));
                }
                return _imageView;
            }
            set {
                _imageView = value;
            }
        }

        LineTextField _userName = null;

        public LineTextField UserName {
            get {
                if (_userName == null) {
                    _userName = new LineTextField("电话号码");
                }
                return _userName;
            }
        }

        LineTextField _password = null;

        public LineTextField Password {
            get {
                if (_password == null) {
                    _password = new LineTextField("密码");
                    _password.SecureTextEntry = true;
                }
                return _password;
            }
        }

        LineTextField _confirmPassword = null;

        public LineTextField ConfirmPassword {
            get {
                if (_confirmPassword == null) {
                    _confirmPassword = new LineTextField("请确认密码");
                    _confirmPassword.SecureTextEntry = true;
                }
                return _confirmPassword;
            }
        }

        CircleButton _registerButton = null;

        public CircleButton RegisterButton {
            get {
                if (_registerButton == null) {   
                    _registerButton = new CircleButton("注册");
                }
                return _registerButton;
            }
            set {
                _registerButton = value;
            }
        }

        UIButton _contractButton = null;

        public UIButton ContractButton {
            get {
                if (_contractButton == null) {   
                    _contractButton = new UIButton();
                    _contractButton.BackgroundColor = MvxTouchColor.ToUIColor(MvxColors.White);
                    _contractButton.TitleLabel.Font = UIFont.SystemFontOfSize(11);
                    _contractButton.SetTitle("贝贝巴士服务协议", UIControlState.Normal);
                    _contractButton.SetTitleColor(MvxTouchColor.Blue, UIControlState.Normal);
                    _contractButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                    _contractButton.Frame = new CGRect(0, 0, 98f, EasyLayout.NormalTextFieldHeight);
                    var line = EasyLayout.CreateLineLayer(_contractButton, MvxTouchColor.Blue);
                    _contractButton.Layer.AddSublayer(line);
                    _contractButton.TitleEdgeInsets = new UIEdgeInsets(0, 0, -20, 0);
                }
                return _contractButton;
            }
            set {
                _contractButton = value;
            }
        }

        UISwitch _switch = null;

        public UISwitch ConfirmContract {
            get {
                if (_switch == null) {   
                    _switch = new UISwitch();
                    _switch.OnTintColor = MvxTouchColor.Green;
                    _switch.Transform = CGAffineTransform.MakeScale(0.75f, 0.75f);
                }
                return _switch;
            }
        }

        void CreateBinding() {
            // Perform any additional setup after loading the view, typically from a nib.
            var set = this.CreateBindingSet<RegisterView, RegisterViewModel>();
            set.Bind(UserName).To(vm => vm.Phone);
            set.Bind(Password).To(vm => vm.Password);
            set.Bind(ConfirmPassword).To(vm => vm.ConfirmedPassword);
            set.Bind(RegisterButton).To(vm => vm.DetialRegisterCommand);
            set.Bind(ContractButton).To(vm => vm.ShowContractCommand);
            set.Bind(ConfirmContract).To(vm => vm.AgreeWithDocuments);
            set.Apply();

            RegisterButton.TouchUpInside += (sender, e) => View.EndEditing(true);
        }

      

        public override void SetUpConstrainLayout() {
            base.SetUpConstrainLayout();
            nfloat contractButtonHeight = 100f;
            View.ConstrainLayout 
            (
                // Analysis disable CompareOfFloatsByEqualityOperator
                () => 
                ImageAndTitle.Frame.Top == Container.Frame.Top + EasyLayout.MarginTopToFrameWithoutBar
                && ImageAndTitle.Frame.GetMidX() == Container.Frame.GetMidX()
                && ImageAndTitle.Frame.Height == EasyLayout.ImageAndTitleHeight
                && ImageAndTitle.Frame.Width == EasyLayout.ImageAndTitleWidth

                && UserName.Frame.Height == EasyLayout.NormalTextFieldHeight
                && UserName.Frame.Width == EasyLayout.TextFieldWidth
                && UserName.Frame.GetCenterX() == Container.Frame.GetCenterX()
                && UserName.Frame.Top == ImageAndTitle.Frame.Bottom + EasyLayout.MarginMedium

                && Password.Frame.Height == EasyLayout.NormalTextFieldHeight
                && Password.Frame.Width == EasyLayout.TextFieldWidth
                && Password.Frame.GetCenterX() == Container.Frame.GetCenterX()
                && Password.Frame.Top == UserName.Frame.Bottom + EasyLayout.MarginMedium

                && ConfirmPassword.Frame.Height == EasyLayout.NormalTextFieldHeight
                && ConfirmPassword.Frame.Width == EasyLayout.TextFieldWidth
                && ConfirmPassword.Frame.GetCenterX() == Container.Frame.GetCenterX()
                && ConfirmPassword.Frame.Top == Password.Frame.Bottom + EasyLayout.MarginMedium

                && ContractButton.Frame.Height == EasyLayout.NormalTextFieldHeight
                && ContractButton.Frame.Width == contractButtonHeight
                && ContractButton.Frame.Left == ConfirmPassword.Frame.Left
                && ContractButton.Frame.Top == ConfirmPassword.Frame.Bottom + EasyLayout.MarginMedium

                && ConfirmContract.Frame.Left == ContractButton.Frame.Right
                && ConfirmContract.Frame.Bottom == ContractButton.Frame.Bottom + EasyLayout.MarginSmall

                && RegisterButton.Frame.Height == EasyLayout.CircleButtonHeight
                && RegisterButton.Frame.Width == EasyLayout.CircleButtonHeight
                && RegisterButton.Frame.GetCenterX() == Container.Frame.GetCenterX()
                && RegisterButton.Frame.Top == ContractButton.Frame.Bottom + EasyLayout.MarginMedium

                && Container.Frame.Bottom == RegisterButton.Frame.Bottom

                // Analysis restore CompareOfFloatsByEqualityOperator
            );
        }

        public override void PrepareViewHierarchy() {
            base.PrepareViewHierarchy();
            UIView[] v = {
                ImageAndTitle,
                UserName,
                Password,
                ConfirmPassword,
                ContractButton,
                ConfirmContract,
                RegisterButton,
            };
            Container.AddSubviews(v);
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();

            _baseViewModel = ViewModel as RegisterViewModel;

            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;


            this.Title = "注册";
            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;

            RegisterButton.Layer.CornerRadius = UIConstants.CornerRadius;

            CreateBinding();

            UserName.ShouldReturn = (t) => {
                if (string.IsNullOrEmpty(_baseViewModel.Phone)) {
                    ShowBubbleFor(UserName, "请输入用户名");
                } else {
                    HideBubbleFor(UserName);
                }
                UserName.ResignFirstResponder();
                Password.BecomeFirstResponder();
                return true;
            };

            Password.ShouldReturn = (t) => {
                Password.ResignFirstResponder();

                if (string.IsNullOrEmpty(_baseViewModel.Password)) {
                    ShowBubbleFor(Password, "请输入密码");
                } else {
                    HideBubbleFor(Password);
                }

                UserName.ResignFirstResponder();
                Password.ResignFirstResponder();
                ConfirmPassword.BecomeFirstResponder();
                return true;
            };
            ConfirmPassword.ShouldReturn = (t) => {
                ConfirmPassword.ResignFirstResponder();
                bool hasInput = true;
                if (string.IsNullOrEmpty(_baseViewModel.ConfirmedPassword)) {
                    ShowBubbleFor(Password, "请再次输入密码");
                    hasInput = false;
                } else {
                    HideBubbleFor(Password);
                }

                if (hasInput) {
                    if (string.IsNullOrEmpty(_baseViewModel.Phone)) {
                        UserName.BecomeFirstResponder();
                    } else if (string.IsNullOrEmpty(_baseViewModel.Password)) {
                        Password.BecomeFirstResponder();
                    } else {
                        UserName.ResignFirstResponder();
                        Password.ResignFirstResponder();
                        ConfirmPassword.ResignFirstResponder();
                        _baseViewModel.DetialRegisterCommand.Execute();
                    }
                } else {
                    return false;
                }
                return true;
            };


            _baseViewModel.InfoChanged += (object sender, EventArgs e) => {

                bool hasInput = true;
                if (string.IsNullOrEmpty(_baseViewModel.Phone)) {
                    ShowBubbleFor(UserName, "请输入电话号码");
                    hasInput = false;
                } else {
                    HideBubbleFor(UserName);
                }
                if (string.IsNullOrEmpty(_baseViewModel.Password)) {
                    ShowBubbleFor(Password, "请输入密码");
                    hasInput = false;
                } else {
                    HideBubbleFor(Password);
                }

                if (string.IsNullOrEmpty(_baseViewModel.ConfirmedPassword)) {
                    ShowBubbleFor(ConfirmPassword, "请再次输入密码");
                    hasInput = false;
                } else {
                    HideBubbleFor(ConfirmPassword);
                }

                if (hasInput) {
                    ShowHUD();
                }

            };
        }
        //viewdidload
    }
    //class
}

