using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using BabyBusSSApi.ServiceModel.DTO.Update;
using System;


namespace BabyBus.Logic.Shared
{
	public class RePasswordViewModel : BaseViewModel
	{
		private IRemoteService _service;

		public RePasswordViewModel()
		{
			_service = Mvx.Resolve<IRemoteService>();
		}

		MvxCommand _rePasswordCommand = null;

		public MvxCommand RePasswordCommand {
           
			get {
				return  _rePasswordCommand = _rePasswordCommand ?? new MvxCommand(async () => {
					var userAllInfo = BabyBusContext.UserAllInfo;
					if (NewPassword != NewPasswordAgain) {
						ViewModelStatus = new ViewModelStatus("两次新密码不一致。", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					if (NewPassword == OldPassword) {
						ViewModelStatus = new ViewModelStatus("新旧密码一致，请修改。", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
					if (NewPassword.Length < Constants.MiniPasswordLength || NewPasswordAgain.Length < Constants.MiniPasswordLength) {
						ViewModelStatus = new ViewModelStatus("密码的最小长度为6。", false, MessageType.Error, TipsType.DialogDisappearAuto);
						return;
					}
//						var user = new User {LoginName = Username, Password = Password};
//						Result = await _loginService.LoginNew(user);
					var pwd = new UpdatePassword {
						OldPassword = OldPassword,
						NewPassword = NewPassword
					};
                        
					ViewModelStatus = new ViewModelStatus(UIConstants.UPDATING, true, MessageType.Information, TipsType.DialogProgress);
					try {
						await _service.ChangePassword(pwd);
						await _service.Logout();
						ViewModelStatus = new ViewModelStatus(UIConstants.UPDATE_SUCCESS, false, MessageType.Success, TipsType.DialogDisappearAuto);
						ShowViewModel<LoginViewModel>();
					} catch (BabyBusWebServiceException ex) {
						if (ex.IsAny500()) {
							ViewModelStatus = new ViewModelStatus(ex.ErrorMessage, false, MessageType.Error, TipsType.DialogDisappearAuto);
						} else {
							ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
						}
					} catch (Exception ex) {
						Xamarin.Insights.Report(ex);
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
