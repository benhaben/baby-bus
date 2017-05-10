using System;
using ObjCRuntime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

//[assembly: LinkWith ("libPushSDK.a", LinkTarget.ArmV7s | LinkTarget.ArmV7 | LinkTarget.Simulator, ForceLoad = true)]

[assembly: LinkWith("libPushSDK-1.8.3.a", LinkTarget.Simulator | LinkTarget.ArmV7s | LinkTarget.ArmV7 | LinkTarget.Arm64, ForceLoad = true
    , Frameworks = "CFNetwork CoreFoundation CoreTelephony SystemConfiguration CoreGraphics Foundation UIKit Security"
    , IsCxx = true
    , LinkerFlags = "-ObjC -lc++"
    , SmartLink = true
)]
//[assembly: DllImport ("libz.dylib")]

//[LinkWith(SmartLink = true)]

//[assembly: LinkWith ("libSwiperAPI.a", IsCxx = true,
//  LinkTarget = LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s,
//  ForceLoad = true, Frameworks = "CoreAudio AudioToolbox AVFoundation MediaPlayer",
//  LinkerFlags = "-ObjC -lc++" )]
//CoreFoundation CoreTelephony SystemConfiguration CoreGraphics Foundation UIKit Security