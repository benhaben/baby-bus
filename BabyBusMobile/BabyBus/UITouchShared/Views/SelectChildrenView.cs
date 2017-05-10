using System;
using System.Linq;
using BabyBus.iOS;
using CoreGraphics;
using UIKit;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;


namespace BabyBus.iOS
{
    public class SelectChildrenView : MvxBabyBusBaseAutoLayoutViewController
    {
        SelectChildrenViewModel _baseViewModel;

        public SelectChildrenView()
        {
            //Note: 如果不设置这个标志，tap会变成长按
            AddGestureWhenTap = false;
        }

        UICollectionView _collectionViewChildren = null;

        public UICollectionView CollectionViewChildren
        {
            get
            {
                if (_collectionViewChildren == null)
                {
                    // Flow Layout
                    var flowLayout = new UICollectionViewFlowLayout()
                    {
                        HeaderReferenceSize = new CGSize(200, 30),
                        SectionInset = new UIEdgeInsets(2, 2, 2, 2),
                        ScrollDirection = UICollectionViewScrollDirection.Vertical,
                        MinimumInteritemSpacing = 2, // minimum spacing between cells
                        MinimumLineSpacing = 10, // minimum spacing between rows if ScrollDirection is Vertical or between columns if Horizontal
                        ItemSize = new CGSize(70, 80)
                    };

                    //frame is setted in the SetUpConstrainLayout
                    _collectionViewChildren = new UICollectionView(new CGRect(0, 0, 0, 0), flowLayout);
                    _collectionViewChildren.ContentInset = new UIEdgeInsets(2, 2, 2, 2);
                    _collectionViewChildren.BackgroundColor = UIColor.Clear;
                    _collectionViewChildren.RegisterClassForCell(typeof(ChildCell), ChildCell.CellID);
                    _collectionViewChildren.RegisterClassForSupplementaryView(typeof(Header), UICollectionElementKindSection.Header, Header.HeaderId);
                    _collectionViewChildren.ShowsHorizontalScrollIndicator = false;
                }
                return _collectionViewChildren;
            }
        }

        private ChildDataSource _userSource;

        public override void PrepareViewHierarchy()
        {
            base.PrepareViewHierarchy();
            UIView[] v =
                {
                    CollectionViewChildren,
                };

            Container.AddSubviews(v);

            #if DEBUD1
            CollectionViewChildren.BackgroundColor = UIColor.Cyan;
            #endif
        }


        public override void SetUpConstrainLayout()
        {
            base.SetUpConstrainLayout();

            nfloat height = 480;
            View.ConstrainLayout 
            (
                // Analysis disable CompareOfFloatsByEqualityOperator
                () => 
                CollectionViewChildren.Frame.Height == height
                && CollectionViewChildren.Frame.Left == Container.Frame.Left
                && CollectionViewChildren.Frame.Right == Container.Frame.Right
                && CollectionViewChildren.Frame.Top == Container.Frame.Top
                && Container.Frame.Bottom == CollectionViewChildren.Frame.Bottom

                // Analysis restore CompareOfFloatsByEqualityOperator
            );
            CollectionViewChildren.ScrollEnabled = true;
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();

            Title = "选择孩子";
            View.BackgroundColor = UIColor.White;
            _baseViewModel = this.ViewModel as SelectChildrenViewModel;
            _userSource = new ChildDataSource();
            _userSource.FontSize = 11f;
            _userSource.ImageViewSize = new CGSize(60, 60);

            if (BabyBusContext.UserAllInfo.RoleType == RoleType.HeadMaster)
            {
                //                CollectionViewChildren.UserInteractionEnabled = false;
            }
            else
            {
                CollectionViewChildren.UserInteractionEnabled = true;
                this.NavigationItem.SetRightBarButtonItem(
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) =>
                        {
                            _baseViewModel.FinishCommand.Execute();
                        })
                    , true);
            }

            CollectionViewChildren.Source = _userSource;
            CollectionViewChildren.ReloadData();
            CollectionViewChildren.AllowsSelection = true;

            _baseViewModel.FirstLoadedEventHandler += (object sender, EventArgs e) => InvokeOnMainThread(() =>
                {
                    if (!_baseViewModel.IsRunning)
                    {
                        _userSource.SetChildren(_baseViewModel.Children.ToList());
                        CollectionViewChildren.ReloadData();
                    }
                });
        }
    }
    //
    //    public class ChildDataSource : UICollectionViewSource
    //    {
    //        public ChildDataSource()
    //        {
    //            Children = new List<ChildModel>();
    //            SelectedChildren = new List<ChildModel>();
    //        }
    //
    //        private Header _headerView;
    //
    //        private List<ChildModel> Children { get; set; }
    //
    //        private List<ChildModel> SelectedChildren { get; set; }
    //
    //        public void SetChildren(List<ChildModel> list)
    //        {
    //            Children.Clear();
    //            Children = list;
    //            SelectedChildren = (from i in Children
    //                                         where i.IsSelect
    //                                         select i).ToList();
    //        }
    //
    //        public Single FontSize { get; set; }
    //
    //        public CGSize ImageViewSize { get; set; }
    //
    //        public override nint NumberOfSections(UICollectionView collectionView)
    //        {
    //            return 1;
    //        }
    //
    //        public override nint GetItemsCount(UICollectionView collectionView, nint section)
    //        {
    //            return Children.Count;
    //        }
    //
    //        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
    //        {
    //            _headerView = (Header)collectionView.DequeueReusableSupplementaryView(elementKind, Header.HeaderId, indexPath);
    //            _headerView.Text = string.Format("已经选择{0}人。", SelectedChildren.Count);
    //
    //            return _headerView;
    //        }
    //
    //        public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
    //        {
    //            if (BabyBusContext.UserAllInfo.RoleType == RoleType.Master)
    //            {
    //                return false;
    //            }
    //            else
    //            {
    //                var cell = collectionView.CellForItem(indexPath) as ChildCell;
    //                var childModel = Children[indexPath.Row];
    //
    //                if (childModel.IsSelect)
    //                {
    //                    //                已经背选择了，就取消选择
    //                    SelectedChildren.Remove(childModel);
    //                    cell.CheckImageView.Hidden = true;
    //                    childModel.IsSelect = false;
    //                }
    //                else
    //                {
    //                    //                没有被选择。则选择
    //                    SelectedChildren.Add(childModel);
    //                    cell.CheckImageView.Hidden = false;
    //                    childModel.IsSelect = true;
    //                }
    //
    //                _headerView.Text = string.Format("已经选择{0}人。", SelectedChildren.Count);
    //
    //                return true;
    //            }
    //        }
    //
    //        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
    //        {
    //            var cell = (ChildCell)collectionView.DequeueReusableCell(ChildCell.CellID, indexPath);
    //
    //            var childModel = Children[indexPath.Row];
    //
    //            cell.UpdateRow(childModel, FontSize, ImageViewSize);
    //
    //            return cell;
    //        }
    //    }
}

