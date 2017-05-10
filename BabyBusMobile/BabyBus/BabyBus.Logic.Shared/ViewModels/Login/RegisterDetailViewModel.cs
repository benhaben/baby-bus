using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.PictureChooser;
using Cirrious.MvvmCross.ViewModels;


namespace BabyBus.Logic.Shared
{

	public class RegisterDetailViewModel : BaseViewModel
	{
		private readonly IRemoteService _remoteService;
		private readonly IMvxPictureChooserTask _pictureChooserTask;

		public RegisterDetailViewModel ()
		{
			//this._navigation = navigation;
			_pictureChooserTask = Mvx.Resolve<IMvxPictureChooserTask> ();
			_remoteService = Mvx.Resolve<IRemoteService> ();
		}

		public override async void Start ()
		{
			base.Start ();
			try {
				var result = await _remoteService.GetAllCities ();
				Cities = result;
				City = Cities.First ();
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus (UIConstants.PULOAD_ERROR, false, MessageType.Information, TipsType.Undisplay);
			}
		}

		#region Properties

		private ChildModel _child = new ChildModel ("小米");

		public ChildModel Child {
			get { return _child; }
			set {
				_child = value;
				RaisePropertyChanged (() => Child);
			}
		}

		private string _childName = string.Empty;

		public string ChildName {
			get { return _childName; }
			set {
				_childName = value;                
				RaisePropertyChanged (() => ChildName);
				ParentName = ChildName + "的家长";
			}
		}

		private string _parentName = string.Empty;

		public string ParentName {
			get { return _parentName; }
			set {
				_parentName = value;
				RaisePropertyChanged (() => ParentName);
			}
		}

		private string _desc = string.Empty;

		public string Description {
			get { return _desc; }
			set {
				_desc = value;
				RaisePropertyChanged (() => Description);
			}
		}

      
		private DateTime _birthday = new DateTime (2010, 1, 1);

		public DateTime Birthday {
			get { return _birthday; }
			set {
				_birthday = value;
				RaisePropertyChanged (() => Birthday);
			}
		}

		private string _mibaoka;

		public string Mibaoka {
			get { return _mibaoka; }
			set {
				_mibaoka = value;
				RaisePropertyChanged (() => Mibaoka);
			}
		}

		#endregion

		#region Methods

		private async void GetKindergartenByCity (string city)
		{
			var list = new List<KindergartenModel> ();

			if (city != null) {
				try {
					var result = await _remoteService.GetKindergartenByCity (city);
					Kindergartens = result;
					if (Kindergartens.Count == 0) {
						//iOS can't accept null
						var k = new KindergartenModel ();
						k.KindergartenName = "";
						Kindergarten = k;
					} else {
						Kindergarten = Kindergartens.FirstOrDefault ();
					}
				} catch (Exception ex) {
					ViewModelStatus = new ViewModelStatus (UIConstants.PULOAD_ERROR, false, MessageType.Information, TipsType.Undisplay);
				}
			}
		}

