using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace BabyBus.iOS
{


	public class ButtonsView : UIView
	{

		ButtonWithLabel _firstButton = null;

		public ButtonWithLabel FirstButton {
			get { 
				if (_firstButton == null) {
					_firstButton = new ButtonWithLabel();
				}
				return _firstButton;
			}
			set{ _firstButton = value; }
		}

		ButtonWithLabel _secondButton = null;

		public ButtonWithLabel SecondButton {
			get { 
				if (_secondButton == null) {
					_secondButton = new ButtonWithLabel();
				}
				return _secondButton;
			}
			set{ _secondButton = value; }
		}

		ButtonWithLabel _thirdButton = null;

		public ButtonWithLabel ThirdButton {
			get { 
				if (_thirdButton == null) {
					_thirdButton = new ButtonWithLabel();
				}
				return _thirdButton;
			}
			set{ _thirdButton = value; }
		}

		public ButtonsView(IList<UIImage> images, IList<string> texts)
			: base()
		{

			Frame = new CGRect(0, 0, 320, 100);

			if (texts.Count >= 1) {
				FirstButton.Button.SetBackgroundImage(images[0], UIControlState.Normal);
				FirstButton.Button.SetBackgroundImage(images[1], UIControlState.Selected);
				FirstButton.Label.Text = texts[0];
			} 
			if (texts.Count >= 2) {
				SecondButton.Button.SetBackgroundImage(images[2], UIControlState.Normal);
				SecondButton.Button.SetBackgroundImage(images[3], UIControlState.Selected);
				SecondButton.Label.Text = texts[1];
			}
			if (texts.Count >= 3) {
				ThirdButton.Button.SetBackgroundImage(images[4], UIControlState.Normal);
				ThirdButton.Button.SetBackgroundImage(images[5], UIControlState.Selected);
				ThirdButton.Label.Text = texts[2];
			}
		}

		UIView _contentView = new UIView();

		public override void LayoutSubviews() {

			UIView[] v = {
                FirstButton,
                SecondButton,
                ThirdButton
            };
            _contentView.AddSubviews(v);
            AddSubviews(_contentView);
            SetUpConstrainLayout();

            //Note: if you remove this, you will crash in iOS7
            TranslatesAutoresizingMaskIntoConstraints = false;
            base.LayoutSubviews();

            #if DEBUG1
            FirstButton.BackgroundColor = UIColor.Red;
            SecondButton.BackgroundColor = UIColor.Green;
            ThirdButton.BackgroundColor = UIColor.Blue;

            #endif
        }

        void SetUpConstrainLayout()
        {
            nfloat avgWidth = 320 / 3;
            nfloat halfAvgWidth = avgWidth / 2;
            nfloat height = EasyLayout.HomePageNoticeBarHeight;

            this.ConstrainLayout(
                () => 
                _contentView.Frame.Top == Frame.Top
                && _contentView.Frame.Left == Frame.Left
                && _contentView.Frame.Right == Frame.Right
                && _contentView.Frame.Bottom == Frame.Bottom

                && FirstButton.Frame.Top == _contentView.Frame.Top
                && FirstButton.Frame.GetCenterX() == _contentView.Frame.Left + halfAvgWidth
                && FirstButton.Frame.Width == avgWidth
                && FirstButton.Frame.Height == height

                && SecondButton.Frame.Top == _contentView.Frame.Top
                && SecondButton.Frame.GetCenterX() == _contentView.Frame.GetCenterX()
                && SecondButton.Frame.Width == avgWidth
                && SecondButton.Frame.Height == height

                && ThirdButton.Frame.Top == _contentView.Frame.Top
                && ThirdButton.Frame.GetCenterX() == _contentView.Frame.Right - halfAvgWidth
                && ThirdButton.Frame.Width == avgWidth
                && ThirdButton.Frame.Height == height
            );
        }
    }
}

