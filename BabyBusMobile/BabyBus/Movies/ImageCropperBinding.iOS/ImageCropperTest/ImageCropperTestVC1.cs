
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using ImageCropperBinding;

namespace ImageCropperTest {
    public partial class ImageCropperTestVC1 : UIViewController, IVPImageCropperDelegate {
        //iphone6 plus screen
        const int ORIGINAL_MAX_WIDTH = 1080;
        public static  nfloat WIDTHOFIMAGE = 78.0f;


        public ImageCropperTestVC1()
            : base("ImageCropperTestVC1", null) {
        }

        UIImageView _portraitImageView = null;
        UIImagePickerController _imagePicker;
        //		#pragma mark portraitImageView getter
        public UIImageView PortraitImageView {
            get {
                if (_portraitImageView == null) {
                    nfloat w = WIDTHOFIMAGE;
                    var h = w;
                    var x = (this.View.Frame.Size.Width - w) / 2;
                    var y = (this.View.Frame.Size.Height - h) / 2;

                    _portraitImageView = new UIImageView(new CGRect(x, y, w, h));
                    _portraitImageView.Layer.CornerRadius = (_portraitImageView.Frame.Size.Height / 2);
                    _portraitImageView.Layer.MasksToBounds = false;
                    _portraitImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
                    _portraitImageView.ClipsToBounds = true;
                    _portraitImageView.Layer.ShadowColor = UIColor.Black.CGColor;
                    _portraitImageView.Layer.ShadowOffset = new CGSize(4, 4);
                    _portraitImageView.Layer.ShadowOpacity = 0.5f;
                    _portraitImageView.Layer.ShadowRadius = 2.0f;
                    _portraitImageView.Layer.BorderColor = UIColor.Black.CGColor;
                    _portraitImageView.Layer.BorderWidth = 2.0f;
                    _portraitImageView.UserInteractionEnabled = true;
                    _portraitImageView.BackgroundColor = UIColor.Black;
                    UITapGestureRecognizer portraitTap = new  UITapGestureRecognizer(editPortrait);
                    _portraitImageView.AddGestureRecognizer(portraitTap);
                }
                return _portraitImageView;
            }
        }

