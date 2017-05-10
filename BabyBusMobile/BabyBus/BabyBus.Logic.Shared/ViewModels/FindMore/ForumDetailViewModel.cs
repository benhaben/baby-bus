using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Threading.Tasks;
using BabyBusSSApi.ServiceModel.DTO.Create;
using Cirrious.MvvmCross.Plugins.Messenger;
using System.Collections.Generic;
using System.Text;

namespace BabyBus.Logic.Shared
{
	public class ForumDetailViewModel:BaseListViewModel
	{
		private int postInfoId;
		IRemoteService _service = null;
		MvxSubscriptionToken _token;
		IMvxMessenger _messenger;
		private ECPostInfo postInfo;

		public ECPostInfo PostInfo {
			get { 
				return postInfo ?? new ECPostInfo();
			}
			set { 
				postInfo = value;
			}
		}


		public string Description {
			get;
			set;
		}

		private string userComments = "圈子评价：";

		public string UserComment {
			get { 
				return userComments;
			}
			set { 
				userComments = value;
				RaisePropertyChanged(() => UserComment);
			}
		}

		private string postInfoTile = "正在加载……";

		public string PostInfoTiTle {
			get { 
				return postInfoTile;
			}
			set { 
				postInfoTile = value;
				RaisePropertyChanged(() => PostInfoTiTle);
			}
		}

		private string praise;

		public string Praise {
			get { 
				return praise;
			}
			set { 
				praise = value;
				RaisePropertyChanged(() => Praise);
			}
		}

		private string comment;

		public string Comment {
			get { 
				return comment;
			}
			set { 
				comment = value;
				RaisePropertyChanged(() => Comment);
			}
		}

		private string realName;

		public string RealName {
			get { 
				return realName;
			}
			set { 
				realName = value;
				RaisePropertyChanged(() => RealName);
			}
		}

		public  bool IsPraised{ get; set; }

		private string dateTime;

		public string CreateDate {
			get { 
				return dateTime;
			}
			set { 
				dateTime = value;
				RaisePropertyChanged(() => CreateDate);
			}
		}

		List<ECComment> eCCommentList;

		public List<ECComment> ECCommentList {
			get { 
				return eCCommentList ?? new List<ECComment>();
			}
			set { 
				eCCommentList = value;
			}
		}

		public void Init(int id)
		{
			postInfoId = id;
		}

		public ForumDetailViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			_messenger = Mvx.Resolve<IMvxMessenger>();

			_token = _messenger.Subscribe<CommentMessage>((message) => {
				// Analysis disable once LocalVariableHidesMember
				var comment = message.ECComment;
				ECCommentList.Add(comment);
				Comment = (PostInfo.CommentCount + 1).ToString();
				if (AddedCommentEventHandler != null) {
					AddedCommentEventHandler(null, comment);
				}
			});
		}

		public event EventHandler<ECComment> AddedCommentEventHandler;

		public override void InitData()
		{
			base.InitData();

			LoadNewPostInfo().Wait();
			LoadNewECCommentList().Wait();
		}

		private async Task LoadNewPostInfo()
		{
			ViewModelStatus = new ViewModelStatus("正在加载服务信息...", true, MessageType.Information, TipsType.DialogProgress);
			try {
				IsPraised = await _service.GetIsPraised(postInfoId);

			} catch {
				ViewModelStatus = new ViewModelStatus("服务信息已失效！", false, MessageType.Success, TipsType.DialogDisappearAuto);
			}
			try {
				PostInfo = await _service.GetECPostInfoById(postInfoId);
				if (PostInfo == null) {
					ViewModelStatus = new ViewModelStatus("服务信息已失效！", false, MessageType.Success, TipsType.DialogDisappearAuto);
				} else {
					PostInfoTiTle = postInfo.Title;
					Comment = PostInfo.CommentCount.ToString();
					Praise = PostInfo.PraiseCount.ToString();
					CreateDate = Utils.DateTimeString(PostInfo.CreateDate);
					RealName = postInfo.RealName;
					Description = Encoding.Default.GetString(Convert.FromBase64String(PostInfo.Html));
					BabyBusContext.Update(postInfo);
					ViewModelStatus = new ViewModelStatus("加载服务信息成功", false, MessageType.Success, TipsType.Undisplay);
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus("服务信息已失效！", false, MessageType.Success, TipsType.DialogDisappearAuto);
			}
		}

		async Task LoadNewECCommentList()
		{
			ViewModelStatus = new ViewModelStatus("正在加载服务信息...", true, MessageType.Information, TipsType.Undisplay);
			try {
				ECCommentList = await _service.GetECComment(postInfoId, 4);
				UserComment = string.Format("圈子评价：{0}", ECCommentList.Count);
				ViewModelStatus = new ViewModelStatus("加载服务信息成功", false, MessageType.Success, TipsType.Undisplay);
			} catch (Exception) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success, TipsType.DialogDisappearAuto);
			}
		}

		#region Command

		public MvxCommand SendPraiseCommand {
			get {
				return new MvxCommand(async () => {
					if (IsPraised) {
						ViewModelStatus = new ViewModelStatus("已赞！", false, MessageType.Error,
							TipsType.DialogDisappearAuto);
						return;
					}
					var review = new CreateEcComment {
						PostInfoId = postInfoId,
						Content = "",
						CommentType = 2,
					};
					try {
						await _service.SendComment(review);
						IsPraised = true;
						PostInfo.PraiseCount += 1;
						BabyBusContext.Update(PostInfo);
						Praise = PostInfo.PraiseCount.ToString();
					} catch (Exception) {

						ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error,
							TipsType.DialogDisappearAuto);
					}
				});
			}
		}

		IMvxCommand _showCommentListCommand;

		public IMvxCommand ShowCommentListCommand {
			get {
				_showCommentListCommand = _showCommentListCommand ?? new MvxAsyncronizeCommand(
					() => ShowViewModel<ForumCommentListViewModel>(new {id = postInfoId
}));
				return _showCommentListCommand;
			}
		}

		private IMvxCommand _sendCommentCommand;

		public IMvxCommand SendCommentCommand {
			get {
				_sendCommentCommand = _sendCommentCommand ?? new MvxAsyncronizeCommand(
					() => ShowViewModel<SendCommentViewModel>(new {id = postInfoId
}));
				return _sendCommentCommand;
			}
		}

		#endregion

	}
}

