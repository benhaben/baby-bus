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
	public class PostPayOrderAdapter : BaseAdapter
	{
		protected readonly MvxActivity activity;
		public List<ECPayOrder> List;
		protected SettingPayOrderViewModel indexVM;

		public string PicassoTag{ get; private set; }

		public PostPayOrderAdapter(MvxActivity activity, List<ECPayOrder> list)
		{
			this.activity = activity;
			List = list;
			PicassoTag = "PostPayOrderAdapterTag";
		}

		public override int Count {
			get { return List.Count; }
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

				view = activity.LayoutInflater.Inflate(Resource.Layout.Item_EC_PostPayorder, parent, false);
				holder = new ViewHolder();
				holder.Address = view.FindViewById<TextView>(Resource.Id.address);
				holder.ClassTime = view.FindViewById<TextView>(Resource.Id.time);
				holder.Icon_recommendation = view.FindViewById<ImageView>(Resource.Id.icon_recommendation);
				holder.InvolvedCount = view.FindViewById<TextView>(Resource.Id.number);
				holder.Price = view.FindViewById<TextView>(Resource.Id.cost);
				holder.Rating = view.FindViewById<RatingBar>(Resource.Id.user_recommendation);
				holder.Title = view.FindViewById<TextView>(Resource.Id.title);
				holder.Paymentmessage = view.FindViewById<LinearLayout>(Resource.Id.payment_message);
				holder.ReviewMessage = view.FindViewById<LinearLayout>(Resource.Id.reviewmessage);
				holder.ButtonPayment = view.FindViewById<Button>(Resource.Id.button_pay);
				holder.ButtonReview = view.FindViewById<Button>(Resource.Id.button_review);
				holder.ReviewRating = view.FindViewById<RatingBar>(Resource.Id.my_recommendation);
				holder.ReviewContent = view.FindViewById<TextView>(Resource.Id.postreview);
				holder.Reviewed = view.FindViewById<TextView>(Resource.Id.reviewed);
				holder.Paid = view.FindViewById<TextView>(Resource.Id.paid);
				view.Tag = holder;
			} else {
				view = convertView;
				holder = (ViewHolder)view.Tag;
			}
			var item = List[position];
			if (item.ReviewId == 0 || item.Status == 1) {
				holder.Paymentmessage.Visibility = ViewStates.Visible;
				holder.ReviewMessage.Visibility = ViewStates.Gone;
				holder.Address.Text = item.Address;
				holder.ClassTime.Text = item.LessonTime;
				holder.Title.Text = item.Title;
				holder.Paid.Visibility = ViewStates.Gone;
				holder.InvolvedCount.Text = string.Format("{0}/{1}", item.InvolvedCount, item.TotalCount); 
				holder.Price.Text = string.Format("￥{0} 元", item.CurrentPrice);
				holder.Reviewed.Visibility = ViewStates.Gone;
				if (item.Status == 1) {//未支付，未评论或已点评
					holder.ButtonPayment.Visibility = ViewStates.Visible;
					holder.ButtonReview.Visibility = ViewStates.Gone;
					holder.ButtonPayment.Tag = position;
					holder.ButtonPayment.Click -= Pay_Click;
					holder.ButtonPayment.Click += Pay_Click;
				} else if (item.Status == 2) {//已支付，未评论
					holder.Paid.Visibility = ViewStates.Visible;
					holder.ButtonPayment.Visibility = ViewStates.Gone;
					holder.ButtonReview.Visibility = ViewStates.Visible;
					holder.ButtonReview.Tag = position;
					holder.ButtonReview.Click -= Review_Click;
					holder.ButtonReview.Click += Review_Click;
				}
			} else {//已支付，已评论
				holder.ButtonPayment.Visibility = ViewStates.Gone;
				holder.ButtonReview.Visibility = ViewStates.Gone;
				holder.Paymentmessage.Visibility = ViewStates.Gone;
				holder.ReviewMessage.Visibility = ViewStates.Visible;
				holder.Reviewed.Visibility = ViewStates.Visible;
				holder.ReviewRating.Rating = (float)item.ReviewRating;
				holder.ReviewContent.Text = item.ReviewContent;
				holder.Paid.Visibility = ViewStates.Visible;

			}
			holder.Rating.Rating = (float)item.Rating;
			if (!string.IsNullOrEmpty(item.ImageUrls)) {
				Picasso.With(activity).Load(Constants.ThumbServerPath + item.FirstImage).Tag(PicassoTag).CenterCrop()
					.Resize(80, 80).Placeholder(Resource.Drawable.icon_course).Into(holder.Icon_recommendation);
			}
			var ll = view.FindViewById<LinearLayout>(Resource.Id.item_recommendation);
			ll.Tag = position;
			ll.Click -= ll_Click;
			ll.Click += ll_Click;
			return view;
		}

		protected void ll_Click(object sender, EventArgs e)
		{
			indexVM = indexVM ?? ((SettingPayOrderView)activity).ViewModel;
			var positon = (int)((LinearLayout)sender).Tag;
			var item = List[positon];
			indexVM.ShowCourseDetailViewModel(item.PostInfoId, (ECColumnType)item.ColumnType);
		}

		protected void Pay_Click(object sender, EventArgs e)
		{
			indexVM = indexVM ?? ((SettingPayOrderView)activity).ViewModel;
			var positon = (int)((Button)sender).Tag;
			var item = List[positon];
			indexVM.ShowPayment(item.PostInfoId);
		}

		protected void Review_Click(object sender, EventArgs e)
		{
			indexVM = indexVM ?? ((SettingPayOrderView)activity).ViewModel;
			var positon = (int)((Button)sender).Tag;
			var item = List[positon];
			indexVM.SendReview(item.PostInfoId);
		}

		protected class ViewHolder : Object
		{

			public ImageView Icon_recommendation{ get; set; }

			public TextView Title{ get; set; }

			public TextView Address{ get; set; }

			public RatingBar Rating{ get; set; }

			public TextView InvolvedCount{ get; set; }

			public  TextView ClassTime{ get; set; }

			public  TextView Price{ get; set; }

			public LinearLayout Paymentmessage{ get; set; }

			public LinearLayout ReviewMessage{ get; set; }

			public Button ButtonPayment{ get; set; }

			public Button ButtonReview{ get; set; }

			public RatingBar ReviewRating{ get; set; }

			public TextView ReviewContent{ get; set; }

			public TextView Reviewed { get; set; }

			public TextView Paid { get; set; }


		}



	}
}


