using System;
using UIKit;

//#define DEBUGUI
namespace BabyBus.iOS
{
	public class ButtonWithLabel : UIView
	{
		UILabel _label;

		public UILabel Label {
			get {
				if (_label == null) {
					_label = new UILabel();
					_label.TextAlignment = UITextAlignment.Center;
					_label.Font = EasyLayout.SmallFont;
					_label.TextColor = MvxTouchColor.Gray1;
				}
				return _label;
			}
			set {
				_label = value;
			}
		}

		UIButton _button;

		public UIButton Button {
			get {
				if (_button == null) {
					_button = new UIButton();
					_button.SetImage(
						UIImage.FromBundle("physicalHealth.png")
                        , UIControlState.Normal);
					_button.SetImage(
						UIImage.FromBundle("physicalHealthSelected.png")
                        , UIControlState.Selected);
				}
				return _button;
			}
			set {
				_button = value;
			}
		}

		public EventHandler TouchUpInside;

		public ButtonWithLabel()
			: base()
		{
			TranslatesAutoresizingMaskIntoConstraints = false;
			this.Button.TouchUpInside += (object o, EventArgs e) => {
				if (TouchUpInside != null)
					TouchUpInside(o, e);
			};
		}

		UIView _contentView = new UIView();

		public override void LayoutSubviews() {

			UIView[] v = {
                    Label,
                    Button
                };
            _contentView.AddSubviews(v);
            AddSubviews(_contentView);
            SetUpConstrainLayout();
            base.LayoutSubviews();
        }

        void SetUpConstrainLayout()
        {
            nfloat offset = 10;
            this.ConstrainLayout(
                () => 
                _contentView.Frame.Top == Frame.Top
                && _contentView.Frame.Left == Frame.Left
                && _contentView.Frame.Right == Frame.Right
                && _contentView.Frame.Bottom == Frame.Bottom

                && Button.Frame.GetCenterY() == _contentView.Frame.GetCenterY() - offset
                && Button.Frame.GetCenterX() == _contentView.Frame.GetCenterX()
                && Button.Frame.Width == EasyLayout.HomePageButtonImageHeight
                && Button.Frame.Height == EasyLayout.HomePageButtonImageHeight

                && Label.Frame.Width == EasyLayout.HomePageButtonImageWidth
                && Label.Frame.Top == Button.Frame.Bottom + EasyLayout.MarginSmall
                && Label.Frame.GetCenterX() == _contentView.Frame.GetCenterX()
                && Label.Frame.Height == EasyLayout.NormalTextFieldHeight
            );
            #if DEBUGUI
            this.BackgroundColor = UIColor.Brown;
            #endif
        }
    }

}

