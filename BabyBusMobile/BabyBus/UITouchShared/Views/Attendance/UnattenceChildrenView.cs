using System;
using BabyBus.iOS;
using BabyBus.ViewModels.Attendance;
using UIKit;
using CoreGraphics;
using ObjCRuntime;
using CrossUI.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Binding.BindingContext;
using Views.BindableElements;
using BabyBus;
using Cirrious.MvvmCross.Plugins.Color.Touch;
using Foundation;

namespace Views.Attendance
{
    public class UnattenceChildrenView :MvxBabybusDialogViewController
    {
        UnattenceChildrenViewModel _baseViewModel = null;

        public UnattenceChildrenView()
            : base(UITableViewStyle.Plain,
                   null,
                   true)
        {
        }

        AttendanceUIView _attendanceUIView;

        public AttendanceUIView AttendanceUIView
        {
            get
            {
                if (_attendanceUIView == null)
                {   
                    _attendanceUIView = new AttendanceUIView();
                    _attendanceUIView.Frame = new CGRect(0, 0, 320, 150);
                    _attendanceUIView.Layer.BorderWidth = EasyLayout.BorderWidth;
                    _attendanceUIView.Layer.BorderColor = MvxTouchColor.Gray3.CGColor;
                    _attendanceUIView.Layer.CornerRadius = EasyLayout.CornerRadius;
                    _attendanceUIView.Layer.MasksToBounds = true;
                    _attendanceUIView.BackgroundColor = MvxTouchColor.Gray3;
                    _attendanceUIView.IsAttenceLabel.BackgroundColor = MvxTouchColor.Gray1;
                    UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() =>
                        {
                            _baseViewModel.ShowDetailCommand.Execute();
                        });
                    tapGesture.NumberOfTapsRequired = 1;  
                    tapGesture.NumberOfTouchesRequired = 1; 
                    _attendanceUIView.AddGestureRecognizer(tapGesture);
                    _attendanceUIView.UserInteractionEnabled = true;
                    //                    _attendanceUIView.TouchUpInside += (object sender, EventArgs e) => {
                    //                        _baseViewModel.ShowDetailCommand.Execute();
                    //                    };
                }

                return _attendanceUIView;
            }
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();

            _baseViewModel = ViewModel as UnattenceChildrenViewModel;

            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;

            var label = new UILabel(new CGRect(0, 0, 40, 35));
            label.Text = "缺席儿童";
            label.TextAlignment = UITextAlignment.Center;
            this.NavigationItem.TitleView = label;
            label.TextColor = MvxTouchColor.White;

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;


            // Perform any additional setup after loading the view, typically from a nib.
            Root = (RootElement)GetRoot();

           
        }

       

        RootElement _rootElement;

        BindableSection<ChildInfomationElement> bindable = new BindableSection<ChildInfomationElement>();

        BindableSection<ChildInfomationElement> Bindable{ get { return bindable; } set { bindable = value; } }

        RootElement GetRoot()
        {
            _rootElement = new RootElement("");
            _rootElement.Add(Bindable);
            Bindable.HeaderView = AttendanceUIView;
            Bindable.FooterView = new UIView();
            //note: set UnevenRows = true to use SizingSource
            _rootElement.UnevenRows = true;
            return _rootElement;
        }

    }

}

