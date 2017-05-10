using System;
using Cirrious.CrossCore;
using BabyBusSSApi.ServiceModel.DTO.Update;

namespace BabyBus.Logic.Shared
{
	public class TestPrepareViewModel : BaseViewModel
	{
		readonly IRemoteService _service;

		public TestPrepareViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		double _height;

		public double Height {
			get {
				return _height;
			}
			set {
				_height = value;
				RaisePropertyChanged(() => Height);
			}
		}

		double _weight;

		public double Weight {
			get {
				return _weight;
			}
			set {
				_weight = value;
				RaisePropertyChanged(() => Weight);
			}
		}

		string _childName;

		public string ChildName {
			get { 
				return _childName;
			}
			set { 
				_childName = value;
				RaisePropertyChanged(() => ChildName);
			}
		}

		DateTime _birthday;

		public DateTime Birthday {
			get { 
				return _birthday;
			}
			set { 
				_birthday = value;
				RaisePropertyChanged(() => Birthday);
			}
		}

		string _childBirthday;

		public string ChildBirthday { 
			get { 
				return _childBirthday; 
			} 
			set {
				_childBirthday = value;
				RaisePropertyChanged(() => ChildBirthday);
			}
		}

		int _gender;

		public int Gender {
			get { 
				return _gender;
			}
			set {
				_gender = value;
				RaisePropertyChanged(() => Gender);
			}
		}

		public PhysicalExaminationResult Model = new PhysicalExaminationResult();

		public override void InitData()
		{
			base.InitData();
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
				Model = _service.GetPhysicalExaminationResult(BabyBusContext.ChildId).Result;
				if (Model.Id == 0) {
					ViewModelStatus = new ViewModelStatus("宝宝还未体测，请联系我们", false, MessageType.Information, TipsType.DialogDisappearAuto);
				} else {
					ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
					Height = Model.Height;
					Weight = Model.Weight;
					Gender = Model.Gender;
					Birthday = Model.Birthday;
					ChildBirthday = Model.Birthday.ToString("D");
					ChildName = Model.ChildName ?? string.Empty;
				}
			} catch (Exception ex) {
				Mvx.Trace(ex.Message);
			}
		}


		public async void UpdateInfo()
		{
			if (Model.Id == 0) {
				ViewModelStatus = new ViewModelStatus("宝宝还未体测，请联系我们", false, MessageType.Information, TipsType.DialogDisappearAuto);
				return;
			}

			var update = new UpdatePhysicalExamination();
			update.Id = Model.Id;
			update.Height = Height;
			update.Weight = Weight;
			update.Birthday = Birthday;
			update.Gender = Gender;
			update.ChildName = ChildName;
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.UPDATING, true, MessageType.Information, TipsType.DialogProgress);
				var result = await _service.UpdatePhysicalExaminationResult(update);
				if (result != 0) {
					ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, true, MessageType.Success, TipsType.DialogDisappearAuto);
				} else {
					ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
				}
			} catch (Exception) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
		}
	}
}

