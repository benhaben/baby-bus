// MvxImagePickerTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using CoreGraphics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Touch.Platform;
using Cirrious.CrossCore.Touch.Views;
using Foundation;
using UIKit;

using ImageCropperBinding;

namespace Cirrious.MvvmCross.Plugins.PictureChooser.Touch {
    public class MvxImagePickerTask
        : MvxTouchTask
        , IMvxPictureChooserTask {

        private readonly UIImagePickerController _imagePicker;
        private readonly IMvxTouchModalHost _modalHost;
        private bool _currentlyActive;
        private int _maxPixelDimension;
        private int _percentQuality;
        private Action<Stream> _pictureAvailable;
        private Action _assumeCancelled;

        public MvxImagePickerTask() {
            _modalHost = Mvx.Resolve<IMvxTouchModalHost>();
            _imagePicker = new UIImagePickerController {
                //CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo,
                //CameraDevice = UIImagePickerControllerCameraDevice.Front
            };
//            _imagePicker.FinishedPickingMedia += Picker_FinishedPickingMedia;
//            _imagePicker.FinishedPickingImage += Picker_FinishedPickingImage;
//            _imagePicker.Canceled += Picker_Canceled;
        }

        public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                             Action assumeCancelled) {
            _imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            ChoosePictureCommon(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

      

        public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                Action assumeCancelled) {
            _imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;
            ChoosePictureCommon(maxPixelDimension, percentQuality, pictureAvailable, assumeCancelled);
        }

        public Task<Stream> ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality) {
            var task = new TaskCompletionSource<Stream>();
            ChoosePictureFromLibrary(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public Task<Stream> TakePicture(int maxPixelDimension, int percentQuality) {
            var task = new TaskCompletionSource<Stream>();
            TakePicture(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
            return task.Task;
        }

        public void ContinueFileOpenPicker(object args) {
        }

        private void ChoosePictureCommon(int maxPixelDimension, int percentQuality,
                                         Action<Stream> pictureAvailable, Action assumeCancelled) {
            SetCurrentlyActive();
            _maxPixelDimension = maxPixelDimension;
            _percentQuality = percentQuality;
            _pictureAvailable = pictureAvailable;
            _assumeCancelled = assumeCancelled;

            _modalHost.PresentModalViewController(_imagePicker, true);
        }

        private void HandleImagePick(UIImage image) {
            ClearCurrentlyActive();
            if (image != null) {
                if (_maxPixelDimension > 0 && (image.Size.Height > _maxPixelDimension || image.Size.Width > _maxPixelDimension)) {
                    // resize the image
                    image = image.ImageToFitSize(new CGSize(_maxPixelDimension, _maxPixelDimension));
                }

                using (NSData data = image.AsJPEG((float)(_percentQuality / 100.0))) {
                    var byteArray = new byte[data.Length];
                    Marshal.Copy(data.Bytes, byteArray, 0, Convert.ToInt32(data.Length));

                    var imageStream = new MemoryStream(byteArray, false);
                    if (_pictureAvailable != null)
                        _pictureAvailable(imageStream);

                }
            } else {
                if (_assumeCancelled != null)
                    _assumeCancelled();
            }

            _imagePicker.DismissViewController(true, () => {
            });
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }

        void Picker_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e) {
            var image = e.EditedImage ?? e.OriginalImage;
            HandleImagePick(image);

        }

        void Picker_FinishedPickingImage(object sender, UIImagePickerImagePickedEventArgs e) {
            var image = e.Image;
            HandleImagePick(image);
        }

        void Picker_Canceled(object sender, EventArgs e) {
            Console.WriteLine("picker cancelled");
            ClearCurrentlyActive();
            if (_assumeCancelled != null)
                _assumeCancelled();
            _imagePicker.DismissViewController(true, () => {
            });
            _modalHost.NativeModalViewControllerDisappearedOnItsOwn();
        }

        private void SetCurrentlyActive() {
            if (_currentlyActive)
                Mvx.Warning("MvxImagePickerTask called when task already active");
            _currentlyActive = true;
        }

        private void ClearCurrentlyActive() {
            if (!_currentlyActive)
                Mvx.Warning("Tried to clear currently active - but already cleared");
            _currentlyActive = false;
        }

        public void ChoosePictureWithCrop(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                          Action assumeCancelled) {
            if (isPhotoLibraryAvailable()) {
                _pictureAvailable = pictureAvailable;
                _imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
                _imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
                _imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
                _imagePicker.Canceled += Picker_Canceled;
                _modalHost.PresentModalViewController(_imagePicker, false);
                //				ViewController.PresentViewController (_imagePicker, true, null);
            }

        }

        public void TakePictureWithCrop(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailable,
                                        Action assumeCancelled) {
            if (isCameraAvailable()) {
                NSTimer.CreateScheduledTimer(new TimeSpan(1), (t) => {
                    _pictureAvailable = pictureAvailable;
                    _imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;
                    _imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.Camera);
                    _imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
                    _imagePicker.Canceled += Picker_Canceled;
                    _modalHost.PresentModalViewController(_imagePicker, false);
                    if (t != null) {
                        t.Invalidate();
                    }
                });
            }
        }

        //        void Handle_Canceled(object sender, EventArgs e) {
        //            Console.WriteLine("picker cancelled");
        //            _imagePicker.DismissViewController(true, null);
        //        }

        // This is a sample method that handles the FinishedPickingMediaEvent
        protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e) {
            // determine what was selected, video or image
            bool isImage = false;
            switch (e.Info[UIImagePickerController.MediaType].ToString()) {
                case "public.image":
                    Console.WriteLine("Image selected");
                    isImage = true;
                    break;

                case "public.video":
                    Console.WriteLine("Video selected");
                    break;
            }
            _imagePicker.DismissViewController(true, () => {
                if (!isImage)
                    return;
                UIImage portraitImg = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                if (portraitImg != null) {
                    // do something with the image
                    Console.WriteLine("got the original image");
                    var viewController = _modalHost.CurrentTopViewController;
                    VPImageCropperViewController imgCropperVC
						= new VPImageCropperViewController(portraitImg
						, new CGRect(0f, 100.0f, viewController.View.Frame.Size.Width, viewController.View.Frame.Size.Width), 3);
//					var mvxTouchViewPresenter = _modalHost as MvxTouchViewPresenter;
                    imgCropperVC.WeakVPImageCropperDelegate = 
                        new VPImageCropperDelegateImpl(this, HandleImagePick, Picker_Canceled);
//                    imgCropperVC.WeakVPImageCropperDelegate = this;
                    viewController.PresentViewController(imgCropperVC, true, null);
//					ViewController.PresentViewController (imgCropperVC, true, null);
                }
            });

        }

