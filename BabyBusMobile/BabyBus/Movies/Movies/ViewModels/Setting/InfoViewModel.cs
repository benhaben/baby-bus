using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Account;
using BabyBus.Models.Enums;
using BabyBus.Net.Login;
using BabyBus.Services;
using BabyBus.Utilities;
using BabyBus.ViewModels.Main;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Setting {
    public class InfoViewModel : BaseViewModel {
        private readonly IUserInfoService _userService;
        private readonly IMvxPictureChooserTask _pictureChooserTask;

        public InfoViewModel() {
            _pictureChooserTask = Mvx.Resolve<IMvxPictureChooserTask>();
            _userService = Mvx.Resolve<IUserInfoService>();
        }

        public override void Start() {
            base.Start();
            //Load Data
            if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
                Birthday = BabyBusContext.UserAllInfo.Child.Birthday;
                ChildName = BabyBusContext.UserAllInfo.Child.ChildName;
                Gender = Genders.FirstOrDefault(x => x.Id == BabyBusContext.UserAllInfo.Child.Gender);
                if (Gender == null) {
                    Gender = GenderModel.CreateMan();
                }
                Gender.IsSelected = true;//Default Selected
            } else {
                //TODO:老师园长没有生日，性别等信息，所以也无法修改
            }
            LoginName = BabyBusContext.UserAllInfo.LoginName;
            RealName = BabyBusContext.UserAllInfo.RealName;

            KindergartenName = BabyBusContext.UserAllInfo.Kindergarten.KindergartenName;
            if (BabyBusContext.UserAllInfo.Class != null)
                ClassName = BabyBusContext.UserAllInfo.Class.ClassName;

            //Load Iamge
            try {
                var pic = Mvx.Resolve<IPictureService>();
                pic.LoadIamgeFromSource(ImageName,
                    stream => {
                        var ms = stream as MemoryStream;
                        if (ms != null)
                            Bytes = ms.ToArray();
                    });
            } catch (Exception ex) {
                MvxTrace.Trace("Image Loading Exception:" + ex.Message);
            }
        }

        #region Property

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

        private byte[] _bytes;

        public byte[] Bytes {
            get { return _bytes; }
            set {
                _bytes = value;
                RaisePropertyChanged(() => Bytes);
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
                //              RealName = ChildName + "的家长";
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

        private DateTime _birthday = DateTime.Now;

        public DateTime Birthday {
            get { return _birthday; }
            set {
                _birthday = value;
                RaisePropertyChanged(() => Birthday);
            }
        }

        #endregion

        #region Command

        private IMvxCommand _updateBirthdayCommand;

        public IMvxCommand UpdateBirthdayCommand {
            get {
                _updateBirthdayCommand = _updateBirthdayCommand ?? new MvxCommand(() => {
                    var check = new CheckoutModel {
                        ChildId = BabyBusContext.UserAllInfo.ChildId,
                        Birthday = Birthday,
                    };
                    BabyBusContext.UserAllInfo.Child.Birthday = Birthday;
                    UpdateInfo(check);
                });
                return _updateBirthdayCommand;
            }
        }

        private IMvxCommand _updateChildNameCommand;

        public IMvxCommand UpdateChildNameCommand {
            get {
                _updateChildNameCommand = _updateChildNameCommand ?? new MvxCommand(() => {
                    var check = new CheckoutModel {
                        ChildId = BabyBusContext.UserAllInfo.ChildId,
                        ChildName = ChildName,
                    };
                    BabyBusContext.UserAllInfo.Child.ChildName = ChildName;
                    UpdateInfo(check);
                });
                return _updateChildNameCommand;
            }
        }

        private IMvxCommand _updateParentNameCommand;

        public IMvxCommand UpdateParentNameCommand {
            get {
                _updateParentNameCommand = _updateParentNameCommand ?? new MvxCommand(() => {
                    var check = new CheckoutModel {
                        UserId = BabyBusContext.UserAllInfo.UserId,
                        RealName = RealName,
                    };
                    BabyBusContext.UserAllInfo.RealName = RealName;
                    UpdateInfo(check);
                });
                return _updateParentNameCommand;
            }
        }

        private IMvxCommand _updateGenderCommand;

        public IMvxCommand UpdateGenderCommand {
            get {
                _updateGenderCommand = _updateGenderCommand ?? new MvxCommand(() => {
                    var check = new CheckoutModel {
                        ChildId = BabyBusContext.UserAllInfo.ChildId,
                        Gender = Gender.Id,
                    };
                    BabyBusContext.UserAllInfo.Child.Gender = Gender.Id;
                    UpdateInfo(check);
                    RaisePropertyChanged(() => Gender);
                });
                return _updateGenderCommand;
            }
        }

        private IMvxCommand _updateCommand;

        public IMvxCommand UpdateCommand {
            get {
                _updateCommand = _updateCommand ?? new MvxCommand(() => {
                    CheckoutModel check;
                   
                    BabyBusContext.UserAllInfo.RealName = RealName;
                    if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
                        BabyBusContext.UserAllInfo.Child.ChildName = ChildName;
                        BabyBusContext.UserAllInfo.Child.Birthday = Birthday;
                        BabyBusContext.UserAllInfo.Child.Gender = Gender.Id;
                        check = new CheckoutModel {
                            ChildId = BabyBusContext.UserAllInfo.ChildId,
                            Gender = Gender.Id,
                            ChildName = ChildName,
                            RealName = RealName,
                            Birthday = Birthday,
                        };
                    } else {
                        //TODO:老师园长没有生日，性别等信息，所以也无法修改
                        check = new CheckoutModel {
                            RealName = RealName,
                        };
                    }
                    UpdateInfo(check);
                });
                return _updateCommand;
            }
        }

        #endregion

        private IMvxCommand _choosePictureWithCropCommand;

        public IMvxCommand ChoosePictureWithCropCommand {
            get {
                _choosePictureWithCropCommand = _choosePictureWithCropCommand ?? new Cirrious.MvvmCross.ViewModels.MvxCommand(DoCropPicture);
                return _choosePictureWithCropCommand;
            }
        }

        private IMvxCommand _takePictureWithCropCommand;

        public IMvxCommand TakePictureWithCropCommand {
            get {
                _takePictureWithCropCommand = _takePictureWithCropCommand ?? new Cirrious.MvvmCross.ViewModels.MvxCommand(DoCropPictureAfterTakePhoto);
                return _takePictureWithCropCommand;
            }
        }

        private void DoCropPictureAfterTakePhoto() {
            this.InvokeOnMainThread(() => {
                _pictureChooserTask.TakePictureWithCrop(80, 80, OnPicture, null);
            });
        }

        private void DoCropPicture() {
            this.InvokeOnMainThread(() => {
                _pictureChooserTask.ChoosePictureWithCrop(80, 80, OnPicture, null);
            });
        }

        private void OnPicture(Stream pictureStream) {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            Bytes = memoryStream.ToArray();
            //Upload Image
            Task.Run(async () => {
                var check = new CheckoutModel {
                    ChildId = BabyBusContext.UserAllInfo.ChildId,
                    HasHeadImage = true
                };
                UpdateInfo(check);
            });
        }

        private void UpdateInfo(CheckoutModel check) {
            Task.Run(async () => {
                try {
                    check.CheckoutType = CheckoutType.UpdateAll;
                    check.UserId = BabyBusContext.UserAllInfo.UserId;
                    check.RoleType = BabyBusContext.UserAllInfo.RoleType;
                    ViewModelStatus = new ViewModelStatus("正在更改信息...", true, MessageType.Information, TipsType.DialogProgress);
                    var result = await _userService.UpdateUserInfo(check);//1. Update Remote DB

                    if (result.Status) {
                        //1.Upload Image To OSS
                        if (check.CheckoutType == CheckoutType.UploadImage || check.CheckoutType == CheckoutType.UpdateAll) {
                            if (result.Attach != null) {
                                var fileName = result.Attach.ToString();
                                await UploadImage(fileName);
                                ImageName = fileName;
                            }
                        }

                        //2. Update Local DB
                        BabyBusContext.SaveUser(); 

                        ViewModelStatus = new ViewModelStatus("修改信息成功！", false,
                            MessageType.Information,
                            TipsType.DialogDisappearAuto);
                    } else {
                        ViewModelStatus = new ViewModelStatus(result.Message, false, MessageType.Error, TipsType.DialogWithCancelButton);
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

        private async Task UploadImage(string fileName) {
            var data = new UploadImageData(false, fileName, "还未开始上传", Bytes);
            await Task.Run(() => ImageHelper.UploadImage(data));
        }

        private IMvxCommand _rePasswordCommand;

        public IMvxCommand RePasswordCommand {
            get {
                _rePasswordCommand = _rePasswordCommand ??
                new MvxCommand(() => {
                    ShowViewModel<RePasswordViewModel>();
                });
                return _rePasswordCommand;
            }
        }
    }
}
