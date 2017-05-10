using System;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;
using System.IO;
using Cirrious.MvvmCross.Plugins.Messenger;
using BabyBus.Logic.Shared.Message;

namespace BabyBus.Logic.Shared
{
	public class SettingMainViewModel : BaseViewModel
	{
		private readonly IEnvironmentService _eService;
		private readonly IRemoteService _service;
		private readonly IPictureService _picService;
		private readonly MvxSubscriptionToken _token;
		private readonly IMvxMessenger _messenger;

		public event EventHandler<Byte[]> ImageChangeEventHandler;

		public SettingMainViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			_eService = Mvx.Resolve<IEnvironmentService>();
			_picService = Mvx.Resolve<IPictureService>();
			_cacheSize = _eService.GetCacheSize();
			_messenger = Mvx.Resolve<IMvxMessenger>();

			_token = _messenger.Subscribe<ImageBytesMessage>((message) => {
				var imagebytes = message.ImageBytes;

				if (ImageChangeEventHandler != null) {
					ImageChangeEventHandler(null, imagebytes);
				}
			});
			LoadData();


		}

		public void LoadData()
		{
			//Load Basic Info

			RealName = BabyBusContext.UserAllInfo.RealName;
			Phone = BabyBusContext.UserAllInfo.LoginName;
			KindergartenName = BabyBusContext.UserAllInfo.Kindergarten.KindergartenName;
			LoginName = BabyBusContext.UserAllInfo.LoginName;

			if (BabyBusContext.UserAllInfo.Class != null)
				ClassName = BabyBusContext.UserAllInfo.Class.ClassName;
			//Load Image TODO: iOS don't need this, iOS use ImageName 
			#if __ANDROID__
			_picService.LoadIamgeFromSource(ImageName,
				stream => {
					var ms = stream as MemoryStream;
					if (ms != null)
						Bytes = ms.ToArray();
					
				});
			#endif 
		}

		public byte[] Bytes {
			get { return BabyBusContext.UserAllInfo.Child.Image; }
			set {
				BabyBusContext.UserAllInfo.Child.Image = value;
				RaisePropertyChanged(() => Bytes);
			}
		}

		private string _loginName;

		public string LoginName {
			get { return _loginName; }
			set {
				_loginName = value;
				RaisePropertyChanged(() => LoginName);
			}
		}

		#region Property

		private float _cacheSize;

		private string _childName = string.Empty;

