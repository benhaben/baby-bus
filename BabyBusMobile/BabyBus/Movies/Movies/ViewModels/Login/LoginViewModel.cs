using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using BabyBus.Helpers;
using BabyBus.Manager;
using BabyBus.Models;
using BabyBus.Models.Enums;
using BabyBus.Models.Main;
using BabyBus.Net.Login;
using BabyBus.Services;
using BabyBus.Utilities;
using BabyBus.Utilities.Enum;
using BabyBus.ViewModels.Main;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBus.Services.Main;
using BabyBus.SQLite;
using Newtonsoft.Json;

namespace BabyBus.ViewModels.Login {

    public class LoginViewModel : BaseViewModel {
        private readonly ILoginService _loginService;

        public event EventHandler<string> ReCheckout;
        //private readonly IDataService _dataService;
        //        private readonly IMainService _mainService;

        public LoginViewModel() {
            _loginService = Mvx.Resolve<ILoginService>();
            //            var environment = Mvx.Resolve<IEnvironmentService>();
            //            _dataService = new DataService(environment);
        }

        public override void Start() {
            //check cache
            //            if (BabyBusContext.UserAllInfo != null)
            //            {
            //                var user = BabyBusContext.UserAllInfo;
            //                if (user != null)
            //                {
            //                    Username = user.LoginName;
            //                    Password = user.Password;
            //                }
            //            }
            base.Start();
        }

        private int _userId;

        public int UserId {
            get { return _userId; }
            set {
                _userId = value;
                RaisePropertyChanged(() => UserId);
            }
        }

        private string _username = null;

        public string Username {
            get { return _username; }
            set {
                _username = value.Trim();
                RaisePropertyChanged(() => Username);
            }
        }

        private string _password = string.Empty;

        public string Password {
            get { return _password; }
            set {
                _password = value.Trim();
                RaisePropertyChanged(() => Password);
            }
        }

        private ApiResponser _result;

        public ApiResponser Result {
            get { return _result; }
            set {
                _result = value;
                RaisePropertyChanged(() => Result);
            }
        }



        private ChildAllInfo _childID;

        public ChildAllInfo ChildID {
            set {
                _childID = value;

                if (_childID.Child.ChildId == 0) {
                    ShowViewModel<RegisterDetailViewModel>();
                } else {
                    ShowViewModel<MainTabbedViewModel>();
                }
            }
        }

        private MvxCommand _loginCommand;

        public MvxCommand LoginCommand {
            get {
                _loginCommand = _loginCommand ?? new MvxCommand(async () => {
                    if (string.IsNullOrEmpty(Username)) {
                        ViewModelStatus = new ViewModelStatus("请输入用户名。", false, MessageType.Information, TipsType.DialogDisappearAuto);
                    } else if (string.IsNullOrEmpty(Password)) {
                        ViewModelStatus = new ViewModelStatus("请输入密码。", false, MessageType.Information, TipsType.DialogDisappearAuto);
                    } else {
                        ViewModelStatus = new ViewModelStatus("正在加载...", true, MessageType.Information, TipsType.DialogProgress);

                        var user = new User { LoginName = Username, Password = Password };
                        Result = await _loginService.Login(user);
                        if (Result.Status) {
                            user = JsonConvert.DeserializeObject<User>(Result.Attach.ToString());
                            BabyBusContext.UserAllInfo = user;
                            if (AppType == AppType.Parent && user.RoleType != RoleType.Parent) {
                                ViewModelStatus = new ViewModelStatus("您不是家长用户，请选择适合您的APP", false, MessageType.Error,
                                    TipsType.DialogDisappearAuto);
                            } else if (AppType == AppType.Teacher && user.RoleType != RoleType.Teacher) {
                                ViewModelStatus = new ViewModelStatus("您不是老师用户，请选择适合您的APP", false, MessageType.Error,
                                    TipsType.DialogDisappearAuto);
                            } else if (AppType == AppType.Master && user.RoleType != RoleType.Master) {
                                ViewModelStatus = new ViewModelStatus("您不是园长用户，请选择适合您的APP", false,
                                    MessageType.Error,
                                    TipsType.DialogDisappearAuto);
                            } else if (user.RoleType == RoleType.Parent && user.ChildId == 0) {
                                if (user.IsCheckout) {
                                    if (user.CheckoutAuditType == AuditType.Pending) {
                                        ViewModelStatus = new ViewModelStatus("您家宝宝的信息正在审批，请稍后");
                                    } else {
                                        ViewModelStatus = new ViewModelStatus("");
                                        //ViewModelStatus = new ViewModelStatus("您家宝宝的信息审批未通过："+user.CheckoutMemo);
                                        //TODO:Info User whether bind child again?
                                        if (ReCheckout != null) {
                                            ReCheckout(this, "您家宝宝的信息审批未通过：" + user.CheckoutMemo + "。是否重新绑定幼儿？");
                                        }
                                    }
                                } else {
                                    ViewModelStatus = new ViewModelStatus("您尚未绑定孩子，正在跳转绑定页面...", false,
                                        MessageType.Information,
                                        TipsType.DialogDisappearAuto);
                                    
                                    ShowViewModel<RegisterDetailViewModel>();
                                }
                            } else if (user.Kindergarten == null) {
                                ViewModelStatus = new ViewModelStatus("您所在的幼儿园信息有误，请联系客服人员", false,
                                    MessageType.Error,
                                    TipsType.DialogDisappearAuto);
                            } else if (user.RoleType != RoleType.Master && user.Class == null) {
                                ViewModelStatus = new ViewModelStatus("您所在的班级信息有误，请联系客服人员", false,
                                    MessageType.Error,
                                    TipsType.DialogDisappearAuto);
                            } else if (user.RoleType == RoleType.Parent && user.Child == null) {
                                ViewModelStatus = new ViewModelStatus("您的孩子信息有误，请联系客服人员", false,
                                    MessageType.Error,
                                    TipsType.DialogDisappearAuto);
                            } else {
                                ViewModelStatus = new ViewModelStatus("登录成功，正在跳转主页面...", false,
                                    MessageType.Information,
                                    TipsType.DialogDisappearAuto);
                                //Task.Run(() => UpdateManager.UpdateAll());
                                ShowViewModel<MainViewModel>();

                            }
                        } else {
                            ViewModelStatus = new ViewModelStatus(Result.Message, false, MessageType.Error, TipsType.DialogWithCancelButton);
                        }
                    }
                });
                return _loginCommand;
            }
        }


        public MvxCommand GotoRegisterCommand {
            get {
                return new MvxCommand(() => ShowViewModel<RegisterViewModel>());
            }
        }

        public MvxCommand GotoRegisterDetailCommand {
            get {
                return new MvxCommand(() => ShowViewModel<RegisterDetailViewModel>());
            }
        }

    }
}
