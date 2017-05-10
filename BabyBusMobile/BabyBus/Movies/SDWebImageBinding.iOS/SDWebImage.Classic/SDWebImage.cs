using System;
using System.ComponentModel;
using System.Drawing;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;

namespace SDWebImage {

	// @interface SDImageCache : NSObject
	[BaseType (typeof (NSObject))]
	interface SDImageCache {

		// -(id)initWithNamespace:(NSString *)ns;
		[Export ("initWithNamespace:")]
		IntPtr Constructor (string ns);

		// @property (assign, nonatomic) NSUInteger maxMemoryCost;
		[Export ("maxMemoryCost", ArgumentSemantic.UnsafeUnretained)]
		uint MaxMemoryCost { get; set; }

		// @property (assign, nonatomic) SInteger maxCacheAge;
		[Export ("maxCacheAge", ArgumentSemantic.UnsafeUnretained)]
		int MaxCacheAge { get; set; }

		// @property (assign, nonatomic) NSUInteger maxCacheSize;
		[Export ("maxCacheSize", ArgumentSemantic.UnsafeUnretained)]
		uint MaxCacheSize { get; set; }

		// +(SDImageCache *)sharedImageCache;
		[Static, Export ("sharedImageCache")]
		SDImageCache SharedImageCache ();

		// -(void)addReadOnlyCachePath:(NSString *)path;
		[Export ("addReadOnlyCachePath:")]
		void AddReadOnlyCachePath (string path);

		// -(void)storeImage:(UIImage *)image forKey:(NSString *)key;
		[Export ("storeImage:forKey:")]
		void StoreImage (UIImage image, string key);

		// -(void)storeImage:(UIImage *)image forKey:(NSString *)key toDisk:(BOOL)toDisk;
		[Export ("storeImage:forKey:toDisk:")]
		void StoreImage (UIImage image, string key, bool toDisk);

		// -(void)storeImage:(UIImage *)image recalculateFromImage:(BOOL)recalculate imageData:(NSData *)imageData forKey:(NSString *)key toDisk:(BOOL)toDisk;
		[Export ("storeImage:recalculateFromImage:imageData:forKey:toDisk:")]
		void StoreImage (UIImage image, bool recalculate, NSData imageData, string key, bool toDisk);

		// -(NSOperation *)queryDiskCacheForKey:(NSString *)key done:(SDWebImageQueryCompletedBlock)doneBlock;
		[Export ("queryDiskCacheForKey:done:")]
		NSOperation QueryDiskCacheForKey (string key, Action<UIImage, SDImageCacheType> doneBlock);

		// -(UIImage *)imageFromMemoryCacheForKey:(NSString *)key;
		[Export ("imageFromMemoryCacheForKey:")]
		UIImage ImageFromMemoryCacheForKey (string key);

		// -(UIImage *)imageFromDiskCacheForKey:(NSString *)key;
		[Export ("imageFromDiskCacheForKey:")]
		UIImage ImageFromDiskCacheForKey (string key);

		// -(void)removeImageForKey:(NSString *)key;
		[Export ("removeImageForKey:")]
		void RemoveImageForKey (string key);

		// -(void)removeImageForKey:(NSString *)key withCompletion:(SDWebImageNoParamsBlock)completion;
		[Export ("removeImageForKey:withCompletion:")]
		void RemoveImageForKey (string key, Action completion);

		// -(void)removeImageForKey:(NSString *)key fromDisk:(BOOL)fromDisk;
		[Export ("removeImageForKey:fromDisk:")]
		void RemoveImageForKey (string key, bool fromDisk);

		// -(void)removeImageForKey:(NSString *)key fromDisk:(BOOL)fromDisk withCompletion:(SDWebImageNoParamsBlock)completion;
		[Export ("removeImageForKey:fromDisk:withCompletion:")]
		void RemoveImageForKey (string key, bool fromDisk, Action completion);

		// -(void)clearMemory;
		[Export ("clearMemory")]
		void ClearMemory ();

		// -(void)clearDiskOnCompletion:(SDWebImageNoParamsBlock)completion;
		[Export ("clearDiskOnCompletion:")]
		void ClearDiskOnCompletion (Action completion);

