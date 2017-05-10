using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Logic.Shared
{
	public class ParentSchoolOnlineViewModel : BaseViewModel
	{
		private IRemoteService _service;

		public ParentSchoolOnlineViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		public override void InitData()
		{

			try {
				ClassNotice = _service.GetLatestClassNotice().Result;
				ClassNoticeId = ClassNotice.NoticeId;
				ClassNoticeTitle = ClassNotice.Title;
				ClassNoticeContent = ClassNotice.AbstractDisplay;
				ClassNoticeCreateTime = ClassNotice.CreateTime;

				KindergartenNotice = _service.GetLatestKindergartensNotice().Result;

				KindergartenNoticeId = KindergartenNotice.NoticeId;
				KindergartenNoticeTitle = KindergartenNotice.Title;
				KindergartenNoticeContent = KindergartenNotice.AbstractDisplay;
				KindergartenNoticeCreateTime = KindergartenNotice.CreateTime;
			

				AdvertisementModel = _service.GetAdvertisement().Result;

			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogWithCancelButton);
			}
		}


		#region Property

		private byte[] _bytes;

		public byte[] Bytes {
			get { return _bytes; }
			set {
				_bytes = value;
				RaisePropertyChanged(() => Bytes);
			}
		}

		public NoticeModel ClassNotice{ get; set; }

		public NoticeModel KindergartenNotice{ get; set; }

		public string ImageName {
			get { 
				if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
					return BabyBusContext.UserAllInfo.Child.ImageName;
				} else {
					return BabyBusContext.UserAllInfo.ImageName;
				}
			}
		}

		private string _kindergartenName;

		public string KindergartenName {
			get { return _kindergartenName; }
			set {
				_kindergartenName = value;
				RaisePropertyChanged(() => KindergartenName);
			}
		}

		private string _className;

		public string ClassName {
			get { return _className; }
			set {
				_className = value;
				RaisePropertyChanged(() => ClassName);
			}
		}

		private string _childName;

		public string ChildName {
			get { return _childName; }
			set {
				_childName = value;
				RaisePropertyChanged(() => ChildName);
			}
		}

		private string _birthday;

		public string Birthday {
			get { return _birthday; }
			set {
				_birthday = value;
				RaisePropertyChanged(() => Birthday);
			}
		}

		private string _classNoticeTitle;

		public string ClassNoticeTitle {
			get { 
				if (string.IsNullOrEmpty(_classNoticeTitle)) {
					_classNoticeTitle = "无新信息";
				}
				return _classNoticeTitle; 
			}
			set {
				if (!string.IsNullOrEmpty(value)) {
					_classNoticeTitle = value;
				} else {
					_classNoticeTitle = "无新信息";
				}
				RaisePropertyChanged(() => ClassNoticeTitle);
			}
		}

		public DateTime ClassNoticeCreateTime {
			get;
			set;
		}

		public long ClassNoticeId {
			get;
			set;
		}

		private string _classNoticeContent;

		public string ClassNoticeContent {
			get { return _classNoticeContent; }
			set {
				if (!string.IsNullOrEmpty(value))
					_classNoticeContent = value;
				else
					_classNoticeContent = "无新消息";
				RaisePropertyChanged(() => ClassNoticeContent);
			}
		}

		public DateTime KindergartenNoticeCreateTime {
			get;
			set;
		}

		public long KindergartenNoticeId {
			get;
			set;
		}

		private string _kindergartenNoticeTitle;

		public string KindergartenNoticeTitle {
			get { 
				if (string.IsNullOrEmpty(_kindergartenNoticeTitle)) {
					_kindergartenNoticeTitle = "无新消息";
				}

				return _kindergartenNoticeTitle;
			}
			set {
				if (!string.IsNullOrEmpty(value)) {
					_kindergartenNoticeTitle = value;
				} else {
					_kindergartenNoticeTitle = "无新消息";
				}
				RaisePropertyChanged(() => KindergartenNoticeTitle);
			}
		}

		private string _kindergartenNoticeContent;

		public string KindergartenNoticeContent {
			get { return _kindergartenNoticeContent; }
			set {
				if (!string.IsNullOrEmpty(value))
					_kindergartenNoticeContent = value;
				else
					_kindergartenNoticeContent = "无新信息";
				RaisePropertyChanged(() => KindergartenNoticeContent);
			}
		}

		public List<string> ImageList {
			get;
			set;
		}

		public List<AdvertisementModel> AdvertisementModel{ get; set; }

		List<string> _advertisementStrList;

		public List<string> AdvertisementStrList {
			get {
				if (_advertisementStrList == null) {
					_advertisementStrList = new List<string>();
					if (AdvertisementModel != null) {
						AdvertisementModel.ForEach(x => _advertisementStrList.Add(x.NormalPics));
					}
				}
				return _advertisementStrList;
			}
		}

		#endregion

		#region Command

		private IMvxCommand _showFeedbackCommand;

		/// <summary>
		/// Clear Cache, Mainly Clear Image Cache
		/// </summary>
		public IMvxCommand ShowFeedbackCommand {
			get {
				_showFeedbackCommand = _showFeedbackCommand ??
				new MvxCommand(() => ShowViewModel<FeedBackViewModel>());
				return _showFeedbackCommand;
			}
		}

		private IMvxCommand _questionCommand;

		/// <summary>
		/// Clear Cache, Mainly Clear Image Cache
		/// </summary>
		public IMvxCommand QuestionCommand {
			get {
				_questionCommand = _questionCommand ?? new MvxCommand(() => ShowViewModel<SendQuestionViewModel>(new {isSendToWho = (int)RoleType.Teacher}));
				return _questionCommand;
			}
		}

		private IMvxCommand _sendToMasterCommand;

		public IMvxCommand SendToMasterCommand {
			get {
				_sendToMasterCommand = _sendToMasterCommand ?? new MvxCommand(() => ShowViewModel<SendQuestionViewModel>(new {isSendToWho = (int)RoleType.HeadMaster}));
				return _sendToMasterCommand;
			}
		}

		private IMvxCommand _memoryIndexViewModel;

		public IMvxCommand ShowMemoryIndexViewCommand {
			get {
				_memoryIndexViewModel = _memoryIndexViewModel ?? new MvxCommand(() => ShowViewModel <MemoryIndexViewModel >(new{type = NoticeViewType.GrowMemory}));
				return _memoryIndexViewModel;
			}
		}

		private IMvxCommand _noticeIndexViewModel;

		public IMvxCommand NoticeIndexViewCommand {
			get {
				_noticeIndexViewModel = _noticeIndexViewModel ?? new MvxCommand(() => ShowViewModel <NoticeIndexViewModel >(new{type = NoticeViewType.Notice}));
				return _noticeIndexViewModel;
			}
		}

		private IMvxCommand _questionIndexViewModel;

		public IMvxCommand QuestionIndexViewCommand {
			get {
				_questionIndexViewModel = _questionIndexViewModel ?? new MvxCommand(() => ShowViewModel<QuestionIndexViewModel>());
				return _questionIndexViewModel;
			}
		}

		public void ShowNoticeDetailViewModel(NoticeModel notice)
		{
			if (notice.IsHtml) {
				ShowViewModel<NoticeDetailHtmlViewModel>(new {noticeId = notice.NoticeId});
			} else {
				ShowViewModel<NoticeDetailViewModel>(new {noticeId = notice.NoticeId});
			}
		}

		public void ShowLearningMaterialsViewCommand()
		{
			ShowViewModel<LearningMaterialsViewModel>();
		}

		#endregion
	}
}

