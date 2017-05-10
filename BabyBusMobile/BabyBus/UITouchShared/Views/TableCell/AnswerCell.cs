using System;
using System.Drawing;
using UIKit;
using Foundation;
using CoreGraphics;
using BabyBus.iOS;

namespace Views.TableCell
{
	public class AnswerCell: UITableViewCell
	{
		public static readonly NSString KeyRight = new NSString("AnswerCellRight");
		public static readonly NSString KeyLeft = new NSString("AnswerCellLeft");
		nfloat HeadImageSize = 30;
		nfloat NameLabelHeight = 16;
		nfloat BubbleLeftMargin = 40;
		bool _isright;

		public static UIImage bleft, bright, left, right;

		static AnswerCell()
		{
			bright = UIImage.FromBundle("bubble_green.png");
			bleft = UIImage.FromBundle("bubble_grey.png");
			left = bleft.StretchableImage(26, 11);
			right = bright.StretchableImage(11, 11);
		}

		UIImageView _bubbleImage = null;

		public UIImageView BubbleImage {
			get { 
				if (_bubbleImage == null) {
					_bubbleImage = _isright ? new UIImageView(right) : new UIImageView(left);
				}
				return _bubbleImage;
			}
		}

		UILabel _teacherNameLabel = null;

		public UILabel TeacherNameLabel {
			get { 
				if (_teacherNameLabel == null) {
					_teacherNameLabel = new UILabel();
					_teacherNameLabel.Text = "姓名";
					_teacherNameLabel.Lines = 0;
					_teacherNameLabel.TextAlignment = UITextAlignment.Left;
					_teacherNameLabel.Font = EasyLayout.SmallFont;
				}
				return _teacherNameLabel;
			}
		}

		UILabel _dateUILabel = null;

		public UILabel DateUILabel {
			get { 
				if (_dateUILabel == null) {
					_dateUILabel = new UILabel();
					_dateUILabel.Text = "时间";
					_dateUILabel.Lines = 0;
					_dateUILabel.TextAlignment = UITextAlignment.Right;
					_dateUILabel.Font = EasyLayout.SmallFont;
				}
				return _dateUILabel;
			}
		}

		UIImageView _headImageView = null;

		public UIImageView HeadImageView {
			get {
				if (_headImageView == null) {
					_headImageView = new UIImageView(UIImage.FromBundle("placeholder.png"));
					var frame = _headImageView.Frame;
					frame.Width = HeadImageSize;
					frame.Height = HeadImageSize;
					_headImageView.Frame = frame;
				}
				return _headImageView;
			}
		}

		UILabel _answerLabel = null;

		public UILabel AnswerLabel {
			get { 
				if (_answerLabel == null) {
					_answerLabel = new UILabel();
					_answerLabel.Text = "回答";
					_answerLabel.Lines = 0;
					_answerLabel.TextAlignment = UITextAlignment.Left;
					_answerLabel.Font = EasyLayout.ContentFont;
					_answerLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
				}
				return _answerLabel;
			}
		}

		UIView view;

		UIView imageView;

		public AnswerCell(bool isright)
			: base(UITableViewCellStyle.Default, isright ? KeyRight : KeyLeft)
		{
			_isright = isright;

			//TODO: use a nice picture 
			SelectionStyle = UITableViewCellSelectionStyle.Blue;

			TeacherNameLabel.TextColor = MvxTouchColor.Gray1;
			DateUILabel.TextColor = MvxTouchColor.Gray1;
			AnswerLabel.TextColor = MvxTouchColor.Black1;


			imageView = new UIView();
			var headimage = new UIImageView(UIImage.FromBundle("placeholder.png"));

			var frame = headimage.Frame;
			frame.Width = 30;
			frame.Height = 30;
			headimage.Frame = frame;
			imageView.Add(headimage);
			var rect = new CGRect(0, 0, 1, 1);
			view = new UIView(rect);
			view.AddSubview(BubbleImage);
			view.AddSubview(AnswerLabel);

			ContentView.Add(BubbleImage);
			ContentView.Add(TeacherNameLabel);
			ContentView.Add(AnswerLabel);
			ContentView.Add(HeadImageView);
			//ContentView.Add(DateUILabel);

			//ContentView.Add(imageView);
			//ContentView.Add(view);


			#if DEBUG1
            DateUILabel.BackgroundColor = UIColor.Orange;
            AnswerLabel.BackgroundColor = UIColor.Blue;
            TeacherNameLabel.BackgroundColor = UIColor.Purple;
			#endif
		}


