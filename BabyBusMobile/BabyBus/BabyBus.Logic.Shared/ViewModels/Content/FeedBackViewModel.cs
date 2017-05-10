
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBus.Logic.Shared;
using System;
using BabyBusSSApi.ServiceModel.DTO.Create;


namespace BabyBus.Logic.Shared
{
	public class FeedBackViewModel:BaseViewModel
	{
		private readonly IRemoteService _mainService;

		public FeedBackViewModel()
		{
			_mainService = Mvx.Resolve<IRemoteService>();

		}

		public override void InitData()
		{
			base.InitData();
		}

		private string _contend = null;

		public string Content {
			get { return _contend; }
			set {
				_contend = value.Trim();
				RaisePropertyChanged(() => Content);
			}
		}

		private MvxCommand _feedbackCommand;

		public MvxCommand SendFeedBackCommand {
			get {
				_feedbackCommand = _feedbackCommand ?? new MvxCommand(async () => {
					if (string.IsNullOrEmpty(Content)) {
						ViewModelStatus = new ViewModelStatus("请输入用户名。", false, MessageType.Information, TipsType.DialogDisappearAuto);
						return;
					}
					try {
						ViewModelStatus = new ViewModelStatus(UIConstants.SENDING, true, MessageType.Information, TipsType.DialogProgress);
						var feedBack = new CreateFeedback {
							Content = Content,
							UserId = (int)BabyBusContext.UserAllInfo.UserId,
							CreatTime = DateTime.Now,
							Status = 1, 
							Name = "贝贝巴士客户端",
							Type = "Mobile"
						};
						await _mainService.SendFeedback(feedBack);

						ViewModelStatus = new ViewModelStatus(UIConstants.SEND_SUCCESS, false, MessageType.Success, TipsType.DialogDisappearAuto);
					} catch (Exception ex) {
						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success, TipsType.DialogDisappearAuto);

					}
                           
				});
				return _feedbackCommand;
			}
		}
	}
}
