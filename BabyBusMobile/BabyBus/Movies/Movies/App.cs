using BabyBus.Models;
using BabyBus.Resx;
using BabyBus.Services;
using BabyBus.Utilities;
using BabyBus.ViewModels.Login;
using BabyBus.ViewModels.Main;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;

namespace BabyBus {
    public class App : MvxApplication {
        public App(bool isMonoTouch = false) {
            IsMonoTouch = isMonoTouch;
        }

        public bool IsMonoTouch { get; set; }

        public override void Initialize() {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.RegisterSingleton(new ResxTextProvider(Resources.ResourceManager));

            //it's not nessary to setlocale here, but if user change locale, you can call the method
            //Mvx.Resolve<IEnvironmentService> ().SetLocale ();

            string str1 = Localize.GetString("String1");
            Mvx.Trace(str1);
            Mvx.Trace(Resources.String1);

            //RegisterAppStart<ArticlePublishViewModel>();
            //RegisterAppStart<MainTabbedViewModel>();
//            RegisterAppStart<RegisterDetailViewModel>();
//            RegisterAppStart<RegisterViewModel>();
//            RegisterAppStart<SendNoticeViewModel>();
//            RegisterAppStart<NoticeIndexViewModel> ();
            //RegisterAppStart(new LoginMvxAppStart<LoginBaseViewModel>());
            //RegisterAppStart<ChildrenBaseViewModel>();
//            RegisterAppStart<MainViewModel>();
//			RegisterAppStart<> ();
            StartApp();
        }

        private void StartApp() {
            if (BabyBusContext.UserAllInfo == null) {
                RegisterAppStart<LoginViewModel>();
            } else {
                if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent
                    && BabyBusContext.UserAllInfo.Child == null) {
                    //UnBind Child, Bind Child First
                    RegisterAppStart<LoginViewModel>();
                } else {
                    RegisterAppStart<MainViewModel>();
                }
            }
        }
    }
}