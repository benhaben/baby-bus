using Cirrious.MvvmCross.Binding.BindingContext;
using CrossUI.Touch.Dialog.Elements;
using ObjCRuntime;
using UIKit;
using BabyBus.Logic.Shared;


namespace BabyBus.iOS
{
    public class RePasswordView : MvxBabybusDialogViewController
    {
        //      public SettingView ParentSettingView{ get; set;}

        public RePasswordView()
            : base(UITableViewStyle.Plain,
                   null,
                   true)
        {
            this.Request = new Cirrious.MvvmCross.ViewModels.MvxViewModelRequest(
                typeof(RePasswordViewModel), null, null, null
            );

        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        RePasswordViewModel _baseViewModel = null;

        public override void ViewDidAppear(bool animated)
        {
            //判断并接收返回的参数
            base.ViewDidAppear(animated);
            _baseViewModel.RaiseAllPropertiesChanged();

        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            //          ParentSettingView.NameCardPhoto.CurrentPhotoElementCell.ImageView.Image = 
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;

            if (this.NavigationController != null)
                this.NavigationController.SetNavigationBarHidden(false, false);
            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;

            _baseViewModel = ViewModel as RePasswordViewModel;

            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) =>
                    {

                        _baseViewModel.RePasswordCommand.Execute();

                        if (this.NavigationController != null && _baseViewModel.IsSuccessStatus)
                            this.NavigationController.PopViewController(true);
                    })
                , true);

            var bindings = this.CreateInlineBindingTarget<RePasswordViewModel>();

            var oldPassword = new EntryElement("旧密码：").Bind(bindings, e => e.Value, vm => vm.OldPassword);
            var newPassword = new EntryElement("新密码：").Bind(bindings, e => e.Value, vm => vm.NewPassword);
            var newPasswordAgain = new EntryElement("新密码：").Bind(bindings, e => e.Value, vm => vm.NewPasswordAgain);
            oldPassword.IsPassword = true;
            newPassword.IsPassword = true;
            newPasswordAgain.IsPassword = true;

            Root = new RootElement(null)
            {
                new Section("")
                {
                    oldPassword,
                    newPassword,
                    newPasswordAgain
                }
            };
        }

    }
}

