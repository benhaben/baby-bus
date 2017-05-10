using System;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System.Collections.Generic;
using Android.OS;
using Cirrious.CrossCore;
using Android.Views;
using System.IO;
using Android.Graphics;
using Android.Content;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid
{
	public class ReadListAdapter : BaseAdapter
	{
		private readonly MvxActivity _activity;
		public IList<ChildModel> _list;
		private IPictureService _pic;
		private Handler _handler;
		private NoticeType _noticeType;

		public  NoticeType  noticetype {
			get { 
				return _noticeType; 
			}
			set {
				_noticeType = value;
			}
		}

		public IList<ChildModel> List {
			get {
				return _list;
			}
			set {
				_list = value;
			}
		}


		public ReadListAdapter(MvxActivity activity, IList<ChildModel> list)
		{
			_activity = activity;
			_list = list;
			_pic = Mvx.Resolve<IPictureService>();
			_handler = new Handler();
		}

		public override Java.Lang.Object GetItem(int position) {
			return null;
		}

		public override long GetItemId(int position) {
			return 0;
		}

		public override View GetView(int position, Android.Views.View convertView, ViewGroup parent) {
			ViewHolder holder;
			View view;

			if (convertView == null) {
				view = _activity.LayoutInflater.Inflate(Resource.Layout.Item_Comm_ReadChild, parent, false);
				holder = new ViewHolder();
				holder.Name = view.FindViewById<TextView>(Resource.Id.child_name);
				holder.HeadImage = view.FindViewById<ImageView>(Resource.Id.head_image);
				holder.Readed = view.FindViewById<TextView>(Resource.Id.label_readed);
				holder.UnRead = view.FindViewById<TextView>(Resource.Id.label_unread);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var child = _list[position];
			holder.Name.Text = child.ChildName;
			if (child.IsRead) {
				holder.Readed.Visibility = ViewStates.Visible;
				holder.UnRead.Visibility = ViewStates.Gone;
			} else {
				holder.Readed.Visibility = ViewStates.Gone;
				holder.UnRead.Visibility = ViewStates.Visible;
			}
			if (!string.IsNullOrEmpty(child.ImageName)) {
				var fileName = child.ImageName + Constants.ThumbRule;
				_pic.LoadIamgeFromSource(fileName, stream => {
					var ms = stream as MemoryStream;
					if (ms != null) {
						var bytes = ms.ToArray();
						var options = new BitmapFactory.Options() { InPurgeable = true };
						var bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
						_handler.Post(() => holder.HeadImage.SetImageBitmap(bmp));
					} else {
						_handler.Post(() => holder.HeadImage.SetImageResource(Resource.Drawable.Child_headImage));
					}
				}, Constants.ThumbServerPath);
			} else {
				_handler.Post(() => holder.HeadImage.SetImageResource(Resource.Drawable.Child_headImage));
			}
			var ll = view.FindViewById<ImageView>(Resource.Id.image_icon_phone);
			ll.Tag = position;
			ll.Click -= ll_Click;
			ll.Click += ll_Click;
			//send message to parent
			var lm = view.FindViewById<ImageView>(Resource.Id.image_icon_massage);
			lm.Tag = position;
			lm.Click -= lm_Click;
			lm.Click += lm_Click;
			return view;
		}

		public override int Count {
			get { return _list.Count; }
		}

		protected void ll_Click(object sender, EventArgs e) {			
			var positon = (int)((ImageView)sender).Tag;
			var item = _list[positon];
			var tel = string.Format("tel:" + item.PhoneNumber);
			var uri = Android.Net.Uri.Parse(tel);
			var intent = new Intent(Intent.ActionCall, uri);
			this._activity.StartActivity(intent);
		}

		protected void lm_Click(object sender, EventArgs e) {			
			var positon = (int)((ImageView)sender).Tag;
			var item = _list[positon];
			var smsUri = Android.Net.Uri.Parse("smsto:" + item.PhoneNumber);
			var typename = noticeTypetostring(noticetype);
			var message = string.Format("尊敬的" + item.ChildName + "家长,我已经通过贝贝巴士软件将" + typename + "发到您的手机，请注意查收！为了宝宝的快乐成长，感谢您的支持。");
			Intent intent = new Intent(Intent.ActionSendto, smsUri);          
			intent.PutExtra("sms_body", message);          
			this._activity.StartActivity(intent);
		}

		private string noticeTypetostring(NoticeType type) {
			string typename;
			switch (type) {
				case NoticeType.ClassHomework:
					typename = "家庭作业";
					break;
				case NoticeType.ClassCommon:
					typename = "班级通知";
					break;
				case NoticeType.KindergartenStaff:
					typename = "园务通知";
					break;
				case NoticeType.KindergartenAll:
					typename = "园区通知";
					break;
				case NoticeType.KindergartenRecipe:
					typename = "宝宝食谱";
					break;
				default:
					typename = "消息通知";
					break;
			}
			return typename;
		}

		public class ViewHolder : Java.Lang.Object
		{
			public TextView Name { get; set; }

			public ImageView HeadImage{ get; set; }

			public TextView Readed { get; set; }

			public TextView UnRead { get; set; }
		}
	}
}

