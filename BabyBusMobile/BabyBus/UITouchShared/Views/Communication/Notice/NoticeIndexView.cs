using UIKit;
using ObjCRuntime;
using CrossUI.Touch.Dialog.Elements;

using CrossUI.Touch.Dialog.OldElements;
using CoreGraphics;
using UITouchShared;
using System.Linq;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.iOS
{
	public sealed class NoticeIndexView : MvxBabybusDialogViewController
	{
		NoticeIndexViewModel _baseViewModel = null;

		public NoticeIndexView()
			: base(UITableViewStyle.Plain,
			                null,
			                false)
		{
			var label = new UILabel(new CGRect(0, 0, 100, 35));
			label.Text = "信息通知";
			this.NavigationItem.TitleView = label;
			label.TextAlignment = UITextAlignment.Center;
			label.TextColor = MvxTouchColor.White;

			this.RefreshRequested += delegate {
				_baseViewModel.RefreshCommand.Execute();
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationItem.SetHidesBackButton(false, false);
			if (_baseViewModel != null)
				_baseViewModel.RefreshCommand.Execute();
			//Stats Page Report
			StatsUtils.LogPageReport(PageReportType.NoticeIndex);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		void SetBadge()
		{
			var unReadList = Bindable.Elements.Where(ele => !ele.Readed());
			if (unReadList != null && unReadList.Any()) {
				this.TabBarItem.SetBadge();
			} else {
				this.TabBarItem.ResetBadge();
			}
		}

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();
			_baseViewModel = ViewModel as NoticeIndexViewModel;
           
			_baseViewModel.FirstLoadedEventHandler += (object sender, object e) => this.InvokeOnMainThread(() => {
				Bindable.InitData(_baseViewModel.ListObject);
				SetBadge();
			});

			_baseViewModel.DataRefreshed += (sender, addList) => this.InvokeOnMainThread(() => {
				Bindable.AddRowsBeforeHead(addList);
				this.ReloadComplete();
				SetBadge();
			});

			_baseViewModel.DataLoadedMore += (sender, addList) => this.InvokeOnMainThread(() => {
				if (addList.Count > 0) {
					Bindable.AddRowsAfterTail(addList);
				}
				this.ReloadComplete();
				_loadMoreElement.Animating = false;
				SetBadge();
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
      
		BindableSection<NoticeIndexViewElement> bindable = new BindableSection<NoticeIndexViewElement>();

		public BindableSection<NoticeIndexViewElement> Bindable{ get { return bindable; } set { bindable = value; } }

		LoadMoreElement _loadMoreElement = null;

		RootElement GetRoot()
		{
			_rootElement = new RootElement("");
			_loadMoreElement = new LoadMoreElement("加载更多", "正在加载", delegate {
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
	//class
}
//namespace

