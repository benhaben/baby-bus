using System;
using UIKit;
using Foundation;
using BabyBus.iOS;


namespace BabyBus.iOS
{
    //has not been use, can be use in ChildInfomationElement later
    public sealed class ChildInfomationTableCell:UITableViewCell
    {
        public static readonly NSString Key = new NSString("ChildInfomationTableCell");
        private bool didSetupConstraints = false;

      
        UIImageView _childImage = null;

        public UIImageView ChildImageView
        {
            get
            { 
                if (_childImage == null)
                {
                    _childImage = new UIImageView();
                    //图形大小的一半
                    _childImage.Layer.CornerRadius = 20;
                    _childImage.Layer.MasksToBounds = false;
                    _childImage.ContentMode = UIViewContentMode.ScaleAspectFill;
                    _childImage.ClipsToBounds = true;
                    _childImage.Image = UIImage.FromBundle("placeholder.png");
                }
                return _childImage;
            }
        }

        public string PhoneNumber{ get; set; }

        UIImageView _message = null;

        public UIImageView MessageImageView
        {
            get
            { 
                if (_message == null)
                {
                    _message = new UIImageView();
                    _message.Image = UIImage.FromBundle("message.png");
                    UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() =>
                        {
                            UIApplication.SharedApplication.OpenUrl(new NSUrl("sms://" + this.PhoneNumber));
                        });
                    tapGesture.NumberOfTapsRequired = 1;  
                    tapGesture.NumberOfTouchesRequired = 1; 
                    _message.AddGestureRecognizer(tapGesture);
                    _message.UserInteractionEnabled = true;
                }
                return _message;
            }
        }

        UIImageView _phone = null;

        public UIImageView Phone
        {
            get
            { 
                if (_phone == null)
                {
                    _phone = new UIImageView();
                    _phone.Image = UIImage.FromBundle("phone.png");
                    UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() =>
                        {
                            UIApplication.SharedApplication.OpenUrl(new NSUrl("tel://" + PhoneNumber));
                        });
                    tapGesture.NumberOfTapsRequired = 1;  
                    tapGesture.NumberOfTouchesRequired = 1; 
                    _phone.AddGestureRecognizer(tapGesture);
                    _phone.UserInteractionEnabled = true;
                }
                return _phone;
            }
        }

        InsetsLabel _isAskForLeave = null;

        public InsetsLabel IsAskForLeave
        {
            get
            { 
                if (_isAskForLeave == null)
                {
                    _isAskForLeave = new InsetsLabel();
                    _isAskForLeave.Text = "未请假";
                    _isAskForLeave.Layer.CornerRadius = EasyLayout.CornerRadius;
                    _isAskForLeave.Layer.MasksToBounds = false;
                    _isAskForLeave.ContentMode = UIViewContentMode.ScaleAspectFill;
                    _isAskForLeave.ClipsToBounds = true;

                    _isAskForLeave.Font = EasyLayout.TinyFont;
                    _isAskForLeave.TextColor = MvxTouchColor.White;
                    _isAskForLeave.BackgroundColor = MvxTouchColor.Gray3;
                    _isAskForLeave.TextAlignment = UITextAlignment.Center;
                }
                return _isAskForLeave;
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

        UILabel _childName = null;

        public UILabel ChildName
        {
            get
            { 
                if (_childName == null)
                {
                    _childName = new UILabel();
                    _childName.Text = "孩子姓名";
                    _childName.TextColor = MvxTouchColor.Gray1;
                    _childName.Font = EasyLayout.SmallFont;
                    _childName.TextAlignment = UITextAlignment.Center;
                }
                return _childName;
            }
            set{ _childName = value; }
        }

        public ChildInfomationTableCell()
        {
            ContentView.Add(ChildImageView);
            ContentView.Add(MessageImageView);
            ContentView.Add(Phone);
            ContentView.Add(ChildName);
            ContentView.Add(IsAskForLeave);
            ContentView.Add(SeparatorView);
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (this.didSetupConstraints)
            {
                return;
            }

            nfloat ChildImageViewWidth = EasyLayout.HeadPortraitImageHeight;
            nfloat PhoneLeft = 320 / 5;
            nfloat MessageImageViewLeft = 300 / 5 * 2;
            nfloat PhoneImageHeight = EasyLayout.PhoneImageWidth / 1.48f;

            this.ContentView.ConstrainLayout(
                () =>
                this.ChildImageView.Frame.GetCenterY() == this.ContentView.Frame.GetCenterY()
                && this.ChildImageView.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
                && this.ChildImageView.Frame.Width == ChildImageViewWidth
                && this.ChildImageView.Frame.Height == ChildImageViewWidth

                && this.ChildName.Frame.GetCenterY() == this.ContentView.Frame.GetCenterY()
                && this.ChildName.Frame.Left == this.ChildImageView.Frame.Right + EasyLayout.MarginMedium

                && this.IsAskForLeave.Frame.GetCenterX() == this.ContentView.Frame.GetCenterX()

                && this.IsAskForLeave.Frame.GetCenterY() == this.ContentView.Frame.GetCenterY()
                && this.IsAskForLeave.Frame.Left == this.ChildName.Frame.Right + EasyLayout.MarginMedium

                && this.Phone.Frame.GetCenterY() == this.ContentView.Frame.GetCenterY()
                && this.Phone.Frame.Left == this.ContentView.Frame.GetCenterX() + PhoneLeft
                && this.Phone.Frame.Width == EasyLayout.PhoneImageWidth
                && this.Phone.Frame.Height == EasyLayout.PhoneImageWidth

                && this.MessageImageView.Frame.GetCenterY() == this.ContentView.Frame.GetCenterY()
                && this.MessageImageView.Frame.Left == this.ContentView.Frame.GetCenterX() + MessageImageViewLeft
                && this.MessageImageView.Frame.Width == EasyLayout.PhoneImageWidth
                && this.MessageImageView.Frame.Height == PhoneImageHeight

                && this.SeparatorView.Frame.Top >= ChildImageView.Frame.Bottom
                && this.SeparatorView.Frame.Left == this.ContentView.Frame.Left
                && this.SeparatorView.Frame.Right == this.ContentView.Frame.Right
                && this.SeparatorView.Frame.Height == EasyLayout.LineHeight

                && this.SeparatorView.Frame.Bottom == this.ContentView.Frame.Bottom
//                && this.Frame.Height >= FrameHeight
            );

            this.didSetupConstraints = true;
        }


        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.ContentView.SetNeedsLayout();
            this.ContentView.LayoutIfNeeded();

            this.ChildName.PreferredMaxLayoutWidth = this.ChildName.Frame.Width;
           
          
        }
    }
}

