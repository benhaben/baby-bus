using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using CoreGraphics;
using SDWebImage;
using AddressBook;
using Cirrious.CrossCore.Platform;

namespace PhotoBrowser {
    public interface IMWPhotoBrowserDelegate {
        int NumberOfPhotosInPhotoBrowser(MWPhotoBrowser photoBrowser);

        MWPhoto PhotoAtIndex(MWPhotoBrowser photoBrowser, int index);

        MWPhoto ThumbPhotoAtIndex(MWPhotoBrowser photoBrowser, int index);

        MWCaptionView CaptionViewForPhotoAtIndex(MWPhotoBrowser photoBrowser, int index);

        string TitleForPhotoAtIndex(MWPhotoBrowser photoBrowser, int index);

        void DidDisplayPhotoAtIndex(MWPhotoBrowser photoBrowser, int index);

        void ActionButtonPressedForPhotoAtIndex(MWPhotoBrowser photoBrowser, int index);

        bool IsPhotoSelectedAtIndex(MWPhotoBrowser photoBrowser, int index);

        void PhotoAtIndexSelectedChanged(MWPhotoBrowser photoBrowser,
                                         int index, bool selected);

        void PhotoBrowserDidFinishModalPresentation(MWPhotoBrowser photoBrowser);

    }

    public class MWPhotoBrowser : UIViewController, IUIScrollViewDelegate {

        #region MWPhotoBrowser properties

        IMWPhotoBrowserDelegate _delegate;

        public int NumberOfPhotos {
            get;
            set;
        }

        public IMWPhotoBrowserDelegate PhotoBrowserDelegate {
            get {
                return _delegate;
            }
            set {
                _delegate = value;
            }
        }

        bool _zoomPhotosToFill;

        public bool ZoomPhotosToFill {
            get {
                return _zoomPhotosToFill;
            }
            set {
                _zoomPhotosToFill = value;
            }
        }

        bool _displayNavArrows;

        public bool DisplayNavArrows {
            get {
                return _displayNavArrows;
            }
            set {
                _displayNavArrows = value;
            }
        }

        bool _displayActionButton;

        public bool DisplayActionButton {
            get {
                return _displayActionButton;
            }
            set {
                _displayActionButton = value;
            }
        }

        bool _displaySelectionButtons;

        public bool DisplaySelectionButtons {
            get {
                return _displaySelectionButtons;
            }
            set {
                _displaySelectionButtons = value;
            }
        }

        bool _alwaysShowControls;

        public bool AlwaysShowControls {
            get {
                return _alwaysShowControls;
            }
            set {
                _alwaysShowControls = value;
            }
        }

        bool _enableGrid;

        public bool EnableGrid {
            get {
                return _enableGrid;
            }
            set {
                _enableGrid = value;
            }
        }

        bool _enableSwipeToDismiss;

        public bool EnableSwipeToDismiss {
            get {
                return _enableSwipeToDismiss;
            }
            set {
                _enableSwipeToDismiss = value;
            }
        }

        bool _startOnGrid;

        public bool StartOnGrid {
            get {
                return _startOnGrid;
            }
            set {
                _startOnGrid = value;
            }
        }

        int _delayToHideElements;

        public int DelayToHideElements {
            get {
                return _delayToHideElements;
            }
            set {
                _delayToHideElements = value;
            }
        }

        IList<MWPhoto> _depreciatedPhotoData;
        bool _hasBelongedToViewController;
        const int NSNotFound = -1;
        int _photoCount = NSNotFound;
        CGRect _previousLayoutBounds;
        int _currentPageIndex;
        int _previousPageIndex;
        bool _performingLayout;
        bool _rotating;
        bool _viewIsActive;
        List<MWZoomingScrollView> _visiblePages;
        List<MWZoomingScrollView> _recycledPages;
        List<MWPhoto> _photos;
        List<MWPhoto> _thumbPhotos;
        CGPoint _currentGridContentOffset;
        bool _didSavePreviousStateOfNavBar;

        // Views
        UIScrollView _pagingScrollView;
        UIToolbar _toolbar;
        UIBarButtonItem _doneButton;

        // Appearance
        bool _previousNavBarHidden;
        bool _previousNavBarTranslucent;
        UIBarStyle _previousNavBarStyle;
        UIStatusBarStyle _previousStatusBarStyle;
        UIColor _previousNavBarTintColor;
        UIColor _previousNavBarBarTintColor;
        UIBarButtonItem _previousViewControllerBackButton;
        UIImage _previousNavigationBarBackgroundImageDefault;
        UIImage _previousNavigationBarBackgroundImageLandscapePhone;

        #endregion

        #region init

        void ReloadData() {

            // Reset
            _photoCount = NSNotFound;

            // Get data
            int numberOfPhotos = this.NumberOfPhotos;
            this.ReleaseAllUnderlyingPhotos(true);
            _photos.Clear();
            _thumbPhotos.Clear();
            for (int i = 0; i < numberOfPhotos; i++) {
                _photos.Add(null);
                _thumbPhotos.Add(null);
            }

            // Update current page index
            if (numberOfPhotos > 0) {
                _currentPageIndex = Math.Max(0, Math.Min(_currentPageIndex, numberOfPhotos - 1));
            } else {
                _currentPageIndex = 0;
            }

            // Update layout
            if (this.IsViewLoaded) {
                while (_pagingScrollView.Subviews.Length > 0) {
                    _pagingScrollView.Subviews[_pagingScrollView.Subviews.Length - 1].RemoveFromSuperview();
                }
                this.PerformLayout();
                this.View.SetNeedsLayout();
            }

        }

        void Initialisation() {
            this.HidesBottomBarWhenPushed = true;
            _hasBelongedToViewController = false;
            _photoCount = NSNotFound;
            _previousLayoutBounds = CGRect.Empty;
            _currentPageIndex = 0;
            _previousPageIndex = int.MaxValue;
            _displayActionButton = true;
            _displayNavArrows = false;
            _zoomPhotosToFill = true;
            _performingLayout = false;// Reset on view did appear
            _rotating = false;
            _viewIsActive = false;
            _enableGrid = true;
            _startOnGrid = false;
            _enableSwipeToDismiss = true;
            _delayToHideElements = 5;
            _visiblePages = new List<MWZoomingScrollView>();
            _recycledPages = new List<MWZoomingScrollView>();
            _photos = new List<MWPhoto>();
            _thumbPhotos = new List<MWPhoto>();
            _currentGridContentOffset = new CGPoint(0, float.MaxValue);
            _didSavePreviousStateOfNavBar = false;
            this.AutomaticallyAdjustsScrollViewInsets = false;
            RegisterNotifications();

        }

