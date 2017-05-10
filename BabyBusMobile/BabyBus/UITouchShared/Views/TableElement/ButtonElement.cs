using System;
using CoreGraphics;
using CrossUI.Touch.Dialog.Elements;
using UIKit;
using CrossUI.Touch.Dialog.Utilities;
using Foundation;

namespace BabyBus.iOS
{
    public class BCWButtonElement : UIViewElement
    {
        public BCWButtonElement(String caption, Action Handler)
            : base("", null, true)
        {
            CGRect rec = new CGRect(0, 0, 300, 50);
            UIView container = new UIView(rec);
            CGSize size = new NSString(caption).StringSize(UIFont.SystemFontOfSize(18), UIScreen.MainScreen.Bounds.Width, UILineBreakMode.TailTruncation);
            //			UIButton button = new UIButton (new RectangleF ((rec.Width - 8) / 2 - (size.Width + 20) / 2, 4, size.Width + 20, rec.Height - 8)) {
            //				Font = UIFont.SystemFontOfSize (18),
            //			};
            GlassButton button = new GlassButton(new CGRect((rec.Width - 8) / 2 - (size.Width + 20) / 2, 4, size.Width + 20, rec.Height - 8))
            {
                Font = UIFont.SystemFontOfSize(18),
                NormalColor = new UIColor(0.471f, 0.553f, 0.659f, 1.0f)
            };
            button.Tapped += delegate
            {
                if (Handler != null)
                {
                    Handler.Invoke();
                }
            };
            button.SetTitle(caption, UIControlState.Normal);
            container.AddSubview(button);
            View = container;
        }
    }
}

