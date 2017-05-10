using System;
using System.Collections.Generic;
using BabyBus.iOS;
using Cirrious.CrossCore;
using CoreGraphics;
using UIKit;
using BabyBus.Logic.Shared;
using Foundation;
using SDWebImage;

namespace BabyBus.iOS
{
	//use binding instead
	public class ScrollAdvertise : UIControl,IUIScrollViewDelegate
	{

		public ScrollAdvertise(IList<string> names)
		{
			_slideImagesName.AddRange(names);
			IsLoadFromBundle = true;
			Height = (float)(EasyLayout.AdvertiseBarHeight - 15);
		}

		public ScrollAdvertise(IList<string> names, bool isLoadFromBundle)
		{
			_slideImagesName.AddRange(names);
			IsLoadFromBundle = isLoadFromBundle;
			Height = (float)(EasyLayout.AdvertiseBarHeight - 15);
		}

		//        @synthesize scrollView, slideImages;
		//        @synthesize text;
		//        @synthesize pageControl;

		UIScrollView _scrollView;
		UIPageControl _pageControl;
		List<string> _slideImagesName = new List<string>();

		public void AddImageNames(List<string> list)
		{
			_slideImagesName.Clear();
			_slideImagesName.AddRange(list);
			this.Setup();
			_pageControl.Pages = _slideImagesName.Count;
		}

		//		public void AddImages(List<UIImage> images)
		//		{
		//			_slideImagesName.Clear();
		//			_slideImages.Clear();
		//			_slideImages.AddRange(images);
		//			this.Setup();
		//			_pageControl.Pages = _slideImages.Count;
		//		}

		public bool IsLoadFromBundle{ get; set; }

		public float Height{ get; set; }

		public UIScrollView ScrollView {
			get {
				if (_scrollView == null) {
					_scrollView = new UIScrollView(new CGRect(0, 0, 320, Height));
					_scrollView.Bounces = true;
					_scrollView.AlwaysBounceVertical = false;
					_scrollView.PagingEnabled = true;
					_scrollView.Scrolled += Scrolled;
					_scrollView.DecelerationEnded += DecelerationEnded;
					_scrollView.UserInteractionEnabled = true;
					_scrollView.ShowsHorizontalScrollIndicator = false;
					_scrollView.ShowsVerticalScrollIndicator = false;
				}
				return _scrollView;
			}
			set {
				_scrollView = value;
			}
		}

		public UIPageControl PageControl {
			get {
				if (_pageControl == null) {
					_pageControl = new UIPageControl(new CGRect(110, Height, 100, 16));
					_pageControl.Pages = _slideImagesName.Count;
//                    _pageControl.BackgroundColor = MvxTouchColor.White;
					_pageControl.CurrentPage = 0;
					_pageControl.ValueChanged += (object sender, EventArgs e) => TurnPage();
				}
				return _pageControl;
			}
			set {
				_pageControl = value;
			}
		}

		void Setup()
		{
			var width = Frame.Width;
			List<UIImage> slideImages = new List<UIImage>();
			if (IsLoadFromBundle) {
				foreach (var name in _slideImagesName) {
					UIImage image;
					image = UIImage.FromBundle(name);
					slideImages.Add(image);
				}
			}

			//reset
			foreach (var view in ScrollView.Subviews) {
				view.RemoveFromSuperview();
			}

			// 创建四个图片 imageview
			for (int index = 0; index < _slideImagesName.Count; index++) {
				UIImageView imageView = GetIamgeView(slideImages, index);
				// 首页是第0页,默认从第1页开始的。所以+320。。。
				var x = width * index + width;
				imageView.Frame = new CGRect(x, 0, width, Height);
//                imageView.ContentMode = UIViewContentMode.Center;
				ScrollView.AddSubview(imageView);
			}
			// 取数组最后一张图片 放在第0页
			{
				UIImageView imageView = GetIamgeView(slideImages, _slideImagesName.Count - 1);
				imageView.Frame = new CGRect(0, 0, width, Height);
//                imageView.ContentMode = UIViewContentMode.Center;
				// 添加最后1页在首页 循环
				ScrollView.AddSubview(imageView);
			}
			// 取数组第一张图片 放在最后1页
			{
				UIImageView imageView = GetIamgeView(slideImages, 0);
				imageView.Frame = new CGRect((width * (_slideImagesName.Count + 1)), 0, width, Height);
//                imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
				// 添加第1页在最后 循环
				ScrollView.AddSubview(imageView);
			}
			//example 4 page:  +上第1页和第4页  原理：4-[1-2-3-4]-1
			//set height 0, vertical scroll disable
			ScrollView.ContentSize = new CGSize(width * (_slideImagesName.Count + 2), Height);
			ScrollView.SetContentOffset(new CGPoint(0, 0), false);
			ScrollView.ScrollRectToVisible(new CGRect(width, 0, width, Height), false);
		}

		UIImageView GetIamgeView(List<UIImage> slideImages, int index)
		{
			if (IsLoadFromBundle) {
				var imageView = new UIImageView(slideImages[index]);
				return imageView;
			} else {
				var imageView = new UIImageView();
				var imageName = Constants.ImageServerPath + _slideImagesName[index];
				var uri = new Uri(imageName);
				var nsurl = new NSUrl(uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped));
				imageView.SetImage(nsurl);
				return imageView;
			}
		}



    

		public virtual void Scrolled(object o, EventArgs e)
		{
			var width = Frame.Width;
			int page = (int)(ScrollView.ContentOffset.X / width); // 和上面两行效果一样
			page--;  // 默认从第二页开始
			Mvx.Trace("Scrolled Page {0}", page);
//            if (page < 0) {
//                page = this._slideImages.Count - 1;
//            } else if (page == this._slideImages.Count) {
//                page = 0;
//            }
			PageControl.CurrentPage = page;
		}
		//        [Export("scrollViewDidEndDecelerating:"), CompilerGenerated]
		public virtual void DecelerationEnded(object o, EventArgs e)
		{
			var pagewidth = ScrollView.Frame.Size.Width;
			var width = Frame.Width;
			int currentPage = (int)(ScrollView.ContentOffset.X / width); // 和上面两行效果一样
			if (currentPage == 0) {
				// 序号0 最后1页
				ScrollView.ScrollRectToVisible(new CGRect(width * _slideImagesName.Count, 0, width, Height), false);
			} else if (currentPage == _slideImagesName.Count + 1) {
				ScrollView.ScrollRectToVisible(new CGRect(width, 0, width, Height), false);
			}
		}

		//TODO: no use
		void TurnPage()
		{
			var width = Frame.Width;
			var page = PageControl.CurrentPage; // 获取当前的page
			ScrollView.ScrollRectToVisible(new CGRect(width * (page + 1), 0, width, Height), false);
		}

		public override void LayoutSubviews()
		{
			UIView[] v = {
				ScrollView,
				PageControl,
			};
			AddSubviews(v);
			base.LayoutSubviews();

			Setup();
			_pageControl.Pages = _slideImagesName.Count;
		}

       
	}
}