		// -(void)clearDisk;
		[Export ("clearDisk")]
		void ClearDisk ();

		// -(void)cleanDiskWithCompletionBlock:(SDWebImageNoParamsBlock)completionBlock;
		[Export ("cleanDiskWithCompletionBlock:")]
		void CleanDiskWithCompletionBlock (Action completionBlock);

		// -(void)cleanDisk;
		[Export ("cleanDisk")]
		void CleanDisk ();

		// -(NSUInteger)getSize;
		[Export ("getSize")]
		uint GetSize ();

		// -(NSUInteger)getDiskCount;
		[Export ("getDiskCount")]
		uint GetDiskCount ();

		// -(void)calculateSizeWithCompletionBlock:(SDWebImageCalculateSizeBlock)completionBlock;
		[Export ("calculateSizeWithCompletionBlock:")]
		void CalculateSizeWithCompletionBlock (Action<uint, uint> completionBlock);

		// -(void)diskImageExistsWithKey:(NSString *)key completion:(SDWebImageCheckCacheCompletionBlock)completionBlock;
		[Export ("diskImageExistsWithKey:completion:")]
		void DiskImageExistsWithKey (string key, Action<sbyte> completionBlock);

		// -(BOOL)diskImageExistsWithKey:(NSString *)key;
		[Export ("diskImageExistsWithKey:")]
		bool DiskImageExistsWithKey (string key);

		// -(NSString *)cachePathForKey:(NSString *)key inPath:(NSString *)path;
		[Export ("cachePathForKey:inPath:")]
		string CachePathForKey (string key, string path);

		// -(NSString *)defaultCachePathForKey:(NSString *)key;
		[Export ("defaultCachePathForKey:")]
		string DefaultCachePathForKey (string key);
	}

	// @interface ForceDecode (UIImage)
	[Category]
	[BaseType (typeof (UIImage))]
	interface ForceDecode {

		// +(UIImage *)decodedImageWithImage:(UIImage *)image;
		[Static, Export ("decodedImageWithImage:")]
		UIImage DecodedImageWithImage (UIImage image);
	}

	// @protocol SDWebImageOperation <NSObject>
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SDWebImageOperation {

		// @required -(void)cancel;
		[Export ("cancel")]
		[Abstract]
		void Cancel ();
	}

	// @interface SDWebImageDownloader : NSObject
	[BaseType (typeof (NSObject))]
	interface SDWebImageDownloader {

		// @property (assign, nonatomic) SInteger maxConcurrentDownloads;
		[Export ("maxConcurrentDownloads", ArgumentSemantic.UnsafeUnretained)]
		int MaxConcurrentDownloads { get; set; }

		// @property (readonly, nonatomic) NSUInteger currentDownloadCount;
		[Export ("currentDownloadCount")]
		uint CurrentDownloadCount { get; }

		// @property (assign, nonatomic) NSTimeInterval downloadTimeout;
		[Export ("downloadTimeout", ArgumentSemantic.UnsafeUnretained)]
		double DownloadTimeout { get; set; }

		// @property (assign, nonatomic) SDWebImageDownloaderExecutionOrder executionOrder;
		[Export ("executionOrder", ArgumentSemantic.UnsafeUnretained)]
		SDWebImageDownloaderExecutionOrder ExecutionOrder { get; set; }

		// @property (nonatomic, strong) NSString * username;
		[Export ("username", ArgumentSemantic.Retain)]
		string Username { get; set; }

		// @property (nonatomic, strong) NSString * password;
		[Export ("password", ArgumentSemantic.Retain)]
		string Password { get; set; }

		// @property (copy, nonatomic) SDWebImageDownloaderHeadersFilterBlock headersFilter;
		[Export ("headersFilter", ArgumentSemantic.Copy)]
		Func<NSUrl, NSDictionary, NSDictionary> HeadersFilter { get; set; }

		// +(SDWebImageDownloader *)sharedDownloader;
		[Static, Export ("sharedDownloader")]
		SDWebImageDownloader SharedDownloader ();

		// -(void)setValue:(NSString *)value forHTTPHeaderField:(NSString *)field;
		[Export ("setValue:forHTTPHeaderField:")]
		void SetValue (string value, string field);

