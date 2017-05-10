using System;
using CrossUI.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using BabyBus.iOS;
using UIKit;
using CrossUI.Touch.Dialog;
using Foundation;

namespace BabyBus.iOS
{
    public class MITestElement : StyledMultilineElement , IBindableElement
    {
        public MITestElement()
            : base("", "", UIKit.UITableViewCellStyle.Subtitle)
        {
            this.CreateBindingContext();
            Font = EasyLayout.ContentFont;
            SubtitleFont = EasyLayout.TinyFont;
        }

        public IMvxBindingContext BindingContext { get; set; }

        #region IBindableElement implementation

        public void DoBind()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<MITestElement,MIAssessIndex>();	
                    set.Bind().For(me => me.AssessIndexName).To(p => p.Name);
                    set.Bind().For(me => me.TestList).To(p => p.MITestList);
                    set.Apply();
                });
        }

        public object DataContext
        {
            get
            {
                return BindingContext.DataContext;
            }
            set
            {
                BindingContext.DataContext = value;
            }
        }

        #endregion

        public string AssessIndexName{ get; set; }

        public List<MITestQuestion> TestList{ get; set; }

        protected override void PrepareCell(UIKit.UITableViewCell cell)
        {
            if (cell == null)
                return;

            var miTestCell = cell as MITestCell;
            cell.Accessory = Accessory;

            miTestCell.AssessIndexName.Font = Font;
            miTestCell.AssessIndexName.Text = AssessIndexName;

            var collectionViewQuesions = miTestCell.CollectionViewQuestions;
            var dataSource = new MITestCell.CollectionViewQuestionsDataSource();
            dataSource.SetQuestions(TestList);

            collectionViewQuesions.Source = dataSource;
            collectionViewQuesions.ReloadData();
            collectionViewQuesions.AllowsSelection = true;
        }

        public override float GetHeight(UIKit.UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var data = TestList;
            nfloat height = 0f;
            nfloat questionHeight = 80f;

            if (data != null)
            {
                height = questionHeight * data.Count;
            }
            height += EasyLayout.NormalTextFieldHeight;
            return (float)height;
        }

        protected override UIKit.UITableViewCell GetCellImpl(UIKit.UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey) ?? new MITestCell() as UITableViewCell;
            PrepareCell(cell);

            cell.SetNeedsUpdateConstraints();
            cell.UpdateConstraintsIfNeeded();
            return cell;
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            base.Selected(dvc, tableView, path);
        }
    }
}

