//#define __DEBUDUI__
using System;
using System.Collections.Generic;
using System.Linq;
using BabyBus.iOS;
using CoreGraphics;
using Foundation;
using SDWebImage;
using UIKit;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.iOS
{
    public class AttendanceDetailView : MvxBabyBusBaseAutoLayoutViewController
    {
        AttendanceDetailViewModel _baseViewModel;

        public AttendanceDetailView()
        {
            //Note: 如果不设置这个标志，tap会变成长按
            AddGestureWhenTap = false;
        }

        nfloat _cellWidth = 70f;
        nfloat _imageViewSize = 60f;

        UICollectionView _collectionViewChildren = null;

        public UICollectionView CollectionViewChildren
        {
            get
            {
                if (_collectionViewChildren == null)
                {
                    nfloat marginSection = 2f;

                    // Flow Layout
                    var flowLayout = new UICollectionViewFlowLayout()
                    {

                        HeaderReferenceSize = new CGSize(200, 30),
                        SectionInset = new UIEdgeInsets(marginSection, marginSection, marginSection, marginSection),
                        ScrollDirection = UICollectionViewScrollDirection.Vertical,
                        MinimumInteritemSpacing = 2, // minimum spacing between cells
                        MinimumLineSpacing = 10, // minimum spacing between rows if ScrollDirection is Vertical or between columns if Horizontal
                        ItemSize = new CGSize(_cellWidth, 80)
                    };

                    nfloat margin = 2f;
                    //frame is setted in the SetUpConstrainLayout
                    _collectionViewChildren = new UICollectionView(new CGRect(0, 0, 0, 0), flowLayout);
                    _collectionViewChildren.ContentInset = new UIEdgeInsets(margin, margin, margin, margin);
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

            Title = "点名";
            View.BackgroundColor = UIColor.White;
            _baseViewModel = this.ViewModel as AttendanceDetailViewModel;
            _userSource = new ChildDataSource();
            _userSource.FontSize = 11f;
            _userSource.ImageViewSize = new CGSize(_imageViewSize, _imageViewSize);

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
                            _baseViewModel.AttenceCommand.Execute();
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

    public class ChildDataSource : UICollectionViewSource
    {
        public ChildDataSource()
        {
            Children = new List<ChildModel>();
            SelectedChildren = new List<ChildModel>();
        }

        private Header _headerView;

        private List<ChildModel> Children { get; set; }

        private List<ChildModel> SelectedChildren { get; set; }

        public void SetChildren(List<ChildModel> list)
        {
            Children.Clear();
            Children = list;
            SelectedChildren = (from i in Children
                                         where i.IsSelect
                                         select i).ToList();
        }

        public Single FontSize { get; set; }

        public CGSize ImageViewSize { get; set; }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return Children.Count;
        }

        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            _headerView = (Header)collectionView.DequeueReusableSupplementaryView(elementKind, Header.HeaderId, indexPath);
            _headerView.Text = string.Format("全班{0}人，已经选择{1}人。", Children.Count, SelectedChildren.Count);

            return _headerView;
        }

        public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (BabyBusContext.UserAllInfo.RoleType == RoleType.HeadMaster)
            {
                return false;
            }
            else
            {
                var cell = collectionView.CellForItem(indexPath) as ChildCell;
                var childModel = Children[indexPath.Row];
           
                if (childModel.IsSelect)
                {
//                已经背选择了，就取消选择
                    SelectedChildren.Remove(childModel);
                    childModel.IsSelect = false;
                    cell.DeselectElement();
                }
                else
                {
//                没有被选择。则选择
                    SelectedChildren.Add(childModel);
                    childModel.IsSelect = true;
                    cell.SelectElement();
                }

                _headerView.Text = string.Format("全班{0}人，已经选择{1}人。", Children.Count, SelectedChildren.Count);

                return true;
            }
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (ChildCell)collectionView.DequeueReusableCell(ChildCell.CellID, indexPath);

            var childModel = Children[indexPath.Row];

            cell.UpdateRow(childModel, FontSize, ImageViewSize);

            return cell;
        }
    }


    //do not support ConstrainLayout?
    public class ChildCell : UICollectionViewCell
    {
        public static NSString CellID = new NSString("ChildCell");

        //        ChildUIView _childUIView = null;

        UILabel _childName = null;

        public UILabel ChildNameLabel
        {
            get
            { 
                if (_childName == null)
                {
                    _childName = new UILabel();
                    _childName.Text = "孩子姓名";
                    _childName.Font = EasyLayout.TinyFont;
                    _childName.TextAlignment = UITextAlignment.Center;
                }
                return _childName;
            }
            set{ _childName = value; }
        }

        UIImageView _checkImage = null;

        public UIImageView CheckImageView
        {
            get
            { 
                if (_checkImage == null)
                {
                    _checkImage = new UIImageView();
                    _checkImage.Image = UIImage.FromBundle("check.png");
                    _checkImage.Hidden = true;
                }
                return _checkImage;
            }
        }


        UIImageView _childImage = null;

        public UIImageView ChildImageView
        {
            get
            { 
                if (_childImage == null)
                {
                    _childImage = new UIImageView();
                    _childImage.Image = UIImage.FromBundle("placeholder.png");
                }
                return _childImage;
            }
        }

        [Export("initWithFrame:")]
        public ChildCell(CGRect frame)
            : base(frame)
        {

//            ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
//            ContentView.Layer.BorderWidth = 1.0f;

            ChildImageView.Frame = new CGRect(5, 0, 60, 60);
            ChildNameLabel.Frame = new CGRect(0, 60, 70, 20);
            CheckImageView.Frame = new CGRect(30, 30, 30, 30);
            CheckImageView.Alpha = (nfloat)1f;
            CheckImageView.BackgroundColor = UIColor.Clear;

            UIView[] v =
                {
                    ChildImageView,
                    ChildNameLabel,
                    CheckImageView
                };

            ContentView.AddSubviews(v);


        }

        #region IImageUpdated implementation

        //        void IImageUpdated.UpdatedImage(Uri uri) {
        //            ChildImageView.Image = ImageLoader.DefaultRequestImage(uri, this);
        //        }

        #endregion

        public void SelectElement()
        {
            CheckImageView.Hidden = false;
            ChildImageView.Layer.BorderColor = MvxTouchColor.White.CGColor;
            ChildImageView.Layer.BorderWidth = 1.0f;
            ChildImageView.Alpha = 0.6f;
        }

        public void DeselectElement()
        {
            CheckImageView.Hidden = true;
            ChildImageView.Layer.BorderColor = MvxTouchColor.Blue.CGColor;
            ChildImageView.Layer.BorderWidth = 2.0f;
            ChildImageView.Alpha = 1.0f;
        }

        public void UpdateRow(ChildModel childModel, Single fontSize, CGSize imageViewSize)
        {
            ChildNameLabel.Text = childModel.ChildName;
            var uri = new Uri(Constants.ThumbServerPath + childModel.ImageName + Constants.ThumbRule);

            ChildImageView.SetImage(uri, UIImage.FromBundle("placeholder.png"));

//            ChildImageView.Frame = new CGRect(0, 0, imageViewSize.Width, imageViewSize.Height);
//            ChildNameLabel.Frame = new CGRect(0, ChildImageView.Frame.Bottom, imageViewSize.Width,
//                ContentView.Frame.Height - ChildImageView.Frame.Bottom);

            ChildImageView.Layer.CornerRadius = (imageViewSize.Height / 2);
            ChildImageView.Layer.MasksToBounds = false;
            ChildImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            ChildImageView.ClipsToBounds = true;
            ChildImageView.Layer.ShadowColor = UIColor.Black.CGColor;
            ChildImageView.Layer.ShadowOffset = new CGSize(4, 4);
            ChildImageView.Layer.ShadowOpacity = 0.5f;
            ChildImageView.Layer.ShadowRadius = 1.0f;
          
            ChildImageView.BackgroundColor = UIColor.Gray;

            if (childModel.IsSelect)
            {
                SelectElement();
            }
            else
            {
                DeselectElement();
            }

            if (childModel.IsAskForLeave)
            {
                ChildNameLabel.Text = ChildNameLabel.Text + ":" + "请假";
            }
        }
    }

    public class Header : UICollectionReusableView
    {
        public static NSString HeaderId = new NSString("ChildHeader");
        UILabel _label;

        public string Text
        {
            get
            {
                return _label.Text;
            }
            set
            {
                _label.Text = value;
                SetNeedsDisplay();
            }
        }

        [Export("initWithFrame:")]
        public Header(CGRect frame)
            : base(frame)
        {
            _label = new UILabel()
            {
                Frame = new CGRect(10, 0, frame.Width, EasyLayout.NormalTextFieldHeight),
//                BackgroundColor = UIColor.Yellow
                TextColor = MvxTouchColor.Black1
            };
            _label.TextAlignment = UITextAlignment.Center;
            AddSubview(_label);
        }
    }
}

