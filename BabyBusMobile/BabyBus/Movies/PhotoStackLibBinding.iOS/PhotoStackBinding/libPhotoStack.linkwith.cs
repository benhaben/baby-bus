using System;
using ObjCRuntime;

//[assembly: LinkWith ("libPhotoStack.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s, ForceLoad = true)]
[assembly: LinkWith("libPhotoStack.a"
    , LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64
	, ForceLoad = true
//	, Frameworks = "Foundation CoreGraphics UIKit"
//	, LinkerFlags = "-ObjC"
//	, SmartLink = true
)]