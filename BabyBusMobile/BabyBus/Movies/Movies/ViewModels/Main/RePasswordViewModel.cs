using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BabyBus.Helpers;
using BabyBus.Services.Main;
using BabyBus.Utilities;
using Cirrious.MvvmCross.ViewModels;
using BabyBus.ViewModels.Login;

namespace BabyBus.ViewModels.Main {
    public class RePasswordViewModel : BaseViewModel {
        private IMainService _service;

        public RePasswordViewModel(IMainService service) {
            _service = service;
        }

        MvxCommand _rePasswordCommand = null;

        public MvxCommand RePasswordCommand {
           
            get {
                return  _rePasswordCommand = _rePasswordCommand ?? new MvxCommand(async () => {
                    var userAllInfo = BabyBusContext.UserAllInfo;
                    if (NewPassword != NewPasswordAgain) {
                        ViewModelStatus = new ViewModelStatus("两次新密码不一致。");
                        return;
                    }
                    if (NewPassword == OldPassword) {
                        ViewModelStatus = new ViewModelStatus("新旧密码一致，请修改。");
                        return;
                    }
                    if (NewPassword.Length < Constants.MiniPasswordLength || NewPasswordAgain.Length < Constants.MiniPasswordLength) {
                        ViewModelStatus = new ViewModelStatus("密码的最小长度为6。");
                        return;
                    }
//						var user = new User {LoginName = Username, Password = Password};
//						Result = await _loginService.LoginNew(user);
                    var pwd = new {
							userAllInfo.UserId,
							OldPassword,
							NewPassword
						};
                    var result = await _service.ChangePassword(pwd);
                    if (result.Status) {
                        ViewModelStatus = new ViewModelStatus("修改成功。");
                        ShowViewModel<LoginViewModel>();
                    } else {
                        ViewModelStatus = new ViewModelStatus(result.Message);
                    }
                });
            }
        }

        public ICommand CancelCommand {
            get {
                return new MvxCommand(async () => {
                    await Task.Delay(1);
                });
            }
        }

        private string _newPassword = string.Empty;

        public string NewPassword {
            get { return _newPassword; }
            set {
                _newPassword = value;
                RaisePropertyChanged(() => NewPassword);
            }
        }

        private string _oldPassword = string.Empty;

        public string OldPassword {
            get { return _oldPassword; }
            set {
                _oldPassword = value;
                RaisePropertyChanged(() => OldPassword);
            } 
        }

        private string _newPasswordAgain = string.Empty;

        public string NewPasswordAgain {
            get { return _newPasswordAgain; }
            set {
                _newPasswordAgain = value;
                RaisePropertyChanged(() => NewPasswordAgain);
            }
        }


    }
}
