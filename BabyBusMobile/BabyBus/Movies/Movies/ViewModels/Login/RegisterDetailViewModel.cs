using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Account;
using BabyBus.Models.Enums;
using BabyBus.Net.Login;
using BabyBus.Services;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using Cirrious.MvvmCross.ViewModels;
using System.Net;
using System.Windows.Input;
using BabyBus.Utilities;
using BabyBus.ViewModels.Login;
using BabyBus.ViewModels.Main;

namespace BabyBus.ViewModels.Login {

    public class RegisterDetailViewModel : BaseViewModel {
        private readonly ILoginService _loginService;
        private readonly IMvxPictureChooserTask _pictureChooserTask;

        public RegisterDetailViewModel() {
            //this._navigation = navigation;
            _pictureChooserTask = Mvx.Resolve<IMvxPictureChooserTask>();
            _loginService = Mvx.Resolve<ILoginService>();
        }

        public override async void Start() {
            base.Start();

            var result = await _loginService.GetAllCities();
            if (!result.Status) {
                ViewModelStatus = new ViewModelStatus(result.Message);
//				Information = result.Message;
            } else {
                var list = result.Items;
//				list.Insert (0,
//					new CityModel (0, "请选择城市"));
                Cities = list;
                City = Cities.First();
            }
        }

        #region Properties

        private ChildModel _child = new ChildModel("小米");

        public ChildModel Child {
            get { return _child; }
            set {
                _child = value;
                RaisePropertyChanged(() => Child);
            }
        }

        private string _childName = string.Empty;

        public string ChildName {
            get { return _childName; }
            set {
                _childName = value;                
                RaisePropertyChanged(() => ChildName);
                ParentName = ChildName + "的家长";
            }
        }

        private string _parentName = string.Empty;

        public string ParentName {
            get { return _parentName; }
            set {
                _parentName = value;
                RaisePropertyChanged(() => ParentName);
            }
        }

        private string _desc = string.Empty;

        public string Description {
            get { return _desc; }
            set {
                _desc = value;
                RaisePropertyChanged(() => Description);
            }
        }

      
        private DateTime _birthday = new DateTime(2010, 1, 1);

        public DateTime Birthday {
            get { return _birthday; }
            set {
                _birthday = value;
                RaisePropertyChanged(() => Birthday);
            }
        }

        private string _mibaoka;

        public string Mibaoka {
            get { return _mibaoka; }
            set {
                _mibaoka = value;
                RaisePropertyChanged(() => Mibaoka);
            }
        }

        #endregion

        #region Methods

        private async void GetKindergartenByCity(CityModel city) {
            var list = new List<KindergartenModel>();
            if (city.CityId > 0) {
                var result = await _loginService.GetKindergartenByCity(city.CityName);
                if (!result.Status) {
                    ViewModelStatus = new ViewModelStatus(result.Message);
//					Information = result.Message;
                } else {
                    list = result.Items;
                }
            }
            Kindergartens = list;
            if (list.Count == 0) {
                //iOS can't accept null
                var k = new KindergartenModel();
                k.KindergartenName = "";
                Kindergarten = k;
            } else {
                Kindergarten = Kindergartens.FirstOrDefault();
            }
        }

        private async void GetClassByKgId(KindergartenModel kg) {
            var list = new List<KindergartenClassModel>();
            if (kg != null && kg.KindergartenId > 0) {
                var result = await _loginService.GetClassByKgId(kg.KindergartenId);
                if (!result.Status) {
                    ViewModelStatus = new ViewModelStatus(result.Message);
//					Information = result.Message;
                } else {
                    list = result.Items;
                }
            }
            KindergartenClasses = list;
            if (list.Count == 0) {
                //iOS can't accept null
                var kc = new KindergartenClassModel();
                kc.ClassName = "";
                KindergartenClass = kc;
            } else {
                KindergartenClass = KindergartenClasses.FirstOrDefault();
            }
        }

