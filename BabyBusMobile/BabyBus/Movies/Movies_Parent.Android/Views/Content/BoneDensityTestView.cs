using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace BabyBus.Droid.Views.Content
{
    [Activity(Label = "π«√‹∂»Ω‚∂¡")]
    public class BoneDensityTestView : ActivityBase
    {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Page_Content_WebView);
            var webview = FindViewById<WebView>(Resource.Id.LocalWebView);

            webview.LoadUrl("file:///android_asset/Content/bone_density_test.htm");
        }
    }
}