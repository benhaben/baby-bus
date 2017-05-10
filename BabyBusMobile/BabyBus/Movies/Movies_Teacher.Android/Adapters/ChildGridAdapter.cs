using System.IO;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Java.Lang;
using System.Collections.Generic;
using System;
using Object = Java.Lang.Object;
using BabyBus.Logic.Shared;
using Com.Squareup.Picasso;


namespace BabyBus.Droid.Adapters
{
	public class ChildGridAdapter : BaseAdapter
	{
		private readonly MvxActivity activity;
		public List<ChildModel> list;
		private IPictureService pic;
		private readonly Handler handler;
		private ChildModel child;


		public string PicassoTag{ get { return "ChildGridAdapterTag"; } }

		public ChildGridAdapter(MvxActivity activity, List<ChildModel> list)
		{
			this.list = list;
			this.activity = activity;
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
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_Attence_Child, parent, false);
				holder = new ViewHolder();
				holder.Name = view.FindViewById<TextView>(Resource.Id.child_name);
				holder.HeadImage = view.FindViewById<ImageView>(Resource.Id.head_image);
				holder.SelectedImage = view.FindViewById<ImageView>(Resource.Id.signed);
				holder.SelectedText = view.FindViewById<TextView>(Resource.Id.item_grid_image_text);
				holder.AskforleaveImage = view.FindViewById<ImageView>(Resource.Id.isaskforleave);

				holder.SelectedImage.SetImageResource(Resource.Drawable.icon_sign);
				holder.AskforleaveImage.SetImageResource(Resource.Drawable.icon_isaskforleave);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}

			child = list[position];
			holder.Name.Text = child.ChildName;

			holder.SelectedImage.Visibility = child.IsSelect ? ViewStates.Visible : ViewStates.Gone;
			holder.AskforleaveImage.Visibility = child.IsAskForLeave ? ViewStates.Visible : ViewStates.Gone;
			//Head Image
			if (!string.IsNullOrEmpty(child.ImageName)) {
				Picasso.With(activity).Load(Constants.ThumbServerPath + child.ImageName).Placeholder(Resource.Drawable.baby_icon).CenterCrop()
					.Resize(68, 68).Tag(PicassoTag).Into(holder.HeadImage);
			} else {
				handler.Post(() => holder.HeadImage.SetImageResource(Resource.Drawable.baby_icon));
			}
			return view;
		}

		public class ViewHolder : Object
		{
			public TextView Name { get; set; }

			public ImageView HeadImage { get; set; }

			public ImageView SelectedImage { get; set; }

			public ImageView AskforleaveImage { get; set; }

			public TextView SelectedText { get; set; }
		}
	}
}