using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;
using Cirrious.MvvmCross.Droid.Fragging;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Adapters
{
	public class AttMasterListAdapter:BaseAdapter
	{
		private readonly MvxFragmentActivity activity;
		public List<AttendanceMasterModel> list;
		private Handler handler;

		public AttMasterListAdapter(MvxFragmentActivity activity, List<AttendanceMasterModel> list)
		{
			this.activity = activity;
			this.list = list;
		}

		public override Object GetItem(int position) {
			return null;
		}

		public override long GetItemId(int position) {
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder;
			View view;
			if (convertView == null) {
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_Attence_Master, parent, false);
				holder = new ViewHolder();
				holder.LayoutUnattence = view.FindViewById<LinearLayout>(Resource.Id.layout_unattence);
				holder.LayoutAttence = view.FindViewById<LinearLayout>(Resource.Id.layout_attence);
				holder.ClassName1 = view.FindViewById<TextView>(Resource.Id.text_unattence_classname);
				holder.Total1 = view.FindViewById<TextView>(Resource.Id.text_unattence_total);
				holder.ClassName2 = view.FindViewById<TextView>(Resource.Id.text_attence_classname);
				holder.Total2 = view.FindViewById<TextView>(Resource.Id.text_attence_total);
				holder.Attence = view.FindViewById<TextView>(Resource.Id.text_attence_attence);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];
			holder.LayoutAttence.Visibility = !item.IsAttence ? ViewStates.Gone : ViewStates.Visible;
			holder.LayoutUnattence.Visibility = item.IsAttence ? ViewStates.Gone : ViewStates.Visible;
			holder.ClassName1.Text = item.ClassName;
			holder.Total1.Text = item.Total.ToString();
			holder.ClassName2.Text = item.ClassName;
			holder.Total2.Text = item.Attence.ToString();
			holder.Attence.Text = item.UnAttence.ToString();

			return view;
		}

		public override int Count {
			get { return list.Count; }
		}

		public class ViewHolder : Object
		{
			public LinearLayout LayoutAttence { get; set; }

			public LinearLayout LayoutUnattence { get; set; }

			public TextView ClassName1 { get; set; }

			public TextView ClassName2 { get; set; }

			public TextView Total1 { get; set; }

			public TextView Total2 { get; set; }

			public TextView Attence { get; set; }
		}
	}
}