namespace SDWebImage {

	[Native]
	public enum SDImageCacheType : long /* nint */ {
		None,
		Disk,
		Memory
	}

	[Native]
	public enum SDWebImageDownloaderOptions : ulong /* nuint */ {
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
	public enum SDWebImageDownloaderExecutionOrder : long /* nint */ {
		SDWebImageDownloaderFIFOExecutionOrder,
		SDWebImageDownloaderLIFOExecutionOrder
	}

	[Native]
	public enum SDWebImageOptions : ulong /* nuint */ {
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
