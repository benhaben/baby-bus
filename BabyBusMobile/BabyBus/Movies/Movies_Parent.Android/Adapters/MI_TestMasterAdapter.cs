using System;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using BabyBus.Models.Communication;
using BabyBus.ViewModels.Communication;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using Android.Views;
using BabyBus.Droid.Adapters;
using Object = Java.Lang.Object;
using Cirrious.CrossCore;
using BabyBus.Services;
using System.IO;
using Android.Graphics;
using Java.IO;
using BabyBus.ViewModels.MultipleIntelligence;


namespace BabyBus.Droid
{
	public class MI_TestMasterAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<MI_TestMasterModel> list;
		protected ParentModalityViewModel indexVM;
		//private IPictureService pic;
		public MI_TestMasterAdapter (MvxActivity activity, List<MI_TestMasterModel> list)
		{
			this.activity = activity;
			this.list = list;
			//pic = Mvx.Resolve<IPictureService>();
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

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			
			ViewHolder holder;
			View view;
			if (convertView == null) {
				
				view = activity.LayoutInflater.Inflate (Resource.Layout.Item_MI_Testmaster, parent, false);
				holder = new ViewHolder ();
				holder.AssessIndexImage = view.FindViewById<ImageView> (Resource.Id.AssessIndex_ImageName);
				holder.AssessIndexName = view.FindViewById<TextView> (Resource.Id.AssessIndex_name);
				holder.AssessIndexCompleteness = view.FindViewById<TextView> (Resource.Id.testcompleteness);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];
			holder.AssessIndexName.Text = item.Name;
			holder.AssessIndexCompleteness.Text = string.Format("({0}/{1})",item.CompletedTest,item.TotalTest);
			holder.AssessIndexImage.SetImageResource (item.ModalityImageId);
			/*if (item.ImageName != null) {
				pic.LoadIamgeFromSource (item.ImageName, stream => {
					var ms = stream as MemoryStream;
					if (ms != null) {
						var bytes = ms.ToArray ();
						var options = new BitmapFactory.Options () { InPurgeable = true };
						var bmp = BitmapFactory.DecodeByteArray (bytes, 0, bytes.Length, options);
						holder.AssessIndexImage.SetImageBitmap (bmp);

					}
				}, Constants.PNGSuffix);
			}*/
			var ll = view.FindViewById<LinearLayout>(Resource.Id.Item_MI_testmaster);
			ll.Tag = position;
			ll.Click -= ll_Click;
			ll.Click += ll_Click;


			return view;
		}
		protected void ll_Click(object sender, EventArgs e) {
			indexVM = indexVM ?? ((ParentModalityView)activity).ViewModel;
			var positon = (int)((LinearLayout)sender).Tag;
			var item = list[positon];
			indexVM.ShowDetailCommand(item.ModalityId);
		}
		protected class ViewHolder : Object
		{
		
			public ImageView AssessIndexImage{ get; set; }

			public TextView AssessIndexName{ get; set; }

			public TextView AssessIndexCompleteness { get; set; }

		}
	}
}


