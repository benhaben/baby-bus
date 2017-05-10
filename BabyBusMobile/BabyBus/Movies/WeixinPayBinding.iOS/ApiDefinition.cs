using System;
using System.Drawing;
using ObjCRuntime;
using Foundation;
using UIKit;


namespace WeixinPayBinding.iOS
{
	// The first step to creating a binding is to add your native library ("libNativeLibrary.a")
	// to the project by right-clicking (or Control-clicking) the folder containing this source
	// file and clicking "Add files..." and then simply select the native library (or libraries)
	// that you want to bind.
	//
	// When you do that, you'll notice that MonoDevelop generates a code-behind file for each
	// native library which will contain a [LinkWith] attribute. MonoDevelop auto-detects the
	// architectures that the native library supports and fills in that information for you,
	// however, it cannot auto-detect any Frameworks or other system libraries that the
	// native library may depend on, so you'll need to fill in that information yourself.
	//
	// Once you've done that, you're ready to move on to binding the API...
	//
	//
	// Here is where you'd define your API definition for the native Objective-C library.
	//
	// For example, to bind the following Objective-C class:
	//
	//     @interface Widget : NSObject {
	//     }
	//
	// The C# binding would look like this:
	//
	//     [BaseType (typeof (NSObject))]
	//     interface Widget {
	//     }
	//
	// To bind Objective-C properties, such as:
	//
	//     @property (nonatomic, readwrite, assign) CGPoint center;
	//
	// You would add a property definition in the C# interface like so:
	//
	//     [Export ("center")]
	//     PointF Center { get; set; }
	//
	// To bind an Objective-C method, such as:
	//
	//     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
	//
	// You would add a method definition to the C# interface like so:
	//
	//     [Export ("doSomething:atIndex:")]
	//     void DoSomething (NSObject object, int index);
	//
	// Objective-C "constructors" such as:
	//
	//     -(id)initWithElmo:(ElmoMuppet *)elmo;
	//
	// Can be bound as:
	//
	//     [Export ("initWithElmo:")]
	//     IntPtr Constructor (ElmoMuppet elmo);
	//
	// For more information, see http://developer.xamarin.com/guides/ios/advanced_topics/binding_objective-c/
	//

	// @interface BaseReq : NSObject
	[BaseType(typeof(NSObject))]
	interface BaseReq
	{

		// @property (assign, nonatomic) int type;
		[Export("type", ArgumentSemantic.UnsafeUnretained)]
		int Type { get; set; }

		// @property (retain, nonatomic) NSString * openID;
		[Export("openID", ArgumentSemantic.Retain)]
		string OpenID { get; set; }
	}

	// @interface BaseResp : NSObject
	[BaseType(typeof(NSObject))]
	interface BaseResp
	{

		// @property (assign, nonatomic) int errCode;
		[Export("errCode", ArgumentSemantic.UnsafeUnretained)]
		int ErrCode { get; set; }

		// @property (retain, nonatomic) NSString * errStr;
		[Export("errStr", ArgumentSemantic.Retain)]
		string ErrStr { get; set; }

		// @property (assign, nonatomic) int type;
		[Export("type", ArgumentSemantic.UnsafeUnretained)]
		int Type { get; set; }
	}

	// @interface PayReq : BaseReq
	[BaseType(typeof(BaseReq))]
	interface PayReq
	{

		// @property (retain, nonatomic) NSString * partnerId;
		[Export("partnerId", ArgumentSemantic.Retain)]
		string PartnerId { get; set; }

		// @property (retain, nonatomic) NSString * prepayId;
		[Export("prepayId", ArgumentSemantic.Retain)]
		string PrepayId { get; set; }

		// @property (retain, nonatomic) NSString * nonceStr;
		[Export("nonceStr", ArgumentSemantic.Retain)]
		string NonceStr { get; set; }

		// @property (assign, nonatomic) UInt32 timeStamp;
		[Export("timeStamp", ArgumentSemantic.UnsafeUnretained)]
		uint TimeStamp { get; set; }

