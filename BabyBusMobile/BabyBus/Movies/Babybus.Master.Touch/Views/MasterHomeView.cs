using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;


namespace BabyBus.iOS
{

	public class MasterHomeView : MvxBabyBusBaseAutoLayoutViewController
	{

		private MasterHomeViewModel _baseViewModel;

		ScrollAdvertise _advertiseBar = null;

		public ScrollAdvertise AdvertiseBar {
			get {
				if (_advertiseBar == null) {
					_advertiseBar = new ScrollAdvertise(new List<string> { 
						"ad-1.png", "ad-2.png", "ad-3.png"
					});
				}
				return _advertiseBar;
			}
		}

		ButtonsView _firstRow = null;

		public ButtonsView FirstRow {
			get {
				if (_firstRow == null) {
					UIImage[] images = new UIImage[] { 
						UIImage.FromBundle("images/master_home_view/b-notice.png"),
						UIImage.FromBundle("images/master_home_view/b-notice-2.png"),
						UIImage.FromBundle("images/master_home_view/neibu-1.png"),
						UIImage.FromBundle("images/master_home_view/neibu-2.png"),
						UIImage.FromBundle("images/master_home_view/chark-1.png"),
						UIImage.FromBundle("images/master_home_view/chark-2.png"),
                       
					};
					string[] texts = new string[] { 
						"园区通知",
						"内部管理",
						"发送食谱"
					};
					_firstRow = new ButtonsView(images, texts);
				}
				return _firstRow;
			}
		}

		ButtonsView _secondRow = null;

		public ButtonsView SecondRow {
			get {
				if (_secondRow == null) {
					UIImage[] images = new UIImage[] { 
						UIImage.FromBundle("images/master_home_view/find-1.png"),
						UIImage.FromBundle("images/master_home_view/find-2.png"),
						UIImage.FromBundle("images/master_home_view/mail-1.png"),
						UIImage.FromBundle("images/master_home_view/mail-2.png"),
                       
					};
					string[] texts = new string[] { 
						"查阅考勤",
						"园长信箱",
					};
					_secondRow = new ButtonsView(images, texts);
				}
				return _secondRow;
			}
		}


		public MasterHomeView()
		{
			UILabel titleUILabel = new UILabel(new CGRect(0, 0, 100, 35));
			titleUILabel.Text = BabyBusContext.Kindergarten.KindergartenName;
			this.NavigationItem.TitleView = titleUILabel;
			titleUILabel.TextAlignment = UITextAlignment.Center;
			titleUILabel.TextColor = MvxTouchColor.White;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
		}

		public override void SetUpConstrainLayout()
		{
			base.SetUpConstrainLayout();

			View.ConstrainLayout(
				() => 
                AdvertiseBar.Frame.Top == Container.Frame.Top
				&& AdvertiseBar.Frame.Left == Container.Frame.Left
				&& AdvertiseBar.Frame.Right == Container.Frame.Right
				&& AdvertiseBar.Frame.Height == EasyLayout.AdvertiseBarHeight
				&& AdvertiseBar.Frame.Width == EasyLayout.AdvertiseBarWidth

				&& FirstRow.Frame.Top == AdvertiseBar.Frame.Bottom + EasyLayout.MarginLarge
				&& FirstRow.Frame.Left == Container.Frame.Left
				&& FirstRow.Frame.Right == Container.Frame.Right
				&& FirstRow.Frame.Height == EasyLayout.HomePageNoticeBarHeight

				&& SecondRow.Frame.Top == FirstRow.Frame.Bottom + EasyLayout.MarginMedium
				&& SecondRow.Frame.Left == Container.Frame.Left
				&& SecondRow.Frame.Right == Container.Frame.Right
				&& SecondRow.Frame.Height == EasyLayout.HomePageNoticeBarHeight

                //Note: very importatant, set bound
				&& Container.Frame.Bottom == SecondRow.Frame.Bottom
			);
		}

		public override void PrepareViewHierarchy()
		{
			base.PrepareViewHierarchy();
			UIView[] v = {
				AdvertiseBar,
				FirstRow,
				SecondRow
			};
			Container.AddSubviews(v);
		}

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();
			_baseViewModel = ViewModel as MasterHomeViewModel;

			FirstRow.FirstButton.TouchUpInside += (object sender, EventArgs e) => {
//                "园区通知",
				_baseViewModel.NoticeType = NoticeType.KindergartenAll;
				_baseViewModel.ShowSendNoticeCommand.Execute();
			};

			FirstRow.SecondButton.TouchUpInside += (object sender, EventArgs e) => {
//                "园务通知",
				_baseViewModel.NoticeType = NoticeType.KindergartenStaff;
				_baseViewModel.ShowSendNoticeCommand.Execute();

			};

			FirstRow.ThirdButton.TouchUpInside += (object sender, EventArgs e) => {
				_baseViewModel.NoticeType = NoticeType.KindergartenRecipe;
				_baseViewModel.ShowSendNoticeCommand.Execute();
			};

			SecondRow.FirstButton.TouchUpInside += (object sender, EventArgs e) => {
//                    "查看考勤"
				_baseViewModel.ShowAttenceCommand.Execute();
                
			};

			SecondRow.SecondButton.TouchUpInside += (object sender, EventArgs e) => {
				if (this.NavigationController != null) {
					var mainView = this.ParentViewController.ParentViewController as MainView;
					if (mainView != null) {
						mainView.SelectedIndex = 2;
					}
				}
			};
		}
	}
}

