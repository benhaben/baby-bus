
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using BabyBusSSApi.ServiceModel.Enumeration;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace BabyBus.Logic.Shared
{
    

    public class App : MvxApplication
    {
        public App(bool isMonoTouch = false)
        {
            IsMonoTouch = isMonoTouch;
        }

        public bool IsMonoTouch { get; set; }

        public override void Initialize()
        {
            ConvertExtensions.Initiate();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
            
            #if __TOUCH__
//            IosPclExportClient.Configure();
            #endif
          

            #if __PARENT__
            BaseViewModel.AppType = AppType.Parent;
            #elif __TEACHER__
            BaseViewModel.AppType = AppType.Teacher;
            #else
			BaseViewModel.AppType = AppType.Master;
            #endif

            StartApp();
        }

        private void StartApp()
        {

            if (BabyBusContext.UserAllInfo == null)
            {
                RegisterAppStart<LoginViewModel>();
            }
            else
            {
                if ((BabyBusContext.UserAllInfo.RoleType == RoleType.Parent
                    && BabyBusContext.UserAllInfo.Child == null)
                    || BabyBusContext.UserAllInfo.Cookie == null)
                {
                    //UnBind Child, Bind Child First
                    RegisterAppStart<LoginViewModel>();
                }
                else
                {
                    var iremoteService = Mvx.Resolve<IRemoteService>();
                    iremoteService.Cookie = JsonConvert.DeserializeObject <List<Cookie>>(BabyBusContext.UserAllInfo.Cookie);
                    RegisterAppStart<MainViewModel>();
                }
            }
        }



    }
}