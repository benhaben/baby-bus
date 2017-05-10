using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Communication;
using BabyBus.Models.Enums;
using BabyBus.Net.Communication;
using BabyBus.Utilities;
using BabyBus.Utilities.Enum;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus.ViewModels.Communication {

    public class NoticeIndexViewModel : BaseViewModel {
        private INoticeService _service;
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

        public event EventHandler<List<NoticeDetailViewModel>> DataRefreshed;
        public event EventHandler<List<NoticeDetailViewModel>> DataLoadedMore;

        public NoticeViewType NoticeViewType;

        //showviewModel need this
        public NoticeIndexViewModel() {
            _service = Mvx.Resolve<INoticeService>();
        }

        //main view model need this to constractor
        public NoticeIndexViewModel(NoticeViewType type) {
            _service = Mvx.Resolve<INoticeService>();
            Init(type);
        }

        //showviewModel need this to init
        public void Init(NoticeViewType type) {
            if (type == NoticeViewType.Notice && AppType == AppType.Master) {
                NoticeViewType = NoticeViewType.NoticeOnlyKg;
            } else {
                NoticeViewType = type;
            }
        }

        public override void Start() {
            base.Start();
            FirstLoad();
        }

        //android call this function
        public void InitData() {
            FirstLoad();
        }

        public override void FirstLoad() {
            IsLoading = true;
            
            Task.Run(async () => {
                List<NoticeModel> list;
                if (NoticeViewType == NoticeViewType.Notice)
                    list = BabyBusContext.NoticeList.OrderByDescending(x => x.CreateTime).Take(PageSize).ToList();
                else if (NoticeViewType == NoticeViewType.NoticeOnlyKg)
                    list = BabyBusContext.NoticeList
                        .Where(x => x.NoticeType == NoticeType.KindergartenAll || x.NoticeType == NoticeType.KindergartenStaff)
                        .OrderByDescending(x => x.CreateTime).Take(PageSize).ToList();
                else
                    list = BabyBusContext.GrowMemoryList.OrderByDescending(x => x.CreateTime).Take(PageSize).ToList();
                if (list.Count == 0) {
                    await LoadNewNotices();
                } else {
                    _maxId = list.FirstOrDefault().NoticeId;
                    _minId = list.LastOrDefault().NoticeId;
                    Notices = GetNoticeDetails(list);
                }
                base.FirstLoad();
            });
        }

        private async Task<List<NoticeDetailViewModel>> LoadNewNotices() {
            if (NumberOfLoadingNewNoticesThread > 0) {
                return new List<NoticeDetailViewModel>();
            }
            NumberOfLoadingNewNoticesThread += 1;
            var user = BabyBusContext.UserAllInfo;
            try {
                //Load Attendance Data
                var attlist = new List<NoticeModel>();
                if (NoticeViewType == NoticeViewType.Notice) {
                    attlist = BabyBusContext.NoticeList.Where(x => x.NoticeId > _maxId && x.NoticeType == NoticeType.ClassEmergency)
                    .OrderByDescending(x => x.CreateTime).ToList();
                }
                Notices.InsertRange(0, GetNoticeDetails(attlist));

                ViewModelStatus = new ViewModelStatus("正在加载新数据...", true, MessageType.Information, TipsType.DialogProgress);
                var result = await _service.GetNewNoticeList(user, NoticeViewType, _maxId);
                ViewModelStatus = new ViewModelStatus("加载新数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);

                if (!result.Status) {
                    ViewModelStatus = new ViewModelStatus(result.Message);
                } else {
                    if (result.Items.Count > 0) {
                        var addlist = GetNoticeDetails(result.Items);
                        Notices.InsertRange(0, addlist);
                        BabyBusContext.InsertAll(result.Items);
                        _maxId = result.Items.First().NoticeId;
                        if (Notices.Count == addlist.Count)
                            _minId = result.Items.Last().NoticeId;
                        return addlist;
                    }
                }

            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                ViewModelStatus = new ViewModelStatus("加载新数据失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
            } finally {
                NumberOfLoadingNewNoticesThread -= 1;
            }
            return new List<NoticeDetailViewModel>();
        }

        private List<NoticeDetailViewModel> LoadMoreNoticesFromLocalDB() {
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
                var addlist = GetNoticeDetails(list);
                Notices.AddRange(addlist);
                MinId = list.Last().NoticeId;
                return addlist;
            } else {
                return new List<NoticeDetailViewModel>();
            }
        }

        private async Task<List<NoticeDetailViewModel>> LoadMoreNotices() {
            if (_isEnd)
                return new List<NoticeDetailViewModel>();

            if (NumberOfLoadingMoreNoticesThread > 0) {
                return new List<NoticeDetailViewModel>();
            }
            NumberOfLoadingMoreNoticesThread += 1;

            ViewModelStatus = new ViewModelStatus("正在加载历史数据...", true, MessageType.Information, TipsType.DialogProgress);

            try {
                var localDBList = LoadMoreNoticesFromLocalDB();
                if (localDBList.Count == 0) {
                    //API
                    var user = BabyBusContext.UserAllInfo;
                    var result = await _service.GetOldNoticeList(user, NoticeViewType, MinId);
                    ViewModelStatus = new ViewModelStatus("加载历史数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
                    if (!result.Status) {
                        ViewModelStatus = new ViewModelStatus(result.Message);
                    } else {
                        if (result.Items.Count > 0) {
                            var addlist = GetNoticeDetails(result.Items);
                            foreach (var item in addlist) {
                                Notices.Add(item);
                            }
                            BabyBusContext.InsertAll(result.Items);
                            MinId = result.Items.Last().NoticeId;
                            //refresh all
                            return addlist;
                        } else {
                            _isEnd = true;
                        }
                    }
                } else {
                    ViewModelStatus = new ViewModelStatus("加载历史数据成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
                    return localDBList;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                ViewModelStatus = new ViewModelStatus("加载历史数据失败", false, MessageType.Error, TipsType.DialogDisappearAuto);
            } finally {
                NumberOfLoadingMoreNoticesThread -= 1;
            }
            return new List<NoticeDetailViewModel>();
        }

        //TODO:到底使用NoticeDetailViewModel还是NoticeModel还可以再探讨
        private List<NoticeDetailViewModel> notices = new List<NoticeDetailViewModel>();

        public List<NoticeDetailViewModel> Notices {
            get { return notices; }
            set {
                notices = value;
                RaisePropertyChanged(() => Notices);
            }
        }

        private NoticeDetailViewModel selectedNotice;

        public NoticeDetailViewModel SelectedNotice {
            get { return selectedNotice; }
            set {
                selectedNotice = value;
            }
        }

        private List<NoticeDetailViewModel> GetNoticeDetails(List<NoticeModel> notices) {
            var list = new List<NoticeDetailViewModel>();
            foreach (var notice in notices) {
                var vm = new NoticeDetailViewModel(notice);
                list.Add(vm);
            }
            return list;
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

        #endregion
    }
}