        void editPortrait() {
            //			UIActionSheet *choiceSheet = [[UIActionSheet alloc] initWithTitle:nil
            //				delegate:self
            //				cancelButtonTitle:@"取消"
            //                                               destructiveButtonTitle:nil
            //				otherButtonTitles:@"拍照", @"从相册中选取", nil];
            //			[choiceSheet showInView:self.view];

            UIActionSheet actionSheet = new UIActionSheet(
                                            null, null, "取消", null,
                                            new string[] { "拍照", "从相册中选取" });
            actionSheet.ShowInView(this.View);

            actionSheet.Clicked += (object sender, UIButtonEventArgs e) => {
                var buttonIndex = e.ButtonIndex;
                if (buttonIndex == 0) {
                    // 拍照
                    if (isCameraAvailable() && doesCameraSupportTakingPhotos()) {
                        _imagePicker = new UIImagePickerController();
                        _imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;
                        //						if ([self isFrontCameraAvailable]) {
                        //							controller.cameraDevice = UIImagePickerControllerCameraDeviceFront;
                        //						}
                        string[] mediaTypes = { "kUTTypeImage" };
                        //						NSMutableArray *mediaTypes = [[NSMutableArray alloc] init];
                        //						[mediaTypes addObject:(__bridge NSString *)kUTTypeImage];
                        _imagePicker.MediaTypes = mediaTypes;
                        //						_imagePicker.delegate = self;

                        _imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
                        _imagePicker.Canceled += Handle_Canceled;
                        this.PresentViewController(_imagePicker, true, null);

                    }
                    //
                } else if (buttonIndex == 1) {
                    // 从相册中选取
                    if (isPhotoLibraryAvailable()) {
                        _imagePicker = new UIImagePickerController();
                        _imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
                        _imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
                        _imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
                        _imagePicker.Canceled += Handle_Canceled;
                        this.PresentViewController(_imagePicker, true, null);

                    }
                }

            };
            actionSheet.Canceled += (object sender, EventArgs e) => {

            };

        }
        // Do something when the
        void Handle_Canceled(object sender, EventArgs e) {
            Console.WriteLine("picker cancelled");
            _imagePicker.DismissViewController(true, null);
        }

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
//					portraitImg =  imageByScalingToMaxSize(portraitImg);
                    VPImageCropperViewController imgCropperVC
						= new VPImageCropperViewController(portraitImg, new CGRect(0f, 100.0f, View.Frame.Size.Width, View.Frame.Size.Width), 3);
                    imgCropperVC.WeakVPImageCropperDelegate = this;
                    PresentViewController(imgCropperVC, true, null);
                }
            });

        }

        public UIImage ImageByScalingToMaxSize(UIImage sourceImage, nfloat maxWithd) {
            maxWithd = maxWithd > ORIGINAL_MAX_WIDTH ? ORIGINAL_MAX_WIDTH : maxWithd;
            if (sourceImage.Size.Width < maxWithd)
                return sourceImage;
            nfloat btWidth = 0.0f;
            nfloat btHeight = 0.0f;
            if (sourceImage.Size.Width > sourceImage.Size.Height) {
                btHeight = maxWithd;
                btWidth = sourceImage.Size.Width * (maxWithd / sourceImage.Size.Height);
            } else {
                btWidth = maxWithd;
                btHeight = sourceImage.Size.Height * (maxWithd / sourceImage.Size.Width);
            }
            CGSize targetSize = new CGSize(btWidth, btHeight);
            return this.imageByScalingAndCroppingForSourceImage(sourceImage, targetSize);
        }

        UIImage imageByScalingAndCroppingForSourceImage(UIImage sourceImage, CGSize targetSize) {
            UIImage newImage = null;
            CGSize imageSize = sourceImage.Size;
            nfloat width = imageSize.Width;
            nfloat height = imageSize.Height;
            nfloat targetWidth = targetSize.Width;
            nfloat targetHeight = targetSize.Height;
            nfloat scaleFactor = 0.0f;
            nfloat scaledWidth = targetWidth;
            nfloat scaledHeight = targetHeight;
            CGPoint thumbnailPoint = new CGPoint(0.0f, 0.0f);
            if (imageSize == targetSize) {
                nfloat widthFactor = targetWidth / width;
                nfloat heightFactor = targetHeight / height;

                if (widthFactor > heightFactor)
                    scaleFactor = widthFactor; // scale to fit height
				else
                    scaleFactor = heightFactor; // scale to fit width
                scaledWidth = width * scaleFactor;
                scaledHeight = height * scaleFactor;

                // center the image
                if (widthFactor > heightFactor) {
                    thumbnailPoint.Y = (targetHeight - scaledHeight) * 0.5f;
                } else if (widthFactor < heightFactor) {
                    thumbnailPoint.X = (targetWidth - scaledWidth) * 0.5f;
                }
            }
            UIGraphics.BeginImageContext(targetSize); // this will crop
            CGRect thumbnailRect = new CGRect();

            thumbnailRect.Location = thumbnailPoint;
            thumbnailRect.Width = scaledWidth;
            thumbnailRect.Height = scaledHeight;

            sourceImage.Draw(thumbnailRect);

            newImage = UIGraphics.GetImageFromCurrentImageContext();
            if (newImage == null)
                Console.WriteLine(@"could not scale image");

            //pop the context to get back to the default
            UIGraphics.EndImageContext();
            return newImage;
        }



        public override void ViewDidLoad() {
            base.ViewDidLoad();

            // Perform any additional setup after loading the View, typically from a nib.


            this.View.AddSubview(PortraitImageView);
            UIImage imageCrop =	UIImage.FromBundle("baby.jpeg");
//				FromUrl (@"http://photo.l99.com/bigger/31/1363231021567_5zu910.jpg");
            _portraitImageView.Image = imageCrop;

        }

        static UIImage FromUrl(string uri) {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }


        //		#pragma mark camera utility
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

        void IVPImageCropperDelegate.ImageCropper(VPImageCropperViewController cropperViewController, UIImage editedImage) {
            this.PortraitImageView.Image = 
                this.ImageByScalingToMaxSize(editedImage, ImageCropperTestVC1.WIDTHOFIMAGE);
            cropperViewController.DismissViewController(true, null);
        }

        void IVPImageCropperDelegate.ImageCropperDidCancel(VPImageCropperViewController cropperViewController) {
            cropperViewController.DismissViewController(true, null);
        }
    }

    //    public class VPImageCropperDelegateImpl : VPImageCropperDelegate {
    //        ImageCropperTestVC1 _imageCropperTestVC;
    //
    //        public VPImageCropperDelegateImpl(ImageCropperTestVC1 imageCropperTestVC) {
    //            _imageCropperTestVC = imageCropperTestVC;
    //        }
    //
    //        public override void ImageCropper(VPImageCropperViewController cropperViewController, UIImage editedImage) {
    //            _imageCropperTestVC.PortraitImageView.Image =
    //				_imageCropperTestVC.ImageByScalingToMaxSize(editedImage, ImageCropperTestVC1.WIDTHOFIMAGE);
    //            cropperViewController.DismissViewController(true, null);
    //
    //        }
    //
    //        public override void ImageCropperDidCancel(VPImageCropperViewController cropperViewController) {
    //            cropperViewController.DismissViewController(true, null);
    //            //			throw new You_Should_Not_Call_base_In_This_Method ();
    //        }
    //    }
}

