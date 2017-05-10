using Android.Content;
using Android.Net;
using System;
using BabyBus.Logic.Shared;


namespace BabyBus.Droid
{
	public class ConnectionChangeReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent) {
			ConnectivityManager manager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
			var gprs = manager.GetNetworkInfo(ConnectivityType.Mobile).GetState();
			var wifi = manager.GetNetworkInfo(ConnectivityType.Wifi).GetState();
			if (!(gprs == NetworkInfo.State.Connected) && !(wifi == NetworkInfo.State.Connected)) {
				BabyBusContext.IsAvailable = false;
			} else {
				BabyBusContext.IsAvailable = true; 
			}
			if (ReachabilityChanged != null) {
				ReachabilityChanged.Invoke(null, null);
			}
				
		}

		public static event EventHandler ReachabilityChanged;

	}
}

