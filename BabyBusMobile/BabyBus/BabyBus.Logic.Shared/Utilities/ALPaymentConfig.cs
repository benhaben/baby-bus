using System;

namespace BabyBus.Logic.Shared
{
	public class ALPaymentConfig
	{
		//商户PID
		public  static String PARTNER = "2088712736582414";
		//商户收款账号
		public  static String SELLER = "it@mreliable.com";
		//商户私钥，pkcs8格式
		public static  String RSA_PRIVATE = 
			"MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBANvUsiE8DQEoLEIS\n" +
			"XDJSy5mIvnr9YxWLoGTAYdydzHdEA4HSAoAC4f8tuf8erzQa1ZlxEkNqMb6r8TI5\n" +
			"QR72ScE2P0csatiwi5x5YZDqeXgFZovy8QuTn1OtD5EqPLOBuZZtCH+2Yz2mB45h\n" +
			"k5alBEGWFiVAYCan5wodttcQ6sNjAgMBAAECgYBG7vPni3P6ypa1Xy1Gw7aUvS4R\n" +
			"i4+cVSiVOgqZ4IUoetbS3gwWeFeqOnwI2ULZgksoLvcgr7SLfPngJd9geUJEr/4M\n" +
			"FIG/r81MhhQLXHjywHoDtgl8xOzE6up0ZTNCYvLFZVmHS0YSKjpEouXSFsaJDd5s\n" +
			"RkGQgZqntwE4ENTdcQJBAO7Poqc50rMuUcXbzihsbU4twX/muRez3yeYJqfcRbT7\n" +
			"QMAxUcL2Oks4BiKRpXxI9wVgtYWEK2wZxGKe5gYDIIsCQQDrp1QCq2XfYPGcTxYA\n" +
			"vih9nQxaNYSPfPrpM3ysDH4j16TCriIdJl7uVol6mT1bhQwq237BzD8uWbUI+42C\n" +
			"NSuJAkB419LbwhPPndG9SHPy2qMZG2g+G3dv+hIjDAgLixgu87EZUBuqh0SKSYg5\n" +
			"N/BAiv+M1hokvPPoGMXajcOiKTTvAkAjpeplhPwiMI4cMTKI5jtF1U4bD2GAO03R\n" +
			"nUJM3I7waRy5fpIWisltkJW3gBryD0xp505jjrw4DMYAF92uRtDRAkBkvMzsNPHJ\n" +
			"MJqu18JKsNO9BRDlk7NcpXseTwoau3beLnOWpodoiaSCPrvDgpLskLoNgvnH61nS\n" +
			"7amVpsEtNSb3";
		public static int SDK_PAY_FLAG = 1;

		public static int SDK_CHECK_FLAG = 2;



		public  String getOrderInfo(String subject, String body, String price, string ordernum)
		{
			// 签约合作者身份ID
			String orderInfo = "partner=" + "\"" + "2088712736582414" + "\"";

			// 签约卖家支付宝账号
			orderInfo += "&seller_id=" + "\"" + "it@mreliable.com" + "\"";

			// 商户网站唯一订单号
			//orderInfo += "&out_trade_no=" + "\"" + getOutTradeNo () + "\"";
			orderInfo += "&out_trade_no=" + "\"" + ordernum + "\"";

			// 商品名称
			orderInfo += "&subject=" + "\"" + subject + "\"";

			// 商品详情
			orderInfo += "&body=" + "\"" + body + "\"";

			// 商品金额
			orderInfo += "&total_fee=" + "\"" + price + "\"";

			// 服务器异步通知页面路径
			orderInfo += "&notify_url=" + "\"" + Constants.BaseApiUrl + "/alipay/" + "\"";

			// 服务接口名称， 固定值
			orderInfo += "&service=\"mobile.securitypay.pay\"";

			// 支付类型， 固定值
			orderInfo += "&payment_type=\"1\"";

			// 参数编码， 固定值
			orderInfo += "&_input_charset=\"utf-8\"";

			// 设置未付款交易的超时时间
			// 默认30分钟，一旦超时，该笔交易就会自动被关闭。
			// 取值范围：1m～15d。
			// m-分钟，h-小时，d-天，1c-当天（无论交易何时创建，都在0点关闭）。
			// 该参数数值不接受小数点，如1.5h，可转换为90m。
			orderInfo += "&it_b_pay=\"30m\"";

			// extern_token为经过快登授权获取到的alipay_open_id,带上此参数用户将使用授权的账户进行支付
			//orderInfo += "&extern_token=" + "\"" + extern_token + "\"";

			// 支付宝处理完请求后，当前页面跳转到商户指定页面的路径，可空
			orderInfo += "&return_url=\"m.alipay.com\"";

			// 调用银行卡支付，需配置此参数，参与签名， 固定值 （需要签约《无线银行卡快捷支付》才能使用）
			// orderInfo += "&paymethod=\"expressGateway\"";

			return orderInfo;
		}

		private  String Sign(String content)
		{
//			return SignUtils.sign(content, RSA_PRIVATE);
			return string.Empty;
		}

		public String getOutTradeNo()
		{
	
			String key = DateTime.Now.ToString("yyyymmmmdd");

			Random r = new Random();
			key = key + r.NextDouble().ToString();
			key = key.Substring(0, 15);
			return key;
		}
	}
}


