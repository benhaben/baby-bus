//#define __DEBUDUI__ 
using System;
using UIKit;
using BabyBus.iOS;
using BabyBus.Logic.Shared;


namespace BabyBus.iOS
{
    public class AttendanceUIView : UIControl
    {

        UILabel _theClassName = null;

        public UILabel TheClassName
        {
            get
            { 
                if (_theClassName == null)
                {
                    _theClassName = new UILabel();
                    _theClassName.Text = "班级";
                    _theClassName.Font = EasyLayout.ContentFont;
                    _theClassName.TextColor = MvxTouchColor.Black1;
                }
                return _theClassName;
            }
            set{ _theClassName = value; }
        }

        UILabel _attenceLabel = null;

        public UILabel AttenceLabel
        {
            get
            { 
                if (_attenceLabel == null)
                {
                    _attenceLabel = new UILabel();
                    _attenceLabel.Text = "到校人数";
                    _attenceLabel.Font = EasyLayout.ContentFont;
                    _attenceLabel.TextColor = MvxTouchColor.Black1;
                }
                return _attenceLabel;
            }
            set{ _attenceLabel = value; }
        }

        UILabel _attenceNumLabel = null;

        public UILabel AttenceNumLabel
        {
            get
            { 
                if (_attenceNumLabel == null)
                {
                    _attenceNumLabel = new UILabel();
                    _attenceNumLabel.Text = "0";
                    _attenceNumLabel.Font = EasyLayout.ContentFont;
                    _attenceNumLabel.TextColor = MvxTouchColor.Black1;

                }
                return _attenceNumLabel;
            }
            set{ _attenceNumLabel = value; }
        }

        UILabel _unattenceLabel = null;

        public UILabel UnattenceLabel
        {
            get
            { 
                if (_unattenceLabel == null)
                {
                    _unattenceLabel = new UILabel();
                    _unattenceLabel.Text = "缺席人数";
                    _unattenceLabel.Font = EasyLayout.ContentFont;
                    _unattenceLabel.TextColor = MvxTouchColor.Black1;
                }
                return _unattenceLabel;
            }
            set{ _unattenceLabel = value; }
        }

        UILabel _unattenceNumLabel = null;

        public UILabel UnattenceNumLabel
        {
            get
            { 
                if (_unattenceNumLabel == null)
                {
                    _unattenceNumLabel = new UILabel();
                    _unattenceNumLabel.Text = "0";
                    _unattenceNumLabel.Font = EasyLayout.ContentFont;
                    _unattenceNumLabel.TextColor = MvxTouchColor.Black1;
                }
                return _unattenceNumLabel;
            }
            set{ _unattenceNumLabel = value; }
        }

        UILabel _isAttenceLabel = null;

        public UILabel IsAttenceLabel
        {
            get
            { 
                if (_isAttenceLabel == null)
                {
                    _isAttenceLabel = new UILabel();
                    _isAttenceLabel.Text = "是否点名了";
                    _isAttenceLabel.Font = EasyLayout.ContentFont;
                    _isAttenceLabel.TextColor = MvxTouchColor.Black1;
                }
                return _isAttenceLabel;
            }
            set{ _isAttenceLabel = value; }
        }

        UILabel _dateLabel = null;

        public UILabel DateLabel
        {
            get
            { 
                if (_dateLabel == null)
                {
                    _dateLabel = new UILabel();
                    _dateLabel.Text = LogicUtils.DateTimeString(DateTime.Now);
                    _dateLabel.Font = EasyLayout.ContentFont;
                    _dateLabel.TextColor = MvxTouchColor.Black1;
                }
                return _dateLabel;
            }
            set{ _dateLabel = value; }
        }

        public AttendanceUIView()
        {
            //            TranslatesAutoresizingMaskIntoConstraints = false;
        }

        UIView _container = new UIView();

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            UIView[] v =
                {
                    TheClassName,
                    AttenceLabel,
                    AttenceNumLabel,
                    UnattenceLabel,
                    UnattenceNumLabel,
                    IsAttenceLabel,
                    ////                DateLabel,
                };
            _container.AddSubviews(v);
            AddSubviews(_container);
            SetUpConstrainLayout();

            IsAttenceLabel.TextAlignment = UITextAlignment.Center;
            AttenceLabel.TextAlignment = UITextAlignment.Right;
            UnattenceLabel.TextAlignment = UITextAlignment.Right;

            #if DEBUG1
            TheClassName.BackgroundColor = UIColor.Brown;
            AttenceLabel.BackgroundColor = UIColor.Red;
            AttenceNumLabel.BackgroundColor = UIColor.Blue;

            UnattenceLabel.BackgroundColor = UIColor.Brown;
            UnattenceNumLabel.BackgroundColor = UIColor.Red;
            IsAttenceLabel.BackgroundColor = UIColor.Blue;
            #endif
            base.LayoutSubviews();
        }

        void SetUpConstrainLayout()
        {
            nfloat IsAttenceLabelWidth = (nfloat)(EasyLayout.MaxLabelWidthInCell * 0.618);
            this.ConstrainLayout(
                () => 
                _container.Frame.Top == Frame.Top
                && _container.Frame.Left == Frame.Left
                && _container.Frame.Right == Frame.Right
                && _container.Frame.Bottom == Frame.Bottom

                && TheClassName.Frame.GetCenterY() == _container.Frame.GetCenterY()
                && TheClassName.Frame.Left == _container.Frame.Left + EasyLayout.MarginMedium
                && TheClassName.Frame.Width == EasyLayout.MaxLabelWidthInCell
                && TheClassName.Frame.Height == EasyLayout.NormalTextFieldHeight

                && AttenceLabel.Frame.Top == _container.Frame.GetCenterY() - EasyLayout.NormalTextFieldHeight
                //                && AttenceLabel.Frame.Top == _container.Frame.Top + EasyLayout.MarginMedium
                && AttenceLabel.Frame.GetCenterX() == _container.Frame.GetCenterX() - EasyLayout.MaxNumberWidthInCell
                && AttenceLabel.Frame.Width == EasyLayout.MaxLabelWidthInCell
                && AttenceLabel.Frame.Height == EasyLayout.NormalTextFieldHeight

                && AttenceNumLabel.Frame.Top == AttenceLabel.Frame.Top
                && AttenceNumLabel.Frame.Left == AttenceLabel.Frame.Right
                && AttenceNumLabel.Frame.Width == EasyLayout.MaxNumberWidthInCell
                && AttenceNumLabel.Frame.Height == EasyLayout.NormalTextFieldHeight

                && UnattenceLabel.Frame.Top == AttenceLabel.Frame.Bottom
                && UnattenceLabel.Frame.GetCenterX() == _container.Frame.GetCenterX() - EasyLayout.MaxNumberWidthInCell
                && UnattenceLabel.Frame.Width == EasyLayout.MaxLabelWidthInCell
                && UnattenceLabel.Frame.Height == EasyLayout.NormalTextFieldHeight

                && UnattenceNumLabel.Frame.Top == UnattenceLabel.Frame.Top
                && UnattenceNumLabel.Frame.Left == UnattenceLabel.Frame.Right
                && UnattenceNumLabel.Frame.Width == EasyLayout.MaxNumberWidthInCell
                && UnattenceNumLabel.Frame.Height == EasyLayout.NormalTextFieldHeight


                && IsAttenceLabel.Frame.Top == _container.Frame.Top
                && IsAttenceLabel.Frame.Bottom == _container.Frame.Bottom
                && IsAttenceLabel.Frame.Right == Frame.Right
                && IsAttenceLabel.Frame.Width == IsAttenceLabelWidth
            );
        }
    }
}

