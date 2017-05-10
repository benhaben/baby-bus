using System;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using Android.Views;
using Object = Java.Lang.Object;


namespace BabyBus.Droid
{
	public class MITestMasterAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<MITestMaster> list;
		protected ParentModalityViewModel indexVM;

		public MITestMasterAdapter(MvxActivity activity, List<MITestMaster> list)
		{
			this.activity = activity;
			this.list = list;
		}

		public override int Count {
			get { return list.Count; }
		}

		public override Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int position)
		{
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			//throw new NotImplementedException ();
			ViewHolder holder;
			View view;
			if (convertView == null) {
				holder = new ViewHolder();
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_MI_Testmaster, parent, false);
				holder.ModalityImage = view.FindViewById<ImageView>(Resource.Id.modality_ImageName);
				holder.ModalityName = view.FindViewById<TextView>(Resource.Id.modality_name);
				holder.ModalityCompleteness = view.FindViewById<TextView>(Resource.Id.testcompleteness);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];
			holder.ModalityName.Text = item.ModalityName;
			holder.ModalityCompleteness.Text = string.Format("({0}/{1})", item.CompletedTest, item.TotalTest);
			holder.ModalityImage.SetImageResource(item.ModalityImageId);
			var ll = view.FindViewById<LinearLayout>(Resource.Id.Item_MI_testmaster);
			ll.Tag = position;
			ll.Click -= ll_Click;
			ll.Click += ll_Click;


			return view;
		}

		protected void ll_Click(object sender, EventArgs e)
		{
//			if (BabyBusContext.IsMember) {
//				indexVM = indexVM ?? ((ParentModalityView)activity).ViewModel;
//				var positon = (int)((LinearLayout)sender).Tag;
//				var item = list[positon];
//				indexVM.CurrentModalityId = item.ModalityId;
//				indexVM.TestMasterId = item.TestMasterId;
//				indexVM.ShowTestDetailCommand.Execute();
//			} else {
//				var intent = new Intent(activity, typeof(PaymentView));
//				activity.StartActivity(intent);
//			}
			indexVM = indexVM ?? ((ParentModalityView)activity).ViewModel;
			var positon = (int)((LinearLayout)sender).Tag;
			var item = list[positon];
			indexVM.CurrentModalityId = item.ModalityId;
			indexVM.TestMasterId = item.TestMasterId;
			indexVM.ShowTestDetailCommand.Execute();

		}

		protected class ViewHolder : Object
		{
		
			public ImageView ModalityImage{ get; set; }

			public TextView ModalityName{ get; set; }

			public TextView ModalityCompleteness { get; set; }

		}
	}
}


