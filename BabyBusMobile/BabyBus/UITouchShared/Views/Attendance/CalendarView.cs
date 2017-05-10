using System;

using UIKit;
using CoreGraphics;


namespace BabyBus.iOS
{
    public class MyWrapView:UIView
    {
        public MyWrapView()
        {
        }

        public MyWrapView(CGRect frame)
            : base(frame)
        {
        }

        public override bool CanBecomeFirstResponder
        {
            get
            {
                return base.CanBecomeFirstResponder;
            }
        }

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
        }

        public override UIView HitTest(CoreGraphics.CGPoint point, UIEvent uievent)
        {
            // 当touch point是在_btn上，则hitTest返回_btn
//            CGPoint btnPointInA = [_btn convertPoint:point fromView:self];
//            if ([_btn pointInside:btnPointInA withEvent:event]) {
//                return _btn;
//            }

            // 否则，返回默认处理
            return base.HitTest(point, uievent);
        }
    }

    public sealed class CalendarView:UIView
    {
        private FMCalendar _fmCalendar;
        MyWrapView _offsetView = null;
        MyWrapView _adjustView = null;

        public override UIColor BackgroundColor
        {
            get
            {
                return base.BackgroundColor;
            }
            set
            {
                _offsetView.BackgroundColor = value;
                _adjustView.BackgroundColor = value;
                base.BackgroundColor = value;
            }
        }

        public CalendarView(float withd = 320f)
        {
            var widthFMCalendar = FMCalendar.MainViewSize.Width;
            var heightFMCalendar = FMCalendar.MainViewSize.Height;
            _adjustView = new MyWrapView(new CGRect(0, 0, withd, heightFMCalendar));

            var x = (withd - widthFMCalendar) / 2;
            _offsetView = new MyWrapView(new CGRect(x, 0, widthFMCalendar, heightFMCalendar));

            _adjustView.AddSubview(_offsetView);
            _offsetView.UserInteractionEnabled = true;
            _adjustView.UserInteractionEnabled = true;
            _adjustView.BackgroundColor = MvxTouchColor.White;
            this.AddSubview(_adjustView);

//            http://www.cocoachina.com/ios/20140925/9755.html
            this.Bounds = new CGRect(0, 0, withd, heightFMCalendar);
            //call setBounds first, bounds will make Frame inflate
            this.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
        }

        public override CGRect Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;
            }
        }

        public override CGRect Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;
            }
        }

        public override bool CanBecomeFirstResponder
        {
            get
            {
                return base.CanBecomeFirstResponder;
            }
        }

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
        }

        public override UIView HitTest(CGPoint point, UIEvent uievent)
        {
            return base.HitTest(point, uievent);
        }

        /// <summary>
        /// Fired when new date selected.
        /// </summary>
        public Action<DateTime> DateSelected
        {
            set
            {
                _fmCalendar.DateSelected += value;
            }
        }

        /// <summary>
        /// Fired when date selection finished
        /// </summary>
        public Action<DateTime> DateSelectionFinished
        {
            set
            {
                _fmCalendar.DateSelectionFinished += value;
            }
        }

        /// <summary>
        /// Fired when Selected month changed
        /// </summary>
        public Action<DateTime> MonthChanged
        {
            set
            {
                _fmCalendar.MonthChanged += value;
            }
        }

        /// <summary>
        /// Mark with a dot dates that fulfill the predicate
        /// </summary>
        public Func<DateTime, bool> IsDayMarkedDelegate
        {
            set
            {
                _fmCalendar.IsDayMarkedDelegate = value;
            }
        }

        /// <summary>
        /// Turn gray dates that fulfill the predicate
        /// </summary>
        public Func<DateTime, bool> IsDateAvailable
        {
            set
            {
                _fmCalendar.IsDateAvailable = value;
            }
        }

        public void CreateCalendar()
        {
            _fmCalendar = new FMCalendar();
            _fmCalendar.SelectionColor = MvxTouchColor.LightRed;
            _fmCalendar.TodayCircleColor = MvxTouchColor.Gray2;
            _fmCalendar.TodayMarkColor = MvxTouchColor.LightRed;
            _fmCalendar.SundayFirst = false;
            _fmCalendar.UserInteractionEnabled = true;
            _offsetView.AddSubview(_fmCalendar);
            _fmCalendar.BecomeFirstResponder();
        }
       
    }
}

