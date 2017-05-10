using System;

namespace WeixinPayBinding.iOS
{
	public enum WXErrCode
	{
		WXSuccess = 0,
		Common = -1,
		UserCancel = -2,
		SentFail = -3,
		AuthDeny = -4,
		Unsupport = -5
	}

	public enum WXScene : uint
	{
		Session = 0,
		Timeline = 1,
		Favorite = 2
	}

	public enum WXAPISupport : uint
	{
		Session = 0
	}

	public enum WXBizProfileType : uint
	{
		_Normal = 0,
		_Device = 1
	}

	public enum WXMPWebviewType : uint
	{
		_Ad = 0
	}
}

