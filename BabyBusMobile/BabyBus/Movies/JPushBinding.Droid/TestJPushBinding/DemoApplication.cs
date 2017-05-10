using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CN.Jpush.Android.Api;

namespace TestJPushBinding
{
	[Application (Icon = "@drawable/Icon", Label = "@string/app_name")]
	public class DemoApplication : Application
	{
		protected DemoApplication (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}

		public override void OnCreate ()
		{
			base.OnCreate ();
			JPushInterface.SetDebugMode (true);
			JPushInterface.Init (this.ApplicationContext);           
		}
	}
}