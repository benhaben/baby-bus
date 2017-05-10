using System;
using UIKit;
using BabyBus.iOS;
using CoreGraphics;

namespace BabyBus.iOS
{
	public class ImageLabel
		:UIView
	{
		bool _leftIcon = true;
		UIFont _font = EasyLayout.TinyFont;
		nfloat _imageSize = 10f;
		int _lines = 1;


		public ImageLabel(bool leftIcon = true)
		{
			_leftIcon = leftIcon;
		}

		public ImageLabel(bool leftIcon, UIFont font, nfloat imageSize)
		{
			_leftIcon = leftIcon;
			_font = font;
			_imageSize = imageSize;
			_lines = 0;
		}

		UIImageView _imageView;

		public UIImageView ImageView {
			get { 
				if (_imageView == null) {
					_imageView = new UIImageView();
					_imageView.Image = UIImage.FromBundle("placeholder.png");
				}
				return _imageView;
			}
		}

		UILabel _label;

		public UILabel Label {
			get { 
				if (_label == null) {
					_label = new UILabel();
					_label.Font = _font;
					_label.Text = "Label";
					_label.TextColor = MvxTouchColor.Black1;
					_label.TextAlignment = UITextAlignment.Left;
					_label.Lines = _lines;
				}
				return _label;
			}
		}

		public string Text {
			get{ return Label.Text; }
			set{ Label.Text = value; }
		}

		public UIImage Image {
			set { 
				ImageView.Image = value;
			}
		}

		public ImageLabel()
		{
		}

		UIView _container = new UIView();

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			UIView[] v = {
				ImageView,
				Label
			};
			_container.AddSubviews(v);
			AddSubviews(_container);
			SetUpConstrainLayout();

			base.LayoutSubviews();
		}

		bool didSetupConstrain = false;

		void SetUpConstrainLayout()
		{
			if (didSetupConstrain) {
				return;
			}

			if (_leftIcon) {
				this.ConstrainLayout(
					() =>
					_container.Frame.Top == Frame.Top
					&& _container.Frame.Left == Frame.Left
					&& _container.Frame.Right == Frame.Right
					&& _container.Frame.Bottom == Frame.Bottom

					&& ImageView.Frame.Top == _container.Frame.Top
					&& ImageView.Frame.Left == _container.Frame.Left
					&& ImageView.Frame.Bottom == _container.Frame.Bottom
					&& ImageView.Frame.Width == _imageSize
					&& ImageView.Frame.Height == _imageSize

					&& Label.Frame.Top == _container.Frame.Top
					&& Label.Frame.Left == ImageView.Frame.Right + EasyLayout.MarginSmall
					&& Label.Frame.Bottom == _container.Frame.Bottom
					&& Label.Frame.Right == _container.Frame.Right
				);
			} else {
				this.ConstrainLayout(
					() =>
					_container.Frame.Top == Frame.Top
					&& _container.Frame.Left == Frame.Left
					&& _container.Frame.Right == Frame.Right
					&& _container.Frame.Bottom == Frame.Bottom

					&& Label.Frame.Top == _container.Frame.Top
					&& Label.Frame.Left == _container.Frame.Left
					&& Label.Frame.Bottom == _container.Frame.Bottom

					&& ImageView.Frame.Top == _container.Frame.Top
					&& ImageView.Frame.Left == Label.Frame.Right + EasyLayout.MarginSmall
					&& ImageView.Frame.Bottom == _container.Frame.Bottom
					&& ImageView.Frame.Width == _imageSize
					&& ImageView.Frame.Height == _imageSize
				);
			}

			var constrains = this.ConstrainLayout(
				                 () => Label.Frame.Height == _imageSize
				                 && ImageView.Frame.Height == _imageSize
			                 );

			labelHeightConstraint = constrains[0];
			imageHeightConstraint = constrains[1];

			didSetupConstrain = true;
		}

		NSLayoutConstraint labelHeightConstraint;
		NSLayoutConstraint imageHeightConstraint;

		public override void SizeToFit()
		{
			var size = Label.SizeThatFits(new CGSize(300, 100));
			Frame = new CGRect(0, 0, size.Width + _imageSize + EasyLayout.MarginSmall, size.Height);
		}

		public override CGSize SizeThatFits(CGSize size)
		{
			if (string.IsNullOrEmpty(Text)) {
				Hidden = true;
				return new CGSize(0, 0);
				labelHeightConstraint.Constant = 0f;
				imageHeightConstraint.Constant = 0f;
			} else {
				size = Label.SizeThatFits(new CGSize(300, 100));
				labelHeightConstraint.Constant = size.Height;
				return new CGSize(size.Width + _imageSize + EasyLayout.MarginSmall, size.Height);
			}
		}
	}
}

