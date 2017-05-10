using AlipayApp;
using Android.App;
using Android.OS;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Com.Alipay.Sdk.App;
using Android.Support.V4.App;
using AliPayTest;

namespace AlipayApp.AlipayAPI
{
    public class ExternalFragment : Android.Support.V4.App.Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.pay_external, container, false);
        }
    }
}
