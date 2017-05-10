using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using BabyBus.Logic.Shared;
using Object = Java.Lang.Object;
using Cirrious.MvvmCross.Droid.Views;
using Com.Squareup.Picasso;


namespace BabyBus.Droid
{
	public class PostAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<ECPostInfo> list;
		protected SettingCommentViewModel indexVM;

		public string PicassoTag{ get; private set; }

		public PostAdapter(MvxActivity activity, List<ECPostInfo> list)
		{
			this.activity = activity;
			this.list = list;
			PicassoTag = "PostAdapterTag";
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
			var ischanged = true;
			var item = list[position];
			if (convertView == null) {

				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_EC_Forum, parent, false);
				holder = new ViewHolder();
				holder.ImagePost = view.FindViewById<ImageView>(Resource.Id.image_post);
				holder.CreateTime = view.FindViewById<TextView>(Resource.Id.createtime);
				holder.Comment = view.FindViewById<TextView>(Resource.Id.forum_comment);
				holder.Favarite = view.FindViewById<TextView>(Resource.Id.forum_favorite);
				holder.Abstract = view.FindViewById<TextView>(Resource.Id.post_abstract);
				holder.Title = view.FindViewById<TextView>(Resource.Id.post_title);
				holder.PostInfoId = item.PostInfoId;
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
				if (holder.PostInfoId != item.PostInfoId) {
					holder.PostInfoId = item.PostInfoId;
				} else {
					ischanged = false;
				}
			}

			if (ischanged) {
				holder.CreateTime.Text = BabyBus.Logic.Shared.Utils.DateTimeString(item.CreateDate);
				holder.Comment.Text = item.CommentCount.ToString();
				holder.Favarite.Text = item.PraiseCount.ToString();
				holder.Abstract.Text = item.DisplayAbstract;
				holder.Title.Text = item.Title;
				if (!string.IsNullOrEmpty(item.FirstImage)) {
					Picasso.With(activity).Load(Constants.ThumbServerPath + item.FirstImage).Tag(PicassoTag).CenterCrop()
						.Resize(80, 80).Placeholder(Resource.Drawable.icon_course).Into(holder.ImagePost);
				} else {
					holder.ImagePost.Visibility = ViewStates.Gone;
				}
				var ll = view.FindViewById<LinearLayout>(Resource.Id.item_forum_show);
				ll.Tag = position;
				ll.Click -= ll_Click;
				ll.Click += ll_Click;
			}
			return view;
		}

		protected void ll_Click(object sender, EventArgs e)
		{
			indexVM = indexVM ?? ((SettingCommentView)activity).ViewModel;
			var positon = (int)((LinearLayout)sender).Tag;
			var item = list[positon];
			indexVM.ShowdationDetailViewModel(item.PostInfoId);
		}

		protected class ViewHolder : Object
		{
			public TextView Title{ get; set; }

			public int PostInfoId{ get; set; }

			public ImageView ImagePost{ get; set; }

			public TextView UserName{ get; set; }

			public TextView CreateTime{ get; set; }

			public TextView Favarite{ get; set; }

			public TextView Comment{ get; set; }

			public  TextView Abstract{ get; set; }

		}



	}
}



