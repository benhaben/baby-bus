// Decompiled with JetBrains decompiler
// Type: RadialProgress.BezierPathGenerator
// Assembly: RadialProgress.iOS, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 4ACC5FD7-E0EF-4B72-8CE9-961D44E56BA5
// Assembly location: C:\Users\jkm\Desktop\RadialProgress.iOS.dll

using System;
using UIKit;
using CoreGraphics;

namespace BabyBus.iOS
{
    internal class BezierPathGenerator
    {
        private const float HalfMathPi = 1.570796f;

        public static UIBezierPath Bagel(CGPoint center, nfloat startRadius, nfloat endRadius, nfloat startAngle, nfloat endAngle)
        {
            UIBezierPath uiBezierPath = new UIBezierPath();
            float num = -1.570796f;
            nfloat radius1 = (startRadius + endRadius) / (nfloat)2f;
            nfloat radius2 = (endRadius - startRadius) / (nfloat)2f;
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.AddArc(BezierPathGenerator.RotatePoint(center, radius1, (double)(startAngle + (nfloat)num)), radius2, -1.570796f, -4.712389f, true);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.AddArc(center, startRadius, startAngle + (nfloat)num, endAngle + (nfloat)num, true);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.AddArc(BezierPathGenerator.RotatePoint(center, radius1, (double)(endAngle + (nfloat)num)), radius2, (nfloat)1.570796f + endAngle, (nfloat)4.712389f + endAngle, false);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.AddArc(center, endRadius, endAngle + (nfloat)num, startAngle + (nfloat)num, false);
            // ISSUE: reference to a compiler-generated method
            uiBezierPath.ClosePath();
            return uiBezierPath;
        }

        private static CGPoint RotatePoint(CGPoint center, nfloat radius, double phi)
        {
            double num1 = Math.Sin(phi);
            double num2 = Math.Cos(phi);
            return new CGPoint((nfloat)((double)center.X + (double)radius * num2), (nfloat)((double)center.Y + (double)radius * num1));
        }
    }
}
