using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin;



namespace BabyBus.iOS {
    public class Application {
        // This is the main entry point of the application.
        static void Main(string[] args) {

            Insights.Initialize("6f47e6fabd50994b2908fecc33c6b64f4f965f89");

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
