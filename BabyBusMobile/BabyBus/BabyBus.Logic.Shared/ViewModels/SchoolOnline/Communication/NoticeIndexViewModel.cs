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

	public class NoticeIndexViewModel : BaseListViewModel
	{
		private IRemoteService _service;
		//当前最大的id，注意不是数据库中最大的id
		private long _maxId = 0;
		//在多线程环境中
		public long MaxId {
			get {
				return Interlocked.Read(ref _maxId);
			}
			set {
				Interlocked.Exchange(ref _maxId, value);
			}
		}

		//当前最小的id
		private long _minId = 0;

		public long MinId {
			get {
				return Interlocked.Read(ref _minId);
			}
			set {
				//better to use Interlocked.Increment(), but this way is OK
				Interlocked.Exchange(ref _minId, value);
			}
		}

		//是否已经拿到最久以前发送的notice, 0 is false, 1 is true
		private bool _isEnd = false;

		//使用await以后基本上是在ui线程做修改操作，不会有同步问题，以防万一还是加上好
		public bool IsEnd {
			get {
				return _isEnd;
			}
			set {
				_isEnd = value;
//                Interlocked.Exchange(ref _isEnd, value);
			}
		}

		public event EventHandler<List<NoticeModel>> DataRefreshed;
		public event EventHandler<List<NoticeModel>> DataLoadedMore;

		public NoticeViewType NoticeViewType;

		//showviewModel need this
		public NoticeIndexViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		//main view model need this to constractor
		public NoticeIndexViewModel(NoticeViewType type)
		{
			_service = Mvx.Resolve<IRemoteService>();
			Init(type);
		}

		List<NoticeType> NoticeTypes;

		//showviewModel need this to init
		public void Init(NoticeViewType type)
		{
			NoticeViewType = type;

			NoticeTypes = new List<NoticeType>();
			if (type == NoticeViewType.Notice) {
				NoticeTypes.Add(NoticeType.ClassCommon);
				NoticeTypes.Add(NoticeType.ClassHomework);
				NoticeTypes.Add(NoticeType.KindergartenAll);
				NoticeTypes.Add(NoticeType.KindergartenRecipe);
				NoticeTypes.Add(NoticeType.BabyBusNotice);
				NoticeTypes.Add(NoticeType.BabyBusNoticeHtml);
				if (AppType != AppType.Parent) {
					NoticeTypes.Add(NoticeType.KindergartenStaff);
				}
			}
			if (type == NoticeViewType.GrowMemory) {
				NoticeTypes.Add(NoticeType.GrowMemory);
			}
		}

		//线程同步：一是互斥/加锁，目的是保证临界区代码操作的“原子性”；
		//另一种是信号灯操作，目的是保证多个线程按照一定顺序执行，如生产者线程要先于消费者线程执行
		//InitData LoadNewNotices LoadMoreNotices都需要使用_maxId，_minId,这里使用lock确保他们不同时操作
		private Object _idLock = new Object();

		//确保InitData先于 RefreshCommand 调用
		private AutoResetEvent _autoevent = new AutoResetEvent(false);

		public override  void InitData()
		{
			base.InitData();
			List<NoticeModel> list;
			if (NoticeViewType == NoticeViewType.Notice) {
				list = BabyBusContext.NoticeList.OrderByDescending(x => x.CreateTime).Take(PageSize).ToList();
			} else if (NoticeViewType == NoticeViewType.NoticeOnlyKg) {
				list = BabyBusContext.NoticeList
                        .Where(x => x.NoticeType == NoticeType.KindergartenAll || x.NoticeType == NoticeType.KindergartenStaff)
                        .OrderByDescending(x => x.CreateTime).Take(PageSize).ToList();
			} else {
				list = BabyBusContext.GrowMemoryList.OrderByDescending(x => x.CreateTime).Take(PageSize).ToList();
			}

			if (list.Count == 0) {
				Task task = Task.Factory.StartNew(() => {
					//init notices
					ListObject = LoadNewNoticesCore(true).Result;
				});
				//init data本身要是同步的，因为firstload是在另一个线程，这里没必要再异步
				task.Wait();
			} else {
				lock (_idLock) {
					MaxId = list.FirstOrDefault().NoticeId;
					MinId = list.LastOrDefault().NoticeId;
					ListObject = list;//这个也加到锁里面，防止乱序或者重复设置
				}
				//设置完_maxId再给锁，不然view那边的LoadNewNotices可能先执行
				_autoevent.Set();
			}
		}

		async Task<List<NoticeModel>> LoadNewNoticesCore(bool isFirst)
		{
			var user = BabyBusContext.UserAllInfo;
			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, isFirst ? TipsType.DialogProgress : TipsType.Undisplay);
				var notices = await _service.GetNewNoticeList(NoticeViewType, MaxId);
                
				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);

				if (notices.Count > 0) {
					lock (_idLock) {
						//Notices.InsertRange(0, addlist);
						BabyBusContext.InsertAll(notices);
						MaxId = notices.First().NoticeId;
						if (MinId == 0)
							MinId = notices.Last().NoticeId;
						return notices;
					}
				}
			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
				_autoevent.Set();
			}
			return new List<NoticeModel>();
		}

		private async Task<List<NoticeModel>> LoadNewNotices()
		{
			//先给锁，不然LoadNewNotices会死锁
			_autoevent.WaitOne();
			return await LoadNewNoticesCore(false);
		}

		private List<NoticeModel> LoadMoreNoticesFromLocalDB()
		{
			List<NoticeModel> list;

			//Local DB
			if (NoticeViewType == NoticeViewType.Notice) {
				list = BabyBusContext.NoticeList.Where(x => x.NoticeId < MinId)
                    .OrderByDescending(x => x.CreateTime)
                    .Take(Constants.PAGESIZE).ToList();
			} else {
				list = BabyBusContext.GrowMemoryList.Where(x => x.NoticeId < MinId)
                    .OrderByDescending(x => x.CreateTime)
                    .Take(Constants.PAGESIZE).ToList();
			}
			if (list.Count > 0) {
				lock (_idLock) {
					var addlist = list;
					//Notices.AddRange(addlist);
					MinId = list.Last().NoticeId;
					return addlist;
				}
			} else {
				return new List<NoticeModel>();
			}
		}

		private async Task<List<NoticeModel>> LoadMoreNotices()
		{
			if (IsEnd)
				return new List<NoticeModel>();

//            ViewModelStatus = new ViewModelStatus("正在加载历史数据...", true, MessageType.Information, TipsType.DialogProgress);

			try {
				var localDBList = LoadMoreNoticesFromLocalDB();
				if (localDBList.Count == 0) {
					//API
					var notices = await _service.GetOldNoticeList(NoticeViewType, MinId);
//                    ViewModelStatus = new ViewModelStatus("加载历史数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
                   
					if (notices.Count > 0) {
						lock (_idLock) {
							BabyBusContext.InsertAll(notices);
							MinId = notices.Last().NoticeId;
							//refresh all
							return notices;
						}
					} else {
						IsEnd = true;
					}
				} else {
//                    ViewModelStatus = new ViewModelStatus("加载历史数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
					return localDBList;
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
				ViewModelStatus = new ViewModelStatus("加载历史数据失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}
			return new List<NoticeModel>();
		}

		#region Command

		private IMvxCommand refreshCommand;

		public IMvxCommand RefreshCommand {
			get {
				refreshCommand = refreshCommand ?? new MvxAsyncronizeCommand(async() => {
					var addList = await LoadNewNotices();
					if (DataRefreshed != null) {
						DataRefreshed(this, addList);
					}
				});
				return refreshCommand;
			}
		}

		private IMvxCommand loadMoreCommand;

		public IMvxCommand LoadMoreCommand {
			get {
				loadMoreCommand = loadMoreCommand ?? new MvxAsyncronizeCommand(async () => {
					var addList = await LoadMoreNotices();
					if (DataLoadedMore != null) {
						DataLoadedMore(this, addList);
					}

				});
				return loadMoreCommand;
			}
		}

		public void ShowNoticeDetailViewModel(long noticeId, bool IsHtml)
		{
			if (IsHtml) {
				ShowViewModel<NoticeDetailHtmlViewModel>(new {noticeId = noticeId});
			} else {
				ShowViewModel<NoticeDetailViewModel>(new {noticeId = noticeId});
			}
		}

		public void ShowNoticeDetailHtmlViewModel(long noticeId)
		{
			ShowViewModel<NoticeDetailHtmlViewModel>(new {noticeId = noticeId});
		}

		public void ShowNoticeDetailViewModel(long noticeId, int position)
		{
			ShowViewModel<MemoryDetailViewModel>(new {noticeId = noticeId,position = position});
		}

		#endregion
	}
}
