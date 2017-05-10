using Foundation;
using UIKit;
using CrossUI.Touch.Dialog.Elements;
using CrossUI.Touch.Dialog;
using BabyBus.iOS;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class ParentSettingElement 
        : Element
        , CrossUI.Touch.Dialog.Elements.IElementSizing
    {
        SettingViewModel _parentSettingModel;

        public ParentSettingElement(SettingViewModel parentSettingModel, UIViewController vc)
            : base("", vc)
        {
            _parentSettingModel = parentSettingModel;
        }

        protected override  UITableViewCell GetCellImpl(UITableView tv)
        {
            ParentSettingTableCell cell = tv.DequeueReusableCell(ParentSettingTableCell.Key) as ParentSettingTableCell ??
                                          ParentSettingTableCell.Create();
            cell.InitWithModel(_parentSettingModel);
            return cell  as UITableViewCell;
        }

        public virtual float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return  160;
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            base.Selected(dvc, tableView, path);
        }

    }
}