		public string ImageName {
			get { 
				if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
					return BabyBusContext.UserAllInfo.Child.ImageName;
				} else {
					return BabyBusContext.UserAllInfo.ImageName;
				}
			}
			set { 
				if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
					BabyBusContext.UserAllInfo.Child.ImageName = value;
				} else {
					BabyBusContext.UserAllInfo.ImageName = value;
				}
				RaisePropertyChanged(() => ImageName);
			}
		}
		//Head Image Bytes Source

		public string ChildCity {
			get { 
				return BabyBusContext.UserAllInfo.Kindergarten.City;
			}
			set {
				BabyBusContext.UserAllInfo.Kindergarten.City = "未设置地址";
				RaisePropertyChanged(() => ChildCity);
			}
		}

		public string ChildName {
			get {
				return BabyBusContext.UserAllInfo.Child.ChildName;	
			}
			set { 
				BabyBusContext.UserAllInfo.Child.ChildName = value;
				RaisePropertyChanged(() => ChildName);
			}
		}

		public string KindergartenName {
			get { return BabyBusContext.UserAllInfo.Kindergarten.KindergartenName; }
			set {
				BabyBusContext.UserAllInfo.Kindergarten.KindergartenName = value;
				if (KindergartenName == null) {
					KindergartenName = "未设置幼儿园";
				}
				RaisePropertyChanged(() => KindergartenName);
			}
		}



		//iOS use this

		private string className;

		public string ClassName {
			get { return className; }
			set {
				className = value;
				RaisePropertyChanged(() => ClassName);
			}
		}

		public string ClassNameAndLoginName {
			get { 
				if (!string.IsNullOrWhiteSpace(ClassName)) {
					return ClassName + ":" + LoginName; 
				} else {
					return LoginName;
				}
			}
		}

		string _phone = "电话号码";
		private string _realName = "家长姓名";

		public string RealName {
			get { return _realName; }
			set {
				_realName = value;
				RaisePropertyChanged(() => RealName);
			}
		}

		public string Phone {
			get { return _phone; }
			set {
				_phone = value;
				RaisePropertyChanged(() => Phone);
			}
		}


		public VersionModel Version { get; set; }

		#endregion

		#region Event

		public event EventHandler NotifyUpdateVersion;

		#endregion

		#region Command

		private IMvxCommand _ShowInfoViewCommand;

		public IMvxCommand ShowInfoViewCommand {
			get { 
				_ShowInfoViewCommand = _ShowInfoViewCommand ??
				new MvxCommand(() => ShowViewModel<InfoViewModel>());
				return _ShowInfoViewCommand;
			}
		}

		private IMvxCommand _ShowMyPayOrderCommand;

		public IMvxCommand ShowPayOrderCommand {
			get { 
				_ShowMyPayOrderCommand = _ShowMyPayOrderCommand ??
				new MvxCommand(() => ShowViewModel<SettingPayOrderViewModel>());
				return _ShowMyPayOrderCommand;
			}
		}

		private IMvxCommand _ShowPostCommentCommand;

		public IMvxCommand ShowPostCommentCommand {
			get { 
				_ShowPostCommentCommand = _ShowPostCommentCommand ??
				new MvxCommand(() => ShowViewModel<SettingCommentViewModel>());
				return _ShowPostCommentCommand;
			}
		}

		private IMvxCommand _showFeedbackViewCommand;

		public IMvxCommand ShowFeedbackCommand {
			get {
				_showFeedbackViewCommand = _showFeedbackViewCommand ?? new MvxCommand(() => ShowViewModel<SendFeedbackViewModel>());
				return _showFeedbackViewCommand;
			}
		}

		private IMvxCommand _showPaymentViewCommand;

		public IMvxCommand ShowPaymentViewCommand {
			get {
				_showPaymentViewCommand = _showPaymentViewCommand ?? new MvxCommand(() => ShowViewModel<SettingPaymentViewModel>());
				return _showPaymentViewCommand;
			}
		}

		private IMvxCommand _updateApkCommand;

		public IMvxCommand UpdateApkCommand {
			get {
				_updateApkCommand = _updateApkCommand ??
				new MvxCommand(async () => {
					ViewModelStatus = new ViewModelStatus("检查版本...", true, MessageType.Information,
						TipsType.DialogDisappearAuto);
					var appType = (int)AppType;
					Version = await _service.GetVersionByAppType(appType);
					if (Version != null && Version.VerCode > VerCode) {
						ViewModelStatus = new ViewModelStatus("");
						//Notify UI
						if (NotifyUpdateVersion != null)
							NotifyUpdateVersion(this, null);
					} else {
						ViewModelStatus = new ViewModelStatus("目前是最新版本，感谢您的支持", false,
							MessageType.Information, TipsType.DialogDisappearAuto);
					}
				});
				return _updateApkCommand;
			}
		}

		private IMvxCommand _showAttendanceCommand;


		public IMvxCommand ShowAttendanceCommand {
			get {
				_showAttendanceCommand = _showAttendanceCommand ??
				new MvxCommand(() => {
					ShowViewModel<ChildAttendanceViewModel>();
				});
				return _showAttendanceCommand;
			}
		}

		IMvxCommand _rePasswordCommand;

		public IMvxCommand RePasswordCommand {
			get {
				_rePasswordCommand = _rePasswordCommand ??
				new MvxCommand(() => {
					ShowViewModel<RePasswordViewModel>();
				});
				return _rePasswordCommand;
			}
		}

		IMvxCommand _logoutCommand;

		public IMvxCommand LogoutCommand {
			get {
				_logoutCommand = _logoutCommand ??
				new MvxCommand(() => {
					//Clear Info
					BabyBusContext.UserAllInfo = null;
					BabyBusContext.ClearInfo();
					_service.Logout();
					ShowViewModel<LoginViewModel>();
					Close(this);
				});
				return _logoutCommand;
			}
		}

		#endregion
	}
}