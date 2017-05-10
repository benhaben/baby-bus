using System;
using UIKit;
using Foundation;
using CoreGraphics;


namespace PhotoBrowser {

    public interface IMWTapDetectingViewDelegate {

        void SingleTapDetected(UIView imageView, UITouch touch);

        void DoubleTapDetected(UIView imageView, UITouch touch);

        void TripleTapDetected(UIView imageView, UITouch touch);

    }


    public class MWTapDetectingView : UIView {
        IMWTapDetectingViewDelegate _tapDelegate;

        public IMWTapDetectingViewDelegate TapDelegate {
            get {
                return _tapDelegate;
            }
            set {
                _tapDelegate = value;
            }
        }

        public MWTapDetectingView() {
            this.UserInteractionEnabled = true;
        }

        public MWTapDetectingView(CGRect frame)
            : base(frame) {
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

