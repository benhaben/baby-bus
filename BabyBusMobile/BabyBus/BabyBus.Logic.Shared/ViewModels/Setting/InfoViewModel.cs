using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;
using Cirrious.MvvmCross.Plugins.Messenger;
using BabyBus.Logic.Shared.Message;



namespace BabyBus.Logic.Shared
{
	public class InfoViewModel : BaseViewModel
	{
		private readonly IRemoteService _userService;
		private readonly IMvxPictureChooserTask _pictureChooserTask;
		private readonly IPictureService _picService;
		readonly IMvxMessenger _messenger;

		public InfoViewModel()
		{
			_pictureChooserTask = Mvx.Resolve<IMvxPictureChooserTask>();
			_userService = Mvx.Resolve<IRemoteService>();
			_picService = Mvx.Resolve<IPictureService>();
			_messenger = _messenger = Mvx.Resolve<IMvxMessenger>();
		}

		public override void InitData()
		{
			base.InitData();

			//Load Data
			if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
				Birthday = BabyBusContext.UserAllInfo.Child.Birthday;
				ChildName = BabyBusContext.UserAllInfo.Child.ChildName;
				Gender = Genders.FirstOrDefault(x => x.Id == BabyBusContext.UserAllInfo.Child.Gender);
				if (Gender == null) {
					Gender = GenderModel.CreateMan();
				}
				Gender.IsSelected = true;//Default Selected
				Bytes = BabyBusContext.UserAllInfo.Child.Image;
				_user = BabyBusContext.UserAllInfo;
				_child = BabyBusContext.UserAllInfo.Child;
			} else {
				//TODO:老师园长没有生日，性别等信息，所以也无法修改
				_user = BabyBusContext.UserAllInfo;
				Bytes = BabyBusContext.UserAllInfo.Image;
			}
			LoginName = BabyBusContext.UserAllInfo.LoginName;
			RealName = BabyBusContext.UserAllInfo.RealName;

			KindergartenName = BabyBusContext.UserAllInfo.Kindergarten.KindergartenName;
			if (BabyBusContext.UserAllInfo.Class != null)
				ClassName = BabyBusContext.UserAllInfo.Class.ClassName;

			//Load Image TODO: iOS don't need this, iOS use ImageName 
//			#if __ANDROID__
//			_picService.LoadIamgeFromSource(ImageName,
//				stream => {
//					var ms = stream as MemoryStream;
//					if (ms != null)
//						Bytes = ms.ToArray();
//				});
//			#endif 
		}

		#region Property

		ChildModel _child;
		UserModel _user;

		private byte[] _bytes;

		public byte[] Bytes {
			get { return _bytes; }
			set {
				_bytes = value;
				RaisePropertyChanged(() => Bytes);
			}
		}

		private string kindergartenName;

		public string KindergartenName {
			get { return kindergartenName; }
			set {
				kindergartenName = value;
				RaisePropertyChanged(() => KindergartenName);
			}
		}

		private string className;

