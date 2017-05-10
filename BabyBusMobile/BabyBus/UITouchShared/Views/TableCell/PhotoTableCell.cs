
using System;

using Foundation;
using UIKit;


namespace BabyBus.iOS
{
    public partial class PhotoTableCell : UITableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("PhotoTableCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("PhotoTableCell");

        const  int ORIGINAL_MAX_WIDTH = 1080;
        public static  float WIDTHOFIMAGE = 78.0f;

        public PhotoTableCell(IntPtr handle)
            : base(handle)
        {
        }

        //        void RoundImage() {
        //            ChildPhoto.Layer.CornerRadius = (ChildPhoto.Frame.Size.Height / 2);
        //            ChildPhoto.Layer.MasksToBounds = false;
        //            ChildPhoto.ContentMode = UIViewContentMode.ScaleAspectFill;
        //            ChildPhoto.ClipsToBounds = true;
        //            ChildPhoto.Layer.ShadowColor = UIColor.Black.CGColor;
        //            ChildPhoto.Layer.ShadowOffset = new CGSize(4, 4);
        //            ChildPhoto.Layer.ShadowOpacity = 0.5f;
        //            ChildPhoto.Layer.ShadowRadius = 1.0f;
        //            ChildPhoto.Layer.BorderColor = UIColor.Black.CGColor;
        //            ChildPhoto.Layer.BorderWidth = 1.0f;
        //            ChildPhoto.UserInteractionEnabled = true;
        //            ChildPhoto.BackgroundColor = UIColor.Black;
        //        }
        //
        //        public void InitWithViewController(UIViewController viewController) {
        ////			var viewController = GetParentTableView (this).DataSource as UIViewController;
        //            RoundImage();
        //        }
        //
        public static PhotoTableCell Create()
        {
            var photoTableCell = (PhotoTableCell)Nib.Instantiate(null, null)[0];
            return photoTableCell;
        }

        public UIKit.UIImageView CameraButtonUIImageView { get { return CameraButton; } }

        public UIKit.UILabel ChildDescriptionUILabel { get { return ChildDescription; } }

        public UIKit.UILabel ChildNameUILabel { get { return ChildName; } }

        public UIKit.UIImageView ChildPhotoUIImageView { get { return ChildPhoto; } }
       
    }
}

