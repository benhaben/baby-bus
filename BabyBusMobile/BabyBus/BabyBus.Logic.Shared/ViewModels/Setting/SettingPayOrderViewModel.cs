using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.Logic.Shared
{
	public class SettingPayOrderViewModel : BaseListViewModel
	{

		IRemoteService _service;

		public SettingPayOrderViewModel()
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

		public event EventHandler<List<ECPayOrder>> DataLoadedMore;

		private Object _idLock = new Object();


		public void Init(int id)
		{
			postInfoId = id;
		}

		public override void InitData()
		{
			base.InitData();


			List<ECPayOrder> list;
			if (true) {
				Task task = Task.Factory.StartNew(() => {

					ListObject = LoadNewPayOrder().Result;
				});
				task.Wait();
			} else {
				lock (_idLock) {
					MinId = long.Parse(list.LastOrDefault().OrderNumber);
					ListObject = list;
				}
			}
		}

		private	async Task<List<ECPayOrder>> LoadNewPayOrder()
		{
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);

				var comments = await _service.GetNewECPayOrdertList();

				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				if (comments.Count > 0) {
					lock (_idLock) {
						if (MinId == 0)
							MinId = long.Parse(comments.Last().OrderNumber);
					}
					return comments;
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
			return new List<ECPayOrder>();
		}

		async Task<List<ECPayOrder>> LoadMorePayOrder()
		{
			List<ECPayOrder> list = new List<ECPayOrder>();
			try {
				if (list.Count > 0) {
					MinId = long.Parse(list.Last().OrderNumber);
					return list;
				} else {
					var coments = await _service.GetOldECPayOrderList(MinId);
					if (coments.Count > 0) {
						MinId = long.Parse(coments.Last().OrderNumber);
					}
					return coments;
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error, TipsType.DialogDisappearAuto);
			}
			return new List<ECPayOrder>();
		}

		public void ShowCourseDetailViewModel(int postInfoId, ECColumnType eCColumnType)
		{
			ShowViewModel<CourseDetailViewModel>(new {postinfoId = postInfoId,eCColumnType = eCColumnType});
		}

		public void ShowPayment(int postInfoId)
		{
			ShowViewModel<ECPaymentViewModel>(new {id = postInfoId});
		}

		public void SendReview(int postInfoId)
		{
			ShowViewModel<SendReviewViewModel>(new {id = postInfoId});
		}

		private IMvxCommand loadMoreCommand;

		public IMvxCommand LoadMoreCommand {
			get {
				loadMoreCommand = loadMoreCommand ?? new MvxAsyncronizeCommand(async () => {
					var addList = await LoadMorePayOrder();
					if (DataLoadedMore != null) {
						DataLoadedMore(this, addList);
					}

				});
				return loadMoreCommand;
			}
		}

		private IMvxCommand _courseDetailViewModelCommand;

		public IMvxCommand CourseDetailViewModelCommand {
			get {
				_courseDetailViewModelCommand = _courseDetailViewModelCommand ?? new MvxAsyncronizeCommand(() => {
					ShowViewModel<CourseDetailViewModel>(new {postinfoId = postInfoId});
				});
				return _courseDetailViewModelCommand;
			}
		}

		private IMvxCommand _paymentCommand;

		public IMvxCommand PaymentCommand {
			get {
				_paymentCommand = _paymentCommand ?? new MvxAsyncronizeCommand(() => {
					ShowViewModel<ECPaymentViewModel>(new {id = postInfoId});
				});
				return _paymentCommand;
			}
		}

		private IMvxCommand _sendreviewCommand;

		public IMvxCommand SendReviewCommand {
			get {
				_sendreviewCommand = _sendreviewCommand ?? new MvxAsyncronizeCommand(() => {
					ShowViewModel<SendReviewViewModel>(new {id = postInfoId});
				});
				return _sendreviewCommand;
			}
		}
	}
}