        List<NSObject> _eventListeners;

        void RegisterNotifications() {
            if (_eventListeners == null) {
                _eventListeners = new List<NSObject>();
            }
            // Listen for photo loading notifications
            _eventListeners.Add(NSNotificationCenter.DefaultCenter.AddObserver(
                MWPhoto.MWPHOTO_LOADING_DID_END_NOTIFICATION, HandleMWPhotoLoadingDidEndNotification));
        }

        void UnRegisterNotifications() {
            if (_eventListeners != null) {
                NSNotificationCenter.DefaultCenter.RemoveObservers(_eventListeners);
                _eventListeners.Clear();
                _eventListeners = null;
            }
        }

        protected override void Dispose(bool disposing) {
            UnRegisterNotifications();
            _pagingScrollView = null;
            this.ReleaseAllUnderlyingPhotos(false);
            SDImageCache.SharedImageCache().ClearMemory();
        }


        void ReleaseAllUnderlyingPhotos(bool preserveCurrent) {
            // Create a copy in case this array is modified while we are looping through
            // Release photos
            var copy = new List<MWPhoto>(_photos);
            foreach (var p in copy) {
                if (p != null) {
                    if (preserveCurrent && p == this.PhotoAtIndex(this._currentIndex)) {
                        continue;
                    }
                    p.UnloadUnderlyingImage();
                }
            }

            copy = new List<MWPhoto>(_thumbPhotos);
            foreach (var p in copy) {
                if (p != null) {
                    p.UnloadUnderlyingImage();
                }
            }

        }

        public MWPhotoBrowser() {
            Initialisation();
        }

        public MWPhotoBrowser(IMWPhotoBrowserDelegate d) {
            _delegate = d;
        }

        public MWPhotoBrowser(IList<MWPhoto> photosArray) {
            _depreciatedPhotoData = photosArray;
        }

        public override void DidReceiveMemoryWarning() {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
            this.ReleaseAllUnderlyingPhotos(true);
            _recycledPages.Clear();
        }

        #endregion

        #region View Loading

        // Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
        public override void ViewDidLoad() {
            base.ViewDidLoad();
            // Validate grid settings
            if (_startOnGrid) {
                _enableGrid = true;
            } else {
                _enableGrid = false;
            }

            // View
            this.View.BackgroundColor = UIColor.Black;
            this.View.ClipsToBounds = true;

            // Setup paging scrolling view
            CGRect pagingScrollViewFrame = this.FrameForPagingScrollView();
            _pagingScrollView = new UIScrollView(pagingScrollViewFrame);
            _pagingScrollView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            _pagingScrollView.PagingEnabled = true;
            _pagingScrollView.Delegate = this;
//            _pagingScrollView.ViewForZoomingInScrollView
            _pagingScrollView.ShowsHorizontalScrollIndicator = false;
            _pagingScrollView.ShowsVerticalScrollIndicator = false;
            _pagingScrollView.BackgroundColor = UIColor.Black;
            _pagingScrollView.ContentSize = this.ContentSizeForPagingScrollView();
            this.View.AddSubview(_pagingScrollView);

            //toolbar
            _toolbar = new UIToolbar(this.FrameForToolbarAtOrientation(this.InterfaceOrientation));
            _toolbar.TintColor = UIColor.White;
            _toolbar.BarTintColor = null;
            _toolbar.SetBackgroundImage(null, UIToolbarPosition.Any, UIBarMetrics.Default);
            _toolbar.SetBackgroundImage(null, UIToolbarPosition.Any, UIBarMetrics.LandscapePhone);
            _toolbar.BarStyle = UIBarStyle.BlackTranslucent;
            _toolbar.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;

            this.ReloadData();
           
            //TODO: don't have toolbar items and swipe to dismiss 
        }

        void PerformLayout() {
            //setup
            _performingLayout = true;
            var numberOfPhotos = this.NumberOfPhotos;

            // Setup pages
            _visiblePages.Clear();
            _recycledPages.Clear();

            // Navigation buttons
            if (this.NavigationController.ViewControllers[0] == this) {
                // We're first on stack so show done button
                _doneButton = new UIBarButtonItem("完成", UIBarButtonItemStyle.Plain, DoneButtonPressed);
                _doneButton.SetBackgroundImage(null, UIControlState.Normal, UIBarMetrics.Default);
                _doneButton.SetBackgroundImage(null, UIControlState.Normal, UIBarMetrics.LandscapePhone);
                _doneButton.SetBackgroundImage(null, UIControlState.Highlighted, UIBarMetrics.Default);
                _doneButton.SetBackgroundImage(null, UIControlState.Highlighted, UIBarMetrics.LandscapePhone);
                _doneButton.SetTitleTextAttributes(null, UIControlState.Normal);
                _doneButton.SetTitleTextAttributes(null, UIControlState.Highlighted);
                this.NavigationItem.RightBarButtonItem = _doneButton;
            } else {
                // We're not first so show back button
                UIViewController previousViewController = 
                    this.NavigationController.ViewControllers[this.NavigationController.ViewControllers.Length - 2];

                string backButtonTitle = previousViewController.NavigationItem.BackBarButtonItem != null ? 
                    previousViewController.NavigationItem.BackBarButtonItem.Title : previousViewController.Title;

                UIBarButtonItem newBackButton = new UIBarButtonItem(backButtonTitle, UIBarButtonItemStyle.Plain, null, null);
                // Appearance
                newBackButton.SetBackgroundImage(null, UIControlState.Normal, UIBarMetrics.Default);
                newBackButton.SetBackgroundImage(null, UIControlState.Normal, UIBarMetrics.LandscapePhone);
                newBackButton.SetBackgroundImage(null, UIControlState.Highlighted, UIBarMetrics.Default);
                newBackButton.SetBackgroundImage(null, UIControlState.Highlighted, UIBarMetrics.LandscapePhone);
                newBackButton.SetTitleTextAttributes(null, UIControlState.Normal);
                newBackButton.SetTitleTextAttributes(null, UIControlState.Highlighted);

                _previousViewControllerBackButton = previousViewController.NavigationItem.BackBarButtonItem; // remember previous
                previousViewController.NavigationItem.BackBarButtonItem = newBackButton;
            }

            //TODO: do not have tool bar items

            this.UpdateNavigation();

            // Content offset
            _pagingScrollView.ContentOffset = this.ContentOffsetForPageAtIndex(_currentPageIndex);
            this.TilePages();
            _performingLayout = false;
        }

