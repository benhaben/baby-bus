using System;
using System.Threading.Tasks;
using System.Windows.Input;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Net.Login;
using BabyBus.Services;
using BabyBus.Utilities;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using BabyBus.Models.Enums;

namespace BabyBus.ViewModels.Login
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly ILoginService _loginService;

        public RegisterViewModel()
        {
            _loginService = Mvx.Resolve<ILoginService>();
        }

        public bool IsVaild()
        {
            if (Phone.Length <= 0
                || Password.Length <= 0
                || ConfirmedPassword.Length <= 0)
            {
                ViewModelStatus = new ViewModelStatus("手机号和密码不能为空", false, MessageType.Error, TipsType.Bubble);
                return false;
            }
            if (!Utils.IsHandset(Phone))
            {
                ViewModelStatus = new ViewModelStatus("请输入正确的手机号", false, MessageType.Error, TipsType.Bubble);
                return false;
            }
            if (!Password.Equals(ConfirmedPassword))
            {
                ViewModelStatus = new ViewModelStatus("两次输入的密码不一致", false, MessageType.Error, TipsType.DialogWithOkButton);
                return false;
            }

            if (Password.Length < Constants.MiniPasswordLength || ConfirmedPassword.Length < Constants.MiniPasswordLength)
            {
                ViewModelStatus = new ViewModelStatus("密码的最小长度为6", false, MessageType.Error, TipsType.DialogWithOkButton);
                return false;
            }

            if (!AgreeWithDocuments)
            {
                ViewModelStatus = new ViewModelStatus("请阅读并同意服务条款", false, MessageType.Warning, TipsType.DialogWithOkButton);
                return false;
            }
            return true;         
        }

        private string _phone = string.Empty;

        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
            }
        }

        private string _password = string.Empty;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
            }
        }

        private string _confirmedpassword = string.Empty;

        public string ConfirmedPassword
        {
            get { return _confirmedpassword; }
            set
            {
                _confirmedpassword = value;
            }
        }

        private ApiResponser _result;

        public ApiResponser Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged(() => Result);
                Mvx.Trace(Result.ToString());
            }
        }

        private Cirrious.MvvmCross.ViewModels.MvxCommand _detialRegisterCommand;

        public MvxCommand DetialRegisterCommand
        {
            get
            {
                _detialRegisterCommand = _detialRegisterCommand ?? new MvxCommand(async () =>
                    {
                        Mvx.Trace("click DetialRegisterCommand......");
                        if (!IsVaild())
                        {
                            return;
                        }

                        ViewModelStatus = new ViewModelStatus("正在注册...", true, MessageType.Information, TipsType.DialogDisappearAuto);

                        var user = new User
                        {
                            LoginName = Phone
                        , PhoneNumber = Phone
                        , RealName = Phone
                        , Password = Password
                        , RoleType = RoleType.Parent
                        };
                        ViewModelStatus = new ViewModelStatus("正在注册...", true, MessageType.Information, TipsType.DialogDisappearAuto);

                        Result = await _loginService.Register(user);

                        if (Result.Status)
                        {
                            ViewModelStatus = new ViewModelStatus(Result.Message, false, MessageType.Information, TipsType.DialogDisappearAuto);
                            user.UserId = Convert.ToInt32(Result.Attach);
                            BabyBusContext.UserAllInfo = user;
                            ShowViewModel<RegisterDetailViewModel>();
                        }
                        else
                        {
                            ViewModelStatus = new ViewModelStatus(Result.Message, false, MessageType.Error, TipsType.DialogWithOkButton);
                        }
                    });
                return _detialRegisterCommand;
            }

        }

        bool _agreeWithDocuments = false;

        public bool AgreeWithDocuments
        {
            get { return _agreeWithDocuments; }
            set
            {
                _agreeWithDocuments = value;
                RaisePropertyChanged(() => AgreeWithDocuments);
            }
        }

        private Cirrious.MvvmCross.ViewModels.MvxCommand _showContractCommand;

        public MvxCommand ShowContractCommand
        {
            get
            {
                _showContractCommand = _showContractCommand ?? new MvxCommand(() =>
                    {
                        ViewModelStatus = new ViewModelStatus("服务协议在睿莱网站", false, MessageType.Information, TipsType.DialogDisappearAuto);

//					Information = "请输入正确的手机号码和密码";
                    });
                return _showContractCommand;
            }
        }
    }
}
