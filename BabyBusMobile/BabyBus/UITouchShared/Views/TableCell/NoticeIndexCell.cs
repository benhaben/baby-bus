using System;
using UIKit;
using Foundation;
using BabyBus.iOS;

namespace BabyBus.iOS
{
	public class NoticeIndexCell: UITableViewCell
	{
		public static readonly NSString Key = new NSString("NoticeIndexCell");


		UIImageView _noticeTypeImageView = null;

		public UIImageView NoticeTypeImageView {
			get { 
				if (_noticeTypeImageView == null) {
					_noticeTypeImageView = new UIImageView();
					_noticeTypeImageView.Image = UIImage.FromBundle("placeholder.png");
				}
				return _noticeTypeImageView;
			}
		}

		UILabel _teacherNameLabel = null;

		public UILabel TeacherNameLabel {
			get { 
				if (_teacherNameLabel == null) {
					_teacherNameLabel = new UILabel();
					_teacherNameLabel.Text = "姓名";
					_teacherNameLabel.Font = EasyLayout.SmallFont;
					_teacherNameLabel.TextColor = MvxTouchColor.Gray1;
				}
				return _teacherNameLabel;
			}
		}

		UILabel _titleUILabel = null;

		public UILabel TitleUILabel {
			get { 
				if (_titleUILabel == null) {
					_titleUILabel = new UILabel();
					_titleUILabel.Text = "标题";
					_titleUILabel.Font = EasyLayout.TitleFont;
					_titleUILabel.TextColor = MvxTouchColor.Black1;
					_titleUILabel.Lines = 0;
				}
				return _titleUILabel;
			}
		}

		UILabel _dateUILabel = null;

		public UILabel DateUILabel {
			get { 
				if (_dateUILabel == null) {
					_dateUILabel = new UILabel();
					_dateUILabel.Text = "时间";
					_dateUILabel.Font = EasyLayout.SmallFont;
					_dateUILabel.TextColor = MvxTouchColor.Gray1;
				}
				return _dateUILabel;
			}
		}

		UIImageView _unreadUIImageView = null;

		public UIImageView UnreadUIImageView {
			get { 
				if (_unreadUIImageView == null) {
					_unreadUIImageView = new UIImageView();
					_unreadUIImageView.Image = UIImage.FromBundle("images/notice_index_view/unread.png");
				}
				return _unreadUIImageView;
			}
		}

		UILabel _contentUILabel = null;

		public UILabel ContentUILabel {
			get { 
				if (_contentUILabel == null) {
					_contentUILabel = new UILabel();
					_contentUILabel.Lines = 3;
					_contentUILabel.Font = EasyLayout.ContentFont;
					_contentUILabel.Text = "内容";
					_contentUILabel.TextColor = MvxTouchColor.Gray1;

				}
				return _contentUILabel;
			}
		}

		UILabel _unReadLabel = null;

		public UILabel UnReadLabel {
			get { 
				if (_unReadLabel == null) {
					_unReadLabel = new UILabel();
					_unReadLabel.Lines = 1;
					_unReadLabel.Font = EasyLayout.ContentFont;
					_unReadLabel.Text = "未读";
					_unReadLabel.TextColor = MvxTouchColor.White;
					_unReadLabel.BackgroundColor = MvxTouchColor.Red;
				}
				return _unReadLabel;
			}
		}

		InsetsLabel _typeUILabel = null;

