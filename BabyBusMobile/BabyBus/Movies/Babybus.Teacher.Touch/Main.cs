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

            Insights.Initialize("8a08208c57bff8ccecaf477ba6b9ff34bdcbb958");

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
