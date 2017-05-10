using System;
using System.Threading.Tasks;
using BabyBus.Utilities.Enum;
using Cirrious.MvvmCross.ViewModels;
using System.Threading;

namespace BabyBus.ViewModels {
    public class BaseViewModel : MvxViewModel {
        public bool IsLoading { get; set; }

        public event EventHandler FirstLoadedEventHandler;

        private long _numberOfFirstLoadThread = 0;

        public long NumberOfFirstLoadThread {
            get {
                return Interlocked.Read(ref _numberOfFirstLoadThread);
            }
            set {
                //                Interlocked.Increment(ref _numberOfFirstLoadThread);
                Interlocked.Exchange(ref _numberOfFirstLoadThread, value);
            }
        }

        private long _numberOfLoadingNewNoticesThread = 0;
        //在多线程环境中
        public long NumberOfLoadingNewNoticesThread {
            get {
                return Interlocked.Read(ref _numberOfLoadingNewNoticesThread);
            }
            set {
                Interlocked.Exchange(ref _numberOfLoadingNewNoticesThread, value);
            }
        }

        private long _numberOfLoadingMoreNoticesThread = 0;
        //在多线程环境中
        public long NumberOfLoadingMoreNoticesThread {
            get {
                return Interlocked.Read(ref _numberOfLoadingMoreNoticesThread);
            }
            set {
                Interlocked.Exchange(ref _numberOfLoadingMoreNoticesThread, value);
            }
        }

        public BaseViewModel() {
            IsLoading = false;
            PageIndex = 0;
            PageSize = Constants.PAGESIZE;
        }

        public string this [string index] {
            get {
                return Localize.GetString(index);
            }
        }

        public static AppType AppType { get; set; }

        public int VerCode { get; set; }

        public static BaseViewModel CurrentBaseViewModel { get; private set; }

        private ViewModelStatus _viewModelStatus = new ViewModelStatus("");

        public ViewModelStatus ViewModelStatus {
            get { return _viewModelStatus; }
            set {
                _viewModelStatus = value;
                if (InfoChanged != null) {
                    InfoChanged(this, null);
                }
                RaisePropertyChanged(() => ViewModelStatus);
            }
        }

        public virtual void FirstLoad() {
            if (FirstLoadedEventHandler != null) {
                FirstLoadedEventHandler(null, null);
            }
        }

        public bool IsSuccessStatus {
            get { return ViewModelStatus != null && ViewModelStatus.MessageType == MessageType.Success; }

        }

        public bool IsErrorStatus {
            get { return ViewModelStatus != null && ViewModelStatus.MessageType == MessageType.Error; }

        }

        public bool IsInformationStatus {
            get { return ViewModelStatus != null && ViewModelStatus.MessageType == MessageType.Information; }

        }

        public bool IsWarningStatus {
            get { return ViewModelStatus.MessageType == MessageType.Warning; }

        }

        public bool IsRunning {
            get { return ViewModelStatus.IsRunning; }

        }

        public string Information {
            get { return ViewModelStatus.Information; }
        }

        public event EventHandler InfoChanged;

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
