using BabyBus.iOS;
using Cirrious.MvvmCross.Binding.BindingContext;
using CoreGraphics;
using CrossUI.Touch.Dialog.Elements;
using Foundation;
using ObjCRuntime;
using UIKit;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class ChildInfoView : MvxBabybusDialogViewController
    {
        private ChildInfoViewModel _baseViewModel;

        public ChildInfoView()
            : base(UITableViewStyle.Plain,
                   null,
                   true)
        {
        }

        RootElement GetRoot()
        {

            UIImage image;
            if (_baseViewModel.Bytes == null)
            {
                image = UIImage.FromBundle("placeholder.png");
            }
            else
            {
                var imageData = NSData.FromArray(_baseViewModel.Bytes);
                image = UIImage.LoadFromData(imageData);
            }
            var bindings = this.CreateInlineBindingTarget<ChildInfoViewModel>();

            ChildInfomationElement photoElement = new ChildInfomationElement();
            photoElement.Bind(bindings, e => e.ImageUri, vm => vm.ImageName, "StringToUriThumb", null, null, Cirrious.MvvmCross.Binding.MvxBindingMode.Default);
            photoElement.Bind(bindings, e => e.Caption, vm => vm.ChildName);
            photoElement.Accessory = UITableViewCellAccessory.None;
            photoElement.Alignment = UITextAlignment.Center;
            photoElement.HideSeparator = true;

            StyledStringElement className = new StyledStringElement("班级", _baseViewModel.ClassName, UITableViewCellStyle.Value2);
            className.Alignment = UITextAlignment.Left;
            StyledStringElement birthDay = new StyledStringElement("生日", _baseViewModel.Birthday.ToString("yyyy年M月d日"), UITableViewCellStyle.Value2);
            birthDay.Alignment = UITextAlignment.Left;
            StyledStringElement parentName = new StyledStringElement("家长", _baseViewModel.ParentName, UITableViewCellStyle.Value2);
            parentName.Alignment = UITextAlignment.Left;
            StyledStringElement phone = new StyledStringElement("电话", _baseViewModel.PhoneNumber, UITableViewCellStyle.Value2);
            phone.Alignment = UITextAlignment.Left;
            StyledStringElement address = new StyledStringElement("地址", _baseViewModel.Address, UITableViewCellStyle.Value2);
            address.Alignment = UITextAlignment.Left;
            var section = new Section("")
            {
                photoElement
                , className
                , birthDay
                , parentName
                , phone
                , address
            };
            var footerSection = new Section();
            _rootElement = new RootElement("")
            { 
                section,
            };
           

            return _rootElement;
        }

        RootElement _rootElement;

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;

            _baseViewModel = ViewModel as ChildInfoViewModel;
            var label = new UILabel(new CGRect(0, 0, 40, 35));
            label.Text = "孩子信息";
            label.TextAlignment = UITextAlignment.Center;
            this.NavigationItem.TitleView = label;
            label.TextColor = MvxTouchColor.White;

            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;
            Root = (RootElement)GetRoot();
            this.TableView.TableFooterView = new UIView();
        }
    }
}

