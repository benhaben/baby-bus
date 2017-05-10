using System;
using UIKit;
using Foundation;
using BabyBus.iOS;


namespace BabyBus.iOS
{
    public class QuestionAnswerCell: UITableViewCell
    {
        public static readonly NSString Key = new NSString("QuestionAnswerCell");


        //        UILabel _answerLabel = null;
        //
        //        public UILabel AnswerLabel {
        //            get {
        //                if (_answerLabel == null) {
        //                    _answerLabel = new UILabel();
        //                    _answerLabel.Text = "回答";
        //                    _answerLabel.Font = EasyLayout.SmallFont;
        //                }
        //                return _answerLabel;
        //            }
        //        }

        UILabel _nameLabel = null;

        public UILabel NameLabel
        {
            get
            { 
                if (_nameLabel == null)
                {
                    _nameLabel = new UILabel();
                    _nameLabel.Text = "姓名";
                    _nameLabel.Font = EasyLayout.SmallFont;
                    _nameLabel.TextColor = MvxTouchColor.Gray1;
                }
                return _nameLabel;
            }
        }

        UILabel _dateLabel = null;

        public UILabel DateLabel
        {
            get
            { 
                if (_dateLabel == null)
                {
                    _dateLabel = new UILabel();
                    _dateLabel.Text = "日期";
                    _dateLabel.Font = EasyLayout.SmallFont;
                    _dateLabel.TextColor = MvxTouchColor.Gray1;
                }
                return _dateLabel;
            }
        }

        UILabel _questionLabel = null;

        public UILabel QuestionLabel
        {
            get
            {
                if (_questionLabel == null)
                {
                    _questionLabel = new UILabel();
                    _questionLabel.Text = "是否回答";
                    _questionLabel.Lines = 3;
                    _questionLabel.Font = EasyLayout.ContentFont;
                    _questionLabel.TextColor = MvxTouchColor.Gray1;
                }
                return _questionLabel;
            }
        }

        //        UILabel _teacherNameLabel = null;
        //
        //        public UILabel TeacherNameLabel {
        //            get {
        //                if (_teacherNameLabel == null) {
        //                    _teacherNameLabel = new UILabel();
        //                    _teacherNameLabel.Text = "老师姓名";
        //                    _teacherNameLabel.Font = EasyLayout.ContentFont;
        //                }
        //                return _teacherNameLabel;
        //            }
        //        }

        UIImageView _typeImageView = null;

        public UIImageView TypeImageView
        {
            get
            { 
                if (_typeImageView == null)
                {
                    _typeImageView = new UIImageView();
                    _typeImageView.Image = UIImage.FromBundle("placeholder.png");
                }
                return _typeImageView;
            }
        }

        InsetsLabel _typeUILabel = null;

        public InsetsLabel TypeUILabel
        {
            get
            { 
                if (_typeUILabel == null)
                {
                    _typeUILabel = new InsetsLabel();
//                    _typeUILabel.Layer.CornerRadius = 4;
//                    _typeUILabel.Layer.MasksToBounds = false;
//                    _typeUILabel.ContentMode = UIViewContentMode.ScaleAspectFill;
//                    _typeUILabel.ClipsToBounds = true;
                    _typeUILabel.BackgroundColor = MvxTouchColor.Blue;
                    _typeUILabel.Text = "类型";
                    _typeUILabel.Font = EasyLayout.SmallFont;
                    _typeUILabel.TextColor = MvxTouchColor.White;
                }
                return _typeUILabel;
            }
        }

        InsetsLabel _isAnsweredLabel = null;

        public InsetsLabel IsAnsweredLabel
        {
            get
            { 
                if (_isAnsweredLabel == null)
                {
                    _isAnsweredLabel = new InsetsLabel();
//                    _isAnsweredLabel.Layer.CornerRadius = 4;
//                    _isAnsweredLabel.Layer.MasksToBounds = false;
//                    _isAnsweredLabel.ContentMode = UIViewContentMode.ScaleAspectFill;
//                    _isAnsweredLabel.ClipsToBounds = true;
                    _isAnsweredLabel.BackgroundColor = MvxTouchColor.LightRed;
                    _isAnsweredLabel.Text = "未回复";
                    _isAnsweredLabel.Font = EasyLayout.SmallFont;
                    _isAnsweredLabel.TextColor = MvxTouchColor.White;
                }
                return _isAnsweredLabel;
            }
        }

