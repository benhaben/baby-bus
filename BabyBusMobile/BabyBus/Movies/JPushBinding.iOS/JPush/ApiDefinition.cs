using System;
using System.Drawing;

using ObjCRuntime;
using Foundation;
using UIKit;

namespace JPushBinding {
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
    // For more information, see http://docs.xamarin.com/ios/advanced_topics/binding_objective-c_libraries
    //
  
   
    [BaseType(typeof(NSObject))]
    interface APService {
        //		#pragma - mark 基本功能
        //		// 以下四个接口是必须调用的
        //		+ (void)setupWithOption:(NSDictionary *)launchingOption;  // 初始化
        //		+ (void)registerForRemoteNotificationTypes:(NSUInteger)types
        //		categories:(NSSet *)categories;  // 注册APNS类型
        //		+ (void)registerDeviceToken:(NSData *)deviceToken;  // 向服务器上报Device Token
        //		+ (void)handleRemoteNotification:(NSDictionary *)
        //		remoteInfo;  // 处理收到的APNS消息，向服务器上报收到APNS消息

        [Static, Export("setupWithOption:")]
        void SetupWithOption([NullAllowed] NSDictionary launchingOption);

        [Static, Export("registerForRemoteNotificationTypes:categories:")]
        void RegisterForRemoteNotificationTypes(UIRemoteNotificationType types, [NullAllowed] NSSet categories);

        [Static, Export("registerDeviceToken:")]
        void RegisterDeviceToken(NSData deviceToken);

        [Static, Export("handleRemoteNotification:")]
        void HandleRemoteNotification(NSDictionary remoteInfo);

        [Static, Export("setTags:callbackSelector:object:")]
        void SetTags(NSSet tags, [NullAllowed] Selector cbSelector, [NullAllowed] NSObject theTarget);

        [Static, Export("setTags:alias:callbackSelector:target:")]
        void SetTagsAndAlias(NSSet tags, string alias, [NullAllowed] Selector cbSelector, [NullAllowed] NSObject theTarget);

        [Static, Export("setAlias:callbackSelector:object:")]
        void SetAlias(string alias, [NullAllowed] Selector cbSelector, [NullAllowed] NSObject theTarget);

        [Static, Export("resetBadge")]
        void ResetBadge();
    }
}

