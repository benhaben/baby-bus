using System;
using Uri = Android.Net.Uri;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using BabyBus.Droid.Views;
using BabyBus.Logic.Shared;
using BabyBus.Droid.Broadcast;
using BabyBus.Droid.Utils;
using Cirrious.CrossCore;
using System.Threading.Tasks;
using AndroidHUD;
using Com.Squareup.Picasso;


namespace BabyBus.Droid
{
	[Activity(Label = "SettingMainView", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class SettingMainView : ViewBase<SettingMainViewModel>
	{
		private DownloadManager downloadManager;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			//Register Download Complete
			var receiver = new DownloadCompleteReceiver();
			RegisterReceiver(receiver, new IntentFilter(DownloadManager.ActionDownloadComplete));


		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);
			if (BabyBusContext.UserAllInfo.Child.Image == null && !string.IsNullOrEmpty(BabyBusContext.UserAllInfo.Child.ImageName)) {
				var chidimage = FindViewById<ImageView>(Resource.Id.child_head);
				Picasso.With(this).Load(Constants.ThumbServerPath + BabyBusContext.UserAllInfo.Child.ImageName).Priority(Picasso.Priority.High)
					.Placeholder(Resource.Drawable.Child_headImage).Error(Resource.Drawable.Child_headImage).Into(chidimage);
			}
		}

		protected override void OnViewModelSet()
		{
			SetContentView(Resource.Layout.Page_Setting_Main);	

			//UpdateVersion
			ViewModel.VerCode = PackageManager.GetPackageInfo(CustomConfig.PackageName, 0).VersionCode;

			ViewModel.NotifyUpdateVersion += NotifyUpdateVersion;

			var call = FindViewById<TextView>(Resource.Id.user_call);
			call.Click += (sender, args) => {
				Uri uri = Uri.Parse("tel:4009922586");
				var intent = new Intent(Intent.ActionCall, uri);
				StartActivity(intent);
			};
			ViewModel.ImageChangeEventHandler += (sender, e) => {
				ViewModel.Bytes = e;
			};
		}

		private void NotifyUpdateVersion(object sender, EventArgs e)
		{
			RunOnUiThread(() => {
				var ll = new TextView(this);
				try {
					var dlg = new AlertDialog.Builder(this);
					dlg.SetTitle(Resources.GetString(Resource.String.home_label_appupdate));
					dlg.SetMessage(string.Format(Resources.GetString(Resource.String.home_label_appupdatedesc), ViewModel.Version.VerName
						, ViewModel.Version.Description));
					dlg.SetView(ll);
					dlg.SetPositiveButton(Resource.String.mine_label_update, (o, args1) => DownloadApkByAndroid());
					dlg.SetCancelable(true);
					dlg.Show();
				} catch (Exception ex) {
					Mvx.Trace(ex.Message);
				}
			});
		}

		private void DownloadApkByAndroid()
		{
			BabyBusContext.UpdateStatus = true;
			BabyBusContext.SaveUser();
			Task.Run(() => {
				downloadManager = (DownloadManager)GetSystemService(DownloadService);
				//URI Resource
				string url = ViewModel.Version.Link;
				Uri resource = Uri.Parse(url);
				var request = new DownloadManager.Request(resource);
				request.SetMimeType("application/vnd.android.package-archive");
				#pragma warning disable 618
				request.SetShowRunningNotification(true);
				#pragma warning restore 618
				request.SetVisibleInDownloadsUi(true);
				request.SetTitle(ViewModel.Version.AppName);
				request.SetDestinationInExternalPublicDir(DownloadService, ViewModel.Version.ApkName);
				request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
				downloadManager.Enqueue(request);

				AndHUD.Shared.ShowToast(this, Resources.GetString(Resource.String.home_label_downloadstatus),
					MaskType.Black, TimeSpan.FromSeconds(1));
			});
		}
	}
}

