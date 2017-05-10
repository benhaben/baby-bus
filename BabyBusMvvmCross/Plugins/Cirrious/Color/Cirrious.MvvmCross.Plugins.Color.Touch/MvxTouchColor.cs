// MvxTouchColor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.UI;
using UIKit;

namespace Cirrious.MvvmCross.Plugins.Color.Touch {
    public class MvxTouchColor : IMvxNativeColor {
        public object ToNative(MvxColor mvxColor) {
            return ToUIColor(mvxColor);
        }

        public static UIColor ToUIColor(MvxColor mvxColor) {
            return new UIKit.UIColor(mvxColor.R / 255.0f, mvxColor.G / 255.0f, mvxColor.B / 255.0f, mvxColor.A / 255.0f);
        }

        public static UIColor BrightGreen = ToUIColor(MvxColors.XFBrightGreen);

    
        public static UIColor Orange = ToUIColor(MvxColors.XFOrange);
        public static UIColor Green = ToUIColor(MvxColors.XFGreen);
        public static UIColor Red = ToUIColor(MvxColors.XFRed);
        public static UIColor LightRed = ToUIColor(MvxColors.XFLightRed);
        public static UIColor Blue = ToUIColor(MvxColors.XFBlue);
        public static UIColor Yellow = ToUIColor(MvxColors.XFYellow);
        public static UIColor White = ToUIColor(MvxColors.White);

        public static UIColor White1 = ToUIColor(MvxColors.XFWhite1);
        public static UIColor White2 = ToUIColor(MvxColors.XFWhite2);
        public static UIColor White3 = ToUIColor(MvxColors.XFWhite3);
        public static UIColor White4 = ToUIColor(MvxColors.XFWhite4);

        public static UIColor Gray1 = ToUIColor(MvxColors.XFGray1);
        public static UIColor Gray2 = ToUIColor(MvxColors.XFGray2);
        public static UIColor Gray3 = ToUIColor(MvxColors.XFGray3);

        public static UIColor Black1 = ToUIColor(MvxColors.XFBlack1);
       
        public static UIColor Orange1 = ToUIColor(MvxColors.XFOrange1);
        public static UIColor Orange2 = ToUIColor(MvxColors.XFOrange2);

        public static UIColor Blue1 = ToUIColor(MvxColors.XFBlue1);
        public static UIColor Blue2 = ToUIColor(MvxColors.XFBlue2);

        public static UIColor Lake1 = ToUIColor(MvxColors.XFLake1);
        public static UIColor Lake2 = ToUIColor(MvxColors.XFLake2);

        public static UIColor Green1 = ToUIColor(MvxColors.XFGreen1);
        public static UIColor Green2 = ToUIColor(MvxColors.XFGreen2);

        public static UIColor Yellow1 = ToUIColor(MvxColors.XFYellow1);
        public static UIColor Yellow2 = ToUIColor(MvxColors.XFYellow2);

        public static UIColor Red1 = ToUIColor(MvxColors.XFRed1);
        public static UIColor Red2 = ToUIColor(MvxColors.XFRed2);

        public static UIColor Purple1 = ToUIColor(MvxColors.XFPurple1);
        public static UIColor Purple2 = ToUIColor(MvxColors.XFPurple2);
    }
}