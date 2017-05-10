using Android.App;
using Android.OS;
using Android.Webkit;
using BabyBus.Droid.Views;
using RestSharp;
using BabyBusSSApi.ServiceModel.DTO.Reponse;
using System.Net;
using System.Text;
using System;
using Android.Content.PM;
using Android.Content;
using BabyBus.Droid.Views.Communication;

namespace BabyBus.Droid
	// Analysis restore CheckNamespace
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class ContentDetailView : ActivityBase
	{
		private string Filename = string.Empty;
		private Boolean EnablerShared = false;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);


			string loaddata = string.Empty;
			if (Intent.Extras != null) {
				Filename = Intent.Extras.GetString("FileName");
				Title = Intent.Extras.GetString("Title");
				loaddata = Intent.Extras.GetString("Data");
				EnablerShared = Intent.Extras.GetBoolean("EnablerShared");
			}
			if (EnablerShared) {
				SetShareTitleWithBack(Resource.Layout.Page_Content_WebView, Resource.String.notice_detial);
			} else {
				SetCustomTitleWithBack(Resource.Layout.Page_Content_WebView, Title);
			}


			// Create your application here

			SetContentView(Resource.Layout.Page_Content_WebView);
			var webview = FindViewById<WebView>(Resource.Id.LocalWebView);

			if (!string.IsNullOrEmpty(Filename)) {
					
				if (Filename.Contains("http://")) {
					//					webview.Settings.JavaScriptEnabled = true;
					//					webview.DestroyDrawingCache();
					//
					//					webview.SetHttpAuthUsernamePassword("http://115.28.88.175:8099/api/", "", "18502992708", "123456");
					//webview.LoadUrl(Filename);

					var iServiceClient = new RestClient("http://115.28.2.41:8888/");
					var request = new RestRequest("/authenticate/credentials", Method.GET);
					request.RequestFormat = DataFormat.Json;
					request.AddQueryParameter("Username", "18502992708");
					request.AddQueryParameter("Password", "123456");
					request.AddQueryParameter("Remeberme", "1");

					var authResponse = iServiceClient.Get<AuthenticateResponse>(request);

					if (authResponse != null && authResponse.Cookies.Count > 0) {
						CookieSyncManager.CreateInstance(this);
						var cookieManager = CookieManager.Instance;
						cookieManager.RemoveSessionCookie();
						cookieManager.SetAcceptCookie(true);

						var cookies = new StringBuilder();
						foreach (var sessionCookie in authResponse.Cookies) {
							var cookie = new Cookie(sessionCookie.Name, sessionCookie.Value, sessionCookie.Path, sessionCookie.Domain);

							cookies.AppendFormat("{0}={1};", cookie.Name, cookie.Value);
						}
						cookieManager.SetCookie("115.28.2.41", cookies.ToString());
						CookieSyncManager.Instance.Sync();
					}
					webview.Settings.JavaScriptEnabled = true;
					webview.DestroyDrawingCache();
					webview.LoadUrl(Filename);
				} else {
					webview.LoadUrl("file:///android_asset/Content/" + Filename);
				}
			} else if (!string.IsNullOrEmpty(loaddata)) {
				webview.LoadData(loaddata, "text/html; charset=UTF-8", null);
			}
	            
		}

		public override void ShareMessage()
		{
			var intent = new Intent(this, typeof(OneKeyShare));
			intent.PutExtra("Title", Title);
			intent.PutExtra("WebpageUrl", Filename);
			StartActivity(intent);
		}

		protected override void OnDestroy()
		{
			var webview = FindViewById<WebView>(Resource.Id.LocalWebView);
			base.OnDestroy();
			webview.Destroy();
		}

		private void CreateLoginCook()
		{
			var iServiceClient = new RestClient("http://115.28.2.41:8888/");
			var request = new RestRequest("/authenticate/credentials", Method.GET);
			request.RequestFormat = DataFormat.Json;
			request.AddQueryParameter("Username", "18502992708");
			request.AddQueryParameter("Password", "123456");
			request.AddQueryParameter("Remeberme", "1");
			var authResponse = iServiceClient.Get<AuthenticateResponse>(request);
			if (authResponse != null && authResponse.Cookies.Count > 0) {
			}
			
			//					if (authResponse != null && authResponse.Cookies.Count > 0)
			//					{
			//						//set cookies
			//						List<NSHttpCookie> cookies = new List<NSHttpCookie>();
			//						foreach (var sessionCookie in authResponse.Cookies)
			//						{
			//							var cookie = new Cookie(sessionCookie.Name, sessionCookie.Value, sessionCookie.Path, sessionCookie.Domain);
			//		
			//		
			//		
			//							//load url from remote service
			//							NSHttpCookieStorage.SharedStorage.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;
			//							var cookieDict = new NSMutableDictionary();
			//							cookieDict.Add(NSHttpCookie.KeyName, new NSString(sessionCookie.Name));
			//							cookieDict.Add(NSHttpCookie.KeyValue, new NSString(sessionCookie.Value));
			//							cookieDict.Add(NSHttpCookie.KeyPath, new NSString(sessionCookie.Path));
			//							cookieDict.Add(NSHttpCookie.KeyDomain, new NSString(sessionCookie.Domain));
			//		
			//							cookies.Add(new NSHttpCookie(cookieDict));
			//						}
			//						NSHttpCookieStorage.SharedStorage.SetCookies(cookies.ToArray(), new NSUrl("http://115.28.2.41:8888/"), new NSUrl("http://115.28.2.41:8888/"));
			//						NSUrlRequest path = new NSUrlRequest(_url);
			//						_webView.LoadRequest(path);
			//						_webView.ScalesPageToFit = false;
			//						_webView.ContentMode = UIViewContentMode.ScaleAspectFit;
			//					}
		}
	}
}