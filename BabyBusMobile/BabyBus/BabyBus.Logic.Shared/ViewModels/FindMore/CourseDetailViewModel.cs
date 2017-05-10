using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Messenger;

namespace BabyBus.Logic.Shared
{
	public class CourseDetailViewModel : BaseListViewModel
	{

		IRemoteService _service = null;
		MvxSubscriptionToken _token;
		IMvxMessenger _messenger;

		public int PostinfoId;

		private ECPostInfo postinfo;

		public ECPostInfo PostInfo {
			get { 
				return postinfo ?? new ECPostInfo();
			}
			set {
				postinfo = value;
			}
		}

		public ECColumnType ECColumnType{ get; set; }

		private List<ECReview> eCReview;

		public List<ECReview> ECReviewList {
			get { 
				return eCReview ?? new List<ECReview>();
			}
			set { 
				eCReview = value;
			}
		}

		private float rating;

		public float PostInfoRating {
			get { 
				return rating;
			}
			set { 
				rating = value;
				RaisePropertyChanged(() => PostInfoRating);
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

		private string isAccredited;

		public string IsAccredited {
			get { 
				return isAccredited;
			}
			set { 
				isAccredited = value;
				RaisePropertyChanged(() => IsAccredited);
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

		private string userAppraisal;

		public string UserAppraisal {
			get { 
				return userAppraisal;
			}
			set { 
				userAppraisal = value;
				RaisePropertyChanged(() => UserAppraisal);
			}
		}

		private string description;

		public string Description {
			get { 
				return description;
			}
			set { 
				description = value;
			}
		}

		string _involved;

		public string Involved {
			get {
				return _involved;
			}
			set {
				_involved = value;
				RaisePropertyChanged(() => Involved);
			}
		}

		public int CommentCount{ get; set; }

		public void Init(int postinfoId, ECColumnType eCColumnType)
		{
			PostinfoId = postinfoId;
			ECColumnType = eCColumnType;
//			postinfo = BabyBusContext.ECPostInfoList.Where(xx => xx.PostInfoId == postinfoId).FirstOrDefault();
//			if (postinfo.PostInfoId != 0) {
//				Description = System.Text.UTF8Encoding.Default.GetString(Convert.FromBase64String(postinfo.Html));
//			}
		}

		public CourseDetailViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			_messenger = Mvx.Resolve<IMvxMessenger>();

			_token = _messenger.Subscribe<ReviewMessage>((message) => {
				var review = message.ECReview;
				PostInfoRating = (PostInfo.Rating * PostInfo.CommentCount + review.Rating) / (PostInfo.CommentCount + 1);
				CommentCount = PostInfo.CommentCount + 1;
				Comment = string.Format("{0}条评论", CommentCount);
				UserAppraisal = string.Format("用户点评({0})", CommentCount);

				ECReviewList.Insert(0, review);

				if (AddedReviewEventHandler != null) {
					AddedReviewEventHandler(null, review);
				}
			});
		}

		public event EventHandler<ECReview> AddedReviewEventHandler;

		public  override void InitData()
		{
			base.InitData();
			LoadNewECReview().Wait();
			LoadNewPostInfo().Wait();

		}

		private async Task LoadNewPostInfo()
		{
			ViewModelStatus = new ViewModelStatus("正在加载服务信息...", true, MessageType.Information, TipsType.DialogProgress);
			try {
				postinfo = await _service.GetECPostInfoById(PostinfoId);
				if (postinfo == null) {
					ViewModelStatus = new ViewModelStatus("服务信息已失效！", false, MessageType.Success, TipsType.DialogDisappearAuto);
				} else {
					PostInfoRating = (float)postinfo.Rating;
					PostInfoTiTle = postinfo.Title;
					CommentCount = postinfo.CommentCount;
					Comment = string.Format("{0}条评论", postinfo.CommentCount);
					UserAppraisal = string.Format("用户点评({0})", postinfo.CommentCount);
					Involved = string.Format("已报{0}/{1}", postinfo.InvolvedCount, postinfo.TotalCount);
					Description = Encoding.Default.GetString(Convert.FromBase64String(postinfo.Html));
					IsAccredited = "已认证机构";
					BabyBusContext.Update(postinfo);
				}

				ViewModelStatus = new ViewModelStatus("加载服务信息成功", false, MessageType.Success, TipsType.Undisplay);
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success, TipsType.DialogDisappearAuto);
			}
		}

		private async Task LoadNewECReview()
		{
			ViewModelStatus = new ViewModelStatus("正在加载服务信息...", true, MessageType.Information, TipsType.Undisplay);
			try {
				ECReviewList = await _service.GetECReview(PostinfoId, 4);
				ViewModelStatus = new ViewModelStatus("加载服务信息成功", false, MessageType.Success, TipsType.Undisplay);
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success, TipsType.Undisplay);
			}
		}

		IMvxCommand _showReviewListCommand;

		public IMvxCommand ShowReviewListCommand {
			get {
				_showReviewListCommand = _showReviewListCommand
				?? new MvxCommand(() => 
						ShowViewModel<CourseReviewListViewModel>(new {id = PostinfoId}));
				return _showReviewListCommand;
			}
		}

		IMvxCommand _showPaymentCommand;

		public IMvxCommand PaymentCommand {
			get {
				_showPaymentCommand = _showPaymentCommand
				?? new MvxCommand(() =>
						ShowViewModel<ECPaymentViewModel>(new { id = PostinfoId,type = PaymentType.FindMore }));
				return _showPaymentCommand;
			}
		}

		IMvxCommand _showSendReviewCommand;

		public IMvxCommand SendReviewCommand {
			get {
				_showSendReviewCommand = _showSendReviewCommand
				?? new MvxCommand(() =>
                        ShowViewModel<SendReviewViewModel>(new { id = PostinfoId }));
				return _showSendReviewCommand;
			}
		}
	}
}

