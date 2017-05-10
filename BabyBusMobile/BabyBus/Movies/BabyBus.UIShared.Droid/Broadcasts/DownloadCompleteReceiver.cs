using Android.App;
using Android.Content;
using Java.IO;

namespace BabyBus.Droid.Broadcast
{
	public class DownloadCompleteReceiver : BroadcastReceiver
	{

		public override void OnReceive(Context context, Intent intent)
		{
			if (intent.Action.Equals(DownloadManager.ActionDownloadComplete)) {
				var downloadManager = (DownloadManager)context.GetSystemService(Context.DownloadService);
				var query = new DownloadManager.Query();
				query.SetFilterByStatus(DownloadStatus.Successful);
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