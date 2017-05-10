using Android.App;
using Android.OS;
using Android.Webkit;
using BabyBus.Droid.Views;
using RestSharp;
using BabyBusSSApi.ServiceModel.DTO.Reponse;
using System.Net;
using System.Text;
using System;
using Android.Content;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Views.Communication;

namespace BabyBus.Droid
// Analysis restore CheckNamespace
{
	[Activity(Theme = "@style/CustomTheme")]
	public class  NoticeHtmlView : ViewBase<NoticeDetailHtmlViewModel>
	{
		private string Filename = string.Empty;
		private readonly Handler handler = new Handler();

		public override void ShareMessage()
		{
			base.ShareMessage();
			var intent = new Intent(this, typeof(OneKeyShare));
			intent.PutExtra("Description", ViewModel.Notice.Abstract);
			intent.PutExtra("ImageName", ViewModel.Notice.FirstImage);
			intent.PutExtra("Title", ViewModel.Notice.Title);
			intent.PutExtra("WebpageUrl", string.Format("http://115.28.88.175:8099/api/sharehtml?ContentType={0}&Id={1}", 0, ViewModel.Notice.NoticeId));
			StartActivity(intent);
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);
			SetShareTitleWithBack(Resource.Layout.Page_Content_WebView, " 详细");
			var webview = FindViewById<WebView>(Resource.Id.LocalWebView);

			ViewModel.FirstLoadedEventHandler += (sender1, e1) => handler.Post(() => {
				var data = System.Text.Encoding.Default.GetString(Convert.FromBase64String(ViewModel.Notice.Content));
				webview.LoadData(data, "text/html; charset=UTF-8", null);
			});
		}

		protected override void OnDestroy()
		{
			var webview = FindViewById<WebView>(Resource.Id.LocalWebView);
			base.OnDestroy();
			webview.Destroy();
		}
			
	}
}
