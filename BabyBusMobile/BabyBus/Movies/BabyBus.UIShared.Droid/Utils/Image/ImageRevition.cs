using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;

namespace BabyBus.Droid.Utils.Image
{
    public class ImageRevition
    {
        public static Bitmap RevitionImageSize(string path,int reqWidth,int reqHeight) {
            var option = new BitmapFactory.Options {
                InJustDecodeBounds = true,
            };
            using (Bitmap dispose = BitmapFactory.DecodeFile(path, option)) {
            }

            option.InSampleSize = CalculateInSampleSize(option,reqWidth,reqHeight);

            option.InJustDecodeBounds = false;
            option.InPurgeable = true;
            return BitmapFactory.DecodeFile(path, option);
        }

        private static int CalculateInSampleSize(BitmapFactory.Options option,int reqWidth,int reqHeight) {
            var height = (float)option.OutHeight;
            var width = (float)option.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth) {
                inSampleSize = width > height ? height / reqHeight : width / reqWidth;
            }
            return (int)inSampleSize;
        }
    }
}
