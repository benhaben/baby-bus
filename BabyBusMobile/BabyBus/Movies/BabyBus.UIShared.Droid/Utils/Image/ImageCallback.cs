using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Util;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;

namespace BabyBus.Droid.Utils.Image
{
    /// <summary>
    /// Image Call back
    /// </summary>
    public interface IImageCallback
    {
        void ImageLoad(ImageView iv, Bitmap bmp);
    }

    public class ImageCallback : IImageCallback
    {
        private Handler handler = new Handler();
        public void ImageLoad(ImageView iv, Bitmap bmp) {
            handler.Post(() => {
				if (iv != null && bmp != null)
                {
					var bmpDrawable = iv.Drawable as BitmapDrawable;
                    if (bmpDrawable != null && !bmpDrawable.Bitmap.IsRecycled) {
                        var preBmp = bmpDrawable.Bitmap;
                        preBmp.Recycle();
                        preBmp = null;
                    }

                    iv.SetImageBitmap(bmp);
                }
				else{
					iv.SetImageResource(Resource.Color.black);
				}
            });
        }
    }

    public class BitmapLruCache : LruCache {
        protected BitmapLruCache(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
        }

        public BitmapLruCache(int maxSize) : base(maxSize) {
        }

//        protected override void EntryRemoved(bool evicted, Object key, Object oldValue, Object newValue) {
//            base.EntryRemoved(evicted, key, oldValue, newValue);
//
//            var wrapper = oldValue as ObjectWrapper<Bitmap>;
//            if (wrapper != null && wrapper.Value != null) {
//                wrapper.Value.Recycle();
//                wrapper.Value = null;
//            }
//        }
    }
}