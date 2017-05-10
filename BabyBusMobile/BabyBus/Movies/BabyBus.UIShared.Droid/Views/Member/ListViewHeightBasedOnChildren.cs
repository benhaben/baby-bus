using System;
using Android.Widget;
using Android.Views;

namespace BabyBus.Droid.Views.Member
{
	public static class ListViewHeightBasedOnChildren
	{
		public static void SetListView(ListView listView)
		{
			var listAdapter = listView.Adapter;
			if (listAdapter == null) {
				return;
			}

			int totalHeight = 0;
			for (int i = 0; i < listAdapter.Count; i++) {
				var listItem = listAdapter.GetView(i, null, listView);
				// listItem.measure(0, 0);
				listItem.Measure(
					View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
					View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
				totalHeight += (listItem.MeasuredHeight + 10);
			}	


			ViewGroup.LayoutParams test = listView.LayoutParameters;
			test.Height = totalHeight
			+ (listView.DividerHeight * (listAdapter.Count - 1));
			listView.LayoutParameters = test;
		}
	}
}

