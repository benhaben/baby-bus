using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ELCImagePickerControllerBinding;
using MonoTouch.MobileCoreServices;
using System.Collections.Generic;

namespace ECLImagePickerControllerTest
{
	public partial class ECLImagePickerControllerTestViewController : UIViewController, IELCImagePickerControllerDelegate
	{
		public ECLImagePickerControllerTestViewController (IntPtr handle) : base (handle)
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
//			MonoTouch.ObjCRuntime.Class.ThrowOnInitFailure = false;

			var button = new UIButton (new RectangleF (10, 100, 100, 40));
			button.SetTitle ("click me now", UIControlState.Normal);
			button.BackgroundColor = UIColor.Blue;
			button.TouchUpInside += (object sender, EventArgs e) => {
				ELCImagePickerController elcPicker = new ELCImagePickerController ();
				elcPicker.MaximumImagesCount = 100; //Set the maximum number of images to select to 100
				elcPicker.ReturnsOriginalImage = true; //Only return the fullScreenImage, not the fullResolutionImage
				elcPicker.ReturnsImage = true; //Return UIimage if YES. If NO, only return asset location information
				elcPicker.OnOrder = true; //For multiple image selection, display and return order of selected images
				elcPicker.MediaTypes = new string[]{ UTType.Image };
				elcPicker.WeakELCImagePickerControllerDelegate = this;

				PresentViewController (elcPicker, true, null);
			};
			View.AddSubview (button);
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

			IList<UIImage> listImages = new List<UIImage> ();

			foreach (NSDictionary dict in info) {
				var nsObj = dict.ValueForKey (new NSString ("UIImagePickerControllerOriginalImage"));
				UIImage image = nsObj as UIImage;
				listImages.Add (image);
			}

		}

		void IELCImagePickerControllerDelegate.ECLImagePickerControllerDidCancel (ELCImagePickerController picker)
		{
			this.DismissViewController (true, null);
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
	}
}

