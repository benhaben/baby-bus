using System;
using UIKit;
using CoreGraphics;
using CoreAnimation;
using Foundation;

//This code has not been test. do not copy it
namespace BabyBus.iOS
{
    public class AdvertiseBarOfTeacher : UIControl
    {

        UILabel _childName = null;

        public UILabel Name
        {
            get
            { 
                if (_childName == null)
                {
                    _childName = new UILabel();
                    _childName.Font = EasyLayout.TitleFontBold;
                    _childName.TextColor = EasyLayout.TextColor;
                }
                return _childName;
            }
            set{ _childName = value; }
        }

        UILabel _childBirthday = null;

        public UILabel ChildBirthday
        {
            get
            { 
                if (_childBirthday == null)
                {
                    _childBirthday = new UILabel();
                    _childBirthday.Font = EasyLayout.ContentFont;
                    _childBirthday.TextColor = EasyLayout.TextColor;

                }
                return _childBirthday;
            }
            set{ _childBirthday = value; }
        }

        UILabel _childGender = null;

        public UILabel ChildGender
        {
            get
            { 
                if (_childGender == null)
                {
                    _childGender = new UILabel();
                    _childGender.Font = EasyLayout.TitleFont;
                    _childGender.TextColor = EasyLayout.TextColor;
                }
                return _childGender;
            }
            set{ _childGender = value; }
        }

        UILabel _city = null;

        public UILabel City
        {
            get
            { 
                if (_city == null)
                {
                    _city = new UILabel();
                    _city.Font = EasyLayout.ContentFont;
                    _city.TextColor = EasyLayout.TextColor;
                }
                return _city;
            }
            set{ _city = value; }
        }

        UILabel _kindergarden = null;

        public UILabel Kindergarden
        {
            get
            { 
                if (_kindergarden == null)
                {
                    _kindergarden = new UILabel();
                    _kindergarden.Font = EasyLayout.ContentFont;
                    _kindergarden.TextColor = EasyLayout.TextColor;
                }
                return _kindergarden;
            }
            set{ _kindergarden = value; }
        }

        UILabel _className = null;

        public UILabel ClassName
        {
            get
            { 
                if (_className == null)
                {
                    _className = new UILabel();
                    _className.Font = EasyLayout.ContentFont;
                    _className.TextColor = EasyLayout.TextColor;
                }
                return _className;
            }
            set{ _className = value; }
        }

        UITextField _blurLabel = null;

        public UITextField BlurLabel
        {
            get
            { 
                if (_blurLabel == null)
                {
                    _blurLabel = new UITextField();
                    _blurLabel.Frame = new CGRect(0, 0, 20, 100);
                    _blurLabel.Alpha = 0.8f;
                    CAGradientLayer gradient = new CAGradientLayer();
                    gradient.StartPoint = new CGPoint(0f, 0.5f);
                    gradient.EndPoint = new CGPoint(1f, 0.5f);
                    gradient.Frame = new CGRect(0, 0, 20, 100);
                    gradient.Locations = new Foundation.NSNumber[]
                    { 
                        new NSNumber(0.0),
                        new NSNumber(0.98),
                        new NSNumber(1.0)
                    };
                    gradient.Colors = new CoreGraphics.CGColor[]
                    { 
                        MvxTouchColor.BrightGreen.CGColor,
                        UIColor.White.CGColor,
                    };
                    
                    _blurLabel.Layer.InsertSublayer(gradient, 0);
                }
                return _blurLabel;
            }
            set{ _blurLabel = value; }
        }

        UIImageView _childImage = null;

        public UIImageView ChildImage
        {
            get
            { 
                if (_childImage == null)
                {
                    _childImage = new UIImageView();
                    _childImage.Image = UIImage.FromBundle("placeholder.png");
                }
                return _childImage;
            }
            set{ _childImage = value; }
        }

        public AdvertiseBarOfTeacher()
            : base()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            Frame = new CGRect(0, 0, 320, 100);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Name.Text = "王大翠";
            ChildBirthday.Text = "2011年5月9日";
            ChildGender.Text = "男";
            City.Text = "西安";
            Kindergarden.Text = "陕西第一国立幼儿园";
            ClassName.Text = "一年级一班";

            UIView[] v =
                {
                Name,
                ChildBirthday,
                ChildGender,
                City,
                Kindergarden,
                ClassName,
                ChildImage,
                BlurLabel
            };
            AddSubviews(v);
            SetUpConstrainLayout();
        }

        void SetUpConstrainLayout()
        {

            this.ConstrainLayout(
                () => 
                Name.Frame.Top == Frame.Top
                && Name.Frame.Left == Frame.Left
//                && Name.Frame.Right == Frame.Right

                && ChildGender.Frame.Top == Name.Frame.Top
                && ChildGender.Frame.Left == Name.Frame.Right + EasyLayout.MarginSmall

                && ChildBirthday.Frame.Top == Name.Frame.Bottom + EasyLayout.MarginSmall
                && ChildBirthday.Frame.Left == Name.Frame.Left
                && ChildBirthday.Frame.Right == Name.Frame.Right

                && City.Frame.Top == ChildBirthday.Frame.Bottom
                && City.Frame.Left == ChildBirthday.Frame.Left
//                && City.Frame.Right == ChildBirthday.Frame.Right

                && Kindergarden.Frame.Top == City.Frame.Top
                && Kindergarden.Frame.Left == City.Frame.Right + EasyLayout.MarginSmall
//                && Kindergarden.Frame.Right == City.Frame.Right

                && ClassName.Frame.Top == Kindergarden.Frame.Bottom
                && ClassName.Frame.Left == Frame.Left
                && ClassName.Frame.Right == Kindergarden.Frame.Right

                && ChildImage.Frame.Top == Frame.Top
                && ChildImage.Frame.Right == Frame.Right
                && ChildImage.Frame.Height == 100
                && ChildImage.Frame.Width == 100

                && BlurLabel.Frame.Top == ChildImage.Frame.Top
                && BlurLabel.Frame.Left == ChildImage.Frame.Left
                && BlurLabel.Frame.Bottom == ChildImage.Frame.Bottom
                && BlurLabel.Frame.Width == 20
            );
        }
    }
}

