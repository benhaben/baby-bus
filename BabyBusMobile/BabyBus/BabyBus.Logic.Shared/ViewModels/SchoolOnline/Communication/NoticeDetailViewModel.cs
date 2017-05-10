using System;
using System.Linq;
using System.Threading.Tasks;


using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using BabyBusSSApi.ServiceModel.DTO.Create;



namespace BabyBus.Logic.Shared
{
	public class NoticeDetailViewModel : BaseViewModel
	{
		IRemoteService _service = null;

		protected int _noticeId;
		private int _position = 0;
		private NoticeType _noticetype;

		public int NoticeId{ get { return _noticeId; } }

		//Android PhotoView will use this Position
		public int Position {
			get { return _position; }
			set { _position = value; }
		}

		public NoticeDetailViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		public NoticeDetailViewModel(NoticeModel notice)
		{
			_service = Mvx.Resolve<IRemoteService>();
			Notice = notice;
		}

		public void Init(int noticeId)
		{
			_noticeId = noticeId;
		}

		//Android PhotoView
		public void Init(int noticeId, int position)
		{
			_noticeId = noticeId;
			_position = position;
		}

		public override void InitData()
		{
			base.InitData();
			try {
				Notice = BabyBusContext.BaseNoticeList.Where(x => x.NoticeId == _noticeId).FirstOrDefault();
			} catch (Exception ex) {
				Mvx.Error(ex.Message);
			}
			if (Notice == null) {
				try {
					ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
					Notice = _service.GetNoticeById(_noticeId).Result;
					ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				} catch (Exception ex) {
					ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Success, TipsType.Undisplay);
				}

			}
			#if __TEACHER__
            if (BabyBusContext.RoleType != RoleType.Parent
                && Notice.NoticeType != NoticeType.GrowMemory)
            {
                Task.Factory.StartNew(async () =>
                    {
                        var res = await _service.GetNoticeReadersSummaryByNoticeId(_noticeId);
                        TotalCount = res.TotalCount;
                        ReadedCount = res.ReadedCount;
                    });
            }
			#endif

			if (BabyBusContext.RoleType == RoleType.Parent && Notice != null && !Notice.IsReaded) {
				Notice.IsReaded = true;
				BabyBusContext.Update(Notice);
				Task.Factory.StartNew(() => SendComment(Notice.NoticeId));
			}
			_noticetype = Notice.NoticeType;
		}

		private NoticeModel notice;

		public NoticeModel Notice {
			get { return notice; }
			set {
				notice = value;
				RaisePropertyChanged(() => Notice);
			}
		}

		private long _totalCount;

		public long TotalCount {
			get {
				return _totalCount;
			}
			set {
				_totalCount = value;
				RaisePropertyChanged(() => TotalCount);
				RaisePropertyChanged(() => UnReadedCount);
			}
		}

		private long _readedCount;

		public long ReadedCount {
			get {
				return _readedCount;
			}
			set {
				_readedCount = value;
				RaisePropertyChanged(() => ReadedCount);
				RaisePropertyChanged(() => UnReadedCount);
				RaisePropertyChanged(() => ReadedStatus);
			}
		}

		private string _readedStatus = "已读 - 未读 -";

		public string ReadedStatus {
			get {
				return "已读 " + ReadedCount + "未读 " + UnReadedCount;
			}
		}

		public long UnReadedCount {
			get{ return TotalCount - ReadedCount; }
		}

		private void SendComment(long  id)
		{
			var comment = new CreateComment {
				CommentType = (int)CommentType.CheckedType,
				NoticeId = (int)id,
			};
			_service.CreateComment(comment);
		}

		/// <summary>
		/// For Android. Shows the notice image detail with PhotoViewer.
		/// </summary>
		public void ShowNoticeImageDetail(int position)
		{
			ShowViewModel<MemoryDetailViewModel>(new {noticeId = _noticeId,position = position});
		}

		private IMvxCommand _showReadList;

		public IMvxCommand ShowReadList {
			get { 
				_showReadList = _showReadList
				?? new MvxCommand(() => 
						ShowViewModel<ReadListViewModel>(new {noticeId = _noticeId,noticetype = _noticetype}));

				return _showReadList;
			}
		}
	}
}
