using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

using BabyBusSSApi.ServiceModel.DTO.Create;


namespace BabyBus.Logic.Shared
{

	public class SendFeedbackViewModel : BaseViewModel
	{
		private IRemoteService _service;

		public SendFeedbackViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();

		}

		#region Property


		private string content = string.Empty;

		public string Content {
			get { return content; }
			set {
				content = value;
				RaisePropertyChanged(() => Content);
			}
		}

		#endregion

		#region Command

		public MvxCommand SendCommand {
			get {
				return new MvxCommand(async () => {
					//Check

					if (string.IsNullOrEmpty(Content)) {
						ViewModelStatus = new ViewModelStatus("请输入问题和意见", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					if (string.IsNullOrEmpty(Content.Trim())) {
						ViewModelStatus = new ViewModelStatus("内容不能为空格", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}

					ViewModelStatus = new ViewModelStatus(UIConstants.SENDING, true, MessageType.Information, TipsType.DialogProgress);

					var question = new CreateFeedback {
						Content = Content,
						UserId = (int)BabyBusContext.UserAllInfo.UserId,
						CreatTime = DateTime.Now,
						Status = 1, 
						Name = "贝贝巴士客户端",
						Type = "Mobile"
					};
					try {
						await _service.SendFeedback(question);
                           
						ViewModelStatus = new ViewModelStatus(UIConstants.SEND_SUCCESS, false, MessageType.Success);
						Task.Delay(1000).ContinueWith(delegate(Task task) {
							this.InvokeOnMainThread(() =>
                                        Close(this));
						});
                          
					} catch (Exception ex) {

						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error,
							TipsType.DialogDisappearAuto);
					}
				});
			}
		}

		#endregion
	}
}


