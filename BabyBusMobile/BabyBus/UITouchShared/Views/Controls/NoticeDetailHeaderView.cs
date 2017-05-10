using System;
using UIKit;
using BabyBus.iOS;

namespace BabyBus.iOS
{
    public class NoticeDetailHeaderView:UIView
    {
        public NoticeDetailHeaderView()
        {
        }

        InsetsLabel _typeUILabel = null;

        public InsetsLabel TypeUILabel
        {
            get
            { 
                if (_typeUILabel == null)
                {
                    _typeUILabel = new InsetsLabel();
                    _typeUILabel.Font = EasyLayout.SmallFont;
                    _typeUILabel.BackgroundColor = MvxTouchColor.Gray1;
                    _typeUILabel.TextColor = MvxTouchColor.White;
                    _typeUILabel.Text = "类型";
                }
                return _typeUILabel;
            }
        }

        UILabel _teacherName = null;

        public UILabel TeacherName
        {
            get
            { 
                if (_teacherName == null)
                {
                    _teacherName = new InsetsLabel();
                    _teacherName.Font = EasyLayout.SmallFont;
                    //                    _teacherName.BackgroundColor = MvxTouchColor.Gray1;
                    _teacherName.TextColor = MvxTouchColor.Gray1;
                    _teacherName.Text = "老师名字";
                }
                return _teacherName;
            }
        }

        UILabel _date = null;

        public UILabel DateLabel
        {
            get
            { 
                if (_date == null)
                {
                    _date = new UILabel();
                    _date.Font = EasyLayout.SmallFont;
                    //                    _date.BackgroundColor = MvxTouchColor.Gray1;
                    _date.TextColor = MvxTouchColor.Gray1;
                    _date.Text = DateTime.Now.ToString();
                }
                return _date;
            }
        }

        public Action TapReadPersons;
        InsetsLabel _readPersons = null;

        public InsetsLabel ReadPersons
        {
            get
            { 
                if (_readPersons == null)
                {
                    _readPersons = new InsetsLabel();
                    _readPersons.Font = EasyLayout.SmallFont;
                    _readPersons.BackgroundColor = MvxTouchColor.Gray1;
                    _readPersons.TextColor = MvxTouchColor.White;
                    _readPersons.Text = "已读：0人";
                    _readPersons.UserInteractionEnabled = true;
                    var g = new UITapGestureRecognizer(() =>
                        {
                            if (TapReadPersons != null)
                            {
                                TapReadPersons();
                            }
                        });
                    g.NumberOfTapsRequired = 1;
                    _readPersons.AddGestureRecognizer(g);

                }
                return _readPersons;
            }
        }

        //
        //        UIImageView _teacherImage = null;
        //
        //        public UIImageView TeacherImage
        //        {
        //            get
        //            {
        //                if (_teacherImage == null)
        //                {
        //                    _teacherImage = new UIImageView();
        //                    _teacherImage.Layer.CornerRadius = 20;
        //                    _teacherImage.Layer.MasksToBounds = false;
        //                    _teacherImage.ContentMode = UIViewContentMode.ScaleAspectFill;
        //                    _teacherImage.ClipsToBounds = true;
        //                }
        //                return _teacherImage;
        //            }
        //        }


        UITextView _title = null;

        public UITextView NoticeTitle
        {
            get
            {
                if (_title == null)
                {   
                    _title = new UITextView();
                    _title.TextAlignment = UITextAlignment.Left;
                    _title.Editable = false;
                    _title.TextContainerInset = new UIEdgeInsets(10, 10, 10, 10);
                    _title.ScrollEnabled = true;
                    _title.Font = EasyLayout.TitleFont;
                    _title.BackgroundColor = MvxTouchColor.White;
                    _title.TextColor = MvxTouchColor.Black1;
                }
                return _title;
            }
        }

        UIView _container = null;

        public UIView Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new UIView();
                    _container.BackgroundColor = MvxTouchColor.White;
                }
                return _container;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            UIView[] v =
                {
                    TypeUILabel,
                    ReadPersons,
                    NoticeTitle,
                    TeacherName,
                    DateLabel
                };
            Container.AddSubviews(v);
            AddSubviews(Container);
            SetUpConstrainLayout();
            base.LayoutSubviews();
        }

        void SetUpConstrainLayout()
        {

            this.ConstrainLayout(
                () => 
                Container.Frame.Top == Frame.Top
                && Container.Frame.Left == Frame.Left
                && Container.Frame.Right == Frame.Right
                && Container.Frame.Bottom == Frame.Bottom

                && TypeUILabel.Frame.Left == Container.Frame.Left + EasyLayout.MarginNormal
                && TypeUILabel.Frame.Top == Container.Frame.Top + EasyLayout.MarginNormal

                && ReadPersons.Frame.Left == TypeUILabel.Frame.Right + EasyLayout.MarginMedium
                && ReadPersons.Frame.Top == TypeUILabel.Frame.Top

                && NoticeTitle.Frame.Left == Container.Frame.Left
                && NoticeTitle.Frame.Right == Container.Frame.Right
                && NoticeTitle.Frame.Top == TypeUILabel.Frame.Bottom + EasyLayout.MarginSmall

                //                && TeacherImage.Frame.Left == Container.Frame.Left + EasyLayout.MarginNormal
                //                && TeacherImage.Frame.Right == Container.Frame.Right
                //                && TeacherImage.Frame.Top == TypeUILabel.Frame.Bottom + EasyLayout.MarginSmall

                && TeacherName.Frame.Left == TypeUILabel.Frame.Left
                && TeacherName.Frame.Top == NoticeTitle.Frame.Bottom + EasyLayout.MarginSmall

                && DateLabel.Frame.Right == Container.Frame.Right - EasyLayout.MarginMedium
                && DateLabel.Frame.GetCenterY() == TeacherName.Frame.GetCenterY()
            );

            var constrains =
                this.ConstrainLayout(
                    () => 
                    NoticeTitle.Frame.Height == EasyLayout.NormalTextViewHeight
                );
            _heightConstrainTitle = constrains[0];

        }

        NSLayoutConstraint _heightConstrainTitle;

    }
}