		// @property (retain, nonatomic) NSString * package;
		[Export("package", ArgumentSemantic.Retain)]
		string Package { get; set; }

		// @property (retain, nonatomic) NSString * sign;
		[Export("sign", ArgumentSemantic.Retain)]
		string Sign { get; set; }
	}

	// @interface PayResp : BaseResp
	[BaseType(typeof(BaseResp))]
	interface PayResp
	{

		// @property (retain, nonatomic) NSString * returnKey;
		[Export("returnKey", ArgumentSemantic.Retain)]
		string ReturnKey { get; set; }
	}

	// @interface SendAuthReq : BaseReq
	[BaseType(typeof(BaseReq))]
	interface SendAuthReq
	{

		// @property (retain, nonatomic) NSString * scope;
		[Export("scope", ArgumentSemantic.Retain)]
		string Scope { get; set; }

		// @property (retain, nonatomic) NSString * state;
		[Export("state", ArgumentSemantic.Retain)]
		string State { get; set; }
	}

	// @interface SendAuthResp : BaseResp
	[BaseType(typeof(BaseResp))]
	interface SendAuthResp
	{

		// @property (retain, nonatomic) NSString * code;
		[Export("code", ArgumentSemantic.Retain)]
		string Code { get; set; }

		// @property (retain, nonatomic) NSString * state;
		[Export("state", ArgumentSemantic.Retain)]
		string State { get; set; }

		// @property (retain, nonatomic) NSString * lang;
		[Export("lang", ArgumentSemantic.Retain)]
		string Lang { get; set; }

		// @property (retain, nonatomic) NSString * country;
		[Export("country", ArgumentSemantic.Retain)]
		string Country { get; set; }
	}

	// @interface SendMessageToWXReq : BaseReq
	[BaseType(typeof(BaseReq))]
	interface SendMessageToWXReq
	{

		// @property (retain, nonatomic) NSString * text;
		[Export("text", ArgumentSemantic.Retain)]
		string Text { get; set; }

		// @property (retain, nonatomic) WXMediaMessage * message;
		[Export("message", ArgumentSemantic.Retain)]
		WXMediaMessage Message { get; set; }

		// @property (assign, nonatomic) BOOL bText;
		[Export("bText", ArgumentSemantic.UnsafeUnretained)]
		bool BText { get; set; }

		// @property (assign, nonatomic) int scene;
		[Export("scene", ArgumentSemantic.UnsafeUnretained)]
		int Scene { get; set; }
	}

	// @interface SendMessageToWXResp : BaseResp
	[BaseType(typeof(BaseResp))]
	interface SendMessageToWXResp
	{

		// @property (retain, nonatomic) NSString * lang;
		[Export("lang", ArgumentSemantic.Retain)]
		string Lang { get; set; }

		// @property (retain, nonatomic) NSString * country;
		[Export("country", ArgumentSemantic.Retain)]
		string Country { get; set; }
	}

	// @interface GetMessageFromWXReq : BaseReq
	[BaseType(typeof(BaseReq))]
	interface GetMessageFromWXReq
	{

		// @property (retain, nonatomic) NSString * lang;
		[Export("lang", ArgumentSemantic.Retain)]
		string Lang { get; set; }

		// @property (retain, nonatomic) NSString * country;
		[Export("country", ArgumentSemantic.Retain)]
		string Country { get; set; }
	}

	// @interface GetMessageFromWXResp : BaseResp
	[BaseType(typeof(BaseResp))]
	interface GetMessageFromWXResp
	{

		// @property (retain, nonatomic) NSString * text;
		[Export("text", ArgumentSemantic.Retain)]
		string Text { get; set; }

		// @property (retain, nonatomic) WXMediaMessage * message;
		[Export("message", ArgumentSemantic.Retain)]
		WXMediaMessage Message { get; set; }

		// @property (assign, nonatomic) BOOL bText;
		[Export("bText", ArgumentSemantic.UnsafeUnretained)]
		bool BText { get; set; }
	}

