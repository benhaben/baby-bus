using System;
using System.Collections.Generic;
using BabyBusSSApi.ServiceModel.Enumeration;



namespace BabyBus.Logic.Shared
{
	public static class Constants
	{
		public static string RoleTypeString(RoleType roleType)
		{
			if (roleType == RoleType.HeadMaster) {
				return Master;
			} else if (roleType == RoleType.Parent) {
				return Parent;
			} else if (roleType == RoleType.Teacher) {
				return Teacher;
			} else {
				return "Admin";
			}
		}

		public static string Parent = "Parent";
		public static string Teacher = "Teacher";
		public static string Master = "Master";
		public static string Anonymous = "匿名用户";


		#region Base

		#if DEBUG
		public const string BaseApiUrl = "http://115.28.2.41:8099/api";
		public const string BaseHtmlUrl = "http://115.28.2.41:8888/Pages";
		
		#else
		//public const string BaseUrl = "http://imreliable.net/api";
		public const string BaseApiUrl = "http://115.28.2.41:8099/api";
		public const string BaseHtmlUrl = "http://115.28.2.41:8888/Pages";
		#endif

		#endregion

		#region WX Pay

		//应用的APPID
		public static String APPID = "wxbafe0fadb82b35b5";
		//微信支付商户号对应的API Key
		public static String APIKey = "81dc9bdb52d04dc20036dbd8313edrnt";
		//微信支付商户号
		public static String MCHID = "1254794201";

		#endregion


		#region Null Value

		public const int IntNull = -999;
		public static readonly DateTime DateNull = DateTime.MinValue;
		public const string StringNull = null;

		#endregion

		#region Permission 暂时把权限硬编码，后面再挪

		#if __Android__
        static Constants()
        {
            RolePermissionMap.Add(RoleType.Parent
                , new List<string>{ "Setting_ChildClassInfo" });
            RolePermissionMap.Add(RoleType.Teacher
                , new List<string> { "Setting_ClassInfo" });
            RolePermissionMap.Add(RoleType.HeadMaster
                , new List<string> { "Setting_KGInfo" });
        }

        public static Dictionary<RoleType, List<String>> RolePermissionMap = new Dictionary<RoleType, List<string>>();
        #endif
		public const int PAGESIZE = 8;

		public const int STATSUPLOADTHRESHOLD = 20;

		#endregion



		#region OSS

		public const string PNGSuffix = ".png";
		public const string ImageServerPath = "http://babybus.emolbase.com/";
		public const string ThumbServerPath = "http://image.emolbase.com/";
		public const string ThumbRule = "@1e_80w_80h_1c_0i_1o_1x.png";
		public const string ThumbRule80_60 = "@1e_80w_60h_1c_0i_1o_1x.png";
		public const string AdvertiseIcon = "@1e_320w_123h_1c_0i_1o_1x.png";
		public const string ThumbRule40 = "@1e_40w_40h_1c_0i_1o_1x.png";
		public const string OSSKey = "oMhFxiUEplUV9xIt";
		public const string OSSSecret = "OZfbMNMOP8iHNJOvvZld1ZNFGcdijj";
		public const string BucketName = "babybus-image";
		public const string OSSEndPoint = "http://oss-cn-qingdao.aliyuncs.com";

		#endregion

		#region Constants value

		public const int MiniPasswordLength = 6;
		public const int RefreshTime = 1000;
		public const int ProgressLongTime = 10000;
		public const int MaxRetrySendImageTime = 3;
		public const int MaxContentLength = 3000;
		public const int MaxTitleLength = 100;
		public const double EPSILON = 0.0000001;

		#endregion


		#region CSS

		public const string CSS = @"html {
  background: transparent;
}

body {
  margin-bottom: 25px;
}

h1, h2 {
  line-height: 1;
  font-family: Microsoft YaHei,'ËÎÌå', Tahoma, Helvetica, Arial, ""\5b8b\4f53"", sans-serif;
  font-weight: bold;
  margin: 0 5px 25px 0;
  padding: 0 0 8px;
}

h1 {
  font-size: 27px;
}

h2 {
  font-size: 18px;
}

p, li {
  line-height: 1.5;
}

p, ul, ol {
  padding-bottom: 25px;
}

a {
  color: inherit;
}

strong, b {
  font-weight: bold;
}

i, em {
  font-style: italic;
}

li {
  margin-left: 30px;
}

ul li {
  list-style: disc outside;
}

ol li {
  list-style: decimal outside;
}

li img {
  vertical-align: middle;
  margin: 2px 5px 5px 0;
}

.wysiwyg-color-black {
  color: black;
}

.wysiwyg-color-silver {
  color: silver;
}

.wysiwyg-color-gray {
  color: gray;
}

.wysiwyg-color-white {
  color: white;
}

.wysiwyg-color-maroon {
  color: maroon;
}

.wysiwyg-color-red {
  color: red;
}

.wysiwyg-color-purple {
  color: purple;
}

.wysiwyg-color-fuchsia {
  color: fuchsia;
}

.wysiwyg-color-green {
  color: green;
}

.wysiwyg-color-lime {
  color: lime;
}

.wysiwyg-color-olive {
  color: olive;
}

.wysiwyg-color-yellow {
  color: yellow;
}

.wysiwyg-color-navy {
  color: navy;
}

.wysiwyg-color-blue {
  color: blue;
}

.wysiwyg-color-teal {
  color: teal;
}

.wysiwyg-color-aqua {
  color: aqua;
}";

		#endregion
	}

	public static class UIConstants
	{
		public const string WebEx_NameResolutionFailure = "您的网络异常，请稍后重试";
		public const string WebEx_ConnectFailure = "无法连接网络，请检查";
		public const string WebEx_Default = "您的网络异常，请稍后重试";

		public const float CornerRadius = 4;
		public const string RETURN_PREVIOUS_PAGE = "返回";

		public const string UPDATING = "正在更新...";
		public const string UPDATE_SUCCESS = "更新成功.";

		public const string LOADING = "正在加载...";
		public const string LOAD_SUCCESS = "加载成功.";

      
		public const string SENDING = "正在发送...";
		public const string SEND_SUCCESS = "发送成功.";
		public const string INVALID_USERNAME_OR_PASSWORD = "用户名或者密码错误.";


		public const string WEB_EXCEPTION = "网络中断或者长时间未登录！请尝试退出重新登录！";

		public const string PULOAD_ERROR = "加载信息失败";
	}

   

}
