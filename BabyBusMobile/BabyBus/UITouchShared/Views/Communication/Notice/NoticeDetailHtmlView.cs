using System;
using BabyBus.iOS;
using UIKit;
using BabyBus.Logic.Shared;
using System.IO;
using Foundation;
using Cirrious.MvvmCross.Touch.Views;

namespace UITouchShared
{
	public class NoticeDetailHtmlView:MvxBabyBusBaseViewController
	{
		NoticeDetailHtmlViewModel _baseViewModel = null;

		UIWebView _webView;

		public override void OnViewDidLoad()
		{
			base.OnViewDidLoad();

			_baseViewModel = ViewModel as NoticeDetailHtmlViewModel;
			UIBarButtonItem backItem = new UIBarButtonItem();
			this.NavigationItem.BackBarButtonItem = backItem;

			var shareImage = UIImage.FromBundle("icon_share.png");

			var btnShare = new UIBarButtonItem(shareImage, UIBarButtonItemStyle.Plain
				, (sender, e) => {
				var share = new ShareModel {
					Id = _baseViewModel.NoticeId,
					Title = _baseViewModel.Notice.Title,
					ContentType = 2,
					Description = _baseViewModel.Notice.AbstractDisplayForiOS,
				};

				this.ShowSharedActionSheet(share);
			});
			NavigationItem.SetRightBarButtonItem(btnShare, true);

			_baseViewModel.FirstLoadedEventHandler += (sender, e) => InvokeOnMainThread(() => {
				string header = "<html><head><style type='text/css'>" + Constants.CSS + "</style></head><body>";
				string footer = "</body></html>";
				string contentDirectoryPath = Path.Combine(NSBundle.MainBundle.BundlePath, "Views/WebContents/Content/");
				var str = System.Text.UTF8Encoding.Default.GetString(Convert.FromBase64String(_baseViewModel.Notice.Content));
				var html = header + str + footer;
				_webView.LoadHtmlString(html ?? string.Empty, new NSUrl(contentDirectoryPath, true));
			});

			var frame = View.Frame;
			_webView = new UIWebView(frame);

			View.AddSubview(_webView);
		}


	}
}

