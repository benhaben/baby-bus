using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Com.Tencent.MM.Sdk.Openapi;
using Com.Tencent.MM.Sdk.Modelpay;
using System.Collections.Generic;
using Org.Apache.Http;
using Org.Apache.Http.Message;
using System.Threading.Tasks;
using System.Xml;
using Cirrious.CrossCore;
using BabyBus.Droid.Views;
using Com.Alipay.Sdk.App;
using BabyBus.Logic.Shared;
using Java.Net;
using Java.IO;
using Android.Views;
using Android.Content;
using System.Text;

namespace BabyBus.Droid
{
	[Activity(Theme = "@style/CustomTheme", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class PaymentView : ViewBase<SettingPaymentViewModel>
	{
		private PayReq req;
		private readonly IWXAPI msgApi;
		private readonly PayTask mAliPay;
		//		IDictionary<string,string> resultunifiedorder;
		StringBuilder packagesb;
		private const int PayMany = 0;
		private static PayType payType = new PayType();

		public PaymentView()
		{
			this.msgApi = WXAPIFactory.CreateWXAPI(this, null);
			this.mAliPay = new PayTask(this);
			payType = PayType.AlPay;
		}

		public override void OnCreateCalled(object sender, Cirrious.CrossCore.Core.MvxValueEventArgs<Bundle> e)
		{
			base.OnCreateCalled(sender, e);	

			SetCustomTitleWithBack(Resource.Layout.Bar_Default, Resource.String.regester_lable_onlinepryment);
			// Create your application here
			SetContentView(Resource.Layout.Page_Setting_Payment);

			NetContectStatus.registerReceiver(this);
			var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
			NetContectStatus.NetStatus(this, netStatus);

			req = new PayReq();
			msgApi.RegisterApp(WXConfig.APPID);
			packagesb = new StringBuilder();

			//prePayId = FindViewById<TextView>(Resource.Id.prePayId);
			//paySign = FindViewById<TextView>(Resource.Id.paySign);

			var btalpay = FindViewById<LinearLayout>(Resource.Id.bt_alpay);
			var btwxpay = FindViewById<LinearLayout>(Resource.Id.bt_wxpay);
			var IconAlPay = FindViewById<ImageView>(Resource.Id.icon_alpay_select); 
			var IconWxPay = FindViewById<ImageView>(Resource.Id.icon_wxpay_select);
			var IconUnalPay = FindViewById<ImageView>(Resource.Id.icon_alpay_unselect); 
			var IconUnwxPay = FindViewById<ImageView>(Resource.Id.icon_wxpay_unselect);
			var _handler = new Handler();
			ViewModel.FirstLoadedEventHandler += (sender1, listObject) =>
				_handler.Post(new Java.Lang.Runnable(InitMessage));
			btalpay.Click += (sender1, e1) => {
				payType = PayType.AlPay;
				IconAlPay.Visibility = ViewStates.Visible;
				IconWxPay.Visibility = ViewStates.Gone;

				IconUnalPay.Visibility = ViewStates.Gone;
				IconUnwxPay.Visibility = ViewStates.Visible;
			};
			btwxpay.Click += (sender2, e2) => {
				payType = PayType.Wxpay;
				IconAlPay.Visibility = ViewStates.Gone;
				IconWxPay.Visibility = ViewStates.Visible;

				IconUnwxPay.Visibility = ViewStates.Gone;
				IconUnalPay.Visibility = ViewStates.Visible;
			};

			var btnPay = FindViewById<Button>(Resource.Id.btn_pay);
			btnPay.Click += (sender3, e3) => {
				{	
					if (BabyBusContext.IsAvailable) {

						switch (payType) {
							case PayType.AlPay:
								ViewModel.PaymentProcessing(1, parms => {

									Task task = Task.Factory.StartNew(() => {
										var orderInfor = parms["alpayinfo"];
										var payInfor = ALGetPayInfo(orderInfor);
										var m = mAliPay.Pay(payInfor);
									});

								});
								break;

							case PayType.Wxpay:
								ViewModel.PaymentProcessing(2, parms => {
									req.AppId = parms["appid"];
									req.PartnerId = parms["partnerid"];
									req.NonceStr = parms["noncestr"];
									req.PackageValue = parms["package"];
									req.PrepayId = parms["prepayid"];
									req.TimeStamp = parms["timestamp"];
									req.Sign = parms["sign"];

									msgApi.RegisterApp(WXConfig.APPID);
									msgApi.SendReq(req);
								});
								break;

							default:
								break;
						}

					} else {
						Simpleer("无网络连接", "由于网络故障无法完成支付，请检查网络设置！");
					}
				}

			};
		}

		private void InitMessage()
		{
			CheckBox[] checkBox = new CheckBox[8] {
				FindViewById<CheckBox>(Resource.Id.Checkbox_1),
				FindViewById<CheckBox>(Resource.Id.Checkbox_2),
				FindViewById<CheckBox>(Resource.Id.Checkbox_3),
				FindViewById<CheckBox>(Resource.Id.Checkbox_4),
				FindViewById<CheckBox>(Resource.Id.Checkbox_5),
				FindViewById<CheckBox>(Resource.Id.Checkbox_6),
				FindViewById<CheckBox>(Resource.Id.Checkbox_7),
				FindViewById<CheckBox>(Resource.Id.Checkbox_8),
			};
			//			var checkBox_1 = FindViewById<CheckBox>(Resource.Id.Checkbox_1);
			//			var checkbox_2 = FindViewById<CheckBox>(Resource.Id.Checkbox_2);
			//			var checkbox_3 = FindViewById<CheckBox>(Resource.Id.Checkbox_3);
			//			var checkbox_4 = FindViewById<CheckBox>(Resource.Id.Checkbox_4);
			//			var checkbox_5 = FindViewById<CheckBox>(Resource.Id.Checkbox_5);
			//			var checkbox_6 = FindViewById<CheckBox>(Resource.Id.Checkbox_6);
			//			var checkbox_7 = FindViewById<CheckBox>(Resource.Id.Checkbox_7);
			//			var checkbox_8 = FindViewById<CheckBox>(Resource.Id.Checkbox_8);
			var paymentfee = FindViewById<TextView>(Resource.Id.paymentfee);
		

			string[] Hint = new string[8] {
				ViewModel.AddedValue.Hint1,
				ViewModel.AddedValue.Hint2,
				ViewModel.AddedValue.Hint3,
				ViewModel.AddedValue.Hint4,
				ViewModel.AddedValue.Hint5,
				ViewModel.AddedValue.Hint6,
				ViewModel.AddedValue.Hint7,
				ViewModel.AddedValue.Hint8,
			};
			for (int i = 0; i < 8; i++) {
				if (!string.IsNullOrEmpty(Hint[i])) {
					checkBox[i].Text = Hint[i];
					checkBox[i].Visibility = ViewStates.Visible;
				}
			}
			paymentfee.Text = ViewModel.AddedValue.CurrentPrice.ToString("C");
		}

		protected override void OnResume()
		{
			base.OnResume();

			ViewModel.CheckPaymentResult();
		}

		void SendPayReq()
		{
			try {
				msgApi.RegisterApp(WXConfig.APPID);
				msgApi.SendReq(req);
			} catch (Exception ex) {
				Mvx.Trace(ex.Message);
			}
		}

		async Task<IDictionary<string,string>> GetPrePayId(int many_yuan)
		{
			string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
			string entity = GenProductArgs(many_yuan);

			string content = await BabyBus.Logic.Shared.Utils.PaymentHttpPost(url, entity);
			IDictionary<string,string> result = DecodeXml(content);
			return result;
		}

		void GenPaySign()
		{
			req.AppId = WXConfig.APPID;
			req.PartnerId = WXConfig.MCHID;
//			req.PrepayId = resultunifiedorder["prepay_id"];
			req.PackageValue = "Sign=WXPay";
			req.NonceStr = GenNonceStr();
			req.TimeStamp = GenTimeStamp();

			var signParams = new List<INameValuePair>();

			signParams.Add(new BasicNameValuePair("appid", req.AppId));
			signParams.Add(new BasicNameValuePair("noncestr", req.NonceStr));
			signParams.Add(new BasicNameValuePair("package", req.PackageValue));
			signParams.Add(new BasicNameValuePair("partnerid", req.PartnerId));
			signParams.Add(new BasicNameValuePair("prepayid", req.PrepayId));
			signParams.Add(new BasicNameValuePair("timestamp", req.TimeStamp));

			req.Sign = GenAppSign(signParams);

			packagesb.Append("sign\n" + req.Sign + "\n\n");

		}

		string GenAppSign(List<INameValuePair> parms)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < parms.Count; i++) {
				sb.Append(parms[i].Name);
				sb.Append('=');
				sb.Append(parms[i].Value);
				sb.Append('&');
			}
			sb.Append("key=");
			sb.Append(WXConfig.APIKey);

			packagesb.Append("sign str\n" + sb + "\n\n");
			string appSign = MD5.GetMessageDigest(Encoding.UTF8.GetBytes(sb.ToString())).ToUpper();
			return appSign;
		}

		string GenTimeStamp()
		{
			return (DateTime.Now.Ticks / 1000).ToString();
		}

		IDictionary<string,string> DecodeXml(string content)
		{
			try {
				IDictionary<string, string> xml = new Dictionary<string, string>();

				//Use XmlReader to Parse Content
				var parser = XmlReader.Create(new System.IO.StringReader(content));
				while (parser.Read()) {
					if (parser.Name == "xml") {
						continue;
					}
					if (parser.IsStartElement()) {
						var nodeName = parser.Name;
						parser.Read();
						var nodeValue = parser.Value;
						xml.Add(nodeName, nodeValue);
					}
				}
				return xml;

			} catch (Exception ex) {
				return null;
			}
		}

		string GenProductArgs(int nmany_yuan)
		{
			var xml = new StringBuilder();
			var many_fen = nmany_yuan * 100;
			try {
				var nonceStr = GenNonceStr();

				xml.Append("</xml>");

				var packageParams = new List<INameValuePair>();

				packageParams.Add(new BasicNameValuePair("appid", WXConfig.APPID));
				//产品的名字
				packageParams.Add(new BasicNameValuePair("body", "weixin"));
				packageParams.Add(new BasicNameValuePair("mch_id", WXConfig.MCHID));
				//随机字符串
				packageParams.Add(new BasicNameValuePair("nonce_str", nonceStr));
				//通知地址，支付成功后通知我们的服务器完成业务逻辑
				packageParams.Add(new BasicNameValuePair("notify_url", "http://121.40.35.3/test"));
				//订单号
				packageParams.Add(new BasicNameValuePair("out_trade_no", GenOutTradeNo()));
				packageParams.Add(new BasicNameValuePair("spbill_create_ip", "127.0.0.1"));
				//金额，单位是分
				packageParams.Add(new BasicNameValuePair("total_fee", many_fen.ToString()));
				//交易类型
				packageParams.Add(new BasicNameValuePair("trade_type", "APP"));

				string sign = GenPackageSign(packageParams);
				packageParams.Add(new BasicNameValuePair("sign", sign));

				string xmlstring = ToXml(packageParams);

				return xmlstring;
			} catch (Exception ex) {
				return null;
			}
		}


		private string ALGetPayInfo(String orderInfo)
		{

			// 对订单做RSA 签名
			String sign = SignUtils.sign(orderInfo, ALPaymentConfig.RSA_PRIVATE);
			if (sign != null) {
				try {

					sign = URLEncoder.Encode(sign, "UTF-8");
				} catch (UnsupportedEncodingException e) {
					e.PrintStackTrace();
				}	
			}
			// 完整的符合支付宝参数规范的订单信息
			String payInfo = orderInfo + "&sign=\"" + sign + "\"&" + getSignType();


			return payInfo;

		}

		private   String getSignType()
		{
			return "sign_type=\"RSA\"";
		}

		string GenNonceStr()
		{
			var random = new System.Random();
			//			return MD5.GetMessageDigest(Encoding.UTF8.GetBytes("1234"));
			return MD5.GetMessageDigest(Encoding.UTF8.GetBytes(random.Next(10000).ToString()));
		}

		string GenOutTradeNo()
		{
			var random = new System.Random();
			//			return MD5.GetMessageDigest(Encoding.UTF8.GetBytes("1234"));
			return MD5.GetMessageDigest(Encoding.UTF8.GetBytes(random.Next(10000).ToString()));
		}

		string GenPackageSign(List<INameValuePair> parms)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < parms.Count; i++) {
				sb.Append(parms[i].Name);
				sb.Append('=');
				sb.Append(parms[i].Value);
				sb.Append('&');
			}
			sb.Append("key=");
			sb.Append(WXConfig.APIKey);


			string packageSign = MD5.GetMessageDigest(Encoding.UTF8.GetBytes(sb.ToString())).ToUpper();
			return packageSign;
		}

		string ToXml(List<INameValuePair> parms)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<xml>");
			for (int i = 0; i < parms.Count; i++) {
				sb.Append("<" + parms[i].Name + ">");


				sb.Append(parms[i].Value);
				sb.Append("</" + parms[i].Name + ">");
			}
			sb.Append("</xml>");

			return sb.ToString();
		}

		private void Simpleer(string title, string message)
		{
			AlertDialog.Builder buider = new AlertDialog.Builder(this);
			buider.SetTitle(title);
			buider.SetMessage(message);
			buider.SetPositiveButton(Resource.String.concel, ((sender, e) => {

			}));
			buider.SetNegativeButton(Resource.String.confirm, ((sender, e) => {
				StartActivity(new Intent(Android.Provider.Settings.ActionWirelessSettings));	
			}));
			buider.Create();
			buider.Show();

		}
	}
}

