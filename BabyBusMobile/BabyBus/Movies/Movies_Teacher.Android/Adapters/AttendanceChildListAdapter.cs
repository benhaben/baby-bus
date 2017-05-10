using System;
using System.IO;

using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Object = Java.Lang.Object;
using System.Collections.Generic;
using BabyBus.Logic.Shared;
using Uri = Android.Net.Uri;
using Cirrious.MvvmCross.Droid.Fragging;

namespace BabyBus.Droid
{
	public class AttendanceChildListAdapter :BaseAdapter
	{
		private readonly MvxFragmentActivity activity;
		public IList<ChildModel> list;
		private IPictureService pic;
		private Handler handler;
		protected AttendanceMasterViewModel indexVM;

		public AttendanceChildListAdapter(MvxFragmentActivity activity, IList<ChildModel> list)
		{
			this.activity = activity;
			this.list = list;
			pic = Mvx.Resolve<IPictureService>();
			handler = new Handler();
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
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_Attence_ChildList, parent, false);
				holder = new ViewHolder();
				holder.Name = view.FindViewById<TextView>(Resource.Id.child_name);
				holder.HeadImage = view.FindViewById<ImageView>(Resource.Id.head_image);
				holder.IsaskForLeave = view.FindViewById<TextView>(Resource.Id.text_icon_askforleave);
				holder.IsUnattendance = view.FindViewById<TextView>(Resource.Id.text_icon_unattence);

				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			ChildModel child = list[position];
			//Child Name
			holder.Name.Text = child.ChildName;
			//child Absence reason
			if (child.IsAskForLeave) {
				holder.IsaskForLeave.Visibility = ViewStates.Visible;
				holder.IsUnattendance.Visibility = ViewStates.Gone;
			} else {
				holder.IsaskForLeave.Visibility = ViewStates.Gone;
				holder.IsUnattendance.Visibility = ViewStates.Visible;
			}
			//call praent
			var ll = view.FindViewById<ImageView>(Resource.Id.image_icon_phone);
			ll.Tag = position;
			ll.Click -= ll_Click;
			ll.Click += ll_Click;
			//send message to parent
			var lm = view.FindViewById<ImageView>(Resource.Id.image_icon_massage);
			lm.Tag = position;
			lm.Click -= lm_Click;
			lm.Click += lm_Click;

			//Child image
			if (!string.IsNullOrEmpty(child.ImageName)) {
				var fileName = child.ImageName + Constants.ThumbRule;
				pic.LoadIamgeFromSource(fileName, stream => {
					var ms = stream as MemoryStream;
					if (ms != null) {
						var bytes = ms.ToArray();
						var options = new BitmapFactory.Options() { InPurgeable = true };
						var bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
						handler.Post(() => holder.HeadImage.SetImageBitmap(bmp));
					} else {
						handler.Post(() => holder.HeadImage.SetImageResource(Resource.Drawable.Child_headImage));
					}
				}, Constants.ThumbServerPath);
			} else {
				handler.Post(() => holder.HeadImage.SetImageResource(Resource.Drawable.Child_headImage));
			}
			return view;
		}

		public override int Count {
			get { return list.Count; }
		}

		protected void ll_Click(object sender, EventArgs e) {			
			var positon = (int)((ImageView)sender).Tag;
			var item = list[positon];
			var tel = string.Format("tel:" + item.PhoneNumber);
			Uri uri = Uri.Parse(tel);
			var intent = new Intent(Intent.ActionCall, uri);
			this.activity.StartActivity(intent);
		}

		protected void lm_Click(object sender, EventArgs e) {	
			var positon = (int)((ImageView)sender).Tag;
			var item = list[positon];
			var smsUri = Android.Net.Uri.Parse("smsto:" + item.PhoneNumber);
			var message = item.IsAskForLeave ? string.Format("尊敬的" + item.ChildName + "家长:" + item.ChildName + " 请假的消息我们已经收到。希望宝宝能尽早重返校园，和小伙伴们一起快乐地玩耍！") 
				: string.Format("您家宝宝" + item.ChildName + "今天没有到园，请问是什么原因呢？");
			Intent intent = new Intent(Intent.ActionSendto, smsUri);          
			intent.PutExtra("sms_body", message);          
			this.activity.StartActivity(intent);
		}

		public class ViewHolder : Object
		{
			public TextView Name { get; set; }

			public ImageView HeadImage { get; set; }

			public TextView IsaskForLeave{ get; set; }

			public TextView IsUnattendance{ get; set; }

			public ImageView Message{ get; set; }

			public ImageView Phone{ get; set; }
		}
	}
}
