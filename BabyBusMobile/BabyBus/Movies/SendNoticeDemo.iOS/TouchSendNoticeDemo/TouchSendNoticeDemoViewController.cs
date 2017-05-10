using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ELCImagePickerControllerBinding;
using PhotoStackBinding;
using MonoTouch.MobileCoreServices;
using System.Collections.Generic;
using MonoTouch.CoreGraphics;

namespace TouchSendNoticeDemo
{
    public partial class TouchSendNoticeDemoViewController 
		: UIViewController
		, IELCImagePickerControllerDelegate
		, IPhotoStackViewDataSource
		, IPhotoStackViewDelegate
    {
        public TouchSendNoticeDemoViewController ()
        {
        }

        public override void DidReceiveMemoryWarning ()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning ();
			
            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			
            // Perform any additional setup after loading the view, typically from a nib.
            Setup ();
        }

        public override void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);
        }

        public override void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);
        }

        public override void ViewWillDisappear (bool animated)
        {
            base.ViewWillDisappear (animated);
        }

        public override void ViewDidDisappear (bool animated)
        {
            base.ViewDidDisappear (animated);
        }

        #endregion

        const float SmallButtonWidthAndHeight = 20f;
        const float Height = 40f;
        const float HeightOfContent = 100f;
        const float MarginLeftRight = 10.0f;
        const float MarginTopToFrame = 20f;
        const float Margin = 10f;
        const float MarginSmall = 3f;
        const int MaximumImagesCount = 9;
        const float PhotoStackWidth = 200f;

        const float Red = 230.0f / 255.0f;
        const float Green = 250.0f / 255.0f;
        const float Blue = 250.0f / 255.0f;
        const float Alpha = 1.0f;
        const float BorderWidth = 1.0f;
        const float CornerRadius = 3.0f;


        IList<UIImage> _images = null;

        IList<UIImage> Images {
            get { 
                if (_images == null) {
                    _images = new List<UIImage> ();
                }
                return _images;
            }
            set { 
                _images = value;
            }
        }


        UIPageControl _pageControl = null;

        UIPageControl PageControl {
            get { 
                if (_pageControl == null) {
                    _pageControl = new UIPageControl ();
                    _pageControl.BackgroundColor = UIColor.Gray;
                }
                return _pageControl;
            }
        }

        PhotoStackView _photoStack = null;

        PhotoStackView PhotoStack {
            get {
                if (_photoStack == null) {   
                    _photoStack = new PhotoStackView ();
//                    _photoStack.BackgroundColor = UIColor.Blue;
                    _photoStack.ShowBorder = true;
//                    _photoStack.BorderWidth = BorderWidth;
                    _photoStack.Layer.BorderWidth = BorderWidth;
                    _photoStack.Layer.BorderColor = new CGColor (Red, Green, Blue, Alpha);
                    _photoStack.Layer.CornerRadius = CornerRadius;
                    _photoStack.Layer.MasksToBounds = true;
                    _photoStack.WeakPhotoStackViewDataSourceDelegate = this;
                    _photoStack.WeakPhotoStackViewDelegate = this;
                }

                return _photoStack;
            }
        }

        UITextField _title = null;

        public UITextField NoticeTitle {
            get {
                if (_title == null) {   
                    _title = new UITextField ();
                    //			title.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleBottomMargin;
                    _title.Placeholder = "标题";
//                    _title.BorderStyle = UITextBorderStyle.Line;
                    _title.Layer.BorderWidth = BorderWidth;
                    _title.Layer.BorderColor = new CGColor (Red, Green, Blue, Alpha);
                    _title.Layer.CornerRadius = CornerRadius;
                    _title.Layer.MasksToBounds = true;

//                    _title.BackgroundColor = UIColor.Blue;
                }
                return _title;
            }
        }

        UIButton _chooseType = null;

        public UIButton ChooseType {
            get {
                if (_chooseType == null) {
                   
                    _chooseType = new UIButton (UIButtonType.System);
                    _chooseType.Layer.BorderWidth = BorderWidth;
                    _chooseType.Layer.BorderColor = new CGColor (Red, Green, Blue, Alpha);
                    _chooseType.Layer.CornerRadius = CornerRadius;
                    _chooseType.Layer.MasksToBounds = true;
                    _chooseType.SetTitle ("发布对象，默认为全班", UIControlState.Normal);
                    UIImage chooseImages = UIImage.FromFile ("delete.png");
                    _chooseType.SetImage (chooseImages, UIControlState.Normal);
                    //top left bottom right
//                    _chooseType.ImageEdgeInsets = new UIEdgeInsets (0, 0, 0, -_chooseType.Frame.Width);

                    _chooseType.ImageEdgeInsets = new UIEdgeInsets (0, 0, -chooseImages.Size.Width, 0);
                    _chooseType.TitleEdgeInsets = new UIEdgeInsets (0, 0, 0, -_chooseType.TitleLabel.Bounds.Size.Width);


//                    UIImage *img = [UIImage imageNamed:@"forward.png"];
//                    self.navigationItem.rightBarButtonItem = [[[UIBarButtonItem alloc] initWithImage:img 
//                        style:UIBarButtonItemStylePlain 
//                        target:self 
//                        action:@selector(RightArrow:)] autorelease];

                }
                return _chooseType;
            }
        }

        UITextView _content = null;

        public UITextView Content {
            get {
                if (_content == null) {
                    _content = new UITextView ();
                    _content.Layer.BorderColor = new CGColor (Red, Green, Blue, Alpha);
                    _content.Layer.BorderWidth = BorderWidth;
                    _content.Layer.CornerRadius = CornerRadius;
                    _content.Layer.MasksToBounds = true;
                }
                return _content;
            }
        }

        UIButton _chooseImagesButton = null;

        public UIButton ChooseImagesButton {
            get {
                if (_chooseImagesButton == null) {
                    _chooseImagesButton = new UIButton ();
                    UIImage chooseImages = UIImage.FromFile ("camera.jpeg");
                    _chooseImagesButton.SetBackgroundImage (chooseImages, UIControlState.Normal);
                    _chooseImagesButton.TouchUpInside += (object sender, EventArgs e) => {
                        ELCImagePickerController elcPicker = new ELCImagePickerController ();
                        elcPicker.MaximumImagesCount = MaximumImagesCount;
                        //Set the maximum number of images to select to 100
                        elcPicker.ReturnsOriginalImage = true;
                        //Only return the fullScreenImage, not the fullResolutionImage
                        elcPicker.ReturnsImage = true;
                        //Return UIimage if YES. If NO, only return asset location information
                        elcPicker.OnOrder = true;
                        //For multiple image selection, display and return order of selected images
                        elcPicker.MediaTypes = new string[] {
                            UTType.Image
                        };
                        elcPicker.WeakELCImagePickerControllerDelegate = this;
                        PresentViewController (elcPicker, true, null);
                    };
                }
                return _chooseImagesButton;
            }
        }

        UIButton _deleteImageButton = null;

        public UIButton DeleteImageButton {
            get {
                if (_deleteImageButton == null) {
                    _deleteImageButton = new UIButton ();
                    UIImage deleteImage = UIImage.FromFile ("delete.png");
                    _deleteImageButton.SetBackgroundImage (deleteImage, UIControlState.Normal);
                    _deleteImageButton.TouchUpInside += (object sender, EventArgs e) => {
                        int pageIndex = this.PageControl.CurrentPage;
                        if (pageIndex < Images.Count) {
                            Images.RemoveAt (pageIndex);
                            PhotoStack.ReloadData ();
//                            NSObject[] deleteImages = new NSObject[Images.Count];
//                            for (int i = 0; i < Images.Count; i++) {
//                                deleteImages [i] = Images [i];
//                            }
//                            PhotoStack.SetPhotoViews (deleteImages);
                        }
                    };
                }
                return _deleteImageButton;
            }
        }

        void Setup ()
        {
            View.BackgroundColor = UIColor.White;
            UIView[] v = {
                NoticeTitle,
                ChooseType,
                Content,
                PhotoStack,
                ChooseImagesButton,
                DeleteImageButton,
                PageControl
            };
            View.AddSubviews (v);
            SetUpConstrainLayout ();
            PhotoStack.Enabled = false;

//			PageControl.Pages = Photos.Count;
        }

        void SetUpConstrainLayout ()
        {
            View.ConstrainLayout 
			(
				// Analysis disable CompareOfFloatsByEqualityOperator
                () => 
				NoticeTitle.Frame.Height == Height
                && NoticeTitle.Frame.Left == View.Frame.Left + MarginLeftRight
                && NoticeTitle.Frame.Right == View.Frame.Right - MarginLeftRight
                && NoticeTitle.Frame.Top == View.Frame.Top + (MarginTopToFrame)

                && ChooseType.Frame.Height == Height && ChooseType.Frame.Left == View.Frame.Left + MarginLeftRight
                && ChooseType.Frame.Right == View.Frame.Right - MarginLeftRight
                && ChooseType.Frame.Top == NoticeTitle.Frame.Bottom + Margin

                && Content.Frame.Height == HeightOfContent
                && Content.Frame.Left == View.Frame.Left + MarginLeftRight
                && Content.Frame.Right == View.Frame.Right - MarginLeftRight
                && Content.Frame.Top == ChooseType.Frame.Bottom + Margin

                && PhotoStack.Frame.Height == PhotoStackWidth
                && PhotoStack.Frame.Top == (Content.Frame.Bottom + Margin)
                && PhotoStack.Frame.Left == View.Frame.Left + MarginLeftRight
                && PhotoStack.Frame.Right == View.Frame.Right - MarginLeftRight

                && ChooseImagesButton.Frame.Width == SmallButtonWidthAndHeight
                && ChooseImagesButton.Frame.Height == SmallButtonWidthAndHeight
                && ChooseImagesButton.Frame.Top == PhotoStack.Frame.Top + MarginSmall
                && ChooseImagesButton.Frame.Right == PhotoStack.Frame.Right - MarginSmall

                && DeleteImageButton.Frame.Width == SmallButtonWidthAndHeight
                && DeleteImageButton.Frame.Height == SmallButtonWidthAndHeight
                && DeleteImageButton.Frame.Top == PhotoStack.Frame.Top + MarginSmall
                && DeleteImageButton.Frame.Right == ChooseImagesButton.Frame.Left - MarginSmall

                && PageControl.Frame.Height == SmallButtonWidthAndHeight
                && PageControl.Frame.Top == PhotoStack.Frame.Bottom + MarginSmall
                && PageControl.Frame.Left == View.Frame.Left + MarginLeftRight
                && PageControl.Frame.Right == View.Frame.Right - MarginLeftRight


				// Analysis restore CompareOfFloatsByEqualityOperator
            );
        }

        void IELCImagePickerControllerDelegate.ECLImagePickerController (ELCImagePickerController picker, NSObject[] info)
        {
            //			info[0]
            //			{
            //				ALAssetPropertyLocation = "<+34.24585000,+108.92559167> +/- 0.00m (speed 0.00 mps / course 0.00) @ 01/1/1 \U4e2d\U56fd\U6807\U51c6\U65f6\U95f4\U4e0a\U53488:00:00";
            //				UIImagePickerControllerMediaType = ALAssetTypePhoto;
            //				UIImagePickerControllerOriginalImage = "<UIImage: 0x155230b0>";
            //				UIImagePickerControllerReferenceURL = "assets-library://asset/asset.JPG?id=89AD2FB6-81CB-42E0-A98A-833D073AC69E&ext=JPG";
            //			}


            foreach (NSDictionary dict in info) {
                var nsObj = dict.ValueForKey (new NSString ("UIImagePickerControllerOriginalImage"));
                UIImage image = nsObj as UIImage;
                image = ImageByScalingToMaxSize (image, PhotoStackWidth);
                Images.Add (image);
            }
            PageControl.Pages = Images.Count;
//			PageControl.ReloadData ();
//            NSObject[] deleteImages = new NSObject[Images.Count];
//            for (int i = 0; i < Images.Count; i++) {
//                deleteImages [i] = Images [i];
//            }
//            PhotoStack.SetPhotoViews (deleteImages);
            PhotoStack.ReloadData ();
            PhotoStack.Enabled = true;
            this.DismissViewController (true, null);
        }

        void IELCImagePickerControllerDelegate.ECLImagePickerControllerDidCancel (ELCImagePickerController picker)
        {
            this.DismissViewController (true, null);
        }

        public  virtual int NumberOfPhotosInPhotoStackView (PhotoStackView photoStack)
        {
            return Images.Count;
        }

        public  virtual UIImage PhotoStackViewPhotoForIndex (PhotoStackView photoStack, int index)
        {
            return Images.Count != 0 ? Images [index] : null;
        }

        public  virtual SizeF PhotoStackViewPhotoSizeForIndex (PhotoStackView photoStack, int index)
        {
            return Images.Count != 0 ? Images [index].Size : new SizeF ();
//			return Images [index].Size;
        }

        public  virtual void PhotoStackViewWillStartMovingPhotoAtIndex (PhotoStackView photoStackView, int index)
        {
        }

        public  virtual void PhotoStackViewWillFlickAwayPhotoFromIndex (PhotoStackView photoStackView, int fromIndex, int toIndex)
        {
        }

        public  virtual void PhotoStackViewDidRevealPhotoAtIndex (PhotoStackView photoStackView, int index)
        {
            this.PageControl.CurrentPage = index;
        }

        public  virtual void PhotoStackViewDidSelectPhotoAtIndex (PhotoStackView photoStackView, int index)
        {
        }

        public UIImage ImageByScalingToMaxSize (UIImage sourceImage, float maxWithd)
        {
            if (sourceImage.Size.Width < maxWithd)
                return sourceImage;
            float btWidth = 0.0f;
            float btHeight = 0.0f;
            if (sourceImage.Size.Width < sourceImage.Size.Height) {
                btHeight = maxWithd;
                btWidth = sourceImage.Size.Width * (maxWithd / sourceImage.Size.Height);
            } else {
                btWidth = maxWithd;
                btHeight = sourceImage.Size.Height * (maxWithd / sourceImage.Size.Width);
            }
            SizeF targetSize = new SizeF (btWidth, btHeight);
            return this.imageByScalingAndCroppingForSourceImage (sourceImage, targetSize);
        }

        UIImage imageByScalingAndCroppingForSourceImage (UIImage sourceImage, SizeF targetSize)
        {
            UIImage newImage = null;
            SizeF imageSize = sourceImage.Size;
            float width = imageSize.Width;
            float height = imageSize.Height;
            float targetWidth = targetSize.Width;
            float targetHeight = targetSize.Height;
            float scaleFactor = 0.0f;
            float scaledWidth = targetWidth;
            float scaledHeight = targetHeight;
            PointF thumbnailPoint = new PointF (0.0f, 0.0f);
            if (imageSize == targetSize) {
                float widthFactor = targetWidth / width;
                float heightFactor = targetHeight / height;
        
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
            UIGraphics.BeginImageContext (targetSize); // this will crop
            RectangleF thumbnailRect = new RectangleF ();
        
            thumbnailRect.Location = thumbnailPoint;
            thumbnailRect.Width = scaledWidth;
            thumbnailRect.Height = scaledHeight;
        
            sourceImage.Draw (thumbnailRect);
        
            newImage = UIGraphics.GetImageFromCurrentImageContext ();
            if (newImage == null)
                Console.WriteLine (@"could not scale image");
        
            //pop the context to get back to the default
            UIGraphics.EndImageContext ();
            return newImage;
        }
        //		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
        //		{
        //			return UIInterfaceOrientationMask.Portrait;
        //		}

    }
}
//
//(UIImage*) getOneImageButtonWithArrow{
//    //tmpView做附控件
//    UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(0.0f, 0.0f, 38.0f, 32.0f)];
//    tmpView.backgroundColor = [UIColor clearColor];
//
//    //bgImg作为背景
//    UIImage* bgImg = [UIImage imageNamed:@"background.png"];
//    UIImageView *bgImageView = [[UIImageView alloc] initWithImage:bgImg];
//    bgImageView.frame = tmpView.frame;
//
//    //加入tmpView
//    [tmpView addSubview:bgImageView];
//    [bgImageView release];
//
//    //里面的icon
//    UIImageView *tmpImageView = [[UIImageView alloc] initWithImage:[UIImage imageNamed:@"icon.png"]];
//    tmpImageView.frame = CGRectMake(4.0f, 4.0f, 24.0f, 24.0f);
//    tmpImageView.backgroundColor = [UIColor clearColor];
//    tmpImageView.alpha = 1.0f;
//    [tmpView addSubview:tmpImageView];
//
//    //箭头
//    UIImage *arrowImage = [UIImage imageNamed:@"arrow.png"];
//    UIImageView *arrowImageView = [[UIImageView alloc] initWithImage:arrowImage];
//    arrowImageView.frame = CGRectMake(28.0f, 4.0f, 6.0f, 24.0f);
//    [tmpView addSubview:arrowImageView];
//
//    //获取上下文，得到这个UIImage
//    UIGraphicsBeginImageContextWithOptions(tmpView.bounds.size, NO, 0.0);
//    [tmpView.layer renderInContext:UIGraphicsGetCurrentContext()];
//    UIImage *tmpImage = UIGraphicsGetImageFromCurrentImageContext();
//    UIGraphicsEndImageContext();
//    return tmpImage;
//}