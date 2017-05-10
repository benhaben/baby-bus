using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.DTO.Create;
using System.Threading.Tasks;
using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.CrossCore;


namespace BabyBus.Logic.Shared
{
	public class SendForumViewModel : BaseViewModel
	{
		readonly IRemoteService _service;

		public float Rating;

		private int postInfoid;

		private string content = string.Empty;

		readonly IMvxMessenger _messenger;

		public string Content {
			get { return content; }
			set {
				content = value;
			}
		}

		private string title = string.Empty;

		public string Title {
			get { 
				return title;
			}
			set { 
				title = value;
			}
		}

		public SendForumViewModel(IRemoteService service)
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
					if (string.IsNullOrEmpty(Title.Trim())) {
						ViewModelStatus = new ViewModelStatus("帖子标题不能为空", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					if (string.IsNullOrEmpty(Content.Trim())) {
						ViewModelStatus = new ViewModelStatus("帖子内容不能为空", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					ViewModelStatus = new ViewModelStatus(UIConstants.SENDING, true, MessageType.Information, TipsType.DialogProgress);
					string sendcontent;
					var html = "<div>" + Content + "</div>";
					var bytes = System.Text.Encoding.UTF8.GetBytes(html);
					sendcontent = Convert.ToBase64String(bytes);
					var postinfo = new  CreateEcPostInfo {
						Title = Title,
						UserId = BabyBusContext.UserAllInfo.UserId,
						ColumnType = ColumnType.Forum,
						CategoryId = 13,
						City = BabyBusContext.UserAllInfo.Kindergarten.City,
						CreateDate = DateTime.Now,
						Abstract = Content.Length < 40 ? Content : Content.Substring(0, 40),
						Html = sendcontent,
					};
					try {
						await _service.SendForum(postinfo);

						ViewModelStatus = new ViewModelStatus(UIConstants.SEND_SUCCESS, false, MessageType.Success);
						Task.Delay(1000).ContinueWith(task => this.InvokeOnMainThread(() => Close(this))).Wait();

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