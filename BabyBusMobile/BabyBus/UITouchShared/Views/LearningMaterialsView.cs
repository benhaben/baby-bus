using System;
using UIKit;
using CrossUI.Touch.Dialog.Elements;
using BabyBus.Logic.Shared;

namespace BabyBus.iOS
{
	public class LearningMaterialsView: MvxBabybusDialogViewController
	{
		LearningMaterialsViewModel _baseViewModel = null;
		bool firstLoad = true;

		public LearningMaterialsView()
			: base(UITableViewStyle.Plain,
			       null,
			       true)
		{
			this.HidesBottomBarWhenPushed = true;
		}

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();

			_baseViewModel = ViewModel as LearningMaterialsViewModel;
			Root = new RootElement("");
			_baseViewModel.FirstLoadedEventHandler += (sender, e) => this.InvokeOnMainThread(() => {
				InitRoot();
			});
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			if (!firstLoad) {
				_baseViewModel.FirstLoad();
			}
		}

		Section _section;

		void InitRoot()
		{
			if (firstLoad) {
				_section = new Section();
			} else {
				_section.Clear();
			}

			var baseUrl = Constants.BaseHtmlUrl + "/AlbumSongs_Mobile.html";
			int i = 0;
			foreach (var album in _baseViewModel.Albums) {
				_section.Add(new HtmlElement(album.Name, string.Format(baseUrl + "?AlbumId={0}", album.Id)));
			}
			_section.Add(new HtmlElement("经典儿歌", "http://g.m.beva.com/?from=wwface"));
			_section.Add(new HtmlElement("经典故事", "http://t.m.beva.com/gushi/?from=wwface"));
			_section.Add(new HtmlElement("幼儿百科", "http://m.beva.com/baike/?from=wwface"));
			_section.Add(new HtmlElement("亲子乐园", "http://m.beva.com/qinzi/?from=wwface"));

			#if false
			foreach (var album in _baseViewModel.Albums) {
				if (_baseViewModel.PaymentStatus) {
					_section.Add(new HtmlElement(album.Name, string.Format(baseUrl + "?AlbumId={0}", album.Id)));
				} else if (!_baseViewModel.PaymentStatus && i++ <= 3) {
					_section.Add(new HtmlElement(album.Name + "(试听)", string.Format(baseUrl + "?AlbumId={0}", album.Id)));
				} else {
					_section.Add(new PaymentElement(album.Name));
				}
			}

			if (_baseViewModel.PaymentStatus) {
				_section.Add(new HtmlElement("经典儿歌", "http://g.m.beva.com/?from=wwface"));
				_section.Add(new HtmlElement("经典故事", "http://t.m.beva.com/gushi/?from=wwface"));
				_section.Add(new HtmlElement("幼儿百科", "http://m.beva.com/baike/?from=wwface"));
				_section.Add(new HtmlElement("亲子乐园", "http://m.beva.com/qinzi/?from=wwface"));
			} else {
				_section.Add(new PaymentElement("经典儿歌"));
				_section.Add(new PaymentElement("经典故事"));
				_section.Add(new PaymentElement("幼儿百科"));
				_section.Add(new PaymentElement("亲子乐园"));
			}
			#endif

			if (firstLoad) {
				Root.Add(_section);
			} else {
				Root.Reload(_section, UITableViewRowAnimation.Automatic);
			}
			firstLoad = false;
		}
	}

}