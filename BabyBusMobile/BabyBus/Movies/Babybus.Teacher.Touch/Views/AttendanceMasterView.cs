using System;
using BabyBus.iOS;
using UIKit;
using Cirrious.MvvmCross.Binding.BindingContext;
using CrossUI.Touch.Dialog.Elements;
using CoreGraphics;
using ObjCRuntime;
using Utilities.Touch;
using Cirrious.CrossCore;
using UITouchShared;
using Cirrious.CrossCore.Platform;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class AttendanceMasterView :MvxBabybusDialogViewController
    {
        AttendanceMasterViewModel _baseViewModel = null;

        public AttendanceMasterView()
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
                    _attendanceUIView.Frame = new CGRect(0, 0, 320, 190);
                    _attendanceUIView.BackgroundColor = MvxTouchColor.White;
                    AttendanceUIView.CheckButton.TouchUpInside += (object sender, EventArgs e) =>
                    {
                        _baseViewModel.ShowDetailCommand.Execute();
                    };
                }

                return _attendanceUIView;
            }
        }

        public bool IsAttence
        {
            get;
            set;
        }

        ChildInfomationElement.ChildInfoType _oldType;

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ChildInfomationElement.ChildInfoTypeStatic = _oldType;
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            _oldType = ChildInfomationElement.ChildInfoTypeStatic;
            ChildInfomationElement.ChildInfoTypeStatic = ChildInfomationElement.ChildInfoType.AskForLeave;

            this.TableView.Bounces = false;
            _baseViewModel = ViewModel as AttendanceMasterViewModel;

            UIBarButtonItem backItem = new UIBarButtonItem();
            backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
            this.NavigationItem.BackBarButtonItem = backItem;

            var label = new UILabel(new CGRect(0, 0, 40, 35));
            label.Text = "考勤";
            label.TextAlignment = UITextAlignment.Center;
            this.NavigationItem.TitleView = label;
            label.TextColor = MvxTouchColor.White;

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;


            // Perform any additional setup after loading the view, typically from a nib.
            Root = (RootElement)GetRoot();

            _baseViewModel.FirstLoadedEventHandler += (object sender, EventArgs e) => this.InvokeOnMainThread(() =>
                {
                    Bindable.InitData(_baseViewModel.Children);

                    var set = this.CreateBindingSet<AttendanceMasterView, AttendanceMasterViewModel>();
                    set.Bind(AttendanceUIView.TheClassName).To(vm => vm.ClassName);
                    set.Bind(AttendanceUIView.AttenceNumLabel).To(vm => vm.Attence);
                    set.Bind(AttendanceUIView.UnattenceNumLabel).To(vm => vm.UnAttence);
                    set.Bind(AttendanceUIView.IsAttenceLabel).To(vm => vm.AttenceHint);
                    set.Apply();
                   
                    if (_baseViewModel.IsAttence)
                    {
                        AttendanceUIView.IsAttenceLabel.Font = EasyLayout.SmallFont;
                        AttendanceUIView.IsAttenceLabel.TextColor = MvxTouchColor.Black1;
                    }
                    else
                    {
                        AttendanceUIView.IsAttenceLabel.Font = UIFont.BoldSystemFontOfSize(12);
                        AttendanceUIView.IsAttenceLabel.TextColor = MvxTouchColor.Black1;
                    }
                    //can not work...
//                    if (_baseViewModel.Date.Date != DateTime.Now.Date)
//                    {
//                        if (_baseViewModel.IsAttence)
//                        {
//                            AttendanceUIView.IsAttenceLabel.Text = _baseViewModel.Date.ToShortDateString() + "已经考勤";
//                        }
//                        else
//                        {
//                            AttendanceUIView.IsAttenceLabel.Text = _baseViewModel.Date.ToShortDateString() + "还没有考勤";
//                        }
//                    }
                    this.TableView.ReloadData();
                });
            var v = UIImage.FromBundle("calendar.png");
            var doneButton = new UIBarButtonItem(v.ImageByScalingToMaxSize(20, 2), UIBarButtonItemStyle.Done, (sender, args) =>
                {
                    SwithDateControl();
                });
            this.NavigationItem.SetRightBarButtonItem(doneButton, true);

            SwithDateControl();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _baseViewModel.FirstLoad();
            _baseViewModel.FirstLoadedEventHandler += (object sender, EventArgs e) => this.InvokeOnMainThread(() =>
                {
                    _baseViewModel.RaiseAllPropertiesChanged();
                });
        }

        RootElement _rootElement;

        BindableSection<ChildInfomationElement> bindable = new BindableSection<ChildInfomationElement>();

        BindableSection<ChildInfomationElement> Bindable{ get { return bindable; } set { bindable = value; } }

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
//            _dateSection.Add(_dateElement);

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
            Bindable.HeaderView = AttendanceUIView;
            Bindable.FooterView = new UIView();
           
            return _rootElement;
        }

    }


}

