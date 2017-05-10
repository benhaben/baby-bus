using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidHUD;
using BabyBus.Droid.Control;
using BabyBus.Droid.Utils;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using Newtonsoft.Json;
using Xamarin;
using Uri = Android.Net.Uri;
using BabyBus.Droid.Broadcast;
using BabyBus.Logic.Shared;

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
			if (BabyBusContext.Kindergarten != null)
				Title = BabyBusContext.Kindergarten.KindergartenName;
			//Set Push Tags
			Utils.Utils.SetPushTags(this);
			//Set Push Alias 
			Utils.Utils.SetPushAlias(this);
			//Insights User
			var infos = new Dictionary<string, string> {
				{ "Name", BabyBusContext.UserAllInfo.RealName },
				{ "Phone", BabyBusContext.UserAllInfo.LoginName }
			};
			var unique = BabyBusContext.Kindergarten.KindergartenName + "_" + BabyBusContext.UserAllInfo.RealName;
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
			spec.SetContent(this.CreateIntentFor(ViewModel.MasterHomeViewModel));
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
			spec = TabHost.NewTabSpec("mine");
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

		protected override void OnNewIntent(Intent intent)
		{
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
								if (bool.Parse(dic["IsHtml"])) {
									ViewModel.ShowNoticeDetailViewModel(int.Parse(dic["NoticeId"]), true);
								} else {
									ViewModel.ShowNoticeDetailViewModel(int.Parse(dic["NoticeId"]), false);
								}
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


		//public override bool OnCreateOptionsMenu(IMenu menu)
		//{
		//    MenuInflater.Inflate(Resource.Menu.main_prt_menu,menu);
		//    return true;
		//}

		//public override bool OnOptionsItemSelected(IMenuItem item)
		//{
		//    switch (item.ItemId)
		//    {
		//        //如果选取的是 开启当麻许的超技八
		//        case Resource.ChildId.main_prt_menu_item_home:
		//            Toast.MakeText(this, "开启当麻许的超技八", ToastLength.Short).Show();
		//            //开启一个Inetnt 并且将此呼叫起来
		//            StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://no2don.blogspot.com")));
		//            return true;
		//        //如果选取的是 开启Market的爱料理 Download
		//        case Resource.ChildId.main_prt_menu_item_photo:
		//            Toast.MakeText(this, "开启Market的爱料理 Download", ToastLength.Short).Show();
		//            //开启一个Inetnt 并且将此呼叫起来
		//            StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + "com.polydice.icook")));
		//            return true;
		//        case Resource.ChildId.main_prt_menu_item_notice:
		//            Toast.MakeText(this, "开启Market的爱料理 Download", ToastLength.Short).Show();
		//            //开启一个Inetnt 并且将此呼叫起来
		//            StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + "com.polydice.icook")));
		//            return true;
		//        case Resource.ChildId.main_prt_menu_item_me:
		//            Toast.MakeText(this, "开启Market的爱料理 Download", ToastLength.Short).Show();
		//            //开启一个Inetnt 并且将此呼叫起来
		//            StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + "com.polydice.icook")));
		//            return true;
		//        default:
		//            return base.OnOptionsItemSelected(item);
		//    }
		//}
	}
}