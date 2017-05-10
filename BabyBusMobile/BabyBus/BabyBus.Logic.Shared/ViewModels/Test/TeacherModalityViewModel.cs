using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Logic.Shared
{
	public class TeacherModalityViewModel :BaseViewModel
	{

		readonly IRemoteService _service;
		private IMvxMessenger _messenger;
		private int _modalityId;
		private MvxSubscriptionToken _token;

		public MITestMaster Master{ get; set; }

		List<MIModality> _testModality = new List<MIModality>();

		public List<MIModality> TestModality {
			get{ return _testModality; }
			set { 
				_testModality = value;
			}
		}

		public TeacherModalityViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			_messenger = Mvx.Resolve<IMvxMessenger>();

			_token = _messenger.Subscribe<MITestMasterMessage>((message) => {
				Master = message.Master;
				if (Master.IsFinished) {
					TestModality[(int)Master.ModalityId - 1].Completed += 1;
					if (MasterMessageChange != null) {
						MasterMessageChange(null, null);
					}
				}
			});
		}

		public event EventHandler MasterMessageChange;



		public override void InitData() {
			base.InitData();
			ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);

			try {
				TestModality = _service.GetTeacherModality().Result;
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, true, MessageType.Success, TipsType.DialogDisappearAuto);
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.Message);
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}
		}

		public void ShowDetailCommand(long modalityId) {
			ShowViewModel<MIChildrenViewModel>(new {modalityId = modalityId});
		}
	}
}

