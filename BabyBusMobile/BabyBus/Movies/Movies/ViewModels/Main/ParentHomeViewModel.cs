using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models;
using BabyBus.Models.Communication;
using BabyBus.Services;
using BabyBus.Services.Main;
using BabyBus.Utilities;
using BabyBus.ViewModels.Communication;
using BabyBus.ViewModels.Content;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBus.Models.Enums;
using ViewModels.Communication;

namespace BabyBus.ViewModels.Main {
    public class ParentHomeViewModel : BaseViewModel {
        private readonly IMainService _service;
        private readonly IPictureService _picService;
        public ParentHomeViewModel() {
            _service = Mvx.Resolve<IMainService>();
            _picService = Mvx.Resolve<IPictureService>();
        }

        public override void Start() {
            base.Start();
            ChildName = BabyBusContext.UserAllInfo.Child.ChildName;
            KindergartenName = BabyBusContext.UserAllInfo.Kindergarten.KindergartenName;
            ClassName = BabyBusContext.UserAllInfo.Class.ClassName;
            Birthday = BabyBusContext.UserAllInfo.Child.Birthday.ToString("D");

            _picService.LoadIamgeFromSource(BabyBusContext.UserAllInfo.ImageName,
                stream => {
                    var ms = stream as MemoryStream;
                    if (ms != null)
                        Bytes = ms.ToArray();
                });


            RefreshData();
        }

        /// <summary>
        /// Init & Refresh Child's Notices & Grow Memory
        /// </summary>
        public void RefreshData() {
            Task.Run(async () => {
                var cs = await _service.GetChildSummary(BabyBusContext.ChildId);
                ClassNoticeTitle = cs.ClassNoticeTitle;
                ClassNoticeContent = cs.ClassNoticeContent;
                KindergartenNoticeTitle = cs.KindergartenNoticeTitle;
                KindergartenNoticeContent = cs.KindergartenNoticeConent;
                Pics = cs.Pics;
                //Notify UI
                base.FirstLoad();
            });
        }

        #region Property

        private byte[] _bytes;

        public byte[] Bytes {
            get { return _bytes; }
            set {
                _bytes = value;
                RaisePropertyChanged(() => Bytes);
            }
        }

        private string _kindergartenName;

        public string KindergartenName {
            get { return _kindergartenName; }
            set {
                _kindergartenName = value;
                RaisePropertyChanged(() => KindergartenName);
            }
        }

        private string _className;

        public string ClassName {
            get { return _className; }
            set {
                _className = value;
                RaisePropertyChanged(() => ClassName);
            }
        }

        private string _childName;

        public string ChildName {
            get { return _childName; }
            set {
                _childName = value;
                RaisePropertyChanged(() => ChildName);
            }
        }

        private string _birthday;

        public string Birthday {
            get { return _birthday; }
            set {
                _birthday = value;
                RaisePropertyChanged(() => Birthday);
            }
        }

        private string _classNoticeTitle;

        public string ClassNoticeTitle {
            get { return _classNoticeTitle; }
            set {
                if (!string.IsNullOrEmpty(value))
                    _classNoticeTitle = value;
                else
                    _classNoticeTitle = "无新信息";
                RaisePropertyChanged(() => ClassNoticeTitle);
            }
        }

        private string _classNoticeContent;

        public string ClassNoticeContent {
            get { return _classNoticeContent; }
            set {
                if (!string.IsNullOrEmpty(value))
                    _classNoticeContent = value;
                else
                    _classNoticeContent = "无新消息";
                RaisePropertyChanged(() => ClassNoticeContent);
            }
        }

        private string _kindergartenNoticeTitle;

        public string KindergartenNoticeTitle {
            get { return _kindergartenNoticeTitle; }
            set {
                if (!string.IsNullOrEmpty(value))
                    _kindergartenNoticeTitle = value;
                else
                    _kindergartenNoticeTitle = "无新消息";
                RaisePropertyChanged(() => KindergartenNoticeTitle);
            }
        }

        private string _kindergartenNoticeContent;

        public string KindergartenNoticeContent {
            get { return _kindergartenNoticeContent; }
            set {
                if (!string.IsNullOrEmpty(value))
                    _kindergartenNoticeContent = value;
                else
                    _kindergartenNoticeContent = "无新信息";
                RaisePropertyChanged(() => KindergartenNoticeContent);
            }
        }

        public string Pics { get; set; }

        public List<string> ImageList {
            get {
                if (!string.IsNullOrEmpty(Pics)) {
                    return new List<string>(Pics.Split(','));
                }
                else {
                    return new List<string>();
                }
            }
        }
        #endregion

        #region Command

        public IMvxCommand _showFeedbackCommand;

        /// <summary>
        /// Clear Cache, Mainly Clear Image Cache
        /// </summary>
        public IMvxCommand ShowFeedbackCommand {
            get {
                _showFeedbackCommand = _showFeedbackCommand ??
                new MvxCommand(() => ShowViewModel<FeedBackViewModel>());
                return _showFeedbackCommand;
            }
        }

        private IMvxCommand _questionCommand;
        private IMvxCommand _noticeCommand;
        private IMvxCommand _memoryCommand;

        /// <summary>
        /// Clear Cache, Mainly Clear Image Cache
        /// </summary>
        public IMvxCommand QuestionCommand {
            get {
                _questionCommand = _questionCommand ?? new MvxCommand(() => ShowViewModel<SendQuestionViewModel>(new { type = QuestionType.NormalMessage }));
                return _questionCommand;
            }
        }

        private IMvxCommand _askForLeaveCommand;

        public IMvxCommand AskForLeaveCommand {
            get {
                _askForLeaveCommand = _askForLeaveCommand ?? new MvxCommand(() => ShowViewModel<SendQuestionViewModel>(new{type = QuestionType.AskforLeave}));
                return _askForLeaveCommand;
            }
        }

        private IMvxCommand _memoryIndexViewModel;

        public IMvxCommand ShowMemoryIndexViewCommand {
            get {
                _memoryIndexViewModel = _memoryIndexViewModel ?? new MvxCommand(() => ShowViewModel <MemoryIndexViewModel >());
                return _memoryIndexViewModel;
            }
        }

        private IMvxCommand _noticeIndexViewModel;

        public IMvxCommand NoticeIndexViewCommand {
            get {
                _noticeIndexViewModel = _noticeIndexViewModel ?? new MvxCommand(() => ShowViewModel <NoticeIndexViewModel >(new{type = NoticeViewType.Notice}));
                return _noticeIndexViewModel;
            }
        }

        private IMvxCommand _questionIndexViewModel;

        public IMvxCommand QuestionIndexViewCommand {
            get {
                _questionIndexViewModel = _questionIndexViewModel ?? new MvxCommand(() => ShowViewModel<QuestionIndexViewModel>());
                return _questionIndexViewModel;
            }
        }

        #endregion
    }
}
