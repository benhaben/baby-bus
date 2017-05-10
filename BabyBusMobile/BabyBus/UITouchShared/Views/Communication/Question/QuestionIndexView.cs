using UIKit;
using ObjCRuntime;
using CrossUI.Touch.Dialog.Elements;

using System.Threading.Tasks;
using CrossUI.Touch.Dialog.OldElements;
using CoreGraphics;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using UITouchShared;
using BabyBus.Logic.Shared;


namespace BabyBus.iOS
{
	public sealed class QuestionIndexView : MvxBabybusDialogViewController
	{
		QuestionIndexViewModel _baseViewModel = null;

		public QuestionIndexView()
			: base(UITableViewStyle.Plain,
			                null,
			                false)
		{

			var label = new UILabel(new CGRect(0, 0, 40, 35));
			label.Text = "家园联系";
			label.TextAlignment = UITextAlignment.Center;
			this.NavigationItem.TitleView = label;
			label.TextColor = MvxTouchColor.White;

			this.RefreshRequested += delegate {
				Task.Run(() => {
					_baseViewModel.RefreshCommand.Execute();
				});
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
			//Question always get all data, so don't refresh here
			if (_baseViewModel != null)
				_baseViewModel.RefreshCommand.Execute();
			this.TabBarItem.ResetBadge();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		//        void SetBadge(List<QuestionModel> addList)
		//        {
		//            var unReadList = addList.Select(data => data.IsHaveAnswers != true);
		//            if (unReadList != null && unReadList.Any())
		//            {
		//                this.TabBarItem.SetBadge();
		//            }
		//            else
		//            {
		//                this.TabBarItem.ResetBadge();
		//            }
		//        }

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();
			_baseViewModel = ViewModel as QuestionIndexViewModel;

			var messenger = Mvx.Resolve<IMvxMessenger>();
			messenger.Subscribe<JPushNotificationMessage>(msg => {
				this.InvokeOnMainThread(() => {
					var mainView = msg.Sender as MainView;
					if (mainView != null && mainView.SelectedIndex == 3) {
						_baseViewModel.ShowDetailCommand(msg.Id);
					}
				});
			});


			_baseViewModel.FirstLoadedEventHandler += (object sender, object e) => this.InvokeOnMainThread(() => {
				Bindable.InitData(_baseViewModel.ListObject);
//                    var set = this.CreateBindingSet<QuestionIndexView, QuestionIndexViewModel>();
//                    set.Bind(Bindable).For(v => v.ItemsSource).To(vm => vm.ListObject);
//                    set.Apply();
			});

			_baseViewModel.DataRefreshed += (sender, addList) => this.InvokeOnMainThread(() => {
				Bindable.UpdateElementsAndReloadTable(addList);
				this.ReloadComplete();

//                    SetBadge(addList);
			});

			_baseViewModel.DataLoadedMore += (sender, addList) => this.InvokeOnMainThread(() => {
				Bindable.AddRowsAfterTail(addList);
				this.ReloadComplete();
				_loadMoreElement.Animating = false;
//                    SetBadge(addList);
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

		BindableSection<QuestionAnswerElement> bindable = new BindableSection<QuestionAnswerElement>();

		BindableSection<QuestionAnswerElement> Bindable{ get { return bindable; } set { bindable = value; } }

		LoadMoreElement _loadMoreElement = null;

		RootElement GetRoot()
		{
			_rootElement = new RootElement("");

			_loadMoreElement = new LoadMoreElement("加载数据", "正在加载", delegate {
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

