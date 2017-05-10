using System;
using UIKit;
using CoreGraphics;

namespace BabyBus.iOS
{
    public class InsetsTextField :UITextField
    {
        public UIEdgeInsets Insets
        {
            get;
            set;
        }

        public InsetsTextField(UIEdgeInsets insets)
        {
            Insets = insets;
        }

        public override CGRect TextRect(CGRect forBounds)
        {
            //placeholder
            return forBounds.Inset(Insets.Left, 0);
        }

        public override CGRect EditingRect(CGRect forBounds)
        {
            //edit
            return forBounds.Inset(Insets.Left, 0);
        }
       

    }
}

