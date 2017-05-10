using System;
using BabyBus.iOS;
using UIKit;
using CoreGraphics;
using ObjCRuntime;
using CrossUI.Touch.Dialog.Elements;

using UITouchShared;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
	public class MITestView : MvxBabybusDialogViewController
	{
		MITestViewModel _baseViewModel = null;

		public MITestView()
			: base(UIKit.UITableViewStyle.Plain, null, false)
		{
			var label = new UILabel(new CGRect(0, 0, 100, 35));
			label.Text = "";
			this.NavigationItem.TitleView = label;
			label.TextAlignment = UITextAlignment.Center;
			label.TextColor = MvxTouchColor.White;

//			this.RefreshRequested += delegate {
//				//_baseViewModel.RefreshCommand.Execute();
//			};
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			NavigationItem.SetHidesBackButton(false, false);
//			if (_baseViewModel != null)
//				_baseViewModel.RefreshCommand.Execute();
		}

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();

			_baseViewModel = ViewModel as MITestViewModel;

			_baseViewModel.FirstLoadedEventHandler += (sender, e) => InvokeOnMainThread(() => {
				Bindable.InitData(_baseViewModel.AssessIndexList);
				this.ReloadData();


			});

			var btnSend = new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, e) => {
				var alert = new UIAlertView("提示", "您确定要发送吗，需要再检查一下吗？", null, "发送", new String []{ "检查一下" });
				alert.Clicked += (s, args) => {
					if (args.ButtonIndex == 0) {
						if (_baseViewModel != null) {
							_baseViewModel.SendQuestions.Execute();
						}
					}
				};
				alert.AlertViewStyle = UIAlertViewStyle.Default;
				alert.Show();
			});
			NavigationItem.SetRightBarButtonItem(btnSend, true);

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			// Perform any additional setup after loading the view, typically from a nib.
			Root = (RootElement)GetRoot();
		}

		RootElement _rootElement;

		BindableSection<MITestElement> bindable = new BindableSection<MITestElement>();

		BindableSection<MITestElement> Bindable {
			get{ return bindable; }
			set{ bindable = value; }
		}

		RootElement GetRoot()
		{
			_rootElement = new RootElement("");
			_rootElement.Add(Bindable);

			return _rootElement;
		}
	}
}

