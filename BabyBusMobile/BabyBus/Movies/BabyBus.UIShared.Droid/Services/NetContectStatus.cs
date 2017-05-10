
using System;
using Android.App;
using Android.Widget;
using BabyBus.Droid;
using Android.Views;
using Android.Content;
using Android.Net;

namespace BabyBus.Logic.Shared
{
	public class NetContectStatus
	{
		private static ConnectionChangeReceiver myReceiver = new ConnectionChangeReceiver();

		public static void NetStatus(Activity activity, LinearLayout nonet) {
			if (nonet != null) {
				nonet.Click += (object sender, EventArgs e) => {
					activity.StartActivity(new Intent(Android.Provider.Settings.ActionWirelessSettings));	
				};
			} else {
				return;
			}

		
			ConnectionChangeReceiver.ReachabilityChanged += (sender, e) => {
				nonet.Visibility = BabyBusContext.IsAvailable ? ViewStates.Gone : ViewStates.Visible;
			};
		}

		public static void  registerReceiver(Activity activity) {
			IntentFilter filter = new IntentFilter(ConnectivityManager.ConnectivityAction);
			activity.RegisterReceiver(myReceiver, filter);
		}

		public static void unregisterReceiver(Activity activity) {
			activity.UnregisterReceiver(myReceiver);
		}

	}
}

