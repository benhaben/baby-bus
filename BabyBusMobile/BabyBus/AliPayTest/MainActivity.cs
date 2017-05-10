using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Alipay.Sdk.App;
using Android.Support.V4.App;
using Java.Lang;
using AlipayApp.AlipayAPI;
using AliPayTest;

namespace AlipayApp
{
    [Activity(Label = "支付宝Mono", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : FragmentActivity
    {
        //商户PID
        public const string PARTNER = "2088712736582414";
        //商户收款账号
        public const string SELLER = "it@mreliable.com";
        //商户私钥，pkcs8格式
        public const string RSA_PRIVATE = "MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBANvUsiE8DQEoLEIS\n" +
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
        //支付宝公钥
        public const string RSA_PUBLIC = "";

        private const int SDK_PAY_FLAG = 1;
        //支付
        private const int SDK_CHECK_FLAG = 2;
        //检查账户
        private Handler mHandler { get; set; }
        //Handler对象用作发送和接收消息

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.pay_main);

            //支付
            var pay = FindViewById<Button>(Resource.Id.pay);
            pay.Click += ConfirmAliPayBtn;

            //检查账户
            var check = FindViewById<Button>(Resource.Id.check);
            check.Click += ConfirmcheckAliPayBtn;

            #region 接收线程回复然后判断是否成功
            mHandler = new Handler((msg) =>
                {
                    switch (msg.What)
                    {
                        case SDK_PAY_FLAG:
                            {
                                PayResult payResult = new PayResult((string)msg.Obj);
                                // 支付宝返回此次支付结果及加签，建议对支付宝签名信息拿签约时支付宝提供的公钥做验签
                                string resultInfo = payResult.getResult();
                                string resultStatus = payResult.getResultStatus();

                                // 判断resultStatus 为“9000”则代表支付成功，具体状态码代表含义可参考接口文档
                                if (Android.Text.TextUtils.Equals(resultStatus, "9000"))
                                {
                                    Toast.MakeText(this, "支付成功", ToastLength.Short).Show();
                                }
                                else
                                {
                                    // 判断resultStatus 为非“9000”则代表可能支付失败
                                    // “8000”代表支付结果因为支付渠道原因或者系统原因还在等待支付结果确认，最终交易是否成功以服务端异步通知为准（小概率状态）
                                    if (Android.Text.TextUtils.Equals(resultStatus, "8000"))
                                    {
                                        Toast.MakeText(this, "支付结果确认中", ToastLength.Short).Show();
                                    }
                                    else
                                    {
                                        // 其他值就可以判断为支付失败，包括用户主动取消支付，或者系统返回的错误
                                        Toast.MakeText(this, "支付失败", ToastLength.Short).Show();
                                    }
                                }
                                break;
                            }
                        case SDK_CHECK_FLAG:
                            {
                                Toast.MakeText(this, "检查结果为：" + msg.Obj, ToastLength.Short).Show();
                                break;
                            }
                        default:
                            break;
                    }
                }
            );
            #endregion
        }

        //支付
        void ConfirmAliPayBtn(object sender, EventArgs e)
        {
            // 订单
            string orderInfo = getOrderInfo("测试的商品", "该测试商品的详细描述", "0.01");
            // 对订单做RSA 签名
            string sign = signRsa(orderInfo);
            try
            {
                // 仅需对sign 做URL编码
                sign = Java.Net.URLEncoder.Encode(sign, "UTF-8");
            }
            catch (Java.Lang.Exception eex)
            {
                eex.StackTrace.ToString();
            }
            // 完整的符合支付宝参数规范的订单信息
            string payInfo = orderInfo + "&sign=\"" + sign + "\"&" + getSignType();
            var payRunnable = new Runnable(() =>
                {
                    var alipay = new PayTask(this);
                    var result = alipay.Pay(payInfo);
                    var msg = new Message();
                    msg.What = SDK_PAY_FLAG;
                    msg.Obj = result;
                    mHandler.SendMessage(msg);
                });
            var payThread = new Thread(payRunnable);
            payThread.Start();
        }

        //检查账户
        void ConfirmcheckAliPayBtn(object sender, EventArgs e)
        {
            var checkAlipayRunnable = new Runnable(() =>
                {
                    var payTask = new PayTask(this);
                    var isExist = payTask.CheckAccountIfExist();

                    var msg = new Message();
                    msg.What = SDK_CHECK_FLAG;
                    msg.Obj = isExist;
                    mHandler.SendMessage(msg);
                });
            var payThread = new Thread(checkAlipayRunnable);
            payThread.Start();
        }

        //获取SDK版本
        public void getSDKVersion()
        {
            PayTask payTask = new PayTask(this);
            string version = payTask.Version;
            Toast.MakeText(this, version, ToastLength.Short).Show();
        }

        //创建订单信息
        public string getOrderInfo(string subject, string body, string price)
        {
            // 签约合作者身份ID
            string orderInfo = "partner=" + "\"" + PARTNER + "\"";
            // 签约卖家支付宝账号
            orderInfo += "&seller_id=" + "\"" + SELLER + "\"";
            // 商户网站唯一订单号
            orderInfo += "&out_trade_no=" + "\"" + getOutTradeNo() + "\"";
            // 商品名称
            orderInfo += "&subject=" + "\"" + subject + "\"";
            // 商品详情
            orderInfo += "&body=" + "\"" + body + "\"";
            // 商品金额
            orderInfo += "&total_fee=" + "\"" + price + "\"";
            // 服务器异步通知页面路径
            orderInfo += "&notify_url=" + "\"" + "http://notify.msp.hk/notify.htm" + "\"";
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
            // orderInfo += "&extern_token=" + "\"" + extern_token + "\"";
            // 支付宝处理完请求后，当前页面跳转到商户指定页面的路径，可空
            orderInfo += "&return_url=\"m.alipay.com\"";
            // 调用银行卡支付，需配置此参数，参与签名， 固定值 （需要签约《无线银行卡快捷支付》才能褂茫ㄉ            // orderInfo += "&paymethod=\"expressGateway\"";
            return orderInfo;
        }

        //生成商户订单号，该值在商户端应保持唯一（可自定义格式规范）
        public string getOutTradeNo()
        {
            Java.Text.SimpleDateFormat format = new Java.Text.SimpleDateFormat("MMddHHmmss", Java.Util.Locale.Default);
            Java.Util.Date date = new Java.Util.Date();
            string key = format.Format(date);
            Java.Util.Random r = new Java.Util.Random();
            key = key + r.NextInt();
            key = key.Substring(0, 15);
            return key;
        }

        //对订单信息进行签名   待签名订单信息
        public string signRsa(string content)
        {
            return SignUtils.sign(content, RSA_PRIVATE);
        }

        //获取签名方式
        public string getSignType()
        {
            return "sign_type=\"RSA\"";
        }
    }
}