using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Dialog.Touch;
using CrossUI.Touch.Dialog;
using CrossUI.Touch.Dialog.Elements;
using Foundation;
using UIKit;
using BabyBus.Logic.Shared;


namespace BabyBus.iOS
{
    public class AttendanceUIViewElement 
        : StyledMultilineElement
    , IBindableElement
    {
        public IMvxBindingContext BindingContext { get; set; }

        public AttendanceUIViewElement()
            : base("", "", UITableViewCellStyle.Subtitle)
        {

            this.CreateBindingContext();
//            this.LineBreakMode = UILineBreakMode.TailTruncation;
//            this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
//            //just one row
//            this.Lines = 0;
//            LineBreakMode = UILineBreakMode.TailTruncation;
//            this.Font = UIFont.SystemFontOfSize(15);
        }

        public virtual void DoBind()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<AttendanceUIViewElement, AttendanceMasterViewModel>();
                    set.Bind().For(me => me.TheClassName).To(p => p.ClassName);
                    set.Bind().For(me => me.IsAttence).To(p => p.IsAttence);
                    set.Bind().For(me => me.Attence).To(p => p.Attence);
                    set.Bind().For(me => me.UnAttence).To(p => p.UnAttence);
                    set.Bind().For(me => me.IsAttenceString).To(p => p.IsAttence).WithConversion("IsAttenceMaster");
                    set.Apply();
                });
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            base.Selected(dvc, tableView, path);
            var dialogVC = dvc as MvxDialogViewController;
            var attendanceMasterViewModel = dialogVC.ViewModel as AttendanceMasterViewModel;
            var row = path.Row;
            if (row >= 0 && row <= attendanceMasterViewModel.Attendances.Count)
            {
                var detailViewModel = attendanceMasterViewModel.Attendances[row];
                attendanceMasterViewModel.ClassId = detailViewModel.ClassId;
                attendanceMasterViewModel.MasterId = detailViewModel.MasterId;
                attendanceMasterViewModel.ShowDetailCommand.Execute();
            }
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            UITableViewCell cell;
            cell = tv.DequeueReusableCell(AttendanceUIViewTableCell.Key) ?? (new AttendanceUIViewTableCell() as UITableViewCell);
            cell.Frame = new CoreGraphics.CGRect(0, 0, 320, 60);
            PrepareCell(cell);
            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();
            return cell as UITableViewCell;
        }

        protected override void PrepareCell(UITableViewCell cell)
        {
            if (cell == null)
                return;
            var attendanceUIViewTableCell = cell as AttendanceUIViewTableCell;
            var attendanceUIView = attendanceUIViewTableCell.AttendanceUIView;
            attendanceUIView.TheClassName.Text = TheClassName;
            attendanceUIView.AttenceNumLabel.Text = Convert.ToString(Attence); 
            attendanceUIView.UnattenceNumLabel.Text = Convert.ToString(UnAttence);
            attendanceUIView.IsAttenceLabel.Text = IsAttenceString;
            if (IsAttence)
            {
                attendanceUIView.IsAttenceLabel.BackgroundColor = MvxTouchColor.Gray1;
            }
            else
            {
                attendanceUIView.IsAttenceLabel.BackgroundColor = MvxTouchColor.Red;
            }
        }

        public override float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return 60;
        }

        public string TheClassName{ get; set; }

        public int Attence{ get; set; }

        public int UnAttence{ get; set; }

        public string IsAttenceString{ get; set; }

        public bool IsAttence{ get; set; }

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

