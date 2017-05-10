using System;
using UIKit;
using Foundation;
using ObjCRuntime;

namespace ImageEffects {

    [Category, BaseType(typeof(UIImage))]
    public partial interface ImageEffects_UIImage {

        [Export("applyLightEffect")]
        UIImage ApplyLightEffect();

        [Export("applyExtraLightEffect")]
        UIImage ApplyExtraLightEffect();

        [Export("applyDarkEffect")]
        UIImage ApplyDarkEffect();

        [Export("applyTintEffectWithColor")]
        UIImage ApplyTintEffectWithColor(UIColor tintColor);

        [Export("applyBlurWithRadius:tintColor:saturationDeltaFactor:maskImage:")]
        UIImage ApplyBlurWithRadius(nfloat blurRadius, UIColor tintColor, nfloat saturationDeltaFactor, [NullAllowed]UIImage maskImage);
    }
}
