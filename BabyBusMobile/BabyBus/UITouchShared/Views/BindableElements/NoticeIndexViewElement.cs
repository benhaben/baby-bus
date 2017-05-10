using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using CrossUI.Touch.Dialog;
using Foundation;
using UIKit;
using CrossUI.Touch.Dialog.Elements;
using CoreGraphics;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.iOS
{
	public class NoticeIndexViewElement
        :Element
        ,CrossUI.Touch.Dialog.Elements.IElementSizing
        ,IBindableElement
        ,IReadableElement
	{
		#region IReadable implementation

		public override bool Readed()
		{
			return IsReaded;
		}

		#endregion

		public IMvxBindingContext BindingContext { get; set; }

		public NoticeIndexViewElement()
			: base("")
		{

			this.CreateBindingContext();
		}

		public virtual void DoBind()
		{
			this.DelayBind(() => {
				var set = this.CreateBindingSet<NoticeIndexViewElement, NoticeModel>();
				set.Bind().For(me => me.Id).To(p => p.NoticeId);
				set.Bind().For(me => me.IsReaded).To(p => p.IsReaded);
				set.Bind().For(me => me.NoticeTitle).To(p => p.Title);
				set.Bind().For(me => me.Content).To(p => p.AbstractDisplayForiOS);
				set.Bind().For(me => me.TeacherName).To(p => p.RealName);
				set.Bind().For(me => me.CreateTimeString).To(p => p.CreateTime).WithConversion("DateTimeOffset");
				set.Bind().For(me => me.NoticeType).To(p => p.NoticeType);
				set.Bind().For(me => me.IsHtml).To(p => p.IsHtml);
				set.Apply();
			});
		}

		bool _isReaded;

		public bool IsReaded {
			get { return _isReaded; }
			set {
				_isReaded = value;
//                RaisePropertyChanged(() => Unread);
			}
		}

		public int Id{ get; set; }

		public  string NoticeTitle{ get; set; }

		public string Content{ get; set; }

		public string TeacherName{ get; set; }

		public string CreateTimeString{ get; set; }

		public NoticeType NoticeType{ get; set; }

		public bool IsHtml{ get; set; }

		public static void SetNoticeTypeColor(UILabel typeUILabel, NoticeType noticeType)
		{
			if (noticeType == NoticeType.ClassCommon) {
				//                noticeIndexCell.NoticeTypeImageView.Image = UIImage.FromBundle("images/notice_index_view/message.png");
				typeUILabel.Text = "班级通知";
				typeUILabel.BackgroundColor = MvxTouchColor.Blue1;
			} else if (noticeType == NoticeType.ClassEmergency) {
				//                noticeIndexCell.NoticeTypeImageView.Image = UIImage.FromBundle("images/notice_index_view/message.png");
				typeUILabel.Text = "孩子考勤";
			} else if (noticeType == NoticeType.ClassHomework) {
				//                noticeIndexCell.NoticeTypeImageView.Image = UIImage.FromBundle("images/notice_index_view/homework.png");
				typeUILabel.Text = "家庭作业";
				typeUILabel.BackgroundColor = MvxTouchColor.Purple1;
			} else if (noticeType == NoticeType.GrowMemory) {
				//                noticeIndexCell.NoticeTypeImageView.Image = UIImage.FromBundle("images/notice_index_view/message.png");
				typeUILabel.Text = "成长记忆";
			} else if (noticeType == NoticeType.KindergartenAll) {
				//                noticeIndexCell.NoticeTypeImageView.Image = UIImage.FromBundle("images/notice_index_view/yqnotice.png");
				typeUILabel.Text = "园区通知";
				typeUILabel.BackgroundColor = MvxTouchColor.Orange1;
			} else if (noticeType == NoticeType.KindergartenStaff) {
				//                noticeIndexCell.NoticeTypeImageView.Image = UIImage.FromBundle("images/notice_index_view/ywnotice.png");
				typeUILabel.Text = "园务通知";
				typeUILabel.BackgroundColor = MvxTouchColor.Lake1;
			} else if (noticeType == NoticeType.KindergartenRecipe) {
				//                noticeIndexCell.NoticeTypeImageView.Image = UIImage.FromBundle("images/notice_index_view/recipe.png");
				typeUILabel.Text = "校园食谱";
				typeUILabel.BackgroundColor = MvxTouchColor.Green1;
			} else if (noticeType == NoticeType.BabyBusNotice) {
				typeUILabel.Text = "优贝小报";
				typeUILabel.BackgroundColor = MvxTouchColor.Gray1;
			} else if (noticeType == NoticeType.BabyBusNoticeHtml) {
				typeUILabel.Text = "优贝小报";
				typeUILabel.BackgroundColor = MvxTouchColor.Gray1;
			} else {
				//                noticeIndexCell.NoticeTypeImageView.Image = UIImage.FromBundle("placeholder.png");
			}
		}

		protected virtual void PrepareCell(UITableViewCell cell)
		{
			if (cell == null)
				return;


			var noticeIndexCell = cell as NoticeIndexCell;
			cell.Accessory = UITableViewCellAccessory.None;

			noticeIndexCell.UnreadUIImageView.Hidden = IsReaded;
			noticeIndexCell.TitleUILabel.Text = NoticeTitle;
			noticeIndexCell.ContentUILabel.Text = Content;
			noticeIndexCell.TeacherNameLabel.Text = TeacherName;
			noticeIndexCell.DateUILabel.Text = CreateTimeString;

//            ClassCommon -> Blue1
//            ClassHomework -> Purple1
//            KindergartenAll -> Orange1
//            KindergartenRecipe -> Green1
//            KindergartenStaff -> Lake1


			SetNoticeTypeColor(noticeIndexCell.TypeUILabel, NoticeType);
		}

		protected override UITableViewCell GetCellImpl(UITableView tv)
		{
			UITableViewCell cell;
			tv.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			cell = tv.DequeueReusableCell(NoticeIndexCell.Key) ?? (new NoticeIndexCell() as UITableViewCell);
			PrepareCell(cell);
			cell.SetNeedsUpdateConstraints();
			cell.UpdateConstraintsIfNeeded();
			return cell as UITableViewCell;
		}

		public float GetCellHeight(NoticeIndexCell cell, UITableView tableView)
		{
			cell.LayoutIfNeeded();
			cell.UpdateConstraintsIfNeeded();

			nfloat delta = EasyLayout.MarginMedium * 4 + EasyLayout.MarginLarge * 1;

			nfloat width = 320 - EasyLayout.MarginNormal - EasyLayout.MarginMedium;

			//250是因为有向右小箭头
			var heightTypeUILabel = cell.TypeUILabel.SizeThatFits(new CGSize(100, float.MaxValue)).Height;
			var heightTitleUILabel = cell.TitleUILabel.SizeThatFits(new CGSize(width, float.MaxValue)).Height;
			var heightContentUILabel = cell.ContentUILabel.SizeThatFits(new CGSize(width, float.MaxValue)).Height;
			var heightTeacherNameLabel = cell.TeacherNameLabel.SizeThatFits(new CGSize(width, float.MaxValue)).Height;

			var height = heightTypeUILabel + heightTitleUILabel + heightContentUILabel + heightTeacherNameLabel + EasyLayout.SeparatorHeight + delta;

			//Note: SystemLayoutSizeFittingSize, 设置上下左右的margin才能用这个拿到值
			var height1 = cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height;
			height = (nfloat)Math.Max(height, height1);
			return (float)(height);
		}

		static NoticeIndexCell _calcSizeCell = null;

		public virtual float GetHeight(UITableView tableView, NSIndexPath indexPath)
		{
        
			if (_calcSizeCell == null) {
				_calcSizeCell = new NoticeIndexCell();
			}
        
        
			PrepareCell(_calcSizeCell);
			return GetCellHeight(_calcSizeCell, tableView);
		}

		public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			base.Selected(dvc, tableView, path);
			var noticeIndexView = dvc as NoticeIndexView;
			var noticeIndexViewModel = noticeIndexView.ViewModel as NoticeIndexViewModel;

			var noticeIndexCell = tableView.CellAt(path) as NoticeIndexCell;
			if (noticeIndexCell != null) {
				IsReaded = true;
				noticeIndexCell.UnreadUIImageView.Hidden = IsReaded;
			}
			noticeIndexViewModel.ShowNoticeDetailViewModel(Id, IsHtml);
		}

		protected override void Dispose(bool disposing)
		{
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