        bool isCameraAvailable() {
            return UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera);
            //			return [UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera];
        }

        bool isRearCameraAvailable() {
            return UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Rear);
            //			return [UIImagePickerController isCameraDeviceAvailable:UIImagePickerControllerCameraDeviceRear];
        }

        bool isFrontCameraAvailable() {
            return UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Front);
            //			return [UIImagePickerController isCameraDeviceAvailable:UIImagePickerControllerCameraDeviceFront];
        }


        bool doesCameraSupportTakingPhotos() {
            return cameraSupportsMedia("kUTTypeImage", UIImagePickerControllerSourceType.Camera);

            //			return [self cameraSupportsMedia:(__bridge NSString *)kUTTypeImage sourceType:UIImagePickerControllerSourceTypeCamera];
        }

        bool isPhotoLibraryAvailable() {
            return UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.PhotoLibrary);
            //			return [UIImagePickerController isSourceTypeAvailable:
            //				UIImagePickerControllerSourceTypePhotoLibrary];
        }

        bool canUserPickVideosFromPhotoLibrary() {
            return cameraSupportsMedia("kUTTypeMovie", UIImagePickerControllerSourceType.PhotoLibrary);

            //			return [self
            //				cameraSupportsMedia:(__bridge NSString *)kUTTypeMovie sourceType:UIImagePickerControllerSourceTypePhotoLibrary];
        }

        bool canUserPickPhotosFromPhotoLibrary() {
            return cameraSupportsMedia("kUTTypeImage", UIImagePickerControllerSourceType.PhotoLibrary);

            //			return [self
            //				cameraSupportsMedia:(__bridge NSString *)kUTTypeImage sourceType:UIImagePickerControllerSourceTypePhotoLibrary];
        }

        bool cameraSupportsMedia(string paramMediaType, UIImagePickerControllerSourceType paramSourceType) {
            bool result = false;
            if (string.IsNullOrEmpty(paramMediaType)) {
                return false;
            }
            //			AvailableMediaTypesForSourceType
            var availableMediaTypes = UIImagePickerController.AvailableMediaTypes(paramSourceType);
            foreach (var mediaType in availableMediaTypes) {
                if (mediaType == paramMediaType) {
                    result = true;
                    break;
                }
            }

            return result;
        }


        //        void IVPImageCropperDelegate.ImageCropper(VPImageCropperViewController cropperViewController, UIImage editedImage) {
        //            cropperViewController.DismissViewController(true, null);
        //            HandleImagePick(editedImage);
        //        }
        //
        //        void IVPImageCropperDelegate.ImageCropperDidCancel(VPImageCropperViewController cropperViewController) {
        //            cropperViewController.DismissViewController(true, null);
        //            Picker_Canceled(null);
        //        }
        public class VPImageCropperDelegateImpl : VPImageCropperDelegate {
            public Action<UIImage> Callback { get; set; }

            public EventHandler CancellHandler;
            MvxImagePickerTask _holdThisObject;

            public VPImageCropperDelegateImpl(MvxImagePickerTask that, Action<UIImage> callback, EventHandler h) {
                Callback = callback;
                CancellHandler = h;
                _holdThisObject = that;
            }

            public override void ImageCropper(VPImageCropperViewController cropperViewController, UIImage editedImage) {
                cropperViewController.DismissViewController(true, null);
                if (Callback != null)
                    Callback(editedImage);
            }

            public override void ImageCropperDidCancel(VPImageCropperViewController cropperViewController) {
                cropperViewController.DismissViewController(true, null);

                if (CancellHandler != null)
                    CancellHandler(null, null);
            }
        }
       
    }
}