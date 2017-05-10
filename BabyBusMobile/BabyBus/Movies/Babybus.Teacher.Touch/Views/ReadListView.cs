using BabyBus.iOS;
using UIKit;
using BabyBus.Logic.Shared;
using CoreGraphics;
using ObjCRuntime;
using CrossUI.Touch.Dialog.Elements;


namespace BabyBus.iOS
{
    public class ReadListView:MvxBabybusDialogViewController
    {

        ReadListViewModel _baseViewModel = null;

        public ReadListView()
            : base(UITableViewStyle.Plain,
                   null,
                   true)
        {
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ChildInfomationElement.ChildInfoTypeStatic = _oldType;
        }

        ChildInfomationElement.ChildInfoType _oldType;

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            _oldType = ChildInfomationElement.ChildInfoTypeStatic;
            ChildInfomationElement.ChildInfoTypeStatic = ChildInfomationElement.ChildInfoType.IsRead;

            this.TableView.Bounces = false;
            _baseViewModel = ViewModel as ReadListViewModel;

            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;

            var label = new UILabel(new CGRect(0, 0, 40, 35));
            label.Text = "已读未读情况";
            label.TextAlignment = UITextAlignment.Center;
            this.NavigationItem.TitleView = label;
            label.TextColor = MvxTouchColor.White;

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;


            // Perform any additional setup after loading the view, typically from a nib.
            Root = (RootElement)GetRoot();

            _baseViewModel.FirstLoadedEventHandler += (object sender, object e) => this.InvokeOnMainThread(() =>
                {
                    Bindable.InitData(_baseViewModel.Readers);
                });
          
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        RootElement _rootElement;

        BindableSection<ChildInfomationElement> bindable = new BindableSection<ChildInfomationElement>();

        BindableSection<ChildInfomationElement> Bindable{ get { return bindable; } set { bindable = value; } }

        RootElement GetRoot()
        {
            _rootElement = new RootElement("");
            _rootElement.Add(Bindable);
            Bindable.FooterView = new UIView();

            return _rootElement;
        }
    }
}

