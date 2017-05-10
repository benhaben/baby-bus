
using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using CoreGraphics;
using Foundation;
using JPushBinding;
using ObjCRuntime;
using UIKit;
using BabyBus.Logic.Shared;
using WeixinPayBinding.iOS;
using Cirrious.CrossCore;

namespace BabyBus.iOS
{
    

	public class LoginView : MvxBabyBusBaseAutoLayoutViewController
	{
		private LoginViewModel _baseViewModel;

		#region init

		public LoginView()
		{
			NSSet set = new NSSet(new string [] { 
				"null"
			});
			APService.SetTags(set, null, null);

			NeedRegisterShowHUD = false;

		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			//            _scrollView.ContentSize = new SizeF(320, 1000);
		}


		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			this.NavigationController.SetNavigationBarHidden(true, animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			this.NavigationController.SetNavigationBarHidden(false, animated);
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.

			//TODO: test low memory case
			ReleaseDesignerOutlets();
		}

		void ReleaseDesignerOutlets()
		{
			if (Password != null) {
				Password.Dispose();
			}

			if (UserName != null) {
				UserName.Dispose();
			}

			if (ImageTitle != null) {
				ImageTitle.Dispose();
			}
		}

		UIImageView _imageView = null;

		public UIImageView ImageTitle {
			get {
				if (_imageView == null) {   
					_imageView = new UIImageView(UIImage.FromFile("icon-512.png"));
				}
				return _imageView;
			}
			set {
				_imageView = value;
			}
		}

		UIColor GetColor()
		{
			#if __TEACHER__
			return MvxTouchColor.Blue;
			#elif __PARENT__
			return MvxTouchColor.BrightGreen;
			#elif __MASTER__
            return MvxTouchColor.Orange;
			#else
            return MvxTouchColor.Purple1;
			#endif
		}

		string GetVersionName()
		{
			#if __TEACHER__
			return "老师版";
			#elif __PARENT__
			return "家长版";
			#elif __MASTER__
            return "园长版";
			#else
            return "未知版本";
			#endif
		}

		UILabel _title1 = null;

		public UILabel Title1 {
			get {
				if (_title1 == null) {   
					_title1 = new UILabel();
					_title1.Text = "贝贝巴士";
					_title1.Font = EasyLayout.TitleFontBold;
					_title1.TextColor = GetColor();
					_title1.TextAlignment = UITextAlignment.Center;
				}
				return _title1;
			}
			set {
				_title1 = value;
			}
		}

		UILabel _title2 = null;

		public UILabel Title2 {
			get {
				if (_title2 == null) {   
					_title2 = new UILabel();
					_title2.Text = "BABY BUS";
					_title2.Font = EasyLayout.ContentFont;
					_title2.TextColor = GetColor();
					_title2.TextAlignment = UITextAlignment.Center;
				}
				return _title2;
			}
			set {
				_title2 = value;
			}
		}

		UIView _line = null;

		public UIView Line {
			get {
				if (_line == null) {   
					_line = new UIView();
					_line.Frame = new CGRect(
						0,
						0,
						EasyLayout.TextFieldWidth,
						2);

					var layer = EasyLayout.CreateLineLayer(_line, MvxTouchColor.Orange);
					_line.Layer.AddSublayer(layer);
				}
				return _line;
			}
			set {
				_line = value;
			}
		}

		UILabel _title3 = null;

		public UILabel Title3 {
			get {
				if (_title3 == null) {   
					_title3 = new UILabel();
					_title3.Text = GetVersionName();
					_title3.Font = EasyLayout.ContentFont;
					_title3.TextColor = GetColor();
					_title3.TextAlignment = UITextAlignment.Center;
				}
				return _title3;
			}
			set {
				_title3 = value;
			}
		}

		LineTextField _userName = null;

		//Note: if you change the UserName's height, the line's postion should be adjust by yourself
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

		CircleButton _loginButton = null;

		public CircleButton LoginButton {
			get {
				if (_loginButton == null) {   
					_loginButton = new CircleButton("登陆");
					_loginButton.TitleLabel.Font = UIFont.SystemFontOfSize(22);
				}
				return _loginButton;
			}
			set {
				_loginButton = value;
			}
		}

