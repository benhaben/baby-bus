using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using CoreGraphics;
using UIKit;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.iOS
{
    public class SendQuestionView : MvxBabyBusBaseAutoLayoutViewController
    {
        private SendQuestionViewModel _baseViewModel;

        public SendQuestionView()
        {
            this.Request = new Cirrious.MvvmCross.ViewModels.MvxViewModelRequest(
                typeof(SendQuestionViewModel), null, null, null
            );
            this.AddGestureWhenTap = false;
        }

        void ShowPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(_baseViewModel.Content))
            {
                MessagePlaceholder.Text = _baseViewModel.ContentHolder;
            }
            else
            {
                MessagePlaceholder.Text = "";
            }
        }

        void SwitchViews()
        {
            var selectedSegmentQuestionType = (QuestionType)(int)(MessageTypeUISegmentedControl.SelectedSegment);

            if (selectedSegmentQuestionType != QuestionType.AskforLeave)
            {
                _heightSelectDate.Constant = 0;
                _heightSickRadioButton.Constant = 0;
                this.SelectDate.Hidden = true;
                this.SickRadioButton.Hidden = true;
            }
            else
            {
                _heightSelectDate.Constant = SelectDateViewHeight;
                _heightSickRadioButton.Constant = SickRadioButtonHeight;
                this.SelectDate.Hidden = false;
                this.SickRadioButton.Hidden = false;
            }
        }

        UISegmentedControl _messageTypeUISegmentedControl;

        public UISegmentedControl MessageTypeUISegmentedControl
        {
            get
            {
                if (_messageTypeUISegmentedControl == null)
                {
                    _messageTypeUISegmentedControl = new UISegmentedControl();
                    _messageTypeUISegmentedControl.InsertSegment("向老师请假", 0, false);
                    _messageTypeUISegmentedControl.InsertSegment("给老师留言", 1, false);
                    _messageTypeUISegmentedControl.SelectedSegment = 1; 
                    _messageTypeUISegmentedControl.ControlStyle = UISegmentedControlStyle.Plain;
                    _messageTypeUISegmentedControl.ValueChanged += (sender, e) =>
                    {
                        var selectedSegmentQuestionType = (QuestionType)(int)(sender as UISegmentedControl).SelectedSegment;
                        // do something with selectedSegmentId
                        if (_baseViewModel.SelectMemoType.Type != selectedSegmentQuestionType)
                        {
                            _baseViewModel.SelectMemoType.Type = selectedSegmentQuestionType;
                            _baseViewModel.Content = "";
                        }
                        ShowPlaceholder();

                        SwitchViews();
                    };
                }
                return _messageTypeUISegmentedControl;
            }
        }

        UITextView _message;

        public UITextView MessageTextView
        {
            get
            {
                if (_message == null)
                {
                    _message = new UITextView();
                    _message.BackgroundColor = MvxTouchColor.White;
                    _message.TextColor = MvxTouchColor.Gray1;
                    _message.Font = EasyLayout.ContentFont;
                    _message.TextContainerInset = new UIEdgeInsets(10, 10, 10, 10);
                    _message.Changed += (sender, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(_message.Text))
                        {
                            MessagePlaceholder.Text = "";
                        }
                    };
                }
                return _message;
            }
        }

        InsetsLabel _messagePlaceholder;

        public InsetsLabel MessagePlaceholder
        {
            get
            {
                if (_messagePlaceholder == null)
                {
                    _messagePlaceholder = new InsetsLabel(new UIEdgeInsets(10, 10, 10, 10));
                    _messagePlaceholder.Layer.BorderColor = MvxTouchColor.Gray3.CGColor;
                    _messagePlaceholder.Layer.BorderWidth = EasyLayout.BorderWidth;
                    _messagePlaceholder.Layer.CornerRadius = EasyLayout.CornerRadius;
                    _messagePlaceholder.Layer.MasksToBounds = true;
                    _messagePlaceholder.Lines = 0;
                    _messagePlaceholder.Font = EasyLayout.ContentFont;
                    _messagePlaceholder.TextColor = MvxTouchColor.Gray2;
                }
                return _messagePlaceholder;
            }
        }

        UIButton _selectChildren;

        public UIButton SelectChildrenButton
        {
            get
            {
                if (_selectChildren == null)
                {
                    _selectChildren = new UIButton();
                    _selectChildren.Font = EasyLayout.TitleFont;
                    _selectChildren.SetTitleColor(MvxTouchColor.Gray1, UIControlState.Normal);
                    _selectChildren.BackgroundColor = MvxTouchColor.White;
                    _selectChildren.Layer.CornerRadius = EasyLayout.CornerRadius;
                    _selectChildren.Layer.MasksToBounds = true;
                    _selectChildren.SetTitle("请点击选择孩子", UIControlState.Normal);
                    _selectChildren.TouchUpInside += (object sender, EventArgs e) =>
                    {
                        _baseViewModel.ShowSelectChildrenCommand.Execute();
                    };
                }
                return _selectChildren;
            }
        }

        SelectBeginAndEndDateView _selectDate;

        public SelectBeginAndEndDateView SelectDate
        {
            get
            {
                if (_selectDate == null)
                {
                    _selectDate = new SelectBeginAndEndDateView();
                    _selectDate.BackgroundColor = MvxTouchColor.White;
                    _selectDate.UserInteractionEnabled = true;
                   
                }
                return _selectDate;
            }
        }


        InsetsLabel _selectedChildren;

        public InsetsLabel SelectedChildrenLabel
        {
            get
            {
                if (_selectedChildren == null)
                {
                    _selectedChildren = new InsetsLabel(new UIEdgeInsets(5, 10, 5, 10));
                    _selectedChildren.Lines = 0;
                    _selectedChildren.BackgroundColor = MvxTouchColor.White;
                    _selectedChildren.TextColor = MvxTouchColor.Gray1;
                    _selectedChildren.Font = EasyLayout.ContentFont;
                }
                return _selectedChildren;
            }
        }

        RadioButtonView _sickRadioButton;

        public RadioButtonView SickRadioButton
        {
            get
            {
                if (_sickRadioButton == null)
                {
                    _sickRadioButton = new RadioButtonView();
                    _sickRadioButton.BackgroundColor = MvxTouchColor.White;
                    _sickRadioButton.Title = "您的孩子是否患有以下疾病";

                }
                return _sickRadioButton;
            }
        }

        public override void PrepareViewHierarchy()
        {
            base.PrepareViewHierarchy();
            UIView[] v =
                {
                    SelectChildrenButton,
                    SelectedChildrenLabel,
                    MessageTypeUISegmentedControl,
                    MessageTextView,
                    MessagePlaceholder,
                    SickRadioButton,
                    SelectDate,
                };

            Container.AddSubviews(v);
            #if DEBUG1
//            SelectChildrenButton.BackgroundColor = UIColor.Red;
            SelectedChildrenLabel.BackgroundColor = UIColor.Green;
            MessageType.BackgroundColor = UIColor.Gray;
            MessageTextView.BackgroundColor = UIColor.Blue;
            #endif
        }

        nfloat SickRadioButtonHeight = 78;
        nfloat SelectDateViewHeight = SelectBeginAndEndDateView.SelectDateViewHeight;

        public override void SetUpConstrainLayout()
        {
            base.SetUpConstrainLayout();


            View.ConstrainLayout 
            (
                
                // Analysis disable CompareOfFloatsByEqualityOperator
                () => 

				 MessageTypeUISegmentedControl.Frame.Left == Container.Frame.Left
                && MessageTypeUISegmentedControl.Frame.Right == Container.Frame.Right
                && MessageTypeUISegmentedControl.Frame.Top == Container.Frame.Top + EasyLayout.MarginMedium

                && SelectChildrenButton.Frame.Left == Container.Frame.Left
                && SelectChildrenButton.Frame.Right == Container.Frame.Right
                && SelectChildrenButton.Frame.Top == Container.Frame.Top + EasyLayout.MarginMedium

                && SelectedChildrenLabel.Frame.Left == Container.Frame.Left
                && SelectedChildrenLabel.Frame.Right == Container.Frame.Right
                && SelectedChildrenLabel.Frame.Top == SelectChildrenButton.Frame.Bottom + EasyLayout.MarginMedium

                && MessageTextView.Frame.Left == Container.Frame.Left
                && MessageTextView.Frame.Right == Container.Frame.Right
                && MessageTextView.Frame.Top >= SelectedChildrenLabel.Frame.Bottom + EasyLayout.MarginMedium
                && MessageTextView.Frame.Top >= MessageTypeUISegmentedControl.Frame.Bottom + EasyLayout.MarginMedium

                && MessagePlaceholder.Frame.Height == MessageTextView.Frame.Height
                && MessagePlaceholder.Frame.Left == MessageTextView.Frame.Left
                && MessagePlaceholder.Frame.Right == MessageTextView.Frame.Right
                && MessagePlaceholder.Frame.Top == MessageTextView.Frame.Top

                && SickRadioButton.Frame.Top == MessageTextView.Frame.Bottom + EasyLayout.MarginMedium
                && SickRadioButton.Frame.Left == Container.Frame.Left
                && SickRadioButton.Frame.Right == Container.Frame.Right

                && SelectDate.Frame.Top == SickRadioButton.Frame.Bottom + EasyLayout.MarginMedium
                && SelectDate.Frame.Left == Container.Frame.Left
                && SelectDate.Frame.Right == Container.Frame.Right

                && Container.Frame.Bottom == SelectDate.Frame.Bottom

                // Analysis restore CompareOfFloatsByEqualityOperator
            );
            var constrains =
                View.ConstrainLayout(
                    () => MessageTextView.Frame.Height == EasyLayout.HeightOfContent
                    && MessageTypeUISegmentedControl.Frame.Height == EasyLayout.SmallButtonWidthAndHeight
                    && SelectChildrenButton.Frame.Height == EasyLayout.SmallButtonWidthAndHeight
                    && SelectedChildrenLabel.Frame.Height == EasyLayout.NormalTextFieldHeight
                    && SelectDate.Frame.Height == SelectDateViewHeight
                    && SickRadioButton.Frame.Height == SickRadioButtonHeight

                );
            _heightConstrainContent = constrains[0];
            _heightConstrainMessageType = constrains[1];
            _heightSelectChildrenButton = constrains[2];
            _heightSelectedChildrenLabel = constrains[3];
            _heightSelectDate = constrains[4];
            _heightSickRadioButton = constrains[5];
        }

        NSLayoutConstraint _heightConstrainContent;
        NSLayoutConstraint _heightConstrainMessageType;
        NSLayoutConstraint _heightSelectChildrenButton;
        NSLayoutConstraint _heightSelectedChildrenLabel;
        NSLayoutConstraint _heightSelectDate;
        NSLayoutConstraint _heightSickRadioButton;


        protected override void SetBackgroundImage()
        {
            base.SetBackgroundImage();
            this.View.BackgroundColor = MvxTouchColor.White2;
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            _baseViewModel = this.ViewModel as SendQuestionViewModel;

            if (_baseViewModel.Children == null || _baseViewModel.Children.Count == 0)
            {
                _heightSelectedChildrenLabel.Constant = 0;
            }
          
            if (_baseViewModel.SendToWho == RoleType.Parent)
            {
                this.MessageTypeUISegmentedControl.Hidden = true;
                _heightConstrainMessageType.Constant = 0;
            }
            else if (_baseViewModel.SendToWho == RoleType.HeadMaster)
            {
                this.MessageTypeUISegmentedControl.Hidden = true;
                _heightConstrainMessageType.Constant = 0;
                _heightSelectedChildrenLabel.Constant = 0;

                _heightSelectChildrenButton.Constant = 0;
                this.SelectChildrenButton.Hidden = true;
            }
            else
            {
                //send to teacher
                _heightSelectedChildrenLabel.Constant = 0;
                _heightSelectedChildrenLabel.Constant = 0;

                _heightSelectChildrenButton.Constant = 0;
                this.SelectChildrenButton.Hidden = true;
            }

            _baseViewModel.SelectChildrenEventHandler += (object sender, EventArgs e) => this.InvokeOnMainThread(() =>
                {
                    SelectedChildrenLabel.Text = _baseViewModel.SelectedText;
                    nfloat heightChanged = SelectedChildrenLabel.SizeThatFits(new CGSize(280, float.MaxValue)).Height;
                    _heightSelectedChildrenLabel.Constant = heightChanged + 10;
                }
            );

            _baseViewModel.FirstLoadedEventHandler += (object sender, EventArgs e) => this.InvokeOnMainThread(() =>
                {
                    ShowPlaceholder();

                    var set = this.CreateBindingSet<SendQuestionView, SendQuestionViewModel>();
                    set.Bind(MessageTextView).To(vm => vm.Content);
                    set.Apply();
                }
            );
           

            MessageTextView.Changed += (sender, e) =>
            {
                nfloat heightChanged = MessageTextView.SizeThatFits(new CGSize(300, float.MaxValue)).Height;
                _heightConstrainContent.Constant = _heightConstrainContent.Constant > heightChanged ? _heightConstrainContent.Constant : heightChanged;
            };

            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) =>
                {
                    var alert = new UIAlertView("提示", "您确定要发送吗，别忘了检查错别字哦？", null, "发送", new String []{ "回到编辑" });
                    alert.Clicked += (object s, UIButtonEventArgs e) =>
                    {
                        if (e.ButtonIndex == 0)
                        {
                            if (_baseViewModel != null)
                            {
                                _baseViewModel.BeginDate = this.SelectDate.BeginDate;
                                _baseViewModel.EndDate = this.SelectDate.EndDate;
                                _baseViewModel.SendCommand.Execute();
                            }
                        }
                        else
                        {
                        }
                    };
                    alert.AlertViewStyle = UIAlertViewStyle.Default;
                    alert.Show();
                });
            this.NavigationItem.SetRightBarButtonItem(doneButton, true);


            SickRadioButton.SetButtonsName(_baseViewModel.SickNames);
            SickRadioButton.TouchButton += (object sender, int e) =>
            {
                _baseViewModel.Content = _baseViewModel.SickMessage[e];
                MessagePlaceholder.Text = "";
            };

            SwitchViews();
        }
    }
}

