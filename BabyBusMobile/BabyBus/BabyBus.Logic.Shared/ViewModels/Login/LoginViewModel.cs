using System;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;
using System.Threading.Tasks;
using BabyBusSSApi.ServiceModel.DTO.Reponse;


namespace BabyBus.Logic.Shared
{

	public class LoginViewModel : BaseViewModel,IWxApiDelegateAdapter
	{
		private readonly IRemoteService _remoteService;
		//private readonly IDataService _dataService;
		//        private readonly IMainService _mainService;

		public LoginViewModel()
		{
			_remoteService = Mvx.Resolve<IRemoteService>();

			//            var environment = Mvx.Resolve<IEnvironmentService>();
			//            _dataService = new DataService(environment);



		}

		private int _userId;

		public int UserId {
			get { return _userId; }
			set {
				_userId = value;
				RaisePropertyChanged(() => UserId);
			}
		}

		private string _username = null;

		public string Username {
			get { return _username; }
			set {
				_username = value.Trim();
				RaisePropertyChanged(() => Username);
			}
		}


		private string _password = string.Empty;

		public string Password {
			get { return _password; }
			set {
				_password = value.Trim();
				RaisePropertyChanged(() => Password);
			}
		}


		private ChildAllInfo _childID;

		public ChildAllInfo ChildID {
			set {
				_childID = value;

				if (_childID.Child.ChildId == 0) {
					//error

				} else {
					ShowViewModel<MainTabbedViewModel>();
				}
			}
		}

		void StoreAuthResponse(AuthenticateResponse authResponse)
		{
			var user = new UserModel();
			if (authResponse.Meta.ContainsKey("UserId"))
				user.UserId = Convert.ToInt64(authResponse.Meta["UserId"]);
			if (authResponse.Meta.ContainsKey("ChildId"))
				user.ChildId = Convert.ToInt64(authResponse.Meta["ChildId"]);
			if (authResponse.Meta.ContainsKey("KindergartenId"))
				user.KindergartenId = Convert.ToInt64(authResponse.Meta["KindergartenId"]);
			if (authResponse.Meta.ContainsKey("ClassId"))
				user.ClassId = Convert.ToInt64(authResponse.Meta["ClassId"]);
			if (authResponse.Meta.ContainsKey("RoleType"))
				user.RoleType = (RoleType)Convert.ToInt64(authResponse.Meta["RoleType"]);
			if (authResponse.Meta.ContainsKey("LoginName"))
				user.LoginName = Convert.ToString(authResponse.Meta["LoginName"]);
			if (authResponse.Meta.ContainsKey("RealName"))
				user.RealName = Convert.ToString(authResponse.Meta["RealName"]);
			if (authResponse.Meta.ContainsKey("HeadImage"))
				user.ImageName = Convert.ToString(authResponse.Meta["HeadImage"]);
			user.Kindergarten = new KindergartenModel();
			user.Kindergarten.KindergartenId = user.KindergartenId;
			if (authResponse.Meta.ContainsKey("KindergartenName")) {
				user.Kindergarten.KindergartenName = authResponse.Meta["KindergartenName"];
			} else {
				user.Kindergarten.KindergartenName = Constants.Anonymous;
			}
			user.Class = new KindergartenClassModel();
			user.Class.ClassId = user.ClassId;
			user.Class.KindergartenId = user.KindergartenId;
			if (authResponse.Meta.ContainsKey("ClassName")) {
				user.Class.ClassName = authResponse.Meta["ClassName"];
			} else {
				user.Class.ClassName = Constants.Anonymous;
			}
			user.Child = new ChildModel();
			user.Child.ClassId = user.ClassId;
			user.Child.KindergartenId = user.KindergartenId;
			if (authResponse.Meta.ContainsKey("ChildName")) {
				user.Child.ChildName = authResponse.Meta["ChildName"];
			} else {
				user.Child.ChildName = Constants.Anonymous;
			}
			if (authResponse.Meta.ContainsKey("ChildId"))
				user.Child.ChildId = Convert.ToInt64(authResponse.Meta["ChildId"]);
			if (authResponse.Meta.ContainsKey("Birthday"))
				user.Child.Birthday = Convert.ToDateTime(authResponse.Meta["Birthday"]);
			if (authResponse.Meta.ContainsKey("Gender"))
				user.Child.Gender = Convert.ToInt32(authResponse.Meta["Gender"]);
			if (authResponse.Meta.ContainsKey("HeadImage"))
				user.Child.ImageName = Convert.ToString(authResponse.Meta["HeadImage"]);
			user.Cookie = _remoteService.CookiesJsonString;
			//store in local DB
			BabyBusContext.UserAllInfo = user;
			ViewModelStatus = new ViewModelStatus("登录成功，正在跳转主页面...", false, MessageType.Information, TipsType.DialogDisappearAuto);
			ShowViewModel<MainViewModel>();
		}

		#region IWxApiDelegateAdapter implementation

		public void OnReq(BaseReqOfBabyBus req)
		{
		}

		public void OnResp(BaseRespOfBabyBus resp)
		{
			ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
			Task.Factory.StartNew(async () => {
				SendAuthRespOfBabyBus authResp = (SendAuthRespOfBabyBus)resp;
				if (authResp != null) {
					Mvx.Trace(authResp.Code);
					//send code to service and wait session
					//                0318de945d2c973d4dce3e537472d345

					if (string.IsNullOrEmpty(authResp.Code)) {
						ViewModelStatus = new ViewModelStatus("没有获得微信用户授权。", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					try {
						var authResponse = await _remoteService.LoginWithWechat(authResp.Code);
						StoreAuthResponse(authResponse);
					} catch (BabyBusWebServiceException ex) {
						ViewModelStatus = new ViewModelStatus(ex.ErrorMessage, false, MessageType.Error, TipsType.DialogDisappearAuto);
					} catch (Exception ex) {
						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
					}
				}
			});
		}

		#endregion

		private MvxCommand _loginCommand;

		public MvxCommand LoginCommand {
			get {
				_loginCommand = _loginCommand ?? new MvxCommand(async () => {
					if (string.IsNullOrEmpty(Username)) {
						ViewModelStatus = new ViewModelStatus("请输入用户名。", false, MessageType.Information, TipsType.DialogDisappearAuto);
						return;
					}
					if (string.IsNullOrEmpty(Password)) {
						ViewModelStatus = new ViewModelStatus("请输入密码。", false, MessageType.Information, TipsType.DialogDisappearAuto);
						return;
					}

					ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
					try {
						RoleType roleType;
						#if __PARENT__
						roleType = RoleType.Parent;
						#elif __TEACHER__
                            roleType = RoleType.Teacher;
						#else
						    roleType = RoleType.HeadMaster;
						#endif
						var authResponse = await _remoteService.Login(Username, Password, roleType);
						StoreAuthResponse(authResponse);
					} catch (BabyBusWebServiceException ex) {
						ViewModelStatus = new ViewModelStatus(ex.ErrorMessage, false, MessageType.Error, TipsType.DialogDisappearAuto);
					} catch (Exception ex) {
						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
					}
				});
				return _loginCommand;
			}
		}


		public MvxCommand GotoRegisterCommand {
			get {
				return new MvxCommand(() => ShowViewModel<RegisterViewModel>(new {registerViewModelType = RegisterViewModelType.RegisterNewUser}));
			}
		}

		public MvxCommand FindPasswordCommand{ get { return new MvxCommand(() => ShowViewModel<RegisterViewModel>(new {registerViewModelType = RegisterViewModelType.FindPassword})); } }
	}
}
