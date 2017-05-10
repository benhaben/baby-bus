using System;
using UIKit;
using Foundation;
using SDWebImage;
using System.Diagnostics;
using System.Threading.Tasks;
using AssetsLibrary;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;

namespace PhotoBrowser {
    public class MWPhoto : NSObject, IMWPhotoProtocol, ISDWebImageOperation {
        public static  NSString MWPHOTO_LOADING_DID_END_NOTIFICATION = new NSString("MWPHOTO_LOADING_DID_END_NOTIFICATION");
        public  static NSString MWPHOTO_PROGRESS_NOTIFICATION = new NSString("MWPHOTO_PROGRESS_NOTIFICATION");

        void ISDWebImageOperation.Cancel() {
            throw new NotImplementedException();
        }

        public  MWPhoto(UIImage image) {
            base.Init();
            Image = image;
        }

        private  MWPhoto(NSUrl url) {
            base.Init();
            PhotoUrl = url;
        }

        private UIImage Image {
            get;
            set;
        }

        private NSUrl PhotoUrl {
            get;
            set;
        }

        private bool LoadingInProgress {
            get;
            set;
        }

        private SDWebImageOperation WebImageOperation {
            get;
            set;
        }

        // Return underlying UIImage to be displayed
        // Return nil if the image is not immediately available (loaded into memory, preferably
        // already decompressed) and needs to be loaded from a source (cache, file, web, etc)
        // IMPORTANT: You should *NOT* use this method to initiate
        // fetching of images from any external of source. That should be handled
        // in -loadUnderlyingImageAndNotify: which may be called by the photo browser if this
        // methods returns nil.
        public UIImage UnderlyingImage {
            get;
            set;
        }

        public string Caption {
            get;
            set;
        }

        public static MWPhoto PhotoWithImage(UIImage image) {
            return new MWPhoto(image);

        }

        public static MWPhoto PhotoWithURL(NSUrl url) {
            return new MWPhoto(url);
        }

        public virtual void LoadUnderlyingImageAndNotify() {
            Debug.Assert(NSThread.Current.IsMainThread, "This method must be called on the main thread.");
            if (LoadingInProgress) {
                return;
            } else {
                LoadingInProgress = true;
            }


            try {
                if (UnderlyingImage != null) {
                    this.ImageLoadingComplete();
                } else {
                    PerformLoadUnderlyingImageAndNotify();
                }
            } catch (Exception ex) {
                this.UnderlyingImage = null;
                LoadingInProgress = false;
                this.ImageLoadingComplete();
                MvxTrace.Trace("Photo from asset library error: {0}", ex.Message);
            }
        }

        public virtual void PerformLoadUnderlyingImageAndNotify() {
            if (Image != null) {
                // We have UIImage!
                this.UnderlyingImage = Image;
                this.ImageLoadingComplete();
            } else if (PhotoUrl != null) {
                // Check what type of url it is
                if (PhotoUrl.Scheme.ToLower() == "assets-library") {
                    LoadFromAssetsLibrary();
                } else if (PhotoUrl.IsFileUrl) {
                    LoadFromFile();
                } else {
                    // Load async from web (using SDWebImage)
                    LoadFromWeb();
                }
            } else {
                // Failed - no source
                throw new Exception("Failed - no source");
            }
        }

        public virtual void UnloadUnderlyingImage() {
            LoadingInProgress = false;
            this.UnderlyingImage = null;
        }

        public  virtual void CancelAnyLoading() {
            if (WebImageOperation != null) {
                WebImageOperation.Cancel();
                LoadingInProgress = false;
            }
        }

        void LoadFromAssetsLibrary() {
            Task.Run(() => {
                try {
                    ALAssetsLibrary assetslibrary = new ALAssetsLibrary();
                    assetslibrary.AssetForUrl(PhotoUrl, (ALAsset asset) => {
                        ALAssetRepresentation rep = asset.DefaultRepresentation;
                        var iref = rep.GetFullScreenImage();
                        if (iref != null) {
                            this.UnderlyingImage = UIImage.FromImage(iref);
                        }
                        this.InvokeOnMainThread(() => this.ImageLoadingComplete());
                    }, (NSError error) => {
                        this.UnderlyingImage = null;
                        MvxTrace.Trace("Photo from asset library error: {0}", error.LocalizedFailureReason);
                        this.InvokeOnMainThread(() => this.ImageLoadingComplete());
                    });
                } catch (Exception ex) {
                    MvxTrace.Trace("Photo from asset library error: {0}", ex.Message);
                    this.InvokeOnMainThread(() => this.ImageLoadingComplete());
                }
            });
            //task run
        }

        void LoadFromWeb() {
            try {
                SDWebImageManager manager = SDWebImageManager.SharedManager();
                manager.DownloadImageWithURL(PhotoUrl, SDWebImageOptions.SDWebImageRetryFailed, (int receivedSize, int expectedSize) => {
                    //                            progress
                    if (expectedSize > 0) {
                        float progress = receivedSize / (float)expectedSize;
                        NSDictionary dict = new NSDictionary();
                        dict.SetValueForKey(NSNumber.FromFloat(progress), new NSString("progress"));
                        dict.SetValueForKey(this, new NSString("photo"));
                        NSNotificationCenter.DefaultCenter.PostNotificationName(MWPhoto.MWPHOTO_PROGRESS_NOTIFICATION, dict);
                    }
                }, (UIImage image, NSError error, SDImageCacheType cacheType, sbyte finished, NSUrl imageUrl) => {
                    if (error != null) {
                        MvxTrace.Trace("Photo from asset library error: {0}", error.LocalizedFailureReason);
                    }
                    WebImageOperation = null;
                    this.UnderlyingImage = image;
                    this.ImageLoadingComplete();
                });
            } catch (Exception ex) {
                MvxTrace.Trace("Photo from web:: {0}", ex.Message);
                WebImageOperation = null;
                this.ImageLoadingComplete();
            }
        }

        void LoadFromFile() {
            Task.Run(() => {
                try {
                    this.UnderlyingImage = UIImage.FromFile(PhotoUrl.Path);
                    if (UnderlyingImage == null) {
                        MvxTrace.Trace("Photo from asset library error: {0}", PhotoUrl.Path);
                    }
                } finally {
                    this.InvokeOnMainThread(() => this.ImageLoadingComplete());
                }
            });
        }

        void ImageLoadingComplete() {
            Debug.Assert(NSThread.Current.IsMainThread, "This method must be called on the main thread.");

            // Complete so notify
            LoadingInProgress = false;
            // Notify on next run loop
            PostCompleteNotification();
        }

        void PostCompleteNotification() {
            NSNotificationCenter.DefaultCenter.PostNotificationName(MWPhoto.MWPHOTO_LOADING_DID_END_NOTIFICATION, this);
        }
    }
}

