using System.Collections.Generic;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using BabyBus.Droid.Utils.Image;
using BabyBus.Droid.Views.Common.Album;
using Cirrious.MvvmCross.Droid.Views;
using Java.Lang;

namespace BabyBus.Droid.Adapters {
    public class ImageGridAdapter : BaseAdapter {
        private readonly MvxActivity activity;
        private readonly BitmapCache cache = new BitmapCache();
        private readonly IImageCallback callback = new ImageCallback();
        private readonly List<ImageItem> list;

        public ImageGridAdapter(MvxActivity activity, List<ImageItem> list) {
            this.activity = activity;
            this.list = list;
        }

        public override int Count {
            get { return list.Count; }
        }

        public override Object GetItem(int position) {
            return null;
        }

        public override long GetItemId(int position) {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent) {
            ViewHolder holder;
            View view;
            if (convertView == null) {
                view = activity.LayoutInflater.Inflate(
                    Resource.Layout.Item_ImageGrid, parent, false);
                holder = new ViewHolder();
                holder.Image = view.FindViewById<ImageView>(Resource.Id.albumImage);
                holder.SelectedImage = view.FindViewById<ImageView>(Resource.Id.isselected);
                holder.SelectedText = view.FindViewById<TextView>(Resource.Id.item_grid_image_text);
                view.Tag = holder;
            }
            else {
                view = convertView;
                holder = (ViewHolder) view.Tag;
            }

            ImageItem item = list[position];
            holder.Image.SetBackgroundResource(Resource.Color.black);
            holder.Image.Tag = item.ImagePath;
            //Async Load & Display Image
            cache.DisplayBmp(holder.Image, item.ThumbnailPath, item.ImagePath, item.ImageId, callback);
            if (item.IsSelect) {
                holder.SelectedImage.SetImageResource(Resource.Drawable.icon_data_select);
                holder.SelectedText.SetBackgroundResource(Resource.Drawable.bgd_relatly_line);
            }
            else {
                holder.SelectedImage.SetImageBitmap(null);
                holder.SelectedText.SetBackgroundColor(new Color(0x00000000));
            }
            return view;
        }

        public void ClearCache() {
            cache.ClearCache();
        }

        public class ViewHolder : Object {
            public ImageView Image { get; set; }
            public ImageView SelectedImage { get; set; }
            public TextView SelectedText { get; set; }
        }

        
    }
}