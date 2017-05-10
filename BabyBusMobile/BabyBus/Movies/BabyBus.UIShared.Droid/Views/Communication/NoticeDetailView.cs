using System;
using System.IO;

using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Uri = Android.Net.Uri;
using BabyBus.Droid.Utils;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using Android.Content;
using Android.Content.PM;

namespace BabyBus.Droid.Views.Communication
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = ScreenOrientation.Portrait)]
	public class NoticeDetailView : ViewBase<NoticeDetailViewModel>
	{
		private LinearLayout ll;
		private readonly Handler handler = new Handler();
		private TextView content;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetShareTitleWithBack(Resource.Layout.Page_Comm_NoticeDetail, Resource.String.notice_detial);
		}

		public override void ShareMessage()
		{
			base.ShareMessage();
			var intent = new Intent(this, typeof(OneKeyShare));
			intent.PutExtra("Description", ViewModel.Notice.Abstract);
			intent.PutExtra("ImageName", ViewModel.Notice.FirstImage);
			intent.PutExtra("Title", ViewModel.Notice.Title);
			intent.PutExtra("WebpageUrl", string.Format("http://115.28.88.175:8099/api/sharehtml?ContentType={0}&Id={1}", 0, ViewModel.Notice.NoticeId));
			StartActivity(intent);
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);
		
			ViewModel.FirstLoadedEventHandler += (sender1, args) => RunOnUiThread(() => {
				var pic = Mvx.Resolve<IPictureService>();

				content = FindViewById<TextView>(Resource.Id.notice_detail_content);
				content.SetTextIsSelectable(true);

				var ReadList = FindViewById<TextView>(Resource.Id.readlist);
				if (CustomConfig.ApkType == AppType.Parent) {
					ReadList.Visibility = ViewStates.Gone;
				} else if (CustomConfig.ApkType == AppType.Teacher
				           && (ViewModel.Notice.NoticeType == NoticeType.KindergartenAll
				           || ViewModel.Notice.NoticeType == NoticeType.KindergartenRecipe
				           || ViewModel.Notice.NoticeType == NoticeType.KindergartenStaff)) {
					ReadList.Visibility = ViewStates.Gone;
				} else if (CustomConfig.ApkType == AppType.Master
				           && ViewModel.Notice.NoticeType == NoticeType.KindergartenStaff) {
					ReadList.Visibility = ViewStates.Gone;
				} else {
					ReadList.Visibility = ViewStates.Visible;
					ReadList.Click += (sende, ee) => {

						ViewModel.ShowReadList.Execute();
					};
				}

				var NoticeTypeLabel = FindViewById<TextView>(Resource.Id.label_notice_type);
				switch (ViewModel.Notice.NoticeType) {
					case NoticeType.ClassCommon:
						NoticeTypeLabel.SetText(Resource.String.comm_label_notice);
						NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFBlue1);
						break;
					case NoticeType.ClassHomework:
						NoticeTypeLabel.SetText(Resource.String.comm_label_homework);
						NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFPurple1);
						break;
					case NoticeType.KindergartenAll:
						NoticeTypeLabel.SetText(Resource.String.comm_label_kindergartenall);
						NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFOrange1);
						break;
					case NoticeType.KindergartenRecipe:
						NoticeTypeLabel.SetText(Resource.String.comm_label_kindergartenrecipe);
						NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFGreen1);
						break;
					case NoticeType.KindergartenStaff:
						NoticeTypeLabel.SetText(Resource.String.comm_label_kindergartenstaff);
						NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.XFLake1);
						break;
					case NoticeType.BabyBusNotice:
						NoticeTypeLabel.SetText(Resource.String.ybtabloid);
						NoticeTypeLabel.SetBackgroundColor(BabyBus.Logic.Shared.Color.Gray);
						break;
				}


				ll = this.FindViewById<LinearLayout>(Resource.Id.notice_detail_layout);
				if (ViewModel.Notice.ImageList != null) {
					int position = 0;
					foreach (var imageName in ViewModel.Notice.ImageList) {
						var rl = (RelativeLayout)LayoutInflater.Inflate(Resource.Layout.Item_Notice_DetailImage, null);

						var image = rl.FindViewById<ImageView>(Resource.Id.notice_detailImage);
						//var bounder = rl.FindViewById<TextView>(Resource.Id.notice_detailImage_bounder);
						ll.AddView(rl);
						rl.Tag = position;
						position++;
						rl.Click += (sender2, args2) => {
							var imagePosition = (int)((RelativeLayout)sender2).Tag;
							ViewModel.ShowNoticeImageDetail(imagePosition);
						};
						pic.LoadIamgeFromSource(imageName, stream => {
							var ms = stream as MemoryStream;
							if (ms != null) {
								var bytes = ms.ToArray();
								var options = new BitmapFactory.Options() { InPurgeable = true };
								var bmp = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);
								handler.Post(() => {
									image.SetImageBitmap(bmp);
									var parms = image.LayoutParameters;
									float factor = (float)image.Width / bmp.Width;
									parms.Height = (int)(bmp.Height * factor);
									//                                var parms1 = bounder.LayoutParameters;
									//                                parms1.Height = parms.Height + 10;
								});
							}
						});
					}
				}
			});

			//Stats Page Report
			StatsUtils.LogPageReport(PageReportType.NoticeDetail);
		}

	}
}