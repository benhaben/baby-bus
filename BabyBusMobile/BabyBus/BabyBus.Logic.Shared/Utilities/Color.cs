using System;

#if __IOS__
using UIKit;
using CoreGraphics;
#endif

namespace BabyBus.Logic.Shared
{
    public struct Color
    {
        public static readonly Color Purple = 0xB455B6;
        public static readonly Color Blue = 0x3498DB;
        public static readonly Color DarkBlue = 0x2C3E50;
        public static readonly Color Green = 0x77D065;
        public static readonly Color Gray = 0x738182;
        public static readonly Color LightGray = 0xB4BCBC;
        public static readonly Color Tan = 0xDAD0C8;
        public static readonly Color DarkGray = 0x333333;
        public static readonly Color Tint = 0x5AA09B;

        //晓夫设计颜色
        public static Color XFWhite0 = 0xfefefe;
        public static Color XFWhite1 = 0xf8f8f8;
        public static Color XFWhite2 = 0xe8e8e8;
        public static Color XFWhite3 = 0xdfdfdf;
        public static Color XFWhite4 = 0xd1d1d1;

        public static Color XFGray1 = 0x929292;
        public static Color XFGray2 = 0x838383;
        public static Color XFGray3 = 0xEEEEEF;

        public static Color XFBlack1 = 0x5f5e5e;

        public static Color XFOrange1 = 0xf29300;
        public static Color XFOrange2 = 0xd98400;

        public static Color XFBlue1 = 0x7db2d7;
        public static Color XFBlue2 = 0x70a0c1;

        public static Color XFLake1 = 0x66c39f;
        public static Color XFLake2 = 0x5cb08f;

        public static Color XFGreen1 = 0xbbdb83;
        public static Color XFGreen2 = 0xa9c476;

        public static Color XFYellow1 = 0xfdd800;
        public static Color XFYellow2 = 0xe6c300;

        public static Color XFRed1 = 0xe85a95;
        public static Color XFRed2 = 0xd24b4a;

        public static Color XFPurple1 = 0xa79ccb;
        public static Color XFPurple2 = 0x958bb6;

        public double R, G, B;

        public static Color FromHex(int hex)
        {
            Func<int, int> at = offset => (hex >> offset) & 0xFF;
            return new Color
            {
                R = at(16) / 255.0,
                G = at(8) / 255.0,
                B = at(0) / 255.0
            };
        }

        public static implicit operator Color(int hex)
        {
            return FromHex(hex);
        }

        #if __IOS__
        public UIColor ToUIColor()
        {
            return UIColor.FromRGB((float)R, (float)G, (float)B);
        }

        public static implicit operator UIColor(Color color)
        {
            return color.ToUIColor();
        }

        public static implicit operator CGColor(Color color)
        {
            return color.ToUIColor().CGColor;
        }
        
        #endif

        //        public Xamarin.Forms.Color ToFormsColor()
        //        {
        //            return Xamarin.Forms.Color.FromRgb((int)(255 * R), (int)(255 * G), (int)(255 * B));
        //        }


        #if __ANDROID__
        
		public global::Android.Graphics.Color ToAndroidColor() {
			return global::Android.Graphics.Color.Rgb((int)(255 * R), (int)(255 * G), (int)(255 * B));
		}

		public static implicit operator global::Android.Graphics.Color(Color color) {
			return color.ToAndroidColor();
		}
		#endif
    }
}
