using System;
using UIKit;
using CrossUI.Touch.Dialog.Elements;
using BabyBus.iOS;
using CoreGraphics;
using ObjCRuntime;
using BabyBus.Logic.Shared;


namespace BabyBus.iOS
{
    public class ChildrenView :MvxBabybusDialogViewController
    {
        ChildrenViewModel _baseViewModel = null;

        public ChildrenView()
            : base(UITableViewStyle.Plain,
                   null,
                   true)
        {
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;


            _baseViewModel = ViewModel as ChildrenViewModel;
            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;

            var label = new UILabel(new CGRect(0, 0, 40, 35));
            label.Text = "幼儿列表";
            label.TextAlignment = UITextAlignment.Center;
            this.NavigationItem.TitleView = label;
            label.TextColor = MvxTouchColor.White;

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;

            _baseViewModel.FirstLoadedEventHandler += (object sender, EventArgs e) => this.InvokeOnMainThread(() =>
                {
                    SectionOfChildren.InitData(_baseViewModel.Children);
//                    var set = this.CreateBindingSet<ChildrenView, ChildrenViewModel>(); 
//                    set.Bind(Bindable).For(v => v.ItemsSource).To(vm => vm.Children);
//                    set.Apply();
                });

            // Perform any additional setup after loading the view, typically from a nib.

            Root = (RootElement)GetRoot();
        }

        RootElement _rootElement;

        RootElement GetRoot()
        {
            _rootElement = new RootElement("");
            var footerSection = new Section();
            _rootElement.Add(SectionOfChildren);
            _rootElement.Add(footerSection);
            SectionOfChildren.FooterView = new UIView();
            return _rootElement;
        }

        BindableSection<ChildInfomationElement> bindable = new BindableSection<ChildInfomationElement>();

        BindableSection<ChildInfomationElement> SectionOfChildren{ get { return bindable; } set { bindable = value; } }
    }
}

