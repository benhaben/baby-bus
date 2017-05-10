using System.Collections.Specialized;
using System.Windows.Input;
using Android.Views;
using Android.Widget;

namespace BabyBus.Droid
{
	class  BroadcastReceiver{

	   
	     public  bool NetContentStatus() {
	        ConnectivityManager manager = (ConnectivityManager)context.getSystemService(Context.CONNECTIVITY_SERVICE);
	        NetworkInfo gprs = manager.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);
	        NetworkInfo wifi = manager.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
	        if(!gprs.isConnected() && !wifi.isConnected())
	        {
	        	return false;
	        }  
	        else
	        {
	        	return true;
	        } 
	                    
	            
	  }
   }
}


