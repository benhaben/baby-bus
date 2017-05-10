using Foundation;
using UIKit;
using CrossUI.Touch.Dialog.Elements;
using CrossUI.Touch.Dialog;
using BabyBus.iOS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using Utilities.Touch;
using SDWebImage;

namespace BabyBus.iOS
{
    public class NameCardPhotoElement : StyledMultilineElement , IBindableElement
    {
        public IMvxBindingContext BindingContext { get; set; }


        public NameCardPhotoElement()
            : base("", "", UITableViewCellStyle.Subtitle)
        {
            this.CreateBindingContext();
            this.LineBreakMode = UILineBreakMode.TailTruncation;
            this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            this.Lines = 1;
            this.DefaultImageName = "placeholder.png";

            Font = EasyLayout.TitleFont;
            SubtitleFont = EasyLayout.SubTitleFont;
            TextColor = MvxTouchColor.Black1;
            DetailColor = MvxTouchColor.Black1;
        }

        #region IBindableElement implementation

        void IBindableElement.DoBind()
        {
            //Note: don't need use in the list now 
//            this.DelayBind(() => {
//                var set = this.CreateBindingSet<NameCardPhotoElement, SettingViewModel>();
//                set.Bind().For(me => me.Name).To(p => p.ChildName);
//                set.Bind().For(me => me.ImageUri).To(p => p.ImageName).WithConversion("StringToUriThumb");
//                set.Bind().For(me => me.ChoosePictureWithCropCommand).To(p => p.ChoosePictureWithCropCommand);
//                set.Bind().For(me => me.TakePictureWithCropCommand).To(p => p.TakePictureWithCropCommand);
//                set.Apply();
//            });
        }


        public virtual object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        #endregion

        MvxCommand _choosePictureWithCropCommand;
        MvxCommand _takePictureWithCropCommand;

        public MvxCommand ChoosePictureWithCropCommand { get { return _choosePictureWithCropCommand; } set { _choosePictureWithCropCommand = value; } }

        public MvxCommand TakePictureWithCropCommand { get { return _takePictureWithCropCommand; } set { _takePictureWithCropCommand = value; } }

        public string Name{ get; set; }

        public string DescriptionOfChild{ get; set; }

        //TODO: reflator - duplicate code
        void AddImage(UIImageView imgView, UIImage img)
        {
            if (img != null)
            {
                //                        imgView.Image = img;
                imgView.Image = img.ImageByScalingToMaxSize(ImageHeight());
                //xib imgView height is 50
                imgView.Layer.CornerRadius = 25;
                imgView.Layer.MasksToBounds = false;
                imgView.ContentMode = UIViewContentMode.ScaleAspectFill;
                imgView.ClipsToBounds = true;
            }
        }

        protected override void PrepareCell(UITableViewCell cell)
        {
            if (cell == null)
                return;

            var photoTableCell = cell as PhotoTableCell;
            cell.Accessory = Accessory;
            var imgView = photoTableCell.ChildPhotoUIImageView;
          

            if (ImageUri != null)
            {
                imgView.SetImage(ImageUri, UIImage.FromBundle(DefaultImageName), 0,
                    delegate(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl url)
                    { 
                        if (image != null)
                        {
                            AddImage(imgView, image);
                        }
                    });
            }
            else
            {
                var img = UIImage.FromBundle(DefaultImageName);
                AddImage(imgView, img);
            }

//            var img = ImageLoader.DefaultRequestImage(ImageUri, this);
//            if (img == null)
//                img = UIImage.FromBundle("Dialog/Images/" + DefaultImageName);
//            else {
//                photoTableCell.ChildPhotoUIImageView.Image = img;
//                var imgView = photoTableCell.ChildPhotoUIImageView;
//                imgView.Image = img.ImageByScalingToMaxSize(ImageHeight());
//                imgView.Layer.CornerRadius = (imgView.Image.Size.Height / 2);
//                imgView.Layer.MasksToBounds = false;
//                imgView.ContentMode = UIViewContentMode.ScaleAspectFill;
//                imgView.ClipsToBounds = true;
//            }
            photoTableCell.ChildNameUILabel.Font = EasyLayout.ContentFont;
            photoTableCell.ChildNameUILabel.TextColor = MvxTouchColor.Black1;
            photoTableCell.ChildNameUILabel.Text = Name;
            photoTableCell.ChildDescriptionUILabel.Text = DescriptionOfChild;
            photoTableCell.ChildDescriptionUILabel.Font = EasyLayout.ContentFont;
            photoTableCell.ChildDescriptionUILabel.TextColor = MvxTouchColor.Black1;
        }

        public override float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return 60;
        }

        protected override  NSString CellKey
        {
            get { return PhotoTableCell.Key; }
        }

        protected override  UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey) ??
                       PhotoTableCell.Create() as UITableViewCell;

            //Note : set transparent of cell
//			cell.BackgroundColor = UIColor.Clear;
//			cell.BackgroundColor = new UIColor (255.0f, 255.0f, 255.0f, 0.03f);
//			cell.BackgroundColor = UIColor.
            return cell;
        }

        /// <summary>
        /// Returns a summary of the value represented by this object, suitable 
        /// for rendering as the result of a RootElement with child objects.
        /// </summary>
        /// <returns>
        /// The return value must be a short description of the value.
        /// </returns>
        public override string Summary()
        {
            return "";
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            base.Selected(dvc, tableView, path);
            var infoView = new InfoView();
            dvc.NavigationController.PushViewController(infoView, true);
//			dvc.PresentViewController (new InfoView (), true, null);
        }

    }
    //class
}

