using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;
using System.Net;
using System.Timers;
using Xamarin;


namespace BabyBus.Logic.Shared
{
	public enum RegisterStepStatus
	{
		Error = 0,
		GetIdentifyCode = 1,
		CheckIdentifyCode = 2,
		InputPassword = 3,
		UserNotExist = 4,
	}

	public enum RegisterViewModelType
	{
		RegisterNewUser = 0,
		FindPassword = 1
	}

	public class RegisterViewModel : BaseViewModel
	{
		public readonly int TimerSeconds = 60;

		public Timer CreateTimer()
		{
			// Create a timer with a two second interval.
			var timer = new System.Timers.Timer(1000 * TimerSeconds);
			// Hook up the Elapsed event for the timer. 
			//            timer.Elapsed += OnTimedEvent;
			timer.Interval = 1000;
			timer.AutoReset = true;
			return timer;
		}

		private readonly IRemoteService _remoteService;

		public Action SwitchUIControlStateByStepAction = null;
		RegisterStepStatus _step = RegisterStepStatus.GetIdentifyCode;

		public RegisterStepStatus Step { 
			get { return _step; } 
			set {
				_step = value; 
				if (SwitchUIControlStateByStepAction != null) {
					SwitchUIControlStateByStepAction();
				}
			}
		}


		public RegisterViewModelType RegisterViewModelType {
			get;
			set;
		}

		public event EventHandler IdentifyEventHandler;

		private void IdentifyEventInvoke()
		{
			if (IdentifyEventHandler != null) {
				try {
					InvokeOnMainThread(() => IdentifyEventHandler(this, null));
				} catch (Exception ex) {
					Insights.Report(ex, Insights.Severity.Error);
				}
			}
		}

		public RegisterViewModel()
		{
			_remoteService = Mvx.Resolve<IRemoteService>();
		}

		public void Init(RegisterViewModelType registerViewModelType)
		{
			RegisterViewModelType = registerViewModelType;
		}

		public bool IsVaild()
		{
			if (Phone.Length <= 0
			    || Password.Length <= 0
			    || ConfirmedPassword.Length <= 0) {
				ViewModelStatus = new ViewModelStatus("手机号和密码不能为空", false, MessageType.Error, TipsType.Bubble);
				return false;
			}

//			if (!LogicUtils.IsHandset (Phone)) {
//				ViewModelStatus = new ViewModelStatus ("请输入正确的手机号", false, MessageType.Error, TipsType.Bubble);
//				return false;
//			}
			if (!Password.Equals(ConfirmedPassword)) {
				ViewModelStatus = new ViewModelStatus("两次输入的密码不一致", false, MessageType.Error, TipsType.DialogWithOkButton);
				return false;
			}

			if (Password.Length < Constants.MiniPasswordLength || ConfirmedPassword.Length < Constants.MiniPasswordLength) {
				ViewModelStatus = new ViewModelStatus("密码的最小长度为6", false, MessageType.Error, TipsType.DialogWithOkButton);
				return false;
			}

			return true;         
		}

		private string _phone = string.Empty;

		public string Phone {
			get { return _phone; }
			set {
				_phone = value;
			}
		}

		public string IdentifyCode {
			get;
			set;
		}

		private string _password = string.Empty;

		public string Password {
			get { return _password; }
			set {
				_password = value;
			}
		}

		private string _confirmedpassword = string.Empty;

		public string ConfirmedPassword {
			get { return _confirmedpassword; }
			set {
				_confirmedpassword = value;
			}
		}

		public Action GetIdentifyCodeSuccessAction = null;
		private MvxCommand _getIdentifyCodeCommand;

