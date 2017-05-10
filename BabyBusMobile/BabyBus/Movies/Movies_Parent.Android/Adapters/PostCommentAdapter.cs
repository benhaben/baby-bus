using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using BabyBus.Logic.Shared;
using Object = Java.Lang.Object;
using System.IO;
using Android.Graphics;
using Cirrious.MvvmCross.Droid.Views;


namespace BabyBus.Droid
{
	public class PostCommentAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<ECPostComment> list;
		protected SettingCommentViewModel indexVM;

		public PostCommentAdapter(MvxActivity activity, List<ECPostComment> list)
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

			ViewHolder holder;
			View view;
			if (convertView == null) {

				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_Setting_ECPostComment, parent, false);
				holder = new ViewHolder();
				holder.Title = view.FindViewById<TextView>(Resource.Id.item_title);
				holder.Abstract = view.FindViewById<TextView>(Resource.Id.item_comment_content);
				holder.CommentUserName = view.FindViewById<TextView>(Resource.Id.comment_username);
				holder.CreateTiem = view.FindViewById<TextView>(Resource.Id.createtime);
				holder.CommentContent = view.FindViewById<TextView>(Resource.Id.comment_content);
				holder.ItemButton = view.FindViewById<LinearLayout>(Resource.Id.item_button);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = list[position];
			holder.Title.Text = item.Title;
			holder.Abstract.Text = item.Abstract;
			holder.CommentContent.Text = item.CommnetContent;
			holder.CommentUserName.Text = item.CommentUserName;
			holder.CreateTiem.Text = item.CommentCreateDate.ToString();

			holder.ItemButton.Tag = position;
			holder.ItemButton.Click -= ll_Click;
			holder.ItemButton.Click += ll_Click;
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

			public TextView Abstract{ get; set; }

			public TextView CommentUserName{ get; set; }

			public  TextView CreateTiem{ get; set; }

			public  TextView CommentContent{ get; set; }

			public LinearLayout ItemButton{ get; set; }
		}



	}
}



