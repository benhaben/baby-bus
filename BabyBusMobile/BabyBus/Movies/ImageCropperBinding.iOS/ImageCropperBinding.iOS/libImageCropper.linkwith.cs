using System;
using ObjCRuntime;

//[assembly: LinkWith ("libImageCropper.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s, ForceLoad = true)]
[assembly: LinkWith("libImageCropper.a", LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64
    , Frameworks = "Foundation CoreGraphics UIKit"
    , LinkerFlags = "-ObjC"
    , SmartLink = true)]