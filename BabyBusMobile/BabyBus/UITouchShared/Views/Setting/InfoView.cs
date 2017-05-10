using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using CrossUI.Touch.Dialog.Elements;
using ObjCRuntime;
using UIKit;

using BigTed;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class InfoView : MvxBabybusDialogViewController
    {
        //		public SettingView ParentSettingView{ get; set;}

        public InfoView()
            : base(UITableViewStyle.Plain,
                   null,
                   true)
        {
            this.Request = new Cirrious.MvvmCross.ViewModels.MvxViewModelRequest(
                typeof(InfoViewModel), null, null, null
            );

        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        InfoViewModel _baseViewModel = null;

        public override void ViewDidAppear(bool animated)
        {
            //判断并接收返回的参数
            base.ViewDidAppear(animated);
//            _baseViewModel.RaiseAllPropertiesChanged();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
//			ParentSettingView.NameCardPhoto.CurrentPhotoElementCell.ImageView.Image = 
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            if (this.NavigationController != null)
                this.NavigationController.SetNavigationBarHidden(false, false);
            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;

            _baseViewModel = ViewModel as InfoViewModel;
            _baseViewModel.FirstLoadedEventHandler += (object sender1, EventArgs e1) =>
            {
                Section registerSection = new Section("注册信息");

                this.NavigationItem.SetRightBarButtonItem(
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) =>
                        {
                            var vm = this.ViewModel as InfoViewModel;
                            vm.UpdateCommand.Execute();
                            if (this.NavigationController != null)
                                this.NavigationController.PopViewController(true);
                        })
				, true);

                // ios7 layout
                if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                    EdgesForExtendedLayout = UIRectEdge.None;


                var bindings = this.CreateInlineBindingTarget<InfoViewModel>();
                var birthday = new DateTime(2011, 1, 1);

                ProgressHUD.Shared.HudForegroundColor = UIColor.White;
                ProgressHUD.Shared.HudToastBackgroundColor = MvxTouchColor.Gray1;
                ProgressHUD.Shared.HudBackgroundColour = MvxTouchColor.Gray1;

                var photo = new PhotoElement(this)
                .Bind(bindings, e => e.Name, vm => vm.KindergartenName)
                .Bind(bindings, e => e.DescriptionOfChild, vm => vm.ClassNameAndLoginName)
                .Bind(bindings, e => e.ImageUri, vm => vm.ImageName, "StringToUriThumb")
                .Bind(bindings, e => e.ChoosePictureWithCropCommand, vm => vm.ChoosePictureWithCropCommand)
                .Bind(bindings, e => e.TakePictureWithCropCommand, vm => vm.TakePictureWithCropCommand);

                registerSection.Add(photo);
                var birthdayDateElement = new DateElement("生日", birthday).Bind(bindings, e => e.Value, vm => vm.Birthday);

                Element name;
                EntryElement nameParent;

                #if __PARENT__

                var radioBinable = new BindableSection<GenderBindableRadioElement>();
                radioBinable.InitData(_baseViewModel.Genders);
                var genderRadioRootElement = new RootElement("选择性别")
                {
                    radioBinable
                }.Bind(bindings, e => e.RadioSelected, vm => vm.Gender, "GenderModel2Id");
                genderRadioRootElement.Group = new RadioGroup(_baseViewModel.Gender.Gender, _baseViewModel.Gender.Id);
                //TODO: two way binding should be used here, enhance later
                genderRadioRootElement.RadioSelectedChanged += (object o, EventArgs e) =>
                {
                    var ele = o as RootElement;

                    if (_baseViewModel.Gender == null)
                    {
                        _baseViewModel.Gender = GenderModel.CreateMan();
                    }

                    if (ele.RadioSelected == 1)
                    {
                        var man = GenderModel.CreateMan();
                        //WARNING: don't set Gender directly, raise property change will call RadioSelectedChanged again
                        //should find why tow way binding doesn't work
                        _baseViewModel.Gender.Id = man.Id;
                        _baseViewModel.Gender.Gender = man.Gender;

                    }
                    else
                    {
                        var Woman = GenderModel.CreateWoman();
                        _baseViewModel.Gender.Id = Woman.Id;
                        _baseViewModel.Gender.Gender = Woman.Gender;
                    }
                };
                    
                name = new EntryElement("姓名").Bind(bindings, e => e.Value, vm => vm.ChildName);
                nameParent = new EntryElement("家长姓名").Bind(bindings, e => e.Value, vm => vm.RealName);
                registerSection.Add(genderRadioRootElement);
                registerSection.Add(birthdayDateElement);
                registerSection.Add(nameParent);
                #else
                name = new EntryElement("姓名").Bind(bindings, e => e.Value, vm => vm.RealName);
                #endif
                registerSection.Add(name);

                
             
                Root = new RootElement(null)
                {
                    registerSection,
                };
            };
        }

    }
}