		CircleButton _registerButton = null;

		public CircleButton RegisterButton {
			get {
				if (_registerButton == null) {   
					_registerButton = new CircleButton("注册");
					_registerButton.TitleLabel.Font = UIFont.SystemFontOfSize(18);

				}
				return _registerButton;
			}
		}

		UIButton _findPassword = null;

		public UIButton FindPassword {
			get {
				if (_findPassword == null) {
					_findPassword = new UIButton();
					_findPassword.SetTitle("找回密码", UIControlState.Normal);
					_findPassword.SetTitleColor(MvxTouchColor.Green1, UIControlState.Normal);
				}
				return _findPassword;
			}
		}

		UIButton _weChatLogin = null;

		public UIButton WeChatLogin {
			get {
				if (_weChatLogin == null) {
					_weChatLogin = new UIButton();
					_weChatLogin.SetTitle("微信登录", UIControlState.Normal);
					_weChatLogin.SetTitleColor(MvxTouchColor.Green1, UIControlState.Normal);
				}
				return _weChatLogin;
			}
		}

		#endregion

		void CreateBinding()
		{
			var set = this.CreateBindingSet<LoginView, LoginViewModel>();
			set.Bind(UserName).To(vm => vm.Username);
			set.Bind(Password).To(vm => vm.Password);
			set.Bind(LoginButton).To(vm => vm.LoginCommand);
			set.Bind(RegisterButton).To(vm => vm.GotoRegisterCommand);
			set.Bind(FindPassword).To(vm => vm.FindPasswordCommand);
			set.Apply();
		}

		void CreateReturnButton()
		{
			UIBarButtonItem backItem = new UIBarButtonItem();
			this.NavigationItem.BackBarButtonItem = backItem;
			backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
		}

       

		//        void BlurControl()
		//        {
		//            //            ApplyBackgroundToButton (RegisterButton, View);
		//            ApplyBlurBackgroundToTextField(UserName, View);
		//            ApplyBlurBackgroundToTextField(Password, View);
		//            ApplyBlurBackgroundToButton(LoginButton, View);
		//        }

		public override void SetUpConstrainLayout()
		{
			base.SetUpConstrainLayout();
			nfloat adjustTitle2Top = 5;
			nfloat registerHeight = EasyLayout.CircleButtonHeight * EasyLayout.GoldenRatio;
			View.ConstrainLayout(
				() => 
                ImageTitle.Frame.Top == Container.Frame.Top + EasyLayout.MarginTopToFrameWithoutBar
				&& ImageTitle.Frame.GetMidX() == Container.Frame.GetMidX()
				&& ImageTitle.Frame.Height == EasyLayout.ImageAndTitleWidth
				&& ImageTitle.Frame.Width == EasyLayout.ImageAndTitleWidth

				&& Title1.Frame.Height == EasyLayout.NormalTextFieldHeight
				&& Title1.Frame.Width == EasyLayout.TextFieldWidth
				&& Title1.Frame.GetCenterX() == Container.Frame.GetCenterX()
				&& Title1.Frame.Top == ImageTitle.Frame.Bottom + EasyLayout.MarginMedium

				&& Title2.Frame.Height == EasyLayout.NormalTextFieldHeight
				&& Title2.Frame.Width == EasyLayout.TextFieldWidth
				&& Title2.Frame.GetCenterX() == Container.Frame.GetCenterX()
				&& Title2.Frame.Top == Title1.Frame.Bottom - adjustTitle2Top

				&& Line.Frame.Height == EasyLayout.LineHeight
				&& Line.Frame.Width == EasyLayout.TextFieldWidth
				&& Line.Frame.GetCenterX() == Container.Frame.GetCenterX()
				&& Line.Frame.Top == Title2.Frame.Bottom

				&& Title3.Frame.Height == EasyLayout.NormalTextFieldHeight
				&& Title3.Frame.Width == EasyLayout.TextFieldWidth
				&& Title3.Frame.GetCenterX() == Container.Frame.GetCenterX()
				&& Title3.Frame.Top == Line.Frame.Bottom + EasyLayout.MarginMedium

				&& UserName.Frame.Height == EasyLayout.NormalTextFieldHeight
				&& UserName.Frame.Width == EasyLayout.TextFieldWidth
				&& UserName.Frame.GetCenterX() == Container.Frame.GetCenterX()
				&& UserName.Frame.Top == Title3.Frame.Bottom + EasyLayout.MarginMedium

				&& Password.Frame.Height == EasyLayout.NormalTextFieldHeight
				&& Password.Frame.Width == EasyLayout.TextFieldWidth
				&& Password.Frame.GetCenterX() == Container.Frame.GetCenterX()
				&& Password.Frame.Top == UserName.Frame.Bottom + EasyLayout.MarginMedium

				&& LoginButton.Frame.Height == EasyLayout.CircleButtonHeight
				&& LoginButton.Frame.Width == EasyLayout.CircleButtonHeight
				&& LoginButton.Frame.GetCenterX() == Container.Frame.GetCenterX()
				&& LoginButton.Frame.Top == Password.Frame.Bottom + EasyLayout.MarginTopToFrameWithoutBar

//				&& RegisterButton.Frame.Height == registerHeight
//				&& RegisterButton.Frame.Width == registerHeight
//				&& RegisterButton.Frame.GetCenterX() == Container.Frame.GetCenterX()
//				&& RegisterButton.Frame.Top == LoginButton.Frame.Bottom + EasyLayout.MarginMedium

				&& FindPassword.Frame.Bottom == LoginButton.Frame.Bottom
				&& FindPassword.Frame.Right == Container.Frame.Right - EasyLayout.MarginMedium

//				&& WeChatLogin.Frame.Bottom == RegisterButton.Frame.Bottom
//				&& WeChatLogin.Frame.Left == Container.Frame.Left + EasyLayout.MarginMedium

                //Note: very importatant, set bound
				&& Container.Frame.Bottom == FindPassword.Frame.Bottom
			);
		}


