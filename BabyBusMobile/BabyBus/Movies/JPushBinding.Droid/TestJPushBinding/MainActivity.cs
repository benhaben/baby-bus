using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CN.Jpush.Android.Api;

namespace TestJPushBinding
{
    [Activity(Label = "极光Push", MainLauncher = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new string[] { "android.intent.action.MAIN", "android.intent.category.LAUNCHER" })]
    public class MainActivity : InstrumentedActivity
    {
        int count = 1;

        public static bool isForeground = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate
            {
                button.Text = string.Format("{0} clicks!", count++);
            };
        }


        protected override void OnResume()
        {
            isForeground = true;
            base.OnResume();
        }



        protected override void OnPause()
        {
            isForeground = false;
            base.OnPause();
        }
    }
}


