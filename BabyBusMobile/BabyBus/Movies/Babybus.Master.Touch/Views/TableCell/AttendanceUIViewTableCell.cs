using System;
using UIKit;
using Foundation;

namespace BabyBus.iOS
{
    public class AttendanceUIViewTableCell:UITableViewCell
    {
        public static readonly NSString Key = new NSString("AttendanceUIViewTableCell");
        private bool didSetupConstraints;

        AttendanceUIView _attendanceUIView = null;

        public AttendanceUIView AttendanceUIView
        {
            get
            { 
                if (_attendanceUIView == null)
                {
                    _attendanceUIView = new AttendanceUIView();
                    _attendanceUIView.Layer.BorderWidth = EasyLayout.BorderWidth;
                    _attendanceUIView.Layer.BorderColor = MvxTouchColor.Gray3.CGColor;
                    _attendanceUIView.Layer.CornerRadius = EasyLayout.CornerRadius;
                    _attendanceUIView.Layer.MasksToBounds = true;
                    _attendanceUIView.BackgroundColor = MvxTouchColor.Gray3;
                    _attendanceUIView.IsAttenceLabel.BackgroundColor = MvxTouchColor.Gray1;
//                    UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() => {
//                        _baseViewModel.ShowDetailCommand.Execute();
//                    });
//                    tapGesture.NumberOfTapsRequired = 1;  
//                    tapGesture.NumberOfTouchesRequired = 1; 
//                    _attendanceUIView.AddGestureRecognizer(tapGesture);
//                    _attendanceUIView.UserInteractionEnabled = true;
                    _attendanceUIView.UserInteractionEnabled = false;
                }
                return _attendanceUIView;
            }
        }

        public AttendanceUIViewTableCell()
        {
            UIView[] v =
                {
                AttendanceUIView,
            };
            _container.AddSubviews(v);
            ContentView.Add(_container);
        }

        UIView _container = new UIView();

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (this.didSetupConstraints)
            {
                return;
            }
            nfloat height = 60f;
            this.ContentView.ConstrainLayout(
                () =>
//                this.ContentView.Frame.Height == height

               _container.Frame.Top == ContentView.Frame.Top
                && _container.Frame.Left == ContentView.Frame.Left
                && _container.Frame.Right == ContentView.Frame.Right
                && _container.Frame.Bottom == ContentView.Frame.Bottom

                && this.AttendanceUIView.Frame.Top == this._container.Frame.Top + EasyLayout.MarginSmall
                && this.AttendanceUIView.Frame.Bottom == this._container.Frame.Bottom - EasyLayout.MarginSmall
                && this.AttendanceUIView.Frame.Left == this._container.Frame.Left + EasyLayout.MarginSmall
                && this.AttendanceUIView.Frame.Right == this._container.Frame.Right - EasyLayout.MarginSmall
            );
            this.didSetupConstraints = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
          
            this.ContentView.SetNeedsLayout();
            this.ContentView.LayoutIfNeeded();

            //Note: if you remove this, you will crash in iOS7
//            TranslatesAutoresizingMaskIntoConstraints = false;
            base.LayoutSubviews();
        }

    }
}

