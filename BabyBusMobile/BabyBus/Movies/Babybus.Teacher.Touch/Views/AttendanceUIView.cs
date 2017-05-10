//#define __DEBUDUI__ 
using System;
using UIKit;
using BabyBus.iOS;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class AttendanceUIView 
        : UIControl
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
                    _theClassName.TextColor = MvxTouchColor.White;
                    _theClassName.TextAlignment = UITextAlignment.Center;
                }
                return _theClassName;
            }
            set{ _theClassName = value; }
        }

        UIView _separator = null;

        public UIView SeparatorView
        {
            get
            { 
                if (_separator == null)
                {
                    _separator = new UIView();
                    _separator.BackgroundColor = MvxTouchColor.White2;
                }
                return _separator;
            }
        }

        UILabel _attenceLabel = null;

        public UILabel AttenceLabel
        {
            get
            { 
                if (_attenceLabel == null)
                {
                    _attenceLabel = new UILabel();
                    _attenceLabel.Text = "到勤";
                    _attenceLabel.TextColor = MvxTouchColor.Gray1;
                    _attenceLabel.TextAlignment = UITextAlignment.Center;
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
                    _attenceNumLabel.Font = EasyLayout.BigFontBold;
                    _attenceNumLabel.TextColor = MvxTouchColor.Green;
                    _attenceNumLabel.TextAlignment = UITextAlignment.Center;
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
                    _unattenceLabel.Text = "缺勤";
                    _unattenceLabel.TextColor = MvxTouchColor.Gray1;
                    _unattenceLabel.TextAlignment = UITextAlignment.Center;
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
                    _unattenceNumLabel.Font = EasyLayout.BigFontBold;
                    _unattenceNumLabel.TextColor = MvxTouchColor.LightRed;
                    _unattenceNumLabel.TextAlignment = UITextAlignment.Center;
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
                    _isAttenceLabel.Text = "本日还未进行考勤，请点击上方的按钮开始考勤";
                    _isAttenceLabel.TextAlignment = UITextAlignment.Center;
                    _isAttenceLabel.Font = EasyLayout.SmallFont;
                    _isAttenceLabel.TextColor = MvxTouchColor.Black1;
                }
                return _isAttenceLabel;
            }
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
                    _dateLabel.TextColor = EasyLayout.TextColor;
                }
                return _dateLabel;
            }
            set{ _dateLabel = value; }
        }

        UIView _backgroundLabel = null;

        public UIView BackgroundLabel
        {
            get
            { 
                if (_backgroundLabel == null)
                {
                    _backgroundLabel = new UIView();
                    _backgroundLabel.BackgroundColor = MvxTouchColor.Blue;
                }
                return _backgroundLabel;
            }
        }

        UIView _circleBackgroundView = null;

        public UIView CircleBackgroundView
        {
            get
            { 
                if (_circleBackgroundView == null)
                {
                    _circleBackgroundView = new UIView();
//                    _circleBackgroundView.Layer.BorderWidth = borderSize * 4;
//                    _circleBackgroundView.Layer.BorderColor = MvxTouchColor.White.CGColor;
                    _circleBackgroundView.Layer.CornerRadius = EasyLayout.CircleButtonHeight / 2 + borderSize / 2;
                    _circleBackgroundView.Layer.MasksToBounds = true;
                    _circleBackgroundView.BackgroundColor = MvxTouchColor.White;
                }
                return _circleBackgroundView;
            }
        }

        float borderSize = 5f;
        CircleButton _checkButton = null;

        public CircleButton CheckButton
        {
            get
            {
                if (_checkButton == null)
                {   
                    _checkButton = new CircleButton("");
                    _checkButton.TitleLabel.Font = UIFont.SystemFontOfSize(22);
                    _checkButton.BackgroundColor = MvxTouchColor.White;
                    _checkButton.BorderColor = MvxTouchColor.Blue;
                    _checkButton.BorderSize = borderSize;
                    _checkButton.SetBackgroundImage(UIImage.FromBundle("dianming.png"), UIControlState.Normal);
                }
                return _checkButton;
            }
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
                    BackgroundLabel,
                    CircleBackgroundView,
                    CheckButton,
                    TheClassName,
                    AttenceLabel,
                    AttenceNumLabel,
                    UnattenceLabel,
                    UnattenceNumLabel,
                    IsAttenceLabel,
                    SeparatorView
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
            nfloat witdhCircleBackgroundView = EasyLayout.CircleButtonHeight + borderSize * 2;
            nfloat leftCenterX = 320 / 5;
            nfloat IsAttenceLabelMargin = 30f;
            this.ConstrainLayout(
                () => 
                _container.Frame.Top == Frame.Top
                && _container.Frame.Left == Frame.Left
                && _container.Frame.Right == Frame.Right
                && _container.Frame.Bottom == Frame.Bottom

                && TheClassName.Frame.Top == _container.Frame.Top + EasyLayout.MarginMedium
                && TheClassName.Frame.GetCenterX() == _container.Frame.GetCenterX()
                && TheClassName.Frame.Width == EasyLayout.MaxLabelWidthInCell
                && TheClassName.Frame.Height == EasyLayout.NormalTextFieldHeight

               
                && BackgroundLabel.Frame.Top == Frame.Top
                && BackgroundLabel.Frame.Left == Frame.Left
                && BackgroundLabel.Frame.Right == Frame.Right
//                && BackgroundLabel.Frame.Bottom == Frame.Bottom * factor

                && CheckButton.Frame.GetCenterY() == BackgroundLabel.Frame.Bottom
                && CheckButton.Frame.GetCenterX() == _container.Frame.GetCenterX()
                && CheckButton.Frame.Height == EasyLayout.CircleButtonHeight
                && CheckButton.Frame.Width == EasyLayout.CircleButtonHeight

                && CircleBackgroundView.Frame.GetCenterY() == BackgroundLabel.Frame.Bottom
                && CircleBackgroundView.Frame.GetCenterX() == _container.Frame.GetCenterX()
                && CircleBackgroundView.Frame.Height == witdhCircleBackgroundView
                && CircleBackgroundView.Frame.Width == witdhCircleBackgroundView

                && AttenceNumLabel.Frame.Top == CircleBackgroundView.Frame.GetCenterY() + EasyLayout.MarginMedium
                && AttenceNumLabel.Frame.GetCenterX() == _container.Frame.Left + leftCenterX
//                && AttenceNumLabel.Frame.Width == EasyLayout.MaxNumberWidthInCell
//                && AttenceNumLabel.Frame.Height == EasyLayout.NormalTextFieldHeight

                && AttenceLabel.Frame.Top == AttenceNumLabel.Frame.Bottom + EasyLayout.MarginMedium
                && AttenceLabel.Frame.GetCenterX() == _container.Frame.Left + leftCenterX
//                && AttenceLabel.Frame.Width == EasyLayout.MaxLabelWidthInCell
//                && AttenceLabel.Frame.Height == EasyLayout.NormalTextFieldHeight
               
                && UnattenceNumLabel.Frame.Top == CircleBackgroundView.Frame.GetCenterY() + EasyLayout.MarginMedium
                && UnattenceNumLabel.Frame.GetCenterX() == _container.Frame.Right - leftCenterX
//                && UnattenceNumLabel.Frame.Width == EasyLayout.MaxNumberWidthInCell
//                && UnattenceNumLabel.Frame.Height == EasyLayout.NormalTextFieldHeight

                && UnattenceLabel.Frame.Top == AttenceLabel.Frame.Top
                && UnattenceLabel.Frame.GetCenterX() == _container.Frame.Right - leftCenterX
//                && UnattenceLabel.Frame.Width == EasyLayout.MaxLabelWidthInCell
//                && UnattenceLabel.Frame.Height == EasyLayout.NormalTextFieldHeight

                && IsAttenceLabel.Frame.GetCenterX() == _container.Frame.GetCenterX()
                && IsAttenceLabel.Frame.Top == CircleBackgroundView.Frame.Bottom + IsAttenceLabelMargin
                && IsAttenceLabel.Frame.Height == EasyLayout.NormalTextFieldHeight

                && SeparatorView.Frame.Top == IsAttenceLabel.Frame.Bottom + EasyLayout.MarginNormal
                && SeparatorView.Frame.Bottom == _container.Frame.Bottom
                && SeparatorView.Frame.Right == _container.Frame.Right
                && SeparatorView.Frame.Left == _container.Frame.Left
                && SeparatorView.Frame.Height == EasyLayout.LineHeight
            );
        }
    }
}

