using System;
using UIKit;
using BabyBus.iOS;

namespace BabyBus.iOS
{
    public class TeacherModalityButtonView : UIView
    {
        public TeacherModalityButtonView()
            : base()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
        }

        public TeacherModalityButtonView(UIImage image, string text)
            : base()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            Button.SetBackgroundImage(image, UIControlState.Normal);
            Label.Text = text;

            Button.TouchUpInside += (sender, e) =>
            {
                if (TouchUpInside != null)
                {
                    TouchUpInside(this, e);
                }
            };
        }

        UILabel _label;

        public UILabel Label
        {
            get
            { 
                if (_label == null)
                {
                    _label = new UILabel();
                    _label.TextAlignment = UITextAlignment.Center;
                    _label.Font = EasyLayout.ContentFont;
                    _label.TextColor = MvxTouchColor.Black1;
                    _label.Text = "语言言语智力";
                }
                return _label;
            }
        }

        UILabel _statusLabel;

        public UILabel StatusLabel
        {
            get
            { 
                if (_statusLabel == null)
                {
                    _statusLabel = new UILabel();
                    _statusLabel.TextAlignment = UITextAlignment.Center;
                    _statusLabel.Font = EasyLayout.TinyFont;
                    _statusLabel.TextColor = MvxTouchColor.Black1;
                    _statusLabel.Text = "-/-";
                }
                return _statusLabel;
            }
        }

        UIButton _button;

        public UIButton Button
        {
            get
            { 
                if (_button == null)
                {
                    _button = new UIButton();
                    _button.SetBackgroundImage(UIImage.FromBundle("modality_1.png"), UIControlState.Normal);
                    _button.ContentEdgeInsets = new UIEdgeInsets(10, 10, 10, 10);
                }
                return _button;
            }
            set
            { 
                _button = value;
            }
        }

        public EventHandler TouchUpInside;

        UIView _contentView = new UIView();

        public override void LayoutSubviews()
        {
            UIView[] v =
            {
                Button,
                Label,
                StatusLabel,
            };

            _contentView.AddSubviews(v);
            AddSubviews(_contentView);
            SetUpConstrainLayout();

            base.LayoutSubviews();
        }

        void SetUpConstrainLayout()
        {
            nfloat width = 90;
            nfloat height = 90;
            nfloat labelHeight = 30;
            nfloat statusLabelWidth = 30;

            this.ConstrainLayout(
                () => 
					_contentView.Frame.Top == Frame.Top
                && _contentView.Frame.Left == Frame.Left
                && _contentView.Frame.Right == Frame.Right
                && _contentView.Frame.Bottom == Frame.Bottom

                && Button.Frame.Top == _contentView.Frame.Top
                && Button.Frame.Left == _contentView.Frame.Left
//				&& Button.Frame.Right == _contentView.Frame.Right
                && Button.Frame.Width == width
                && Button.Frame.Height == height

                && StatusLabel.Frame.Left == Button.Frame.Right
                && StatusLabel.Frame.Bottom == Button.Frame.Bottom
                && StatusLabel.Frame.Width == statusLabelWidth

                && Label.Frame.Width == width
                && Label.Frame.Height == labelHeight
                && Label.Frame.Left == _contentView.Frame.Left
                && Label.Frame.Right == _contentView.Frame.Right
                && Label.Frame.Top == Button.Frame.Bottom
                && Label.Frame.Bottom == _contentView.Frame.Bottom
            );
        }

        public void SetStatusLabel(int total, int completed)
        {
            StatusLabel.Text = string.Format("{0}/{1}", completed, total);
        }
    }
}

