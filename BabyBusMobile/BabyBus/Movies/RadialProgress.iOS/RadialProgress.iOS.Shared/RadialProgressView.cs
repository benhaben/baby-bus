// Decompiled with JetBrains decompiler
// Type: RadialProgress.RadialProgressView
// Assembly: RadialProgress.iOS, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 4ACC5FD7-E0EF-4B72-8CE9-961D44E56BA5
// Assembly location: C:\Users\jkm\Desktop\RadialProgress.iOS.dll

using CoreAnimation;
using CoreGraphics;
using System;
using UIKit;

namespace RadialProgress
{
  public class RadialProgressView : UIView
  {
    private nfloat currentValue = (nfloat) 0.0f;
    private const float DefaultMinValue = 0.0f;
    private const float DefaultMaxValue = 1f;
    private CGPoint CenterPoint;
    private UILabel percentageLabel;
    private UIImageView backgroundImageView;
    private RadialProgressLayer radialProgressLayer;
    private RadialProgressViewStyle progressType;
    private UIColor progressColor;
    private bool labelHidden;

    public nfloat MinValue { get; set; }

    public nfloat MaxValue { get; set; }

    public UIColor ProgressColor
    {
      get
      {
        return this.progressColor;
      }
      set
      {
        this.progressColor = value;
        this.radialProgressLayer.Color = this.ProgressColor;
      }
    }

    public Func<nfloat, string> LabelTextDelegate { get; set; }

    public bool LabelHidden
    {
      get
      {
        if (this.progressType != RadialProgressViewStyle.Tiny)
          return this.labelHidden;
        return true;
      }
      set
      {
        if (this.labelHidden == value)
          return;
        this.labelHidden = value;
        if (!this.labelHidden && this.percentageLabel == null)
          this.InitPercentageLabel();
        if (this.percentageLabel == null)
          return;
        this.percentageLabel.Hidden = value;
      }
    }

    public nfloat Value
    {
      get
      {
        return this.currentValue;
      }
      set
      {
        if (value > this.MaxValue)
          value = this.MaxValue;
        if (value < this.MinValue)
          value = this.MinValue;
        if (!(this.currentValue != value))
          return;
        this.currentValue = value;
        this.InvokeOnMainThread(new Action(this.OnCurrentValueChanged));
      }
    }

    public bool IsDone
    {
      get
      {
        return this.Value == this.MaxValue;
      }
    }

    public RadialProgressView(Func<nfloat, string> labelText = null, RadialProgressViewStyle progressType = RadialProgressViewStyle.Big)
    {
      this.progressType = progressType;
      this.LabelTextDelegate = labelText;
      this.radialProgressLayer = this.GetRadialLayerByType(progressType);
      this.Bounds = this.radialProgressLayer.BackBounds;
      this.CenterPoint = new CGPoint(RectangleFExtensions.GetMidX(this.Bounds), RectangleFExtensions.GetMidY(this.Bounds));
      this.BackgroundColor = UIColor.Clear;
      this.UserInteractionEnabled = false;
      this.MinValue = (nfloat) 0.0f;
      this.MaxValue = (nfloat) 1f;
      this.InitSubviews();
    }

    public void Reset()
    {
      this.currentValue = this.MinValue;
      this.OnCurrentValueChanged();
    }

    private UIFont FontForProgressType(RadialProgressViewStyle progressType)
    {
      if (progressType == RadialProgressViewStyle.Big)
      {
        // ISSUE: reference to a compiler-generated method
        return UIFont.FromName("Helvetica-Bold", (nfloat) 76f);
      }
      if (progressType == RadialProgressViewStyle.Small)
      {
        // ISSUE: reference to a compiler-generated method
        return UIFont.FromName("Helvetica-Bold", (nfloat) 12f);
      }
      // ISSUE: reference to a compiler-generated method
      return UIFont.FromName("Helvetica-Bold", (nfloat) 76f);
    }

    private void InitSubviews()
    {
      this.backgroundImageView = new UIImageView(this.radialProgressLayer.GenerateBackgroundImage());
      // ISSUE: reference to a compiler-generated method
      this.AddSubview((UIView) this.backgroundImageView);
      if (!this.LabelHidden)
        this.InitPercentageLabel();
      // ISSUE: reference to a compiler-generated method
      this.Layer.InsertSublayerAbove((CALayer) this.radialProgressLayer, this.backgroundImageView.Layer);
    }

    private RadialProgressLayer GetRadialLayerByType(RadialProgressViewStyle progressType)
    {
      switch (progressType)
      {
        case RadialProgressViewStyle.Big:
          return (RadialProgressLayer) new BigRadialProgressLayer();
        case RadialProgressViewStyle.Small:
          return (RadialProgressLayer) new SmallRadialProgressLayer();
        case RadialProgressViewStyle.Tiny:
          return (RadialProgressLayer) new TinyRadialProgressLayer();
        default:
          return (RadialProgressLayer) new BigRadialProgressLayer();
      }
    }

    private void InitPercentageLabel()
    {
      if (this.percentageLabel != null)
        return;
      this.percentageLabel = new UILabel();
      this.percentageLabel.BackgroundColor = UIColor.Clear;
      this.percentageLabel.TextColor = UIColor.White;
      // ISSUE: reference to a compiler-generated method
      this.percentageLabel.ShadowColor = UIColor.Black.ColorWithAlpha((nfloat) 0.71f);
      this.percentageLabel.ShadowOffset = new CGSize((nfloat) 1f, (nfloat) 1f);
      this.percentageLabel.Font = this.FontForProgressType(this.progressType);
      this.percentageLabel.TextAlignment = UITextAlignment.Center;
      this.percentageLabel.Text = "100";
      // ISSUE: reference to a compiler-generated method
      this.percentageLabel.SizeToFit();
      this.percentageLabel.Text = string.Empty;
      // ISSUE: reference to a compiler-generated method
      this.AddSubview((UIView) this.percentageLabel);
    }

    private void OnCurrentValueChanged()
    {
      nfloat nfloat = this.CalculatePercentage(this.currentValue);
      this.radialProgressLayer.Color = this.ProgressColor;
      this.radialProgressLayer.Percentage = nfloat;
      // ISSUE: reference to a compiler-generated method
      this.radialProgressLayer.SetNeedsDisplay();
      if (this.LabelHidden)
        return;
      this.percentageLabel.Text = this.LabelTextDelegate != null ? this.LabelTextDelegate(this.currentValue) : Math.Floor((double) nfloat).ToString().PadLeft(2, '0');
    }

    public override void LayoutSubviews()
    {
      if (this.progressType != RadialProgressViewStyle.Tiny)
        this.percentageLabel.Center = this.CenterPoint;
      this.backgroundImageView.Center = this.CenterPoint;
      this.radialProgressLayer.Position = this.CenterPoint;
    }

    private nfloat CalculatePercentage(nfloat currentValue)
    {
      nfloat nfloat1 = this.MaxValue - this.MinValue;
      nfloat nfloat2 = this.MaxValue - currentValue;
      if (nfloat1 == (nfloat) 0.0f)
        return (nfloat) 0.0f;
      return ((nfloat) 1 - nfloat2 / nfloat1) * (nfloat) 100f;
    }
  }
}
