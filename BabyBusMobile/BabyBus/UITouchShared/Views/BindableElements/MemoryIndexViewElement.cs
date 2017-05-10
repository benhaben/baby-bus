using System;
using CrossUI.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Binding.BindingContext;
using UIKit;
using Foundation;
using System.Collections.Generic;
using CoreGraphics;
using BabyBus.iOS;
using CrossUI.Touch.Dialog;
using SDWebImage;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
    public class MemoryIndexViewElement : StyledMultilineElement , IBindableElement
    {
        public MemoryIndexViewElement()
            : base("", "", UITableViewCellStyle.Subtitle)
        {
            this.CreateBindingContext();
            this.Font = EasyLayout.ContentFont;
            this.SubtitleFont = EasyLayout.TinyFont;
        }

        public IMvxBindingContext BindingContext { get; set; }

        #region IBindableElement implementation

        void IBindableElement.DoBind()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<MemoryIndexViewElement, NoticeModel>();
                    set.Bind().For(me => me.Title).To(p => p.Title);
                    set.Bind().For(me => me.Content).To(p => p.Content);
                    set.Bind().For(me => me.RealName).To(p => p.RealName);
                    set.Bind().For(me => me.HeadImage).To(p => p.HeadImage);
                    set.Bind().For(me => me.CreateDate).To(p => p.CreateTime).WithConversion("DateTimeOffset");
                    set.Bind().For(me => me.ImageList).To(p => p.ImageList);
                    set.Apply();
                });
        }

        public virtual object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        #endregion

        public string HeadImage
        {
            get;
            set;
        }

        public string CreateDate { get; set; }

        public string RealName { get; set; }

        public string Title{ get; set; }

        public string Content{ get; set; }

        public List<string> ImageList{ get; set; }

        protected override void PrepareCell(UITableViewCell cell)
        {
            if (cell == null)
                return;

            var memoryIndexTableCell = cell as MemoryIndexTableCell;
            cell.Accessory = Accessory;

//            memoryIndexTableCell.ImageView.SetImage(_extraInfo.Uri, UIImage.FromBundle(""));
            memoryIndexTableCell.Title.Font = Font;
            memoryIndexTableCell.Content.Font = this.SubtitleFont;

            memoryIndexTableCell.Title.Text = Title;
            memoryIndexTableCell.Content.Text = CreateDate + "    " + RealName;
            var CollectionView9Picture = memoryIndexTableCell.CollectionView9Picture;
            var dataSource = new MemoryIndexTableCell.CollectionView9PictureDataSource();
            dataSource.SetImageList(ImageList);
            dataSource.Caption = Title;
            dataSource.ImageViewSize = new CGSize(80, 80);

            string url = Constants.ThumbServerPath + HeadImage + Constants.ThumbRule;
            var uri = new Uri(url);
            //处理中文url
            var nsurl = new NSUrl(uri.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped));
            var defaultImageName = "placeholder.png";
            memoryIndexTableCell.ImageView.SetImage(nsurl, UIImage.FromBundle(defaultImageName));

            CollectionView9Picture.Source = dataSource;
            CollectionView9Picture.ReloadData();
            CollectionView9Picture.AllowsSelection = true;
        }

        public override float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            var data = ImageList;
            nfloat height = 250f;
            nfloat imageheight = 80f;
            if (data != null)
            {
                if (data.Count <= 0)
                {
                    height = imageheight * 0f;
                }
                else if (data.Count > 0 && data.Count <= 3)
                {
                    height = imageheight;
                }
                else if (data.Count > 3 && data.Count <= 6)
                {
                    height = imageheight * 2;
                }
                else if (data.Count > 6 && data.Count <= 9)
                {
                    height = imageheight * 3;
                }
            }

            if (!string.IsNullOrWhiteSpace(this.Content))
            {
                height += EasyLayout.NormalTextFieldHeight;
            }

            height += EasyLayout.NormalTextFieldHeight;

            return (float)height + 60;
        }

        protected override  UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey) ??
                       new MemoryIndexTableCell() as UITableViewCell;
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

