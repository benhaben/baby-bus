using System.Runtime.InteropServices;
using ObjCRuntime;
using UIKit;


namespace MWPhotoBrowserBinding
{
}

namespace SDWebImage
{

    static class CFunctions
    {
        // extern UIImage * SDScaledImageForKey (NSString * key, UIImage * image);
        [DllImport("__Internal")]
        static extern UIImage SDScaledImageForKey(string key, UIImage image);
    }

    [Native]
    public enum SDWebImageDownloaderOptions : ulong
    {
        LowPriority = 1 << 0,
        ProgressiveDownload = 1 << 1,
        UseNSURLCache = 1 << 2,
        IgnoreCachedResponse = 1 << 3,
        ContinueInBackground = 1 << 4,
        HandleCookies = 1 << 5,
        AllowInvalidSSLCertificates = 1 << 6,
        HighPriority = 1 << 7
    }

    [Native]
    public enum SDWebImageDownloaderExecutionOrder : long
    {
        FIFOExecutionOrder,
        LIFOExecutionOrder
    }

    [Native]
    public enum SDImageCacheType : long
    {
        None,
        Disk,
        Memory
    }

    [Native]
    public enum SDWebImageOptions : ulong
    {
        RetryFailed = 1 << 0,
        LowPriority = 1 << 1,
        CacheMemoryOnly = 1 << 2,
        ProgressiveDownload = 1 << 3,
        RefreshCached = 1 << 4,
        ContinueInBackground = 1 << 5,
        HandleCookies = 1 << 6,
        AllowInvalidSSLCertificates = 1 << 7,
        HighPriority = 1 << 8,
        DelayPlaceholder = 1 << 9
    }

}

