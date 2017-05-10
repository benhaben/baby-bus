using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using BabyBus.iOS;
using CoreGraphics;
using Cirrious.MvvmCross.Binding.BindingContext;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{

    public class FeedBackView : MvxBabyBusBaseViewController
    {
        FeedBackViewModel _baseViewModel = null;


        UITableView _table;

        public UITableView Table
        {
            get
            {
                if (_table == null)
                {
                    _table = new UITableView();
                    _table.Source = new FeedBackTableSource();
                    _table.TableFooterView = new UIView();
                }
                return _table;
            }
        }

        public FeedBackView()
        {
            AddGestureWhenTap = false;
           
        }

        UILabel _title1 = null;

        public virtual UILabel Title
        {
            get
            {
                if (_title1 == null)
                {   
                    _title1 = new UILabel();
                    _title1.Text = "请输入您的宝贵建议";
                    _title1.Font = EasyLayout.TitleFont;
                    _title1.TextAlignment = UITextAlignment.Center;
                }
                return _title1;
            }
           
        }


        UITextView _content = null;

        public UITextView Content
        {
            get
            {
                if (_content == null)
                {
                    _content = new UITextView();
                    _content.Font = EasyLayout.ContentFont;
                    _content.BackgroundColor = MvxTouchColor.White;
                    _content.TextColor = MvxTouchColor.Gray1;
                    _content.TextContainerInset = new UIEdgeInsets(10, 10, 10, 10);
                }
                return _content;
            }
        }

        void CreateBinding()
        {
            var set = this.CreateBindingSet<FeedBackView, FeedBackViewModel>();
            set.Bind(Content).To(vm => vm.Content);
            set.Apply();
        }

        public override void PrepareViewHierarchy()
        {
            base.PrepareViewHierarchy();

            UIView[] v =
                {
                    Table,
                    Title,
                    Content,
                };

            Container.AddSubviews(v);
            #if DEBUG1
            Table.BackgroundColor = UIColor.Red;
            Content.BackgroundColor = UIColor.Orange;
            this.View.BackgroundColor = UIColor.Blue;
            #endif
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            _baseViewModel = this.ViewModel as FeedBackViewModel;
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) =>
                {
                    var alert = new UIAlertView("提示", "您确定要发送吗，别忘了检查错别字哦？", null, "发送", new String []{ "回到编辑" });
                    alert.Clicked += (object s, UIButtonEventArgs e) =>
                    {
                        if (e.ButtonIndex == 0)
                        {
                            _baseViewModel.SendFeedBackCommand.Execute();
                            this.NavigationController.PopViewController(true);
                        }
                        else
                        {
                        }
                    };
                    alert.AlertViewStyle = UIAlertViewStyle.Default;
                    alert.Show();
                });
            this.NavigationItem.SetRightBarButtonItem(doneButton, true);
            Content.Changed += (sender, e) =>
            {
                nfloat heightChanged = Content.SizeThatFits(new CGSize(300, float.MaxValue)).Height;
                _heightConstrainContent.Constant = _heightConstrainContent.Constant > heightChanged ? _heightConstrainContent.Constant : heightChanged;
            };

            CreateBinding();
        }

        protected override void SetBackgroundImage()
        {
            base.SetBackgroundImage();
            this.View.BackgroundColor = MvxTouchColor.White2;
        }

        public override void SetUpConstrainLayout()
        {
            base.SetUpConstrainLayout();
            nfloat oneP = 0f;
            nfloat TableHeight = 100f;

            View.ConstrainLayout 
            (
                // Analysis disable CompareOfFloatsByEqualityOperator
                () => 
                Table.Frame.Height == TableHeight
                && Table.Frame.Left == Container.Frame.Left
                && Table.Frame.Right == Container.Frame.Right
                && Table.Frame.Top == Container.Frame.Top + oneP


                && Title.Frame.Left == Container.Frame.Left
                && Title.Frame.Right == Container.Frame.Right
                && Title.Frame.Top == Table.Frame.Bottom + EasyLayout.MarginMedium

                && Content.Frame.Left == Container.Frame.Left
                && Content.Frame.Right == Container.Frame.Right
                && Content.Frame.Top == Title.Frame.Bottom + EasyLayout.MarginMedium

                && Container.Frame.Bottom == Content.Frame.Bottom

                // Analysis restore CompareOfFloatsByEqualityOperator
            );

            //Note: do not change frame, change Constrain to resize control
            var constrains =
                View.ConstrainLayout(
                    () => Content.Frame.Height == EasyLayout.HeightOfContent
                );
            _heightConstrainContent = constrains[0];
        }

        NSLayoutConstraint _heightConstrainContent;


        public class FeedBackTableSource : UITableViewSource
        {
            string cellIdentifier = "FeedBackTableSourceTableCell";

            //TODO: change to dic if you like
            private readonly List<string> titles = new List<string>
            {
                "反馈邮箱", "反馈电话"
            };



            private readonly List<string> subTitle = new List<string>
            {
                "support@mreliable.com", "4009922586"
            };

            public FeedBackTableSource()
            {
            }

            /// <summary>
            /// Called by the TableView to determine how many cells to create for that particular section.
            /// </summary>
            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return titles.Count;
            }

            /// <summary>
            /// Called when a row is touched
            /// </summary>
            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                //            ShowWeb(subTitle[indexPath.Row]);
                var row = indexPath.Row;
                if (row == 0)
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl("mailto://support@mreliable.com"));
                }
                else if (row == 1)
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl("tel://4009922586"));
                }
            }

            /// <summary>
            /// Called when the DetailDisclosureButton is touched.
            /// Does nothing if DetailDisclosureButton isn't in the cell
            /// </summary>
            public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
            {
                //            ShowWeb(subTitle[indexPath.Row]);
            }

            /// <summary>
            /// Called by the TableView to get the actual UITableViewCell to render for the particular row
            /// </summary>
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                // request a recycled cell to save memory
                UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
                // UNCOMMENT one of these to use that style
                //            var cellStyle = UITableViewCellStyle.Default;
                //          var cellStyle = UITableViewCellStyle.Subtitle;
                //          var cellStyle = UITableViewCellStyle.Value1;
                var cellStyle = UITableViewCellStyle.Value1;

                // if there are no cells to reuse, create a new one
                if (cell == null)
                {
                    cell = new UITableViewCell(cellStyle, cellIdentifier);
                }

                // UNCOMMENT one of these to see that accessory
                //            cell.Accessory = UITableViewCellAccessory.Checkmark;
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                //            cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;  // implement AccessoryButtonTapped
                //cell.Accessory = UITableViewCellAccessory.None; // to clear the accessory





                cell.TextLabel.Text = titles[indexPath.Row];

                //             Default style doesn't support Subtitle
                if (cellStyle == UITableViewCellStyle.Subtitle
                    || cellStyle == UITableViewCellStyle.Value1
                    || cellStyle == UITableViewCellStyle.Value2)
                {
                    cell.DetailTextLabel.Text = subTitle[indexPath.Row];
                    cell.DetailTextLabel.TextColor = MvxTouchColor.Blue;
                }
                //
                //            // Value2 style doesn't support an image
                //            if (cellStyle != UITableViewCellStyle.Value2)
                //                cell.ImageView.Image = UIImage.FromFile("Images/" + tableItems[indexPath.Row].ImageName);

                return cell;
            }
        }
    }

   
}

