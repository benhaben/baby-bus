using System;
using BabyBus.iOS;
using BabyBus.Logic.Shared;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using SDWebImage;

namespace BabyBus.iOS
{
	public class MIChildrenView : MvxBabyBusBaseAutoLayoutViewController
	{
		MIChildrenViewModel _baseViewModel;

		public MIChildrenView()
		{
			AddGestureWhenTap = false;
		}

		nfloat _cellWidth = 70f;
		nfloat _imageViewSize = 60f;

		UICollectionView _collectionViewChildren = null;

		public UICollectionView CollectionViewChildren {
			get { 
				if (_collectionViewChildren == null) {
					nfloat marginSection = 2f;

					// Flow Layout
					var flowLayout = new UICollectionViewFlowLayout() {

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
					_collectionViewChildren.RegisterClassForCell(typeof(MIChildCell), MIChildCell.CellID);
					_collectionViewChildren.RegisterClassForSupplementaryView(typeof(MIHeader), UICollectionElementKindSection.Header, MIHeader.HeaderId);
					_collectionViewChildren.ShowsHorizontalScrollIndicator = false;
				}
				return _collectionViewChildren;
			}
		}

		MIChildDataSource _userSource;

		public override void PrepareViewHierarchy()
		{
			base.PrepareViewHierarchy();

			UIView[] v = {
				CollectionViewChildren,
			};

			Container.AddSubviews(v);
		}

		public override void SetUpConstrainLayout()
		{
			base.SetUpConstrainLayout();

			nfloat height = 480;

			View.ConstrainLayout(
				() => 
					CollectionViewChildren.Frame.Height == height
				&& CollectionViewChildren.Frame.Left == Container.Frame.Left
				&& CollectionViewChildren.Frame.Right == Container.Frame.Right
				&& CollectionViewChildren.Frame.Top == Container.Frame.Top
				&& Container.Frame.Bottom == CollectionViewChildren.Frame.Bottom
			);
			CollectionViewChildren.ScrollEnabled = true;
		}

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();

			Title = "多元智能";
			View.BackgroundColor = UIColor.White;
			_baseViewModel = ViewModel as MIChildrenViewModel;
			_userSource = new MIChildDataSource(_baseViewModel);
			_userSource.FontSize = 11f;
			_userSource.ImageViewSize = new CGSize(_imageViewSize, _imageViewSize);

			CollectionViewChildren.UserInteractionEnabled = true;

			CollectionViewChildren.Source = _userSource;
			CollectionViewChildren.ReloadData();
			CollectionViewChildren.AllowsSelection = true;

			_baseViewModel.FirstLoadedEventHandler += (sender, e) => InvokeOnMainThread(() => {
				if (!_baseViewModel.IsRunning) {
					_userSource.SetChildren(_baseViewModel.TestMasters);
					CollectionViewChildren.ReloadData();
				}
			});

			_baseViewModel.MasterMessageChange += (sender, e) => InvokeOnMainThread(() => {
				CollectionViewChildren.ReloadData();
			});

		}
	}

	public class MIChildDataSource : UICollectionViewSource
	{
		MIChildrenViewModel _viewModel;

		public MIChildDataSource(MIChildrenViewModel viewModel)
		{
			Children = new List<MITestMaster>();
			_viewModel = viewModel;
		}

		private MIHeader _headerView;

		private List<MITestMaster> Children{ get; set; }

		public void SetChildren(List<MITestMaster> list)
		{
			Children.Clear();
			Children = list;
		}

		public Single FontSize{ get; set; }

		public CGSize ImageViewSize { get; set; }

		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return Children.Count;
		}

		public override bool ShouldSelectItem(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			var cell = collectionView.CellForItem(indexPath) as MIChildCell;
			var model = Children[indexPath.Row];

			_viewModel.ShowDetailCommand(model.ModalityId, model.TestMasterId, model.ChildId);

			//跳转到选领域界面
			return true;
		}

