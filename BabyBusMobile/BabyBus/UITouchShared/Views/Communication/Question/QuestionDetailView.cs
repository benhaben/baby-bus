using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using Views.TableCell;
using CoreGraphics;
using Cirrious.MvvmCross.Binding.BindingContext;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{

	public class QuestionDetailView : MvxBabyBusBaseAutoLayoutViewController
	{

		QuestionDetailViewModel _baseViewModel;

		UITableView _answersTable;

		public UITableView AnswersTable {
			get {
				if (_answersTable == null) {   
					_answersTable = new UITableView();
					_answersTable.ScrollEnabled = true;
					_answersTable.UserInteractionEnabled = true;
					_answersTable.BackgroundColor = MvxTouchColor.White;
				}

				return _answersTable;
			}
		}

		UITextView _questionText;

		public UITextView QuestionText {
			get {
				if (_questionText == null) {
					_questionText = new UITextView();
					_questionText.TextAlignment = UITextAlignment.Left;
					_questionText.Editable = false;
					_questionText.TextContainerInset = new UIEdgeInsets(10, 10, 10, 10);
					_questionText.ScrollEnabled = true;
					_questionText.Font = EasyLayout.ContentFont;
					_questionText.BackgroundColor = MvxTouchColor.White;
					_questionText.TextColor = MvxTouchColor.Gray1;
				}
				return _questionText;
			}
		}

		UITextView _answerText;

		public UITextView AnswerText {
			get {
				if (_answerText == null) {
					_answerText = new UITextView();
					_answerText.Font = EasyLayout.ContentFont;
					//_answerText.BackgroundColor = MvxTouchColor.White;
					_answerText.Layer.BorderColor = MvxTouchColor.Gray3.CGColor;
					_answerText.Layer.BorderWidth = 1;
					_answerText.Layer.CornerRadius = 5;
					_answerText.TextColor = MvxTouchColor.Gray1;
					_answerText.TextContainerInset = new UIEdgeInsets(10, 10, 10, 10);
				}
				return _answerText;
			}
		}

		UILabel _inforLabel = null;

		public UILabel InfoLabel {
			get { 
				if (_inforLabel == null) {
					_inforLabel = new UILabel();
					_inforLabel.Font = EasyLayout.ContentFont;
					_inforLabel.BackgroundColor = MvxTouchColor.White;
					_inforLabel.TextColor = MvxTouchColor.Gray1;
					_inforLabel.Text = "请在下面方框中输入回复";
					_inforLabel.TextAlignment = UITextAlignment.Center;
					_inforLabel.Lines = 0;
					_inforLabel.Font = EasyLayout.ContentFont;
					_inforLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
				}
				return _inforLabel;
			}
		}

		UIButton _answerButton = null;

		public UIButton AnswerButton {
			get { 
				if (_answerButton == null) {
					_answerButton = new UIButton();
					_answerButton.Font = EasyLayout.ContentFont;
					_answerButton.SetTitleColor(MvxTouchColor.Black1, UIControlState.Normal);
					_answerButton.BackgroundColor = MvxTouchColor.White;
					_answerButton.Layer.CornerRadius = EasyLayout.CornerRadius;
					_answerButton.Layer.MasksToBounds = true;
					_answerButton.SetTitle("回复", UIControlState.Normal);
					_answerButton.ContentEdgeInsets = new UIEdgeInsets(6, 10, 7, 10);
//					_answerButton.TouchUpInside += (object sender, EventArgs e) => {
//						_baseViewModel.ShowSelectChildrenCommand.Execute();
//					};
				}
				return _answerButton;
			}
		}

		public QuestionDetailView()
		{
			AddGestureWhenTap = true;
		}

		public override void PrepareViewHierarchy()
		{
			base.PrepareViewHierarchy();
			UIView[] v = {
				AnswerText,
				QuestionText,
				AnswersTable,
				InfoLabel,
				AnswerButton,
			};

			Container.AddSubviews(v);
			if (UIScreen.MainScreen.Bounds.Height > 480) {
				AnswersTableHeight = 200;
				QuestionTextHeight = 120;

			} else {
				AnswersTableHeight = 150;
				QuestionTextHeight = 60;
			}

			#if DEBUG1
            QuestionText.BackgroundColor = UIColor.Cyan;

            AnswersTable.BackgroundColor = UIColor.Red;
            AnswersTable.Layer.BorderWidth = EasyLayout.BorderWidth;
            AnswersTable.Layer.BorderColor = MvxTouchColor.Red.CGColor;
            AnswersTable.Layer.CornerRadius = EasyLayout.CornerRadius;
            AnswersTable.Layer.MasksToBounds = true;

            AnswerText.BackgroundColor = UIColor.Blue;
            SubmitButton.BackgroundColor = UIColor.Green;
			#endif
		}

		protected override void SetBackgroundImage()
		{
			base.SetBackgroundImage();
			this.View.BackgroundColor = MvxTouchColor.White2;
		}

		//TODO: iphone4s need smaller
		nfloat AnswersTableHeight = 200;
		nfloat AnswersTextHeight = 30;
		nfloat QuestionTextHeight = 120;

		nfloat StaticAnswersTableHeight {
			get {
				return UIScreen.MainScreen.Bounds.Height - QuestionTextHeight - AnswersTextHeight - EasyLayout.MarginMedium * 3 - 44 - 17 - 20 - 10;
			}
		}

		nfloat DynamicAnswerTableHeight {
			get {
				return UIScreen.MainScreen.Bounds.Height - QuestionTextHeight - _heightConstrainAnswerText.Constant - EasyLayout.MarginMedium * 3 - 44 - 17 - 20 - 10;
			}
		}

		public override void SetUpConstrainLayout()
		{
			base.SetUpConstrainLayout();

			View.ConstrainLayout 
            (
                // Analysis disable CompareOfFloatsByEqualityOperator
				() => 

                QuestionText.Frame.Top == Container.Frame.Top + EasyLayout.MarginMedium
				&& QuestionText.Frame.Left == Container.Frame.Left
				&& QuestionText.Frame.Right == Container.Frame.Right

				&& AnswersTable.Frame.Left == Container.Frame.Left
				&& AnswersTable.Frame.Right == Container.Frame.Right
				&& AnswersTable.Frame.Top == QuestionText.Frame.Bottom + EasyLayout.MarginMedium

				&& InfoLabel.Frame.Left == Container.Frame.Left
				&& InfoLabel.Frame.Right == Container.Frame.Right
				&& InfoLabel.Frame.Top == AnswersTable.Frame.Bottom + EasyLayout.MarginMedium

				&& AnswerText.Frame.Left == Container.Frame.Left + EasyLayout.MarginMedium
//				&& AnswerText.Frame.Right == Container.Frame.Right - EasyLayout.MarginMedium
				&& AnswerText.Frame.Top == InfoLabel.Frame.Bottom + EasyLayout.MarginMedium
//                && AnswerText.Frame.Bottom == Container.Frame.Bottom - EasyLayout.MarginMedium

				&& AnswerButton.Frame.Left == AnswerText.Frame.Right + EasyLayout.MarginMedium
				&& AnswerButton.Frame.Right == Container.Frame.Right - EasyLayout.MarginMedium
				&& AnswerButton.Frame.Bottom == AnswerText.Frame.Bottom

				&& Container.Frame.Bottom == AnswerText.Frame.Bottom
//                && SubmitButton.Frame.Left == Container.Frame.Left + EasyLayout.MarginMedium
//                && SubmitButton.Frame.Right == Container.Frame.Right - EasyLayout.MarginMedium
//                && SubmitButton.Frame.Top == AnswerText.Frame.Bottom + EasyLayout.MarginMedium


                // Analysis restore CompareOfFloatsByEqualityOperator
			);

			//Note: do not change frame, change Constrain to resize control
			var constrains =
				View.ConstrainLayout(
					() => 
                    AnswerText.Frame.Height == AnswersTextHeight
//                    && SubmitButton.Frame.Height == EasyLayout.SmallButtonWidthAndHeight
					&& QuestionText.Frame.Height == QuestionTextHeight
					&& AnswersTable.Frame.Height >= AnswersTableHeight 
				);
			_heightConstrainAnswerText = constrains[0];
//            _heightConstrainSubmitButton = constrains[1];
			_heightConstrainQuestionTable = constrains[1];
			_heightConstrainAnswersTable = constrains[2];

		}

		NSLayoutConstraint _heightConstrainAnswersTable;
		NSLayoutConstraint _heightConstrainAnswerText;
		//        NSLayoutConstraint _heightConstrainSubmitButton;
		NSLayoutConstraint _heightConstrainQuestionTable;

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();

			_baseViewModel = this.ViewModel as QuestionDetailViewModel;

			_baseViewModel.FirstLoadedEventHandler += (object sender, EventArgs e) => this.InvokeOnMainThread(() => {
				var set = this.CreateBindingSet<QuestionDetailView, QuestionDetailViewModel>();
				set.Bind(AnswerText).To(vm => vm.Answer);
				set.Bind(QuestionText).To(vm => vm.Question.ContentWithDate);
//                    set.Bind(SubmitButton).To(vm => vm.SendAnswerCommand);
				set.Apply();

				var sizeMax = new CGSize(300, QuestionTextHeight);
//				var heightQuestionText = QuestionText.SizeThatFits(sizeMax).Height;
				_heightConstrainQuestionTable.Constant = QuestionTextHeight;

				if (_baseViewModel.Question != null) {
					AnswersTable.Source = new QuestionDetailTableSource(_baseViewModel.Question.Answers, this);
//                        if (BabyBusContext.RoleType == BabyBus.Models.RoleType.Parent)
//                        {
//                            SubmitButton.Hidden = true;
//                            AnswerText.Hidden = true;
//                            _heightConstrainAnswerText.Constant = 0;
//                            _heightConstrainSubmitButton.Constant = 0;
//                        }
				}
				AnswersTable.ReloadData();

				if (_baseViewModel.Question.Answers != null && _baseViewModel.Question.Answers.Count > 0) {
					AnswersTable.ScrollToRow(NSIndexPath.FromRowSection(_baseViewModel.Question.Answers.Count - 1, 0)
        					, UITableViewScrollPosition.Bottom
        					, false);
				}

				AnswersTableHeight = StaticAnswersTableHeight;
				//var tableSize = AnswersTable.SizeThatFits(sizeMax);
				_heightConstrainAnswersTable.Constant = AnswersTableHeight;
			});

			_baseViewModel.RefreshAnswers += (object sender, EventArgs e) => 
				InvokeOnMainThread(() => {
				AnswersTable.Source = new QuestionDetailTableSource(_baseViewModel.Question.Answers, this);
				AnswersTable.ReloadData();
				var sizeMax = new CGSize(View.Frame.Width, float.MaxValue);
				var tableSize = AnswersTable.SizeThatFits(sizeMax);
				_heightConstrainAnswersTable.Constant = tableSize.Height > AnswersTableHeight ? AnswersTableHeight : tableSize.Height;
				_heightConstrainAnswerText.Constant = AnswersTextHeight;
				AnswersTable.ScrollToRow(NSIndexPath.FromRowSection(_baseViewModel.Question.Answers.Count - 1, 0), UITableViewScrollPosition.Bottom, true);
			});

			AnswersTable.TableFooterView = new UIView();
			AnswersTable.SeparatorStyle = UITableViewCellSeparatorStyle.None;


//			var doneButton = new UIBarButtonItem("回复", UIBarButtonItemStyle.Done, (sender, args) => {
//				var alert = new UIAlertView("提示", "您确定要发送吗，别忘了检查错别字哦？", null, "发送", new String []{ "回到编辑" });
//				alert.Clicked += (object s, UIButtonEventArgs e) => {
//					if (e.ButtonIndex == 0) {
//						_baseViewModel.SendAnswerCommand.Execute(); 
//					} else {
//					}
//				};
//				alert.AlertViewStyle = UIAlertViewStyle.Default;
//				alert.Show();
//			});
//			this.NavigationItem.SetRightBarButtonItem(doneButton, true);

			this.AnswerButton.TouchUpInside += (object sender, EventArgs e) => {
				_baseViewModel.SendAnswerCommand.Execute();
			};

			this.AnswerText.Changed += (sender, e) => {
				nfloat heightChanged = AnswerText.SizeThatFits(new CGSize(300, float.MaxValue)).Height;
				if (heightChanged < 100) {
					_heightConstrainAnswerText.Constant = heightChanged;
					_heightConstrainAnswersTable.Constant = DynamicAnswerTableHeight;
//					_heightConstrainAnswersTable.Constant = _heightConstrainAnswerText.Constant >= heightChanged
//					? _heightConstrainAnswersTable.Constant : _heightConstrainAnswersTable.Constant - heightChanged + AnswersTextHeight;
//					_heightConstrainAnswerText.Constant = _heightConstrainAnswerText.Constant >= heightChanged 
//					? _heightConstrainAnswerText.Constant : heightChanged;
				}
			};
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public class QuestionDetailTableSource : UITableViewSource
		{
			UIViewController _vc;
			IList<AnswerModel> _answers;

			public QuestionDetailTableSource(IList<AnswerModel> items, UIViewController vc)
			{
				_answers = items;
				_vc = vc;
			}

			/// <summary>
			/// Called by the TableView to determine how many cells to create for that particular section.
			/// </summary>
			public override nint RowsInSection(UITableView tableview, nint section)
			{
				if (_answers != null) {
					return _answers.Count;
				} else {
					return 0;
				}
			}

			/// <summary>
			/// Called when a row is touched
			/// </summary>
			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
			}

			/// <summary>
			/// Called when the DetailDisclosureButton is touched.
			/// Does nothing if DetailDisclosureButton isn't in the cell
			/// </summary>
			public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
			{
			}

			public float GetCellHeight(UITableViewCell cell)
			{
				cell.LayoutIfNeeded();
				cell.UpdateConstraintsIfNeeded();

				var answerCell = cell as AnswerCell;

				//计算有些情况没有考虑，测试的时候调整
				nfloat delta = EasyLayout.MarginMedium * 3 + EasyLayout.MarginNormal * 3;

				//Note: SystemLayoutSizeFittingSize, 设置上下左右的margin才能用这个拿到值,多行的时候计算不对
//                var heightTest = cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height;

				var height1 = answerCell.TeacherNameLabel.SizeThatFits(new CGSize(250, float.MaxValue)).Height;
				var height2 = answerCell.AnswerLabel.SizeThatFits(new CGSize(240, float.MaxValue)).Height;

//				var height3 = answerCell.DateUILabel.SizeThatFits(new CGSize(300, float.MaxValue)).Height;
				var heightSys = cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height;

				var height = height1 + height2 + delta;


				return (float)(Math.Max(height, heightSys));
			}

			static UITableViewCell _calcSizeCell = null;

			public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				if (_calcSizeCell == null) {
					var answerModel = _answers[indexPath.Row];
					_calcSizeCell = new AnswerCell(answerModel.IsMyself);
				}

				PrepareCell(_calcSizeCell, indexPath);
				return GetCellHeight(_calcSizeCell);
			}

			void PrepareCell(UITableViewCell cell, NSIndexPath indexPath)
			{
				var answerCell = cell as AnswerCell;
				if (answerCell != null) {
					answerCell.Accessory = UITableViewCellAccessory.None;
					var answerModel = _answers[indexPath.Row];
					answerCell.TeacherNameLabel.Text = answerModel.RealName;
					answerCell.DateUILabel.Text = LogicUtils.DateTimeString(answerModel.CreateTime);
					answerCell.AnswerLabel.Text = answerModel.Content;
				}
			}

			/// <summary>
			/// Called by the TableView to get the actual UITableViewCell to render for the particular row
			/// </summary>
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var answerModel = _answers[indexPath.Row];
				UITableViewCell cell = tableView.DequeueReusableCell(answerModel.IsMyself ? AnswerCell.KeyRight : AnswerCell.KeyLeft);

				// if there are no cells to reuse, create a new one
				if (cell == null) {
					cell = new AnswerCell(answerModel.IsMyself);
				}
				PrepareCell(cell, indexPath);
				cell.SetNeedsUpdateConstraints();
				cell.UpdateConstraintsIfNeeded();
				return cell;
			}
		}
	}


}

