using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;
using System.IO;




namespace BabyBus.Logic.Shared
{
	public class ParentHomeViewModel : BaseViewModel
	{
		private IRemoteService _service;
		readonly IPictureService _picService;

		public ParentHomeViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			_picService = Mvx.Resolve<IPictureService>();

		}


		public override  void InitData()
		{
			AdvertisementModel = GetNewADvertisementlist().Result;

			DisplayCity = BabyBusContext.UserAllInfo.Kindergarten.City;
			if (DisplayCity == null) {
				DisplayCity = "--/ -- ";
			}
			DisplayKindergarten = BabyBusContext.UserAllInfo.Kindergarten.KindergartenName;
			if (DisplayKindergarten == null) {
				DisplayKindergarten = "虚拟印象幼儿园";
			}
			DisplayChildName = BabyBusContext.UserAllInfo.Child.ChildName;
			if (DisplayChildName == null) {
				DisplayChildName = "宝宝贝";
			}
			DisplayClassName = BabyBusContext.UserAllInfo.Class.ClassName;
			if (DisplayClassName == null) {
				DisplayClassName = "小森林幼儿园无敌一班";
			}
			DisplayTeacherName = BabyBusContext.UserAllInfo.Class.Teacher;
			if (DisplayTeacherName == null) {
				DisplayTeacherName = "梧桐老师";
			}
			#if __ANDROID__
			_picService.LoadIamgeFromSource(ImageName,
				stream => {
					var ms = stream as MemoryStream;
					if (ms != null)
						Bytes = ms.ToArray();
				});
			#endif
		}
		private async Task<List<AdvertisementModel>> GetNewADvertisementlist()
		{
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.Undisplay);

				var list = await _service.GetAdvertisement();

				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				return list;

			} catch (Exception ) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
				return null;
			}
		}

		#region Property

		public List<AdvertisementModel> AdvertisementModel{ get; set; }

		List<string> _advertisementStrList;

		public List<string> AdvertisementStrList {
			get {
				if (_advertisementStrList == null) {
					_advertisementStrList = new List<string>();
					AdvertisementModel.ForEach(x => _advertisementStrList.Add(x.NormalPics));
				}
				return _advertisementStrList;
			}
		}

		private string city;

		public string DisplayCity {
			get { 
				return city;
			}
			set { 
				city = value;
				RaisePropertyChanged(() => DisplayCity);
			}
		}

		private string kindergarten;

		public string DisplayKindergarten {
			get { 
				return kindergarten;
			}
			set { 
				kindergarten = value;
				RaisePropertyChanged(() => DisplayKindergarten);
			}
		}

		private string childNmae;

		public string DisplayChildName {
			get { 
				return childNmae;
			}
			set { 
				childNmae = value;
				RaisePropertyChanged(() => DisplayChildName);
			}
		}

		private string className;

		public string DisplayClassName {
			get { 
				return className;
			}
			set { 
				className = value;
				RaisePropertyChanged(() => DisplayClassName);
			}
		}

		private string teacherName;

		public string DisplayTeacherName {
			get { 
				return teacherName;
			}
			set { 
				teacherName = value;
				RaisePropertyChanged(() => DisplayTeacherName);
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
		//Head Image Bytes Source
		private byte[] _bytes;

		public byte[] Bytes {
			get { return _bytes; }
			set {
				_bytes = value;
				RaisePropertyChanged(() => Bytes);
			}
		}

		#endregion

	}
}
