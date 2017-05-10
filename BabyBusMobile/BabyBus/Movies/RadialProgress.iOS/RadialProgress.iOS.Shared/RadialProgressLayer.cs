// Decompiled with JetBrains decompiler
// Type: RadialProgress.RadialProgressLayer
// Assembly: RadialProgress.iOS, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 4ACC5FD7-E0EF-4B72-8CE9-961D44E56BA5
// Assembly location: C:\Users\jkm\Desktop\RadialProgress.iOS.dll

using CoreAnimation;
using CoreGraphics;
using System;
using UIKit;

namespace RadialProgress
{
  internal abstract class RadialProgressLayer : CALayer
  {
    private static readonly UIColor DefaultFillColor = UIColor.FromRGB(114, 184, (int) byte.MaxValue);
    protected static readonly CGColor BackBorderColor = UIColor.Black.ColorWithAlpha((nfloat) 0.41f).CGColor;
    protected static readonly CGColor BackInnerBorderColor = UIColor.White.ColorWithAlpha((nfloat) 0.2f).CGColor;
    protected static readonly CGColor BackCircleBackgroundColor = UIColor.Black.ColorWithAlpha((nfloat) 0.5f).CGColor;
    protected const float FullCircleAngle = 6.283185f;
    protected CGPoint CenterPoint;
    private UIImage fullProgressImage;
    protected nfloat endRadius;
    protected nfloat startRadius;
    protected nfloat backgroundWidth;
    protected nfloat progressLayerWidth;
    private UIColor color;

    protected CGSize BoundsSize { get; set; }

    public CGRect BackBounds { get; set; }

    public nfloat Percentage { get; set; }

    public UIColor Color
    {
      get
      {
        return this.color ?? RadialProgressLayer.DefaultFillColor;
      }
      set
      {
        if (this.color == value)
          return;
        this.color = value;
        this.fullProgressImage = this.GenerateFullProgressImage();
      }
    }

    public RadialProgressLayer(nfloat startRadius, nfloat endRadius, nfloat backgroundWidth, nfloat progressLayerWidth)
      : this()
    {
      this.startRadius = startRadius;
      this.endRadius = endRadius;
      this.backgroundWidth = backgroundWidth;
      this.progressLayerWidth = progressLayerWidth;
      this.Bounds = new CGRect(CGPoint.Empty, new CGSize(progressLayerWidth, progressLayerWidth));
      this.BackBounds = new CGRect(CGPoint.Empty, new CGSize(backgroundWidth, backgroundWidth));
      this.CenterPoint = new CGPoint(RectangleFExtensions.GetMidX(this.Bounds), RectangleFExtensions.GetMidY(this.Bounds));
      this.fullProgressImage = this.GenerateFullProgressImage();
    }

    public RadialProgressLayer()
    {
      this.BackgroundColor = UIColor.Clear.CGColor;
      this.ContentsScale = UIScreen.MainScreen.Scale;
    }

    public override void DrawInContext(CGContext context)
    {
      using (UIBezierPath uiBezierPath = BezierPathGenerator.Bagel(this.CenterPoint, this.startRadius, this.endRadius, (nfloat) 0.0f, this.CalculateProgressAngle(this.Percentage)))
      {
        context.AddPath(uiBezierPath.CGPath);
        context.Clip();
        context.DrawImage(this.Bounds, this.fullProgressImage.CGImage);
      }
    }

    protected virtual UIImage GenerateFullProgressImage()
    {
      UIGraphics.BeginImageContextWithOptions(this.Bounds.Size, false, UIScreen.MainScreen.Scale);
      using (CGContext currentContext = UIGraphics.GetCurrentContext())
      {
        using (UIBezierPath uiBezierPath = BezierPathGenerator.Bagel(this.CenterPoint, this.startRadius, this.endRadius, (nfloat) 0.0f, (nfloat) 6.283185f))
        {
          currentContext.SaveState();
          currentContext.SetFillColor(this.Color.CGColor);
          currentContext.AddPath(uiBezierPath.CGPath);
          currentContext.FillPath();
          currentContext.RestoreState();
          return UIGraphics.GetImageFromCurrentImageContext();
        }
      }
    }

    private nfloat CalculateProgressAngle(nfloat percentage)
    {
      return (nfloat) Math.PI / (nfloat) 50f * percentage;
    }

    public abstract UIImage GenerateBackgroundImage();
  }
}
