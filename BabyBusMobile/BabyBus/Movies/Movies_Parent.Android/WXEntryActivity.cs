
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
using Com.Tencent.MM.Sdk.Openapi;
using BabyBus.Droid;

namespace BabyBus.Droid
{
	[Activity(Name = "com.rnt.babybus.parent.wxapi.WXEntryActivity", Label = "WXEntryActivity", Exported = true)]
	[IntentFilter(new string[]{ "com.rnt.babybus.parent.wxapi.WXEntryActivity" }, Categories = new string[]{ "com.rnt.babybus.parent" })]
	public class WXEntryActivity : Activity,IWXAPIEventHandler
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Create your application here
		}

		public void OnReq(Com.Tencent.MM.Sdk.Modelbase.BaseReq p0)
		{
			throw new NotImplementedException();
		}

		public void OnResp(Com.Tencent.MM.Sdk.Modelbase.BaseResp p0)
		{
			throw new NotImplementedException();
		}
	}
}