        public override void ViewDidUnload() {
            base.ViewDidUnload();
            _currentPageIndex = 0;
            _pagingScrollView = null;
            _visiblePages = null;
            _recycledPages = null;
            _toolbar = null;
            //            _progressHUD = nil; don't get from web
        }

        public override void ViewWillAppear(bool animated) {
            base.ViewWillAppear(animated);
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, animated);

            if (!_viewIsActive && this.NavigationController.ViewControllers[0] != this) {
                this.StorePreviousNavBarAppearance();
            }
            this.SetNavBarAppearance(animated);
            // Update UI
            this.HideControlsAfterDelay();

            // Initial appearance
            if (!_viewHasAppearedInitially) {
                if (_startOnGrid) {
                    this.ShowGrid(false);
                }
                _viewHasAppearedInitially = true;
            }

        }

        bool _viewHasAppearedInitially;

        public override void ViewWillDisappear(bool animated) {
            base.ViewWillDisappear(animated);
            // Check that we're being popped for good
            if (this.NavigationController.ViewControllers[0] != this) {
                bool contains = false;
                foreach (var view in this.NavigationController.ViewControllers) {
                    if (view == this) {
                        contains = true;
                        break;
                    }
                }
                if (!contains) {
                    // State
                    _viewIsActive = false;

                    // Bar state / appearance
                    this.RestorePreviousNavBarAppearance(animated);
                } 
            }

            // Controls
            NavigationController.NavigationBar.Layer.RemoveAllAnimations(); // Stop all animations on nav bar
            NSObject.CancelPreviousPerformRequest(this);
            this.SetControlsHidden(false, false, true);

            // Status bar
            bool fullScreen = true;
        }

        public override void ViewDidAppear(bool animated) {
            base.ViewDidAppear(animated);
            _viewIsActive = true;
        }

        public override void WillMoveToParentViewController(UIViewController parent) {
            base.WillMoveToParentViewController(parent);
            //TODO: throw exception here
        }

        void SetNavBarAppearance(bool animated) {
            this.NavigationController.SetNavigationBarHidden(false, animated);
            UINavigationBar navBar = this.NavigationController.NavigationBar;
            navBar.TintColor = UIColor.White;
            navBar.BarTintColor = null;
            navBar.ShadowImage = null;
            navBar.Translucent = true;
            navBar.BarStyle = UIBarStyle.BlackTranslucent;
            navBar.SetBackgroundImage(null, UIBarMetrics.Default);
            navBar.SetBackgroundImage(null, UIBarMetrics.LandscapePhone);
        }

        void StorePreviousNavBarAppearance() {
            _didSavePreviousStateOfNavBar = true;
            _previousNavBarBarTintColor = this.NavigationController.NavigationBar.BarTintColor;
            _previousNavBarTranslucent = this.NavigationController.NavigationBar.Translucent;
            _previousNavBarTintColor = this.NavigationController.NavigationBar.TintColor;
            _previousNavBarHidden = this.NavigationController.NavigationBarHidden;
            _previousNavBarStyle = this.NavigationController.NavigationBar.BarStyle;
            _previousNavigationBarBackgroundImageDefault = this.NavigationController.NavigationBar.GetBackgroundImage(UIBarMetrics.Default);
            _previousNavigationBarBackgroundImageLandscapePhone = this.NavigationController.NavigationBar.GetBackgroundImage(UIBarMetrics.LandscapePhone);
        }

        void RestorePreviousNavBarAppearance(bool animated) {
            if (_didSavePreviousStateOfNavBar) {
                this.NavigationController.SetNavigationBarHidden(_previousNavBarHidden, animated);
                UINavigationBar navBar = this.NavigationController.NavigationBar;
                navBar.TintColor = _previousNavBarTintColor;
                navBar.Translucent = _previousNavBarTranslucent;
                navBar.BarTintColor = _previousNavBarBarTintColor;
                navBar.BarStyle = _previousNavBarStyle;

                navBar.SetBackgroundImage(_previousNavigationBarBackgroundImageDefault, UIBarMetrics.Default);
                navBar.SetBackgroundImage(_previousNavigationBarBackgroundImageLandscapePhone, UIBarMetrics.LandscapePhone);

                // Restore back button if we need to
                if (_previousViewControllerBackButton != null) {
                    UIViewController previousViewController = NavigationController.TopViewController; // We've disappeared so previous is now top
                    previousViewController.NavigationItem.BackBarButtonItem = _previousViewControllerBackButton;
                    _previousViewControllerBackButton = null;
                }
            }
        }

        #endregion

        #region Layout

        public override void ViewWillLayoutSubviews() {
            base.ViewWillLayoutSubviews();
            this.LayoutVisiblePages();
        }


        void LayoutVisiblePages() {
            // Flag
            _performingLayout = true;

            // Toolbar
            _toolbar.Frame = this.FrameForToolbarAtOrientation(this.InterfaceOrientation);

            // Remember index
            int indexPriorToLayout = _currentPageIndex;

            // Get paging scroll view frame to determine if anything needs changing
            CGRect pagingScrollViewFrame = this.FrameForPagingScrollView();

            // Frame needs changing
            if (!_skipNextPagingScrollViewPositioning) {
                _pagingScrollView.Frame = pagingScrollViewFrame;
            }
            _skipNextPagingScrollViewPositioning = false;

            // Recalculate contentSize based on current orientation
            _pagingScrollView.ContentSize = this.ContentSizeForPagingScrollView();

            // Adjust frames and configuration of each visible page
            foreach (MWZoomingScrollView page in _visiblePages) {
                int index = page.Index;
                page.Frame = this.FrameForPageAtIndex(index);
                if (page.CaptionView != null) {
                    page.CaptionView.Frame = this.FrameForCaptionView(page.CaptionView, index);
                }
                if (page.SelectedButton != null) {
                    page.SelectedButton.Frame = this.FrameForSelectedButton(page.SelectedButton, index);
                }

                // Adjust scales if bounds has changed since last time
                if (_previousLayoutBounds.X == this.View.Bounds.X &&
                    _previousLayoutBounds.Y == this.View.Bounds.Y &&
                    _previousLayoutBounds.Width == this.View.Bounds.Width &&
                    _previousLayoutBounds.Height == this.View.Bounds.Height) {
                    // Update zooms for new bounds
                    page.SetMaxMinZoomScalesForCurrentBounds();
                    _previousLayoutBounds = this.View.Bounds;
                }

            }

            // Adjust contentOffset to preserve page location based on values collected prior to location
            _pagingScrollView.ContentOffset = this.ContentOffsetForPageAtIndex(indexPriorToLayout);
            this.DidStartViewingPageAtIndex(_currentPageIndex); // initial

            // Reset
            _currentPageIndex = indexPriorToLayout;
            _performingLayout = false;
        }

