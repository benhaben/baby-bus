using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.Logic.Shared
{
	public class CourseReviewListViewModel : BaseListViewModel
	{
		int _postinfoId;
		private IRemoteService _service;


		public CourseReviewListViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		public void Init(int id)
		{
			_postinfoId = id;
		}

		long _minId = 0;

		public long MinId {
			get {
				return Interlocked.Read(ref _minId);
			}
			set {
				Interlocked.Exchange(ref _minId, value);
			}
		}

		public event EventHandler<List<ECReview>> DataLoadedMore;

		private Object _idLock = new Object();

		public override void InitData()
		{
			base.InitData();

			List<ECReview> list = new List<ECReview>();
			//	list = BabyBusContext.ECReviewList.Where(x => x.PostInfoId == _postinfoId).Take(PageSize).OrderByDescending(x => x.CreateDate).ToList();

			if (list.Count == 0) {
				Task task = Task.Factory.StartNew(() => {
					//init notices
					ListObject = LoadNewReviews().Result;
				});
				task.Wait();
			} else {
				lock (_idLock) {
					MinId = list.LastOrDefault().ReviewId;
					ListObject = list;
				}
			}
		}

		async Task<List<ECReview>> LoadNewReviews()
		{
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
				var reviews = await _service.GetECReview(_postinfoId);
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				if (reviews.Count > 0) {
					lock (_idLock) {
						BabyBusContext.InsertAll(reviews);
						if (MinId == 0)
							MinId = reviews.Last().ReviewId;
					}
					return reviews;
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
			return new List<ECReview>();
		}

		async Task<List<ECReview>> LoadMoreReviews()
		{
			List<ECReview> list;
			try {
				//Local DB
				list = BabyBusContext.ECReviewList
					.Where(x => x.PostInfoId == _postinfoId && x.ReviewId < MinId).OrderByDescending(x => x.CreateDate)
					.Take(Constants.PAGESIZE).ToList();

				if (list.Count > 0) {
					MinId = list.Last().ReviewId;
					return list;
				} else {
					var reviews = await _service.GetOldECReviewList(_postinfoId, MinId);
					if (reviews.Count > 0) {
						BabyBusContext.InsertAll(reviews);
						MinId = reviews.Last().ReviewId;
					}
					return reviews;
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
			return new List<ECReview>();
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

	}
}

