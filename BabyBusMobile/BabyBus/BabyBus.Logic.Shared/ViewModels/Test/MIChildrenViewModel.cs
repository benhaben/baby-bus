using System;

using System.Collections.Generic;


using Cirrious.CrossCore;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.MvvmCross.Plugins.Messenger;



namespace BabyBus.Logic.Shared
{
	public class MIChildrenViewModel : BaseViewModel
	{
		readonly IRemoteService _service;
		private IMvxMessenger _messenger;
		private int _modalityId;
		private MvxSubscriptionToken _token;

		public MITestMaster Master{ get; set; }

		public event EventHandler MasterMessageChange;


		public void Init(int modalityId) {
			_modalityId = modalityId;
		}

		List<MITestMaster> _testMasters = new List<MITestMaster>();

		public List<MITestMaster> TestMasters {
			get{ return _testMasters; }
			set { 
				_testMasters = value;
			}
		}

       
		public MIChildrenViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			_messenger = Mvx.Resolve<IMvxMessenger>();

			_token = _messenger.Subscribe<MITestMasterMessage>((message) => {
				Master = message.Master;
				foreach (var m in TestMasters) {
					if (m.ChildId == Master.ChildId) {
						m.CompletedTest = Master.CompletedTest;
						m.IsFinished = Master.IsFinished;
					}
				}
				if (MasterMessageChange != null) {
					MasterMessageChange(null, null);
				}
			});
		}

		public override void InitData() {
			base.InitData();
			ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
           
			try {
				_testMasters = _service.GetModalityChild(_modalityId, (int)BabyBusContext.ClassId).Result;
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Information, TipsType.Undisplay);
			} catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex.Message);
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}

               
		}

		public void ShowDetailCommand(long modalityId, int testMasterId, long childId) {
			ShowViewModel<MITestViewModel>(new{ modalityId = modalityId,testMasterId = testMasterId,childId = childId});

		}
	}
}

