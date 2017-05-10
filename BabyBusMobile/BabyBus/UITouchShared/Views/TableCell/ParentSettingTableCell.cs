
using System;
using Foundation;
using UIKit;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public partial class ParentSettingTableCell : UITableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("ParentSettingTableCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("ParentSettingTableCell");

        public ParentSettingTableCell(IntPtr handle)
            : base(handle)
        {
        }

        public static ParentSettingTableCell Create()
        {
            return (ParentSettingTableCell)Nib.Instantiate(null, null)[0];
        }

        public void InitWithModel(SettingViewModel parentSettingModel)
        {
            KindergartenName.Text = parentSettingModel.KindergartenName;
            ClassName.Text = parentSettingModel.ClassName;
//            MasterTitleAndName.Text = parentSettingModel.;
//            TeacherTitleAndName.Text = parentSettingModel.TeacherTitleAndName;
//            MasterPhone.TitleLabel.Text = parentSettingModel.MasterPhone;
//            TeacherPhone.TitleLabel.Text = parentSettingModel.TeacherPhone;
            //TODO: picture

            MasterPhone.TouchUpInside += (object sender, EventArgs e) =>
            {
                var tel = "tel://" + MasterPhone.TitleLabel.Text;
                UIApplication.SharedApplication.OpenUrl(new NSUrl(tel));
            };

            TeacherPhone.TouchUpInside += (object sender, EventArgs e) =>
            {
                var tel = "tel://" + TeacherPhone.TitleLabel.Text;
                UIApplication.SharedApplication.OpenUrl(new NSUrl(tel));
            };
        }
    }
}

