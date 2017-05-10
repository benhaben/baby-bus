using System;
using System.Collections.Generic;
using Cirrious.CrossCore;

namespace BabyBus.Logic.Shared
{
	public class TestHomeViewModel : BaseViewModel
	{
		private IRemoteService _service;

		public TestHomeViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}


		public override void InitData()
		{
			base.InitData();
			ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.Undisplay);
			try {
				AdvertisementModel = _service.GetAdvertisement().Result;
			} catch (Exception ex) {
				Mvx.Trace(ex.Message);
			}
		}

		public List<AdvertisementModel> AdvertisementModel{ get; set; }

		List<string> _advertisementStrList;

		public List<string> AdvertisementStrList {
			get {
				if (_advertisementStrList == null) {
					_advertisementStrList = new List<string>();
					if (AdvertisementModel == null) {
						AdvertisementModel = new List<AdvertisementModel>();
					}
					AdvertisementModel.ForEach(x => _advertisementStrList.Add(x.NormalPics));
				}
				return _advertisementStrList;
			}
		}

		public void ShowMITest()
		{
			ShowViewModel<ParentModalityViewModel>();
		}

		public void ShowPhysicalTest()
		{
			ShowViewModel<TestPrepareViewModel>();
		}

		public void ShowTemperamentTest()
		{
			ViewModelStatus = new ViewModelStatus("敬请期待！", false, MessageType.Information, TipsType.DialogDisappearAuto);
		}
	}
}

