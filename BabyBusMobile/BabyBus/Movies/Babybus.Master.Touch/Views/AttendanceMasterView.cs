using System;
using BabyBus.iOS;

using UIKit;
using ObjCRuntime;

using CrossUI.Touch.Dialog.Elements;
using CoreGraphics;

using Foundation;

using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using BabyBus.Logic.Shared;
using Utilities.Touch;

namespace BabyBus.iOS
{
    public class AttendanceMasterView : MvxBabybusDialogViewController
    {
        AttendanceMasterViewModel _baseViewModel = null;

        public AttendanceMasterView()
            : base(UITableViewStyle.Plain,
                   null,
                   true)
        {
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            _baseViewModel = ViewModel as AttendanceMasterViewModel;

            //            TableView.RowHeight = UITableView.AutomaticDimension;
            //            TableView.EstimatedRowHeight = 50;

            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;

            var label = new UILabel(new CGRect(0, 0, 40, 35));
            label.Text = "考勤情况";
            label.TextAlignment = UITextAlignment.Center;
            this.NavigationItem.TitleView = label;
            label.TextColor = MvxTouchColor.White;

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;

            _baseViewModel.FirstLoadedEventHandler += (object sender, EventArgs e) => this.InvokeOnMainThread(() =>
                {
                    //                    var set = this.CreateBindingSet<AttendanceMasterView, AttendanceMasterViewModel>(); 
                    //                    set.Bind(Bindable).For(v => v.ItemsSource).To(vm => vm.Attendances);
                    //                    set.Apply();
                    Bindable.InitData(_baseViewModel.Attendances);

                    var header = Bindable.HeaderView as AttendanceMasterViewHeader;
                    header.Total = Convert.ToString(_baseViewModel.TotalTotal);
                    header.TotalAttence = Convert.ToString(_baseViewModel.TotalAttence);
                    header.TotalUnattence = Convert.ToString(_baseViewModel.TotalUnattence);
                    header.TotalAbsence = Convert.ToString(_baseViewModel.TotalAbsence);
                });

            // Perform any additional setup after loading the view, typically from a nib.
            Root = (RootElement)GetRoot();

            var v = UIImage.FromBundle("calendar.png");
            var doneButton = new UIBarButtonItem(v.ImageByScalingToMaxSize(20, 2), UIBarButtonItemStyle.Done, (sender, args) =>
                {
                    SwithDateControl();
                });
            this.NavigationItem.SetRightBarButtonItem(doneButton, true);

            SwithDateControl();
        }

        RootElement _rootElement;
        BindableSection<AttendanceUIViewElement> bindable = new BindableSection<AttendanceUIViewElement>();

        BindableSection<AttendanceUIViewElement> Bindable{ get { return bindable; } set { bindable = value; } }

        Section _dateSection = new Section("选择考勤日期");
        UIViewElement _calendarView = null;

        void SwithDateControl()
        {
            if (_rootElement.ContainsElement(_dateSection))
            {
                _rootElement.Remove(_dateSection);
            }
            else
            {
                _rootElement.Insert(0, _dateSection);
            }
        }

        RootElement GetRoot()
        {
            _rootElement = new RootElement("");


            var cal = new CalendarView();
            cal.CreateCalendar();

            // Turn gray dates that fulfill the predicate
            cal.IsDateAvailable = date =>
            {
                return (date <= DateTime.Today);
            };

            cal.DateSelected = date =>
            {
                if ((date > DateTime.Today))
                {
                    var alert = new UIAlertView("提示", "您只能选择过去的时间。", null, "知道了", null);
                    alert.Clicked += (object sender, UIButtonEventArgs e) =>
                    {
                    };
                    alert.AlertViewStyle = UIAlertViewStyle.Default;
                    alert.Show();
                }
            };
            cal.DateSelectionFinished = date =>
            {
                Mvx.Trace("Date Selection Finished: {0}", date);
                var alert = new UIAlertView("提示", "您确认要切换日期吗？", null, "放弃", new String []{ "确认" });
                alert.Clicked += (object sender, UIButtonEventArgs e) =>
                {
                    var str = string.Format("UIAlertView click button {0}th", e.ButtonIndex);
                    Mvx.Trace(MvxTraceLevel.Diagnostic, str);
                    if (e.ButtonIndex == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        //切换考勤日期
                        _baseViewModel.Date = date;
                        _baseViewModel.FirstLoad();
                        SwithDateControl();
                    }
                };
                alert.AlertViewStyle = UIAlertViewStyle.Default;
                alert.Canceled += (object sender, EventArgs e) =>
                {
                    Mvx.Trace(MvxTraceLevel.Diagnostic, "Canceled UIAlertView 放弃发送");
                };
                alert.Show();
            };

            _calendarView = new  UIViewElement("选择考勤日期", cal, false);
            _dateSection.Add(_calendarView);
            _rootElement.Add(_dateSection);

            _rootElement.Add(Bindable);
            Bindable.FooterView = new UIView();

            Bindable.HeaderView = new AttendanceMasterViewHeader(new CGRect(0, 0, 320, 50));


            //note: set UnevenRows = true to use SizingSource
            _rootElement.UnevenRows = true;
            return _rootElement;
        }
    }

