using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using Cirrious.CrossCore;
using BabyBusSSApi.ServiceModel.DTO.Reponse;


namespace BabyBus.Logic.Shared
{
	public class ReadListViewModel : BaseListViewModel
	{
		private IRemoteService _service;
		private long _noticeId;
		private  NoticeType _noticetype;

		public NoticeType Noticetype {
			get {  
				return _noticetype;
			}
			set { 
				_noticetype = value;
			}
		}

		public ReadListViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		private List<ChildModel> readers = new List<ChildModel>();

		public List<ChildModel> Readers {
			get { return readers; }
			set {
				readers = value;
				RaisePropertyChanged(() => Readers);
			}
		}

		public void Init(long noticeId, NoticeType noticetype) {
			_noticeId = noticeId;
			_noticetype = noticetype;

		}

		public override void InitData() {
			base.InitData();

			ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);

			try {
				var result = _service.GetReadersListByNoticeId(_noticeId).Result;
				Readers = result;
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}

           
		}
	}
}