		public InsetsLabel TypeUILabel {
			get { 
				if (_typeUILabel == null) {
					_typeUILabel = new InsetsLabel();
					_typeUILabel.Font = EasyLayout.SmallFont;
					_typeUILabel.BackgroundColor = MvxTouchColor.Blue;
					_typeUILabel.TextColor = MvxTouchColor.White;
					_typeUILabel.Text = "类型  ";
				}
				return _typeUILabel;
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


		public NoticeIndexCell()
			: base(UITableViewCellStyle.Default, Key)
		{

			SelectionStyle = UITableViewCellSelectionStyle.Blue;

//            ContentView.Add(NoticeTypeImageView);
			ContentView.Add(TitleUILabel);
			ContentView.Add(TeacherNameLabel);
			ContentView.Add(TypeUILabel);
			ContentView.Add(DateUILabel);
			ContentView.Add(ContentUILabel);
			ContentView.Add(UnreadUIImageView);
			ContentView.Add(SeparatorView);
//            ContentView.Add(UnReadLabel);
		}


		//TODO: check if necessary
		private bool _didSetupConstraints = false;

		public override void UpdateConstraints() {
			base.UpdateConstraints();

			if (this._didSetupConstraints) {
				return;
			}
          
			nfloat UnreadUIImageViewOffset = 280;
			nfloat UnreadUIImageViewHeight = 27;
			nfloat UnreadUIImageViewWidth = 12;

			#if DEBUG1
            TitleUILabel.BackgroundColor = UIColor.Blue;
            DateUILabel.BackgroundColor = UIColor.Red;
            TeacherNameLabel.BackgroundColor = UIColor.Purple;
            this.ContentView.BackgroundColor = UIColor.Green;
			#endif

			this.ContentView.ConstrainLayout(
				() => 
//                _container.Frame.Top == this.ContentView.Frame.Top
//                && _container.Frame.Left == this.ContentView.Frame.Left
//                && _container.Frame.Right == this.ContentView.Frame.Right
//                && _container.Frame.Bottom == this.ContentView.Frame.Bottom

                this.TypeUILabel.Frame.Top == this.ContentView.Frame.Top + EasyLayout.MarginMedium
				&& this.TypeUILabel.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
				&& this.TypeUILabel.Frame.Height <= EasyLayout.NormalTextFieldHeight
//                && this.TypeUILabel.Frame.Width == EasyLayout.NormalTextFieldHeight


				&& this.UnreadUIImageView.Frame.Top == ContentView.Frame.Top - EasyLayout.MarginSmall
				&& this.UnreadUIImageView.Frame.Left == ContentView.Frame.Left + UnreadUIImageViewOffset
				&& this.UnreadUIImageView.Frame.Width == UnreadUIImageViewWidth
				&& this.UnreadUIImageView.Frame.Height == UnreadUIImageViewHeight

//                && this.UnReadLabel.Frame.Top == this.ContentView.Frame.Top + EasyLayout.MarginMedium
//                && this.UnReadLabel.Frame.Left == this.TypeUILabel.Frame.Right + EasyLayout.MarginMedium

				&& this.TitleUILabel.Frame.Top == this.TypeUILabel.Frame.Bottom + EasyLayout.MarginMedium
				&& this.TitleUILabel.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
				&& this.TitleUILabel.Frame.Right == this.ContentView.Frame.Right - EasyLayout.MarginMedium
//                && this.TitleUILabel.Frame.Height == EasyLayout.NormalTextFieldHeight

				&& this.ContentUILabel.Frame.Top == TitleUILabel.Frame.Bottom + EasyLayout.MarginMedium
				&& this.ContentUILabel.Frame.Left == this.TitleUILabel.Frame.Left
				&& this.ContentUILabel.Frame.Right == this.TitleUILabel.Frame.Right

				&& this.TeacherNameLabel.Frame.Top == ContentUILabel.Frame.Bottom + EasyLayout.MarginNormal
				&& this.TeacherNameLabel.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
//                && this.TeacherNameLabel.Frame.Height == EasyLayout.NormalTextFieldHeight


				&& this.DateUILabel.Frame.Top == TeacherNameLabel.Frame.Top
				&& this.DateUILabel.Frame.Right == this.TitleUILabel.Frame.Right
//                && this.DateUILabel.Frame.Height == EasyLayout.NormalTextFieldHeight


				&& this.SeparatorView.Frame.Top == DateUILabel.Frame.Bottom + EasyLayout.MarginMedium
				&& this.SeparatorView.Frame.Left == this.ContentView.Frame.Left
				&& this.SeparatorView.Frame.Right == this.ContentView.Frame.Right
				&& this.SeparatorView.Frame.Height == EasyLayout.SeparatorHeight

				&& this.SeparatorView.Frame.Bottom == this.ContentView.Frame.Bottom
			);

			this._didSetupConstraints = true;
		}

		public override void LayoutSubviews() {
			base.LayoutSubviews();

			this.ContentView.SetNeedsLayout();
			this.ContentView.LayoutIfNeeded();
			base.LayoutSubviews();
		}
	}
}
