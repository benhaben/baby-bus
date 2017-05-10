using System;
using UIKit;
using Foundation;
using CoreGraphics;
using RadialProgress;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhotoBrowser {
    public sealed class MWZoomingScrollView:UIScrollView,IUIScrollViewDelegate, IMWTapDetectingImageViewDelegate,IMWTapDetectingViewDelegate {
        public int Index {
            get;
            set;
        }

        public MWPhoto Photo {
            get;
            set;
        }

        public MWCaptionView CaptionView {
            get;
            set;
        }

        public UIButton SelectedButton {
            get;
            set;
        }

        //weak is better
        MWPhotoBrowser _photoBrowser;

        // for background taps
        MWTapDetectingView _tapView;

        MWTapDetectingImageView _photoImageView;

        RadialProgressView _loadingIndicator;

        UIImageView _loadingError;

        public MWZoomingScrollView(MWPhotoBrowser photoBrowser) {
            Index = int.MaxValue;
            _photoBrowser = photoBrowser;

            // Tap view for background
            _tapView = new MWTapDetectingView(this.Bounds);
            //TODO: change to C# event
            _tapView.TapDelegate = this;
            _tapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            _tapView.BackgroundColor = UIColor.Black;
            this.AddSubview(_tapView);

            // Image view
            _photoImageView = new MWTapDetectingImageView(CGRect.Empty);
            _photoImageView.TapDelegate = this;
            _photoImageView.ContentMode = UIViewContentMode.Center;
            _photoImageView.BackgroundColor = UIColor.Black;
            this.AddSubview(_photoImageView);

            // Loading indicator
            _loadingIndicator = new RadialProgressView();
            _loadingIndicator.Frame = new CGRect(140.0f, 30.0f, 40.0f, 40.0f);
            _loadingIndicator.UserInteractionEnabled = false;
            _loadingIndicator.AutoresizingMask = (UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleTopMargin |
            UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleRightMargin);
            this.AddSubview(_loadingIndicator);

            BackgroundColor = UIColor.Black;
            this.WeakDelegate = this; 
            ShowsHorizontalScrollIndicator = false;
            ShowsVerticalScrollIndicator = false;
            DecelerationRate = DecelerationRateFast;
            AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
        }

        List<NSObject> _eventListeners;

        void RegisterNotifications() {
            if (_eventListeners == null) {
                _eventListeners = new List<NSObject>();
            }
            // Listen for photo loading notifications
            _eventListeners.Add(NSNotificationCenter.DefaultCenter
                .AddObserver(MWPhoto.MWPHOTO_PROGRESS_NOTIFICATION, SetProgressFromNotification));
        }

        void UnRegisterNotifications() {
            if (_eventListeners != null) {
                NSNotificationCenter.DefaultCenter.RemoveObservers(_eventListeners);
                _eventListeners.Clear();
                _eventListeners = null;
            }
        }

        protected override void Dispose(bool disposing) {
            UnRegisterNotifications();
        }

        public void PrepareForReuse() {
            HideImageFailure();
            Photo = null;
            CaptionView = null;
            SelectedButton = null;
            _photoImageView.Image = null;
            Index = int.MaxValue;
        }

       

        #region Image

        public void SetPhoto(MWPhoto photo) {
            if (this.Photo != null) {
                Photo.CancelAnyLoading();
            }
            Photo = photo;
            UIImage img = _photoBrowser.ImageForPhoto(Photo);
            if (img != null) {
                this.DisplayImage();
            } else {
                this.ShowLoadingIndicator();
            }
        }
        // Get and display image
        public void DisplayImage() {
            if (Photo != null && _photoImageView.Image == null) {
                this.MaximumZoomScale = 1;
                this.MinimumZoomScale = 1;
                this.ZoomScale = 1;
                this.ContentSize = CGSize.Empty;
                // Get image from browser as it handles ordering of fetching
                var img = _photoBrowser.ImageForPhoto(Photo);
                if (img != null) {
                    this.HideLoadingIndicator();

                    // Set image
                    _photoImageView.Image = img;
                    _photoImageView.Hidden = false;

                    // Setup photo frame
                    CGRect photoImageViewFrame = new CGRect();
                    photoImageViewFrame.Location = CGPoint.Empty;
                    photoImageViewFrame.Size = img.Size;
                    _photoImageView.Frame = photoImageViewFrame;
                    this.ContentSize = photoImageViewFrame.Size;

                    // Set zoom to minimum zoom
                    SetMaxMinZoomScalesForCurrentBounds();
                } else {
                    // Failed no image
                    this.DisplayImageFailure();
                }
                this.SetNeedsLayout();
            } 
        }

        public void DisplayImageFailure() {
            this.HideLoadingIndicator();
            _photoImageView.Image = null;
            if (_loadingError == null) {
                _loadingError = new UIImageView();
                //TODO: check FromBundle path
                _loadingError.Image = UIImage.FromBundle("images/ImageError.png");
                _loadingError.UserInteractionEnabled = false;
                _loadingError.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin |
                UIViewAutoresizing.FlexibleTopMargin |
                UIViewAutoresizing.FlexibleBottomMargin |
                UIViewAutoresizing.FlexibleRightMargin;

                _loadingError.SizeToFit();
                this.AddSubview(_loadingError);
            }
            float x = Convert.ToSingle(Math.Floor((this.Bounds.Size.Width - _loadingError.Frame.Size.Width) / 2));
            float y = Convert.ToSingle(Math.Floor((this.Bounds.Size.Height - _loadingError.Frame.Size.Height) / 2));
            _loadingError.Frame = new CGRect(x, y, _loadingError.Frame.Size.Width, _loadingError.Frame.Size.Height);

        }

        void HideImageFailure() {
            if (_loadingError != null) {
                _loadingError.RemoveFromSuperview();
                _loadingError = null;
            }
        }

        #endregion

        #region  Loading Progress

        void SetProgressFromNotification(NSNotification notification) {
            NSDictionary dict = (NSDictionary)notification.Object;
            MWPhoto photoWithProgress = (MWPhoto)dict.ObjectForKey(new NSString("photo"));

            //TODO: check equal
            if (photoWithProgress == this.Photo) {
                var progress = Convert.ToSingle(dict.ValueForKey(new NSString("progress")) as NSNumber);
                this._loadingIndicator.Value = Math.Max(Math.Min(1, progress), 0);
            }
        }

        void HideLoadingIndicator() {
            if (_loadingIndicator != null) {
                _loadingIndicator.Hidden = true;
            } else {
                Debug.Assert(false);
            }
        }

        void ShowLoadingIndicator() {
            ZoomScale = 0;
            MinimumZoomScale = 0;
            MaximumZoomScale = 0;
            _loadingIndicator.Value = 0;
            _loadingIndicator.Hidden = false;
            HideLoadingIndicator();
        }

        #endregion

        #region Setup

        public  nfloat InitialZoomScaleWithMinScale() {
            var zoomScale = this.MinimumZoomScale;
            if (_photoImageView != null && _photoBrowser.ZoomPhotosToFill) {
                // Zoom image to fill if the aspect ratios are fairly similar
                CGSize boundsSize = this.Bounds.Size;
                CGSize imageSize = _photoImageView.Image.Size;
                var boundsAR = boundsSize.Width / boundsSize.Height;
                var imageAR = imageSize.Width / imageSize.Height;
                var xScale = boundsSize.Width / imageSize.Width;    // the scale needed to perfectly fit the image width-wise
                var yScale = boundsSize.Height / imageSize.Height;  // the scale needed to perfectly fit the image height-wise
                // Zooms standard portrait images on a 3.5in screen but not on a 4in screen.
                if (Math.Abs(boundsAR - imageAR) < 0.17) {
                    zoomScale = (nfloat)Math.Max(xScale, yScale);
                    // Ensure we don't zoom in or out too far, just in case
                    zoomScale = (nfloat)Math.Min(Math.Max(this.MinimumZoomScale, zoomScale), this.MaximumZoomScale);
                }
            }
            return zoomScale;
        }

        public void SetMaxMinZoomScalesForCurrentBounds() {
            // Reset
            this.MaximumZoomScale = 1;
            this.MinimumZoomScale = 1;
            this.ZoomScale = 1;

            // Bail if no image
            if (_photoImageView.Image == null) {
                return;
            }

            _photoImageView.Frame = new CGRect(0, 0, _photoImageView.Frame.Size.Width, _photoImageView.Frame.Size.Height);

            // Sizes
            CGSize boundsSize = this.Bounds.Size;
            CGSize imageSize = _photoImageView.Image.Size;


            // Calculate Min
            var xScale = boundsSize.Width / imageSize.Width;    // the scale needed to perfectly fit the image width-wise
            var yScale = boundsSize.Height / imageSize.Height;  // the scale needed to perfectly fit the image height-wise
            float minScale = (float)Math.Min(xScale, yScale);                 // use minimum of these to allow the image to become fully visible
        

            // Calculate Max
            float maxScale = 3f;

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
                // Let them go a bit bigger on a bigger screen!
                maxScale = 4f;
            }
            // Image is smaller than screen so no zooming!
            if (xScale >= 1 && yScale >= 1) {
                minScale = 1.0f;
            }

            // Set min/max zoom
            this.MaximumZoomScale = maxScale;
            this.MinimumZoomScale = minScale;

            // Initial zoom
            this.ZoomScale = this.InitialZoomScaleWithMinScale();

            // If we're zooming to fill then centralise
            if (this.ZoomScale != minScale) {
                // Centralise
                this.ContentOffset = new CGPoint((imageSize.Width * this.ZoomScale - boundsSize.Width) / 2.0f,
                    (imageSize.Height * this.ZoomScale - boundsSize.Height) / 2.0f);
                // Disable scrolling initially until the first pinch to fix issues with swiping on an initally zoomed in photo
                this.ScrollEnabled = false;
            }

            // Layout
            this.SetNeedsLayout();
        }

        #endregion

        #region layout

        public override void LayoutSubviews() {
            base.LayoutSubviews();

            // Update tap view frame
            _tapView.Frame = this.Bounds;

            // Position indicators (centre does not seem to work!)
            if (!_loadingIndicator.Hidden) {
                _loadingIndicator.Frame = new CGRect(((this.Bounds.Size.Width - _loadingIndicator.Frame.Size.Width) / 2f),
                    ((this.Bounds.Size.Height - _loadingIndicator.Frame.Size.Height) / 2f),
                    _loadingIndicator.Frame.Size.Width,
                    _loadingIndicator.Frame.Size.Height);
            }
            if (_loadingError != null) {
                _loadingError.Frame = new CGRect(((this.Bounds.Size.Width - _loadingError.Frame.Size.Width) / 2f),
                    ((this.Bounds.Size.Height - _loadingError.Frame.Size.Height) / 2f),
                    _loadingError.Frame.Size.Width,
                    _loadingError.Frame.Size.Height);
            }

            // Center the image as it becomes smaller than the size of the screen
            CGSize boundsSize = this.Bounds.Size;
            CGRect frameToCenter = _photoImageView.Frame;
            // Horizontally
            if (frameToCenter.Size.Width < boundsSize.Width) {
                frameToCenter.X = (nfloat)((boundsSize.Width - frameToCenter.Size.Width) / 2.0f);
            } else {
                frameToCenter.X = 0;
            }


            // Vertically
            if (frameToCenter.Size.Height < boundsSize.Height) {
                frameToCenter.Y = ((boundsSize.Height - frameToCenter.Size.Height) / 2.0f);
            } else {
                frameToCenter.Y = 0;
            }

            // Center
            if (_photoImageView.Frame.X != frameToCenter.X
                || _photoImageView.Frame.Y != frameToCenter.Y
                || _photoImageView.Frame.Width != frameToCenter.Width
                || _photoImageView.Frame.Height != frameToCenter.Height) {
                _photoImageView.Frame = frameToCenter;
            }

        }

        #endregion

     
        #region UIScrollViewDelegate

        public UIView ViewForZoomingInScrollView(UIScrollView scrollView) {
            return _photoImageView;
        }

        public void DraggingStarted(UIScrollView scrollView) {
            _photoBrowser.CancelControlHiding();
        }

        //[Export("scrollViewWillBeginZooming:withView:"), Availability(Introduced = Platform.iOS_3_2), CompilerGenerated]
        public  void ZoomingStarted(UIScrollView scrollView, UIView view) {
            this.ScrollEnabled = true;
            _photoBrowser.CancelControlHiding();
        }


        //        [Export("scrollViewDidEndDragging:willDecelerate:"), CompilerGenerated]
        public  void DraggingEnded(UIScrollView scrollView, bool willDecelerate) {
            _photoBrowser.HideControlsAfterDelay();
        }

        //        [Export("scrollViewDidZoom:"), Availability(Introduced = Platform.iOS_3_2), CompilerGenerated]
        public  void DidZoom(UIScrollView scrollView) {
            this.SetNeedsLayout();
            this.LayoutIfNeeded();
        }

        #endregion

        #region Tap Detection

        void HandleSingleTap(CGPoint touchPoint) {
//            NSTimer.CreateScheduledTimer(TimeSpan.FromSeconds(0.2), () => _photoBrowser.ToggleControls());

            _photoBrowser.PerformSelector(new ObjCRuntime.Selector("toggleControls"), null, 0.2);
//            [_photoBrowser performSelector:@selector(toggleControls) withObject:nil afterDelay:0.2];
        }

        void HandleDoubleTap(CGPoint touchPoint) {

            // Cancel any single tap handling
//            [NSObject cancelPreviousPerformRequestsWithTarget:_photoBrowser];
            NSObject.CancelPreviousPerformRequest(_photoBrowser);

            // Zoom
            float EPSILON = 0.000001f;
            if (Math.Abs((float)this.ZoomScale - (float)this.MinimumZoomScale) > EPSILON && Math.Abs((float)this.ZoomScale - (float)this.InitialZoomScaleWithMinScale()) > EPSILON) {

                // Zoom out
                this.SetZoomScale(this.MinimumZoomScale, true);
            } else {
                // Zoom in to twice the size
                var newZoomScale = ((this.MaximumZoomScale + this.MinimumZoomScale) / 2f);
                var xsize = this.Bounds.Size.Width / newZoomScale;
                var ysize = this.Bounds.Size.Height / newZoomScale;
                this.ZoomToRect(new CGRect(touchPoint.X - xsize / 2f, touchPoint.Y - ysize / 2f, xsize, ysize), true);
            }

            // Delay controls
            _photoBrowser.HideControlsAfterDelay();
        }

        #region IMWTapDetectingImageViewDelegate implementation

        void IMWTapDetectingImageViewDelegate.SingleTapDetected(UIImageView imageView, UITouch touch) {
//            [self handleSingleTap:[touch locationInView:imageView]];
            this.HandleSingleTap(touch.LocationInView(imageView));
        }

        void IMWTapDetectingImageViewDelegate.DoubleTapDetected(UIImageView imageView, UITouch touch) {
            this.HandleDoubleTap(touch.LocationInView(imageView));
        }

        void IMWTapDetectingImageViewDelegate.TripleTapDetected(UIImageView imageView, UITouch touch) {
        }

        #endregion

        #region IMWTapDetectingViewDelegate implementation

        void IMWTapDetectingViewDelegate.SingleTapDetected(UIView imageView, UITouch touch) {
            // Translate touch location to image view location
            var touchX = touch.LocationInView(imageView).X;
            var touchY = touch.LocationInView(imageView).Y;
            touchX *= 1 / this.ZoomScale;
            touchY *= 1 / this.ZoomScale;
            touchX += this.ContentOffset.X;
            touchY += this.ContentOffset.Y;
            this.HandleSingleTap(new CGPoint(touchX, touchY));
        }

        void IMWTapDetectingViewDelegate.DoubleTapDetected(UIView imageView, UITouch touch) {
            // Translate touch location to image view location
            var touchX = touch.LocationInView(imageView).X;
            var touchY = touch.LocationInView(imageView).Y;
            touchX *= 1 / this.ZoomScale;
            touchY *= 1 / this.ZoomScale;
            touchX += this.ContentOffset.X;
            touchY += this.ContentOffset.Y;
            this.HandleDoubleTap(new CGPoint(touchX, touchY));
        }

        void IMWTapDetectingViewDelegate.TripleTapDetected(UIView imageView, UITouch touch) {
        }

        #endregion

        #endregion

    }
}

