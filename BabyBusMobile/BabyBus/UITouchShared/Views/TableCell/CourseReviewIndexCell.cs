using System;
using UIKit;
using Foundation;
using BabyBus.iOS;
using CoreGraphics;
using PatridgeDev;

namespace BabyBus.iOS
{
	public class CourseReviewIndexCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("CourseReviewIndexCell");
		bool didSetupConstraints;

		public CourseReviewIndexCell()
		{
			ContentView.Add(ImageView);
			ContentView.Add(UserName);
			ContentView.Add(CreateDate);
			ContentView.Add(RatingView);
			ContentView.Add(Content);
			ContentView.Add(SeparatorView);
			SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		UIImageView _image = null;

		public virtual UIImageView ImageView {
			get {
				if (_image == null) {
					_image = new UIImageView();
					_image.Layer.CornerRadius = 20;
					_image.Layer.MasksToBounds = false;
					_image.ContentMode = UIViewContentMode.ScaleAspectFill;
					_image.ClipsToBounds = true;
				}
				return _image;
			}
		}

		PDRatingView _ratingView;

		public PDRatingView RatingView {
			get { 
				if (_ratingView == null) {
					var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("rating_empty.png"),
						                   filledImage: UIImage.FromBundle("rating_chosen.png"),
						                   chosenImage: UIImage.FromBundle("rating_chosen.png"));

					ratingConfig.ItemPadding = 2f;

					var ratingFrame = new CGRect(CGPoint.Empty, new CGSize(100f, 30f));

					_ratingView = new PDRatingView(ratingFrame, ratingConfig);
					_ratingView.AverageRating = 4;
				}
				return _ratingView;
			}
		}

		UILabel _userName = null;

		public UILabel UserName {
			get {
				if (_userName == null) {   
					_userName = new UILabel();
					_userName.Text = "UserName";
					_userName.Font = EasyLayout.TitleFont;
					_userName.TextColor = MvxTouchColor.Black1;
				}
				return _userName;
			}
		}

		UILabel _createDate = null;

		public UILabel CreateDate {
			get {
				if (_createDate == null) {   
					_createDate = new UILabel();
					_createDate.Text = "Create";
					_createDate.Font = EasyLayout.SmallFont;
					_createDate.TextColor = MvxTouchColor.Gray1;
				}
				return _createDate;
			}
		}

		UILabel _content = null;

		public UILabel Content {
			get {
				if (_content == null) {   
					_content = new UILabel();
					_content.Text = "Content";
					_content.Font = EasyLayout.SmallFont;
					_content.TextColor = MvxTouchColor.Gray1;
					_content.Lines = 0;
				}
				return _content;
			}
		}

		UIView _separator = null;

		public UIView SeparatorView {
			get { 
				if (_separator == null) {
					_separator = new UIView();
					_separator.BackgroundColor = MvxTouchColor.White2;
				}
				return _separator;
			}
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();
			if (this.didSetupConstraints) {
				return;
			}
			nfloat PhoneImageWidth = 244;
			nfloat ratingWidth = 100;
			nfloat ratingHeight = 15;

			this.ContentView.ConstrainLayout(
				() =>
				this.ImageView.Frame.Top == this.ContentView.Frame.Top + EasyLayout.MarginMedium
				&& this.ImageView.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
				&& this.ImageView.Frame.Height == EasyLayout.HeadPortraitImageHeight
				&& this.ImageView.Frame.Width == EasyLayout.HeadPortraitImageHeight

				&& this.UserName.Frame.Top == this.ContentView.Frame.Top + EasyLayout.MarginSmall
				&& this.UserName.Frame.Left == this.ImageView.Frame.Right + EasyLayout.MarginMedium
				&& this.UserName.Frame.Right == this.ContentView.Frame.Right - EasyLayout.MarginSmall
				&& this.UserName.Frame.Height == EasyLayout.NormalTextFieldHeight

				&& this.RatingView.Frame.Top == this.UserName.Frame.Bottom
				&& this.RatingView.Frame.Left == this.UserName.Frame.Left
				&& RatingView.Frame.Width == ratingWidth
				&& RatingView.Frame.Height == ratingHeight

				&& this.CreateDate.Frame.Top == this.UserName.Frame.Bottom
				&& this.CreateDate.Frame.Left == this.RatingView.Frame.Right

				&& Content.Frame.Top == ImageView.Frame.Bottom
				&& Content.Frame.Left == UserName.Frame.Left
				&& Content.Frame.Right == ContentView.Frame.Right - EasyLayout.MarginNormal
//				&& Content.Frame.Height == EasyLayout.SmallTextFieldHeight

				&& this.SeparatorView.Frame.Top == Content.Frame.Bottom + EasyLayout.MarginMedium
				&& this.SeparatorView.Frame.Left == this.ContentView.Frame.Left
				&& this.SeparatorView.Frame.Right == this.ContentView.Frame.Right
				&& this.SeparatorView.Frame.Height == EasyLayout.SeparatorHeight

				&& this.SeparatorView.Frame.Bottom == this.ContentView.Frame.Bottom
			);
			//Note: do not change frame, change Constrain to resize control

			this.didSetupConstraints = true;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			this.ContentView.SetNeedsLayout();
			this.ContentView.LayoutIfNeeded();
			this.ContentView.UpdateConstraintsIfNeeded();

		}
	}
}

