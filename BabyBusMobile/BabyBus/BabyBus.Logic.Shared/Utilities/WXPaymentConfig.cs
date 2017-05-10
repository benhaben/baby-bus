using System;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace BabyBus.Logic.Shared
{
	public class WXPaymentConfig
	{
		public WXPaymentConfig()
		{
		}

		public async Task<IDictionary<string, string>> GetOrderInfo(string orderNumber, float fee)
		{
			//1. Gen Pre PayId
			var wxResultUnifiedOrder = await GenPrePayId(orderNumber, fee);
			if (wxResultUnifiedOrder == null) {
				return null;
			}

			//2. Gen Gen Pay Sign
			var requestParams = GenPaySign(wxResultUnifiedOrder);
			return requestParams;

		}

		async Task<IDictionary<string, string>> GenPrePayId(string orderNumber, float fee)
		{
			string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
			string entity = GenProductArgs(orderNumber, fee);

			string content = await Utils.PaymentHttpPost(url, entity);
			return DecodeXml(content);
		}

		string GenProductArgs(string orderNumber, float fee)
		{
			var xml = new StringBuilder();

			try {
				var noncestr = GenNonceStr();
				xml.Append("</xml>");

				var packageParams = new Dictionary<string,string>();

				packageParams.Add("appid", Constants.APPID);
				//产品的名字
				packageParams.Add("body", "增值服务");
				packageParams.Add("mch_id", Constants.MCHID);
				//随机字符串
				packageParams.Add("nonce_str", noncestr);
				//通知地址，支付成功后通知我们的服务器完成业务逻辑
				packageParams.Add("notify_url", Constants.BaseApiUrl + "/wxnotify/");
				//订单号
				packageParams.Add("out_trade_no", orderNumber);
				packageParams.Add("spbill_create_ip", "127.0.0.1");
				//金额，单位是分
				packageParams.Add("total_fee", (fee * 100).ToString());
				//交易类型
				packageParams.Add("trade_type", "APP");

				string sign = GenPackageSign(packageParams);
				packageParams.Add("sign", sign);

				string xmlstring = ToXml(packageParams);

				return xmlstring;
			} catch (Exception ex) {
				return null;
			}
		}

		string GenPackageSign(Dictionary<string,string> parms)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var key in parms.Keys) {
				sb.Append(key);
				sb.Append('=');
				sb.Append(parms[key]);
				sb.Append('&');
			}


			sb.Append("key=");
			sb.Append(Constants.APIKey);


			string packageSign = Utils.GetMd5String(Encoding.UTF8.GetBytes(sb.ToString())).ToUpper();
			return packageSign;
		}

		string GenNonceStr()
		{
			var random = new System.Random();
			return Utils.GetMd5String(Encoding.UTF8.GetBytes(random.Next(10000).ToString()));
		}

		uint GenTimeStamp()
		{
			var timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
			return Convert.ToUInt32(timestamp);
		}

		string ToXml(Dictionary<string,string> parms)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<xml>");
			foreach (var key in parms.Keys) {
				sb.Append("<" + key + ">");


				sb.Append(parms[key]);
				sb.Append("</" + key + ">");
			}

			sb.Append("</xml>");

			return sb.ToString();
		}

		IDictionary<string,string> DecodeXml(string content)
		{
			try {
				IDictionary<string, string> xml = new Dictionary<string, string>();

				//Use XmlReader to Parse Content
				var parser = XmlReader.Create(new StringReader(content));
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

		IDictionary<string,string> GenPaySign(IDictionary<string,string> resultUnifiedOrder)
		{
			if (!resultUnifiedOrder.ContainsKey("prepay_id")) {
				return null;
			}

			var requestParams = new Dictionary<string,string>();

			requestParams.Add("appid", Constants.APPID);
			requestParams.Add("noncestr", GenNonceStr());
			requestParams.Add("package", "Sign=WXPay");
			requestParams.Add("partnerid", Constants.MCHID);
			requestParams.Add("prepayid", resultUnifiedOrder["prepay_id"]);
			requestParams.Add("timestamp", GenTimeStamp().ToString());

			var sign = GenAppSign(requestParams);
			requestParams.Add("sign", sign);
			return requestParams;
		}

		string GenAppSign(Dictionary<string, string> parms)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var key in parms.Keys) {
				sb.Append(key);
				sb.Append('=');
				sb.Append(parms[key]);
				sb.Append('&');
			}

			sb.Append("key=");
			sb.Append(Constants.APIKey);

			//packagesb.Append("sign str\n" + sb + "\n\n");
			string appSign = Utils.GetMd5String(Encoding.UTF8.GetBytes(sb.ToString())).ToUpper();
			return appSign;
		}
	}


}

