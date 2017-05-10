using System;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using Android.Views;
using Object = Java.Lang.Object;
using Cirrious.CrossCore;
using System.IO;
using Android.Graphics;
using Android.OS;


namespace BabyBus.Droid
{
	public class MISelectTestChildAdpter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<MITestMaster> list;
		protected MIChildrenViewModel indexVM;
		private IPictureService pic;
		private Handler handler;


		public MISelectTestChildAdpter(MvxActivity activity, List<MITestMaster> list)
		{
			this.activity = activity;
			this.list = list;
			pic = Mvx.Resolve<IPictureService>();
			handler = new Handler();
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

			ViewHolder holder;
			View view;
			if (convertView == null) {

				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_SelectTestchild, parent, false);
				holder = new ViewHolder();
				holder.ImageName = view.FindViewById<ImageView>(Resource.Id.child_Image);
				holder.Completeness = view.FindViewById<TextView>(Resource.Id.completeness);
				holder.ChildName = view.FindViewById<TextView>(Resource.Id.child_Name);
				holder.NonPaying = view.FindViewById<TextView>(Resource.Id.non_paying);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];

			holder.ChildName.Text = item.ChildName;

			if (item.CompletedTest != item.TotalTest) {
				holder.Completeness.Text = string.Format("{0}/{1}", item.CompletedTest, item.TotalTest);
			} else {
				holder.Completeness.Text = "完成";
			}
			if (item.IsMember) {
				holder.NonPaying.SetBackgroundColor(BabyBus.Logic.Shared.Color.Green);
				holder.NonPaying.Text = "已付费";
			} else {
				holder.NonPaying.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFRed1);
				holder.NonPaying.Text = "未付费";
			}

			if (!string.IsNullOrEmpty(item.ImageName)) {
				var fileName = item.ImageName + Constants.ThumbRule;
				pic.LoadIamgeFromSource(fileName, stream => {
					var ms = stream as MemoryStream;
					if (ms != null) {
						var bytes = ms.ToArray();
						var options = new BitmapFactory.Options() { InPurgeable = true };
						var bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
						handler.Post(() => holder.ImageName.SetImageBitmap(bmp));
					} else {
						handler.Post(() => holder.ImageName.SetImageResource(Resource.Drawable.Child_headImage));
					}
				}, Constants.ThumbServerPath);
			} else {
				handler.Post(() => holder.ImageName.SetImageResource(Resource.Drawable.Child_headImage));
			}
			var ll = view.FindViewById<LinearLayout>(Resource.Id.Item_SelectTestChild);
			ll.Tag = position;
			ll.Click -= ll_Click;
			ll.Click += ll_Click;


			return view;
		}

		protected void ll_Click(object sender, EventArgs e)
		{
			indexVM = indexVM ?? ((SelectTestChildView)activity).ViewModel;
			var positon = (int)((LinearLayout)sender).Tag;
			var item = list[positon];
			indexVM.ShowDetailCommand(item.ModalityId, item.TestMasterId, item.ChildId);
		}

		protected class ViewHolder : Object
		{

			public ImageView ImageName{ get; set; }

			public TextView Completeness{ get; set; }

			public TextView ChildName { get; set; }

			public TextView NonPaying{ get; set; }

		}
	}
}


