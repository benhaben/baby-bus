using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using Cirrious.CrossCore;

namespace BabyBus.Logic.Shared
{
	public class ActivityCourseIndexViewModel:BaseListViewModel
	{

		private IRemoteService _service;
		private Object _idLock = new Object();

		public event EventHandler<List<ECPostInfo>> DataRefreshed;
		public event EventHandler<List<ECPostInfo>> DataLoadedMore;

		public ActivityCourseIndexViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		public ECColumnType ECCoulumnType { get; set; }

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

		public void Init(ECColumnType eCCoulumType)
		{
			ECCoulumnType = eCCoulumType;
		}

		private AutoResetEvent _autoevent = new AutoResetEvent(false);

		public override void InitData()
		{
			base.InitData();
				
			var infoList = BabyBusContext.ECPostInfoList.Where(x => x.ColumnType == (int)ECCoulumnType)
				.OrderByDescending(x => x.CreateDate).Distinct().Take(Constants.PAGESIZE).ToList();
			if (infoList.Count == 0) {
				Task task = Task.Factory.StartNew(() => {
					//init notices
					infoList = LoadNewECPostInfoCore(true).Result;
					ListObject = infoList;
				});
				task.Wait();
			} else {
				lock (_idLock) {
					MaxId = infoList.FirstOrDefault().PostInfoId;
					MinId = infoList.LastOrDefault().PostInfoId;
					ListObject = infoList;
				}
				//设置完_maxId再给锁，不然view那边的LoadNewNotices可能先执行

			}
			_autoevent.Set();
		}

		async Task<List<ECPostInfo>> LoadNewECPostInfoCore(bool isFirst)
		{

			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, isFirst ? TipsType.DialogProgress : TipsType.Undisplay);

				var ec = new ECColumnType[] {
					ECCoulumnType
				};
				var list = await _service.GetNewECPostInfo(ec, MaxId, null);

				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				if (list.Count > 0) {
					lock (_idLock) {
						//TODO: 为了确保取到课程或活动，不在本地保存课程或活动
//						BabyBusContext.InsertAll(list);
						MaxId = list.First().PostInfoId;
						if (MinId == 0)
							MinId = list.Last().PostInfoId;
						return list;
					}
				}
				return list;

			} catch (Exception ex) {
				ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
				return null;
			} finally {
				_autoevent.Set();
			}

		}

		private async Task<List<ECPostInfo>> LoadNewECPostInfo()
		{
			//先给锁，不然LoadNewNotices会死锁
			_autoevent.WaitOne();
			return await LoadNewECPostInfoCore(false);
		}

		private List<ECPostInfo> LoadMoreECPostInfoFromLocalDB()
		{

			List<ECPostInfo> list;

			//Local DB
			list = LoadInfoFromLocalDB(MinId);
			if (list.Count > 0) {
				lock (_idLock) {
					var addlist = list;
					//Notices.AddRange(addlist);
					MinId = list.Last().PostInfoId;
					return addlist;
				}
			} else {
				return new List<ECPostInfo>();
			}
		}

		private async Task<List<ECPostInfo>> LoadMoreECPostInfo()
		{
			if (IsEnd)
				return new List<ECPostInfo>();

			//ViewModelStatus = new ViewModelStatus("正在加载历史数据...", true, MessageType.Information, TipsType.DialogProgress);

			try {
				var localDBList = LoadMoreECPostInfoFromLocalDB();
				if (localDBList.Count == 0) {
					//API
					var ec = new ECColumnType[] {
						ECCoulumnType
					};
					var postInfo = await _service.GetOldECPostInfo(ec, MinId, null);
					// ViewModelStatus = new ViewModelStatus("加载历史数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);

					if (postInfo.Count > 0) {
						lock (_idLock) {
							//TODO: 为了确保取到课程或活动，不在本地保存课程或活动
//							BabyBusContext.InsertAll(postInfo);
							MinId = postInfo.Last().PostInfoId;
							//refresh all
							return postInfo;
						}
					} else {
						IsEnd = true;
					}
				} else {
					//  ViewModelStatus = new ViewModelStatus("加载历史数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
					return localDBList;
				}
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
				ViewModelStatus = new ViewModelStatus("加载历史数据失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
			} finally {
			}
			return new List<ECPostInfo>();
		}

		private List<ECPostInfo> LoadInfoFromLocalDB(long minId)
		{
			List<ECPostInfo> list = new List<ECPostInfo>();
			if (ECCoulumnType == ECColumnType.Activity) {
				list = BabyBusContext.ECActivityInfoList.Where(x => x.PostInfoId < MinId)
					.OrderByDescending(x => x.CreateDate)
					.Take(Constants.PAGESIZE).ToList();
			} else if (ECCoulumnType == ECColumnType.Course) {
				list = BabyBusContext.ECCourceInfoList.Where(x => x.PostInfoId < MinId)
					.OrderByDescending(x => x.CreateDate)
					.Take(Constants.PAGESIZE).ToList();
			} else {
				list = BabyBusContext.ECForumInfoList.Where(x => x.PostInfoId < MinId)
					.OrderByDescending(x => x.CreateDate)
					.Take(Constants.PAGESIZE).ToList();
			}
			return list;
		}

		#region Command

		private IMvxCommand refreshCommand;

		public IMvxCommand RefreshCommand {
			get {
				refreshCommand = refreshCommand ?? new MvxAsyncronizeCommand(async() => {
					var addList = await LoadNewECPostInfo();
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
					var addList = await LoadMoreECPostInfo();
					if (DataLoadedMore != null) {
						DataLoadedMore(this, addList);
					}

				});
				return loadMoreCommand;
			}
		}

		public void ShowdationDetailViewModel(int postInfoId, ECColumnType eCColumnType)
		{
			ShowViewModel<CourseDetailViewModel>(new {postinfoId = postInfoId,eCColumnType = eCColumnType});
		}

		#endregion
	}
}

