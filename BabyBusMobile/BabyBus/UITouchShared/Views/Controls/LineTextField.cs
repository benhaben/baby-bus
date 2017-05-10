using UIKit;
using CoreAnimation;
using CoreGraphics;

namespace BabyBus.iOS
{
    public sealed class LineTextField : UITextField
    {
        public LineTextField(string placeHolder = null, UIColor color = null)
            : base(new CGRect(
                    0,
                    0,
                    EasyLayout.TextFieldWidth,
                    EasyLayout.NormalTextFieldHeight))
        {
            if (color == null)
            {
                color = MvxTouchColor.Gray2;
            }
            this.Placeholder = placeHolder;
            var line = EasyLayout.CreateLineLayer(this, color);
            Layer.AddSublayer(line);
        }
    }
}