    public class AttendanceMasterViewHeader : UIView
    {
        public static NSString HeaderId = new NSString("AttendanceMasterViewHeader");
        UILabel _total;
        UILabel _totalAttence;
        UILabel _totalAbsence;
        UILabel _totalUnattence;

        public string Total
        {
            get
            {
                return _total.Text;
            }
            set
            {
                _total.Text = "总人数：" + value;
            }
        }

        public string TotalAttence
        {
            get
            {
                return _totalAttence.Text;
            }
            set
            {
                _totalAttence.Text = "考勤人数：" + value;
            }
        }

        public string TotalAbsence
        {
            get
            {
                return _totalAbsence.Text;
            }
            set
            {
                _totalAbsence.Text = "缺席总人数：" + value;
            }
        }

        public string TotalUnattence
        {
            get
            {
                return _totalUnattence.Text;
            }
            set
            {
                _totalUnattence.Text = "未考勤人数：" + value;
            }
        }

        public AttendanceMasterViewHeader(CGRect frame)
            : base(frame)
        {
            _total = new UILabel();
            _totalAttence = new UILabel();
            _totalAbsence = new UILabel();
            _totalUnattence = new UILabel();

            _total.Font = EasyLayout.ContentFont;
            _total.TextColor = MvxTouchColor.Black1;

            _totalAttence.Font = EasyLayout.ContentFont;
            _totalAttence.TextColor = MvxTouchColor.Black1;

            _totalAbsence.Font = EasyLayout.ContentFont;
            _totalAbsence.TextColor = MvxTouchColor.Black1;

            _totalUnattence.Font = EasyLayout.ContentFont;
            _totalUnattence.TextColor = MvxTouchColor.Black1;
        }

        UIView _contentView = new UIView();

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            UIView[] v =
                {
                    _total, _totalAttence, _totalAbsence, _totalUnattence
                };
            _contentView.AddSubviews(v);
            AddSubviews(_contentView);
            SetUpConstrainLayout();

            //            http://stackoverflow.com/questions/12610783/auto-layout-still-required-after-executing-layoutsubviews-with-uitableviewcel
            //            I had a similar problem not on UITableViewCell but rather on UITableView itself. Because it is the first result in Google I'll post it here. It turned out that viewForHeaderInSection was the problem. I created a UITableViewHeaderFooterView and set translatesAutoresizingMaskIntoConstraints to NO. Now here comes the interesting part:
            //
            //            iOS 7:
            //
            //            // don't do this on iOS 7
            //            sectionHeader.translatesAutoresizingMaskIntoConstraints = NO;
            //            If I do this the app crashes with
            //
            //                Auto Layout still required after executing -layoutSubviews. UITableView's implementation of -layoutSubviews needs to call super.
            //                OK, I thought you can't use auto layout on a table view header and only on the subviews. But that's not the full truth as you see it later. To sum up: Don't disable auto resizing mask for the header on iOS 7. Otherwise it's working fine.
            //
            //                iOS 8:
            //
            //                // you have to do this, otherwise you get an auto layout error
            //                sectionHeader.translatesAutoresizingMaskIntoConstraints = NO;
            //            If I wouldn't use this I'd get the following output:
            //
            //            Unable to simultaneously satisfy constraints.
            //            For iOS 8 you have to disable auto resizing mask for the header.
            //
            //                Don't know why it behaves in this way but it seems that Apple did fix some things in iOS 8 and auto layout is working differently on iOS 7 and iOS 8.
            //Note: if you remove this, you will crash in iOS7
            //            TranslatesAutoresizingMaskIntoConstraints = false;
            base.LayoutSubviews();
        }

        void SetUpConstrainLayout()
        {
            nfloat avgWidth = 320 / 2 - 20;
            nfloat height = EasyLayout.NormalTextFieldHeight;

            this.ConstrainLayout(
                () => 
                _contentView.Frame.Top == Frame.Top
                && _contentView.Frame.Left == Frame.Left + EasyLayout.MarginNormal
                && _contentView.Frame.Right == Frame.Right
                && _contentView.Frame.Bottom == Frame.Bottom

                && _total.Frame.Top == _contentView.Frame.Top
                && _total.Frame.Left == _contentView.Frame.Left
                && _total.Frame.Width == avgWidth
                && _total.Frame.Height == height

                && _totalAttence.Frame.Top == _total.Frame.Top
                && _totalAttence.Frame.Left == _total.Frame.Right
                && _totalAttence.Frame.Width == avgWidth
                && _totalAttence.Frame.Height == height

                && _totalAbsence.Frame.Top == _total.Frame.Bottom
                && _totalAbsence.Frame.Left == _total.Frame.Left
                && _totalAbsence.Frame.Width == avgWidth
                && _totalAbsence.Frame.Height == height

                && _totalUnattence.Frame.Top == _totalAbsence.Frame.Top
                && _totalUnattence.Frame.Left == _totalAbsence.Frame.Right
                && _totalUnattence.Frame.Width == avgWidth
                && _totalUnattence.Frame.Height == height
            );
        }
    }
}

