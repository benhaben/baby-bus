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
using Java.IO;

namespace BabyBus.Droid.Broadcast
{
    public class DownloadCompleteReceiver : BroadcastReceiver
    {

        public override void OnReceive(Context context, Intent intent) {
            if (intent.Action.Equals(DownloadManager.ActionDownloadComplete)) {
                var downloadManager = (DownloadManager)context.GetSystemService(Context.DownloadService);
                var query = new DownloadManager.Query();
                query.SetFilterByStatus(Android.App.DownloadStatus.Successful);
                var cursor = downloadManager.InvokeQuery(query);
                string fileName = string.Empty;
                if (cursor.MoveToFirst()) {
                    fileName = cursor.GetString(cursor.GetColumnIndex(DownloadManager.ColumnLocalUri));

                }
                File f = new File(fileName.Replace("file://", ""));
                Utils.Utils.InstallApk(f, context);
            }
        }
    }
}