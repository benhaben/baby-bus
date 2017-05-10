
using Android.App;
using Android.OS;
using Android.Widget;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Member
{
	[Activity(Label = "@string/mine_label_repwd", NoHistory = true, Theme = "@style/CustomTheme")]
	public class RePasswordView : ViewBase<RePasswordViewModel>
	{
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			SetCustomTitleWithBack(Resource.Layout.Page_Mine_RePassword, Resource.String.mine_label_repwd);

			var logout = FindViewById<Button>(Resource.Id.logout_button);
			logout.Click += (sender, args) => {
				if (ViewModel.NewPassword != ViewModel.NewPasswordAgain) {
					this.ShowInfo(Resources.GetString(Resource.String.mine_info_pwdnotsame));

					return;
				}
				if (ViewModel.NewPassword.Length < Constants.MiniPasswordLength ||
				                ViewModel.NewPasswordAgain.Length < Constants.MiniPasswordLength) {
					this.ShowInfo(Resources.GetString(Resource.String.mine_info_minlength));
					return;
				}
				this.ShowConfirm(Resources.GetString(Resource.String.mine_label_repwd)
					, Resources.GetString(Resource.String.mine_info_haveyourepwd), () => ViewModel.RePasswordCommand.Execute());
			};
		}
	}
}