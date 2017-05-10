using System;
using ObjCRuntime;

//[assembly: LinkWith ("libWeChatSDK.a", LinkTarget.ArmV7 | LinkTarget.Simulator, SmartLink = true, ForceLoad = true)]
[assembly: LinkWith("libWeChatSDK.a", LinkTarget.Simulator | LinkTarget.ArmV7s | LinkTarget.ArmV7 | LinkTarget.Arm64, ForceLoad = true
	, Frameworks = "CFNetwork CoreFoundation CoreTelephony SystemConfiguration CoreGraphics Foundation UIKit Security"
	, IsCxx = true
	, LinkerFlags = "-ObjC -lc++ -lz -lsqlite3.0"
	, SmartLink = true
)]