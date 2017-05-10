using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace PhotoBrowser {

    public interface IMWTapDetectingImageViewDelegate {

        //        - (void)imageView:(UIImageView *)imageView singleTapDetected:(UITouch *)touch;
        //        - (void)imageView:(UIImageView *)imageView doubleTapDetected:(UITouch *)touch;
        //        - (void)imageView:(UIImageView *)imageView tripleTapDetected:(UITouch *)touch;

        void SingleTapDetected(UIImageView imageView, UITouch touch);

        void DoubleTapDetected(UIImageView imageView, UITouch touch);

        void TripleTapDetected(UIImageView imageView, UITouch touch);

    }

    public sealed class MWTapDetectingImageView :UIImageView {

        IMWTapDetectingImageViewDelegate _tapDelegate;

        public IMWTapDetectingImageViewDelegate TapDelegate {
            get {
                return _tapDelegate;
            }
            set {
                _tapDelegate = value;
            }
        }

        public MWTapDetectingImageView(CGRect frame)
            : base(frame) {
            this.UserInteractionEnabled = true;
        }

        public MWTapDetectingImageView(UIImage image, UIImage highlightedImage)
            : base(image, highlightedImage) {
            this.UserInteractionEnabled = true;
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt) {
            base.TouchesEnded(touches, evt);
            UITouch touch = (UITouch)touches.AnyObject;
            int tapCount = -1;
            if (touch != null) {
                tapCount = (int)touch.TapCount;
            } else {
                return;
            }

            if (tapCount == 1) {
                HandleSingleTap(touch);
            } else if (tapCount == 2) {
                HandleDoubleTap(touch);
            } else if (tapCount == 3) {
                HandleTripleTap(touch);
            } else {
            }
            this.NextResponder.TouchesEnded(touches, evt);
        }

        void HandleSingleTap(UITouch touch) {
            if (_tapDelegate != null) {
                _tapDelegate.SingleTapDetected(this, touch);
            }
        }

        void HandleDoubleTap(UITouch touch) {
            if (_tapDelegate != null) {
                _tapDelegate.DoubleTapDetected(this, touch);
            }
        }

        void HandleTripleTap(UITouch touch) {
            if (_tapDelegate != null) {
                _tapDelegate.TripleTapDetected(this, touch);
            }
        }
    }
}