	// @interface ShowMessageFromWXReq : BaseReq
	[BaseType(typeof(BaseReq))]
	interface ShowMessageFromWXReq
	{

		// @property (retain, nonatomic) WXMediaMessage * message;
		[Export("message", ArgumentSemantic.Retain)]
		WXMediaMessage Message { get; set; }

		// @property (retain, nonatomic) NSString * lang;
		[Export("lang", ArgumentSemantic.Retain)]
		string Lang { get; set; }

		// @property (retain, nonatomic) NSString * country;
		[Export("country", ArgumentSemantic.Retain)]
		string Country { get; set; }
	}

	// @interface ShowMessageFromWXResp : BaseResp
	[BaseType(typeof(BaseResp))]
	interface ShowMessageFromWXResp
	{

	}

	// @interface LaunchFromWXReq : BaseReq
	[BaseType(typeof(BaseReq))]
	interface LaunchFromWXReq
	{

		// @property (retain, nonatomic) WXMediaMessage * message;
		[Export("message", ArgumentSemantic.Retain)]
		WXMediaMessage Message { get; set; }

		// @property (retain, nonatomic) NSString * lang;
		[Export("lang", ArgumentSemantic.Retain)]
		string Lang { get; set; }

		// @property (retain, nonatomic) NSString * country;
		[Export("country", ArgumentSemantic.Retain)]
		string Country { get; set; }
	}

	// @interface JumpToBizProfileReq : BaseReq
	[BaseType(typeof(BaseReq))]
	interface JumpToBizProfileReq
	{

		// @property (retain, nonatomic) NSString * username;
		[Export("username", ArgumentSemantic.Retain)]
		string Username { get; set; }

		// @property (retain, nonatomic) NSString * extMsg;
		[Export("extMsg", ArgumentSemantic.Retain)]
		string ExtMsg { get; set; }

		// @property (assign, nonatomic) int profileType;
		[Export("profileType", ArgumentSemantic.UnsafeUnretained)]
		int ProfileType { get; set; }
	}

	// @interface JumpToBizWebviewReq : BaseReq
	[BaseType(typeof(BaseReq))]
	interface JumpToBizWebviewReq
	{

		// @property (assign, nonatomic) int webType;
		[Export("webType", ArgumentSemantic.UnsafeUnretained)]
		int WebType { get; set; }

		// @property (retain, nonatomic) NSString * tousrname;
		[Export("tousrname", ArgumentSemantic.Retain)]
		string Tousrname { get; set; }

		// @property (retain, nonatomic) NSString * extMsg;
		[Export("extMsg", ArgumentSemantic.Retain)]
		string ExtMsg { get; set; }
	}

	// @interface WXCardItem : NSObject
	[BaseType(typeof(NSObject))]
	interface WXCardItem
	{

		// @property (retain, nonatomic) NSString * cardId;
		[Export("cardId", ArgumentSemantic.Retain)]
		string CardId { get; set; }

		// @property (retain, nonatomic) NSString * extMsg;
		[Export("extMsg", ArgumentSemantic.Retain)]
		string ExtMsg { get; set; }

		// @property (assign, nonatomic) UInt32 cardState;
		[Export("cardState", ArgumentSemantic.UnsafeUnretained)]
		uint CardState { get; set; }
	}

	// @interface AddCardToWXCardPackageReq : BaseReq
	[BaseType(typeof(BaseReq))]
	interface AddCardToWXCardPackageReq
	{

		// @property (retain, nonatomic) NSArray * cardAry;
		[Export("cardAry", ArgumentSemantic.Retain)]
		NSObject [] CardAry { get; set; }
	}

	// @interface AddCardToWXCardPackageResp : BaseResp
	[BaseType(typeof(BaseResp))]
	interface AddCardToWXCardPackageResp
	{

		// @property (retain, nonatomic) NSArray * cardAry;
		[Export("cardAry", ArgumentSemantic.Retain)]
		NSObject [] CardAry { get; set; }
	}

	// @interface WXMediaMessage : NSObject
	[BaseType(typeof(NSObject))]
	interface WXMediaMessage
	{

