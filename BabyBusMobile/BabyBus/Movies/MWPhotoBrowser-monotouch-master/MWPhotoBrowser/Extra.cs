using System;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Threading;

namespace SDWebImage {
    public partial class SDImageCache : NSObject {
    }

    public partial class SDWebImageManager : NSObject {
        public ISDWebImageOperation Download(string url, SDWebImageOptions options, SDWebImageDownloaderProgressBlock progress, SDWebImageCompletedWithFinishedBlock completed) {
            // TEMP - remove after Xamarin.iOS update (7.0.2 or whatever)
            #if DEBUG
            completed(new UIImage(), null, 0, true);
            return null;
            #endif
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new Exception(String.Format("Malformed url: {0}", url));
            return Download(NSUrl.FromString(url), options, progress, completed);
        }

        public Task<ImageDownloadResult> DownloadAsync(NSUrl url, SDWebImageOptions options = SDWebImageOptions.None, SDWebImageDownloaderProgressBlock progress = null, CancellationToken token = default(CancellationToken)) {
            var tcs = new TaskCompletionSource<ImageDownloadResult>();

            ISDWebImageOperation operation = null;

            operation = Download(url, options, progress, (image, error, cacheType, finished) => {
                if (token.IsCancellationRequested) {
                    operation.Cancel();
                    tcs.TrySetCanceled();
                    return;
                }
                if (finished) {
                    if (image == null) {
                        tcs.TrySetException(new NSErrorException(error));
                        return;
                    }
                    tcs.TrySetResult(new ImageDownloadResult(image, cacheType));
                }
            });

            return tcs.Task;
        }

        public Task<ImageDownloadResult> DownloadAsync(string url, SDWebImageOptions options = SDWebImageOptions.None, SDWebImageDownloaderProgressBlock progress = null, CancellationToken token = default(CancellationToken)) {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new Exception(String.Format("Malformed url: {0}", url));

            return DownloadAsync(NSUrl.FromString(url), options, progress, token);
        }

        public void ClearMemoryCache() {
            SDWebImageManager.SharedManager.ImageCache.ClearMemory();
        }
    }

    public class ImageDownloadResult {
        public ImageDownloadResult(UIImage image, SDImageCacheType cacheType) {
            Image = image;
            CacheType = cacheType;
        }

        public UIImage Image { get; private set; }

        public SDImageCacheType CacheType { get; private set; }
    }

}

