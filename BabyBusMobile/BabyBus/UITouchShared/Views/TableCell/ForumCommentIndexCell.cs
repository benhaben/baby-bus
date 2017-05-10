using System;
using UIKit;
using Foundation;
using BabyBus.iOS;
using CoreGraphics;
using PatridgeDev;

namespace BabyBus.iOS
{
	public class ForumCommentIndexCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString("ForumCommentIndexCell");
		bool didSetupConstraints;

		public ForumCommentIndexCell()
		{
			ContentView.Add(UserName);
			ContentView.Add(CreateDate);
			ContentView.Add(Content);
			ContentView.Add(SeparatorView);
			SelectionStyle = UITableViewCellSelectionStyle.None;
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
					_createDate.Font = EasyLayout.TinyFont;
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

				this.UserName.Frame.Top == this.ContentView.Frame.Top
				&& this.UserName.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginMedium
				&& this.UserName.Frame.Height == EasyLayout.NormalTextFieldHeight


				&& this.CreateDate.Frame.Top == this.UserName.Frame.Bottom
				&& this.CreateDate.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal

				&& Content.Frame.Top == CreateDate.Frame.Bottom + EasyLayout.MarginMedium
				&& Content.Frame.Left == ContentView.Frame.Left + EasyLayout.MarginNormal
				&& Content.Frame.Right == ContentView.Frame.Right - EasyLayout.MarginNormal
//				&& Content.Frame.Height == EasyLayout.SmallTextFieldHeight

				&& this.SeparatorView.Frame.Top == Content.Frame.Bottom
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

