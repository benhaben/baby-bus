using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper.Mappers;

namespace AdministratorManagement.Controllers
{
    //TODO: i don't not how to write destructor in the C#, IDispose?
//    public class BabybusStopWatch
//    {
//        Stopwatch _stopWatch = new Stopwatch();
//        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(BabybusStopWatch));
//        private string _name;
//        public BabybusStopWatch(string name)
//        {
//            _name = name;
//            StartWatch();
//        }
//        
//        protected void StartWatch()
//        {
//            _stopWatch.Start();
//        }
//
//        protected void EndWatch()
//        {
//            _stopWatch.Stop();
//            // Get the elapsed time as a TimeSpan value.
//            TimeSpan ts = _stopWatch.Elapsed;
//
//            // Format and display the TimeSpan value.
//            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
//                ts.Hours, ts.Minutes, ts.Seconds,
//                ts.Milliseconds / 10);
//            Log.Info("RunTime " + elapsedTime);
//            Console.WriteLine("RunTime " + elapsedTime);
//        }
//    }
    public class BabyBusApiController : ApiController
    {
         Stopwatch _stopWatch = new Stopwatch();
         private string _stopWatchName;
        protected void StartWatch(string name = "")
        {
            _stopWatchName = name;
            _stopWatch.Start();
        }

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(BabyBusApiController));
        
        protected void EndWatch()
        {
            _stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = _stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Log.Info(_stopWatchName + "RunTime " + elapsedTime);
            Console.WriteLine("RunTime " + elapsedTime);
        }

    }
}