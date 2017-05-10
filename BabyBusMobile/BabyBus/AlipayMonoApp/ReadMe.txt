1、在支付宝账号里面配置公钥（https://uemprod.alipay.com/user/ihome.htm）
我的支付宝-》签约订单-》查看PID|Key(输入支付宝支付密码)—>无线产品密钥管理（wap专用）->设置<RSA加密>

2、设置MainActivity.cs里面的四个参数
//商户PID	public const string PARTNER = ""; 首先说一下就是支付宝的支付是这样的，企业用户申请了支付宝之后，支付宝就会提供一个合作者id，就是所谓的pid，是2088开头的16位纯数字；
//商户收款账号	public const string SELLER = "";   这个就是你用于收款用的支付宝账号，要跟申请时候同一个
//商户私钥，pkcs8格式	public const string RSA_PRIVATE = "";
//支付宝公钥	public const string RSA_PUBLIC = "";

PS：商户私钥，pkcs8格式和支付宝公钥产生的方法，使用openssl.zip。
<1>、找到文件openssl.zip，解压，在目录\openssl\bin文件夹下面的openssl.exe生成，这个是由支付宝提供的。
<2>、RSA私钥：genrsa -out rsa_private_key.pem 1024   运行完命令行可以看到bin文件夹下面会生成私钥
<3>、RSA公钥：rsa -in rsa_private_key.pem -pubout -out rsa_public_key.pem    运行完命令行后可以看到生成了公钥
<4>、PKCS8编码的私钥：pkcs8 -topk8 -inform PEM -in rsa_private_key.pem -outform PEM -nocrypt    运行完后，将生成的东西拷贝下来放到文本文件里面就行。这后面要用到的！！包括begin跟end那两句一起存起来