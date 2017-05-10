using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.Security.Cryptography;

namespace Utilities.Touch
{
	public static class UtilitiesTouch
	{
		private static readonly MD5CryptoServiceProvider checksum = new MD5CryptoServiceProvider();

		private static int Hex(int v)
		{
			if (v < 10)
				return '0' + v;
			return 'a' + v - 10;
		}

      
		public static string GetMd5String(byte[] inputbytes)
		{
			var bytes = checksum.ComputeHash(inputbytes);
			var ret = new char[32];
			for (int i = 0; i < 16; i++) {
				ret[i * 2] = (char)Hex(bytes[i] >> 4);
				ret[i * 2 + 1] = (char)Hex(bytes[i] & 0xf);
			}
			return new string(ret);
		}

		public static DateTime NSDateToDateTime(this NSDate date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
				                              new DateTime(2001, 1, 1, 0, 0, 0));
			return reference.AddSeconds(date.SecondsSinceReferenceDate);
		}

		public static NSDate DateTimeToNSDate(this DateTime date)
		{
			DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
				                              new DateTime(2001, 1, 1, 0, 0, 0));
			return NSDate.FromTimeIntervalSinceReferenceDate(
				(date - reference).TotalSeconds);
		}
		//        img = ImageLoader.DefaultRequestImage(_extraInfo.Uri, this);


		static public UIImage ImageByScalingToMaxSize(this UIImage sourceImage, nfloat maxWidth, float scale = 2.0f)
		{
			if (sourceImage == null)
				return null;

			if (sourceImage.Size.Width < maxWidth)
				return sourceImage;
			nfloat btWidth = 0.0f;
			nfloat btHeight = 0.0f;
			if (sourceImage.Size.Width < sourceImage.Size.Height) {
				btHeight = maxWidth;
				btWidth = sourceImage.Size.Width * (maxWidth / sourceImage.Size.Height);
			} else {
				btWidth = maxWidth;
				btHeight = sourceImage.Size.Height * (maxWidth / sourceImage.Size.Width);
			}
			CGSize targetSize = new CGSize(btWidth, btHeight);
			return imageByScalingAndCroppingForSourceImage(sourceImage, targetSize, scale);
		}

		static UIImage imageByScalingAndCroppingForSourceImage(UIImage sourceImage, CGSize targetSize, float scale)
		{
			UIImage newImage = null;
			CGSize imageSize = sourceImage.Size;
			var width = imageSize.Width;
			var height = imageSize.Height;
			var targetWidth = targetSize.Width;
			var targetHeight = targetSize.Height;
			var scaleFactor = 0.0;
			var scaledWidth = targetWidth;
			var scaledHeight = targetHeight;
			CGPoint thumbnailPoint = new CGPoint(0.0f, 0.0f);
			if (imageSize == targetSize) {
				var widthFactor = targetWidth / width;
				var heightFactor = targetHeight / height;

				if (widthFactor > heightFactor)
					scaleFactor = widthFactor; // scale to fit height
                else
					scaleFactor = heightFactor; // scale to fit width
				scaledWidth = (nfloat)(width * scaleFactor);
				scaledHeight = (nfloat)(height * scaleFactor);

				// center the image
				if (widthFactor > heightFactor) {
					thumbnailPoint.Y = (targetHeight - scaledHeight) * 0.5f;
				} else if (widthFactor < heightFactor) {
					thumbnailPoint.X = (targetWidth - scaledWidth) * 0.5f;
				}
			}

//            // 创建一个bitmap的context
//            // 并把它设置成为当前正在使用的context
//            //Determine whether the screen is retina
//            if([[UIScreen mainScreen] scale] == 2.0){
//                UIGraphicsBeginImageContextWithOptions(size, NO, 2.0);
//            }else{
//                UIGraphicsBeginImageContext(size);
//            }
//            // 绘制改变大小的图片
//            [self drawInRect:CGRectMake(0, 0, size.width, size.height)];
//            // 从当前context中创建一个改变大小后的图片
//            UIImage* scaledImage = UIGraphicsGetImageFromCurrentImageContext();
//            // 使当前的context出堆栈
//            UIGraphicsEndImageContext();
//            // 返回新的改变大小后的图片
//            return scaledImage;

			if (UIScreen.MainScreen.Scale == 2.0) {
				UIGraphics.BeginImageContextWithOptions(targetSize, false, scale);
			} else {
				UIGraphics.BeginImageContext(targetSize); // this will crop
			}
			CGRect thumbnailRect = new CGRect();

			thumbnailRect.Location = thumbnailPoint;
			thumbnailRect.Width = scaledWidth;
			thumbnailRect.Height = scaledHeight;

			sourceImage.Draw(thumbnailRect);

			newImage = UIGraphics.GetImageFromCurrentImageContext();
			if (newImage == null)
				Console.WriteLine(@"could not scale image");

			//pop the context to get back to the default
			UIGraphics.EndImageContext();
			return newImage;
		}
	}
}

