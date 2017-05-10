using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using CoreGraphics;
using Foundation;
using MWPhotoBrowserBinding;

using UIKit;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using UITouchShared;

namespace BabyBus.iOS
{
	public partial class NoticeDetailView : MvxBabyBusBaseAutoLayoutViewController
	{
		NoticeDetailViewModel _baseViewModel = null;

		public NoticeDetailView()
		{
			this.Request = new Cirrious.MvvmCross.ViewModels.MvxViewModelRequest(
				typeof(NoticeDetailViewModel), null, null, null
			);
			AddGestureWhenTap = false;
		}

		NoticeDetailHeaderView _noticeDetailHeaderView = null;

		public NoticeDetailHeaderView NoticeDetailHeader {
			get {
				if (_noticeDetailHeaderView == null) {
					_noticeDetailHeaderView = new NoticeDetailHeaderView();
					_noticeDetailHeaderView.BackgroundColor = MvxTouchColor.White;
				}
				return _noticeDetailHeaderView;
			}
		}

		NoticeDetailDataSource _noticeImageDataSource = null;

		public NoticeDetailDataSource NoticeImageDataSource {
			get {
				if (_noticeImageDataSource == null) {
					_noticeImageDataSource = new NoticeDetailDataSource(this);
					_noticeImageDataSource.ImageViewSize = new CGSize(70, 70);
				}
				return _noticeImageDataSource;
			}
			set {
				_noticeImageDataSource = value;
			}
		}

		MyUICollectionView _collectionView = null;

		public MyUICollectionView CollectionView {
			get {
				if (_collectionView == null) {
					// Flow Layout
					var flowLayout = new UICollectionViewFlowLayout() {
						HeaderReferenceSize = new CGSize(0, 0),
						SectionInset = new UIEdgeInsets(1, 1, 1, 1),
						ScrollDirection = UICollectionViewScrollDirection.Vertical,
						MinimumInteritemSpacing = 4, // minimum spacing between cells
						MinimumLineSpacing = 4, // minimum spacing between rows if ScrollDirection is Vertical or between columns if Horizontal
						ItemSize = new CGSize(70, 70)
					};

					//frame is setted in the SetUpConstrainLayout
					_collectionView = new MyUICollectionView(new CGRect(0, 0, 0, 0), flowLayout, View);
					_collectionView.ContentInset = new UIEdgeInsets(1, 1, 1, 1);
					_collectionView.BackgroundColor = UIColor.Clear;
					_collectionView.RegisterClassForCell(typeof(MWImageCollecntionViewCell), MWImageCollecntionViewCell.CellID);
					_collectionView.ShowsHorizontalScrollIndicator = false;

					_collectionView.Layer.BorderWidth = EasyLayout.BorderWidth;
					_collectionView.Layer.BorderColor = MvxTouchColor.Gray3.CGColor;
					_collectionView.Layer.CornerRadius = EasyLayout.CornerRadius;
					_collectionView.Layer.MasksToBounds = true;

				}
				return _collectionView;
			}
		}

		UITextView _content = null;

		public UITextView Content {
			get {
				if (_content == null) {
					_content = new UITextView();
					_content.TextAlignment = UITextAlignment.Left;
					_content.Editable = false;
					_content.TextContainerInset = new UIEdgeInsets(10, 10, 10, 10);
					_content.ScrollEnabled = false;
					_content.Font = EasyLayout.ContentFont;
					_content.BackgroundColor = MvxTouchColor.White;
					_content.TextColor = MvxTouchColor.Black1;
				}
				return _content;
			}
		}

		List<UIImage> _images = null;

