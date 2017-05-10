using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;

namespace BabyBus.Logic.Shared
{
	public class ForumIndexViewModel :BaseListViewModel
	{
		public readonly int SUIBIANSHUO_CATEGORY = 13;

		public object SendForumViewModelCommant {
			get;
			set;
		}

		public ForumIndexViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
			_eCCoulumnType = ECColumnType.Forum;
			CategoryId = 0;

		}

		private const bool _IsFirst = true;

		public ForumIndexViewModel(int categoryId)
			: this()
		{
			CategoryId = categoryId;
		}

		private IRemoteService _service;

		public int CategoryId{ get; set; }

		public List<ECCategory> CategoryList { get; set; }

		private Object _idLock = new Object();

		public event EventHandler<List<ECPostInfo>> DataRefreshed;
		public event EventHandler<List<ECPostInfo>> DataLoadedMore;

		private ECColumnType _eCCoulumnType { get; set; }

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

		private AutoResetEvent _autoevent = new AutoResetEvent(false);

		public override void InitData()
		{
			base.InitData();

			Task task1 = Task.Factory.StartNew(() => {
				if (CategoryList == null) {
					try {
						ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);
						CategoryList = _service.GetECCategoryList(ECColumnType.Forum).Result;
						if (CategoryList.Count > 0) {
							CategoryId = CategoryList.First().Id;
						}	
						ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, true, MessageType.Information, TipsType.Undisplay);
					} catch (Exception ex) {
						ViewModelStatus = new ViewModelStatus(ex.Message, false, MessageType.Error, TipsType.DialogDisappearAuto);
					}
				}
			});
			task1.Wait();

			var infoList = BabyBusContext.ECForumInfoList.Where(x => x.CategoryId == CategoryId).OrderByDescending(x => x.CreateDate).Take(Constants.PAGESIZE).ToList();

			if (infoList.Count == 0) {
				Task task = Task.Factory.StartNew(() => {
					infoList = LoadNewECPostInfoCore(true).Result;
					ListObject = infoList;
				});
				task.Wait();
			} else {
				lock (_idLock) {
					MaxId = infoList.FirstOrDefault().PostInfoId;
					MinId = infoList.LastOrDefault().PostInfoId;
					ListObject = infoList;//这个也加到锁里面，防止乱序或者重复设置
				}
				_autoevent.Set();
			}
		}

		private	async Task<List<ECPostInfo>> LoadNewECPostInfoCore(bool isFirst)
		{

			try {
				ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, isFirst ? TipsType.DialogProgress : TipsType.Undisplay);
				List<ECPostInfo> list;
				if (CategoryId == 0)
					list = await _service.GetNewECPostInfo(new ECColumnType[]{ ECColumnType.Forum }, MaxId, null);
				else
					list = await _service.GetNewECPostInfo(new ECColumnType[]{ ECColumnType.Forum }, MaxId, new []{ CategoryId });

				ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.Undisplay);
				if (list.Count > 0) {
					lock (_idLock) {
						//TODO: 为了确保拿到更新的帖子或有效的删帖，不再本地保存帖子。
						if (CategoryId != SUIBIANSHUO_CATEGORY) {
							BabyBusContext.InsertAll(list);
						}
						//TODO: 这块逻辑写的比较诡异。是为了防止InitData和Refresh重复插入数据的，以后再改。
						if (MaxId == list.First().PostInfoId) {
							list = new List<ECPostInfo>();
						} else {
							MaxId = list.First().PostInfoId;
							if (MinId == 0)
								MinId = list.Last().PostInfoId;
						}
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
			list = BabyBusContext.ECForumInfoList.Where(x => x.PostInfoId < MinId && x.CategoryId == CategoryId)
				.OrderByDescending(x => x.CreateDate)
				.Take(Constants.PAGESIZE).ToList();
			
			if (list.Count > 0) {
				lock (_idLock) {
					var addlist = list;
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
					var ec = new ECColumnType[] {
						_eCCoulumnType
					};
					var postInfo = await _service.GetOldECPostInfo(ec, MinId, new int[]{ CategoryId });
					// ViewModelStatus = new ViewModelStatus("加载历史数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);

					if (postInfo.Count > 0) {
						lock (_idLock) {
							//TODO: 为了确保拿到更新的帖子或有效的删帖，不再本地保存帖子
							if (CategoryId != SUIBIANSHUO_CATEGORY) {
								BabyBusContext.InsertAll(postInfo);
							}
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

		public void ShowForumDetailViewModel(long noticeId)
		{
			ShowViewModel<ForumDetailViewModel>(new {id = noticeId});
		}

		private IMvxCommand sendForumViewModelCommand;

		public IMvxCommand SendForumViewModelCommand {
			get { 
				sendForumViewModelCommand = sendForumViewModelCommand ?? new MvxAsyncronizeCommand(() => {
					ShowViewModel<SendForumViewModel>();
				});
				return sendForumViewModelCommand;
			}

			
		}

		#endregion
	}
}

