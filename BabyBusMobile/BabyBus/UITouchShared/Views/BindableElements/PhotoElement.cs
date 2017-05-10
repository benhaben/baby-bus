using System;
using Foundation;
using UIKit;
using CrossUI.Touch.Dialog;

namespace BabyBus.iOS
{
    public class PhotoElement : NameCardPhotoElement
    {
        public PhotoElement(UIViewController vc)
            : base()
        {
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
//			base.Selected (dvc, tableView, path);
            editPortrait(dvc);
        }

        void editPortrait(DialogViewController dvc)
        {
            UIActionSheet actionSheet = new UIActionSheet(
                                            null, null, "取消", null,
                                            new string[] { "拍照", "从相册中选取" });
            actionSheet.ShowInView(dvc.View);

            actionSheet.Clicked += (object sender, UIButtonEventArgs e) =>
            {
                var buttonIndex = e.ButtonIndex;
                if (buttonIndex == 0)
                {
                    // 拍照
                    TakePictureWithCropCommand.Execute();
                    //
                }
                else if (buttonIndex == 1)
                {
                    // 从相册中选取
                    ChoosePictureWithCropCommand.Execute();
                }
                else
                {
                    actionSheet.Hidden = true;
                }
            };
        }
    }
}

