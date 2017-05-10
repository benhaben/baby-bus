using System;

#if UNIFIED
using ObjCRuntime;
#else
using MonoTouch.ObjCRuntime;
#endif

namespace SDWebImage {

	[Native]
	#if UNIFIED
	public enum SDImageCacheType : long
	#else
	public enum SDImageCacheType : int
	#endif
	{
		None,
		Disk,
		Memory
	}

	[Native]
	#if UNIFIED
	public enum SDWebImageDownloaderOptions : ulong
	#else
	public enum SDWebImageDownloaderOptions : uint
	#endif
	{
		SDWebImageDownloaderLowPriority = 1 << 0,
		SDWebImageDownloaderProgressiveDownload = 1 << 1,
		SDWebImageDownloaderUseNSURLCache = 1 << 2,
		SDWebImageDownloaderIgnoreCachedResponse = 1 << 3,
		SDWebImageDownloaderContinueInBackground = 1 << 4,
		SDWebImageDownloaderHandleCookies = 1 << 5,
		SDWebImageDownloaderAllowInvalidSSLCertificates = 1 << 6,
		SDWebImageDownloaderHighPriority = 1 << 7
	}

	[Native]
	#if UNIFIED
	public enum SDWebImageDownloaderExecutionOrder : long
	#else
	public enum SDWebImageDownloaderExecutionOrder : int
	#endif
	{
		SDWebImageDownloaderFIFOExecutionOrder,
		SDWebImageDownloaderLIFOExecutionOrder
	}
		
	[Native]
	#if UNIFIED
	public enum SDWebImageOptions : ulong
	#else
	public enum SDWebImageOptions : uint
	#endif
	{
		SDWebImageRetryFailed = 1 << 0,
		SDWebImageLowPriority = 1 << 1,
		SDWebImageCacheMemoryOnly = 1 << 2,
		SDWebImageProgressiveDownload = 1 << 3,
		SDWebImageRefreshCached = 1 << 4,
		SDWebImageContinueInBackground = 1 << 5,
		SDWebImageHandleCookies = 1 << 6,
		SDWebImageAllowInvalidSSLCertificates = 1 << 7,
		SDWebImageHighPriority = 1 << 8,
		SDWebImageDelayPlaceholder = 1 << 9,
		SDWebImageTransformAnimatedImage = 1 << 10
	}
}