		List<UIImage> Images {
			get { 
				if (_images == null) {
					_images = new List<UIImage>();
				}
				return _images;
			}
			set { 
				_images = value;
			}
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		void LoadImages()
		{
			if (_baseViewModel == null || _baseViewModel.Notice == null || _baseViewModel.Notice.ImageList == null) {
				this._heightConstrainCollectionView.Constant = 0;
				return;
			} else {
				var imagelist = _baseViewModel.Notice.ImageList;
				NoticeImageDataSource.SetImages(imagelist);
				CollectionView.Source = NoticeImageDataSource;
				this.CollectionView.ReloadData();
			}
		}

		protected override void SetBackgroundImage()
		{
			base.SetBackgroundImage();
			this.View.BackgroundColor = MvxTouchColor.White2;
		}

		void SetReadPersons()
		{
			if (BabyBusContext.RoleType == RoleType.Teacher
			    && (_baseViewModel.Notice.NoticeType == NoticeType.ClassCommon
			    || _baseViewModel.Notice.NoticeType == NoticeType.ClassHomework
			    )) {
				NoticeIndexViewElement.SetNoticeTypeColor(NoticeDetailHeader.TypeUILabel, _baseViewModel.Notice.NoticeType);
//                const string format = "已读：{0}人 》";
//                NoticeDetailHeader.ReadPersons.Text = string.Format(format, _baseViewModel.ReadedCount);
				NoticeDetailHeader.ReadPersons.Hidden = false;
			} else {
				NoticeIndexViewElement.SetNoticeTypeColor(NoticeDetailHeader.TypeUILabel, _baseViewModel.Notice.NoticeType);
				NoticeDetailHeader.ReadPersons.Hidden = true;
			}
		}

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();

			_baseViewModel = ViewModel as NoticeDetailViewModel;
			UIBarButtonItem backItem = new UIBarButtonItem();
			this.NavigationItem.BackBarButtonItem = backItem;

			var shareImage = UIImage.FromBundle("icon_share.png");

			var btnShare = new UIBarButtonItem(shareImage, UIBarButtonItemStyle.Plain
				, (sender, e) => {
				var share = new ShareModel {
					Id = _baseViewModel.NoticeId,
					Title = _baseViewModel.Notice.Title,
					ContentType = 2,
					Description = _baseViewModel.Notice.AbstractDisplayForiOS,
				};

				this.ShowSharedActionSheet(share);
			});
			NavigationItem.SetRightBarButtonItem(btnShare, true);

			_baseViewModel.FirstLoadedEventHandler += (sender, e) => this.InvokeOnMainThread(() => {
				// Perform any additional setup after loading the view, typically from a nib.
				var set = this.CreateBindingSet<NoticeDetailView, NoticeDetailViewModel>();
				set.Bind(NoticeDetailHeader.NoticeTitle).To(vm => vm.Notice.Title);
				set.Bind(Content).To(vm => vm.Notice.Content);
				set.Bind(NoticeDetailHeader.TeacherName).To(vm => vm.Notice.RealName);
				set.Bind(NoticeDetailHeader.DateLabel).To(vm => vm.Notice.CreateTime).WithConversion("DateTimeOffset");
				set.Bind(NoticeDetailHeader.ReadPersons).To(vm => vm.ReadedStatus);


				set.Apply();

				if (_baseViewModel.Notice != null) {
					nfloat heightChanged = Content.SizeThatFits(new CGSize(300, float.MaxValue)).Height;
					_heightConstrainContent.Constant = _heightConstrainContent.Constant > heightChanged ? _heightConstrainContent.Constant : heightChanged;

					nfloat heightChangedTitle = NoticeDetailHeader.SizeThatFits(new CGSize(320, float.MaxValue)).Height;
					_heightNoticeDetailHeaderView.Constant = _heightNoticeDetailHeaderView.Constant > heightChangedTitle ? _heightConstrainContent.Constant : heightChangedTitle;
				}
				LoadImages();
				SetReadPersons();
			});

			NoticeDetailHeader.TapReadPersons = _baseViewModel.ShowReadList.Execute;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			//            this.NavigationController.SetNavigationBarHidden (true, animated);
			//Stats Page Report
			StatsUtils.LogPageReport(BabyBusSSApi.ServiceModel.Enumeration.PageReportType.NoticeDetail);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
		}

		#endregion

		public override void PrepareViewHierarchy()
		{
			base.PrepareViewHierarchy();
			UIView[] v = {
				NoticeDetailHeader,
				Content,
				CollectionView
			};
			View.AddSubviews(v);
			Container.AddSubviews(v);
			this.View.SetNeedsLayout();
			this.View.LayoutIfNeeded();
			this.View.UpdateConstraintsIfNeeded();
			#if DEBUG1
            Content.BackgroundColor = UIColor.Red;
            CollectionView.BackgroundColor = UIColor.Green;
			#endif
		}