		// -(NSString *)valueForHTTPHeaderField:(NSString *)field;
		[Export ("valueForHTTPHeaderField:")]
		string ValueForHTTPHeaderField (string field);

		// -(id<SDWebImageOperation>)downloadImageWithURL:(NSURL *)url options:(SDWebImageDownloaderOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageDownloaderCompletedBlock)completedBlock;
		[Export ("downloadImageWithURL:options:progress:completed:")]
		SDWebImageOperation DownloadImageWithURL (NSUrl url, SDWebImageDownloaderOptions options, Action<int, int> progressBlock, Action<UIImage, NSData, NSError, sbyte> completedBlock);

		// -(void)setSuspended:(BOOL)suspended;
		[Export ("setSuspended:")]
		void SetSuspended (bool suspended);
	}

	// @interface SDWebImageDownloaderOperation : NSOperation <SDWebImageOperation>
	[BaseType (typeof (NSOperation))]
	interface SDWebImageDownloaderOperation : SDWebImageOperation {

		// -(id)initWithRequest:(NSURLRequest *)request options:(SDWebImageDownloaderOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageDownloaderCompletedBlock)completedBlock cancelled:(SDWebImageNoParamsBlock)cancelBlock;
		[Export ("initWithRequest:options:progress:completed:cancelled:")]
		IntPtr Constructor (NSUrlRequest request, SDWebImageDownloaderOptions options, Action<int, int> progressBlock, Action<UIImage, NSData, NSError, sbyte> completedBlock, Action cancelBlock);

		// @property (readonly, nonatomic, strong) NSURLRequest * request;
		[Export ("request", ArgumentSemantic.Retain)]
		NSUrlRequest Request { get; }

		// @property (assign, nonatomic) BOOL shouldUseCredentialStorage;
		[Export ("shouldUseCredentialStorage", ArgumentSemantic.UnsafeUnretained)]
		bool ShouldUseCredentialStorage { get; set; }

		// @property (nonatomic, strong) NSURLCredential * credential;
		[Export ("credential", ArgumentSemantic.Retain)]
		NSUrlCredential Credential { get; set; }

		// @property (readonly, assign, nonatomic) SDWebImageDownloaderOptions options;
		[Export ("options", ArgumentSemantic.UnsafeUnretained)]
		SDWebImageDownloaderOptions Options { get; }
	}

	// @protocol SDWebImageManagerDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SDWebImageManagerDelegate {

		// @optional -(BOOL)imageManager:(SDWebImageManager *)imageManager shouldDownloadImageForURL:(NSURL *)imageURL;
		[Export ("imageManager:shouldDownloadImageForURL:")]
		bool ShouldDownloadImageForURL (SDWebImageManager imageManager, NSUrl imageURL);