		public override void PrepareViewHierarchy()
		{
			base.PrepareViewHierarchy();

			UIView[] v = {
				ImageTitle,
				Title1,
				Title2,
				Title3,
				Line,
				UserName,
				Password,
				LoginButton,
				RegisterButton,
				FindPassword,
				WeChatLogin
			};
			Container.AddSubviews(v);
		}

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();
			//            this.View.TranslatesAutoresizingMaskIntoConstraints = false;
			//            CreateReturnButton ();
			_baseViewModel = ViewModel as LoginViewModel;
			WXApiDelegateImplement.GetWXApiDelegateInstance().IWxApiDelegate = _baseViewModel;

			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			CreateBinding();
			WeChatLogin.TouchUpInside += (object sender, EventArgs e) => {
				var sendAuthReq = new SendAuthReq();
				sendAuthReq.Scope = "snsapi_message,snsapi_userinfo,snsapi_friend,snsapi_contact";
				sendAuthReq.State = "xxx";
				sendAuthReq.OpenID = BabyBus.Logic.Shared.Constants.APPID;
				WXApi.SendReq(sendAuthReq);
				
				_baseViewModel.ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
//                WXApi.SendAuthReq(sendAuthReq, this, WXApiDelegateImplement.GetWXApiDelegateInstance());
			};

			UserName.ShouldReturn = (t) => {
				if (string.IsNullOrEmpty(_baseViewModel.Username)) {
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
				bool hasInput = true;

				if (string.IsNullOrEmpty(_baseViewModel.Password)) {
					ShowBubbleFor(Password, "请输入密码");
					hasInput = false;
				} else {
					HideBubbleFor(Password);
				}

				if (hasInput) {
					if (string.IsNullOrEmpty(_baseViewModel.Username)) {
						UserName.BecomeFirstResponder();
					} else {
						Password.ResignFirstResponder();
						UserName.ResignFirstResponder();
						_baseViewModel.LoginCommand.Execute();
					}
				} else {
					return false;
				}
				return true;
			};

			//            this.();

			_baseViewModel.InfoChanged += (object sender, EventArgs e) => {

				bool hasInput = true;
				if (string.IsNullOrEmpty(_baseViewModel.Username)) {
					ShowBubbleFor(UserName, "请输入用户名");
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

				if (hasInput) {
					ShowHUD();
				}
			};
		}
	}
	//class
}
//namespace