		public MvxCommand GetIdentifyCodeCommand {
			get {
				_getIdentifyCodeCommand = _getIdentifyCodeCommand
				?? new MvxCommand(async () => {
					Mvx.Trace("click GetIdentifyCodeCommand......");
					if (Phone.IsEmpty() || !Phone.IsHandset()) {
						ViewModelStatus = new ViewModelStatus("非法的电话号码", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					try {
						bool checkExist = RegisterViewModelType != RegisterViewModelType.FindPassword;
						await _remoteService.GetIdentifyCode(Phone, checkExist);
						Step = RegisterStepStatus.CheckIdentifyCode;
						if (GetIdentifyCodeSuccessAction != null) {
							GetIdentifyCodeSuccessAction();
						}
					} catch (BabyBusWebServiceException ex) {
						if (ex.StatusCode == HttpStatusCode.MethodNotAllowed) {
							if (RegisterViewModelType == RegisterViewModelType.FindPassword) {
								Step = RegisterStepStatus.UserNotExist;
							} else {
								Step = RegisterStepStatus.Error;
							}
						} 
						
					} catch (WebException ex) {
						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Information, TipsType.DialogDisappearAuto);
					}
				});
				return _getIdentifyCodeCommand;
			}
		}

		private MvxCommand _confirmIdentifyCodeCommand;

		public MvxCommand ConfirmIdentifyCodeCommand {
			get {
				_confirmIdentifyCodeCommand = _confirmIdentifyCodeCommand
				?? new MvxCommand(async () => {
					Mvx.Trace("click ConfirmIdentifyCodeCommand......");
					if (IdentifyCode.IsEmpty() || !IdentifyCode.IsNumber()) {
						ViewModelStatus = new ViewModelStatus("请输入验证码！", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					try {
						ViewModelStatus = new ViewModelStatus("正在验证……", false, MessageType.Success, TipsType.DialogProgress);
						var result = await _remoteService.CheckIdentifyCode(Phone, IdentifyCode);
						if (result) {
							Step = RegisterStepStatus.InputPassword;
							ViewModelStatus = new ViewModelStatus("验证码验证成功！", false, MessageType.Success, TipsType.DialogDisappearAuto);
							IdentifyEventInvoke();
						} else {
							ViewModelStatus = new ViewModelStatus("验证码错误！", false, MessageType.Information, TipsType.DialogDisappearAuto);
						}
					} catch (Exception ex) {
						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Information, TipsType.DialogDisappearAuto);
					}
				});
				return _confirmIdentifyCodeCommand;
			}
		}

		private MvxCommand _findPasswordCommand;

		public MvxCommand FindPasswordCommand {
			get {
				_findPasswordCommand = _findPasswordCommand ?? new MvxCommand(() => {
					RegisterViewModelType = RegisterViewModelType.FindPassword;
					Step = RegisterStepStatus.GetIdentifyCode;
				});
				return _findPasswordCommand;
			}
		}

		private MvxCommand _submitCommand;

		public MvxCommand SubmitCommand {
			get {
				_submitCommand = _submitCommand ?? new MvxCommand(async () => {
					Mvx.Trace("click DetialRegisterCommand......");
					if (!IsVaild()) {
						return;
					}
		
					ViewModelStatus = new ViewModelStatus("请等待...", true, MessageType.Information, TipsType.DialogDisappearAuto);
		
					try {
						if (RegisterViewModelType.FindPassword == RegisterViewModelType) {
							await _remoteService.ResetPassword(Phone, IdentifyCode, Password);
						} else {
							var user = new User {
								LoginName = Phone
                                        , RealName = Phone
									, PhoneNumber = Phone
                                        , Password = Password
								//现在只有家长的注册，所有默认为Parent
                                        , RoleType = RoleType.Parent
							};
							await _remoteService.Register(user);
						}
						ViewModelStatus = new ViewModelStatus("成功，请重新登录！", false, MessageType.Success, TipsType.DialogDisappearAuto);
						ShowViewModel<LoginViewModel>();
						//Close(this);
					} catch {
						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
					}
				});
				return _submitCommand;
			}
		
		}

		bool _agreeWithDocuments = false;

		public bool AgreeWithDocuments {
			get { return _agreeWithDocuments; }
			set {
				_agreeWithDocuments = value;
				RaisePropertyChanged(() => AgreeWithDocuments);
			}
		}

		private MvxCommand _showContractCommand;

		public MvxCommand ShowContractCommand {
			get {
				_showContractCommand = _showContractCommand ?? new MvxCommand(() => {
					ViewModelStatus = new ViewModelStatus("服务协议在睿莱网站", false, MessageType.Information, TipsType.DialogDisappearAuto);

//					Information = "请输入正确的手机号码和密码";
				});
				return _showContractCommand;
			}
		}

		public MvxCommand GotoRegisterDetailCommand {
			get {
				return new MvxCommand(() => ShowViewModel<RegisterDetailViewModel>());
			}
		}

		public MvxCommand GotoFindPasswordCommand {
			get {
				return new MvxCommand(() => ShowViewModel<RegisterViewModel>(new {registerViewModelType = RegisterViewModelType.FindPassword}));
			}
		}

		public MvxCommand GotoRegisterNewUserCommand {
			get {
				return new MvxCommand(() => ShowViewModel<RegisterViewModel>(new {registerViewModelType = RegisterViewModelType.RegisterNewUser}));
			}
		}

		public MvxCommand GotoLoginCommand {
			get {
				return new MvxCommand(() => ShowViewModel<LoginViewModel>());
			}
		}
	}
}
