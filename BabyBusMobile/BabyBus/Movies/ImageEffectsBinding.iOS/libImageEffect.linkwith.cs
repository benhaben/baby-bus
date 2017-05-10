using System;
using ObjCRuntime;

[assembly: LinkWith("libImageEffect.a", 
    LinkTarget.Simulator | LinkTarget.ArmV7 | LinkTarget.ArmV7s 
    , Frameworks = "ImageIO CoreGraphics Accelerate"
    , ForceLoad = true)]