		public override void LayoutSubviews() {
			base.LayoutSubviews();

//			var frame = ContentView.Frame;
//			var size = GetSizeForText(AnswerLabel.Text) + BubblePadding;
//			BubbleImage.Frame = new CGRect(new CGPoint(_isright ? frame.Width - size.Width - 45 : 45, frame.Y), size);
//			view.SetNeedsDisplay();
//			frame = BubbleImage.Frame;
//			AnswerLabel.Frame = new CGRect(new CGPoint(frame.X + (_isright ? 8 : 12), frame.Y + 6), size - BubblePadding);

			this.ContentView.SetNeedsLayout();
			this.ContentView.LayoutIfNeeded();

			base.LayoutSubviews();
		}

		static internal CGSize BubblePadding = new SizeF(22, 16);

		static internal CGSize AvailableSize = new CGSize(240, float.MaxValue);

		private CGSize GetSizeForText(string text) {
			var s = new NSString(text);
			var size = s.StringSize(EasyLayout.ContentFont, AvailableSize, UILineBreakMode.CharacterWrap);
			return size;
		}

		private bool _didSetupConstraints = false;

		public override void UpdateConstraints() {
			base.UpdateConstraints();

			if (this._didSetupConstraints) {
				return;
			}
				
			nfloat margin = 15f;


			if (_isright) {
				this.ContentView.ConstrainLayout(
					() => 

                    this.BubbleImage.Frame.Top >= this.ContentView.Frame.Top
					&& this.BubbleImage.Frame.Right == this.HeadImageView.Frame.Left - EasyLayout.MarginMedium
					&& this.BubbleImage.Frame.Left >= this.ContentView.Frame.Left + BubbleLeftMargin

					&& this.HeadImageView.Frame.Top >= this.ContentView.Frame.Top
					&& this.HeadImageView.Frame.Right == this.ContentView.Frame.Right - EasyLayout.MarginMedium
					&& this.HeadImageView.Frame.Bottom == this.BubbleImage.Frame.Bottom

					&& this.AnswerLabel.Frame.Top == BubbleImage.Frame.Top + EasyLayout.MarginNormal
					&& this.AnswerLabel.Frame.Left == this.BubbleImage.Frame.Left + EasyLayout.MarginNormal
					&& this.AnswerLabel.Frame.Right == this.BubbleImage.Frame.Right - margin
					&& this.AnswerLabel.Frame.Bottom == BubbleImage.Frame.Bottom - margin

					&& this.TeacherNameLabel.Frame.Right == this.ContentView.Frame.Right - EasyLayout.MarginMedium
					&& this.TeacherNameLabel.Frame.Top == this.BubbleImage.Frame.Bottom + EasyLayout.MarginMedium
                    //
//					&& this.DateUILabel.Frame.Top == TeacherNameLabel.Frame.Top
//					&& this.DateUILabel.Frame.Right == this.AnswerLabel.Frame.Right
//					&& this.DateUILabel.Frame.Bottom == this.TeacherNameLabel.Frame.Bottom
//					&& this.DateUILabel.Frame.Left == this.TeacherNameLabel.Frame.Right



				);
			} else {
				this.ContentView.ConstrainLayout(
					() => 

                    this.BubbleImage.Frame.Top >= this.ContentView.Frame.Top
					&& this.BubbleImage.Frame.Left == this.HeadImageView.Frame.Right + EasyLayout.MarginMedium
					&& this.BubbleImage.Frame.Right <= this.ContentView.Frame.Right - BubbleLeftMargin

					&& this.HeadImageView.Frame.Top >= this.ContentView.Frame.Top
					&& this.HeadImageView.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginMedium
					&& this.HeadImageView.Frame.Bottom == this.BubbleImage.Frame.Bottom

					&& this.AnswerLabel.Frame.Top == BubbleImage.Frame.Top + EasyLayout.MarginNormal
					&& this.AnswerLabel.Frame.Left == this.BubbleImage.Frame.Left + margin
					&& this.AnswerLabel.Frame.Right == this.BubbleImage.Frame.Right - EasyLayout.MarginNormal
					&& this.AnswerLabel.Frame.Bottom == BubbleImage.Frame.Bottom - margin

					&& this.TeacherNameLabel.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginMedium
					&& this.TeacherNameLabel.Frame.Top == this.HeadImageView.Frame.Bottom + EasyLayout.MarginMedium
				);
			}
			nfloat NameLabelHeight = 16;

//			var textsize = AnswerLabel.SizeThatFits(AvailableSize);
//
			var constrains = 
				ContentView.ConstrainLayout(
					() => HeadImageView.Frame.Height == HeadImageSize
					&& HeadImageView.Frame.Width == HeadImageSize
					&& this.TeacherNameLabel.Frame.Height == NameLabelHeight
				);

			this._didSetupConstraints = true;
		}

	}
}
