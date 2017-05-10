using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BabyBus.Droid.Views.Common.Album
{
    public class ImageItem : Java.Lang.Object
    {
        public ImageItem() {
            IsSelect = false;
        }

        public string ImageId { get; set; }
        public string ThumbnailPath { get; set; }
        public string ImagePath { get; set; }
        public bool IsSelect { get; set; }
    }
}