        UIView _separator = null;

        public UIView SeparatorView
        {
            get
            { 
                if (_separator == null)
                {
                    _separator = new UIView();
                    _separator.BackgroundColor = MvxTouchColor.White2;
                }
                return _separator;
            }
        }

        public QuestionAnswerCell()
            : base(UITableViewCellStyle.Default, Key)
        {

            SelectionStyle = UITableViewCellSelectionStyle.Blue;


//            ContentView.Add(AnswerLabel);
//            ContentView.Add(TeacherNameLabel);

            ContentView.Add(NameLabel);
            ContentView.Add(QuestionLabel);
            ContentView.Add(IsAnsweredLabel);
            ContentView.Add(DateLabel);
            ContentView.Add(TypeUILabel);
            ContentView.Add(SeparatorView);

//            ContentView.Add(TypeImageView);
        }

        private bool _didSetupConstraints = false;

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (this._didSetupConstraints)
            {
                return;
            }

            nfloat TypeImageViewHeight = 15;

            #if DEBUG1
//            TeacherNameLabel.BackgroundColor = UIColor.Blue;
            NameLabel.BackgroundColor = UIColor.Red;
            IsAnsweredLabel.BackgroundColor = UIColor.Purple;
            DateLabel.BackgroundColor = UIColor.Brown;
//            AnswerLabel.BackgroundColor = UIColor.Cyan;
            QuestionLabel.BackgroundColor = UIColor.DarkGray;
            this.ContentView.BackgroundColor = UIColor.Green;
            #endif

            //老师家长的布局一样，但是看到的东西不一样
            //家长
            //问题 ＋ \n老师 ＋ 是否回答 ＋ 日期
            //老师
            //问题 + \n家长 + 是否回答 + 日期
            this.ContentView.ConstrainLayout(
                () => 
//                this.TypeImageView.Frame.GetCenterY() == this.TypeUILabel.Frame.GetCenterY()
//                && this.TypeImageView.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
//                && this.TypeImageView.Frame.Width == TypeImageViewHeight
//                && this.TypeImageView.Frame.Height == TypeImageViewHeight

                this.TypeUILabel.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
                && this.TypeUILabel.Frame.Top == this.ContentView.Frame.Top + EasyLayout.MarginMedium
                && this.TypeUILabel.Frame.Height <= EasyLayout.NormalTextFieldHeight


                && this.IsAnsweredLabel.Frame.Top == TypeUILabel.Frame.Top
                && this.IsAnsweredLabel.Frame.Left == this.TypeUILabel.Frame.Right + EasyLayout.MarginMedium
                && this.IsAnsweredLabel.Frame.Bottom == this.TypeUILabel.Frame.Bottom


                && this.QuestionLabel.Frame.Top == this.TypeUILabel.Frame.Bottom + EasyLayout.MarginMedium
                && this.QuestionLabel.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
                && this.QuestionLabel.Frame.Right == this.ContentView.Frame.Right - EasyLayout.MarginMedium

                && this.DateLabel.Frame.Top == QuestionLabel.Frame.Bottom + EasyLayout.MarginMedium
                && this.DateLabel.Frame.Right == this.QuestionLabel.Frame.Right
             
                && this.NameLabel.Frame.Top == DateLabel.Frame.Top
                && this.NameLabel.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
                && this.NameLabel.Frame.Bottom == this.DateLabel.Frame.Bottom

                && this.SeparatorView.Frame.Top == DateLabel.Frame.Bottom + EasyLayout.MarginMedium
                && this.SeparatorView.Frame.Left == this.ContentView.Frame.Left
                && this.SeparatorView.Frame.Right == this.ContentView.Frame.Right
                && this.SeparatorView.Frame.Height == EasyLayout.SeparatorHeight

                && this.SeparatorView.Frame.Bottom == this.ContentView.Frame.Bottom

            );

            this._didSetupConstraints = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.ContentView.SetNeedsLayout();
            this.ContentView.LayoutIfNeeded();
        }
    }
}
