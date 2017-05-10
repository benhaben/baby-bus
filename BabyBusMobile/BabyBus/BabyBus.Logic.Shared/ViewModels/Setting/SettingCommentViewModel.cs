using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Logic.Shared
{
	public class SettingCommentViewModel : BaseListViewModel
	{

		IRemoteService _service;

		public SettingCommentViewModel()
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

		public event EventHandler<List<ECPostInfo>> DataLoadedMore;

		private Object _idLock = new Object();


		public void Init(int id)
		{
			postInfoId = id;
		}

		public override void InitData()
		{
			base.InitData();


			List<ECComment> list;
			if (true) {
				Task task = Task.Factory.StartNew(() => {

					ListObject = LoadNewComment().Result;
				});
				task.Wait();
			} else {
				lock (_idLock) {
					MinId = list.LastOrDefault().CommentId;
					ListObject = list;
				}
			}
		}

		private	async Task<List<ECPostInfo>> LoadNewComment()
		{
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);

				var comments = await _service.GetNewPostCommentList();

				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				if (comments.Count > 0) {
					lock (_idLock) {
//						BabyBusContext.InsertAll(comments);
						if (MinId == 0)
							MinId = comments.Last().PostInfoId;
					}
					return comments;
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
			return new List<ECPostInfo>();
		}

		async Task<List<ECPostInfo>> LoadMoreComment()
		{
			try {
				var coments = await _service.GetOldPostCommentList(MinId);
				if (coments.Count > 0) {
					MinId = coments.Last().PostInfoId;
				}
				return coments;

			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
			return new List<ECPostInfo>();
		}

		private IMvxCommand loadMoreCommand;

		public IMvxCommand LoadMoreCommand {
			get {
				loadMoreCommand = loadMoreCommand ?? new MvxAsyncronizeCommand(async () => {
					var addList = await LoadMoreComment();
					if (DataLoadedMore != null) {
						DataLoadedMore(this, addList);
					}

				});
				return loadMoreCommand;
			}
		}

		public void ShowdationDetailViewModel(long postInfoId)
		{
			ShowViewModel<ForumDetailViewModel>(new {id = postInfoId});
		}

	}
}