		public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
		{
			_headerView = (MIHeader)collectionView.DequeueReusableSupplementaryView(elementKind, Header.HeaderId, indexPath);
			int completedCount = 0;
			foreach (var child in Children) {
				if (child.IsFinished) {
					completedCount++;
				}
			}
			_headerView.Text = string.Format("全班{0}人，{1}完成测试。", Children.Count, completedCount);

			return _headerView;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var cell = (MIChildCell)collectionView.DequeueReusableCell(MIChildCell.CellID, indexPath);

			var childModel = Children[indexPath.Row];

			cell.UpdateRow(childModel, FontSize, ImageViewSize);

			return cell;
		}
	}

	public class MIChildCell : UICollectionViewCell
	{
		public static NSString CellID = new NSString("MIChildCell");

		UILabel _childName = null;

		public UILabel ChildName {
			get { 
				if (_childName == null) {
					_childName = new UILabel();
					_childName.Text = "孩子姓名";
					_childName.Font = EasyLayout.TinyFont;
					_childName.TextColor = MvxTouchColor.Black1;
					_childName.TextAlignment = UITextAlignment.Center;
				}
				return _childName;
			}
			set { 
				_childName = value;
			}
		}

		UIImageView _childImage = null;

		public UIImageView ChildImageView {
			get { 
				if (_childImage == null) {
					_childImage = new UIImageView();
					_childImage.Image = UIImage.FromBundle("placeholder.png");
				}
				return _childImage;
			}
		}

		UILabel _scoreLabel = null;

		public UILabel ScoreLabel {
			get { 
				if (_scoreLabel == null) {
					_scoreLabel = new UILabel();
					_scoreLabel.Text = "-/-";
					_scoreLabel.Font = EasyLayout.TinyFont;
					_scoreLabel.TextColor = MvxTouchColor.Black1;
					_scoreLabel.TextAlignment = UITextAlignment.Right;
				}
				return _scoreLabel;
			}
		}

		UILabel _paidLabel = null;

		public UILabel PaidLabel {
			get { 
				if (_paidLabel == null) {
					_paidLabel = new UILabel();
					_paidLabel.Lines = 1;
					_paidLabel.Font = EasyLayout.TinyFont;
					_paidLabel.Text = "已付费";
					_paidLabel.TextColor = MvxTouchColor.White;
					_paidLabel.BackgroundColor = MvxTouchColor.Green1;
				}
				return _paidLabel;
			}
		}

		UILabel _unPayLabel = null;

		public UILabel UnPayLabel {
			get { 
				if (_unPayLabel == null) {
					_unPayLabel = new UILabel();
					_unPayLabel.Lines = 1;
					_unPayLabel.Font = EasyLayout.TinyFont;
					_unPayLabel.Text = "未付费";
					_unPayLabel.TextColor = MvxTouchColor.White;
					_unPayLabel.BackgroundColor = MvxTouchColor.Red1;
				}
				return _unPayLabel;
			}
		}

		[Export("initWithFrame:")]
		public MIChildCell(CGRect frame)
			: base(frame)
		{
			ChildImageView.Frame = new CGRect(5, 0, 60, 60);
			ChildName.Frame = new CGRect(0, 60, 70, 20);
			ScoreLabel.Frame = new CGRect(40, 50, 30, 20);
			PaidLabel.Frame = new CGRect(40, 0, 30, 16);
			UnPayLabel.Frame = new CGRect(40, 0, 30, 16);

			UIView[] v = {
                    ChildImageView,
                    ChildName,
                    ScoreLabel,
                    PaidLabel,
                    UnPayLabel
                };

            ContentView.AddSubviews(v);
        }

        public void UpdateRow(MITestMaster childModel, Single fontSize, CGSize imageViewSize)
        {
            ChildName.Text = childModel.ChildName;

            var uri = new Uri(Constants.ThumbServerPath + childModel.ImageName + Constants.ThumbRule);

            ChildImageView.SetImage(uri, UIImage.FromBundle("placeholder.png"));

            ChildImageView.Layer.CornerRadius = (imageViewSize.Height / 2);
            ChildImageView.Layer.MasksToBounds = false;
            ChildImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            ChildImageView.ClipsToBounds = true;
            ChildImageView.Layer.ShadowColor = UIColor.Black.CGColor;
            ChildImageView.Layer.ShadowOffset = new CGSize(4, 4);
            ChildImageView.Layer.ShadowOpacity = 0.5f;
            ChildImageView.Layer.ShadowRadius = 1.0f;

            ChildImageView.BackgroundColor = UIColor.Gray;

            ScoreLabel.Text = string.Format("{0}/{1}", childModel.CompletedTest, childModel.TotalTest);

            if (childModel.IsMember)
            {
                UnPayLabel.Hidden = true;
                PaidLabel.Hidden = false;

            }
            else
            {
                UnPayLabel.Hidden = false;
                PaidLabel.Hidden = true;
            }
        }
    }

    public class MIHeader : UICollectionReusableView
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
        public MIHeader(CGRect frame)
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

