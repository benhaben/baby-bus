// Decompiled with JetBrains decompiler
// Type: RadialProgress.SmallRadialProgressLayer
// Assembly: RadialProgress.iOS, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 4ACC5FD7-E0EF-4B72-8CE9-961D44E56BA5
// Assembly location: C:\Users\jkm\Desktop\RadialProgress.iOS.dll

using CoreGraphics;
using System;
using UIKit;

namespace BabyBus.iOS
{
    internal class SmallRadialProgressLayer : RadialProgressLayer
    {
        public SmallRadialProgressLayer()
            : base((nfloat)12f, (nfloat)17f, (nfloat)36f, (nfloat)34f)
        {
        }

        public override UIImage GenerateBackgroundImage()
        {
            UIGraphics.BeginImageContextWithOptions(this.BackBounds.Size, false, UIScreen.MainScreen.Scale);
            using (CGContext currentContext = UIGraphics.GetCurrentContext())
            {
                // ISSUE: reference to a compiler-generated method
                using (UIBezierPath uiBezierPath = UIBezierPath.FromOval(new CGRect(CGPoint.Empty, this.BackBounds.Size)))
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
