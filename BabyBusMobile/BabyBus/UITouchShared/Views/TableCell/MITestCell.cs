using System;
using UIKit;
using Foundation;
using BabyBus.iOS;
using CoreGraphics;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using PatridgeDev;

namespace BabyBus.iOS
{
    public class MITestCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MITestCell");
        private bool didSetupConstraints;

        public MITestCell()
        {
            ContentView.Add(AssessIndexName);
            ContentView.Add(CollectionViewQuestions);
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        UILabel _assessIndexName;

        public UILabel AssessIndexName
        {
            get
            { 
                if (_assessIndexName == null)
                {
                    _assessIndexName = new UILabel();
                    _assessIndexName.Font = EasyLayout.TitleFontBold;
                    _assessIndexName.TextColor = MvxTouchColor.Blue1;

                    _assessIndexName.TextAlignment = UITextAlignment.Center;
                }
                return _assessIndexName;
            }
        }

        UICollectionView _collectionViewQuestions = null;

        public UICollectionView CollectionViewQuestions
        {
            get
            { 
                if (_collectionViewQuestions == null)
                {
                    // Flow Layout
                    var flowLayout = new UICollectionViewFlowLayout()
                    {
                        HeaderReferenceSize = new CGSize(0, 0),
                        MinimumLineSpacing = 0,
                        ItemSize = new CGSize(320, 80),
                    };

                    _collectionViewQuestions = new UICollectionView(new CGRect(0, 0, 0, 0), flowLayout);
                    //_collectionViewQuestions.ContentInset = new UIEdgeInsets(1, 1, 1, 1);

                    _collectionViewQuestions.BackgroundColor = UIColor.Clear;
                    _collectionViewQuestions.RegisterClassForCell(typeof(MIQuestionCollectionViewCell), MIQuestionCollectionViewCell.CellID);
                    _collectionViewQuestions.ShowsHorizontalScrollIndicator = false;
                    _collectionViewQuestions.UserInteractionEnabled = true;
                }
                return _collectionViewQuestions;
            }
        }

        NSLayoutConstraint _heightConstrainCollectionViewQuestions;
        NSLayoutConstraint _heightConstrainAssessIndexName;
        NSLayoutConstraint _heightConstrainView;

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            if (didSetupConstraints)
            {
                return;
            }

            ContentView.ConstrainLayout(
                () =>
				AssessIndexName.Frame.Top == ContentView.Frame.Top
                && AssessIndexName.Frame.Left == ContentView.Frame.Left
                && AssessIndexName.Frame.Right == ContentView.Frame.Right

                && CollectionViewQuestions.Frame.Top == AssessIndexName.Frame.Bottom + EasyLayout.MarginNormal
                && CollectionViewQuestions.Frame.Left == ContentView.Frame.Left
                && CollectionViewQuestions.Frame.Right == ContentView.Frame.Right
                && CollectionViewQuestions.Frame.Bottom == ContentView.Frame.Bottom
            );

            var constrains = 
                ContentView.ConstrainLayout(
                    () => AssessIndexName.Frame.Height == EasyLayout.NormalTextFieldHeight
                    && CollectionViewQuestions.Frame.Height == EasyLayout.NormalTextFieldHeight
                    && ContentView.Frame.Height == EasyLayout.NormalTextFieldHeight
                );
            _heightConstrainAssessIndexName = constrains[0];
            _heightConstrainCollectionViewQuestions = constrains[1];
            _heightConstrainView = constrains[2];

            didSetupConstraints = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.ContentView.SetNeedsLayout();
            this.ContentView.LayoutIfNeeded();
            this.ContentView.UpdateConstraintsIfNeeded();

            var data = CollectionViewQuestions.DataSource as CollectionViewQuestionsDataSource;
            nfloat questionHeight = 80f;

            if (data != null)
            {
                _heightConstrainCollectionViewQuestions.Constant = questionHeight * data.Count;
                _heightConstrainView.Constant = questionHeight * data.Count + EasyLayout.NormalTextFieldHeight;
            }
        }

        public class CollectionViewQuestionsDataSource : UICollectionViewSource
        {
            public CollectionViewQuestionsDataSource()
            {
                QuestionList = new List<MITestQuestion>();
            }

            public List<MITestQuestion> QuestionList{ get; set; }

            public int Count{ get { return QuestionList != null ? QuestionList.Count : 0; } }

            public void SetQuestions(List<MITestQuestion> list)
            {
                QuestionList.Clear();
                QuestionList = list;
            }

            public override nint NumberOfSections(UICollectionView collectionView)
            {
                return 1;
            }

            public override nint GetItemsCount(UICollectionView collectionView, nint section)
            {
                if (QuestionList != null)
                {
                    return QuestionList.Count;
                }
                else
                {
                    return 0;
                }
            }

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
            {
                var cell = (MIQuestionCollectionViewCell)collectionView.DequeueReusableCell(MIQuestionCollectionViewCell.CellID, indexPath);
                if (indexPath.Row >= 0 && indexPath.Row < Count)
                {
                    var model = QuestionList[indexPath.Row];
                    cell.UpdateRow(model);
                }
                return cell;
            }
        }
    }

    public class MIQuestionCollectionViewCell : UICollectionViewCell
    {
        public static NSString CellID = new NSString("MIQuestionCollectionViewCell");

        public MITestQuestion Question{ get; set; }

        UILabel _questionLabel;

        public UILabel QuestionLabel
        {
            get
            {
                if (_questionLabel == null)
                {
                    _questionLabel = new UILabel();
                    _questionLabel.Font = EasyLayout.SmallFont;
                    _questionLabel.Lines = 0;
                    _questionLabel.TextColor = MvxTouchColor.Black1;
                    _questionLabel.TextAlignment = UITextAlignment.Left;
                }
                return _questionLabel;
            }
        }

        PDRatingView _ratingView;

        public PDRatingView RatingView
        {
            get
            { 
                if (_ratingView == null)
                {
                    var ratingConfig = new RatingConfig(emptyImage: UIImage.FromBundle("rating_empty.png"),
                                           filledImage: UIImage.FromBundle("rating_chosen.png"),
                                           chosenImage: UIImage.FromBundle("rating_chosen.png"));

                    ratingConfig.ItemPadding = 5f;

                    var ratingFrame = new CGRect(CGPoint.Empty, new CGSize(240f, 40f));

                    _ratingView = new PDRatingView(ratingFrame, ratingConfig);
                    _ratingView.AverageRating = 0;

                    _ratingView.RatingChosen += (sender, e) =>
                    {
                        Question.Score = e.Rating;
                    };
                }
                return _ratingView;
            }
        }

        [Export("initWithFrame:")]
        public MIQuestionCollectionViewCell(CGRect frame)
            : base(frame)
        {
            QuestionLabel.Frame = new CGRect(20, 0, 280, 40);
            RatingView.Frame = new CGRect(20, 35, 240, 40);

            UIView[] v =
                {
                QuestionLabel,
                RatingView
            };

            ContentView.AddSubviews(v);
        }

        public void UpdateRow(MITestQuestion model)
        {
            Question = model;
            QuestionLabel.Text = model.Name;
            RatingView.AverageRating = model.Score;
        }
    }
}

