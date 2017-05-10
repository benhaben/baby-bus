using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;


namespace BabyBus.Logic.Shared
{
	public class ParentModalityViewModel:BaseViewModel
	{

		readonly IRemoteService _service;
		long _currentModalityId;
		private IMvxMessenger _messenger;
		private MvxSubscriptionToken _token;

		public long CurrentModalityId {
			get {
				return _currentModalityId;
			}
			set {
				_currentModalityId = value;
			}
		}

		int _testMasterId;

		public int TestMasterId {
			get {
				return _testMasterId;
			}
			set {
				_testMasterId = value;
			}
		}


		public ParentModalityViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();

			_messenger = Mvx.Resolve<IMvxMessenger>();

			_token = _messenger.Subscribe<MITestMasterMessage>((message) => {
				Master = message.Master;
				foreach (var m in TestMasters) {
					if (m.ModalityId == Master.ModalityId) {
						m.CompletedTest = Master.CompletedTest;
					}
					if (MasterMessageChange != null) {
						MasterMessageChange(null, null);
					}
				}
			});
		}

		public event EventHandler MasterMessageChange;

		public MITestMaster Master{ get; set; }

		List<MITestMaster> _testMasters = new List<MITestMaster>();

		public List<MITestMaster> TestMasters {
			get{ return _testMasters; }
			set { 
				_testMasters = value;
			}
		}

		public override void InitData() {
			ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);

			try {
				TestMasters = _service.GetParentModality().Result;
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}

		}

		MvxCommand _showTestDetailCommand;

		public MvxCommand ShowTestDetailCommand {
			get { 
				_showTestDetailCommand = _showTestDetailCommand ?? new  MvxCommand(() =>
					ShowViewModel<MITestViewModel>(new{modalityId = CurrentModalityId,testMasterId = TestMasterId}));
				return _showTestDetailCommand;
			}
		}
	}
}

