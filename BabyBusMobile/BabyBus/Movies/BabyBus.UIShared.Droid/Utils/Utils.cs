using System;
using System.Collections.ObjectModel;
using Android.Content;
using CN.Jpush.Android.Api;
using Java.IO;
using BabyBus.Logic.Shared;
using Android.Widget;
using Android.Views;

namespace BabyBus.Droid.Utils
{
	public class Utils
	{

		public static string DateTimeString(DateTime datetime) {
			var timeSpan = DateTime.Now - datetime;
			if (timeSpan.Days <= 1) {
				return string.Format("今天 {0}", datetime.ToString("HH:mm"));
			}
			if (timeSpan.Days <= 2) {
				return string.Format("昨天 {0}", datetime.ToString("HH:mm"));
			}
			return datetime.ToString("yyyy年M月d日 HH:mm");
		}

		public static void SetPushTags(Context context) {
			var tags = new Collection<string>();
			tags.Add(string.Format("Kindergarten_{0}", BabyBusContext.KindergartenId));
			if (BabyBusContext.ClassId != 0)
				tags.Add(string.Format("Class_{0}", BabyBusContext.ClassId));

			JPushInterface.SetTags(context, tags, null);
		}

		public static void ClearTags(Context context) {
			JPushInterface.SetTags(context, new Collection<string> { }, null);
		}

		public static void SetPushAlias(Context context) {
			JPushInterface.SetAlias(context, "User_" + BabyBusContext.UserAllInfo.UserId, null);
		}

		public static void ClearAlias(Context context) {
			JPushInterface.SetAlias(context, "User_null", null);
		}

		public static long CurrentMillis {
			get {
				var ts = new TimeSpan(System.DateTime.UtcNow.Ticks - new DateTime(2010, 1, 1, 0, 0, 0).Ticks);
				return (long)ts.TotalMilliseconds;  
			}
		}

		public static void GetDensityDpi() {
            
		}

		public static void InstallApk(File f, Context context) {
			if (f != null) {
				Intent intent = new Intent(Intent.ActionView);

				intent.AddFlags(ActivityFlags.NewTask);
				intent.SetDataAndType(Android.Net.Uri.FromFile(f), "application/vnd.android.package-archive");
				context.StartActivity(intent);
			}
		}

		public static void SetListViewHeightBasedOnChildren(ListView listView) {
			var listAdapter = listView.Adapter;
			if (listAdapter == null) {
				// pre-condition
				return;
			}

			int totalHeight = 0;
			for (int i = 0; i < listAdapter.Count; i++) {
				var listItem = listAdapter.GetView(i, null, listView);
				// listItem.measure(0, 0);
				listItem.Measure(
					View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
					View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
				totalHeight += listItem.MeasuredHeight;
			}


			ViewGroup.LayoutParams test = listView.LayoutParameters;
			test.Height = totalHeight
			+ (listView.DividerHeight * (listAdapter.Count - 1));
			listView.LayoutParameters = test;
		}

		#region Weixin

		#endregion
	}
}
