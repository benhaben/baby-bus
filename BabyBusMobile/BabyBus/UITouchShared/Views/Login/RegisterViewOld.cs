
using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using CoreGraphics;
using Foundation;
using JPushBinding;
using ObjCRuntime;
using UIKit;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class RegisterView : MvxBabyBusBaseViewController
    {
        private RegisterViewModel _baseViewModel;

        public RegisterView()
        {
            NeedRegisterShowHUD = false;
            AddGestureWhenTap = true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            //            this.NavigationController.SetNavigationBarHidden(true, animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            //            this.NavigationController.SetNavigationBarHidden(false, animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
            ReleaseDesignerOutlets();
        }

        void ReleaseDesignerOutlets()
        {
            if (Password != null)
            {
                Password.Dispose();
            }

            if (UserName != null)
            {
                UserName.Dispose();
            }
        }

        //        UIImageView _imageView = null;

        //        public UIImageView ImageAndTitle
        //        {
        //            get
        //            {
        //                if (_imageView == null)
        //                {
        //                    _imageView =
        //                        new UIImageView(UIImage.FromFile("icon-512.png"));
        //                }
        //                return _imageView;
        //            }
        //            set
        //            {
        //                _imageView = value;
        //            }
        //        }


        LineTextField _userName = null;

        public LineTextField UserName
        {
            get
            {
                if (_userName == null)
                {
                    _userName = new LineTextField();
                    _userName.Placeholder = "请输入手机号";

                    _userName.TextAlignment = UITextAlignment.Left;
                    _userName.Font = EasyLayout.ContentFont;
                    _userName.BackgroundColor = MvxTouchColor.White;
                    _userName.TextColor = MvxTouchColor.Black1;

                }
                return _userName;
            }
        }

        //        LineTextField _phoneNumber = null;
        //
        //        public LineTextField PhoneNumber
        //        {
        //            get
        //            {
        //                if (_phoneNumber == null)
        //                {
        //                    _phoneNumber = new LineTextField();
        //                    _phoneNumber.Placeholder = "电话号码";
        //
        //                    _phoneNumber.TextAlignment = UITextAlignment.Left;
        //                    _phoneNumber.Font = EasyLayout.ContentFont;
        //                    _phoneNumber.BackgroundColor = MvxTouchColor.White;
        //                    _phoneNumber.TextColor = MvxTouchColor.Black1;
        //
        //                }
        //                return _phoneNumber;
        //            }
        //        }

        LineTextField _password = null;

        public LineTextField Password
        {
            get
            {
                if (_password == null)
                {
                    _password = new LineTextField();
                    _password.SecureTextEntry = true;
                    _password.Placeholder = "密码";

                }
                return _password;
            }
        }

        LineTextField _confirmPassword = null;

        public LineTextField ConfirmPassword
        {
            get
            {
                if (_confirmPassword == null)
                {
                    _confirmPassword = new LineTextField();
                    _confirmPassword.Placeholder = "请确认密码";

                }
                return _confirmPassword;
            }
        }

        UIButton _identifyCodeButton = null;

        public UIButton IdentifyCodeButton
        {
            get
            {
                if (_identifyCodeButton == null)
                {
                    _identifyCodeButton = new UIButton();
                    _identifyCodeButton.BackgroundColor = MvxTouchColor.White;
                    _identifyCodeButton.TitleLabel.Font = UIFont.SystemFontOfSize(11);
                    _identifyCodeButton.SetTitle("获取验证码", UIControlState.Normal);
                    _identifyCodeButton.SetTitleColor(MvxTouchColor.Blue, UIControlState.Normal);

                }
                return _identifyCodeButton;
            }
                   
        }

        CircleButton _registerButton = null;

        public CircleButton RegisterButton
        {
            get
            {
                if (_registerButton == null)
                {   
                    _registerButton = new CircleButton("注册");
                    _registerButton.TitleLabel.Font = UIFont.SystemFontOfSize(22);
                    _registerButton.Layer.CornerRadius = UIConstants.CornerRadius;
                }
                return _registerButton;
            }
            set
            {
                _registerButton = value;
            }
        }

        UILabel _identifyCodeInformation;

        public UILabel IdentifyCodeInformation
        {
            get
            {
                if (_identifyCodeInformation == null)
                {
                    _identifyCodeInformation = new UILabel();
                    _identifyCodeInformation.Lines = 2;
                    _identifyCodeInformation.Font = UIFont.SystemFontOfSize(10);
                    _identifyCodeInformation.TextColor = MvxTouchColor.Black1;
                    _identifyCodeInformation.Text = "您可以绑定电话号码，这样您使用我们服务的时候，方便我们联系到您。";
                }
                return _identifyCodeInformation;
            }
        }

        //        UIButton _contractButton = null;

        //        public UIButton ContractButton
        //        {
        //            get
        //            {
        //                if (_contractButton == null)
        //                {
        //                    _contractButton = new UIButton();
        //                    _contractButton.BackgroundColor = MvxTouchColor.White;
        //                    _contractButton.TitleLabel.Font = UIFont.SystemFontOfSize(11);
        //                    _contractButton.SetTitle("贝贝巴士服务协议", UIControlState.Normal);
        //                    _contractButton.SetTitleColor(MvxTouchColor.Blue, UIControlState.Normal);
        //                    _contractButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
        //                    _contractButton.Frame = new CGRect(0, 0, 98f, EasyLayout.SmallButtonWidthAndHeight);
        //                    var line = EasyLayout.CreateLineLayer(_contractButton, MvxTouchColor.Blue);
        //                    _contractButton.Layer.AddSublayer(line);
        //                    _contractButton.TitleEdgeInsets = new UIEdgeInsets(0, 0, -20, 0);
        //                }
        //                return _contractButton;
        //            }
        //            set
        //            {
        //                _contractButton = value;
        //            }
        //        }

        //        UISwitch _switch = null;
        //
        //        public UISwitch ConfirmContract
        //        {
        //            get
        //            {
        //                if (_switch == null)
        //                {
        //                    _switch = new UISwitch();
        //                    _switch.OnTintColor = MvxTouchColor.Green;
        //                    _switch.Transform = CGAffineTransform.MakeScale(0.75f, 0.75f);
        //                }
        //                return _switch;
        //            }
        //        }

      

        void CreateBinding()
        {
            // Perform any additional setup after loading the view, typically from a nib.
            var set = this.CreateBindingSet<RegisterView, RegisterViewModel>();
            set.Bind(UserName).To(vm => vm.Phone);
            set.Bind(Password).To(vm => vm.Password);
            set.Bind(ConfirmPassword).To(vm => vm.ConfirmedPassword);
            set.Bind(RegisterButton).To(vm => vm.DetialRegisterCommand);
//            set.Bind(ContractButton).To(vm => vm.ShowContractCommand);
//            set.Bind(ConfirmContract).To(vm => vm.AgreeWithDocuments);
            set.Apply();
        }

       

        public override void SetUpConstrainLayout()
        {
            base.SetUpConstrainLayout();
//            nfloat ContractButtonWidth = 100f;
            nfloat IdentifyCodeInformationWidth = 200f;


            View.ConstrainLayout 
            (
                // Analysis disable CompareOfFloatsByEqualityOperator
                () => 

//                 ImageAndTitle.Frame.Top == Container.Frame.Top + EasyLayout.MarginMedium
//                && ImageAndTitle.Frame.GetMidX() == Container.Frame.GetMidX()
//                && ImageAndTitle.Frame.Height == EasyLayout.ImageAndTitleHeight
//                && ImageAndTitle.Frame.Width == EasyLayout.ImageAndTitleWidth

                UserName.Frame.Height == EasyLayout.NormalTextViewHeight
                && UserName.Frame.Width == EasyLayout.TextFieldWidth
                && UserName.Frame.GetCenterX() == Container.Frame.GetCenterX()
                && UserName.Frame.Top == Container.Frame.Top + EasyLayout.MarginMedium

                && Password.Frame.Height == EasyLayout.NormalTextViewHeight
                && Password.Frame.Width == EasyLayout.TextFieldWidth
                && Password.Frame.GetCenterX() == Container.Frame.GetCenterX()
                && Password.Frame.Top == UserName.Frame.Bottom + EasyLayout.MarginMedium

                && ConfirmPassword.Frame.Height == EasyLayout.NormalTextViewHeight
                && ConfirmPassword.Frame.Width == EasyLayout.TextFieldWidth
                && ConfirmPassword.Frame.GetCenterX() == Container.Frame.GetCenterX()
                && ConfirmPassword.Frame.Top == Password.Frame.Bottom + EasyLayout.MarginMedium

//                && ContractButton.Frame.Height == EasyLayout.NormalTextViewHeight
//                && ContractButton.Frame.Width == ContractButtonWidth
//                && ContractButton.Frame.Left == ConfirmPassword.Frame.Left
//                && ContractButton.Frame.Top == ConfirmPassword.Frame.Bottom + EasyLayout.MarginMedium
//
//                && ConfirmContract.Frame.Left == ContractButton.Frame.Right
//                && ConfirmContract.Frame.Bottom == ContractButton.Frame.Bottom + EasyLayout.MarginSmall


                && IdentifyCodeInformation.Frame.Height == EasyLayout.NormalTextViewHeight
                && IdentifyCodeInformation.Frame.Width == IdentifyCodeInformationWidth
                && IdentifyCodeInformation.Frame.GetCenterX() == Container.Frame.GetCenterX()
                && IdentifyCodeInformation.Frame.Top == ConfirmPassword.Frame.Bottom + EasyLayout.MarginMedium

//                && PhoneNumber.Frame.Height == EasyLayout.NormalTextViewHeight
//                && PhoneNumber.Frame.Width == EasyLayout.TextFieldWidth
//                && PhoneNumber.Frame.GetCenterX() == Container.Frame.GetCenterX()
//                && PhoneNumber.Frame.Top == IdentifyCodeInformation.Frame.Bottom + EasyLayout.MarginMedium

                && RegisterButton.Frame.Height == EasyLayout.CircleButtonHeight
                && RegisterButton.Frame.Width == EasyLayout.CircleButtonHeight
                && RegisterButton.Frame.GetCenterX() == Container.Frame.GetCenterX()
                && RegisterButton.Frame.Top == PhoneNumber.Frame.Bottom + EasyLayout.MarginNormal

                && Container.Frame.Bottom == RegisterButton.Frame.Bottom

                // Analysis restore CompareOfFloatsByEqualityOperator
            );
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _baseViewModel = ViewModel as RegisterViewModel;

            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;


            this.Title = "注册";
            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;
            CreateBinding();

            UserName.ShouldReturn = (t) =>
            {
                if (string.IsNullOrEmpty(_baseViewModel.Phone))
                {
                    ShowBubbleFor(UserName, "请输入用户名");
                }
                else
                {
                    HideBubbleFor(UserName);
                }
                UserName.ResignFirstResponder();
                Password.BecomeFirstResponder();
                return true;
            };

            Password.ShouldReturn = (t) =>
            {
                Password.ResignFirstResponder();

                if (string.IsNullOrEmpty(_baseViewModel.Password))
                {
                    ShowBubbleFor(Password, "请输入密码");
                }
                else
                {
                    HideBubbleFor(Password);
                }

                UserName.ResignFirstResponder();
                Password.ResignFirstResponder();
                ConfirmPassword.BecomeFirstResponder();
                return true;
            };
            ConfirmPassword.ShouldReturn = (t) =>
            {
                ConfirmPassword.ResignFirstResponder();
                bool hasInput = true;
                if (string.IsNullOrEmpty(_baseViewModel.ConfirmedPassword))
                {
                    ShowBubbleFor(Password, "请再次输入密码");
                    hasInput = false;
                }
                else
                {
                    HideBubbleFor(Password);
                }

                if (hasInput)
                {
                    if (string.IsNullOrEmpty(_baseViewModel.Phone))
                    {
                        UserName.BecomeFirstResponder();
                    }
                    else if (string.IsNullOrEmpty(_baseViewModel.Password))
                    {
                        Password.BecomeFirstResponder();
                    }
                    else
                    {
                        _baseViewModel.DetialRegisterCommand.Execute();
                    }
                }
                else
                {
                    return false;
                }
                return true;
            };


            _baseViewModel.InfoChanged += (object sender, EventArgs e) =>
            {

                bool hasInput = true;
                if (string.IsNullOrEmpty(_baseViewModel.Phone))
                {
                    ShowBubbleFor(UserName, "请输入用户名");
                    hasInput = false;
                }
                else
                {
                    HideBubbleFor(UserName);
                }
                if (string.IsNullOrEmpty(_baseViewModel.Password))
                {
                    ShowBubbleFor(Password, "请输入密码");
                    hasInput = false;
                }
                else
                {
                    HideBubbleFor(Password);
                }

                if (string.IsNullOrEmpty(_baseViewModel.ConfirmedPassword))
                {
                    ShowBubbleFor(ConfirmPassword, "请再次输入密码");
                    hasInput = false;
                }
                else
                {
                    HideBubbleFor(ConfirmPassword);
                }

                if (hasInput)
                {
                    ShowHUD();
                }

            };
        }
        //viewdidload

        public override void PrepareViewHierarchy()
        {
            base.PrepareViewHierarchy();
            UIView[] v =
                {
//                    ImageAndTitle,
                    UserName,
                    Password,
                    ConfirmPassword,
//                    ContractButton,
//                    ConfirmContract,
                    IdentifyCodeInformation,
                    PhoneNumber,
                    RegisterButton,
                };
          
            Container.AddSubviews(v);
        }
    }
}

