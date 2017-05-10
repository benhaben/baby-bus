
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.DTO.Create;
using BabybusSSApi.DatabaseModel;
using System.Threading.Tasks;
using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.CrossCore;


namespace BabyBus.Logic.Shared
{
	public class SendCommentViewModel : BaseViewModel
	{
		readonly IRemoteService _service;
		readonly IMvxMessenger _messenger;


		public float Rating;

		private int postInfoid;
		private string content = string.Empty;

		public string Content {
			get { return content; }
			set {
				content = value;
			}
		}

		public SendCommentViewModel(IRemoteService service)
		{
			_service = service;
			_messenger = Mvx.Resolve<IMvxMessenger>();

		}

		#region Command

		public void Init(int id)
		{
			postInfoid = id;
		}

		public MvxCommand SendCommand {
			get {
				return new MvxCommand(async () => {
					if (string.IsNullOrEmpty(Content.Trim())) {
						ViewModelStatus = new ViewModelStatus("评价内容不能为空", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}

					ViewModelStatus = new ViewModelStatus(UIConstants.SENDING, true, MessageType.Information, TipsType.DialogProgress);

					var comment = new CreateEcComment {
						Content = Content,
						PostInfoId = postInfoid,
						CommentType = 1,
					};
					try {
						var model = new ECComment {
							Content = Content,
							PostInfoId = postInfoid,
							UserId = BabyBusContext.UserId,
							RealName = BabyBusContext.UserAllInfo.RealName,
							CreateDate = DateTime.Now,
						};

						model.CommentId = await _service.SendComment(comment);

						var message = new CommentMessage(this, model);
						_messenger.Publish(message);

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

		public void Close()
		{
			Close(this);
		}
	}
}