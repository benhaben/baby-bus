using System;
using CrossUI.Touch.Dialog.Utilities;
using UIKit;

namespace Dialog.Utilities {

    //I want to write a module like https://github.com/rs/SDWebImage/
    //need write extension to UIImageView instead of UIImage
    public class ImageUpdated : IImageUpdated {
        public ImageUpdated(UIImage image) {
            _imageWeakReference = new WeakReference(image);
        }

        readonly WeakReference _imageWeakReference;

        #region IImageUpdated implementation

        void IImageUpdated.UpdatedImage(Uri uri) {
            var img = ImageLoader.DefaultRequestImage(uri, this);
            if (img != null && _imageWeakReference.IsAlive) {
                var target = _imageWeakReference.Target as UIImage;
                if (target != null)
                    target = img;
            }
        }



        #endregion
    }

    public static class ImageUpdatedUtilities {
        /// <summary>
        /// Requests the image by URI.
        /// </summary>
        /// <returns><c>true</c>, in cache, <c>false</c> otherwise.</returns>
        /// <param name="sourceImage">Source image.</param>
        /// <param name="uri">URI.</param>
        /// <param name="defaultImage">Default image.</param>
        static public bool RequestImageByUri(this UIImage sourceImage, Uri uri, UIImage defaultImage) {
            bool ret = false;
            if (sourceImage == null) {
                ret = false;
            } else {
                var updated = new ImageUpdated(sourceImage);
                var img = ImageLoader.DefaultRequestImage(uri, updated);
                if (img != null) {
                    sourceImage = img;
                    ret = true;
                } else {
                    sourceImage = defaultImage;
                    ret = false;
                }
            }
            return ret;
        }
    }
}

