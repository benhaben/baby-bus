using System;
using UIKit;

namespace PhotoBrowser {
    public interface IMWPhotoProtocol {
        // Called when the browser has determined the underlying images is not
        // already loaded into memory but needs it.
        void LoadUnderlyingImageAndNotify() ;

        // Fetch the image data from a source and notify when complete.
        // You must load the image asyncronously (and decompress it for better performance).
        // It is recommended that you use SDWebImageDecoder to perform the decompression.
        // See MWPhoto object for an example implementation.
        // When the underlying UIImage is loaded (or failed to load) you should post the following
        // notification:
        // [[NSNotificationCenter defaultCenter] postNotificationName:MWPHOTO_LOADING_DID_END_NOTIFICATION
        //                                                     object:self];
        void PerformLoadUnderlyingImageAndNotify();

        // This is called when the photo browser has determined the photo data
        // is no longer needed or there are low memory conditions
        // You should release any underlying (possibly large and decompressed) image data
        // as long as the image can be re-loaded (from cache, file, or URL)
        void UnloadUnderlyingImage() ;

        // Cancel any background loading of image data
        void CancelAnyLoading();
    }

    public class MWPhotoProtocol : IMWPhotoProtocol {
        public MWPhotoProtocol() {
        }


       
        public virtual void LoadUnderlyingImageAndNotify() {
        }

        public virtual void PerformLoadUnderlyingImageAndNotify() {
        }

        public virtual void UnloadUnderlyingImage() {
        }

        // Cancel any background loading of image data
        public virtual void CancelAnyLoading() {
        }


    }
}

