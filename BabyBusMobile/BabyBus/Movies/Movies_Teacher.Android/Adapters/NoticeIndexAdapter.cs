using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Utils;
using Cirrious.MvvmCross.Droid.Views;
using Xamarin;
using Object = Java.Lang.Object;
using BabyBus.Droid.Views.Communication;
using Cirrious.CrossCore;
using Android.Graphics;
using System.IO;
using Android.OS;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;


namespace BabyBus.Droid.Adapters
{
	public abstract class BaseNoticeIndexAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<NoticeModel> list;
		private Handler handler;


		public BaseNoticeIndexAdapter(MvxActivity activity, List<NoticeModel> list)
		{
			this.activity = activity;
			this.list = list;
			handler = new Handler();
		}

		public override Object GetItem(int position) {
			return null;
		}

		public override long GetItemId(int position) {
			return 0;
		}

        

		public override int Count {
			get { return list.Count; }
		}



		protected class ViewHolder : Object
		{
			public long NoticeId{ get; set; }

			public TextView NoticeTypeLabel{ get; set; }

			public TextView Title { get; set; }

			public TextView Content { get; set; }

			public TextView RealName { get; set; }

			public TextView CreateTime { get; set; }

			public GridView ImageGrid { get; set; }

			public TextView ReadedType{ get; set; }

