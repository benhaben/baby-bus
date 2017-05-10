// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BabyBus.iOS
{
    [Register("PhotoTableCell")]
    partial class PhotoTableCell
    {
        [Outlet]
        UIKit.UIImageView CameraButton { get; set; }

        [Outlet]
        UIKit.UILabel ChildDescription { get; set; }

        [Outlet]
        UIKit.UILabel ChildName { get; set; }

        [Outlet]
        UIKit.UIImageView ChildPhoto { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (ChildPhoto != null)
            {
                ChildPhoto.Dispose();
                ChildPhoto = null;
            }

            if (ChildName != null)
            {
                ChildName.Dispose();
                ChildName = null;
            }

            if (ChildDescription != null)
            {
                ChildDescription.Dispose();
                ChildDescription = null;
            }

            if (CameraButton != null)
            {
                CameraButton.Dispose();
                CameraButton = null;
            }
        }
    }
}
