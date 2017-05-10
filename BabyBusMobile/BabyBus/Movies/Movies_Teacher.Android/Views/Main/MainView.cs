using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidHUD;
using BabyBus.Droid.Broadcast;
using BabyBus.Droid.Control;
using BabyBus.Droid.Utils;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Newtonsoft.Json;
using Xamarin;
using Uri = Android.Net.Uri;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid.Views.Main
{
	[Activity(Theme = "@style/CustomTheme")]
	public class MainView : TabbedViewBase<MainViewModel>
	{
		private Bundle bundle;

		protected override void OnCreate(Bundle bundle)
		{
			var intent = this.Intent;
			this.bundle = intent.Extras;

			base.OnCreate(bundle);



//            if (BabyBusContext.Kindergarten != null)
//                Title = BabyBusContext.Kindergarten.KindergartenName;
			//Set Push Tags
			Utils.Utils.SetPushTags(this);
			//Set Push Alias 
			Utils.Utils.SetPushAlias(this);
			//Insights User
			var infos = new Dictionary<string, string> {
				{ "Name", BabyBusContext.UserAllInfo.RealName },
				{ "Phone", BabyBusContext.UserAllInfo.LoginName }
			};
			var unique = BabyBusContext.Kindergarten.KindergartenName + "_"
			             + BabyBusContext.Class.ClassName + "_" + BabyBusContext.UserAllInfo.RealName;
			Insights.Identify(unique, infos);

			//Enter Log
			ViewModel.VerCode = PackageManager.GetPackageInfo(CustomConfig.PackageName, 0).VersionCode;
			ViewModel.VerName = PackageManager.GetPackageInfo(CustomConfig.PackageName, 0).VersionName;
			ViewModel.PhoneMode = Build.Model;
			ViewModel.OSType = "Android";
			ViewModel.EnterLog();

			//Register Download Complete
			var receiver = new DownloadCompleteReceiver();
			RegisterReceiver(receiver, new IntentFilter(DownloadManager.ActionDownloadComplete));

		}

		protected override void OnViewModelSet()
		{
			var title = string.Empty;
			if (BabyBusContext.Kindergarten != null)
				title = BabyBusContext.Kindergarten.KindergartenName;

			SetCustomTitle(Resource.Layout.MainPrtView, title);

			TabHost.TabSpec spec;
			TabView view;


			view = new TabView(this, Resource.Drawable.Tab_Home, Resource.Drawable.Tab_Home);
			spec = TabHost.NewTabSpec("Home");
			spec.SetIndicator(view);
//            spec.SetIndicator("ê×ò3",Resources.GetDrawable(Resource.Drawable.Tab_Home));
			if (BabyBusContext.UserAllInfo.RoleType == RoleType.Parent) {
				spec.SetContent(this.CreateIntentFor(ViewModel.ParentHomeViewModel));
			} else if (BabyBusContext.UserAllInfo.RoleType == RoleType.Teacher) {
				spec.SetContent(this.CreateIntentFor(ViewModel.TeacherHomeViewModel));
			} else {
				spec.SetContent(this.CreateIntentFor(ViewModel.TeacherHomeViewModel));
			}
			TabHost.AddTab(spec);

			view = new TabView(this, Resource.Drawable.Tab_Photo, Resource.Drawable.Tab_Photo);
			spec = TabHost.NewTabSpec("GrowMemory");
			spec.SetIndicator(view);
			spec.SetContent(this.CreateIntentFor(ViewModel.MemoryIndexViewModel));
			TabHost.AddTab(spec);

			view = new TabView(this, Resource.Drawable.Tab_Notice, Resource.Drawable.Tab_Notice);
			spec = TabHost.NewTabSpec("Notice");
			spec.SetIndicator(view);
			spec.SetContent(this.CreateIntentFor(ViewModel.NoticeIndexViewModel));
			TabHost.AddTab(spec);

			view = new TabView(this, Resource.Drawable.Tab_Memo, Resource.Drawable.Tab_Memo);
			spec = TabHost.NewTabSpec("Question");
			spec.SetIndicator(view);
			spec.SetContent(this.CreateIntentFor(ViewModel.QuestionIndexViewModel));
			TabHost.AddTab(spec);

			view = new TabView(this, Resource.Drawable.Tab_Me, Resource.Drawable.Tab_Me);
			spec = TabHost.NewTabSpec("Mine");
			spec.SetIndicator(view);
			spec.SetContent(this.CreateIntentFor(ViewModel.SettingViewModel));
			TabHost.AddTab(spec);

			TabHost.SetCurrentTabByTag(ViewModel.Tag ?? "Home");



			//UpdateVersion
			ViewModel.VerCode = PackageManager.GetPackageInfo(CustomConfig.PackageName, 0).VersionCode;
			ViewModel.NotifyUpdateVersion += NotifyUpdateVersion;
			Task.Run(() =>
                ViewModel.UpdateApkCommand.Execute());
		}

		protected override void OnNewIntent(Intent intent) {
			base.OnNewIntent(intent);

			this.bundle = intent.Extras;

			if (bundle != null) {
				var json = bundle.GetString("cn.jpush.android.EXTRA");
				if (!string.IsNullOrEmpty(json)) {
					var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
					if (dic.ContainsKey("Tag")) {
						TabHost.SetCurrentTabByTag(dic["Tag"]);
						if (dic.ContainsKey("NoticeId")) {
							if (dic["Tag"] == "GrowMemory") {
								ViewModel.ShowGrowMemoryDetailViewModel(int.Parse(dic["NoticeId"]));
							} else if (dic["Tag"] == "Notice") {
								ViewModel.ShowNoticeDetailViewModel(int.Parse(dic["NoticeId"]));
							}
						} else if (dic.ContainsKey("QuestionId")) {
							ViewModel.ShowQuestionDetailCommand(int.Parse(dic["QuestionId"]), true);
						}
					}
				}
			}
		}

		private void NotifyUpdateVersion(object sender, EventArgs e)
		{
			var text = new TextView(this);
			if (BabyBusContext.UpdateStatus) {
				try {
					var dlg = new AlertDialog.Builder(this);
					dlg.SetTitle(Resources.GetString(Resource.String.home_label_appupdate));
					dlg.SetMessage(string.Format(Resources.GetString(Resource.String.home_label_appupdatedesc), ViewModel.Version.VerName
						, ViewModel.Version.Description));
					dlg.SetView(text);
					dlg.SetPositiveButton(Resource.String.mine_label_update, (o, args1) => {
						DownloadApkByAndroid();
					});
					dlg.SetNeutralButton(Resource.String.mine_label_cancel, (o, args1) => {

					});
					dlg.SetNegativeButton(Resource.String.mine_label_neverupdate, (o, args1) => {
						BabyBusContext.UpdateStatus = false;
						BabyBusContext.SaveUser();
					});


					dlg.SetCancelable(true);
					RunOnUiThread(() => dlg.Show());
				} catch (Exception ex) {
					Mvx.Trace(ex.Message);
				}
			}
		}

		private void DownloadApkByAndroid()
		{
			Task.Run(() => {
				var downloadManager = (DownloadManager)GetSystemService(DownloadService);
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
	}
}