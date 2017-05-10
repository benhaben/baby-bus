using System;
using ObjCRuntime;

//[assembly: LinkWith ("multipleImagePicker.a", LinkTarget.ArmV7, ForceLoad = true)]
[assembly: LinkWith ("libELCImagePicker.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64,
    Frameworks = "MobileCoreServices AssetsLibrary CoreGraphics Foundation UIKit"
	, LinkerFlags = "-ObjC"
	, SmartLink = true)]