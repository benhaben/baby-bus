using System;
using CircleButtonBinding;
using Foundation;
using UIKit;

namespace BabyBus.iOS
{
    public class CircleButton : DKCircleButton
    {
        public CircleButton(string title)
            : base()
        {
            BorderColor = MvxTouchColor.Orange;
            BackgroundColor = MvxTouchColor.Orange;
            TitleLabel.Font = UIFont.SystemFontOfSize(11);
            base.SetTitle(title, UIControlState.Normal);
            base.SetTitleColor(UIColor.White, UIControlState.Normal);
        }

        public CircleButton(NSCoder code)
            : base(code)
        {
        }

        public CircleButton(NSObjectFlag flag)
            : base(flag)
        {
        }

        public CircleButton(IntPtr handler)
            : base(handler)
        {
        }
    }
}

