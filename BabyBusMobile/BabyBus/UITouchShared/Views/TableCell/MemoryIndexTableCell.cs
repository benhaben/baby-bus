using System;
using UIKit;
using Foundation;
using BabyBus.iOS;
using CoreGraphics;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using MWPhotoBrowserBinding;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
	public class MemoryIndexTableCell :UITableViewCell
	{
		public static readonly NSString Key = new NSString("MemoryIndexTableCell");
		private bool didSetupConstraints;

		public MemoryIndexTableCell()
		{
			ContentView.Add(ImageView);
			ContentView.Add(Title);
			ContentView.Add(Content);
			ContentView.Add(CollectionView9Picture);
			SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		UIImageView _image = null;

		public virtual UIImageView ImageView {
			get {
				if (_image == null) {
					_image = new UIImageView();
					_image.Layer.CornerRadius = 20;
					_image.Layer.MasksToBounds = false;
					_image.ContentMode = UIViewContentMode.ScaleAspectFill;
					_image.ClipsToBounds = true;
				}
				return _image;
			}
		}

		UILabel _title = null;

		public UILabel Title {
			get {
				if (_title == null) {   
					_title = new UILabel();
					_title.Font = EasyLayout.TitleFont;
					_title.TextColor = MvxTouchColor.Black1;
				}
				return _title;
			}
		}

		UILabel _content = null;

		public UILabel Content {
			get {
				if (_content == null) {   
					_content = new UILabel();
					_content.Font = EasyLayout.SmallFont;
					_content.TextColor = MvxTouchColor.Gray1;
				}
				return _content;
			}
		}

		UICollectionView _collectionView9Picture = null;

		public UICollectionView CollectionView9Picture {
			get {
				if (_collectionView9Picture == null) {
					// Flow Layout
					var flowLayout = new UICollectionViewFlowLayout() {
						HeaderReferenceSize = new CGSize(0, 0),
//                        SectionInset = new UIEdgeInsets(2, 2, 2, 2),
//                        ScrollDirection = UICollectionViewScrollDirection.Vertical,
						MinimumInteritemSpacing = 1, // minimum spacing between cells
						MinimumLineSpacing = 1, // minimum spacing between rows if ScrollDirection is Vertical or between columns if Horizontal
						ItemSize = new CGSize(80, 80),
					};

					//frame is setted in the SetUpConstrainLayout
					_collectionView9Picture = new UICollectionView(new CGRect(0, 0, 0, 0), flowLayout);
					_collectionView9Picture.ContentInset = new UIEdgeInsets(1, 1, 1, 1);
					_collectionView9Picture.BackgroundColor = UIColor.Clear;
					_collectionView9Picture.RegisterClassForCell(typeof(MWImageCollecntionViewCell), MWImageCollecntionViewCell.CellID);
					_collectionView9Picture.RegisterClassForSupplementaryView(typeof(Header), UICollectionElementKindSection.Header, Header.HeaderId);
					_collectionView9Picture.ShowsHorizontalScrollIndicator = false;
					_collectionView9Picture.UserInteractionEnabled = true;
				}
				return _collectionView9Picture;
			}
		}

		NSLayoutConstraint _heightConstrainCollectionView9Picture;
		NSLayoutConstraint _widthConstrainCollectionView9Picture;
		NSLayoutConstraint _heightConstrainContent;

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();
			if (this.didSetupConstraints) {
				return;
			}
			nfloat PhoneImageWidth = 244;

			this.ContentView.ConstrainLayout(
				() =>
                this.ImageView.Frame.Top == this.ContentView.Frame.Top + EasyLayout.MarginMedium
				&& this.ImageView.Frame.Left == this.ContentView.Frame.Left + EasyLayout.MarginNormal
				&& this.ImageView.Frame.Height == EasyLayout.HeadPortraitImageHeight
				&& this.ImageView.Frame.Width == EasyLayout.HeadPortraitImageHeight

				&& this.Title.Frame.Top == this.ContentView.Frame.Top + EasyLayout.MarginSmall
				&& this.Title.Frame.Left == this.ImageView.Frame.Right + EasyLayout.MarginMedium
				&& this.Title.Frame.Right == this.ContentView.Frame.Right - EasyLayout.MarginSmall
				&& this.Title.Frame.Height == EasyLayout.NormalTextFieldHeight

				&& this.Content.Frame.Top == this.Title.Frame.Bottom
				&& this.Content.Frame.Left == this.Title.Frame.Left
				&& this.Content.Frame.Right == this.ContentView.Frame.Right - EasyLayout.MarginSmall

				&& this.CollectionView9Picture.Frame.Top == this.Content.Frame.Bottom
				&& this.CollectionView9Picture.Frame.Left == this.Content.Frame.Left 
			);
			//Note: do not change frame, change Constrain to resize control
			var constrains =
				ContentView.ConstrainLayout(
					() => this.CollectionView9Picture.Frame.Height == PhoneImageWidth
					&& this.Content.Frame.Height == EasyLayout.SmallTextFieldHeight
					&& this.CollectionView9Picture.Frame.Width == PhoneImageWidth

				);
			_heightConstrainCollectionView9Picture = constrains[0];
			_heightConstrainContent = constrains[1];
			_widthConstrainCollectionView9Picture = constrains[2];

			this.didSetupConstraints = true;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			this.ContentView.SetNeedsLayout();
			this.ContentView.LayoutIfNeeded();
			this.ContentView.UpdateConstraintsIfNeeded();

			if (string.IsNullOrWhiteSpace(this.Content.Text)) {
				_heightConstrainContent.Constant = 0f;
			}
			var data = this.CollectionView9Picture.DataSource as CollectionView9PictureDataSource;
			nfloat imageheight = 90f;

			if (data != null) {

				if (data.Count <= 0) {
					_heightConstrainCollectionView9Picture.Constant = 0f;
				} else if (data.Count > 0 && data.Count <= 3) {
					_heightConstrainCollectionView9Picture.Constant = imageheight;
				} else if (data.Count > 3 && data.Count <= 6) {
					_heightConstrainCollectionView9Picture.Constant = imageheight * 2;

				} else if (data.Count > 6 && data.Count <= 9) {
					_heightConstrainCollectionView9Picture.Constant = imageheight * 3;
				}
			}

		}

		public class Header : UICollectionReusableView
		{
			public static NSString HeaderId = new NSString("ImageHeader");
			UILabel _label;

			public string Text {
				get {
					return _label.Text;
				}
				set {
					_label.Text = value;
					SetNeedsDisplay();
				}
			}

			[Export("initWithFrame:")]
			public Header(CGRect frame)
				: base(frame)
			{
				_label = new UILabel() {
					Frame = new CGRect(10, 0, frame.Width, EasyLayout.NormalTextFieldHeight),
					//                BackgroundColor = UIColor.Yellow
				};
				_label.TextAlignment = UITextAlignment.Center;
				AddSubview(_label);
			}
		}

		public class CollectionView9PictureDataSource : UICollectionViewSource
		{
			public CollectionView9PictureDataSource()
			{
				ImageNameList = new List<string>();
			}

			public string Caption{ get; set; }

			private Header _headerView;

			public List<string> ImageNameList { get; set; }

			public int Count { get { return ImageNameList != null ? ImageNameList.Count : 0; } }

			public void SetImageList(List<string> list)
			{
				ImageNameList.Clear();
				ImageNameList = list;
			}

			public CGSize ImageViewSize { get; set; }

			public override nint NumberOfSections(UICollectionView collectionView)
			{
				return 1;
			}

			public override nint GetItemsCount(UICollectionView collectionView, nint section)
			{
				if (ImageNameList != null) {
					return ImageNameList.Count;
				} else {
					return 0;
				}
			}

			public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
			{
				_headerView = (Header)collectionView.DequeueReusableSupplementaryView(elementKind, Header.HeaderId, indexPath);
//                _headerView.Text = string.Format("全班{0}人，已经签到{1}人。", Children.Count, SelectedChildren.Count);
				return _headerView;
			}

			public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
			{
				if (indexPath.Row < ImageNameList.Count) {
                   

					List<string> imageNameList = new List<string>(ImageNameList);
					var firstImageName = ImageNameList[indexPath.Row];
					imageNameList.RemoveAt(indexPath.Row);
					imageNameList.Insert(0, firstImageName);

					List<MWPhoto> photos = new List<MWPhoto>();

					foreach (var imageName in imageNameList) {
						string url = Constants.ImageServerPath + imageName;
						var photo = MWPhoto.PhotoWithURL(NSUrl.FromString(url));
						photo.Caption = Caption;
						photos.Add(photo);
					}
                  
					var messenger = Mvx.Resolve<IMvxMessenger>();
					SelectImagesMvxMessage msg = new SelectImagesMvxMessage(this, typeof(SelectImagesMvxMessage));
					msg.ImagesList = photos;
					messenger.Publish<SelectImagesMvxMessage>(msg);
				}
				return;
			}

			public class SelectImagesMvxMessage : MvxSubscriberChangeMessage
			{
               

				public SelectImagesMvxMessage(object sender, Type messageType, int countSubscribers = 0)
					: base(sender, messageType, countSubscribers)
				{
				}

				public List<MWPhoto> ImagesList { get; set; }

			}

			public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
			{
				return true;
			}


			public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
			{
				var cell = (MWImageCollecntionViewCell)collectionView.DequeueReusableCell(MWImageCollecntionViewCell.CellID, indexPath);
				if (indexPath.Row >= 0 && indexPath.Row < ImageNameList.Count) {
					string url = Constants.ThumbServerPath + ImageNameList[indexPath.Row] + Constants.ThumbRule;

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
}

