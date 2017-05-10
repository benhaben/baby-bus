using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBusSSApi.ServiceModel.Enumeration;




namespace BabyBus.Logic.Shared
{
    public class MainViewModel : BaseViewModel
    {
        private IRemoteService _service = null;

        public event EventHandler NotifyUpdateVersion;

        public VersionModel Version { get; set; }

        public TeacherHomeViewModel TeacherHomeViewModel { get; private set; }

        public ParentHomeViewModel ParentHomeViewModel{ get; private set; }



        public MasterHomeViewModel MasterHomeViewModel { get; private set; }

        public NoticeIndexViewModel NoticeIndexViewModel { get; private set; }

        public QuestionIndexViewModel QuestionIndexViewModel { get; private set; }

        public QuestionDetailViewModel QuestionDetailViewModel { get; private set; }

        public MemoryIndexViewModel MemoryIndexViewModel { get; private set; }

        public SettingViewModel SettingViewModel { get; private set; }

        public CommIndexViewModel CommIndexViewModel { get; private set; }

        public ParentSchoolOnlineViewModel ParentSchoolOnlineViewModel { get; private set; }

        public ParentModalityViewModel ParentModalityViewModel{ get; private set; }

        public FindMoreIndexViewModel FindMoreIndeViewModel{ get; private set; }

        public SettingMainViewModel SettingMainViewModel { get; private set; }

        public TestHomeViewModel TestHomeViewModel{ get; set; }

        public MainViewModel()
        {
            _service = Mvx.Resolve<IRemoteService>();//建立服务器连接

            //Permission
            var userInfo = BabyBusContext.UserAllInfo;//获取用户信息
            QuestionDetailViewModel = new QuestionDetailViewModel();

            if (BabyBusContext.UserAllInfo.RoleType == RoleType.HeadMaster)
            {
                MasterHomeViewModel = new MasterHomeViewModel();
                SettingViewModel = new SettingViewModel();
                NoticeIndexViewModel = new NoticeIndexViewModel();
                QuestionIndexViewModel = new QuestionIndexViewModel();
            }
            else if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent)
            {
                ParentHomeViewModel = new ParentHomeViewModel();
                ParentSchoolOnlineViewModel = new ParentSchoolOnlineViewModel();
                FindMoreIndeViewModel = new FindMoreIndexViewModel();
                TestHomeViewModel = new TestHomeViewModel();
                SettingMainViewModel = new SettingMainViewModel();
            }
            else if (BabyBusContext.UserAllInfo.RoleType == RoleType.Teacher)
            {
                TeacherHomeViewModel = new TeacherHomeViewModel();
                NoticeIndexViewModel = new NoticeIndexViewModel(NoticeViewType.Notice);
                MemoryIndexViewModel = new MemoryIndexViewModel();
                QuestionIndexViewModel = new QuestionIndexViewModel();
                SettingViewModel = new SettingViewModel();
            }//根据用户不同，选择不同的homeview
        }

        public void Init(string tag)
        {
            Tag = tag;
        }

        public async override void Start()
        {
            base.Start();

            //android need this
            int appType = (int)AppType;
            try
            {
                Version = await _service.GetVersionByAppType(appType);
            }
            catch (Exception ex)
            {
                ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogWithCancelButton);
            }
        }

        public string Tag { get; set; }

        private IMvxCommand _updateApkCommand;

        public IMvxCommand UpdateApkCommand
        {
            get
            {
                _updateApkCommand = _updateApkCommand ??
                new MvxCommand(async () =>
                    {
                        int appType = (int)AppType;
                        Version = await _service.GetVersionByAppType(appType);
                        if (Version != null && Version.VerCode > VerCode)
                        {
                            ViewModelStatus = new ViewModelStatus("");
                            //Notify UI
                            if (NotifyUpdateVersion != null)
                                NotifyUpdateVersion(this, null);
                        }
                    });
                return _updateApkCommand;
            }

        }

        public void ShowNoticeDetailViewModel(long noticeId, bool Ishtml)
        {
            if (Ishtml)
            {
                ShowViewModel<NoticeDetailHtmlViewModel>(new {noticeId = noticeId});
            }
            else
            {
                ShowViewModel<NoticeDetailViewModel>(new {noticeId = noticeId});
            }
        }



        public void ShowNoticeDetailHtmlViewModel(long noticeId)
        {
            ShowViewModel<NoticeDetailHtmlViewModel>(new {noticeId = noticeId});
        }

        public void ShowMemoryIndexViewModel()
        {
            ShowViewModel<MemoryIndexViewModel>(new{type = NoticeViewType.GrowMemory});
        }

        public void ShowGrowMemoryDetailViewModel(long noticeId)
        {
            ShowViewModel<MemoryDetailViewModel>(new {noticeId = noticeId});
        }

        public void ShowQuestionDetailCommand(long questionId, bool isLoadRemote = false)
        {
            ShowViewModel<QuestionDetailViewModel>(new { questionId = questionId,isLoadRemote = isLoadRemote});
        }

        public void ShowCourseDetailCommand(long postinfoId)
        {
            ShowViewModel<CourseDetailViewModel>(new {postInfoId = postinfoId,eCColumnType = ECColumnType.Course});
        }

        public void ShowForumDetailCommand(long postinfoId)
        {
            ShowViewModel<ForumDetailViewModel>(new {id = postinfoId});
        }
    }
}
