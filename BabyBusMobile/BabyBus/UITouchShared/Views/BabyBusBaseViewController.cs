using System;
using UIKit;
using Foundation;

using System.Collections.Generic;
using CoreGraphics;

using ImageEffects;

using BigTed;
using SDWebImage;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    //do not use mvx
    public abstract class BabyBusBaseViewController : UIViewController
    {

        private Dictionary<UITextField, BubbleView> bubbleViews = new Dictionary<UITextField, BubbleView>();
        // weak subscription to NotifyProperty event
        IDisposable npSubscription;

        public UIImageView BackgroundImage{ get; set; }

        protected void ShowBubbleFor(UITextField field, string text)
        {
            if (!this.bubbleViews.ContainsKey(field))
            {
                // ISSUE: method pointer
                field.ShouldBeginEditing = (new UITextFieldCondition(HideBubbleFor));
                this.bubbleViews.Add(field, (BubbleView)null);
            }
            if (this.bubbleViews[field] == null)
            {
                BubbleView bubbleView1 = new BubbleView();
                BubbleView bubbleView2 = bubbleView1;
                CGRect frame1 = ((UIView)field).Frame;
                // ISSUE: explicit reference operation
                double num1 = (double)((CGRect)frame1).Left + 5.0;
                CGRect frame2 = ((UIView)field).Frame;
                // ISSUE: explicit reference operation
                double num2 = (double)((CGRect)frame2).Bottom - 14.0;
                CGRect frame3 = ((UIView)field).Frame;
                // ISSUE: explicit reference operation
                double num3 = (double)((CGRect)frame3).Width;
                double num4 = 44.0;
                CGRect rectangleF = new CGRect(
                                        (float)num1,
                                        (float)num2,
                                        (float)num3,
                                        (float)num4);
                bubbleView2.Frame = (rectangleF);
                bubbleView1.Text = text;
                BubbleView bubbleView3 = bubbleView1;
                this.bubbleViews[field] = bubbleView3;
                this.Add((UIView)bubbleView3);
            }
            else
                this.bubbleViews[field].Text = text;
        }

        public bool HideBubbleFor(UITextField textFild)
        {
            if (bubbleViews != null && bubbleViews.ContainsKey(textFild))
            {
                BubbleView bubbleView = this.bubbleViews[textFild];
                if (bubbleView != null)
                {
                    bubbleView.RemoveFromSuperview();
                    ((NSObject)bubbleView).Dispose();
                }
                this.bubbleViews[textFild] = (BubbleView)null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public BabyBusBaseViewController()
            : base()
        {
        }

        public BabyBusBaseViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected BabyBusBaseViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        UIView _container = new UIView();
        UIScrollView _scrollView = new UIScrollView();

        protected UIView Container { get { return _container; } set { _container = value; } }

        protected UIScrollView ScrollView{ get { return _scrollView; } set { _scrollView = value; } }

        protected virtual void SetBackgroundImage()
        {
            this.View.BackgroundColor = UIColor.White;

            //            BackgroundImage = new UIImageView ();
            //            BackgroundImage.Image = UIImage.FromBundle ("sea.jpg");
            //            BackgroundImage.Frame = this.View.Bounds;
            //            BackgroundImage.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            //            this.View.InsertSubview (BackgroundImage, 0);
        }

        void HideKeyBoardWhenTap()
        {
            //TODO: test tap in the uitextview
            var g = new UITapGestureRecognizer(() =>
                {
                    var firstResponder = View.FindFirstResponder();
                    if (firstResponder != null)
                    {
                        firstResponder.ResignFirstResponder();
                    }
                });
            View.AddGestureRecognizer(g);
        }

        NSObject _keyboardObserverWillShow;
        NSObject _keyboardObserverWillHide;

        public virtual void SetUpConstrainLayout()
        {
            View.ConstrainLayout(
                () => 
                ScrollView.Frame.Top == View.Frame.Top
                && ScrollView.Frame.Left == View.Frame.Left
                && ScrollView.Frame.Right == View.Frame.Right
                && ScrollView.Frame.Bottom == View.Frame.Bottom

                && Container.Frame.Top == ScrollView.Frame.Top
                && Container.Frame.Bottom == ScrollView.Frame.Bottom

                //Note: let rotate work
                && Container.Frame.Left == View.Frame.Left
                && Container.Frame.Right == View.Frame.Right
            );
        }

        public virtual void PrepareViewHierarchy()
        {
            ScrollView.AddSubview(_container);
            View.AddSubview(_scrollView);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            BTProgressHUD.Dismiss();
            //            this.View.TranslatesAutoresizingMaskIntoConstraints = false;
            HideKeyBoardWhenTap();

            // Setup keyboard event handlers
            RegisterForKeyboardNotifications();

            //please do not change the order
            PrepareViewHierarchy();
            SetUpConstrainLayout();
            SetBackgroundImage();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            UnregisterKeyboardNotifications();
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
            UnregisterKeyboardNotifications();
            SDWebImageManager.SharedManager.ImageCache.CleanDisk();
        }

        protected virtual void RegisterForKeyboardNotifications()
        {
            _keyboardObserverWillShow = NSNotificationCenter.DefaultCenter.AddObserver(
                UIKeyboard.WillShowNotification,
                KeyboardWillShowNotification);
            _keyboardObserverWillHide = NSNotificationCenter.DefaultCenter.AddObserver(
                UIKeyboard.WillHideNotification,
                KeyboardWillHideNotification);
        }

        protected virtual void UnregisterKeyboardNotifications()
        {
            if (_keyboardObserverWillShow != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillShow);
            if (_keyboardObserverWillHide != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillHide);
        }

        protected virtual UIView KeyboardGetActiveView()
        {
            return this.View.FindFirstResponder();
        }

        protected virtual void KeyboardWillShowNotification(NSNotification notification)
        {
            UIView activeView = KeyboardGetActiveView();
            if (activeView == null)
                return;

            UIScrollView scrollView = activeView.FindSuperviewOfType(
                                          this.View,
                                          typeof(UIScrollView)) as UIScrollView;
            if (scrollView == null)
                return;

            CGRect keyboardBounds = UIKeyboard.BoundsFromNotification(notification);

            UIEdgeInsets contentInsets = new UIEdgeInsets(
                                             0.0f,
                                             0.0f,
                                             keyboardBounds.Size.Height,
                                             0.0f);

            // If activeField is hidden by keyboard, scroll it so it's visible
            CGRect viewRectAboveKeyboard = new CGRect(
                                               this.View.Frame.Location,
                                               new CGSize(
                                                   this.View.Frame.Width,
                                                   this.View.Frame.Size.Height - keyboardBounds.Size.Height));

            CGRect activeFieldAbsoluteFrame = activeView.Superview.ConvertRectToView(
                                                  activeView.Frame,
                                                  this.View);

            //let scroll more easier, 64 is this.View.Frame.Location.Y, don't know why have this offset
            activeFieldAbsoluteFrame.Y += 64;
            // activeFieldAbsoluteFrame is relative to this.View so does not include any scrollView.ContentOffset

            // Check if the activeField will be partially or entirely covered by the keyboard 

            if ((viewRectAboveKeyboard.IntersectsWith(activeFieldAbsoluteFrame)
                && !viewRectAboveKeyboard.Contains(activeFieldAbsoluteFrame)
                ) ||
                (!viewRectAboveKeyboard.Contains(activeFieldAbsoluteFrame)))
            {

                scrollView.ContentInset = contentInsets;
                scrollView.ScrollIndicatorInsets = contentInsets;

                // Scroll to the activeField Y position + activeField.Height + current scrollView.ContentOffset.Y - the keyboard Height 
                CGPoint scrollPoint = new CGPoint(0.0f, activeFieldAbsoluteFrame.Location.Y
                                          + activeFieldAbsoluteFrame.Height
                                          + scrollView.ContentOffset.Y
                                          - viewRectAboveKeyboard.Height); 
                scrollView.SetContentOffset(scrollPoint, true); 
                _hasScrollAndSetContentInset = true;
            }
        }

        bool _hasScrollAndSetContentInset = false;

        protected virtual void KeyboardWillHideNotification(NSNotification notification)
        {
            UIView activeView = KeyboardGetActiveView();
            if (activeView == null)
                return;

            UIScrollView scrollView = activeView.FindSuperviewOfType(
                                          this.View,
                                          typeof(UIScrollView)) as UIScrollView;
            if (scrollView == null)
                return;

            // Reset the content inset of the scrollView and animate using the current keyboard animation duration
            double animationDuration = UIKeyboard.AnimationDurationFromNotification(notification);
            UIEdgeInsets contentInsets = new UIEdgeInsets(
                                             0.0f,
                                             0.0f,
                                             0.0f,
                                             0.0f);
            if (_hasScrollAndSetContentInset)
            {
                UIView.Animate(animationDuration, delegate
                    {
                        scrollView.ContentInset = contentInsets;
                        scrollView.ScrollIndicatorInsets = contentInsets;
                    });
                _hasScrollAndSetContentInset = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.npSubscription != null)
            {
                this.npSubscription.Dispose();
                this.npSubscription = null;
            }
            base.Dispose(disposing);
            UnregisterKeyboardNotifications();
        }


        public  static void ApplyBlurBackgroundToTextField(
            UITextField textField,
            UIView backgroundView)
        {
            // This makes sure we have the coordinates relative to the backgroundView. Without this, the image drawn
            // for the button would be at the incorrect place of the background. 
            CGRect buttonRectInBGViewCoords = textField.ConvertRectToView(
                                                  textField.Bounds,
                                                  backgroundView);
            UIGraphics.BeginImageContextWithOptions(
                textField.Frame.Size,
                false,
                1);

            // Make a new image of the backgroundView (basically a screenshot of the view)
            backgroundView.DrawViewHierarchy(
                new CGRect(-buttonRectInBGViewCoords.X, -buttonRectInBGViewCoords.Y,
                    backgroundView.Frame.Width, backgroundView.Frame.Height),
                true);
            UIImage newBGImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            // Apply the blur effect
            newBGImage = newBGImage.ApplyLightEffect();

            // Set the blurred image as the background for the button
            textField.Background = newBGImage;
            textField.Layer.CornerRadius = UIConstants.CornerRadius;
            ;
            textField.Layer.MasksToBounds = true;
        }

        public  static void ApplyBlurBackgroundToButton(
            UIButton button,
            UIView backgroundView)
        {
            // This makes sure we have the coordinates relative to the backgroundView. Without this, the image drawn
            // for the button would be at the incorrect place of the background. 
            CGRect buttonRectInBGViewCoords = button.ConvertRectToView(
                                                  button.Bounds,
                                                  backgroundView);
            UIGraphics.BeginImageContextWithOptions(
                button.Frame.Size,
                false,
                1);


            // Make a new image of the backgroundView (basically a screenshot of the view)
            backgroundView.DrawViewHierarchy(
                new CGRect(-buttonRectInBGViewCoords.X, -buttonRectInBGViewCoords.Y,
                    backgroundView.Frame.Width, backgroundView.Frame.Height),
                true);
            UIImage newBGImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            // Apply the blur effect
            //          UIImage ApplyBlurWithRadius (float blurRadius, UIColor tintColor, float saturationDeltaFactor, [NullAllowed]UIImage maskImage);
            //          newBGImage = newBGImage.ApplyBlurWithRadius (0.8, UIColor.Blue, 1, newBGImage);
            //          newBGImage = newBGImage.ApplyTintEffectWithColor (UIColor.Red);
            newBGImage = newBGImage.ApplyLightEffect();
            // Set the blurred image as the background for the button
            button.SetBackgroundImage(newBGImage, UIControlState.Normal);
            button.Layer.CornerRadius = UIConstants.CornerRadius;
            ;
            button.Layer.MasksToBounds = true;
        }
    }
}

