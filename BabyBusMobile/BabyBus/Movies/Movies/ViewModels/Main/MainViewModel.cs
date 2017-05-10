using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Enums;
using BabyBus.Services.Main;
using BabyBus.Utilities;
using BabyBus.ViewModels.Communication;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.ViewModels;
using BabyBus.ViewModels.Test;
using BabyBus.ViewModels.Login;
using ViewModels.Communication;

namespace BabyBus.ViewModels.Main {
    public class MainViewModel : BaseViewModel {
        private IMainService _service = null;

        public event EventHandler NotifyUpdateVersion;

        public VersionModel Version { get; set; }


        public TeacherHomeViewModel TeacherHomeViewModel { get; private set; }

        public ParentHomeViewModel ParentHomeViewModel { get; private set; }

        public MasterHomeViewModel MasterHomeViewModel { get; private set; }

        public NoticeIndexViewModel NoticeIndexViewModel { get; private set; }

        public QuestionIndexViewModel QuestionIndexViewModel { get; private set; }

        public QuestionDetailViewModel QuestionDetailViewModel { get; private set; }


        //        public NoticeIndexViewModel MemoryIndexViewModel { get; private set; }

        public MemoryIndexViewModel MemoryIndexViewModel { get; private set; }

        public Setting.SettingViewModel SettingViewModel { get; private set; }

        public CommIndexViewModel CommIndexViewModel { get; private set; }



        public MainViewModel() {
            _service = Mvx.Resolve<IMainService>();

            //Permission
            var userInfo = BabyBusContext.UserAllInfo;
            if (userInfo != null) {
            }
            QuestionDetailViewModel = new QuestionDetailViewModel();

            TeacherHomeViewModel = new TeacherHomeViewModel();
            ParentHomeViewModel = new ParentHomeViewModel();
            MasterHomeViewModel = new MasterHomeViewModel();
            SettingViewModel = new Setting.SettingViewModel();
            MemoryIndexViewModel = new MemoryIndexViewModel();
            NoticeIndexViewModel = new NoticeIndexViewModel(NoticeViewType.Notice);
            QuestionIndexViewModel = new QuestionIndexViewModel();
            
//
//            Task.Run(() => {
//                NoticeIndexViewModel.Start();
//                MemoryIndexViewModel.Start();
//            });
            CommIndexViewModel = new CommIndexViewModel();
        }

        public void Init(string tag) {
            Tag = tag;
        }

        public async override void Start() {
            base.Start();

            int appType = (int)AppType;
            Version = await _service.GetVersionByName(appType);
        }

        public string Tag { get; set; }

        private IMvxCommand _updateApkCommand;

        public IMvxCommand UpdateApkCommand {
            get {
                _updateApkCommand = _updateApkCommand ??
                new MvxCommand(async () => {
                    int appType = (int)AppType;
                    Version = await _service.GetVersionByName(appType);
                    if (Version != null && Version.VerCode > VerCode) {
                        ViewModelStatus = new ViewModelStatus("");
                        //Notify UI
                        if (NotifyUpdateVersion != null)
                            NotifyUpdateVersion(this, null);
                    }
                });
                return _updateApkCommand;
            }

        }
    }


}
