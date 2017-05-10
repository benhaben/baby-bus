using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace PhotoBrowser {
    public class MWGridViewController : UICollectionViewController {
        //        @property (nonatomic, assign) MWPhotoBrowser *browser;
        //        @property (nonatomic) BOOL selectionMode;
        //        @property (nonatomic) CGPoint initialContentOffset;


        public MWPhotoBrowser Browser {
            get;
            set;
        }

        public bool SelectionMode {
            get;
            set;
        }

        public CGPoint InitialContentOffset {
            get;
            set;
        }

        // Store margins for current setup
        float _margin, _gutter, _marginL, _gutterL, _columns, _columnsL;

        public float Gutter {
            get {
                if ((UIInterfaceOrientation.Portrait == this.InterfaceOrientation)) {
                    return _gutter;
                } else {
                    return _gutterL;
                }
            }
        }

        public float Columns {
            get {
                if ((UIInterfaceOrientation.Portrait == this.InterfaceOrientation)) {
                    return _columns;
                } else {
                    return _columnsL;
                }
            }
        }


        public float Margin {
            get {
                if ((UIInterfaceOrientation.Portrait == this.InterfaceOrientation)) {
                    return _margin;
                } else {
                    return _marginL;
                }
            }
        }

        //        [Export("initWithFrame:collectionViewLayout:"), CompilerGenerated]
        public MWGridViewController(UICollectionViewLayout layout)
            : base(layout) {

            // Defaults
            _columns = 3;
            _columnsL = 4;
            _margin = 0;
            _gutter = 1;
            _marginL = 0;
            _gutterL = 1;

            // For pixel perfection...
            if (UIScreen.MainScreen.Bounds.Size.Height == 480) {
                // iPhone 3.5 inch
                _columns = 3;
                _columnsL = 4;
                _margin = 0;
                _gutter = 1;
                _marginL = 1;
                _gutterL = 2;
            } else {
                // iPhone 4 inch
                _columns = 3;
                _columnsL = 5;
                _margin = 0;
                _gutter = 1;
                _marginL = 0;
                _gutterL = 2;
            }

            InitialContentOffset = new CGPoint(0, float.MaxValue);

        }

        const string GridCell = "MWGridCell";

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            this.CollectionView.RegisterClassForCell(typeof(MWGridCell), GridCell);
            this.CollectionView.AlwaysBounceVertical = true;
            this.CollectionView.BackgroundColor = UIColor.Black;
        }

        public override void ViewWillDisappear(bool animated) {
            base.ViewWillDisappear(animated);
            var visibleCells = CollectionView.VisibleCells;
            if (visibleCells != null) {
                foreach (var cell in visibleCells) {
                    var mwGridCell = cell as MWGridCell;
                    if (mwGridCell != null && mwGridCell.Photo != null) {
                        mwGridCell.Photo.CancelAnyLoading();
                    }
                }
            }
        }

        public override void ViewWillLayoutSubviews() {
            base.ViewWillLayoutSubviews();
            this.PerformLayout();
        }

        void PerformLayout() {
            UINavigationBar navBar = this.NavigationController.NavigationBar;
            const float yAdjust = 0;
            this.CollectionView.ContentInset = new UIEdgeInsets(navBar.Frame.Y + navBar.Frame.Size.Height + Gutter + yAdjust, 0, 0, 0);
        }

        public override void ViewDidLayoutSubviews() {
            base.ViewDidLayoutSubviews();
            const float EPSILON = 0.000001f;
            if (Math.Abs(InitialContentOffset.Y - float.MaxValue) > EPSILON) {
                CollectionView.ContentOffset = InitialContentOffset;
            }

            CGPoint currentContentOffset = CollectionView.ContentOffset;
            // Get scroll position to have the current photo on screen

            if (Browser.NumberOfPhotos > 0) {
                NSIndexPath currentPhotoIndexPath = NSIndexPath.FromItemSection(Browser.CurrentIndex, 0);
                //TODO: check the effect
                this.CollectionView.ScrollToItem(currentPhotoIndexPath, 0, false);
            }

            CGPoint offsetToShowCurrent = CollectionView.ContentOffset;

            // Only commit to using the scrolled position if it differs from the initial content offset
            if (offsetToShowCurrent != currentContentOffset) {
                //Use offset to show current
                CollectionView.ContentOffset = offsetToShowCurrent;
            } else {
                // Stick with initial
                CollectionView.ContentOffset = currentContentOffset;
            }

        }

        //TODO: check if this is useful
        //        public override void WillAnimateRotationToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation NSTimeInterval duration) {
        //            [self.collectionView reloadData];
        //            [self performLayout]; // needed for iOS 5 & 6
        //        }

        //        [Export("collectionView:numberOfItemsInSection:"), Availability(Introduced = Platform.iOS_6_0), CompilerGenerated]
        public override nint GetItemsCount(UICollectionView collectionView, nint section) {
            return Browser.NumberOfPhotos;
        }


        //        [Export("collectionView:cellForItemAtIndexPath:"), Availability(Introduced = Platform.iOS_6_0), CompilerGenerated]
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) {
            MWGridCell cell = (MWGridCell)CollectionView.DequeueReusableCell(GridCell, indexPath);
            if (cell == null) {
                cell = new MWGridCell();
            }
            MWPhoto photo = Browser.ThumbPhotoAtIndex(indexPath.Row);
            cell.Photo = photo;
            //TODO: does cell get parent like this? 
            cell.GridController = this;
            cell.SelectionMode = SelectionMode;
            cell.IsSelected = Browser.PhotoIsSelectedAtIndex(indexPath.Row);
            cell.Index = indexPath.Row;
            UIImage img = Browser.ImageForPhoto(photo);

            if (img != null) {
                cell.DisplayImage();
            } else {
                photo.LoadUnderlyingImageAndNotify();
            }
            return cell;

        }
        //        [Export("collectionView:didSelectItemAtIndexPath:"), Availability(Introduced = Platform.iOS_6_0), CompilerGenerated]
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath) {
            Browser.SetCurrentPhotoIndex(indexPath.Row);
            Browser.HideGrid();
        }

        //        [Export("collectionView:didEndDisplayingCell:forItemAtIndexPath:"), Availability(Introduced = Platform.iOS_6_0), CompilerGenerated]
        public override void CellDisplayingEnded(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath) {
            var gridCell = cell as MWGridCell;
            if (gridCell != null) {
                gridCell.Photo.CancelAnyLoading();
            }
        }

        //TODO: init at start, this function is call by PSTCollectionView, not useful now

        //        - (CGSize)collectionView:(PSTCollectionView *)collectionView layout:(PSTCollectionViewLayout*)collectionViewLayout sizeForItemAtIndexPath:(NSIndexPath *)indexPath {
        //            CGFloat margin = [self getMargin];
        //            CGFloat gutter = [self getGutter];
        //            CGFloat columns = [self getColumns];
        //            CGFloat value = floorf(((self.view.bounds.size.width - (columns - 1) * gutter - 2 * margin) / columns));
        //            return CGSizeMake(value, value);
        //        }
        //
        //        - (CGFloat)collectionView:(PSTCollectionView *)collectionView layout:(PSTCollectionViewLayout*)collectionViewLayout minimumInteritemSpacingForSectionAtIndex:(NSInteger)section {
        //            return [self getGutter];
        //        }
        //
        //        - (CGFloat)collectionView:(PSTCollectionView *)collectionView layout:(PSTCollectionViewLayout*)collectionViewLayout minimumLineSpacingForSectionAtIndex:(NSInteger)section {
        //            return [self getGutter];
        //        }
        //
        //        - (UIEdgeInsets)collectionView:(PSTCollectionView *)collectionView layout:(PSTCollectionViewLayout*)collectionViewLayout insetForSectionAtIndex:(NSInteger)section {
        //            CGFloat margin = [self getMargin];
        //            return UIEdgeInsetsMake(margin, margin, margin, margin);
        //        }

    }
}

