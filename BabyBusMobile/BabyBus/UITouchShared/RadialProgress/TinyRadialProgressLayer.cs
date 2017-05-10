// Decompiled with JetBrains decompiler
// Type: RadialProgress.TinyRadialProgressLayer
// Assembly: RadialProgress.iOS, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 4ACC5FD7-E0EF-4B72-8CE9-961D44E56BA5
// Assembly location: C:\Users\jkm\Desktop\RadialProgress.iOS.dll

using CoreGraphics;
using System;
using UIKit;

namespace BabyBus.iOS
{
    internal class TinyRadialProgressLayer : RadialProgressLayer
    {
        private const float borderPadding = 0.5f;

        public TinyRadialProgressLayer()
            : base((nfloat)7f, (nfloat)9f, (nfloat)22f, (nfloat)21f)
        {
        }

        public override UIImage GenerateBackgroundImage()
        {
            CGPoint center = new CGPoint(RectangleFExtensions.GetMidX(this.BackBounds), RectangleFExtensions.GetMidY(this.BackBounds));
            UIGraphics.BeginImageContextWithOptions(this.BackBounds.Size, false, UIScreen.MainScreen.Scale);
            using (CGContext currentContext = UIGraphics.GetCurrentContext())
            {
                using (UIBezierPath uiBezierPath = BezierPathGenerator.Bagel(center, this.startRadius - (nfloat)0.5f, this.endRadius + (nfloat)0.5f, (nfloat)0.0f, (nfloat)6.283185f))
                {
                    currentContext.SaveState();
                    currentContext.SetFillColor(RadialProgressLayer.BackCircleBackgroundColor);
                    // ISSUE: reference to a compiler-generated method
                    uiBezierPath.Fill();
                    currentContext.RestoreState();
                    return UIGraphics.GetImageFromCurrentImageContext();
                }
            }
        }
    }
}
