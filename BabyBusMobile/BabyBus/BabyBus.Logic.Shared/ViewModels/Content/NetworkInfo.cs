


using BabyBus.Models.Attendance;
using BabyBus.ViewModels.Attendance;
using System;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.CrossCore;
using BabyBus.Message;

[Activity(Theme = "@style/CustomTheme")]
namespace BabyBus.Logic.Shared
{
	public class NetworkInfo 
	{
		public NetworkInfo ()
		{
			
		}
		public void GetNetworkstate()
		{
			ConnectivityManager conMan = (ConnectivityManager)GetSystemService (Context.ConnectivityService);
			State mobile = conMan.GetNetworkInfo (ConnectivityType.Mobile).GetState ();
			State wifi = conMan.GetAllNetworkInfo (ConnectivityType.Wifi);
		}
	}
}

