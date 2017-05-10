using System;
using UIKit;
using Foundation;
using BabyBus.iOS;
using CoreGraphics;
using PatridgeDev;

namespace BabyBus.iOS
{
	public class CourseIndexCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("CourseIndexCell");

		UIImageView _courseImageView = null;

		public UIImageView CourseImageView {
			get {
				if (_courseImageView == null) {
					_courseImageView = new UIImageView();
					_courseImageView.Layer.CornerRadius = 35;
					_courseImageView.Layer.MasksToBounds = true;
					_courseImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
					_courseImageView.ClipsToBounds = true;
					_courseImageView.Image = UIImage.FromBundle("ad-2.png");
				}
				return _courseImageView;
			}
		}

		UILabel _courseTitle = null;


		public UILabel CourseTitle {
			get {
				if (_courseTitle == null) {
					_courseTitle = new UILabel();
					_courseTitle.Text = "Title";
					_courseTitle.TextColor = MvxTouchColor.Black1;
					_courseTitle.Font = UIFont.SystemFontOfSize(14);
					_courseTitle.TextAlignment = UITextAlignment.Center;
				}
				return _courseTitle;
			}
		}

		ImageLabel _address = null;

		public ImageLabel Address {
			get {
				if (_address == null) {
					_address = new ImageLabel();
					_address.Image = UIImage.FromBundle("icon_address.png");

					_address.Text = "西安体育学院综合楼一楼";
					_address.SizeToFit();
				}
				return _address;
			}
		}

		ImageLabel _enroll = null;

		public ImageLabel Enroll {
			get {
				if (_enroll == null) {
					_enroll = new ImageLabel();
					_enroll.Image = UIImage.FromBundle("icon_people.png");
					_enroll.Text = "已报0/10";
					_enroll.SizeToFit();
				}
				return _enroll;
			}
		}

		ImageLabel _lessonPeroid = null;

		public ImageLabel LessonPeroid {
			get {
				if (_lessonPeroid == null) {
					_lessonPeroid = new ImageLabel();
					_lessonPeroid.Image = UIImage.FromBundle("icon_period.png");
					_lessonPeroid.Text = "每周六早9:00-11:00";
					_lessonPeroid.SizeToFit();
				}
				return _lessonPeroid;
			}
		}

		ImageLabel _coursePrice = null;

		public ImageLabel CoursePrice {
			get {
				if (_coursePrice == null) {
					_coursePrice = new ImageLabel();
					_coursePrice.Image = UIImage.FromBundle("icon_price.png");
					_coursePrice.Text = "1900元";
					_coursePrice.SizeToFit();
				}
				return _coursePrice;
			}
		}

		ImageLabel _detail = null;

		public ImageLabel Detail {
			get {
				if (_detail == null) {
					_detail = new ImageLabel(false);
					_detail.Image = UIImage.FromBundle("icon_detail.png");
					_detail.Text = "查看详情";
					_detail.SizeToFit();
				}
				return _detail;
			}
		}

		PDRatingView _ratingView;

		public PDRatingView RatingView {
			get { 
				if (_ratingView == null) {
					var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("rating_empty.png"),
						                   filledImage: UIImage.FromBundle("rating_chosen.png"),
						                   chosenImage: UIImage.FromBundle("rating_chosen.png"));

					ratingConfig.ItemPadding = 1f;

					var ratingFrame = new CGRect(CGPoint.Empty, new CGSize(60f, 20f));

					_ratingView = new PDRatingView(ratingFrame, ratingConfig);
					_ratingView.AverageRating = 4;
				}
				return _ratingView;
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

		InsetsLabel _paidStatus = null;

		public InsetsLabel PaidStatus {
			get { 
				if (_paidStatus == null) {
					_paidStatus = new InsetsLabel();
					_paidStatus.Font = EasyLayout.SmallFont;
					_paidStatus.BackgroundColor = MvxTouchColor.Blue;
					_paidStatus.TextColor = MvxTouchColor.White;
					_paidStatus.Text = "类型  ";
					_paidStatus.Hidden = true;
				}
				return _paidStatus;
			}
		}

		InsetsLabel _reviewStatus = null;

		public InsetsLabel ReviewStatus {
			get { 
				if (_reviewStatus == null) {
					_reviewStatus = new InsetsLabel();
					_reviewStatus.Font = EasyLayout.SmallFont;
					_reviewStatus.BackgroundColor = MvxTouchColor.Blue;
					_reviewStatus.TextColor = MvxTouchColor.White;
					_reviewStatus.Text = "类型  ";
					_reviewStatus.Hidden = true;
				}
				return _reviewStatus;
			}
		}



		public CourseIndexCell()
			: base(UITableViewCellStyle.Default, Key)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			ContentView.Add(CourseImageView);
			ContentView.Add(CourseTitle);
			ContentView.Add(Address);
			ContentView.Add(Enroll);
			ContentView.Add(LessonPeroid);
			ContentView.Add(CoursePrice);
			ContentView.Add(RatingView);
			ContentView.Add(SeparatorView);
			ContentView.Add(PaidStatus);
			ContentView.Add(ReviewStatus);
			ContentView.Add(Detail);
		}

