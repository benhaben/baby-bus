using UIKit;
using Foundation;
using System.Drawing;
using BabyBus.iOS;
using CoreGraphics;
using System;


namespace BabyBus.iOS
{
    public class BubbleAnswerCell : UITableViewCell
    {
        public static NSString KeyLeft = new NSString("BubbleElementLeft");
        public static NSString KeyRight = new NSString("BubbleElementRight");
        public static UIImage bleft, bright, left, right;
        public static UIFont font = UIFont.SystemFontOfSize(14);
        UIView view;
        UIView imageView;
        bool isLeft;

        UILabel _teacherNameLabel = null;

        public UILabel TeacherNameLabel
        {
            get
            { 
                if (_teacherNameLabel == null)
                {
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

        public UILabel DateUILabel
        {
            get
            { 
                if (_dateUILabel == null)
                {
                    _dateUILabel = new UILabel();
                    _dateUILabel.Text = "时间";
                    _dateUILabel.Lines = 0;
                    _dateUILabel.TextAlignment = UITextAlignment.Right;
                    _dateUILabel.Font = EasyLayout.SmallFont;
                }
                return _dateUILabel;
            }
        }

        UILabel _answerLabel = null;

        public UILabel AnswerLabel
        {
            get
            { 
                if (_answerLabel == null)
                {
                    _answerLabel = new UILabel();
                    _answerLabel.Text = "回答";
                    _answerLabel.Lines = 0;
                    _answerLabel.Font = EasyLayout.ContentFont;
                    _answerLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
                }
                return _answerLabel;
            }
        }

        static BubbleAnswerCell()
        {
            bright = UIImage.FromFile("bubble_green.png");
            bleft = UIImage.FromFile("bubble_grey.png");

            // buggy, see https://bugzilla.xamarin.com/show_bug.cgi?id=6177
            //left = bleft.CreateResizableImage (new UIEdgeInsets (10, 16, 18, 26));
            //right = bright.CreateResizableImage (new UIEdgeInsets (11, 11, 17, 18));
            left = bleft.StretchableImage(26, 16);
            right = bright.StretchableImage(11, 11);
        }

        public BubbleAnswerCell(bool isLeft)
            : base(UITableViewCellStyle.Default, isLeft ? KeyLeft : KeyRight)
        {
            var rect = new RectangleF(0, 0, 1, 1);
            this.isLeft = isLeft;
            view = new UIView(rect);
            imageView = new UIImageView(isLeft ? left : right);
            view.AddSubview(imageView);
            view.AddSubview(AnswerLabel);
            ContentView.Add(view);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var frame = ContentView.Frame;
            var size = GetSizeForText(this, AnswerLabel.Text) + BubblePadding;
            imageView.Frame = new CGRect(new CGPoint(isLeft ? 10 : frame.Width - size.Width - 10, frame.Y), size);
            view.SetNeedsDisplay();
            frame = imageView.Frame;
            AnswerLabel.Frame = new CGRect(new CGPoint(frame.X + (isLeft ? 12 : 8), frame.Y + 6), size - BubblePadding);
        }

        static internal CGSize BubblePadding = new CGSize(22, 16);

        static internal CGSize GetSizeForText(UIView tv, string text)
        {
            var availableSize = new SizeF(296, float.MaxValue);
            var s = new NSString(text);
            var rec = s.GetBoundingRect(
                 availableSize,
                 NSStringDrawingOptions.UsesLineFragmentOrigin,
                 new UIStringAttributes
                {
                    ParagraphStyle = new NSParagraphStyle { LineBreakMode = UILineBreakMode.WordWrap },
                    Font = font
                },
                 context: null);
            return rec.Size;
        }

        public nfloat GetHeight(UIView tv)
        {
            return GetSizeForText(tv, AnswerLabel.Text).Height + BubblePadding.Height;
        }
    }
}