		private async void GetClassByKgId (KindergartenModel kg)
		{
			var list = new List<KindergartenClassModel> ();
			if (kg != null && kg.KindergartenId > 0) {

				try {
					var result = await _remoteService.GetClassByKgId (kg.KindergartenId);
					KindergartenClasses = result;
					if (list.Count == 0) {
						//iOS can't accept null
						var kc = new KindergartenClassModel ();
						kc.ClassName = "";
						KindergartenClass = kc;
					} else {
						KindergartenClass = KindergartenClasses.FirstOrDefault ();
					}
				} catch (Exception ex) {
					ViewModelStatus = new ViewModelStatus (UIConstants.PULOAD_ERROR, false, MessageType.Information, TipsType.Undisplay);
				}
			}
			KindergartenClasses = list;
			if (list.Count == 0) {
				//iOS can't accept null
				var kc = new KindergartenClassModel ();
				kc.ClassName = "";
				KindergartenClass = kc;
			} else {
				KindergartenClass = KindergartenClasses.FirstOrDefault ();
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


		private string _city;

		public string City {
			get { return _city; }
			set {
				_city = value; 
				//Get Kindergarden by city
				GetKindergartenByCity (_city); 
				RaisePropertyChanged (() => City);
			}
		}

		private List<string> _cities;

		public List<string> Cities {   
			get { return _cities; }
			set {
				_cities = value;
				RaisePropertyChanged (() => Cities);
			}
		}

		private GenderModel _gender = new GenderModel (1, "男");

		public GenderModel Gender {
			get { return _gender; }
			set {
				_gender = value;
				RaisePropertyChanged (() => Gender);
			}
		}

		private List<GenderModel> _genders = new List<GenderModel> () {
			new GenderModel (1, "男"),
			new GenderModel (2, "女"),
		};

		public List<GenderModel> Genders {   
			get { return _genders; }
			set {
				_genders = value;
				RaisePropertyChanged (() => Genders);
			}
		}

		private KindergartenModel _kindergarten;

		public KindergartenModel Kindergarten {
			get { return _kindergarten; }
			set {
				_kindergarten = value;
				GetClassByKgId (_kindergarten);
				RaisePropertyChanged (() => Kindergarten);
			}
		}

		private List<KindergartenModel> _kindergartens;

		public List<KindergartenModel> Kindergartens {   
			get { return _kindergartens; }
			set {
				_kindergartens = value;
				//Kindergarten = null;
				RaisePropertyChanged (() => Kindergartens);
			}
		}

		private KindergartenClassModel _kindergartenClass;

		public KindergartenClassModel KindergartenClass {
			get { return _kindergartenClass; }
			set {
				_kindergartenClass = value;
				RaisePropertyChanged (() => KindergartenClass);
			}
		}

		private List<KindergartenClassModel> _kindergartenClasses;

		public List<KindergartenClassModel> KindergartenClasses {   
			get { return _kindergartenClasses; }
			set {
				_kindergartenClasses = value;
				//KindergartenClass = null;
				RaisePropertyChanged (() => KindergartenClasses);
			}
		}

		private string GetImageName ()
		{

			var guid = Guid.NewGuid ();
			var fileName = guid + ".png";
			return fileName.ToString ();
		}

		#endregion

		#region Commands

		public MvxCommand RegisterCommand {
			get {
				return new MvxCommand (async () => {
					//Check
					if (City == null) {
						ViewModelStatus = new ViewModelStatus ("请选择城市", false, MessageType.Error, TipsType.Bubble);
						return;
					}
					if (Kindergarten.IsNull) {
						ViewModelStatus = new ViewModelStatus ("请选择幼儿园", false, MessageType.Error, TipsType.Bubble);
						return;
					}
					if (KindergartenClass.IsNull) {
						ViewModelStatus = new ViewModelStatus ("请选择班级", false, MessageType.Error, TipsType.Bubble);
						return;
					}
					if (ChildName == string.Empty) {
						ViewModelStatus = new ViewModelStatus ("请填写宝宝姓名", false, MessageType.Error, TipsType.Bubble);
						return;
					}
					if (ParentName == string.Empty) {
						ViewModelStatus = new ViewModelStatus ("请填写家长称谓", false, MessageType.Error, TipsType.Bubble);
						return;
					}
					if (ChildName.Length < 2) {
						ViewModelStatus = new ViewModelStatus ("宝宝姓名不得低于2个汉字", false, MessageType.Error, TipsType.Bubble);
						return;
					}
					if (ChildName.Length > 5) {
						ViewModelStatus = new ViewModelStatus ("宝宝姓名不能超过5个汉字", false, MessageType.Error, TipsType.Bubble);
						return;
					}

					if (Birthday > DateTime.Now) {
						ViewModelStatus = new ViewModelStatus ("宝宝还没出生呢", false, MessageType.Error, TipsType.Bubble);
						return;
					}

					ViewModelStatus = new ViewModelStatus ("正在绑定...", true, MessageType.Information, TipsType.DialogProgress);

					var register = new RegisterDetialModel {

						UserName = ParentName,
						City = City,
						KindergartenId = Kindergarten.KindergartenId,
						ClassId = KindergartenClass.ClassId,
						ChildName = ChildName,
						Gender = Gender.Id,
						ChildBirthdy = Birthday,
						HasHeadImage = GetImageName (),
					};



					try {
						var result = await _remoteService.RegisterDetial (register);
						if (!result) {
							ViewModelStatus = new ViewModelStatus ("更新信息失败", false, MessageType.Information, TipsType.Undisplay);
						}
					} catch (Exception ex) {
						ViewModelStatus = new ViewModelStatus (UIConstants.PULOAD_ERROR, false, MessageType.Information, TipsType.Undisplay);
					}
					
					if (Bytes != null) {
						var filename = register.HasHeadImage;
						await UploadImage (filename);
						Filename = filename;
					}
					ShowViewModel<LoginViewModel> (); 
				});
			}
		}

		private string _filename;

		public string Filename {
			get { return _filename; }
			set {
				_filename = value;
				RaisePropertyChanged (() => Filename);
			}
		}

		ImageSendHelper _imageHelp = new ImageSendHelper ();

		private ImageSendHelper ImageHelper {
			get{ return _imageHelp; }
		}

		private async Task UploadImage (string filename)
		{
			var data = new UploadImageData (false, filename, "还未开始上传", Bytes);
			await Task.Run (() => ImageHelper.UploadImage (data));
		}

		#endregion

		#region Picture

		private MvxCommand _choosePictureWithCropCommand;

		public MvxCommand ChoosePictureWithCropCommand {
			get {
				_choosePictureWithCropCommand = _choosePictureWithCropCommand ?? new Cirrious.MvvmCross.ViewModels.MvxCommand (DoCropPicture);
				return _choosePictureWithCropCommand;
			}
		}

		private void DoCropPicture ()
		{
			//width and height
			this.InvokeOnMainThread (() => {
				_pictureChooserTask.ChoosePictureWithCrop (100, 100, OnPicture, null);

			});
		}

		private MvxCommand _takePictureWithCropCommand;

		public MvxCommand TakePictureWithCropCommand {
			get {
				_takePictureWithCropCommand = _takePictureWithCropCommand ?? new Cirrious.MvvmCross.ViewModels.MvxCommand (DoCropPictureAfterTakePhoto);
				return _takePictureWithCropCommand;
			}
		}

		private void DoCropPictureAfterTakePhoto ()
		{
			//width and height
			this.InvokeOnMainThread (() => {
				_pictureChooserTask.TakePictureWithCrop (100, 100, OnPicture, null);
			});
		}

		private byte[] _bytes = null;

		public byte[] Bytes {
			get { return _bytes; }
			set {
				_bytes = value;
				RaisePropertyChanged (() => Bytes);
			}
		}

		private void OnPicture (Stream pictureStream)
		{
			var memoryStream = new MemoryStream ();
			pictureStream.CopyTo (memoryStream);
			Bytes = memoryStream.ToArray ();
		}

		#endregion
	}
}
