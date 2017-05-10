//using System;
//using CrossUI.Touch.Dialog.Elements;
//using BabyBus.Logic.Shared;
//using Cirrious.CrossCore;
//using Cirrious.MvvmCross.Touch.Views;
//using UIKit;
//
//namespace BabyBus.iOS
//{
//	public class PaymentElement : Element
//	{
//		public PaymentElement()
//			: this("")
//		{
//		}
//
//		public PaymentElement(string caption)
//			: base(caption)
//		{
//			
//		}
//
//		protected override UITableViewCell GetCellImpl(UITableView tv)
//		{
//			var cell = tv.DequeueReusableCell(CellKey);
//			if (cell == null) {
//				cell = new UITableViewCell(UITableViewCellStyle.Default, CellKey);
//				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
//				cell.TextLabel.Font = UIFont.SystemFontOfSize(16);
//			}
//			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
//
//			return cell;
//		}
//
//		public override void Selected(CrossUI.Touch.Dialog.DialogViewController dvc, UIKit.UITableView tableView, Foundation.NSIndexPath path)
//		{
//			var vm = new ECPaymentViewModel();
//			vm.Init(0, PaymentType.Album);
//			var view = Mvx.Resolve<IMvxTouchViewCreator>().CreateView(vm)
//				as UIViewController;
//			dvc.NavigationController.PushViewController(view, false);
//		}
//	}
//}
//
