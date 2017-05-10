using System;
using UIKit;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System.Collections.Generic;
using MWPhotoBrowserBinding;

namespace BabyBus.iOS
{
    public sealed class MWImageCollecntionViewCell : UICollectionViewCell
    {
        public static  NSString MWPHOTO_LOADING_DID_END_NOTIFICATION = new NSString("MWPHOTO_LOADING_DID_END_NOTIFICATION");
        public  static NSString MWPHOTO_PROGRESS_NOTIFICATION = new NSString("MWPHOTO_PROGRESS_NOTIFICATION");

        public static NSString CellID = new NSString("ImageCell");

        public int Index
        {
            get;
            set;
        }

        public MWPhoto Photo
        {
            get;
            set;
        }

        UIImageView ImageView{ get; set; }

        UIImageView LoadingError{ get; set; }

        RadialProgressView LoadingIndicator{ get; set; }

        bool IsShowLoadingIndicator { get; set; }

        [Export("initWithFrame:")]
        public MWImageCollecntionViewCell(CGRect frame)
            : base(frame)
        {
            this.BackgroundColor = UIColor.FromWhiteAlpha((nfloat)0.12, 1);
            ImageView = new UIImageView(this.Bounds);
            ImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            ImageView.ClipsToBounds = true;
            ImageView.AutosizesSubviews = true;
            this.AddSubview(ImageView);

            // Loading indicator
            LoadingIndicator = new RadialProgressView(null, RadialProgressViewStyle.Tiny);
            LoadingIndicator.Frame = new CGRect(0, 0, 10, 10);
            LoadingIndicator.UserInteractionEnabled = false;
            this.AddSubview(LoadingIndicator);

            RegisterNotifications();

            this.BackgroundColor = UIColor.Clear;
        }

        List<NSObject> _eventListeners;

        void RegisterNotifications()
        {
            if (_eventListeners == null)
            {
                _eventListeners = new List<NSObject>();
            }
            // Listen for photo loading notifications
            _eventListeners.Add(
                MWPhoto.Notifications.ObserveMWPhotoEnd(HandleMWPhotoLoadingDidEndNotification)
            );
            _eventListeners.Add(
                MWPhoto.Notifications.ObserveMWPhotoProgress(SetProgressFromNotification)
            );
        }

        void UnRegisterNotifications()
        {
            if (_eventListeners != null)
            {
                foreach (var objNS in _eventListeners)
                {
//                    stop receiving notifications
                    objNS.Dispose();
                }
                _eventListeners.Clear();
                _eventListeners = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            UnRegisterNotifications();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.ImageView.Frame = this.Bounds;
            var x = Convert.ToSingle(Math.Floor((this.Bounds.Size.Width - LoadingIndicator.Frame.Size.Width) / 2));
            var y = Convert.ToSingle(Math.Floor((this.Bounds.Size.Height - LoadingIndicator.Frame.Size.Height) / 2));
            this.LoadingIndicator.Frame = new CGRect(x, y, LoadingIndicator.Frame.Size.Width, LoadingIndicator.Frame.Size.Height);
        }

        public override void PrepareForReuse()
        {
            this.Photo = null;
            this.ImageView.Image = null;
            LoadingIndicator.Value = 0;
            this.HideImageFailure();
            base.PrepareForReuse();
        }

        #region Image Handling

        public void SetPhoto(MWPhoto photo)
        {

            this.Photo = photo;
            if (Photo != null)
            {
                if (Photo.UnderlyingImage == null)
                {
                    this.ShowLoadingIndicator();
                }
                else
                {
                    this.HideLoadingIndicator();
                    //等到收通知会延迟，直接显示比较好
                    this.DisplayImage();
                }
            }
            else
            {
                ShowImageFailure();
            }
        }

        public void DisplayImage()
        {
            ImageView.Image = Photo.UnderlyingImage;
            this.HideImageFailure();
        }

        #endregion

        #region touches

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            ImageView.Alpha = 0.6f;
            base.TouchesBegan(touches, evt);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            ImageView.Alpha = 1;
            base.TouchesEnded(touches, evt);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            ImageView.Alpha = 1;
            base.TouchesCancelled(touches, evt);
        }

        #endregion

        #region Indicator

        void ShowLoadingIndicator()
        {
            if (LoadingIndicator != null)
            {
                LoadingIndicator.Value = 0;
                LoadingIndicator.Hidden = false;
                this.HideImageFailure();
            }
        }

        void HideLoadingIndicator()
        {
            if (LoadingIndicator != null)
            {
                LoadingIndicator.Hidden = true;
            }
        }

        void ShowImageFailure()
        {
            if (LoadingError == null)
            {
                LoadingError = new UIImageView();
//                LoadingError.Image = UIImage.FromBundle("MWPhotoBrowser.bundle/images/ImageError.png");
                LoadingError.Image = UIImage.FromBundle("delete.png");
                LoadingError.UserInteractionEnabled = false;
                LoadingError.SizeToFit();
                this.AddSubview(LoadingError);
            }
            HideLoadingIndicator();
            ImageView.Image = null;
//            var x = Convert.ToSingle((this.Bounds.Size.Width - LoadingError.Frame.Size.Width) / 2);
//            var y = Convert.ToSingle((this.Bounds.Size.Height - LoadingError.Frame.Size.Height) / 2);
//            LoadingError.Frame = new CGRect(x, y, LoadingError.Frame.Size.Width, LoadingError.Frame.Size.Height);
            var x = Convert.ToSingle((this.Bounds.Size.Width - 20) / 2);
            var y = Convert.ToSingle((this.Bounds.Size.Height - 20) / 2);
            LoadingError.Frame = new CGRect(x, y, 20, 20);
        }

        void HideImageFailure()
        {
            if (LoadingError != null)
            {
                LoadingError.RemoveFromSuperview();
                LoadingError = null;
            }
        }

        #endregion

        #region Notifications

        void SetProgressFromNotification(object sender, NSNotificationEventArgs notification)
        {
                
            var dict = (NSDictionary)notification.Notification.Object;
            var photoWithProgress = (MWPhoto)dict.ObjectForKey(new NSString("photo"));
            if (photoWithProgress == Photo)
            {
                var p = dict.ValueForKey(new NSString("progress")) as NSNumber;
                var progress = Convert.ToSingle(p.FloatValue);
                LoadingIndicator.Value = Math.Max(Math.Min(1, progress), 0);
            }
                
        }

        void HandleMWPhotoLoadingDidEndNotification(object sender, NSNotificationEventArgs  notification)
        {
            var photo = (MWPhoto)notification.Notification.Object;
            if (photo == Photo)
            {
                if (Photo.UnderlyingImage != null)
                {
                    DisplayImage();
                }
                else
                {
                    // Failed to load
                    ShowImageFailure();
                }
                HideLoadingIndicator();
            }
        }


        #endregion


    }
}

