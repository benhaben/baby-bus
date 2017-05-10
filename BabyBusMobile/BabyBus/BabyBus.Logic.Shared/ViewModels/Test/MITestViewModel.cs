using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.DTO.Create;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Logic.Shared
{
	public class MITestViewModel : BaseViewModel
	{
		readonly IRemoteService _service;
		long _modalityId;
		long _testMasterId;
		long _childId;
		IMvxMessenger _messenger;

		public MITestViewModel(IMvxMessenger messenger)
		{
			_service = Mvx.Resolve<IRemoteService>();
			_messenger = messenger;
		}

		public void Init(long modalityId, long testMasterId, long childId)
		{
			_modalityId = modalityId;
			_testMasterId = testMasterId;
			_childId = childId == 0L ? BabyBusContext.ChildId : childId;
		}

		public override void InitData()
		{
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
				AssessIndexList = _service.GetTestDetail(BabyBusContext.RoleType, _modalityId, _childId).Result;
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}
		}

		private List<MIAssessIndex> _assessIndexList = new List<MIAssessIndex>();

		public List<MIAssessIndex> AssessIndexList {
			get{ return _assessIndexList; }
			set{ _assessIndexList = value; }
		}

		MvxCommand _sendQuestions;

		public MvxCommand SendQuestions {
			get { 
				return new MvxCommand(
					async () => {

						ViewModelStatus = new ViewModelStatus(UIConstants.SENDING, true, MessageType.Information, TipsType.DialogProgress);

						var model = new CreateMiTestDetail();
						model.ChildId = _childId;
						model.ModalityId = _modalityId;
						model.UserId = BabyBusContext.UserId;
						model.TestRoleType = (int)BabyBusContext.RoleType;
						model.TestMasterId = _testMasterId;

						model.TestDetail = new List<MITestQuestion>();
						var templist = new List<MITestQuestion>();

						var master = new MITestMaster();
						foreach (var aiItem in AssessIndexList) {
							templist.AddRange(aiItem.MITestList);
						}
						//Add Score>0 to Test Question List
						foreach (var question in templist) {
							if (question.Score > 0) {
								model.TestDetail.Add(question);
								master.CompletedTest += 1;
							}
							master.TotalTest += 1;
						}
						master.ModalityId = _modalityId;
						master.ChildId = _childId;
						master.IsFinished = master.TotalTest == master.CompletedTest;



						try {
							await _service.SendTestQuestions(model);
							ViewModelStatus = new ViewModelStatus(UIConstants.SEND_SUCCESS, false, MessageType.Success);

							var message = new MITestMasterMessage(this, master);
							_messenger.Publish(message);
							Redirect();
						} catch (Exception ex) {
							ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success);
						}
					}
				);
			}
		}

		void Redirect()
		{
			Task.Delay(1000).ContinueWith(task => {
				this.InvokeOnMainThread(() => {
					Close(this);
				});
			});
		}
	}
}

