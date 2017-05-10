using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidHUD;
using BabyBus.Droid.Broadcast;
using BabyBus.Droid.Utils;
using Cirrious.CrossCore;
using CN.Jpush.Android.Api;
using Uri = Android.Net.Uri;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Setting
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class SettingView : ViewBase<SettingViewModel>
	{
		private DownloadManager downloadManager;
		private Button logout;
		private LinearLayout clearCache, payService;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			//Register Download Complete
			var receiver = new DownloadCompleteReceiver();
			RegisterReceiver(receiver, new IntentFilter(DownloadManager.ActionDownloadComplete));

		}

		protected override void OnViewModelSet()
		{
			
			//SetContentView(Resource.Layout.Page_Mine_MineView);
			SetCustomTitleWithBack(Resource.Layout.Page_Mine_MineView, "我的信息");

			var verText = FindViewById<TextView>(Resource.Id.mine_label_version);
			verText.Text = PackageManager.GetPackageInfo(CustomConfig.PackageName, 0).VersionName;
			logout = FindViewById<Button>(Resource.Id.button_logout);
			logout.Click += (sender, args) =>
				this.ShowConfirm(Resources.GetString(Resource.String.mine_info_logout), 
				Resources.GetString(Resource.String.mine_confirm_logout), () => {
				Utils.Utils.ClearTags(this);
				Utils.Utils.ClearAlias(this);
				ViewModel.LogoutCommand.Execute();
			});

			clearCache = FindViewById<LinearLayout>(Resource.Id.clear_cache);
			clearCache.Click += (sender, args) =>
				this.ShowConfirm(Resources.GetString(Resource.String.mine_info_clearcache), 
				Resources.GetString(Resource.String.mine_comfirm_clearcache), 
				() => ViewModel.ClearCacheCommand.Execute());

			//Pay Service
			payService = FindViewById<LinearLayout>(Resource.Id.pay_service);
			payService.Click += (sender, e) => {
				var intent = new Intent(this, typeof(ECPaymentView));
				StartActivity(intent);
			};

			//UpdateVersion
			ViewModel.VerCode = PackageManager.GetPackageInfo(CustomConfig.PackageName, 0).VersionCode;

			ViewModel.NotifyUpdateVersion += NotifyUpdateVersion;

			var call = FindViewById<TextView>(Resource.Id.user_call);
			call.Click += (sender, args) => {
				Uri uri = Uri.Parse("tel:4009922586");
				var intent = new Intent(Intent.ActionCall, uri);
				StartActivity(intent);
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
				request.SetShowRunningNotification(true);
				request.SetVisibleInDownloadsUi(true);
				request.SetTitle(ViewModel.Version.AppName);
				request.SetDestinationInExternalPublicDir(DownloadService, ViewModel.Version.ApkName);
				request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
				downloadManager.Enqueue(request);

				AndHUD.Shared.ShowToast(this, Resources.GetString(Resource.String.home_label_downloadstatus),
					MaskType.Black, TimeSpan.FromSeconds(1));
			});
		}

		#if false //Custom Download Apk
	    private void DownloadApk() {
	        Task.Run(() => {
	            try
	            {
	                string path = Environment.GetExternalStoragePublicDirectory(DownloadService) + "/";
	                string savePath = path;
                    string url = "http://115.28.225.27/AppAPI/Apk/babybus.apk";
	                var download = Mvx.Resolve<IMvxHttpFileDownloader>();
                    download.RequestDownload(url, savePath + "/babybus.apk", () => {
                        InstallApk(savePath);
                    }, ex => {
                        //Download Failed
                    });
	            }
	            catch (Exception ex)
	            {
	                MvxTrace.Trace(ex.Message);
	            }
	        });
	    }

	    private void InstallApk(string savePath) {
            pBar.Cancel();
            File apkFile = new File(savePath, "babybus.apk");
	        if (!apkFile.Exists())
	            return;
            Intent i = new Intent(Intent.ActionView);
	        i.SetDataAndType(Uri.Parse("file://" + apkFile.ToString()), "application/vnd.android.package-archive");
            StartActivity(i);
	    }
#endif

		protected override void OnResume()
		{
			base.OnResume();
			ViewModel.LoadData();
		}
	}
}