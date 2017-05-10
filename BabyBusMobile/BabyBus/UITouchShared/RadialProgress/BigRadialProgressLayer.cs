// Decompiled with JetBrains decompiler
// Type: RadialProgress.BigRadialProgressLayer
// Assembly: RadialProgress.iOS, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 4ACC5FD7-E0EF-4B72-8CE9-961D44E56BA5
// Assembly location: C:\Users\jkm\Desktop\RadialProgress.iOS.dll

using CoreGraphics;
using System;
using UIKit;

namespace BabyBus.iOS
{
    internal class BigRadialProgressLayer : RadialProgressLayer
    {
        private static readonly CGColor GlowColor = UIColor.White.ColorWithAlpha((nfloat)0.5f).CGColor;
        private static readonly CGColor GradientOverlayStartColor = UIColor.White.ColorWithAlpha((nfloat)0.7f).CGColor;
        private static readonly CGColor GradientOverlayEndColor = UIColor.Clear.CGColor;
        private const float GlowOffset = 1f;
        private const float GlowRadius = 9f;
        private const float BorderPadding = 5f;
        private const float EndBorderRadius = 105f;
        private const float StartBorderRadius = 70f;

        public BigRadialProgressLayer()
            : base((nfloat)75f, (nfloat)100f, (nfloat)214f, (nfloat)200f)
        {
        }

        protected override UIImage GenerateFullProgressImage()
        {
            UIGraphics.BeginImageContextWithOptions(this.Bounds.Size, false, UIScreen.MainScreen.Scale);
            using (CGContext currentContext = UIGraphics.GetCurrentContext())
            {
                using (UIBezierPath uiBezierPath = BezierPathGenerator.Bagel(this.CenterPoint, this.startRadius, this.endRadius, (nfloat)0.0f, (nfloat)6.283185f))
                {
                    currentContext.SaveState();
                    currentContext.SetFillColor(this.Color.CGColor);
                    currentContext.AddPath(uiBezierPath.CGPath);
                    currentContext.FillPath();
                    currentContext.RestoreState();
                    currentContext.SaveState();
                    currentContext.AddPath(uiBezierPath.CGPath);
                    currentContext.Clip();
                    this.DrawGradientOverlay(currentContext);
                    this.DrawInnerGlow(currentContext, this.CenterPoint, this.startRadius, this.endRadius, BigRadialProgressLayer.GlowColor, (nfloat)9f);
                    currentContext.RestoreState();
                    return UIGraphics.GetImageFromCurrentImageContext();
                }
            }
        }

        public override UIImage GenerateBackgroundImage()
        {
            UIGraphics.BeginImageContextWithOptions(this.BackBounds.Size, false, UIScreen.MainScreen.Scale);
            CGPoint center = new CGPoint(RectangleFExtensions.GetMidX(this.BackBounds), RectangleFExtensions.GetMidY(this.BackBounds));
            using (CGContext currentContext = UIGraphics.GetCurrentContext())
            {
                // ISSUE: reference to a compiler-generated method
                using (UIBezierPath uiBezierPath1 = UIBezierPath.FromOval(new CGRect(CGPoint.Empty, this.BackBounds.Size)))
                {
                    using (UIBezierPath uiBezierPath2 = BezierPathGenerator.Bagel(center, (nfloat)70f, (nfloat)105f, (nfloat)0.0f, (nfloat)6.283185f))
                    {
                        using (UIBezierPath uiBezierPath3 = BezierPathGenerator.Bagel(center, (nfloat)75f, (nfloat)100f, (nfloat)0.0f, (nfloat)6.283185f))
                        {
                            currentContext.SaveState();
                            currentContext.SetFillColor(RadialProgressLayer.BackCircleBackgroundColor);
                            // ISSUE: reference to a compiler-generated method
                            uiBezierPath1.Fill();
                            currentContext.RestoreState();
                            currentContext.SaveState();
                            currentContext.SetFillColor(RadialProgressLayer.BackBorderColor);
                            // ISSUE: reference to a compiler-generated method
                            uiBezierPath2.Fill();
                            currentContext.RestoreState();
                            currentContext.SaveState();
                            currentContext.SetFillColor(RadialProgressLayer.BackInnerBorderColor);
                            // ISSUE: reference to a compiler-generated method
                            uiBezierPath3.Fill();
                            currentContext.RestoreState();
                            return UIGraphics.GetImageFromCurrentImageContext();
                        }
                    }
                }
            }
        }

        private void DrawGradientOverlay(CGContext context)
        {
            CGColorSpace deviceRgb = CGColorSpace.CreateDeviceRGB();
            CGColor[] cgColorArray = new CGColor[2];
            int index1 = 0;
            CGColor cgColor1 = BigRadialProgressLayer.GradientOverlayStartColor;
            cgColorArray[index1] = cgColor1;
            int index2 = 1;
            CGColor cgColor2 = BigRadialProgressLayer.GradientOverlayEndColor;
            cgColorArray[index2] = cgColor2;
            CGColor[] colors = cgColorArray;
            nfloat[] nfloatArray = new nfloat[2];
            int index3 = 0;
            nfloatArray[index3] = (nfloat)0.0f;
            int index4 = 1;
            nfloatArray[index4] = (nfloat)1f;
            nfloat[] locations = nfloatArray;
            CGGradient gradient = new CGGradient(deviceRgb, colors, locations);
            context.SaveState();
            context.SetBlendMode(CGBlendMode.Overlay);
            context.DrawLinearGradient(gradient, new CGPoint(RectangleFExtensions.GetMinX(this.Bounds), RectangleFExtensions.GetMaxY(this.Bounds)), new CGPoint(RectangleFExtensions.GetMaxX(this.Bounds), RectangleFExtensions.GetMinY(this.Bounds)), (CGGradientDrawingOptions)0);
            context.RestoreState();
        }

        private void DrawInnerGlow(CGContext context, CGPoint center, nfloat startRadius, nfloat endRadius, CGColor glowColor, nfloat glowRadius)
        {
            using (UIBezierPath uiBezierPath1 = BezierPathGenerator.Bagel(center, startRadius - glowRadius - (nfloat)1f, startRadius - (nfloat)1f, (nfloat)0.0f, (nfloat)6.283185f))
            {
                using (UIBezierPath uiBezierPath2 = BezierPathGenerator.Bagel(center, endRadius + (nfloat)1f, endRadius + glowRadius + (nfloat)1f, (nfloat)0.0f, (nfloat)6.283185f))
                {
                    context.SaveState();
                    context.SetShadow(CGSize.Empty, glowRadius, glowColor);
                    context.SetFillColor(this.Color.CGColor);
                    context.AddPath(uiBezierPath1.CGPath);
                    context.FillPath();
                    context.RestoreState();
                    context.SaveState();
                    context.SetShadow(CGSize.Empty, glowRadius, glowColor);
                    context.SetFillColor(this.Color.CGColor);
                    context.SetFillColor(this.Color.CGColor);
                    context.AddPath(uiBezierPath2.CGPath);
                    context.FillPath();
                    context.RestoreState();
                }
            }
        }
    }
}
