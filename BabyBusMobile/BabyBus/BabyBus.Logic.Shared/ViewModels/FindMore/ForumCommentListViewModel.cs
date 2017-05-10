using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.Logic.Shared
{
	public class ForumCommentListViewModel : BaseListViewModel
	{
		
		IRemoteService _service;

		public ForumCommentListViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		private int postInfoId;

		long _minId = 0;

		public long MinId {
			get {
				return Interlocked.Read(ref _minId);
			}
			set {
				Interlocked.Exchange(ref _minId, value);
			}
		}

		public event EventHandler<List<ECComment>> DataLoadedMore;

		private Object _idLock = new Object();


		public void Init(int id)
		{
			postInfoId = id;
		}

		public override void InitData()
		{
			base.InitData();


			List<ECComment> list;
			list = BabyBusContext.ECCommentList.Where(x => x.PostInfoId == postInfoId).Take(PageSize).OrderByDescending(x => x.CreateDate).ToList();

			if (true) {
				Task task = Task.Factory.StartNew(() => {
					
					ListObject = LoadNewReviews().Result;
				});
				task.Wait();
			} else {
				lock (_idLock) {
					MinId = list.LastOrDefault().CommentId;
					ListObject = list;
				}
			}
		}

		private	async Task<List<ECComment>> LoadNewReviews()
		{
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);

				var comments = await _service.GetECComment(postInfoId);

				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				if (comments.Count > 0) {
					lock (_idLock) {
						BabyBusContext.InsertAll(comments);
						if (MinId == 0)
							MinId = comments.Last().CommentId;
					}
					return comments;
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
			return new List<ECComment>();
		}

		async Task<List<ECComment>> LoadMoreReviews()
		{
			List<ECComment> list;
			try {
				//Local DB
				list = BabyBusContext.ECCommentList
					.Where(x => x.CommentId < MinId && x.PostInfoId == postInfoId).OrderByDescending(x => x.CreateDate)
					.Take(Constants.PAGESIZE).ToList();

				if (false) {
					MinId = list.Last().CommentId;
					return list;
				} else {
					var coments = await _service.GetOldECCommentList(postInfoId, MinId);
					if (coments.Count > 0) {
						BabyBusContext.InsertAll(coments);
						MinId = coments.Last().CommentId;
					}
					return coments;
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
			return new List<ECComment>();
		}

		private IMvxCommand loadMoreCommand;

		public IMvxCommand LoadMoreCommand {
			get {
				loadMoreCommand = loadMoreCommand ?? new MvxAsyncronizeCommand(async () => {
					var addList = await LoadMoreReviews();
					if (DataLoadedMore != null) {
						DataLoadedMore(this, addList);
					}

				});
				return loadMoreCommand;
			}
		}

		private IMvxCommand _sendCommentCommand;

		public IMvxCommand SendCommentCommand {
			get {
				_sendCommentCommand = _sendCommentCommand ?? new MvxAsyncronizeCommand(
					() => ShowViewModel<SendCommentViewModel>(new {id = postInfoId}));
				return _sendCommentCommand;
			}
		}
	}
}

