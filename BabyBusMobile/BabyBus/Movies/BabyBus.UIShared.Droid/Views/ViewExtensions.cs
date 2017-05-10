using System;
using Android.App;
using AndroidHUD;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views
{
	public static class ViewExtensions
	{
		public static void ShowInfo<TViewModel>(this ViewBase<TViewModel> view) where TViewModel : BaseViewModel {
			ShowInfo(view.ViewModel, view);
		}

		public static void ShowInfo<TViewModel>(this FragmentViewBase<TViewModel> view)
			where TViewModel : BaseViewModel {
			ShowInfo(view.ViewModel, view);
		}

		static void ShowInfo<TViewModel>(TViewModel viewmodel, Activity view) 
			where TViewModel : BaseViewModel {
			if (view.IsFinishing) {
				return;
			}

			if (viewmodel.ViewModelStatus.TipsType == TipsType.Undisplay) {
				AndHUD.Shared.Dismiss(view);
				return;
			}
			if (viewmodel.ViewModelStatus.MessageType == MessageType.Success) {
				AndHUD.Shared.ShowSuccess(view, viewmodel.ViewModelStatus.Information
					, MaskType.Clear, TimeSpan.FromSeconds(1));	
				return;
			}
			if (viewmodel.ViewModelStatus.MessageType == MessageType.Error) {
				AndHUD.Shared.ShowError(view, viewmodel.ViewModelStatus.Information
					, MaskType.Clear, TimeSpan.FromSeconds(1));	
				return;
			}
			if (viewmodel.ViewModelStatus.Information == string.Empty) {
				AndHUD.Shared.Dismiss(view);
				return;
			}
			if (!viewmodel.ViewModelStatus.IsRunning) {
				AndHUD.Shared.ShowToast(view, viewmodel.ViewModelStatus.Information,
					MaskType.Clear, TimeSpan.FromSeconds(2), true);
				return;
			}             

			AndHUD.Shared.Show(view, viewmodel.ViewModelStatus.Information, -1, MaskType.None, TimeSpan.FromSeconds(50)
				, null, true, () => AndHUD.Shared.Dismiss(view));
		}

		public static void ShowInfo(this Activity view, string message) {
			AndHUD.Shared.ShowToast(view, message,
				MaskType.None, TimeSpan.FromSeconds(1), true);
		}

		public static void ShowInfoWithCancel(this Activity view, string message) {
			AndHUD.Shared.ShowToast(view, message,
				MaskType.None, TimeSpan.FromSeconds(-1), true
                    , () => AndHUD.Shared.Dismiss(view)
                    , () => AndHUD.Shared.Dismiss(view));
		}

		public static void HideInfo(this Activity view) {
			if (view.IsFinishing) {
				return;
			}
			AndHUD.Shared.Dismiss(view);
		}

		public static void ShowConfirm(this Activity view, string title, string content, Action action) {
			new AlertDialog.Builder(view)
                .SetTitle(title)
                .SetMessage(content)
                .SetPositiveButton("确定", (sender, args) => action.Invoke())
                .SetNegativeButton("取消", (sender, args) => {
			})
                .Show();
		}
	}
}
