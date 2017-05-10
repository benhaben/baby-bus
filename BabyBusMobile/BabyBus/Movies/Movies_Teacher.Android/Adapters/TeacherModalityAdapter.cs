
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using Android.Views;
using Object = Java.Lang.Object;
using Android.OS;


namespace BabyBus.Droid
{
	public class TeacherModalityAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<MIModality> list;
		protected MITestView indexVM;
		private Handler handler;

		public TeacherModalityAdapter(MvxActivity activity, List<MIModality> list)
		{
			this.activity = activity;
			this.list = list;
			handler = new Handler();
		}

		public override int Count {
			get { return list.Count; }
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

				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_Modality, parent, false);
				holder = new ViewHolder();
				holder.ModalityImage = view.FindViewById<ImageView>(Resource.Id.Modality_Image);
				holder.ModalityName = view.FindViewById<TextView>(Resource.Id.Modality_Text);
				holder.Completeness = view.FindViewById<TextView>(Resource.Id.completeness);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];
			holder.ModalityName.Text = item.ModalityName;
			holder.ModalityImage.SetImageResource(item.ModalityImageId);
			holder.Completeness.Text = string.Format("{0}/{1}", item.Completed, item.Total);


			return view;
		}

		protected class ViewHolder : Object
		{

			public TextView ModalityName{ get; set; }

			public ImageView ModalityImage{ get; set; }

			public TextView  Completeness{ get; set; }

		}
	}
}


