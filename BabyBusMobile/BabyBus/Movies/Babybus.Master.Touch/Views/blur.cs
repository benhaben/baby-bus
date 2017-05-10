using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace ViewEffectsDemo
{
	public class EffectsController : UIViewController
	{
		UIImageView imageView;
		UIScrollView scrollView;
		UILabel label;
//		public static UIImage Blur(this UIImage image, float blurRadius = 25f)
//		{
//			if (image != null)
//			{
//				// Create a new blurred image.
//				var inputImage = new CIImage (image);
//				var blur = new CIGaussianBlur ();
//				blur.Image = inputImage;
//				blur.Radius = blurRadius;
//
//				var outputImage = blur.OutputImage;
//				var context = CIContext.FromOptions (new CIContextOptions { UseSoftwareRenderer = false });
//				var cgImage = context.CreateCGImage (outputImage, new RectangleF (new PointF (0, 0), image.Size));
//				var newImage = UIImage.FromImage (cgImage);
//
//				// Clean up
//				inputImage.Dispose ();
//				context.Dispose ();
//				blur.Dispose ();
//				outputImage.Dispose ();
//				cgImage.Dispose ();
//
//				return newImage;
//			}
//			return null;
//		}
//
		public override void ViewDidLoad ()
		{  
			imageView = new UIImageView ();

			using (var image = UIImage.FromFile ("login_background.png")) {
				imageView.Image = image;
				imageView.Frame = new RectangleF (0, 0, image.Size.Width, image.Size.Height);
			}

			scrollView = new UIScrollView (View.Frame) {
				ContentSize = imageView.Image.Size
			};

			scrollView.Add (imageView);

			View.Add (scrollView);

			// blur view
			var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Light);
			var blurView = new UIVisualEffectView (blur) {
				Frame = new RectangleF (0, 0, imageView.Frame.Width, 400)
			};

			View.Add (blurView);

			// vibrancy view
			var frame = new Rectangle (10, 10, 100, 50);  
			var vibrancy = UIVibrancyEffect.FromBlurEffect (blur);
			var vibrancyView = new UIVisualEffectView (vibrancy) {
				Frame = frame
			};

			label = new UILabel {
				Text = "Hello iOS 8!",
				Frame = vibrancyView.Bounds
			};
			UIButton button = new UIButton {
				Frame = vibrancyView.Bounds
			};
			button.SetTitle("sssss", UIControlState.Normal);


			vibrancyView.ContentView.Add (label);
			vibrancyView.ContentView.Add (button);

			blurView.ContentView.Add (vibrancyView);
		}
	}
}