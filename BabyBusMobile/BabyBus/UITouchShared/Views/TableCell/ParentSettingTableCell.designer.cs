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
    [Register("ParentSettingTableCell")]
    public partial class ParentSettingTableCell
    {
        [Outlet]
        UIKit.UILabel ClassName { get; set; }

        [Outlet]
        UIKit.UILabel KindergartenName { get; set; }

        [Outlet]
        UIKit.UIImageView MasterImage { get; set; }

        [Outlet]
        UIKit.UIButton MasterPhone { get; set; }

        [Outlet]
        UIKit.UILabel MasterTitleAndName { get; set; }

        [Outlet]
        UIKit.UIImageView TeacherImage { get; set; }

        [Outlet]
        UIKit.UIButton TeacherPhone { get; set; }

        [Outlet]
        UIKit.UILabel TeacherTitleAndName { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (MasterTitleAndName != null)
            {
                MasterTitleAndName.Dispose();
                MasterTitleAndName = null;
            }

            if (TeacherTitleAndName != null)
            {
                TeacherTitleAndName.Dispose();
                TeacherTitleAndName = null;
            }

            if (MasterPhone != null)
            {
                MasterPhone.Dispose();
                MasterPhone = null;
            }

            if (TeacherPhone != null)
            {
                TeacherPhone.Dispose();
                TeacherPhone = null;
            }

            if (ClassName != null)
            {
                ClassName.Dispose();
                ClassName = null;
            }

            if (KindergartenName != null)
            {
                KindergartenName.Dispose();
                KindergartenName = null;
            }

            if (MasterImage != null)
            {
                MasterImage.Dispose();
                MasterImage = null;
            }

            if (TeacherImage != null)
            {
                TeacherImage.Dispose();
                TeacherImage = null;
            }
        }
    }
}