        bool _skipNextPagingScrollViewPositioning;

        #endregion

        #region Rotation

        //        [Export("shouldAutorotateToInterfaceOrientation:"), Availability(Introduced = Platform.iOS_2_0, Deprecated = Platform.iOS_6_0, Message = "Use both GetSupportedInterfaceOrientations and PreferredInterfaceOrientationForPresentation instead."), CompilerGenerated]
        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation) {
            return false;
        }

        //        [Export("supportedInterfaceOrientations"), Availability(Introduced = Platform.iOS_6_0), CompilerGenerated]
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations() {
            return UIInterfaceOrientationMask.Portrait;
        }

        #endregion

        #region data


        int _currentIndex;

        public int CurrentIndex {
            get {
                return _currentIndex;
            }
            set {
                _currentIndex = value;
            }
        }

        void ReleadData() {

            //reset
            _photoCount = NSNotFound;

            //get data
            int numberOfPhotos = this.NumberOfPhotos;
            this.ReleaseAllUnderlyingPhotos(true);
            _photos.Clear();
            _thumbPhotos.Clear();

            // Update current page index
            if (numberOfPhotos > 0) {
                _currentPageIndex = Math.Max(0, Math.Min(_currentPageIndex, numberOfPhotos - 1));
            } else {
                _currentPageIndex = 0;
            }

            // Update layout
            if (this.IsViewLoaded) {
                while (_pagingScrollView.Subviews.Length > 0) {
                    _pagingScrollView.Subviews[_pagingScrollView.Subviews.Length - 1].RemoveFromSuperview();
                }
                this.PerformLayout();
                this.View.SetNeedsLayout();
            }
        }

        int numberOfPhotos() {
            if (_photoCount == NSNotFound) {
                _photoCount = _delegate.NumberOfPhotosInPhotoBrowser(this);
                if (_photoCount == NSNotFound && _depreciatedPhotoData != null) {
                    _photoCount = _depreciatedPhotoData.Count;
                }
            } 

            if (_photoCount == NSNotFound) {
                _photoCount = 0;
            }
            return _photoCount;
        }

        MWPhoto PhotoAtIndex(int index) {
            MWPhoto photo = null;
            if (index < _photos.Count) {
                if (_photos[index] == null) {
                    photo = _delegate.PhotoAtIndex(this, index);
                    //TODO: check this. what's the _depreciatedPhotoData
                    if (photo == null) {
                        photo = _depreciatedPhotoData[index];
                    }
                    if (photo != null) {
                        _photos[index] = photo;
                    }
                } else {
                    photo = _photos[index];
                }
            }
            return photo;
        }

        public MWPhoto ThumbPhotoAtIndex(int index) {
            MWPhoto photo = null;
            if (index < _thumbPhotos.Count) {
                if (_thumbPhotos[index] == null) {
                    photo = _delegate.ThumbPhotoAtIndex(this, index);
                    if (photo != null) {
                        _thumbPhotos[index] = photo;
                    }
                } else {
                    photo = _thumbPhotos[index];
                }
            }
            return photo;
        }

        MWCaptionView CaptionViewForPhotoAtIndex(int index) {
            MWCaptionView captionView = null;
            captionView = _delegate.CaptionViewForPhotoAtIndex(this, index);
            if (captionView == null) {
                MWPhoto photo = this.PhotoAtIndex(index);
                if (string.IsNullOrWhiteSpace(photo.Caption)) {
                    captionView = new MWCaptionView(photo);
                }
            }
            captionView.Alpha = this.AreControlsHidden ? 0 : 1;
            return captionView;

            //        - (MWCaptionView *)captionViewForPhotoAtIndex:(NSUInteger)index {
            //            MWCaptionView *captionView = nil;
            //            if ([_delegate respondsToSelector:@selector(photoBrowser:captionViewForPhotoAtIndex:)]) {
            //                captionView = [_delegate photoBrowser:self captionViewForPhotoAtIndex:index];
            //            } else {
            //                id <MWPhoto> photo = [self photoAtIndex:index];
            //                if ([photo respondsToSelector:@selector(caption)]) {
            //                    if ([photo caption]) captionView = [[MWCaptionView alloc] initWithPhoto:photo];
            //                }
            //            }
            //            captionView.alpha = [self areControlsHidden] ? 0 : 1; // Initial alpha
            //            return captionView;
            //        }
            //
        }

        public bool PhotoIsSelectedAtIndex(int index) {
            bool value = false;
            if (_displaySelectionButtons) {
                value = this._delegate.IsPhotoSelectedAtIndex(this, index);
            }
            return value;
        }

        public void SetPhotoSelected(bool selected, int index) {
            if (_displaySelectionButtons) {
                this._delegate.PhotoAtIndexSelectedChanged(this, index, selected);
            }
        }


        public UIImage ImageForPhoto(MWPhoto photo) {
            if (photo != null) {
                // Get image or obtain in background
                if (photo.UnderlyingImage != null) {
                    return photo.UnderlyingImage;
                } else {
                    photo.LoadUnderlyingImageAndNotify();
                }
            }
            return null;
        }

        void LoadAdjacentPhotosIfNecessary(MWPhoto photo) {
            MWZoomingScrollView page = this.PageDisplayingPhoto(photo);
            if (page != null) {
                // If page is current page then initiate loading of previous and next pages
                int pageIndex = page.Index;
                if (_currentPageIndex == pageIndex) {
                    if (pageIndex > 0) {
                        // Preload index - 1
                        MWPhoto prePhoto = this.PhotoAtIndex(pageIndex - 1);
                        if (prePhoto.UnderlyingImage == null) {
                            prePhoto.LoadUnderlyingImageAndNotify();
                            MvxTrace.Trace("Pre-loading image at index {0}", pageIndex - 1);
                        }
                    }
                    if (pageIndex < this.NumberOfPhotos - 1) {
                        // Preload index + 1
                        MWPhoto nextPhoto = this.PhotoAtIndex(pageIndex + 1);
                        if (nextPhoto.UnderlyingImage == null) {
                            nextPhoto.LoadUnderlyingImageAndNotify();
                            MvxTrace.Trace("Pre-loading image at index {0}", pageIndex + 1);
                        }
                    }
                }
            }
        }

        #endregion

        #region  MWPhoto Loading Notification

        void HandleMWPhotoLoadingDidEndNotification(NSNotification notification) {

            var photo = (MWPhoto)notification.Object;
            MWZoomingScrollView page = this.PageDisplayingPhoto(photo);
            if (page != null) {
                if (photo.UnderlyingImage != null) {
                    // Successful load
                    page.DisplayImage();
                    this.LoadAdjacentPhotosIfNecessary(photo);
                } else {
                    // Failed to load
                    page.DisplayImageFailure();
                }
                // Update nav
                this.UpdateNavigation();
            }

        }

        #endregion

        #region Paging

        const int PADDING = 10;

        void TilePages() {
            // Calculate which pages should be visible
            // Ignore padding as paging bounces encroach on that
            // and lead to false page loads

            CGRect visibleBounds = _pagingScrollView.Bounds;
            int iFirstIndex = (int)Math.Floor((visibleBounds.GetMinX() + PADDING * 2) / visibleBounds.Width);
            int iLastIndex = (int)Math.Floor((visibleBounds.GetMaxX() - PADDING * 2 - 1) / visibleBounds.Width);

            if (iFirstIndex < 0) {
                iFirstIndex = 0;
            }
            if (iFirstIndex > (this.NumberOfPhotos - 1)) {
                iFirstIndex = this.NumberOfPhotos - 1;
            }
            if (iLastIndex < 0) {
                iLastIndex = 0;
            }
            if (iLastIndex > (this.NumberOfPhotos - 1)) {
                iLastIndex = this.NumberOfPhotos - 1;
            }

            // Recycle no longer needed pages
            int pageIndex;
            foreach (MWZoomingScrollView page in _visiblePages) {
                pageIndex = page.Index;
                if (pageIndex < (int)iFirstIndex || pageIndex > (int)iLastIndex) {
                    _recycledPages.Add(page);
                    page.CaptionView.RemoveFromSuperview();
                    page.SelectedButton.RemoveFromSuperview();
                    page.PrepareForReuse();
                    page.RemoveFromSuperview();
                    MvxTrace.Trace("Removed page at index {0}", (long)pageIndex);
                }
            }

            //TODO: check if can be removed
            _visiblePages.RemoveAll(_recycledPages.Contains);

            // Only keep 2 recycled pages
            while (_recycledPages.Count > 2) {
                _recycledPages.Remove(_recycledPages[_recycledPages.Count - 1]);
            } 


            // Add missing pages
            for (int index = (int)iFirstIndex; index <= (int)iLastIndex; index++) {
                if (!this.IsDisplayingPageForIndex(index)) {

                    // Add new page
                    MWZoomingScrollView page = this.DequeueRecycledPage();
                    if (page == null) {
                        page = new MWZoomingScrollView(this);
                    }
                    _visiblePages.Add(page);
                    this.ConfigurePage(page, index);

                    _pagingScrollView.AddSubview(page);
                    MvxTrace.Trace("Added page at index {0}", (long)index);

                    // Add caption
                    MWCaptionView captionView = this.CaptionViewForPhotoAtIndex(index);
                    if (captionView != null) {
                        captionView.Frame = this.FrameForCaptionView(captionView, index);
                        _pagingScrollView.AddSubview(captionView);
                        page.CaptionView = captionView;
                    }

                    // Add selected button
                    if (this.DisplaySelectionButtons) {
                        UIButton selectedButton = new UIButton(UIButtonType.Custom);
                        selectedButton.SetImage(UIImage.FromBundle("images/ImageSelectedOff.png"), UIControlState.Normal);
                        selectedButton.SetImage(UIImage.FromBundle("images/ImageSelectedOn.png"), UIControlState.Selected);

                        selectedButton.SizeToFit();
                        selectedButton.AdjustsImageWhenHighlighted = false;
                        selectedButton.TouchUpInside += SelectedButtonTapped;
//                        addTarget:self action:@selector(selectedButtonTapped:) forControlEvents:UIControlEventTouchUpInside];
                        selectedButton.Frame = this.FrameForSelectedButton(selectedButton, index);
                        _pagingScrollView.AddSubview(selectedButton);
                        page.SelectedButton = selectedButton;
                        selectedButton.Selected = this.PhotoIsSelectedAtIndex(index);
                    }

                }
            }
        }

        void UpdateVisiblePageStates() {
            //TODO: check if copy a  new list and the value in _visiblePages
            var copy = new List<MWZoomingScrollView>(_visiblePages);
            foreach (MWZoomingScrollView page in copy) {
                // Update selection
                page.SelectedButton.Selected = this.PhotoIsSelectedAtIndex(page.Index);
            }
        }

        bool IsDisplayingPageForIndex(int index) {
            foreach (MWZoomingScrollView page in _visiblePages)
                if (page.Index == index)
                    return true;
            return false;
        }

        MWZoomingScrollView PageDisplayedAtIndex(int index) {
            MWZoomingScrollView thePage = null;
            foreach (MWZoomingScrollView page in _visiblePages) {
                if (page.Index == index) {
                    thePage = page;
                    break;
                }
            }
            return thePage;
        }

        MWZoomingScrollView PageDisplayingPhoto(MWPhoto photo) {
            MWZoomingScrollView thePage = null;
            foreach (MWZoomingScrollView page in _visiblePages) {
                if (page.Photo == photo) {
                    thePage = page;
                    break;
                }
            }
            return thePage;
        }

        void ConfigurePage(MWZoomingScrollView page, int index) {
            page.Frame = this.FrameForPageAtIndex(index);
            page.Index = index;
            page.Photo = this.PhotoAtIndex(index);
        }

        MWZoomingScrollView DequeueRecycledPage() {
            MWZoomingScrollView page = _recycledPages[_recycledPages.Count - 1];
            if (page != null) {
                _recycledPages.RemoveAt(_recycledPages.Count - 1);
            }
            return page;
        }


        // Handle page changes
        void DidStartViewingPageAtIndex(int index) {

            if (this.NumberOfPhotos <= 0) {
                // Show controls
                this.SetControlsHidden(false, true, true);
                return;
            }

            // Release images further away than +/-1
            int i;
            if (index > 0) {
                // Release anything < index - 1
                for (i = 0; i < index - 1; i++) { 
                    var photo = _photos.Count < i ? _photos[i] : null;
                    if (photo != null) {
                        photo.UnloadUnderlyingImage();
                        _photos[i] = null;
                        MvxTrace.Trace("Released underlying image at index {0}", (long)i);
                    }
                }
            }
            if (index < this.NumberOfPhotos - 1) {
                // Release anything > index + 1
                for (i = index + 2; i < _photos.Count; i++) {
                    var photo = _photos[i];
                    if (photo != null) {
                        photo.UnloadUnderlyingImage();
                        _photos[i] = null;
                        MvxTrace.Trace("Released underlying image at index {0}", (long)i);
                    }
                }
            }

            // Load adjacent images if needed and the photo is already
            // loaded. Also called after photo has been loaded in background
            MWPhoto currentPhoto = this.PhotoAtIndex(index);
            if (currentPhoto.UnderlyingImage != null) {
                // photo loaded so load ajacent now
                this.LoadAdjacentPhotosIfNecessary(currentPhoto);
            }

            // Notify delegate
            if (index != _previousPageIndex) {
                _delegate.DidDisplayPhotoAtIndex(this, index);
                _previousPageIndex = index;
            }

            // Update nav
            this.UpdateNavigation();

        }

        #endregion

        #region  Frame Calculations

        CGRect FrameForPagingScrollView() {
            CGRect frame = this.View.Bounds;// [[UIScreen mainScreen] bounds];
            frame.X -= PADDING;
            frame.Width += (2 * PADDING);
            return frame.Integral();
        }

        CGRect FrameForPageAtIndex(int index) {
            // We have to use our paging scroll view's bounds, not frame, to calculate the page placement. When the device is in
            // landscape orientation, the frame will still be in portrait because the pagingScrollView is the root view controller's
            // view, so its frame is in window coordinate space, which is never rotated. Its bounds, however, will be in landscape
            // because it has a rotation transform applied.
            CGRect bounds = _pagingScrollView.Bounds;
            CGRect pageFrame = bounds;
            pageFrame.Width -= (2 * PADDING);
            pageFrame.X = (bounds.Size.Width * index) + PADDING;
            return pageFrame.Integral();
        }

        CGSize ContentSizeForPagingScrollView() {
            // We have to use the paging scroll view's bounds to calculate the contentSize, for the same reason outlined above.
            CGRect bounds = _pagingScrollView.Bounds;
            return new CGSize(bounds.Size.Width * this.NumberOfPhotos, bounds.Size.Height);
        }

        CGPoint ContentOffsetForPageAtIndex(int index) {
            float pageWidth = (float)_pagingScrollView.Bounds.Size.Width;
            float newOffset = index * pageWidth;
            return new CGPoint(newOffset, 0);
        }

        CGRect FrameForToolbarAtOrientation(UIInterfaceOrientation interfaceOrientation) {
            float height = 44;
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone &&
                (interfaceOrientation == UIInterfaceOrientation.LandscapeLeft || interfaceOrientation == UIInterfaceOrientation.LandscapeRight)) {
                height = 32;
            }
            return new CGRect(0, this.View.Bounds.Size.Height - height, this.View.Bounds.Size.Width, height).Integral();
        }

        CGRect FrameForCaptionView(MWCaptionView captionView, int index) {

            CGRect pageFrame = this.FrameForPageAtIndex(index);
            CGSize captionSize = captionView.SizeThatFits(new CGSize(pageFrame.Size.Width, 0));
            CGRect captionFrame = new CGRect(pageFrame.Location.X,
                                      pageFrame.Size.Height - captionSize.Height - (_toolbar.Superview != null ? _toolbar.Frame.Size.Height : 0),
                                      pageFrame.Size.Width,
                                      captionSize.Height);
            return captionFrame.Integral();
        }

        CGRect FrameForSelectedButton(UIButton selectedButton, int index) {
            CGRect pageFrame = this.FrameForPageAtIndex(index);
            float yOffset = 0;
            if (!this.AreControlsHidden) {
                UINavigationBar navBar = this.NavigationController.NavigationBar;
                yOffset = (float)(navBar.Frame.Location.Y + navBar.Frame.Size.Height);
            }
            float statusBarOffset = (float)UIApplication.SharedApplication.StatusBarFrame.Size.Height;
            CGRect captionFrame = new CGRect(pageFrame.Location.X + pageFrame.Size.Width - 20 - selectedButton.Frame.Size.Width,
                                      statusBarOffset + yOffset,
                                      selectedButton.Frame.Size.Width,
                                      selectedButton.Frame.Size.Height);
            return captionFrame.Integral();
        }

        #endregion

        #region UIScrollView Delegate

        //        [Export("scrollViewDidScroll:"), CompilerGenerated]
        public virtual void Scrolled(UIScrollView scrollView) {
            // Checks
            if (!_viewIsActive || _performingLayout || _rotating) {
                return;
            }

            // Tile pages
            this.TilePages();

            // Calculate current page
            CGRect visibleBounds = _pagingScrollView.Bounds;
            int index = (int)(Math.Floor(visibleBounds.GetMidX() / visibleBounds.Width));
            if (index < 0)
                index = 0;
            if (index > this.NumberOfPhotos - 1) {
                index = this.NumberOfPhotos - 1;
            }
            int previousCurrentPage = _currentPageIndex;
            _currentPageIndex = index;
            if (_currentPageIndex != previousCurrentPage) {
                this.DidStartViewingPageAtIndex(index);
            }
        }

        //        [Export("scrollViewWillBeginDragging:"), CompilerGenerated]
        public virtual void DraggingStarted(UIScrollView scrollView) {
            // Hide controls when dragging begins
            this.SetControlsHidden(true, true, false);
        }

        //        [Export("scrollViewDidEndDecelerating:"), CompilerGenerated]
        public virtual void DecelerationEnded(UIScrollView scrollView) {
            // Update nav when page changes
            this.UpdateNavigation();
        }

        #endregion

        #region Navigation

        // Grid
        MWGridViewController _gridController;

        void UpdateNavigation() {
            // Title
            int numberOfPhotos = this.NumberOfPhotos;
            if (_gridController != null) {
                if (_gridController.SelectionMode) {
                    this.Title = "选择照片";
                } else {
                    this.Title = string.Format("选择了{0}张", numberOfPhotos);
                }
            } else if (numberOfPhotos > 1) {
                this.Title = string.Format("第{0}张", _currentPageIndex + 1);
            } else {
                this.Title = null;
            }

            // Buttons
//            _previousButton.Enabled = (_currentPageIndex > 0);
//            _nextButton.Enabled = (_currentPageIndex < numberOfPhotos - 1);
//            _actionButton.Enabled = this.PhotoAtIndex(_currentPageIndex).UnderlyingImage != null;
        }

        void JumpToPageAtIndex(int index, bool animated) {

            // Change page
            if (index < this.NumberOfPhotos) {
                CGRect pageFrame = this.FrameForPageAtIndex(index);
                _pagingScrollView.SetContentOffset(new CGPoint(pageFrame.X - PADDING, 0), animated);
                this.UpdateNavigation();
            }

            // Update timer to give more time
            this.HideControlsAfterDelay();
        }

        void GotoPreviousPage() {
            this.ShowPreviousPhotoAnimated(false);
        }

        void GotoNextPage() {
            this.ShowNextPhotoAnimated(false);
        }

        void ShowPreviousPhotoAnimated(bool animated) {
            this.JumpToPageAtIndex(_currentPageIndex - 1, animated);
        }

        void ShowNextPhotoAnimated(bool animated) {
            this.JumpToPageAtIndex(_currentPageIndex + 1, animated);
        }


        #endregion

        #region Interactions


        void SelectedButtonTapped(object sender, EventArgs e) {
            UIButton selectedButton = (UIButton)sender;
            selectedButton.Selected = !selectedButton.Selected;
            int index = int.MaxValue;
            foreach (MWZoomingScrollView page in _visiblePages) {
                if (page.SelectedButton == selectedButton) {
                    index = page.Index;
                    break;
                }
            }
            if (index != int.MaxValue) {
                this.SetPhotoSelected(selectedButton.Selected, index);
            }
        }

        #endregion

        #region Grid

        void ShowGridAnimated() {
            this.ShowGrid(true);
        }


        void ShowGrid(bool animated) {

            if (_gridController != null) {
                return;
            }
            var flowLayout = new UICollectionViewFlowLayout() {
                HeaderReferenceSize = new CGSize(0, 0),
                SectionInset = new UIEdgeInsets(1, 1, 1, 1),
                ScrollDirection = UICollectionViewScrollDirection.Vertical,
                MinimumInteritemSpacing = 1, // minimum spacing between cells
                MinimumLineSpacing = 1, // minimum spacing between rows if ScrollDirection is Vertical or between columns if Horizontal
                ItemSize = new CGSize(80, 80)
            };

            // Init grid controller
            _gridController = new MWGridViewController(flowLayout);
            _gridController.InitialContentOffset = _currentGridContentOffset;
            _gridController.Browser = this;
            _gridController.SelectionMode = _displaySelectionButtons;
            _gridController.View.Frame = this.View.Bounds;
            _gridController.View.Frame.Offset(0, (this.StartOnGrid ? -1 : 1) * this.View.Bounds.Size.Height);

            // Stop specific layout being triggered
            _skipNextPagingScrollViewPositioning = true;

            // Add as a child view controller
            this.AddChildViewController(_gridController);
            this.View.AddSubview(_gridController.View);

            // Hide action button on nav bar if it exists
//            if (this.NavigationItem.RightBarButtonItem == _actionButton) {
//                _gridPreviousRightNavItem = _actionButton;
//                this.NavigationItem.SetRightBarButtonItem(null, true);
//            } else {
//                _gridPreviousRightNavItem = null;
//            }

            // Update
            this.UpdateNavigation();
            this.SetControlsHidden(false, true, true);

            // Animate grid in and photo scroller out
            UIView.Animate(animated ? 0.3 : 0, () => {
                _gridController.View.Frame = this.View.Bounds;
                CGRect newPagingFrame = this.FrameForPagingScrollView();
                newPagingFrame.Offset(0, (this.StartOnGrid ? 1 : -1) * newPagingFrame.Size.Height);
                _pagingScrollView.Frame = newPagingFrame;
            }, () => _gridController.DidMoveToParentViewController(this));

        }

        public void HideGrid() {

            if (_gridController == null)
                return;

            // Remember previous content offset
            _currentGridContentOffset = _gridController.CollectionView.ContentOffset;

            // Restore action button if it was removed
//            if (_gridPreviousRightNavItem == _actionButton && _actionButton!=null) {
//                this.NavigationItem.SetRightBarButtonItem(_gridPreviousRightNavItem, true);
//            }

            // Position prior to hide animation
            CGRect newPagingFrame = this.FrameForPagingScrollView();
            newPagingFrame.Offset(0, (this.StartOnGrid ? 1 : -1) * newPagingFrame.Size.Height);
            _pagingScrollView.Frame = newPagingFrame;

            // Remember and remove controller now so things can detect a nil grid controller
            MWGridViewController tmpGridController = _gridController;
            _gridController = null;

            // Update
            this.UpdateNavigation();
            this.UpdateVisiblePageStates();

            // Animate, hide grid and show paging scroll view
            UIView.Animate(0.3, () => {
                this.View.Bounds.Offset(0, (this.StartOnGrid ? -1 : 1) * this.View.Bounds.Size.Height);
                _pagingScrollView.Frame = this.FrameForPagingScrollView();
            }, () => {
                tmpGridController.WillMoveToParentViewController(null);
                tmpGridController.View.RemoveFromSuperview();
                tmpGridController.RemoveFromParentViewController();
                this.SetControlsHidden(false, true, false); // retrigger timer
            });
        }

        #endregion

        #region Control Hiding / Showing

        // If permanent then we don't set timers to hide again
        // Fades all controls on iOS 5 & 6, and iOS 7 controls slide and fade
        void SetControlsHidden(bool hidden, bool animated, bool permanent) {
            // Force visible
            if (this.NumberOfPhotos > 0 || _gridController != null || _alwaysShowControls) {
                hidden = false;
            }

            // Cancel any timers
            this.CancelControlHiding();

            // Animations & positions
            bool slideAndFade = true;
            float animatonOffset = 20;
            float animationDuration = (animated ? 0.35f : 0f);

            // Status bar
            if (!_leaveStatusBarAlone) {
                //TODO : check
                // Hide status bar
                // Non-view controller based
                UIApplication.SharedApplication.SetStatusBarHidden(hidden, animated ? UIStatusBarAnimation.Slide : UIStatusBarAnimation.None);

                // View controller based so animate away
                _statusBarShouldBeHidden = hidden;
                UIView.Animate(animationDuration, () => {
                    this.SetNeedsStatusBarAppearanceUpdate();
                }, () => {
                });

            }

            // Toolbar, nav bar and captions
            // Pre-appear animation positions for iOS 7 sliding
            if (slideAndFade && this.AreControlsHidden && !hidden && animated) {

                // Toolbar
                var tempFrame = this.FrameForToolbarAtOrientation(this.InterfaceOrientation);
                tempFrame.Offset(0, animatonOffset);
                _toolbar.Frame = tempFrame;

                // Captions
                foreach (MWZoomingScrollView page in _visiblePages) {
                    if (page.CaptionView != null) {
                        MWCaptionView v = page.CaptionView;
                        // Pass any index, all we're interested in is the Y
                        CGRect captionFrame = this.FrameForCaptionView(v, 0);
                        captionFrame.X = v.Frame.X; // Reset X
                        captionFrame.Offset(0, animatonOffset);
                        v.Frame = captionFrame;
                    }
                }

            }
            UIView.Animate(animationDuration, () => {

                float alpha = hidden ? 0 : 1;

                // Nav bar slides up on it's own on iOS 7
                this.NavigationController.NavigationBar.Alpha = (alpha);

                // Toolbar
                if (slideAndFade) {
                    _toolbar.Frame = this.FrameForToolbarAtOrientation(this.InterfaceOrientation);
                    if (hidden)
                        _toolbar.Frame.Offset(0, animatonOffset);
                }
                _toolbar.Alpha = alpha;

                // Captions
                foreach (MWZoomingScrollView page in _visiblePages) {
                    if (page.CaptionView != null) {
                        MWCaptionView v = page.CaptionView;
                        if (slideAndFade) {
                            // Pass any index, all we're interested in is the Y
                            CGRect captionFrame = this.FrameForCaptionView(v, 0);
                            captionFrame.X = v.Frame.X; // Reset X
                            if (hidden)
                                captionFrame.Offset(0, animatonOffset);
                            v.Frame = captionFrame;
                        }
                        v.Alpha = alpha;
                    }
                }

                // Selected buttons
                foreach (MWZoomingScrollView page in _visiblePages) {
                    if (page.SelectedButton != null) {
                        UIButton v = page.SelectedButton;
                        CGRect newFrame = this.FrameForSelectedButton(v, 0);
                        newFrame.X = v.Frame.X;
                        v.Frame = newFrame;
                    }
                }

            }, () => {
            });

            // Control hiding timer
            // Will cancel existing timer but only begin hiding if
            // they are visible
            if (!permanent)
                this.HideControlsAfterDelay();

        }

        bool _leaveStatusBarAlone;
        bool _statusBarShouldBeHidden;

        bool PrefersStatusBarHidden() {
            if (!_leaveStatusBarAlone) {
                return _statusBarShouldBeHidden;
            } else {
                return this.PresentingViewController.PrefersStatusBarHidden();
            }
        }

        UIStatusBarAnimation PreferredStatusBarUpdateAnimation() {
            return UIStatusBarAnimation.Slide;
        }

        NSTimer _controlVisibilityTimer;

        public void CancelControlHiding() {
            // If a timer exists then cancel and release
            if (_controlVisibilityTimer != null) {
                _controlVisibilityTimer.Invalidate();
                _controlVisibilityTimer = null;
            }
        }

        // Enable/disable control visiblity timer
        public void HideControlsAfterDelay() {
            if (!this.AreControlsHidden) {
                this.CancelControlHiding();
                _controlVisibilityTimer = NSTimer.CreateScheduledTimer(this.DelayToHideElements, HideControls);
            }
        }


        bool AreControlsHidden {
            get {
                return (_toolbar.Alpha == 0);
            }
        }

        void HideControls(NSTimer t) {
            this.SetControlsHidden(true, true, false);
        }

        [Export("toggleControls")]
        void ToggleControls() {
            this.SetControlsHidden(!this.AreControlsHidden, true, false);
        }

        #endregion

        #region Properties

        // Handle depreciated method
        void SetInitialPageIndex(int index) {
            this.SetCurrentPhotoIndex(index);
        }

        public void SetCurrentPhotoIndex(int index) {
            // Validate
            int photoCount = this.NumberOfPhotos;
            if (photoCount == 0) {
                index = 0;
            } else {
                if (index >= photoCount)
                    index = this.NumberOfPhotos - 1;
            }
            _currentPageIndex = index;
            if (this.IsViewLoaded) {
                this.JumpToPageAtIndex(index, false);
                if (!_viewIsActive)
                    this.TilePages(); // Force tiling if view is not visible
            }
        }

        #endregion

        #region Misc

        public  void DoneButtonPressed(object sender, EventArgs e) {
            // Only if we're modal and there's a done button
            if (_doneButton != null) {
                // See if we actually just want to show/hide grid
                if (this.EnableGrid) {
                    if (this.StartOnGrid && _gridController == null) {
                        this.ShowGrid(true);
                        return;
                    } else if (this.StartOnGrid && _gridController != null) {
                        this.HideGrid();
                        return;
                    }
                }
                // Dismiss view controller
                // Call delegate method and let them dismiss us
//                TODO:check
                _delegate.PhotoBrowserDidFinishModalPresentation(this);
                this.DismissViewController(true, null);
            }
        }

        #endregion

        #region Actions

        #endregion

       


    }
}

