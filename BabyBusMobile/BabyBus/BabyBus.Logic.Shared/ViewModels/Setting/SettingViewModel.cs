using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;
using System.IO;
using Cirrious.MvvmCross.Plugins.Messenger;
using BabyBus.Logic.Shared.Message;

namespace BabyBus.Logic.Shared
{
	public class SettingViewModel : BaseViewModel
	{
		public SettingViewModel(IMvxMessenger _messenger)
		{
			this._messenger = _messenger;
		}

		private readonly IEnvironmentService _eService;
		private readonly IRemoteService _service;
		private readonly IPictureService _picService;
		private readonly MvxSubscriptionToken _token;
		private readonly IMvxMessenger _messenger;

		public event EventHandler<Byte[]> ImageChangeEventHandler;

		public SettingViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			_eService = Mvx.Resolve<IEnvironmentService>();
			_picService = Mvx.Resolve<IPictureService>();
			_cacheSize = _eService.GetCacheSize();
			_messenger = Mvx.Resolve<IMvxMessenger>();
			LoadData();

			_token = _messenger.Subscribe<ImageBytesMessage>((message) => {
				var imagebytes = message.ImageBytes;

				if (ImageChangeEventHandler != null) {
					ImageChangeEventHandler(null, imagebytes);
				}
			});
		}

		public void LoadData()
		{
			//Load Basic Info
			RoleType = BabyBusContext.UserAllInfo.RoleType;
			if (RoleType == RoleType.Parent)
				ChildName = BabyBusContext.UserAllInfo.Child.ChildName;
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

		private string _loginName;

		public string LoginName {
			get { return _loginName; }
			set {
				_loginName = value;
				RaisePropertyChanged(() => LoginName);
			}
		}

		//iOS use this
		public string ClassNameAndLoginName {
			get { 
				if (!string.IsNullOrWhiteSpace(ClassName)) {
					return ClassName + ":" + LoginName; 
				} else {
					return LoginName;
				}
			}
		}



		#region Event

		public event EventHandler NotifyUpdateVersion;

		#endregion

		#region Property

		private float _cacheSize;

		private string _childName = string.Empty;
		private List<ChildModel> _children;

		private string _jobTitle = "教导处";
		private string _phone = "电话号码";
		private string _realName = "家长姓名";
		private string className;
		private byte[] _bytes;

		private string kindergartenName;

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
		public byte[] Bytes {
			get { return _bytes; }
			set {
				_bytes = value;
				RaisePropertyChanged(() => Bytes);
			}
		}

		public float CacheSize {
			get { return _cacheSize; }
			set {
				_cacheSize = value;
				RaisePropertyChanged(() => CacheSizeText);
			}
		}

		public string CacheSizeText {
			get {
				string sizeText = (_cacheSize / 1048576).ToString("#0.00") + "MB";
				return sizeText;
			}
		}

		public List<ChildModel> Children {
			get { return _children; }
			set {
				_children = value;
				RaisePropertyChanged(() => Children);
			}
		}

		public string JobTitle {
			get { return _jobTitle; }
			set {
				_jobTitle = value;
				RaisePropertyChanged(() => JobTitle);
			}
		}

		public string KindergartenName {
			get { return kindergartenName; }
			set {
				kindergartenName = value;
				RaisePropertyChanged(() => KindergartenName);
			}
		}

		public string ClassName {
			get { return className; }
			set {
				className = value;
				RaisePropertyChanged(() => ClassName);
			}
		}


		public RoleType RoleType { get; set; }

		public string ChildName {
			get { return _childName; }
			set {
				_childName = value;
				RaisePropertyChanged(() => ChildName);
			}
		}

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

		#region Command

		private IMvxCommand _clearCacheCommand;
		private IMvxCommand _logoutCommand;
		private IMvxCommand _showFeedbackCommand;
		private IMvxCommand _showInfoCommand;
		private IMvxCommand _showchildrenCommand;
		private IMvxCommand _updateApkCommand;
		private IMvxCommand _rePasswordCommand;
		private IMvxCommand _showAttendanceCommand;

		//iOS reset password in the setting view
		public IMvxCommand RePasswordCommand {
			get {
				_rePasswordCommand = _rePasswordCommand ??
				new MvxCommand(() => {
					ShowViewModel<RePasswordViewModel>();
				});
				return _rePasswordCommand;
			}
		}

		public IMvxCommand ShowAttendanceCommand {
			get {
				_showAttendanceCommand = _showAttendanceCommand ??
				new MvxCommand(() => {
					ShowViewModel<ChildAttendanceViewModel>();
				});
				return _showAttendanceCommand;
			}
		}



		public IMvxCommand ShowInfoCommand {
			get {
				_showInfoCommand = _showInfoCommand ?? new MvxCommand(() => ShowViewModel<InfoViewModel>());
				return _showInfoCommand;
			}
		}

		private IMvxCommand _showFeedbackViewCommand;

		public IMvxCommand ShowFeedbackViewCommand {
			get {
				_showFeedbackViewCommand = _showFeedbackViewCommand ?? new MvxCommand(() => ShowViewModel<FeedBackViewModel>());
				return _showFeedbackViewCommand;
			}
		}

		private IMvxCommand _showPaymentViewCommand;

		public IMvxCommand ShowPaymentViewCommand {
			get {
				_showPaymentViewCommand = _showPaymentViewCommand ?? new MvxCommand(() => ShowViewModel<ECPaymentViewModel>());
				return _showPaymentViewCommand;
			}
		}

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

		/// <summary>
		///     Clear Cache, Mainly Clear Image Cache
		/// </summary>
		public IMvxCommand ClearCacheCommand {
			get {
				_clearCacheCommand = _clearCacheCommand ??
				new MvxCommand(() => Task.Run(() => {
					BabyBusContext.ClearInfo();
					//_eService.ClearCache();
					CacheSize = 0;
				}));
				return _clearCacheCommand;
			}
		}

		public IMvxCommand ShowFeedbackCommand {
			get {
				_showFeedbackCommand = _showFeedbackCommand ??
				new MvxCommand(() => ShowViewModel<FeedBackViewModel>());
				return _showFeedbackCommand;
			}
		}


		public IMvxCommand ShowChildrenCommand {
			get {
				_showchildrenCommand = _showchildrenCommand ??
				new MvxCommand(() => ShowViewModel<ChildrenViewModel>());
				return _showchildrenCommand;
			}
		}

		#endregion
	}
}