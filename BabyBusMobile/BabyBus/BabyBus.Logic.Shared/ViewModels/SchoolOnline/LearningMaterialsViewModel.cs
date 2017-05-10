using System;
using Cirrious.CrossCore;
using System.Collections.Generic;

namespace BabyBus.Logic.Shared
{
	public class LearningMaterialsViewModel : BaseViewModel
	{
		IRemoteService _service;

		public bool PaymentStatus { get; set; }

		public LearningMaterialsViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		public List<AlbumModel> Albums{ get; set; }

		public override void InitData()
		{
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, IsFirst ? TipsType.DialogProgress : TipsType.Undisplay);
				PaymentStatus = _service.GetPaymentStatus(PaymentType.Album).Result;
				Albums = _service.GetAlbums().Result;
				IsFirst = false;
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
		}

		public void ShowPaymentView()
		{
			ShowViewModel<ECPaymentViewModel>(new {PaymentType = PaymentType.Album});
		}
	}
}

