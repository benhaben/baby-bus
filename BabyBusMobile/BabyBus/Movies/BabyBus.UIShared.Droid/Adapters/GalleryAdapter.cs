using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Utils.Image;
using BabyBus.Droid.Views.Common.Album;
using Java.Lang;

namespace BabyBus.Droid.Adapters
{
    public class GalleryAdapter : BaseAdapter
    {
        private readonly Context context;
        private readonly Dictionary<string, Bitmap> dic = new Dictionary<string, Bitmap>();

        public GalleryAdapter(Context c) {
            context = c;
        }

        public override int Count {
            get { return ImageCollection.PthList.Count; }
        }

        public override Object GetItem(int position) {
            return null;
        }

        public override long GetItemId(int position) {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent) {
            var iv = new ImageView(context);
            ImageCollection.Selected = position;
            iv.SetImageBitmap(ImageCollection.BmpList[position]);
            iv.LayoutParameters = new Gallery.LayoutParams(context.Resources.DisplayMetrics.WidthPixels,
                context.Resources.DisplayMetrics.HeightPixels);
            iv.SetScaleType(ImageView.ScaleType.CenterCrop);
            if (dic.ContainsKey(ImageCollection.PthList[position]))
            {
                iv.SetImageBitmap(dic[ImageCollection.PthList[position]]);
            }
            else
            {
                Bitmap bmp = ImageRevition.RevitionImageSize(ImageCollection.PthList[position], 600, 600);
                dic.Add(ImageCollection.PthList[position], bmp);
                iv.SetImageBitmap(bmp);
            }
            return iv;
        }
    }
}