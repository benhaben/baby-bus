using System;
using ObjCRuntime;

[assembly: LinkWith("libMWPhotoBrowser.a", 
    LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.Arm64, 
    LinkerFlags = "-ObjC -fobjc-arc",
    Frameworks = "AssetsLibrary CoreGraphics Foundation ImageIO MediaPlayer QuartzCore",
    SmartLink = true, ForceLoad = true)]
