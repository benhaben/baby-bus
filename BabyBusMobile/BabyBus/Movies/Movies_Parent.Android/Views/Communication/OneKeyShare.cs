using Android.App;
using Android.OS;
using System.Collections.Generic;
using BabyBus.Logic.Shared.MobileModel.Enums;
using Android.Widget;
using BabyBus.Droid.Adapters;
using Com.Tencent.MM.Sdk.Openapi;
using Com.Tencent.MM.Sdk.Modelmsg;
using Android.Graphics;
using System;
using Cirrious.MvvmCross.Droid.Fragging;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using System.Text;

namespace BabyBus.Droid.Views.Communication
{
	[Activity(Theme = "@style/MyDialogStyle")]		
	public class OneKeyShare : MvxFragmentActivity
	{
		private readonly List<ShareConfig> ShareAppList;
		private ShareAppAdapter Adapter;
		private readonly IWXAPI msgApi;
		private IPictureService _pic;
		private Handler _handler;
		private string ShareMessageTitle = string.Empty;
		private string Description = string.Empty;

		private string WebpageUrl{ get; set; }

		private string ImageName{ get; set; }

		private bool IsGridLayout{ get; set; }

		public OneKeyShare()
		{
			ShareAppList = new List<ShareConfig>();
			this.msgApi = WXAPIFactory.CreateWXAPI(this, null);
			_pic = Mvx.Resolve<IPictureService>();
			_handler = new Handler();
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			if (Intent.Extras != null) {
				ShareMessageTitle = Intent.Extras.GetString("Title");
				Description = Intent.Extras.GetString("Description");
				WebpageUrl = Intent.Extras.GetString("WebpageUrl");
				ImageName = Intent.Extras.GetString("ImageName");
				IsGridLayout = Intent.Extras.GetBoolean("IsGridLayout");
			}
			SetContentView(Resource.Layout.Page_MessageShare);
			var shareAppGrid = FindViewById<GridView>(Resource.Id.shareapp_grid);
			var shareAppList = FindViewById<ListView>(Resource.Id.shareapp_list);
			var ButtonClose = FindViewById<Button>(Resource.Id.button_close);
			ButtonClose.Click += (sender, e) => {
				;
			};
			ShareAppList.Add(new ShareConfig() {
				ImageId = Resource.Drawable.ssdk_oks_skyblue_logo_wechat_checked,
				AppName = "微信好友",
				Abstract = "把精彩内容分享到通讯录里的小伙伴",
				ShareAppId = ShareAppType.WXSceneSession,
			});
			ShareAppList.Add(new ShareConfig() {
				ImageId = Resource.Drawable.ssdk_oks_skyblue_logo_wechatmoments_checked,
				AppName = "微信朋友圈",
				Abstract = "直接把精彩内容分享到圈子里！",
				ShareAppId = ShareAppType.WXSceneTimeline,
			});

			Adapter = new ShareAppAdapter(this, ShareAppList, false);
			if (IsGridLayout) {
				shareAppGrid.Adapter = Adapter;
				shareAppGrid.ItemClick += (sender, e) => GotoShareApp(ShareAppList[e.Position].ShareAppId);
			} else {
				shareAppList.Adapter = Adapter;
				shareAppList.ItemClick += (sender, e) => GotoShareApp(ShareAppList[e.Position].ShareAppId);
			}

		}

		public void GotoShareApp(ShareAppType shareAppId)
		{
			switch (shareAppId) {
				case ShareAppType.WXSceneSession:
					WXShare(WXShareType.WXSceneSession);
					break;
				case ShareAppType.WXSceneTimeline:
					WXShare(WXShareType.WXSceneTimeline);
					break;
				default:
					break;
			}
		}

		void WXShare(WXShareType WXsharetype)
		{
			msgApi.RegisterApp(WXConfig.APPID);

			var req = new SendMessageToWX.Req();
			WXWebpageObject webpage = new WXWebpageObject();
			if (!string.IsNullOrEmpty(WebpageUrl)) {
				webpage.WebpageUrl = WebpageUrl;
			} else {
				webpage.WebpageUrl = "http://www.baidu.com";
			}

			WXMediaMessage msg = new WXMediaMessage(webpage);
			if (!string.IsNullOrEmpty(ShareMessageTitle)) {
				msg.Title = ShareMessageTitle;
			} else {
				msg.Title = "优贝在线";
			}
			if (!string.IsNullOrEmpty(Description)) {
				msg.Description = Description + "查看更多内容请下载贝贝巴士App，连接：http://fir.im/3bus";
			} else {
				msg.Description = "查看更多内容请下载贝贝巴士App，连接：http://fir.im/3bus";
			}
			var bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Icon);

//			if (!string.IsNullOrEmpty(ImageName)) {
//
//				var fullfilename = Constants.ThumbServerPath + ImageName;
//				var ll = ImageSendHelper.GetData(fullfilename);
//				UTF8Encoding UTF8 = new UTF8Encoding();
//				var bytemessage = UTF8.GetBytes(ll);
//				var option = new BitmapFactory.Options() { InPurgeable = true };
//				bitmap = BitmapFactory.DecodeByteArray(bytemessage, 0, bytemessage.Length, option);
//				bitmap = BitmapFactory.DecodeFile(ll);
//			} 
			msg.SetThumbImage(bitmap);
			req.Transaction = "webpage" + DateTime.Now.Ticks;
			req.Message = msg;
			//WXSceneSession是会话；WXSceneTimeline是朋友圈
			if (WXsharetype == WXShareType.WXSceneSession) {
				req.Scene = SendMessageToWX.Req.WXSceneSession;
			} else {
				req.Scene = SendMessageToWX.Req.WXSceneTimeline;
			}

			msgApi.SendReq(req);
			Finish();
		}
	}

	public  class ShareConfig
	{
		public	int ImageId{ get; set; }

		public	string AppName{ get; set; }

		public string Abstract{ get; set; }

		public ShareAppType ShareAppId{ get; set; }
	}

	public enum WXShareType
	{
		//微信会话
		WXSceneSession = 1,
		//微信朋友圈
		WXSceneTimeline = 2,
	}
}