		// @property (retain, nonatomic) NSString * title;
		[Export("title", ArgumentSemantic.Retain)]
		string Title { get; set; }

		// @property (retain, nonatomic) NSString * description;
		[Export("description", ArgumentSemantic.Retain)]
		string Description { get; set; }

		// @property (retain, nonatomic) NSData * thumbData;
		[Export("thumbData", ArgumentSemantic.Retain)]
		NSData ThumbData { get; set; }

		// @property (retain, nonatomic) NSString * mediaTagName;
		[Export("mediaTagName", ArgumentSemantic.Retain)]
		string MediaTagName { get; set; }

		// @property (retain, nonatomic) NSString * messageExt;
		[Export("messageExt", ArgumentSemantic.Retain)]
		string MessageExt { get; set; }

		// @property (retain, nonatomic) NSString * messageAction;
		[Export("messageAction", ArgumentSemantic.Retain)]
		string MessageAction { get; set; }

		// @property (retain, nonatomic) id mediaObject;
		[Export("mediaObject", ArgumentSemantic.Retain)]
		NSObject MediaObject { get; set; }

		// +(WXMediaMessage *)message;
		[Static, Export("message")]
		WXMediaMessage Message();

		// -(void)setThumbImage:(UIImage *)image;
		[Export("setThumbImage:")]
		void SetThumbImage(UIImage image);
	}

	// @interface WXImageObject : NSObject
	[BaseType(typeof(NSObject))]
	interface WXImageObject
	{

		// @property (retain, nonatomic) NSData * imageData;
		[Export("imageData", ArgumentSemantic.Retain)]
		NSData ImageData { get; set; }

		// @property (retain, nonatomic) NSString * imageUrl;
		[Export("imageUrl", ArgumentSemantic.Retain)]
		string ImageUrl { get; set; }

		// +(WXImageObject *)object;
		[Static, Export("object")]
		WXImageObject Object();
	}

	// @interface WXMusicObject : NSObject
	[BaseType(typeof(NSObject))]
	interface WXMusicObject
	{

		// @property (retain, nonatomic) NSString * musicUrl;
		[Export("musicUrl", ArgumentSemantic.Retain)]
		string MusicUrl { get; set; }

		// @property (retain, nonatomic) NSString * musicLowBandUrl;
		[Export("musicLowBandUrl", ArgumentSemantic.Retain)]
		string MusicLowBandUrl { get; set; }

		// @property (retain, nonatomic) NSString * musicDataUrl;
		[Export("musicDataUrl", ArgumentSemantic.Retain)]
		string MusicDataUrl { get; set; }

		// @property (retain, nonatomic) NSString * musicLowBandDataUrl;
		[Export("musicLowBandDataUrl", ArgumentSemantic.Retain)]
		string MusicLowBandDataUrl { get; set; }

		// +(WXMusicObject *)object;
		[Static, Export("object")]
		WXMusicObject Object();
	}

	// @interface WXVideoObject : NSObject
	[BaseType(typeof(NSObject))]
	interface WXVideoObject
	{

		// @property (retain, nonatomic) NSString * videoUrl;
		[Export("videoUrl", ArgumentSemantic.Retain)]
		string VideoUrl { get; set; }

		// @property (retain, nonatomic) NSString * videoLowBandUrl;
		[Export("videoLowBandUrl", ArgumentSemantic.Retain)]
		string VideoLowBandUrl { get; set; }

		// +(WXVideoObject *)object;
		[Static, Export("object")]
		WXVideoObject Object();
	}

	// @interface WXWebpageObject : NSObject
	[BaseType(typeof(NSObject))]
	interface WXWebpageObject
	{

		// @property (retain, nonatomic) NSString * webpageUrl;
		[Export("webpageUrl", ArgumentSemantic.Retain)]
		string WebpageUrl { get; set; }

		// +(WXWebpageObject *)object;
		[Static, Export("object")]
		WXWebpageObject Object();
	}

	// @interface WXAppExtendObject : NSObject
	[BaseType(typeof(NSObject))]
	interface WXAppExtendObject
	{