        #endregion

        #region ItemSources

        //        private string _phone;
        //
        //        public string Phone {
        //            get { return _phone; }
        //            set {
        //                if (Utils.IsHandset(value)) {
        //                    _phone = value;
        //                    RaisePropertyChanged(() => Phone);
        //                } else if (value.Length > 0) {
        //                    ViewModelStatus = new ViewModelStatus("您输入手机号码有误", false, MessageType.Error, TipsType.Bubble);
        //
        //                } else {
        //                    ViewModelStatus = new ViewModelStatus("请输入手机号码", false, MessageType.Error, TipsType.Bubble);
        //                }
        //            }
        //        }


        private CityModel _city;

        public CityModel City {
            get { return _city; }
            set {
                _city = value; 
                //Get Kindergarden by city
                GetKindergartenByCity(_city); 
                RaisePropertyChanged(() => City);
            }
        }

        private List<CityModel> _cities;

        public List<CityModel> Cities {   
            get { return _cities; }
            set {
                _cities = value;
                RaisePropertyChanged(() => Cities);
            }
        }

        private GenderModel _gender = new GenderModel(1, "男");

        public GenderModel Gender {
            get { return _gender; }
            set {
                _gender = value;
                RaisePropertyChanged(() => Gender);
            }
        }

        private List<GenderModel> _genders = new List<GenderModel>() {
            new GenderModel(1, "男"),
            new GenderModel(2, "女"),
        };

        public List<GenderModel> Genders {   
            get { return _genders; }
            set {
                _genders = value;
                RaisePropertyChanged(() => Genders);
            }
        }

        private KindergartenModel _kindergarten;

        public KindergartenModel Kindergarten {
            get { return _kindergarten; }
            set {
                _kindergarten = value;
                GetClassByKgId(_kindergarten);
                RaisePropertyChanged(() => Kindergarten);
            }
        }

        private List<KindergartenModel> _kindergartens;

        public List<KindergartenModel> Kindergartens {   
            get { return _kindergartens; }
            set {
                _kindergartens = value;
                //Kindergarten = null;
                RaisePropertyChanged(() => Kindergartens);
            }
        }

        private KindergartenClassModel _kindergartenClass;

        public KindergartenClassModel KindergartenClass {
            get { return _kindergartenClass; }
            set {
                _kindergartenClass = value;
                RaisePropertyChanged(() => KindergartenClass);
            }
        }

        private List<KindergartenClassModel> _kindergartenClasses;

        public List<KindergartenClassModel> KindergartenClasses {   
            get { return _kindergartenClasses; }
            set {
                _kindergartenClasses = value;
                //KindergartenClass = null;
                RaisePropertyChanged(() => KindergartenClasses);
            }
        }

        #endregion

        #region User Interactions

        private ApiResponser _result;

        public ApiResponser Result {
            get { return _result; }
            set {
                _result = value;
                RaisePropertyChanged(() => Result);
            }
        }

        #endregion

        #region Commands

