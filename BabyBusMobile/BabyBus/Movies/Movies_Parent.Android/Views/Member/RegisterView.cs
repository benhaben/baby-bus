using BabyBus.Droid.Views;
using Android.App;
using BabyBus.Logic.Shared;
using Android.OS;
using Android.Widget;
using Android.Views;
using BabyBus.Droid.Utils;
using System.Timers;

namespace BabyBus.Droid
{

	[Activity(Label = "RegisterView", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class RegisterView : ViewBase<RegisterViewModel>
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			// Create your application here
			SetContentView(Resource.Layout.RegisterView);
			//TODO:Agreement
			ViewModel.AgreeWithDocuments = true;
			var llRePassword = FindViewById<LinearLayout>(Resource.Id.repassword_edit);  
			var llPassword = FindViewById<LinearLayout>(Resource.Id.password_edit);
			var llIdentifyCode = FindViewById<LinearLayout>(Resource.Id.text_identifycode);
			var btnSignIn = FindViewById<Button>(Resource.Id.signin_button);
			var btnFindPassword = FindViewById<Button>(Resource.Id.button_findpassword);
			var textIdentifyCode = FindViewById<TextView>(Resource.Id.button_getindentifycode);
			var textCheckIdentifyCode = FindViewById<TextView>(Resource.Id.textview_checkIdentifyCode);
			var textInformation = FindViewById<TextView>(Resource.Id.login_user_hint);
			var user_name = FindViewById<LinearLayout>(Resource.Id.setting_info_childname);
			textCheckIdentifyCode.Click += (sender, e) => ViewModel.ConfirmIdentifyCodeCommand.Execute();

			textIdentifyCode.Click += (sender, e) => {
				llIdentifyCode.Focusable = true;
				ViewModel.GetIdentifyCodeCommand.Execute();
				textIdentifyCode.SetBackgroundResource(Resource.Drawable.button_gray);
				textIdentifyCode.Clickable = false;
				var timer = ViewModel.CreateTimer();
				timer.Start();
				int timeticks = 60;
				var handler = new Handler();
				timer.Elapsed += (sender1, timer1) => handler.Post(() => {
					textIdentifyCode.Text = string.Format("重新获取{0}S", timeticks--);
					if (ViewModel.Step == RegisterStepStatus.Error) {
						SimpleerAlertDialog.SimplAlertDialog(this, "号码已注册！", string.Format("号码:{0}已经注册过，请直接登录，如忘记密码，请选择找回密码", ViewModel.Phone)
							, "找回密码", "直接登录", ViewModel.GotoFindPasswordCommand, ViewModel.GotoLoginCommand);
						EnableClick(timer);
					}
					if (ViewModel.Step == RegisterStepStatus.UserNotExist) {
						SimpleerAlertDialog.SimplAlertDialog(this, "该账户不存在！", string.Format("账户:{0}的用户不存在，请检查号码或快速注册！"
							, ViewModel.Phone), "检查号码", "快速注册", null, ViewModel.GotoRegisterNewUserCommand);
						EnableClick(timer);
					}
					if (timeticks <= 0) {
						EnableClick(timer);
					}
				});
			};



			if (ViewModel.RegisterViewModelType == RegisterViewModelType.FindPassword) {
				textInformation.Text = "找回密码";
				llRePassword.Visibility = ViewStates.Gone;
				llPassword.Visibility = ViewStates.Gone;
				textCheckIdentifyCode.Visibility = ViewStates.Gone;
				btnSignIn.Text = "确认验证码";
				btnSignIn.Click += (sender, e) => ViewModel.ConfirmIdentifyCodeCommand.Execute();
				ViewModel.IdentifyEventHandler += (sender, e) => {
					llRePassword.Visibility = ViewStates.Visible;
					llPassword.Visibility = ViewStates.Visible;
					btnFindPassword.Visibility = ViewStates.Visible;
					btnSignIn.Visibility = ViewStates.Gone;
					llIdentifyCode.Visibility = ViewStates.Gone;
					user_name.Visibility = ViewStates.Gone;
				};
			} else {
				textInformation.Text = "新用户注册";
				btnSignIn.Visibility = ViewStates.Gone;
				btnSignIn.Text = "注册";
				llRePassword.Visibility = ViewStates.Gone;
				llPassword.Visibility = ViewStates.Gone;
				btnSignIn.Click += (sender, e) => ViewModel.SubmitCommand.Execute();
				ViewModel.IdentifyEventHandler += (sender, e) => {
					llIdentifyCode.Visibility = ViewStates.Gone;
					textCheckIdentifyCode.Visibility = ViewStates.Gone;
					user_name.Visibility = ViewStates.Gone;
					llRePassword.Visibility = ViewStates.Visible;
					llPassword.Visibility = ViewStates.Visible;
					btnSignIn.Visibility = ViewStates.Visible;

				};
			}
		}

		protected override void OnStart()
		{
			base.OnStart();

			//Hide ActionBar
			ActionBar.Hide();
		}

		private void EnableClick(Timer timer)
		{
			var textIdentifyCode = FindViewById<TextView>(Resource.Id.button_getindentifycode);
			textIdentifyCode.Clickable = true;
			textIdentifyCode.SetBackgroundResource(Resource.Drawable.bg_rectangle_green);
			textIdentifyCode.Text = "重新获取";
			timer.Close();
		}
	}
}
