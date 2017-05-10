using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Dialog.Touch;
using CoreGraphics;
using CrossUI.Touch.Dialog;
using CrossUI.Touch.Dialog.Elements;
using Foundation;
using SDWebImage;
using UIKit;
using Utilities.Touch;
using BabyBus.Logic.Shared;
using Newtonsoft.Json;

namespace BabyBus.iOS
{

	public class ChildInfomationElement
            : StyledMultilineElement
        , IBindableElement
	{
		public IMvxBindingContext BindingContext { get; set; }

		public ChildInfomationElement()
			: base("", "", UITableViewCellStyle.Subtitle)
		{

			this.CreateBindingContext();
			this.LineBreakMode = UILineBreakMode.TailTruncation;
			this.Accessory = UITableViewCellAccessory.None;
			this.Lines = 1;
			this.DefaultImageName = "placeholder.png";
		}

		public virtual void DoBind() {
			//TODO: WithFallback is not work, fix it later
			this.DelayBind(() => {
				var set = this.CreateBindingSet<ChildInfomationElement, ChildModel>();
				set.Bind().For(me => me.Caption).To(p => p.ChildName);
				set.Bind().For(me => me.IsAskForLeave).To(p => p.IsAskForLeave);
				set.Bind().For(me => me.IsRead).To(p => p.IsRead);
				set.Bind().For(me => me.ImageUri).To(p => p.ImageName).WithConversion("StringToUriThumb");
				set.Bind().For(me => me.PhoneNumber).To(p => p.PhoneNumber);
				set.Apply();
			});
		}

		//must set prop is public
		public string PhoneNumber{ get; set; }

		public bool IsRead {
			get;
			set;
		}

		public bool IsAskForLeave {
			get;
			set;
		}

		//TODO: reflator - duplicate code
		void AddImage(UIImageView imgView, UIImage img) {
			if (img != null) {
				//                        imgView.Image = img;
				imgView.Image = img.ImageByScalingToMaxSize(ImageHeight());
			}
		}

		public enum ChildInfoType
		{
			AskForLeave,
			IsRead,
			None
		}

		public static ChildInfoType ChildInfoTypeStatic = ChildInfoType.None;

		public bool HideSeparator = false;


		protected override void PrepareCell(UITableViewCell cell) {
			if (cell == null)
				return;

			var childInfomationTableCell = cell as ChildInfomationTableCell;
			cell.Accessory = Accessory;

			if (HideSeparator) {
				childInfomationTableCell.SeparatorView.Hidden = true;
			}

			var imgView = childInfomationTableCell.ChildImageView;
			if (ImageUri != null) {
				imgView.SetImage(ImageUri, UIImage.FromBundle(DefaultImageName), 0,
					delegate(UIImage img, NSError error, SDImageCacheType cacheType, NSUrl arg3) { 
						if (img != null) {
							AddImage(imgView, img);
						}
					});
			} else {
				var img = UIImage.FromBundle(DefaultImageName);
				AddImage(imgView, img);
			}
            
			childInfomationTableCell.ChildName.Text = Caption;
			childInfomationTableCell.PhoneNumber = PhoneNumber;

			if (ChildInfoTypeStatic == ChildInfoType.AskForLeave) {
				childInfomationTableCell.IsAskForLeave.Text = IsAskForLeave ? "请假" : "缺勤";
				childInfomationTableCell.IsAskForLeave.TextColor = IsAskForLeave ? MvxTouchColor.BrightGreen : MvxTouchColor.LightRed;
				childInfomationTableCell.IsAskForLeave.Hidden = false;

			} else if (ChildInfoTypeStatic == ChildInfoType.IsRead) {
				childInfomationTableCell.IsAskForLeave.Text = IsRead ? "已读" : "未读";
				childInfomationTableCell.IsAskForLeave.TextColor = IsRead ? MvxTouchColor.BrightGreen : MvxTouchColor.LightRed;
				childInfomationTableCell.IsAskForLeave.Hidden = false;

			} else {
				childInfomationTableCell.IsAskForLeave.Text = "";
				childInfomationTableCell.IsAskForLeave.Hidden = true;
			}
		}

		public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path) {
			base.Selected(dvc, tableView, path);
			var dialogVC = dvc as MvxDialogViewController;
			var row = path.Row;

			var childrenViewModel = dialogVC.ViewModel as ChildrenViewModel;
			if (childrenViewModel != null) {
				var children = childrenViewModel.Children;
				if (row >= 0 && row <= children.Count) {
					childrenViewModel.SelectedCheckoutJson = JsonConvert.SerializeObject(children[row]);
				}
				childrenViewModel.ShowDetailCommand.Execute();
				return;
			}
		}

		public override float GetHeight(UITableView tableView, NSIndexPath indexPath) {

			var maxSize = new CGSize(tableView.Bounds.Width - 40, float.MaxValue);

			if (this.Accessory != UITableViewCellAccessory.None)
				maxSize.Width -= 20;
			Font = Font ?? UIFont.SystemFontOfSize(14);

			//            TODO:multiply line test fail
			var height = UIStringDrawing.StringSize("s", Font, maxSize, LineBreakMode).Height;

			return  (float)(height + 35);
		}

		protected override UITableViewCell GetCellImpl(UITableView tv) {
			UITableViewCell cell;
			cell = tv.DequeueReusableCell(ChildInfomationTableCell.Key) ?? (new ChildInfomationTableCell() as UITableViewCell);
			PrepareCell(cell);

			cell.SetNeedsUpdateConstraints();
			cell.UpdateConstraintsIfNeeded();
			return cell as UITableViewCell;
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				BindingContext.ClearAllBindings();
			}
			base.Dispose(disposing);
		}

		public virtual object DataContext {
			get { return BindingContext.DataContext; }
			set { BindingContext.DataContext = value; }
		}
	}
}