		public override void SetUpConstrainLayout()
		{
			base.SetUpConstrainLayout();
			nfloat PhotoStackHeight = EasyLayout.PhotoStackHeight;
			nfloat HeaderHeight = 100f;
			View.ConstrainLayout 
            (
                // Analysis disable CompareOfFloatsByEqualityOperator
				() => 

                NoticeDetailHeader.Frame.Left == Container.Frame.Left
				&& NoticeDetailHeader.Frame.Right == Container.Frame.Right
				&& NoticeDetailHeader.Frame.Top == Container.Frame.Top

				&& Content.Frame.Left == Container.Frame.Left
				&& Content.Frame.Right == Container.Frame.Right
				&& Content.Frame.Top == NoticeDetailHeader.Frame.Bottom + EasyLayout.MarginMedium

				&& CollectionView.Frame.Left == Container.Frame.Left + EasyLayout.MarginMedium
				&& CollectionView.Frame.Right == Container.Frame.Right - EasyLayout.MarginMedium
				&& CollectionView.Frame.Top == Content.Frame.Bottom + EasyLayout.MarginMedium

				&& Container.Frame.Bottom == CollectionView.Frame.Bottom
                // Analysis restore CompareOfFloatsByEqualityOperator
			);
			//Note: do not change frame, change Constrain to resize control
			var constrains =
				View.ConstrainLayout(
					() => Content.Frame.Height == EasyLayout.HeightOfContent
					&& CollectionView.Frame.Height == EasyLayout.HeightOfSelectImageCollection
					&& NoticeDetailHeader.Frame.Height == HeaderHeight
				);
			_heightConstrainContent = constrains[0];
			_heightConstrainCollectionView = constrains[1];
			_heightConstrainCollectionView.Constant += 3;
			_heightNoticeDetailHeaderView = constrains[2];
		}

		NSLayoutConstraint _heightConstrainContent;
		NSLayoutConstraint _heightConstrainCollectionView;
		NSLayoutConstraint _heightNoticeDetailHeaderView;

		public class NoticeDetailDataSource : UICollectionViewSource
		{
			UIViewController _vc;

			public NoticeDetailDataSource(UIViewController vc)
			{
				Images = new List<string>();
				_vc = vc;
			}

			private List<string> Images { get; set; }

			public int Count{ get { return  Images.Count; } }

			public void SetImages(List<string> list)
			{
				Images.Clear();
				Images = list;
			}

			public void InsertIntoHead(List<string> list)
			{
				Images.RemoveRange(0, Images.Count - 1);
				Images.InsertRange(0, list);
			}

			public CGSize ImageViewSize { get; set; }

			public override nint NumberOfSections(UICollectionView collectionView)
			{
				return 1;
			}

			public override nint GetItemsCount(UICollectionView collectionView, nint section)
			{
				return Images.Count;
			}

			public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
			{
                
				//show photo browser
				if (indexPath.Row >= Images.Count) {
					return false;
				}

				var imageList = Images;
				var firstImage = imageList[indexPath.Row];
				imageList.RemoveAt(indexPath.Row);
				imageList.Insert(0, firstImage);

				int number = imageList.Count;
				NSObject[] obj = new NSObject[number];
				for (int i = 0; i < number; i++) { 
					string url = Constants.ImageServerPath + imageList[i];
					var photo = MWPhoto.PhotoWithURL(new NSUrl(url));
					photo.Caption = "";
					obj[i] = photo;
				}

				MWPhotoBrowser browser = new MWPhotoBrowser(obj);
				browser.DisplayActionButton = true;
				browser.DisplayNavArrows = false;
				browser.DisplaySelectionButtons = false;
				browser.AlwaysShowControls = true;
				browser.ZoomPhotosToFill = true;
				browser.EnableGrid = false;
				browser.StartOnGrid = false;
				//                browser.m
				browser.EnableSwipeToDismiss = true;

				_vc.NavigationController.PushViewController(browser, true);
				return true;
			}

           
			public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
			{
				var cell = (MWImageCollecntionViewCell)collectionView.DequeueReusableCell(MWImageCollecntionViewCell.CellID, indexPath);
				if (indexPath.Row >= 0 && indexPath.Row < Images.Count) {
					string url = Constants.ThumbServerPath + Images[indexPath.Row] + Constants.ThumbRule;
					var uri = new Uri(url);
					//处理中文url
					var nsurl = new NSUrl(uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped));
					var photo = MWPhoto.PhotoWithURL(nsurl);
					photo.LoadUnderlyingImageAndNotify();
					cell.SetPhoto(photo);
				}
				return cell;
			}
		}
	}
	//class
}
//namespace

