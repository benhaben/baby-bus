using System;
using UIKit;
using System.Collections.Generic;
using CoreGraphics;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.iOS
{

	public class TeacherHomeView : MvxBabyBusBaseAutoLayoutViewController
	{

		private TeacherHomeViewModel _baseViewModel;

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
						UIImage.FromBundle("images/teacher_home_view/work-1.png"),
						UIImage.FromBundle("images/teacher_home_view/work-2.png"),
						UIImage.FromBundle("images/teacher_home_view/notice-1.png"),
						UIImage.FromBundle("images/teacher_home_view/notice-2.png"),
						UIImage.FromBundle("images/teacher_home_view/photo-1.png"),
						UIImage.FromBundle("images/teacher_home_view/photo-2.png")
					};
					string[] texts = new string[] { 
						"作业信息",
						"班级通知",
						"成长记忆"
					};
					_firstRow = new ButtonsView(images, texts);
					_firstRow.BackgroundColor = MvxTouchColor.White;
				}
				return _firstRow;
			}
		}

		ButtonsView _secondRow = null;

		public ButtonsView SecondRow {
			get {
				if (_secondRow == null) {
					UIImage[] images = new UIImage[] { 
						UIImage.FromBundle("images/teacher_home_view/arr-1.png"),
						UIImage.FromBundle("images/teacher_home_view/arr-2.png"),
						UIImage.FromBundle("images/teacher_home_view/hom-1.png"),
						UIImage.FromBundle("images/teacher_home_view/hom-2.png"),
						UIImage.FromBundle("images/teacher_home_view/exp-1.png"),
						UIImage.FromBundle("images/teacher_home_view/exp-2.png"),
					};
					string[] texts = new string[] { 
						"班级考勤",
						"家园共育",
						"线上家访"
					};
					_secondRow = new ButtonsView(images, texts);
					_secondRow.BackgroundColor = MvxTouchColor.White;
				}
				return _secondRow;
			}
		}

		ButtonsView _thirdRow = null;

		public ButtonsView ThirdRow {
			get {
				if (_thirdRow == null) {
					UIImage[] images = new UIImage[] { 
						UIImage.FromBundle("images/teacher_home_view/talent-1.png"),
						UIImage.FromBundle("images/teacher_home_view/talent-2.png"),
						UIImage.FromBundle("images/teacher_home_view/health-1.png"),
						UIImage.FromBundle("images/teacher_home_view/health-2.png"),
					};
					string[] texts = new string[] { 
						"智能光谱",
						"幼教素材"
					};
					_thirdRow = new ButtonsView(images, texts);
					_thirdRow.BackgroundColor = MvxTouchColor.White;
				}
				return _thirdRow;
			}
		}

		public TeacherHomeView()
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

				&& ThirdRow.Frame.Top == SecondRow.Frame.Bottom + EasyLayout.MarginMedium
				&& ThirdRow.Frame.Left == Container.Frame.Left
				&& ThirdRow.Frame.Right == Container.Frame.Right
				&& ThirdRow.Frame.Height == EasyLayout.HomePageNoticeBarHeight

                //Note: very importatant, set bound
				&& Container.Frame.Bottom == ThirdRow.Frame.Bottom
			);
		}

		protected override void SetBackgroundImage()
		{
			base.SetBackgroundImage();
		}

		public override void PrepareViewHierarchy()
		{
			base.PrepareViewHierarchy();
			UIView[] v = {
                AdvertiseBar,
                FirstRow,
                SecondRow,
                ThirdRow
            };
            Container.AddSubviews(v);
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            _baseViewModel = ViewModel as TeacherHomeViewModel;

            FirstRow.FirstButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                //                "留作业"
                _baseViewModel.NoticeType = NoticeType.ClassHomework;
                _baseViewModel.ShowSendNoticeCommand.Execute();
            };

            FirstRow.SecondButton.TouchUpInside += (object sender, EventArgs e) =>
            {
//                    "发通知",
                _baseViewModel.NoticeType = NoticeType.ClassCommon;
                _baseViewModel.ShowSendNoticeCommand.Execute();
            };

            FirstRow.ThirdButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                //                "传照片",
                _baseViewModel.NoticeType = NoticeType.GrowMemory;
                _baseViewModel.ShowSendNoticeCommand.Execute();
            };

            this.SecondRow.FirstButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                _baseViewModel.ShowAttenceCommand.Execute();
            };

            this.SecondRow.SecondButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                if (this.NavigationController != null)
                {
                    var mainView = this.ParentViewController.ParentViewController as MainView;
                    if (mainView != null)
                    {
                        mainView.SelectedIndex = 3;
                    }
                }
            };

            SecondRow.ThirdButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                //老师发送问答给家长
                _baseViewModel.ShowSendQuestionCommand.Execute();
            };

            ThirdRow.FirstButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                _baseViewModel.ShowMultipleIntelligenceCommand.Execute();
            };

            ThirdRow.SecondButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                if (this.NavigationController != null)
                {
                    var view = new LearningMaterialsView();
                    this.NavigationController.PushViewController(view, true);
                }
            };
        }
    }
}