		bool _didSetupConstraints = false;

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();

			if (_didSetupConstraints) {
				return;
			}

			nfloat imageDiameter = 70;
			nfloat ratingHeight = 20;
			nfloat detailWidth = 60;
			nfloat ratingWidth = imageDiameter;

			ContentView.ConstrainLayout(
				() =>
				CourseImageView.Frame.Top == ContentView.Frame.Top + EasyLayout.MarginMedium
				&& CourseImageView.Frame.Left == ContentView.Frame.Left + EasyLayout.MarginNormal
				&& CourseImageView.Frame.Width == imageDiameter
				&& CourseImageView.Frame.Height == imageDiameter

				&& CourseTitle.Frame.Top == ContentView.Frame.Top + EasyLayout.MarginMedium
				&& CourseTitle.Frame.Left == CourseImageView.Frame.Right + EasyLayout.MarginNormal
				&& CourseTitle.Frame.Right <= ContentView.Frame.Right - EasyLayout.MarginNormal

				&& Address.Frame.Top == CourseTitle.Frame.Bottom + EasyLayout.MarginMedium
				&& Address.Frame.Left == CourseImageView.Frame.Right + EasyLayout.MarginNormal
				&& Address.Frame.Right == ContentView.Frame.Right - EasyLayout.MarginNormal

				&& Enroll.Frame.Top == Address.Frame.Bottom + EasyLayout.MarginMedium
				&& Enroll.Frame.Left == CourseImageView.Frame.Right + EasyLayout.MarginNormal

				&& LessonPeroid.Frame.Top == Address.Frame.Bottom + EasyLayout.MarginMedium
				&& LessonPeroid.Frame.Left == Enroll.Frame.Right + EasyLayout.MarginSmall

				&& CoursePrice.Frame.Top == Enroll.Frame.Bottom + EasyLayout.MarginMedium
				&& CoursePrice.Frame.Left == CourseImageView.Frame.Right + EasyLayout.MarginNormal

				&& this.RatingView.Frame.Top == CourseImageView.Frame.Bottom
				&& this.RatingView.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
				&& RatingView.Frame.Width == ratingWidth
				&& RatingView.Frame.Height == ratingHeight

				&& PaidStatus.Frame.Bottom == RatingView.Frame.Bottom
				&& PaidStatus.Frame.Right == ContentView.Frame.Right - EasyLayout.MarginNormal

				&& ReviewStatus.Frame.Bottom == RatingView.Frame.Bottom
				&& ReviewStatus.Frame.Right == PaidStatus.Frame.Left - EasyLayout.MarginNormal

				&& Detail.Frame.Bottom == RatingView.Frame.Bottom - EasyLayout.MarginMedium
				&& Detail.Frame.Right == ContentView.Frame.Right - EasyLayout.MarginNormal
				&& Detail.Frame.Width == detailWidth

				&& this.SeparatorView.Frame.Top == RatingView.Frame.Bottom
				&& this.SeparatorView.Frame.Left == this.ContentView.Frame.Left
				&& this.SeparatorView.Frame.Right == this.ContentView.Frame.Right
				&& this.SeparatorView.Frame.Height == EasyLayout.SeparatorHeight

				&& this.SeparatorView.Frame.Bottom == this.ContentView.Frame.Bottom
			);
			_didSetupConstraints = true;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			this.ContentView.SetNeedsLayout();
			this.ContentView.LayoutIfNeeded();
			base.LayoutSubviews();
		}
	}
}

