using System;
using UIKit;
using Foundation;
using BabyBus.iOS;
using CoreGraphics;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using MWPhotoBrowserBinding;
using BabyBus.Logic.Shared;
using UITouchShared;

namespace BabyBus.iOS
{
	public class ForumIndexCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("ForumIndexCell");
		private bool _didSetupConstraints;

		public ForumIndexCell()
			: base(UITableViewCellStyle.Default, Key)
		{
			ContentView.Add(HeadImageView);
			ContentView.Add(Title);
			ContentView.Add(Content);
			ContentView.Add(Abstract);
			ContentView.Add(UserName);
			ContentView.Add(FavoriteCount);
			ContentView.Add(CommentCount);
			ContentView.Add(CreateDate);
			ContentView.Add(ImageView);
			ContentView.Add(SeparatorView);

			SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		UIImageView _image = null;

		public virtual UIImageView HeadImageView {
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

		UILabel _title = null;

		public UILabel Title {
			get {
				if (_title == null) {   
					_title = new UILabel();
					_title.Font = EasyLayout.TitleFont;
					_title.TextColor = MvxTouchColor.Black1;
				}
				return _title;
			}
		}

		UILabel _createDate;

		public UILabel CreateDate {
			get {
				if (_createDate == null) {
					_createDate = new UILabel();
					_createDate.Text = "时间";
					_createDate.Font = EasyLayout.TinyFont;
					_createDate.TextColor = MvxTouchColor.Black1;
					_createDate.Lines = 0;
				}
				return _createDate;
			}
		}

		UILabel _userName = null;

		public UILabel UserName {
			get {
				if (_userName == null) {   
					_userName = new UILabel();
					_userName.Text = "UserName";
					_userName.Font = EasyLayout.SubTitleFont;
					_userName.TextColor = MvxTouchColor.Black1;
				}
				return _userName;
			}
		}

		UILabel _content = null;

		public UILabel Content {
			get {
				if (_content == null) {   
					_content = new UILabel();
					_content.Font = EasyLayout.SmallFont;
					_content.TextColor = MvxTouchColor.Gray1;
				}
				return _content;
			}
		}

		UILabel _abstract = null;

		public UILabel Abstract {
			get {
				if (_abstract == null) {   
					_abstract = new UILabel();
					_abstract.Font = EasyLayout.ContentFont;
					_abstract.TextColor = MvxTouchColor.Black1;
				}
				return _abstract;
			}
		}

		ImageLabel _commentCount;

		public ImageLabel CommentCount {
			get {
				if (_commentCount == null) {
					_commentCount = new ImageLabel();
					_commentCount.Image = UIImage.FromBundle("icon_lg_message.png");
					_commentCount.Text = "10";
					_commentCount.SizeToFit();
				}
				return _commentCount;
			}
		}

		ImageLabel _favoriteCount;

		public ImageLabel FavoriteCount {
			get {
				if (_favoriteCount == null) {
					_favoriteCount = new ImageLabel();
					_favoriteCount.Image = UIImage.FromBundle("icon_lg_praise.png");
					_favoriteCount.Text = "5";
					_favoriteCount.SizeToFit();
				}
				return _favoriteCount;
			}
		}

		UIImageView _imageView;

		public UIImageView ImageView {
			get { 
				if (_imageView == null) {
					_imageView = new UIImageView();
				}
				return _imageView;
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

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			this.ContentView.SetNeedsLayout();
			this.ContentView.LayoutIfNeeded();
			this.ContentView.UpdateConstraintsIfNeeded();
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();
			if (this._didSetupConstraints) {
				return;
			}

			nfloat defaultImageSize = 0;
			nfloat contentWidth = 300f;

			this.ContentView.ConstrainLayout(
				() =>
				Title.Frame.Top == ContentView.Frame.Top + EasyLayout.MarginMedium
				&& Title.Frame.Left == ContentView.Frame.Left + EasyLayout.MarginNormal
				&& Title.Frame.Right == this.ImageView.Frame.Left - EasyLayout.MarginNormal

				&& this.Abstract.Frame.Top == Title.Frame.Bottom
				&& this.Abstract.Frame.Left == ContentView.Frame.Left + EasyLayout.MarginNormal
				&& this.Abstract.Frame.Right == this.ImageView.Frame.Left - EasyLayout.MarginNormal

				&& CreateDate.Frame.Bottom == SeparatorView.Frame.Top - EasyLayout.MarginMedium
				&& CreateDate.Frame.Left == ContentView.Frame.Left + EasyLayout.MarginNormal

				&& CommentCount.Frame.Bottom == SeparatorView.Frame.Top - EasyLayout.MarginMedium
				&& CommentCount.Frame.Right == ImageView.Frame.Left - EasyLayout.MarginNormal

				&& FavoriteCount.Frame.Bottom == SeparatorView.Frame.Top - EasyLayout.MarginMedium
				&& FavoriteCount.Frame.Right == CommentCount.Frame.Left - EasyLayout.MarginMedium

				&& ImageView.Frame.Top == ContentView.Frame.Top + EasyLayout.MarginMedium
				&& ImageView.Frame.Left == Title.Frame.Right + EasyLayout.MarginNormal

				&& this.SeparatorView.Frame.Top == ContentView.Frame.Bottom - EasyLayout.MarginMedium
				&& this.SeparatorView.Frame.Left == this.ContentView.Frame.Left
				&& this.SeparatorView.Frame.Right == this.ContentView.Frame.Right
				&& this.SeparatorView.Frame.Height == EasyLayout.SeparatorHeight
			);

			var constrains = ContentView.ConstrainLayout(
				                 () =>
				ImageView.Frame.Height == defaultImageSize
				                 && ImageView.Frame.Width == defaultImageSize
				                 && Title.Frame.Width == contentWidth
				                 && Content.Frame.Width == contentWidth
			                 );

			_imageHeightConstraint = constrains[0];
			_imageWidthConstraint = constrains[1];
			_titleWidthConstraint = constrains[2];
			_contentWidthConstraint = constrains[3];

			_didSetupConstraints = true;
		}

		NSLayoutConstraint _imageHeightConstraint;
		NSLayoutConstraint _imageWidthConstraint;
		NSLayoutConstraint _contentWidthConstraint;
		NSLayoutConstraint _titleWidthConstraint;


		public void FitImage(bool haveImage)
		{
			if (_imageHeightConstraint != null && _imageWidthConstraint != null)
			if (haveImage) {
				_imageHeightConstraint.Constant = 55f;
				_imageWidthConstraint.Constant = 72f;
				_titleWidthConstraint.Constant = 320f - 72f - EasyLayout.MarginNormal * 3;
				_contentWidthConstraint.Constant = 320f - 72f - EasyLayout.MarginNormal * 3;
			} else {
				_imageHeightConstraint.Constant = 0f;
				_imageWidthConstraint.Constant = 0f;
				_titleWidthConstraint.Constant = 320f - EasyLayout.MarginNormal * 2;
				_contentWidthConstraint.Constant = 320f - EasyLayout.MarginNormal * 2;
			}
		}
	}
}