        public MvxCommand RegisterCommand {
            get {
                return new MvxCommand(async () => {
                    //Check
                    if (City == null) {
                        ViewModelStatus = new ViewModelStatus("请选择城市", false, MessageType.Error, TipsType.Bubble);
                        return;
                    }
                    if (Kindergarten.IsNull) {
                        ViewModelStatus = new ViewModelStatus("请选择幼儿园", false, MessageType.Error, TipsType.Bubble);
                        return;
                    }
                    if (KindergartenClass.IsNull) {
                        ViewModelStatus = new ViewModelStatus("请选择班级", false, MessageType.Error, TipsType.Bubble);
                        return;
                    }
                    if (ChildName == string.Empty) {
                        ViewModelStatus = new ViewModelStatus("请填写宝宝姓名", false, MessageType.Error, TipsType.Bubble);
                        return;
                    }
                    if (ParentName == string.Empty) {
                        ViewModelStatus = new ViewModelStatus("请填写家长称谓", false, MessageType.Error, TipsType.Bubble);
                        return;
                    }
                    if (ChildName.Length < 2) {
                        ViewModelStatus = new ViewModelStatus("宝宝姓名不得低于2个汉字", false, MessageType.Error, TipsType.Bubble);
                        return;
                    }
                    if (Birthday > DateTime.Now) {
                        ViewModelStatus = new ViewModelStatus("宝宝还没出生呢", false, MessageType.Error, TipsType.Bubble);
                        return;
                    }

                    ViewModelStatus = new ViewModelStatus("正在绑定...", true, MessageType.Information, TipsType.DialogProgress);

                    var check = new CheckoutModel {
                        AuditType = AuditType.Pending,
                        UserId = BabyBusContext.UserId,
                        RealName = ParentName,
                        VerifyCode = Mibaoka,
                        City = City.CityName,
                        KindergartenId = Kindergarten.KindergartenId,
                        ClassId = KindergartenClass.ClassId,
                        ChildName = ChildName,
                        Gender = Gender.Id,
                        Birthday = Birthday,
                        HasHeadImage = false,
                    };
                    if (Bytes != null) {
                        check.HasHeadImage = true;
                    }

                    Result = await _loginService.Checkout(check);

                    if (Result.Status) {
                        ViewModelStatus = new ViewModelStatus(Result.Message, false, MessageType.Information, TipsType.DialogDisappearAuto);
                        //Save Image in Local
                        if (Bytes != null) {
                            var filename = Result.Attach.ToString();
                            await UploadImage(filename);
                            Filename = filename;
                        }
                        ShowViewModel<LoginViewModel>();
                    } else {
                        ViewModelStatus = new ViewModelStatus(Result.Message, false, MessageType.Error, TipsType.DialogWithOkButton);

                    }
                });
            }
        }

        private string _filename;

        public string Filename {
            get { return _filename; }
            set {
                _filename = value;
                RaisePropertyChanged(() => Filename);
            }
        }

        ImageSendHelper _imageHelp = new ImageSendHelper();

        private ImageSendHelper ImageHelper {
            get{ return _imageHelp; }
        }

        private async Task UploadImage(string filename) {
            var data = new UploadImageData(false, filename, "还未开始上传", Bytes);
            await Task.Run(() => ImageHelper.UploadImage(data));
        }

        #endregion

        #region Picture

        private MvxCommand _choosePictureWithCropCommand;

        public MvxCommand ChoosePictureWithCropCommand {
            get {
                _choosePictureWithCropCommand = _choosePictureWithCropCommand ?? new Cirrious.MvvmCross.ViewModels.MvxCommand(DoCropPicture);
                return _choosePictureWithCropCommand;
            }
        }

        private void DoCropPicture() {
            //width and height
            this.InvokeOnMainThread(() => {
                _pictureChooserTask.ChoosePictureWithCrop(100, 100, OnPicture, null);

            });
        }

        private MvxCommand _takePictureWithCropCommand;

        public MvxCommand TakePictureWithCropCommand {
            get {
                _takePictureWithCropCommand = _takePictureWithCropCommand ?? new Cirrious.MvvmCross.ViewModels.MvxCommand(DoCropPictureAfterTakePhoto);
                return _takePictureWithCropCommand;
            }
        }

        private void DoCropPictureAfterTakePhoto() {
            //width and height
            this.InvokeOnMainThread(() => {
                _pictureChooserTask.TakePictureWithCrop(100, 100, OnPicture, null);
            });
        }

        private byte[] _bytes = null;

        public byte[] Bytes {
            get { return _bytes; }
            set {
                _bytes = value;
                RaisePropertyChanged(() => Bytes);
            }
        }

        private void OnPicture(Stream pictureStream) {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            Bytes = memoryStream.ToArray();
        }

        #endregion
    }
}
