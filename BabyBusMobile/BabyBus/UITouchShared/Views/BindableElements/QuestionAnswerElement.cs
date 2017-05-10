using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using CrossUI.Touch.Dialog.Elements;
using UIKit;
using CrossUI.Touch.Dialog;
using Foundation;
using Cirrious.MvvmCross.Dialog.Touch;
using CoreGraphics;
using System.Diagnostics;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.iOS
{

    /// <summary>
    /// Question answer element.
    /// Note: just get idea from StyledMultilineElement, don't use the function in the base class
    /// TODO: refactor this class to fit common use
    /// </summary>
    public class QuestionAnswerElement
        : StyledMultilineElement
    , IBindableElement
    {
        public IMvxBindingContext BindingContext { get; set; }

        public QuestionAnswerElement()
            : base("", "", UITableViewCellStyle.Subtitle)
        {

            this.CreateBindingContext();
        }

        public virtual void DoBind()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<QuestionAnswerElement, QuestionModel>();
                    set.Bind().For(me => me.Id).To(p => p.QuestionId);
                    set.Bind().For(me => me.ChildName).To(p => p.ChildName);
//                    set.Bind().For(me => me.Answer).To(p => p.AnswerContent);
                    set.Bind().For(me => me.DateString).To(p => p.CreateTime).WithConversion("DateTimeOffset");
                    set.Bind().For(me => me.IsAnsweredString).To(p => p.AnswerString);
                    set.Bind().For(me => me.IsAnswered).To(p => p.HasAnswer);
                    set.Bind().For(me => me.Question).To(p => p.ContentWithDate);
                    set.Bind().For(me => me.TeacherName).To(p => p.SendUserName);
                    set.Bind().For(me => me.QuestionType).To(p => p.QuestionType);
                    set.Apply();
                });
        }

        protected override void PrepareCell(UITableViewCell cell)
        {
            if (cell == null)
                return;
            
            var questionAnswerCell = cell as QuestionAnswerCell;
            cell.Accessory = UITableViewCellAccessory.None;
           
            questionAnswerCell.DateLabel.Text = DateString;
           
            if (QuestionType == QuestionType.NormalMessage)
            {
//                questionAnswerCell.TypeImageView.Image = UIImage.FromBundle("images/notice_index_view/message.png");
                questionAnswerCell.TypeUILabel.Text = "家长留言";
                questionAnswerCell.NameLabel.Text = ChildName;

            }
            else if (QuestionType == QuestionType.AskforLeave)
            { 
//                questionAnswerCell.TypeImageView.Image = UIImage.FromBundle("images/question_index_view/askLeave.png");
                questionAnswerCell.TypeUILabel.Text = "孩子请假";
                questionAnswerCell.NameLabel.Text = ChildName;

            }
            else if (QuestionType == QuestionType.MasterMessage)
            { 
//                questionAnswerCell.TypeImageView.Image = UIImage.FromBundle("images/notice_index_view/message.png");
                questionAnswerCell.TypeUILabel.Text = "园长信箱";
                questionAnswerCell.NameLabel.Text = ChildName;
            }
            else if (QuestionType == QuestionType.PersonalMessage)
            { 
//                questionAnswerCell.TypeImageView.Image = UIImage.FromBundle("images/notice_index_view/message.png");
                questionAnswerCell.TypeUILabel.Text = "线上家访";
                if (!string.IsNullOrWhiteSpace(TeacherName))
                {
                    questionAnswerCell.NameLabel.Text = TeacherName;
                }
                else
                {
                    questionAnswerCell.NameLabel.Text = ChildName;
                }
            }
            else
            {
                Debug.Assert(false);
            }

            questionAnswerCell.IsAnsweredLabel.Hidden = IsAnswered;

            questionAnswerCell.QuestionLabel.Text = Question.Trim() + BeginDateToEndDate;
        }

        public  int Id
        {
            get;
            set;
        }

        public  string TeacherName
        {
            get;
            set;
        }

        public QuestionType QuestionType
        { 
            get;
            set;
        }

        public string BeginDateToEndDate
        {
            get;
            set;
        }

        public string Question
        { 
            get;
            set;
        }

        public string IsAnsweredString
        { 
            get;
            set;
        }

        public bool IsAnswered
        { 
            get;
            set;
        }

        public string DateString
        { 
            get;
            set;
        }

        string _childName;

        public string ChildName
        { 
            get
            { 
                return _childName;
            }set
            {
                _childName = value;
            }
        }

        //        public string Answer
        //        {
        //            get;
        //            set;
        //        }

        public float GetCellHeight(QuestionAnswerCell cell)
        {
            cell.LayoutIfNeeded();
            cell.UpdateConstraintsIfNeeded();
            nfloat delta = EasyLayout.MarginMedium * 4 + EasyLayout.MarginNormal * 0;

            nfloat width = 320 - EasyLayout.MarginNormal - EasyLayout.MarginMedium - 10;
            //250是因为有向右小箭头
            var heightQuestionLabel = cell.QuestionLabel.SizeThatFits(new CGSize(width, float.MaxValue)).Height;
            var heightDateLabel = cell.DateLabel.SizeThatFits(new CGSize(width, float.MaxValue)).Height;
            var heightTypeUILabel = cell.TypeUILabel.SizeThatFits(new CGSize(100, float.MaxValue)).Height;

            var height = heightTypeUILabel + heightQuestionLabel + heightDateLabel + EasyLayout.SeparatorHeight + delta;

            //Note: SystemLayoutSizeFittingSize, 设置上下左右的margin才能用这个拿到值
            var height1 = cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height;
            height = (nfloat)Math.Max(height, height1);
            return (float)(height);
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            UITableViewCell cell;
            cell = tv.DequeueReusableCell(QuestionAnswerCell.Key) ?? new QuestionAnswerCell();
            tv.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            PrepareCell(cell);
            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();
            return cell as UITableViewCell;
        }

        static QuestionAnswerCell _calcSizeCell = null;

        //请参考NoticeIndexCell，那边的方法更好
        public override float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
           
            if (_calcSizeCell == null)
            {
                _calcSizeCell = new QuestionAnswerCell();
            }

            PrepareCell(_calcSizeCell);
            return GetCellHeight(_calcSizeCell);
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            base.Selected(dvc, tableView, path);
            var dialogVC = dvc as MvxDialogViewController;
            var questionIndexViewModel = dialogVC.ViewModel as QuestionIndexViewModel;
            questionIndexViewModel.ShowDetailCommand(Id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }

        public virtual object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}

