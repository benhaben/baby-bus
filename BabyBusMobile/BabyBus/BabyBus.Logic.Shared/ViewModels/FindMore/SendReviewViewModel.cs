
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
	public class SendReviewViewModel : BaseViewModel
	{
		private readonly IRemoteService _service;
		readonly IMvxMessenger _messenger;

		public int Rating;

		private int postInfoid;
		private string content = string.Empty;

		public string Content {
			get { return content; }
			set {
				content = value;
			}
		}

		public SendReviewViewModel(IRemoteService service)
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
					//Check

//					if (Rating <= 0.0 && Rating >= 0.0) {
//						ViewModelStatus = new ViewModelStatus("综合评分不能为0！", false, MessageType.Error, TipsType.DialogDisappearAuto);
//						return;
//					}
					if (string.IsNullOrEmpty(Content.Trim())) {
						ViewModelStatus = new ViewModelStatus("评价内容不能为空", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}

					ViewModelStatus = new ViewModelStatus(UIConstants.SENDING, true, MessageType.Information, TipsType.DialogProgress);

					var review = new CreateReview {
						Content = Content,
						Rating = (int)Rating,
						PostInfoId = postInfoid,
					};
					try {
						var model = new ECReview {
							Content = Content,
							Rating = Rating,
							PostInfoId = postInfoid,
							UserId = BabyBusContext.UserId,
							RealName = BabyBusContext.UserAllInfo.RealName,
							CreateDate = DateTime.Now,
						};
						model.ReviewId = await _service.SendReview(review);

						var message = new ReviewMessage(this, model);
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