		// @optional -(UIImage *)imageManager:(SDWebImageManager *)imageManager transformDownloadedImage:(UIImage *)image withURL:(NSURL *)imageURL;
		[Export ("imageManager:transformDownloadedImage:withURL:")]
		UIImage TransformDownloadedImage (SDWebImageManager imageManager, UIImage image, NSUrl imageURL);
	}

	// @interface SDWebImageManager : NSObject
	[BaseType (typeof (NSObject))]
	interface SDWebImageManager {

		// @property (nonatomic, weak) id<SDWebImageManagerDelegate> delegate;
		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		// @property (nonatomic, weak) id<SDWebImageManagerDelegate> delegate;
		[Wrap ("WeakDelegate")]
		SDWebImageManagerDelegate Delegate { get; set; }

		// @property (readonly, nonatomic, strong) SDImageCache * imageCache;
		[Export ("imageCache", ArgumentSemantic.Retain)]
		SDImageCache ImageCache { get; }

		// @property (readonly, nonatomic, strong) SDWebImageDownloader * imageDownloader;
		[Export ("imageDownloader", ArgumentSemantic.Retain)]
		SDWebImageDownloader ImageDownloader { get; }

		// @property (copy) SDWebImageCacheKeyFilterBlock cacheKeyFilter;
		[Export ("cacheKeyFilter", ArgumentSemantic.Copy)]
		Func<NSUrl, NSString> CacheKeyFilter { get; set; }

		// +(SDWebImageManager *)sharedManager;
		[Static, Export ("sharedManager")]
		SDWebImageManager SharedManager ();

		// -(id<SDWebImageOperation>)downloadImageWithURL:(NSURL *)url options:(SDWebImageOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageCompletionWithFinishedBlock)completedBlock;
		[Export ("downloadImageWithURL:options:progress:completed:")]
		SDWebImageOperation DownloadImageWithURL (NSUrl url, SDWebImageOptions options, Action<int, int> progressBlock, Action<UIImage, NSError, SDImageCacheType, sbyte, NSUrl> completedBlock);

		// -(void)saveImageToCache:(UIImage *)image forURL:(NSURL *)url;
		[Export ("saveImageToCache:forURL:")]
		void SaveImageToCache (UIImage image, NSUrl url);

		// -(void)cancelAll;
		[Export ("cancelAll")]
		void CancelAll ();

		// -(BOOL)isRunning;
		[Export ("isRunning")]
		bool IsRunning ();

		// -(BOOL)cachedImageExistsForURL:(NSURL *)url;
		[Export ("cachedImageExistsForURL:")]
		bool CachedImageExistsForURL (NSUrl url);

		// -(BOOL)diskImageExistsForURL:(NSURL *)url;
		[Export ("diskImageExistsForURL:")]
		bool DiskImageExistsForURL (NSUrl url);

		// -(void)cachedImageExistsForURL:(NSURL *)url completion:(SDWebImageCheckCacheCompletionBlock)completionBlock;
		[Export ("cachedImageExistsForURL:completion:")]
		void CachedImageExistsForURL (NSUrl url, Action<sbyte> completionBlock);

		// -(void)diskImageExistsForURL:(NSURL *)url completion:(SDWebImageCheckCacheCompletionBlock)completionBlock;
		[Export ("diskImageExistsForURL:completion:")]
		void DiskImageExistsForURL (NSUrl url, Action<sbyte> completionBlock);

		// -(NSString *)cacheKeyForURL:(NSURL *)url;
		[Export ("cacheKeyForURL:")]
		string CacheKeyForURL (NSUrl url);
	}

	// @protocol SDWebImagePrefetcherDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface SDWebImagePrefetcherDelegate {

		// @optional -(void)imagePrefetcher:(SDWebImagePrefetcher *)imagePrefetcher didPrefetchURL:(NSURL *)imageURL finishedCount:(NSUInteger)finishedCount totalCount:(NSUInteger)totalCount;
		[Export ("imagePrefetcher:didPrefetchURL:finishedCount:totalCount:")]
		void DidPrefetchURL (SDWebImagePrefetcher imagePrefetcher, NSUrl imageURL, uint finishedCount, uint totalCount);

		// @optional -(void)imagePrefetcher:(SDWebImagePrefetcher *)imagePrefetcher didFinishWithTotalCount:(NSUInteger)totalCount skippedCount:(NSUInteger)skippedCount;
		[Export ("imagePrefetcher:didFinishWithTotalCount:skippedCount:")]
		void DidFinishWithTotalCount (SDWebImagePrefetcher imagePrefetcher, uint totalCount, uint skippedCount);
	}

	// @interface SDWebImagePrefetcher : NSObject
	[BaseType (typeof (NSObject))]
	interface SDWebImagePrefetcher {

		// @property (readonly, nonatomic, strong) SDWebImageManager * manager;
		[Export ("manager", ArgumentSemantic.Retain)]
		SDWebImageManager Manager { get; }

		// @property (assign, nonatomic) NSUInteger maxConcurrentDownloads;
		[Export ("maxConcurrentDownloads", ArgumentSemantic.UnsafeUnretained)]
		uint MaxConcurrentDownloads { get; set; }

		// @property (assign, nonatomic) SDWebImageOptions options;
		[Export ("options", ArgumentSemantic.UnsafeUnretained)]
		SDWebImageOptions Options { get; set; }

		// @property (nonatomic, weak) id<SDWebImagePrefetcherDelegate> delegate;
		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		// @property (nonatomic, weak) id<SDWebImagePrefetcherDelegate> delegate;
		[Wrap ("WeakDelegate")]
		SDWebImagePrefetcherDelegate Delegate { get; set; }

		// +(SDWebImagePrefetcher *)sharedImagePrefetcher;
		[Static, Export ("sharedImagePrefetcher")]
		SDWebImagePrefetcher SharedImagePrefetcher ();

		// -(void)prefetchURLs:(NSArray *)urls;
		[Export ("prefetchURLs:")]
		void PrefetchURLs (NSObject [] urls);

		// -(void)prefetchURLs:(NSArray *)urls progress:(SDWebImagePrefetcherProgressBlock)progressBlock completed:(SDWebImagePrefetcherCompletionBlock)completionBlock;
		[Export ("prefetchURLs:progress:completed:")]
		void PrefetchURLs (NSObject [] urls, Action<uint, uint> progressBlock, Action<uint, uint> completionBlock);

		// -(void)cancelPrefetching;
		[Export ("cancelPrefetching")]
		void CancelPrefetching ();
	}

	// @interface WebCache (UIButton)
	[Category]
	[BaseType (typeof (UIButton))]
	interface UIButton_WebCache {

		// -(NSURL *)sd_currentImageURL;
		[Export ("sd_currentImageURL")]
		NSUrl Sd_currentImageURL ();

		// -(NSURL *)sd_imageURLForState:(UIControlState)state;
		[Export ("sd_imageURLForState:")]
		NSUrl Sd_imageURLForState (UIControlState state);

		// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state;
		[Export ("sd_setImageWithURL:forState:")]
		void Sd_setImageWithURL (NSUrl url, UIControlState state);

		// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder;
		[Export ("sd_setImageWithURL:forState:placeholderImage:")]
		void Sd_setImageWithURL (NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder);

		// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options;
		[Export ("sd_setImageWithURL:forState:placeholderImage:options:")]
		void Sd_setImageWithURL (NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, SDWebImageOptions options);

		// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setImageWithURL:forState:completed:")]
		void Sd_setImageWithURL (NSUrl url, UIControlState state, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setImageWithURL:forState:placeholderImage:completed:")]
		void Sd_setImageWithURL (NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setImageWithURL:forState:placeholderImage:options:completed:")]
		void Sd_setImageWithURL (NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, SDWebImageOptions options, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state;
		[Export ("sd_setBackgroundImageWithURL:forState:")]
		void Sd_setBackgroundImageWithURL (NSUrl url, UIControlState state);

		// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder;
		[Export ("sd_setBackgroundImageWithURL:forState:placeholderImage:")]
		void Sd_setBackgroundImageWithURL (NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder);

		// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options;
		[Export ("sd_setBackgroundImageWithURL:forState:placeholderImage:options:")]
		void Sd_setBackgroundImageWithURL (NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, SDWebImageOptions options);

		// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setBackgroundImageWithURL:forState:completed:")]
		void Sd_setBackgroundImageWithURL (NSUrl url, UIControlState state, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setBackgroundImageWithURL:forState:placeholderImage:completed:")]
		void Sd_setBackgroundImageWithURL (NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setBackgroundImageWithURL:forState:placeholderImage:options:completed:")]
		void Sd_setBackgroundImageWithURL (NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, SDWebImageOptions options, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_cancelImageLoadForState:(UIControlState)state;
		[Export ("sd_cancelImageLoadForState:")]
		void Sd_cancelImageLoadForState (UIControlState state);

		// -(void)sd_cancelBackgroundImageLoadForState:(UIControlState)state;
		[Export ("sd_cancelBackgroundImageLoadForState:")]
		void Sd_cancelBackgroundImageLoadForState (UIControlState state);
	}

	// @interface HighlightedWebCache (UIImageView)
	[Category]
	[BaseType (typeof (UIImageView))]
	interface UIImageView_HighlightedWebCache {

		// -(void)sd_setHighlightedImageWithURL:(NSURL *)url;
		[Export ("sd_setHighlightedImageWithURL:")]
		void Sd_setHighlightedImageWithURL (NSUrl url);

		// -(void)sd_setHighlightedImageWithURL:(NSURL *)url options:(SDWebImageOptions)options;
		[Export ("sd_setHighlightedImageWithURL:options:")]
		void Sd_setHighlightedImageWithURL (NSUrl url, SDWebImageOptions options);

		// -(void)sd_setHighlightedImageWithURL:(NSURL *)url completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setHighlightedImageWithURL:completed:")]
		void Sd_setHighlightedImageWithURL (NSUrl url, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setHighlightedImageWithURL:(NSURL *)url options:(SDWebImageOptions)options completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setHighlightedImageWithURL:options:completed:")]
		void Sd_setHighlightedImageWithURL (NSUrl url, SDWebImageOptions options, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setHighlightedImageWithURL:(NSURL *)url options:(SDWebImageOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setHighlightedImageWithURL:options:progress:completed:")]
		void Sd_setHighlightedImageWithURL (NSUrl url, SDWebImageOptions options, Action<int, int> progressBlock, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_cancelCurrentHighlightedImageLoad;
		[Export ("sd_cancelCurrentHighlightedImageLoad")]
		void Sd_cancelCurrentHighlightedImageLoad ();
	}

	// @interface WebCache (UIImageView)
	[Category]
	[BaseType (typeof (UIImageView))]
	interface UIImageView_WebCache {

		// -(NSURL *)sd_imageURL;
		[Export ("sd_imageURL")]
		NSUrl Sd_imageURL ();

		// -(void)sd_setImageWithURL:(NSURL *)url;
		[Export ("sd_setImageWithURL:")]
		void Sd_setImageWithURL (NSUrl url);

		// -(void)sd_setImageWithURL:(NSURL *)url placeholderImage:(UIImage *)placeholder;
		[Export ("sd_setImageWithURL:placeholderImage:")]
		void Sd_setImageWithURL (NSUrl url, [NullAllowed] UIImage placeholder);

		// -(void)sd_setImageWithURL:(NSURL *)url placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options;
		[Export ("sd_setImageWithURL:placeholderImage:options:")]
		void Sd_setImageWithURL (NSUrl url, [NullAllowed] UIImage placeholder, SDWebImageOptions options);

		// -(void)sd_setImageWithURL:(NSURL *)url completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setImageWithURL:completed:")]
		void Sd_setImageWithURL (NSUrl url, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setImageWithURL:(NSURL *)url placeholderImage:(UIImage *)placeholder completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setImageWithURL:placeholderImage:completed:")]
		void Sd_setImageWithURL (NSUrl url, [NullAllowed] UIImage placeholder, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setImageWithURL:(NSURL *)url placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setImageWithURL:placeholderImage:options:completed:")]
		void Sd_setImageWithURL (NSUrl url, [NullAllowed] UIImage placeholder, SDWebImageOptions options, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setImageWithURL:(NSURL *)url placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setImageWithURL:placeholderImage:options:progress:completed:")]
		void Sd_setImageWithURL (NSUrl url, [NullAllowed] UIImage placeholder, SDWebImageOptions options, Action<int, int> progressBlock, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setImageWithPreviousCachedImageWithURL:(NSURL *)url andPlaceholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageCompletionBlock)completedBlock;
		[Export ("sd_setImageWithPreviousCachedImageWithURL:andPlaceholderImage:options:progress:completed:")]
		void Sd_setImageWithPreviousCachedImageWithURL (NSUrl url, [NullAllowed] UIImage placeholder, SDWebImageOptions options, Action<int, int> progressBlock, Action<UIImage, NSError, SDImageCacheType, NSUrl> completedBlock);

		// -(void)sd_setAnimationImagesWithURLs:(NSArray *)arrayOfURLs;
		[Export ("sd_setAnimationImagesWithURLs:")]
		void Sd_setAnimationImagesWithURLs (NSObject [] arrayOfURLs);

		// -(void)sd_cancelCurrentImageLoad;
		[Export ("sd_cancelCurrentImageLoad")]
		void Sd_cancelCurrentImageLoad ();

		// -(void)sd_cancelCurrentAnimationImagesLoad;
		[Export ("sd_cancelCurrentAnimationImagesLoad")]
		void Sd_cancelCurrentAnimationImagesLoad ();
	}
}