		public string ClassName {
			get { return className; }
			set {
				className = value;
				RaisePropertyChanged(() => ClassName);
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

		private string _phone = "电话号码";

		public string Phone {
			get { return _phone; }
			set {
				_phone = value;
				RaisePropertyChanged(() => Phone);
			}
		}

		private string _realName = string.Empty;

		public string RealName {
			get { return _realName; }
			set {
				_realName = value;
				RaisePropertyChanged(() => RealName);
			}
		}

		private string _childName = string.Empty;

		public string ChildName {
			get { return _childName; }
			set {
				_childName = value;
				RaisePropertyChanged(() => ChildName);
				//              RealName = ChildName + "家长";
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

		private GenderModel _gender;

		public GenderModel Gender {
			get { return _gender; }
			set {
				_gender = value;
				RaisePropertyChanged(() => Gender);
			}
		}

		public GenderModel SelectedGender {
			get { return _gender; }
			set {
				_gender.IsSelected = false;
				_gender = value;
				_gender.IsSelected = true;
			}
		}



		private List<GenderModel> _genders = new List<GenderModel>() {
			GenderModel.CreateMan(),
			GenderModel.CreateWoman(),
		};

		public List<GenderModel> Genders {
			get { return _genders; }
			set {
				_genders = value;
				RaisePropertyChanged(() => Genders);

			}
		}

		private DateTime _birthday;

		public DateTime Birthday {
			get { return _birthday; }
			set {
				_birthday = value;
				RaisePropertyChanged(() => Birthday);
				RaisePropertyChanged(() => BirthdayText);
			}
		}

		public string BirthdayText {
			get {
				return Birthday.ToLongDateString();
			}
		}

		#endregion

		#region Command

		private IMvxCommand _updateBirthdayCommand;

		public IMvxCommand UpdateBirthdayCommand {
			get {
				_updateBirthdayCommand = _updateBirthdayCommand ?? new MvxCommand(() => {
					BabyBusContext.UserAllInfo.Child.Birthday = Birthday;
					UpdateChildInfo(BabyBusContext.UserAllInfo.Child);
				});
				return _updateBirthdayCommand;
			}
		}

		private IMvxCommand _updateChildNameCommand;

		public IMvxCommand UpdateChildNameCommand {
			get {
				_updateChildNameCommand = _updateChildNameCommand ?? new MvxCommand(() => {
					BabyBusContext.UserAllInfo.Child.ChildName = ChildName;
					UpdateChildInfo(BabyBusContext.UserAllInfo.Child);
				});
				return _updateChildNameCommand;
			}
		}

		private IMvxCommand _updateParentNameCommand;

		public IMvxCommand UpdateParentNameCommand {
			get {
				_updateParentNameCommand = _updateParentNameCommand ?? new MvxCommand(() => {
					BabyBusContext.UserAllInfo.RealName = RealName;
					UpdateUserInfo(BabyBusContext.UserAllInfo);
                       
				});
				return _updateParentNameCommand;
			}
		}

		private IMvxCommand _updateGenderCommand;

		public IMvxCommand UpdateGenderCommand {
			get {
				_updateGenderCommand = _updateGenderCommand ?? new MvxCommand(() => {
					BabyBusContext.UserAllInfo.Child.Gender = Gender.Id;
					UpdateChildInfo(BabyBusContext.UserAllInfo.Child);
				});
				return _updateGenderCommand;
			}
		}

		private IMvxCommand _updateCommand;

		public IMvxCommand UpdateCommand {
			get {
				_updateCommand = _updateCommand ?? new MvxCommand(() => {
                   
					BabyBusContext.UserAllInfo.RealName = RealName;
					if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
						BabyBusContext.UserAllInfo.Child.ChildName = ChildName;
						BabyBusContext.UserAllInfo.Child.Birthday = Birthday;
						BabyBusContext.UserAllInfo.Child.Gender = Gender.Id;

						UpdateUserInfo(BabyBusContext.UserAllInfo);
						UpdateChildInfo(BabyBusContext.UserAllInfo.Child);
					} else {
						//TODO:老师园长没有生日，性别等信息，所以也无法修改,应该增加
						UpdateUserInfo(BabyBusContext.UserAllInfo);
					}
				});
				return _updateCommand;
			}
		}

		private IMvxCommand _logoutCommand;

		public IMvxCommand LogoutCommand {
			get {
				_logoutCommand = _logoutCommand ??
				new MvxCommand(() => {
					//Clear Info
					BabyBusContext.UserAllInfo = null;
					BabyBusContext.ClearInfo();
					_userService.Logout();
					ShowViewModel<LoginViewModel>();
					Close(this);
				});
				return _logoutCommand;
			}
		}



		private IMvxCommand _choosePictureWithCropCommand;

		public IMvxCommand ChoosePictureWithCropCommand {
			get {
				_choosePictureWithCropCommand = _choosePictureWithCropCommand ?? new MvxCommand(DoCropPicture);
				return _choosePictureWithCropCommand;
			}
		}

		private IMvxCommand _takePictureWithCropCommand;

		public IMvxCommand TakePictureWithCropCommand {
			get {
				_takePictureWithCropCommand = _takePictureWithCropCommand ?? new MvxCommand(DoCropPictureAfterTakePhoto);
				return _takePictureWithCropCommand;
			}
		}

		#endregion

		private void DoCropPictureAfterTakePhoto()
		{
			this.InvokeOnMainThread(() => _pictureChooserTask.TakePictureWithCrop(80, 80, OnPicture, null));
		}

		private void DoCropPicture()
		{
			InvokeOnMainThread(() => _pictureChooserTask.ChoosePictureWithCrop(80, 80, OnPicture, null));
		}

		public void OnPicture(Stream pictureStream)
		{
			var memoryStream = new MemoryStream();
			pictureStream.CopyTo(memoryStream);
			Bytes = memoryStream.ToArray();
			var task = Task.Factory.StartNew(async () => {
				var check = await _userService.GetChildByParentId(BabyBusContext.UserAllInfo.UserId);
				check.ImageName = GetImageName();
				check.HasHeadImage = true;
				UpdateInfo(check, Bytes);
			});
			//task.Wait();


		}

		public void UpdatePicture(byte[] Bytes)
		{
			
			var task = Task.Factory.StartNew(async () => {
				if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
					var check = await _userService.GetChildByParentId(BabyBusContext.UserAllInfo.UserId);
					check.ImageName = GetImageName();
					check.HasHeadImage = true;
					UpdateInfo(check, Bytes);
				}

			});
			task.Wait();
		}

		private void UpdateInfo(ChildModel check, byte[] bytes = null)
		{
			Task.Run(async () => {
				try {

					ViewModelStatus = new ViewModelStatus("正在更改信息...", true, MessageType.Information, TipsType.DialogProgress);
					var result = await _userService.UpdateChild(check);//1. Update Remote DB

					//TODO: 这里还是先上传其他信息再上传图片，和notice不一样
					if (result) {
						//1.Upload Image To OSS
						{
							var fileName = check.ImageName;
							await UploadImage(fileName, bytes);
							ImageName = fileName;
						}

						//2. Update Local DB
						BabyBusContext.SaveUser(); 
						var message = new ImageBytesMessage(this, bytes);
						_messenger.Publish(message);
						ViewModelStatus = new ViewModelStatus("修改信息成功！", false, MessageType.Information, TipsType.DialogDisappearAuto);
					} else {
						ViewModelStatus = new ViewModelStatus("上传图片失败", false, MessageType.Error, TipsType.DialogWithCancelButton);
					}
				} catch (Exception ex) {
					Mvx.Trace("Upate User Info Exception: " + ex.ToString());
				}
			});
		}

		ImageSendHelper _imageHelp = new ImageSendHelper();

		private ImageSendHelper ImageHelper {
			get{ return _imageHelp; }
		}

		private async Task UploadImage(string fileName, byte[] bytes)
		{
			ImageHelper.InitOssClient();
			var data = new UploadImageData(false, fileName, "还未开始上传", bytes);
			await Task.Run(() => ImageHelper.UploadImage(data));
		}

		void UpdateChildInfo(ChildModel updateChild)
		{

			Task.Run(async () => {


				ViewModelStatus = new ViewModelStatus(UIConstants.UPDATING, true, MessageType.Information, TipsType.DialogProgress);
				try {
					await _userService.UpdateChild(updateChild);

					BabyBusContext.SaveUser(); 
					ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false,
						MessageType.Success,
						TipsType.DialogDisappearAuto);
				} catch (Exception ex) {
					ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error, TipsType.DialogWithCancelButton);
				}

			});
            
		}

		private string GetImageName()
		{

			var guid = Guid.NewGuid();
			var fileName = guid + ".png";
			return fileName.ToString();
		}

		private void UpdateUserInfo(UserModel updateUser, byte[] bytes = null)
		{
			Task.Run(async () => {
                   
                       
				ViewModelStatus = new ViewModelStatus(UIConstants.UPDATING, true, MessageType.Information, TipsType.DialogProgress);
				try {
                      
					string fileName = null;
					if (bytes != null) {
						fileName = Guid.NewGuid().ToString() + ".png";
						await UploadImage(fileName, bytes);
						ImageName = fileName;
						updateUser.ImageName = fileName;
					}
                       
					await _userService.UpdateUser(updateUser);

					BabyBusContext.SaveUser(); 
					ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false,
						MessageType.Success,
						TipsType.DialogDisappearAuto);
				} catch (Exception ex) {
					ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error, TipsType.DialogWithCancelButton);
				}
                  
			});
		}

		private IMvxCommand _rePasswordCommand;

		public IMvxCommand RePasswordCommand {
			get {
				_rePasswordCommand = _rePasswordCommand ??
				new MvxCommand(() => ShowViewModel<RePasswordViewModel>());
				return _rePasswordCommand;
			}
		}
	}
}
