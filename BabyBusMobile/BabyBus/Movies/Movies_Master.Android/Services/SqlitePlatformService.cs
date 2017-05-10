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
using CoolBeans.Services;
//using SQLite.Net.Interop;
//using SQLite.Net;
//using SQLite.Net.Platform.XamarinAndroid;

using Cirrious.MvvmCross.Plugins.Sqlite;

namespace CoolBeans.Droid.Services
{
    public class SqlitePlatformService : ISqlitePlatformService
    {
//        private SQLitePlatformAndroid _sQLitePlatformAndroid;
        private readonly object _lockObject = new object();


        public ISQLitePlatform GetLitePlatform()
        {
            //performance 
            if (_sQLitePlatformAndroid == null)
            {
                lock (_lockObject)
                {
                    if (_sQLitePlatformAndroid == null)
                    {
                        _sQLitePlatformAndroid = new SQLitePlatformAndroid();
                    }
                }
            }
            return _sQLitePlatformAndroid;
        }
    }
}