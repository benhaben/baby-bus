using System;
using UIKit;
using BabyBus.iOS;

namespace BabyBus.iOS
{
    public class ParentModalityButtonView : UIView
    {
        UILabel _label;

        public UILabel Label
        {
            get
            { 
                if (_label == null)
                {
                    _label = new UILabel();
                    _label.TextAlignment = UITextAlignment.Center;
                    _label.Font = EasyLayout.TitleFont;
                    _label.TextColor = MvxTouchColor.White;
                    _label.Layer.BackgroundColor = MvxTouchColor.Green1.CGColor;
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
                    _statusLabel.Font = EasyLayout.TitleFont;
                    _statusLabel.TextColor = MvxTouchColor.White;
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
                }
                return _button;
            }
            set
            { 
                _button = value;
            }
        }

        public void SetStatusLabel(int total, int completed)
        {
            StatusLabel.Text = string.Format("{0}/{1}", completed, total);
        }

        public EventHandler TouchUpInside;

        public ParentModalityButtonView()
            : base()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
        }

        public ParentModalityButtonView(UIImage image, string text)
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
            nfloat width = 300;
            nfloat height = 160;
            nfloat labelHeight = 30;
            nfloat statusLabelWidth = 50;

            this.ConstrainLayout(
                () =>
				_contentView.Frame.Top == Frame.Top
                && _contentView.Frame.Left == Frame.Left
                && _contentView.Frame.Right == Frame.Right
                && _contentView.Frame.Bottom == Frame.Bottom
			
                && Button.Frame.Top == _contentView.Frame.Top + EasyLayout.MarginNormal
                && Button.Frame.Left == _contentView.Frame.Left + EasyLayout.MarginNormal
                && Button.Frame.Right == _contentView.Frame.Right - EasyLayout.MarginNormal
                && Button.Frame.Width == width
                && Button.Frame.Height == height


                && Label.Frame.Width == width
                && Label.Frame.Height == labelHeight
                && Label.Frame.Left == _contentView.Frame.Left + EasyLayout.MarginNormal
                && Label.Frame.Right == _contentView.Frame.Right - EasyLayout.MarginNormal
                && Label.Frame.Bottom == Button.Frame.Bottom

                && StatusLabel.Frame.Width == statusLabelWidth
                && StatusLabel.Frame.Height == labelHeight
                && StatusLabel.Frame.Right == Label.Frame.Right - EasyLayout.MarginLarge
                && StatusLabel.Frame.Bottom == Button.Frame.Bottom

            );
        }
    }
}