		// @property (retain, nonatomic) NSString * url;
		[Export("url", ArgumentSemantic.Retain)]
		string Url { get; set; }

		// @property (retain, nonatomic) NSString * extInfo;
		[Export("extInfo", ArgumentSemantic.Retain)]
		string ExtInfo { get; set; }

		// @property (retain, nonatomic) NSData * fileData;
		[Export("fileData", ArgumentSemantic.Retain)]
		NSData FileData { get; set; }

		// +(WXAppExtendObject *)object;
		[Static, Export("object")]
		WXAppExtendObject Object();
	}

	// @interface WXEmoticonObject : NSObject
	[BaseType(typeof(NSObject))]
	interface WXEmoticonObject
	{

		// @property (retain, nonatomic) NSData * emoticonData;
		[Export("emoticonData", ArgumentSemantic.Retain)]
		NSData EmoticonData { get; set; }

		// +(WXEmoticonObject *)object;
		[Static, Export("object")]
		WXEmoticonObject Object();
	}

	// @interface WXFileObject : NSObject
	[BaseType(typeof(NSObject))]
	interface WXFileObject
	{

		// @property (retain, nonatomic) NSString * fileExtension;
		[Export("fileExtension", ArgumentSemantic.Retain)]
		string FileExtension { get; set; }

		// @property (retain, nonatomic) NSData * fileData;
		[Export("fileData", ArgumentSemantic.Retain)]
		NSData FileData { get; set; }

		// +(WXFileObject *)object;
		[Static, Export("object")]
		WXFileObject Object();
	}

	// @protocol WXApiDelegate <NSObject>
	[BaseType(typeof(NSObject))]
	[Protocol]
	[Model]
	interface WXApiDelegate
	{

		// @optional -(void)onReq:(BaseReq *)req;
		[Export("onReq:")]
		void OnReq(BaseReq req);

		// @optional -(void)onResp:(BaseResp *)resp;
		[Export("onResp:")]
		void OnResp(BaseResp resp);
	}

	// @interface WXApi : NSObject
	[BaseType(typeof(NSObject))]
	interface WXApi
	{

		// +(BOOL)registerApp:(NSString *)appid;
		[Static, Export("registerApp:")]
		bool RegisterApp(string appid);

		// +(BOOL)registerApp:(NSString *)appid withDescription:(NSString *)appdesc;
		[Static, Export("registerApp:withDescription:")]
		bool RegisterApp(string appid, string appdesc);

		// +(BOOL)handleOpenURL:(NSURL *)url delegate:(id<WXApiDelegate>)delegate;
		[Static, Export("handleOpenURL:delegate:")]
		bool HandleOpenURL(NSUrl url, WXApiDelegate wxdelegate);

		// +(BOOL)isWXAppInstalled;
		[Static, Export("isWXAppInstalled")]
		bool IsWXAppInstalled();

		// +(BOOL)isWXAppSupportApi;
		[Static, Export("isWXAppSupportApi")]
		bool IsWXAppSupportApi();

		// +(NSString *)getWXAppInstallUrl;
		[Static, Export("getWXAppInstallUrl")]
		string GetWXAppInstallUrl();

		// +(NSString *)getApiVersion;
		[Static, Export("getApiVersion")]
		string GetApiVersion();

		// +(BOOL)openWXApp;
		[Static, Export("openWXApp")]
		bool OpenWXApp();

		// +(BOOL)sendReq:(BaseReq *)req;
		[Static, Export("sendReq:")]
		bool SendReq(BaseReq req);

		// +(BOOL)sendAuthReq:(SendAuthReq *)req viewController:(UIViewController *)viewController delegate:(id<WXApiDelegate>)delegate;
		[Static, Export("sendAuthReq:viewController:delegate:")]
		bool SendAuthReq(SendAuthReq req, UIViewController viewController, WXApiDelegate wxdelegate);

		// +(BOOL)sendResp:(BaseResp *)resp;
		[Static, Export("sendResp:")]
		bool SendResp(BaseResp resp);
	}
}

