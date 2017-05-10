using System.Collections.Generic;
using System.Threading.Tasks;
using BabyBus.iOS;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using CoreGraphics;
using CrossUI.Touch.Dialog.Elements;
using CrossUI.Touch.Dialog.OldElements;
using Foundation;
using MWPhotoBrowserBinding;
using ObjCRuntime;
using SDWebImage;
using UIKit;
using UITouchShared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.iOS
{


    public sealed class MemoryIndexView : MvxBabybusDialogViewController
    {
        MemoryIndexViewModel _baseViewModel = null;

        public MemoryIndexView()
            : base(UITableViewStyle.Plain,
                   null,
                   false)
        {
            var label = new UILabel(new CGRect(0, 0, 100, 35));
            label.Text = "成长记忆";
            this.NavigationItem.TitleView = label;
            label.TextAlignment = UITextAlignment.Center;
            label.TextColor = MvxTouchColor.White;

            this.RefreshRequested += delegate
            {
                Task.Run(() =>
                    {
                        _baseViewModel.RefreshCommand.Execute();
                    });
            };
            //Note: 如果不设置这个标志，tap会变成长按
            AddGestureWhenTap = false;

            var messenger = Mvx.Resolve<IMvxMessenger>();
            messenger.Subscribe<MemoryIndexTableCell.CollectionView9PictureDataSource.SelectImagesMvxMessage>(msg =>
                {
                    var imageList = msg.ImagesList;
                    NSObject[] array = new NSObject[imageList.Count];
                    for (int i = 0; i < imageList.Count; i++)
                    {
                        array[i] = imageList[i];
                    }

                    MWPhotoBrowser browser = new MWPhotoBrowser(array);
                    browser.DisplayActionButton = true;
                    browser.DisplayNavArrows = true;
                    browser.DisplaySelectionButtons = false;
                    browser.AlwaysShowControls = false;
                    browser.ZoomPhotosToFill = true;
                    browser.EnableGrid = false;
                    browser.StartOnGrid = false;
                    browser.EnableSwipeToDismiss = true;

//                var view = new FullScreenImageView(msg.ImagesList);
//                view.HidesBottomBarWhenPushed = true;

                    //Stats Page Report
                    StatsUtils.LogPageReport(PageReportType.GrowMomeryDetail);

                    if (this.NavigationController != null)
                    {
                        this.NavigationController.PushViewController(browser, false);
                    }
                }, MvxReference.Strong);
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
            SDWebImageManager.SharedManager.ImageCache.ClearMemory();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationItem.SetHidesBackButton(false, false);
            if (_baseViewModel != null)
                _baseViewModel.RefreshCommand.Execute();

            //Stats Page Report
            StatsUtils.LogPageReport(PageReportType.GrowMomeryIndex);
            this.TabBarItem.ResetBadge();
        }

        //        public override UIStatusBarStyle PreferredStatusBarStyle() {
        //            return UIStatusBarStyle.LightContent;
        //        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();
            _baseViewModel = ViewModel as MemoryIndexViewModel;
            var messenger = Mvx.Resolve<IMvxMessenger>();
           
            messenger.Subscribe<JPushNotificationMessage>(msg => this.InvokeOnMainThread(() =>
                    {
                        var mainView = msg.Sender as MainView;
                        if (mainView != null && mainView.SelectedIndex == 1)
                        {
                            _baseViewModel.RefreshCommand.Execute();
                        }
                    }));

            _baseViewModel.FirstLoadedEventHandler += (object sender, object e) => this.InvokeOnMainThread(() =>
                {
                    Bindable.InitData(_baseViewModel.ListObject);
//                    var set = this.CreateBindingSet<MemoryIndexView, MemoryIndexViewModel>();
//                    set.Bind(Bindable).For(v => v.ItemsSource).To(vm => vm.ListObject);
//                    set.Apply();
                });


            _baseViewModel.DataRefreshed += (sender, addList) => this.InvokeOnMainThread(() =>
                {
                    Bindable.AddRowsBeforeHead(addList);
                    this.ReloadComplete();
                    this.ReloadData();
                });

            _baseViewModel.DataLoadedMore += (sender, addList) => this.InvokeOnMainThread(() =>
                {
                    Bindable.AddRowsAfterTail(addList);
                    this.ReloadComplete();
                    _loadMoreElement.Animating = false;
                    //                    var sectionCount = TableView.NumberOfSections() - 1;
                    //                    if (sectionCount >= 0) {
                    //                        var rowCount = TableView.NumberOfRowsInSection(sectionCount) - 1;
                    //                        if (rowCount >= 0) {
                    //                            var indexPath = NSIndexPath.FromRowSection(rowCount, sectionCount);
                    //                            this.TableView.ScrollToRow(indexPath, UITableViewScrollPosition.Bottom, false);
                    //                        }
                    //                    }
                });

            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Refresh
                    , (sender, args) => _baseViewModel.RefreshCommand.Execute())
                , true);

            // ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
                EdgesForExtendedLayout = UIRectEdge.None;

            // Perform any additional setup after loading the view, typically from a nib.
            Root = (RootElement)GetRoot();

        }

        RootElement _rootElement;

        BindableSection<MemoryIndexViewElement> bindable = new BindableSection<MemoryIndexViewElement>();

        BindableSection<MemoryIndexViewElement> Bindable{ get { return bindable; } set { bindable = value; } }

        LoadMoreElement _loadMoreElement = null;

        RootElement GetRoot()
        {
            _rootElement = new RootElement("");

            _loadMoreElement = new LoadMoreElement("加载更多", "正在加载", delegate
                {

                    _baseViewModel.LoadMoreCommand.Execute();
                });
            var footerSection = new Section();
            footerSection.Add(_loadMoreElement);
            _rootElement.Add(Bindable);
            _rootElement.Add(footerSection);
            Bindable.FooterView = new UIView();

            return _rootElement;
        }


    }
}

