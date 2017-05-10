using UIKit;
using CoreGraphics;

namespace BabyBus.iOS
{
    public class InsetsLabel :UILabel
    {

        public UIEdgeInsets Insets
        {
            get;
            set;
        }

        UIEdgeInsets InsetsConstants = new UIEdgeInsets(3, 5, 3, 5);

        public InsetsLabel(UIEdgeInsets insets)
        {
            Insets = insets;
        }

        public InsetsLabel()
        {
            Insets = InsetsConstants;
        }

        public override void DrawText(CoreGraphics.CGRect rect)
        {
            base.DrawText(Insets.InsetRect(rect));
        }

        //        http://stackoverflow.com/questions/18118021/how-to-resize-superview-to-fit-all-subviews-with-autolayout/18155803#18155803
        //        Further considerations exist if you have one or more UILabel's in your view that are multiline. For these it is imperitive that the preferredMaxLayoutWidth property be set correctly such that the label provides a correct intrinsicContentSize, which will be used in systemLayoutSizeFittingSize's calculation.
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (PreferredMaxLayoutWidth != this.Frame.Size.Width)
            {
                PreferredMaxLayoutWidth = Frame.Size.Width;
                this.SetNeedsUpdateConstraints();
            }
        }

        public override CoreGraphics.CGSize IntrinsicContentSize
        {
            get
            {
                CGSize size = base.IntrinsicContentSize;
                size.Width += this.Insets.Left + this.Insets.Right;
                size.Height += this.Insets.Top + this.Insets.Bottom;
                return size;
            }
        }
       
    }
}

