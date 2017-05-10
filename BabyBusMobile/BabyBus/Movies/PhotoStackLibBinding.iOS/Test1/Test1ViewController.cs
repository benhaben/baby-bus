using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PhotoStackBinding;
using System.Collections.Generic;

namespace Test1
{
	public partial class Test1ViewController 
		: UIViewController
	, IPhotoStackViewDataSource
		, IPhotoStackViewDelegate
	{
		public Test1ViewController (IntPtr handle) : base (handle)
		{
		}

		public Test1ViewController ()
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
//			PageControl = null;
			//			self setPageControl:nil];
			//			[super viewDidUnload];
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View.BackgroundColor = UIColor.White;

			// Perform any additional setup after loading the view, typically from a nib.
			View.AddSubview (PhotoStack);
			PageControl.Pages = Photos.Count;
			//			[self.view addSubview:self.photoStack];
			//			self.pageControl.numberOfPages = [self.photos count];
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

		IList<UIImage> _images = null;
		UIPageControl _pageControl = null;

		UIPageControl PageControl {
			get { 
				if (_pageControl == null) {
					_pageControl = new UIPageControl (new RectangleF (20f, 410f, 280f, 36f));
					_pageControl.BackgroundColor = UIColor.Brown;
				}
				_pageControl.Center = new PointF (this.View.Center.X, 400);
				return _pageControl;
			}
//			set { 
//				_pageControl = value;
//			}
		}

		IList<UIImage> Photos {
			get {
				if (_images == null) {
					_images = new List<UIImage> ();
					_images.Add (UIImage.FromBundle ("gao.jpg"));
					_images.Add (UIImage.FromBundle ("zhou.jpg"));
//					_images.Add (UIImage.FromBundle ("photo3.jpg"));
//					_images.Add (UIImage.FromFile ("photo4.jpg"));
//					_images.Add (UIImage.FromFile ("photo5.jpg"));
//					_images.Add (UIImage.FromFile ("photo6.jpg"));
					_images.Add (null);
				}
				return _images;
			}

		}

		PhotoStackView _photoStack = null;

		PhotoStackView PhotoStack {
			get {
				if (_photoStack == null) {   
					_photoStack = new PhotoStackView ();
					_photoStack.Frame = new RectangleF (0f, 0f, 100f, 100f);
					_photoStack.Center = new PointF (this.View.Center.X, 170);
					_photoStack.WeakPhotoStackViewDataSourceDelegate = this;
					_photoStack.WeakPhotoStackViewDelegate = this;
//					_photoStack.BackgroundColor = UIColor.Blue;
				}

				return _photoStack;
			}
		}

		public  virtual int NumberOfPhotosInPhotoStackView (PhotoStackView photoStack)
		{
			return Photos.Count;
		}



		public  virtual UIImage PhotoStackViewPhotoForIndex (PhotoStackView photoStack, int index)
		{
			return Photos [index];
		}

		//		public  virtual SizeF PhotoStackViewPhotoSizeForIndex (PhotoStackView photoStack, int index)
		//		{
		//			return Photos [index].Size;
		//			//			return [self.photos objectAtIndex:index];
		//		}

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

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Portrait;
		}

	}
}