			public ImageView HeadImage{ get; set; }
		}
	}

	public class NoticeIndexAdapter : BaseNoticeIndexAdapter
	{
		
		protected NoticeIndexViewModel indexVM;

		public NoticeIndexAdapter(MvxActivity activity, List<NoticeModel> list)
			: base(activity, list)
		{
			
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder;
			View view = null;
			try {
				if (convertView == null) {
					view = activity.LayoutInflater.Inflate(Resource.Layout.Item_Notice, parent, false);
					holder = new ViewHolder();
					holder.NoticeTypeLabel = view.FindViewById<TextView>(Resource.Id.label_notice_type);
					holder.Title = view.FindViewById<TextView>(Resource.Id.item_notice_title);
					holder.Content = view.FindViewById<TextView>(Resource.Id.item_notice_content);
					holder.RealName = view.FindViewById<TextView>(Resource.Id.item_notice_realname);
					holder.CreateTime = view.FindViewById<TextView>(Resource.Id.item_notice_createtime);
					holder.ImageGrid = view.FindViewById<GridView>(Resource.Id.grid_memory_image);
					holder.ReadedType = view.FindViewById<TextView>(Resource.Id.label_readtype);
					view.Tag = holder;
				} else {
					view = convertView;
					holder = (ViewHolder)view.Tag;
				}
				var item = list[position];
				holder.Title.Text = item.Title;
				holder.Content.Text = item.Abstract;
				holder.RealName.Text = item.RealName;
				holder.CreateTime.Text = BabyBus.Logic.Shared.Utils.DateTimeString(item.CreateTime);
				//Notice Type Label
				switch (item.NoticeType) {
					case NoticeType.ClassCommon:
						holder.NoticeTypeLabel.SetText(Resource.String.comm_label_notice);
						holder.NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFBlue1);
						SetClassNoticeLabel(holder, item);
						break;
					case NoticeType.ClassHomework:
						holder.NoticeTypeLabel.SetText(Resource.String.comm_label_homework);
						holder.NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFPurple1);
						SetClassNoticeLabel(holder, item);
						break;
					case NoticeType.KindergartenAll:
						holder.NoticeTypeLabel.SetText(Resource.String.comm_label_kindergartenall);
						holder.NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFOrange1);
						SetKindergartenNoticeLabel(holder, item);
						break;
					case NoticeType.KindergartenRecipe:
						holder.NoticeTypeLabel.SetText(Resource.String.comm_label_kindergartenrecipe);
						holder.NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFGreen1);
						SetKindergartenNoticeLabel(holder, item);
						break;
					case NoticeType.KindergartenStaff:
						holder.NoticeTypeLabel.SetText(Resource.String.comm_label_kindergartenstaff);
						holder.NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFLake1);
						SetKindergartenNoticeLabel(holder, item);
						break;
					case NoticeType.BabyBusNotice:
						holder.NoticeTypeLabel.SetText(Resource.String.ybtabloid);
						holder.NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.Gray);
						SetKindergartenNoticeLabel(holder, item);
						break;
					default :
						break;
				}

				var ll = view.FindViewById<LinearLayout>(Resource.Id.item_notice_show);
				ll.Tag = position;
				ll.Click -= ll_Click;
				ll.Click += ll_Click;
			} catch (Exception ex) {
				Insights.Report(ex);
			}
			return view;
		}

		private void SetClassNoticeLabel(ViewHolder holder, NoticeModel item) {
			if (BabyBusContext.RoleType == RoleType.Parent) {
				holder.ReadedType.Text = "未读";
				holder.ReadedType.Visibility = item.IsReaded ? ViewStates.Gone : ViewStates.Visible;
			} else {
				holder.ReadedType.Text = string.Empty;
				holder.ReadedType.Visibility = ViewStates.Gone;
				//holder.ReadedType.Text = string.Format("已读人数：{0}/{1}", item.ClassCount, item.ReadedCount);
			}
		}

		private void SetKindergartenNoticeLabel(ViewHolder holder, NoticeModel item) {
			if (BabyBusContext.RoleType == RoleType.Parent) {
				holder.ReadedType.Text = "未读";
				holder.ReadedType.Visibility = item.IsReaded ? ViewStates.Gone : ViewStates.Visible;
			} else {
				holder.ReadedType.Text = string.Empty;
				holder.ReadedType.Visibility = ViewStates.Gone;
				//holder.ReadedType.Text = string.Format("已读人数：{0}/{1}", item.KindergartenCount, item.ReadedCount);
			}
		}



		protected void ll_Click(object sender, EventArgs e) {
			indexVM = indexVM ?? ((NoticeIndexView)activity).ViewModel;
			var positon = (int)((LinearLayout)sender).Tag;
			var item = list[positon];
			item.IsReaded = true;
			indexVM.ShowNoticeDetailViewModel(item.NoticeId);
		}

		protected void HandleItemClick(object sender, AdapterView.ItemClickEventArgs e) {		
			indexVM = indexVM ?? ((NoticeIndexView)activity).ViewModel;
			var positon = (int)((GridView)sender).Tag;
			var item = list[positon];
			item.IsReaded = true;
			indexVM.ShowNoticeDetailViewModel(item.NoticeId);
		}
	}

	public class MemoryIndexAdapter : BaseNoticeIndexAdapter
	{
		private IPictureService pic;
		protected MemoryIndexViewModel indexVM;
		private float dp;
		Handler handler;

		public MemoryIndexAdapter(MvxActivity activity, List<NoticeModel> list, float dp)
			: base(activity, list)
		{
			this.dp = dp;
			pic = Mvx.Resolve<IPictureService>();
			handler = new Handler();
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder;
			View view;
			var ischanged = true;
			var item = list[position];
			if (convertView == null) {
				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_GrowMemory, parent, false);
				holder = new ViewHolder();
				holder.Title = view.FindViewById<TextView>(Resource.Id.item_notice_title);
				holder.RealName = view.FindViewById<TextView>(Resource.Id.item_notice_realname);
				holder.CreateTime = view.FindViewById<TextView>(Resource.Id.item_notice_createtime);
				holder.ImageGrid = view.FindViewById<GridView>(Resource.Id.grid_memory_image);
				holder.HeadImage = view.FindViewById<ImageView>(Resource.Id.send_headimage);
				holder.NoticeId = item.NoticeId;
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
				if (holder.NoticeId != item.NoticeId) {
					holder.NoticeId = item.NoticeId;
				} else {
					ischanged = false;
				}
			}


			if (ischanged) {
				holder.Title.Text = item.Title;
				holder.RealName.Text = item.RealName;
				holder.CreateTime.Text = BabyBus.Logic.Shared.Utils.DateTimeString(item.CreateTime);
				var gridAdapter = new GrowMemoryGridAdapter(activity, item.ImageList);
				holder.ImageGrid.Adapter = gridAdapter;
				//Autofit Grid Height
				//TODO:Refactor
				var DefaultGridHeight = CustomConfig.DefaultThumbImageGridHeight;
				ViewGroup.LayoutParams layoutParams = holder.ImageGrid.LayoutParameters;
				layoutParams.Width = Convert.ToInt32((DefaultGridHeight * 3 + 4) * dp);
				if (item.ImageList == null) {
					layoutParams.Height = 0;
					holder.ImageGrid.LayoutParameters = layoutParams;
				} else if (item.ImageList.Count <= 3) { //One Column
					layoutParams.Height = Convert.ToInt32(DefaultGridHeight * dp);
					holder.ImageGrid.LayoutParameters = layoutParams;
				} else if (item.ImageList.Count <= 6) { //Two Column
					layoutParams.Height = Convert.ToInt32((DefaultGridHeight * 2 + 2) * dp);
					holder.ImageGrid.LayoutParameters = layoutParams;
				} else { //Three Column
					layoutParams.Height = Convert.ToInt32((DefaultGridHeight * 3 + 4) * dp);
					holder.ImageGrid.LayoutParameters = layoutParams;
				}
				holder.ImageGrid.Tag = position;
				holder.ImageGrid.ItemClick -= HandleItemClick;
				holder.ImageGrid.ItemClick += HandleItemClick;
				var ll = view.FindViewById<LinearLayout>(Resource.Id.item_notice_show);
				ll.Tag = position;
				ll.Click -= ll_Click;
				ll.Click += ll_Click;

				if (item.HeadImage != null) {
					var fileName = item.HeadImage + Constants.ThumbRule;
					pic.LoadIamgeFromSource(fileName, stream => {
						var ms = stream as MemoryStream;
						if (ms != null) {
							var bytes = ms.ToArray();
							var options = new BitmapFactory.Options() { InPurgeable = true };
							var bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
							handler.Post(() =>
								holder.HeadImage.SetImageBitmap(bmp));
						}
					}, Constants.ThumbServerPath);
				}
			}
			return view;
		}

		protected void ll_Click(object sender, EventArgs e) {
			indexVM = indexVM ?? ((MemoryIndexView)activity).ViewModel;
			var positon = (int)((LinearLayout)sender).Tag;
			var item = list[positon];
			indexVM.ShowNoticeDetailViewModel(item.NoticeId, 0);
		}

		protected void HandleItemClick(object sender, AdapterView.ItemClickEventArgs e) {		
			indexVM = indexVM ?? ((MemoryIndexView)activity).ViewModel;
			var positon = (int)((GridView)sender).Tag;
			var item = list[positon];
			indexVM.ShowNoticeDetailViewModel(item.NoticeId, e.Position);
		}
	}


}
