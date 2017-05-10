using Foundation;
using UIKit;
using CoreGraphics;

namespace BabyBus.iOS
{
    //copy from Xamarin.Controls.LoginScreen.iOS.dll
    //so the code is strange
    internal class BubbleView : UIView
    {
        private UIImageView backgroundView;
        private UILabel label;

        public string Text
        {
            get
            {
                return this.label.Text;
            }
            set
            {
                this.label.Text = (value);
                ((UIView)this.label).SizeToFit();
                this.SetNeedsLayout();
            }
        }

        public BubbleView()
        {
            this.UserInteractionEnabled = (false);
            this.backgroundView = new UIImageView(UIImage.FromFile("alert.png").StretchableImage(28, 0));
            UILabel uiLabel1 = new UILabel();
            uiLabel1.BackgroundColor = UIColor.Clear;
//			((UIView) uiLabel1).SetBackgroundColor(UIColor.get_Clear());
            uiLabel1.Font = UIFont.FromName("Helvetica", 13f);
//			uiLabel1.Font =  (Fonts.HelveticaNeueMedium(13f));
            uiLabel1.TextColor = (UIColor.White);
            uiLabel1.ShadowColor = (UIColor.Black.ColorWithAlpha(0.4f));
            uiLabel1.ShadowOffset = (new CGSize(0.0f, 1f));
            UILabel uiLabel2 = uiLabel1;
            CGRect frame1 = ((UIView)this.backgroundView).Frame;
            // ISSUE: explicit reference operation
            double num1 = (double)((CGRect)frame1).X + 10.0;
            CGRect frame2 = ((UIView)this.backgroundView).Frame;
            // ISSUE: explicit reference operation
            double num2 = (double)((CGRect)frame2).Y + 2.0;
            CGRect frame3 = ((UIView)this.backgroundView).Frame;
            // ISSUE: explicit reference operation
            double num3 = (double)((CGRect)frame3).Width;
            CGRect frame4 = ((UIView)this.backgroundView).Frame;
            // ISSUE: explicit reference operation
            double num4 = (double)((CGRect)frame4).Height;
            CGRect rectangleF = new CGRect((float)num1, (float)num2, (float)num3, (float)num4);
            ((UIView)uiLabel2).Frame = (rectangleF);
            this.label = uiLabel1;
            UIView[] uiViewArray = new UIView[2];
            int index1 = 0;
            UIImageView uiImageView = this.backgroundView;
            uiViewArray[index1] = (UIView)uiImageView;
            int index2 = 1;
            UILabel uiLabel3 = this.label;
            uiViewArray[index2] = (UIView)uiLabel3;
            this.AddSubviews(uiViewArray);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            UIImageView uiImageView = this.backgroundView;
            double num1 = 0.0;
            double num2 = 0.0;
            CGRect frame = ((UIView)this.label).Frame;
            // ISSUE: explicit reference operation
            double num3 = (double)((CGRect)frame).Width + 20.0;
            CGRect bounds1 = this.Bounds;
            // ISSUE: explicit reference operation
            double num4 = (double)((CGRect)bounds1).Height;
            CGRect rectangleF1 = new CGRect((float)num1, (float)num2, (float)num3, (float)num4);
            ((UIView)uiImageView).Frame = (rectangleF1);
            UILabel uiLabel = this.label;
            double num5 = 10.0;
            double num6 = 2.0;
            CGRect bounds2 = this.Bounds;
            // ISSUE: explicit reference operation
            double num7 = (double)((CGRect)bounds2).Width;
            CGRect bounds3 = this.Bounds;
            // ISSUE: explicit reference operation
            double num8 = (double)((CGRect)@bounds3).Height;
            CGRect rectangleF2 = new CGRect((float)num5, (float)num6, (float)num7, (float)num8);
            ((UIView)uiLabel).Frame = (rectangleF2);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;
            ((NSObject)this.label).Dispose();
            ((NSObject)this.backgroundView.Image).Dispose();
            ((NSObject)this.backgroundView).Dispose();
        }
    }
}

