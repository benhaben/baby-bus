using System;
using Foundation;
using ObjCRuntime;
using UIKit;

// typedef void (^SDWebImageNoParamsBlock)();
delegate void SDWebImageNoParamsBlock ();

// @protocol SDWebImageOperation <NSObject>
[Protocol, Model]
[BaseType (typeof(NSObject))]
interface SDWebImageOperation
{
	// @required -(void)cancel;
	[Abstract]
	[Export ("cancel")]
	void Cancel ();
}

[Verify (ConstantsInterfaceAssociation)]
partial interface Constants
{
	// extern NSString *const SDWebImageDownloadStartNotification;
	[Field ("SDWebImageDownloadStartNotification")]
	NSString SDWebImageDownloadStartNotification { get; }

	// extern NSString *const SDWebImageDownloadStopNotification;
	[Field ("SDWebImageDownloadStopNotification")]
	NSString SDWebImageDownloadStopNotification { get; }
}

// typedef void (^SDWebImageDownloaderProgressBlock)(NSIntegerNSInteger);
delegate void SDWebImageDownloaderProgressBlock (nint arg0, nint arg1);

// typedef void (^SDWebImageDownloaderCompletedBlock)(UIImage *NSData *NSError *BOOL);
delegate void SDWebImageDownloaderCompletedBlock (UIImage arg0, NSData arg1, NSError arg2, bool arg3);

// typedef NSDictionary * (^SDWebImageDownloaderHeadersFilterBlock)(NSURL *NSDictionary *);
delegate NSDictionary SDWebImageDownloaderHeadersFilterBlock (NSURL arg0, NSDictionary arg1);

// @interface SDWebImageDownloader : NSObject
[BaseType (typeof(NSObject))]
interface SDWebImageDownloader
{
	// @property (assign, nonatomic) NSInteger maxConcurrentDownloads;
	[Export ("maxConcurrentDownloads", ArgumentSemantic.Assign)]
	nint MaxConcurrentDownloads { get; set; }

	// @property (readonly, nonatomic) NSUInteger currentDownloadCount;
	[Export ("currentDownloadCount")]
	nuint CurrentDownloadCount { get; }

	// @property (assign, nonatomic) NSTimeInterval downloadTimeout;
	[Export ("downloadTimeout")]
	double DownloadTimeout { get; set; }

	// @property (assign, nonatomic) SDWebImageDownloaderExecutionOrder executionOrder;
	[Export ("executionOrder", ArgumentSemantic.Assign)]
	SDWebImageDownloaderExecutionOrder ExecutionOrder { get; set; }

	// +(SDWebImageDownloader *)sharedDownloader;
	[Static]
	[Export ("sharedDownloader")]
	[Verify (MethodToProperty)]
	SDWebImageDownloader SharedDownloader { get; }

	// @property (nonatomic, strong) NSString * username;
	[Export ("username", ArgumentSemantic.Strong)]
	string Username { get; set; }

	// @property (nonatomic, strong) NSString * password;
	[Export ("password", ArgumentSemantic.Strong)]
	string Password { get; set; }

	// @property (copy, nonatomic) SDWebImageDownloaderHeadersFilterBlock headersFilter;
	[Export ("headersFilter", ArgumentSemantic.Copy)]
	SDWebImageDownloaderHeadersFilterBlock HeadersFilter { get; set; }

	// -(void)setValue:(NSString *)value forHTTPHeaderField:(NSString *)field;
	[Export ("setValue:forHTTPHeaderField:")]
	void SetValue (string value, string field);

	// -(NSString *)valueForHTTPHeaderField:(NSString *)field;
	[Export ("valueForHTTPHeaderField:")]
	string ValueForHTTPHeaderField (string field);

	// -(id<SDWebImageOperation>)downloadImageWithURL:(NSURL *)url options:(SDWebImageDownloaderOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageDownloaderCompletedBlock)completedBlock;
	[Export ("downloadImageWithURL:options:progress:completed:")]
	SDWebImageOperation DownloadImageWithURL (NSURL url, SDWebImageDownloaderOptions options, SDWebImageDownloaderProgressBlock progressBlock, SDWebImageDownloaderCompletedBlock completedBlock);

	// -(void)setSuspended:(BOOL)suspended;
	[Export ("setSuspended:")]
	void SetSuspended (bool suspended);
}

// typedef void (^SDWebImageQueryCompletedBlock)(UIImage *SDImageCacheType);
delegate void SDWebImageQueryCompletedBlock (UIImage arg0, SDImageCacheType arg1);

// typedef void (^SDWebImageCheckCacheCompletionBlock)(BOOL);
delegate void SDWebImageCheckCacheCompletionBlock (bool arg0);

// typedef void (^SDWebImageCalculateSizeBlock)(NSUIntegerNSUInteger);
delegate void SDWebImageCalculateSizeBlock (nuint arg0, nuint arg1);

// @interface SDImageCache : NSObject
[BaseType (typeof(NSObject))]
interface SDImageCache
{
	// @property (assign, nonatomic) NSUInteger maxMemoryCost;
	[Export ("maxMemoryCost", ArgumentSemantic.Assign)]
	nuint MaxMemoryCost { get; set; }

	// @property (assign, nonatomic) NSInteger maxCacheAge;
	[Export ("maxCacheAge", ArgumentSemantic.Assign)]
	nint MaxCacheAge { get; set; }

	// @property (assign, nonatomic) NSUInteger maxCacheSize;
	[Export ("maxCacheSize", ArgumentSemantic.Assign)]
	nuint MaxCacheSize { get; set; }

	// +(SDImageCache *)sharedImageCache;
	[Static]
	[Export ("sharedImageCache")]
	[Verify (MethodToProperty)]
	SDImageCache SharedImageCache { get; }

	// -(id)initWithNamespace:(NSString *)ns;
	[Export ("initWithNamespace:")]
	IntPtr Constructor (string ns);

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
	NSOperation QueryDiskCacheForKey (string key, SDWebImageQueryCompletedBlock doneBlock);

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
	void RemoveImageForKey (string key, SDWebImageNoParamsBlock completion);

	// -(void)removeImageForKey:(NSString *)key fromDisk:(BOOL)fromDisk;
	[Export ("removeImageForKey:fromDisk:")]
	void RemoveImageForKey (string key, bool fromDisk);

	// -(void)removeImageForKey:(NSString *)key fromDisk:(BOOL)fromDisk withCompletion:(SDWebImageNoParamsBlock)completion;
	[Export ("removeImageForKey:fromDisk:withCompletion:")]
	void RemoveImageForKey (string key, bool fromDisk, SDWebImageNoParamsBlock completion);

	// -(void)clearMemory;
	[Export ("clearMemory")]
	void ClearMemory ();

	// -(void)clearDiskOnCompletion:(SDWebImageNoParamsBlock)completion;
	[Export ("clearDiskOnCompletion:")]
	void ClearDiskOnCompletion (SDWebImageNoParamsBlock completion);

	// -(void)clearDisk;
	[Export ("clearDisk")]
	void ClearDisk ();

	// -(void)cleanDiskWithCompletionBlock:(SDWebImageNoParamsBlock)completionBlock;
	[Export ("cleanDiskWithCompletionBlock:")]
	void CleanDiskWithCompletionBlock (SDWebImageNoParamsBlock completionBlock);

	// -(void)cleanDisk;
	[Export ("cleanDisk")]
	void CleanDisk ();

	// -(NSUInteger)getSize;
	[Export ("getSize")]
	[Verify (MethodToProperty)]
	nuint Size { get; }

	// -(NSUInteger)getDiskCount;
	[Export ("getDiskCount")]
	[Verify (MethodToProperty)]
	nuint DiskCount { get; }

	// -(void)calculateSizeWithCompletionBlock:(SDWebImageCalculateSizeBlock)completionBlock;
	[Export ("calculateSizeWithCompletionBlock:")]
	void CalculateSizeWithCompletionBlock (SDWebImageCalculateSizeBlock completionBlock);

	// -(void)diskImageExistsWithKey:(NSString *)key completion:(SDWebImageCheckCacheCompletionBlock)completionBlock;
	[Export ("diskImageExistsWithKey:completion:")]
	void DiskImageExistsWithKey (string key, SDWebImageCheckCacheCompletionBlock completionBlock);

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

// typedef void (^SDWebImageCompletionBlock)(UIImage *NSError *SDImageCacheTypeNSURL *);
delegate void SDWebImageCompletionBlock (UIImage arg0, NSError arg1, SDImageCacheType arg2, NSURL arg3);

// typedef void (^SDWebImageCompletionWithFinishedBlock)(UIImage *NSError *SDImageCacheTypeBOOLNSURL *);
delegate void SDWebImageCompletionWithFinishedBlock (UIImage arg0, NSError arg1, SDImageCacheType arg2, bool arg3, NSURL arg4);

// typedef NSString * (^SDWebImageCacheKeyFilterBlock)(NSURL *);
delegate string SDWebImageCacheKeyFilterBlock (NSURL arg0);

// @protocol SDWebImageManagerDelegate <NSObject>
[Protocol, Model]
[BaseType (typeof(NSObject))]
interface SDWebImageManagerDelegate
{
	// @optional -(BOOL)imageManager:(SDWebImageManager *)imageManager shouldDownloadImageForURL:(NSURL *)imageURL;
	[Export ("imageManager:shouldDownloadImageForURL:")]
	bool ImageManager (SDWebImageManager imageManager, NSURL imageURL);

	// @optional -(UIImage *)imageManager:(SDWebImageManager *)imageManager transformDownloadedImage:(UIImage *)image withURL:(NSURL *)imageURL;
	[Export ("imageManager:transformDownloadedImage:withURL:")]
	UIImage ImageManager (SDWebImageManager imageManager, UIImage image, NSURL imageURL);
}

// @interface SDWebImageManager : NSObject
[BaseType (typeof(NSObject))]
interface SDWebImageManager
{
	[Wrap ("WeakDelegate")]
	SDWebImageManagerDelegate Delegate { get; set; }

	// @property (nonatomic, weak) id<SDWebImageManagerDelegate> delegate;
	[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
	NSObject WeakDelegate { get; set; }

	// @property (readonly, nonatomic, strong) SDImageCache * imageCache;
	[Export ("imageCache", ArgumentSemantic.Strong)]
	SDImageCache ImageCache { get; }

	// @property (readonly, nonatomic, strong) SDWebImageDownloader * imageDownloader;
	[Export ("imageDownloader", ArgumentSemantic.Strong)]
	SDWebImageDownloader ImageDownloader { get; }

	// @property (copy) SDWebImageCacheKeyFilterBlock cacheKeyFilter;
	[Export ("cacheKeyFilter", ArgumentSemantic.Copy)]
	SDWebImageCacheKeyFilterBlock CacheKeyFilter { get; set; }

	// +(SDWebImageManager *)sharedManager;
	[Static]
	[Export ("sharedManager")]
	[Verify (MethodToProperty)]
	SDWebImageManager SharedManager { get; }

	// -(id<SDWebImageOperation>)downloadImageWithURL:(NSURL *)url options:(SDWebImageOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageCompletionWithFinishedBlock)completedBlock;
	[Export ("downloadImageWithURL:options:progress:completed:")]
	SDWebImageOperation DownloadImageWithURL (NSURL url, SDWebImageOptions options, SDWebImageDownloaderProgressBlock progressBlock, SDWebImageCompletionWithFinishedBlock completedBlock);

	// -(void)saveImageToCache:(UIImage *)image forURL:(NSURL *)url;
	[Export ("saveImageToCache:forURL:")]
	void SaveImageToCache (UIImage image, NSURL url);

	// -(void)cancelAll;
	[Export ("cancelAll")]
	void CancelAll ();

	// -(BOOL)isRunning;
	[Export ("isRunning")]
	[Verify (MethodToProperty)]
	bool IsRunning { get; }

	// -(BOOL)cachedImageExistsForURL:(NSURL *)url;
	[Export ("cachedImageExistsForURL:")]
	bool CachedImageExistsForURL (NSURL url);

	// -(BOOL)diskImageExistsForURL:(NSURL *)url;
	[Export ("diskImageExistsForURL:")]
	bool DiskImageExistsForURL (NSURL url);

	// -(void)cachedImageExistsForURL:(NSURL *)url completion:(SDWebImageCheckCacheCompletionBlock)completionBlock;
	[Export ("cachedImageExistsForURL:completion:")]
	void CachedImageExistsForURL (NSURL url, SDWebImageCheckCacheCompletionBlock completionBlock);

	// -(void)diskImageExistsForURL:(NSURL *)url completion:(SDWebImageCheckCacheCompletionBlock)completionBlock;
	[Export ("diskImageExistsForURL:completion:")]
	void DiskImageExistsForURL (NSURL url, SDWebImageCheckCacheCompletionBlock completionBlock);

	// -(NSString *)cacheKeyForURL:(NSURL *)url;
	[Export ("cacheKeyForURL:")]
	string CacheKeyForURL (NSURL url);
}

// typedef void (^SDWebImageCompletedBlock)(UIImage *NSError *SDImageCacheType);
delegate void SDWebImageCompletedBlock (UIImage arg0, NSError arg1, SDImageCacheType arg2);

// typedef void (^SDWebImageCompletedWithFinishedBlock)(UIImage *NSError *SDImageCacheTypeBOOL);
delegate void SDWebImageCompletedWithFinishedBlock (UIImage arg0, NSError arg1, SDImageCacheType arg2, bool arg3);

// @interface WebCache (UIButton)
[Category]
[BaseType (typeof(UIButton))]
interface UIButton_WebCache
{
	// -(NSURL *)sd_currentImageURL;
	[Export ("sd_currentImageURL")]
	[Verify (MethodToProperty)]
	NSURL Sd_currentImageURL { get; }

	// -(NSURL *)sd_imageURLForState:(UIControlState)state;
	[Export ("sd_imageURLForState:")]
	NSURL Sd_imageURLForState (UIControlState state);

	// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state;
	[Export ("sd_setImageWithURL:forState:")]
	void Sd_setImageWithURL (NSURL url, UIControlState state);

	// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder;
	[Export ("sd_setImageWithURL:forState:placeholderImage:")]
	void Sd_setImageWithURL (NSURL url, UIControlState state, UIImage placeholder);

	// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options;
	[Export ("sd_setImageWithURL:forState:placeholderImage:options:")]
	void Sd_setImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageOptions options);

	// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state completed:(SDWebImageCompletionBlock)completedBlock;
	[Export ("sd_setImageWithURL:forState:completed:")]
	void Sd_setImageWithURL (NSURL url, UIControlState state, SDWebImageCompletionBlock completedBlock);

	// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder completed:(SDWebImageCompletionBlock)completedBlock;
	[Export ("sd_setImageWithURL:forState:placeholderImage:completed:")]
	void Sd_setImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageCompletionBlock completedBlock);

	// -(void)sd_setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options completed:(SDWebImageCompletionBlock)completedBlock;
	[Export ("sd_setImageWithURL:forState:placeholderImage:options:completed:")]
	void Sd_setImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageOptions options, SDWebImageCompletionBlock completedBlock);

	// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state;
	[Export ("sd_setBackgroundImageWithURL:forState:")]
	void Sd_setBackgroundImageWithURL (NSURL url, UIControlState state);

	// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder;
	[Export ("sd_setBackgroundImageWithURL:forState:placeholderImage:")]
	void Sd_setBackgroundImageWithURL (NSURL url, UIControlState state, UIImage placeholder);

	// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options;
	[Export ("sd_setBackgroundImageWithURL:forState:placeholderImage:options:")]
	void Sd_setBackgroundImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageOptions options);

	// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state completed:(SDWebImageCompletionBlock)completedBlock;
	[Export ("sd_setBackgroundImageWithURL:forState:completed:")]
	void Sd_setBackgroundImageWithURL (NSURL url, UIControlState state, SDWebImageCompletionBlock completedBlock);

	// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder completed:(SDWebImageCompletionBlock)completedBlock;
	[Export ("sd_setBackgroundImageWithURL:forState:placeholderImage:completed:")]
	void Sd_setBackgroundImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageCompletionBlock completedBlock);

	// -(void)sd_setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options completed:(SDWebImageCompletionBlock)completedBlock;
	[Export ("sd_setBackgroundImageWithURL:forState:placeholderImage:options:completed:")]
	void Sd_setBackgroundImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageOptions options, SDWebImageCompletionBlock completedBlock);

	// -(void)sd_cancelImageLoadForState:(UIControlState)state;
	[Export ("sd_cancelImageLoadForState:")]
	void Sd_cancelImageLoadForState (UIControlState state);

	// -(void)sd_cancelBackgroundImageLoadForState:(UIControlState)state;
	[Export ("sd_cancelBackgroundImageLoadForState:")]
	void Sd_cancelBackgroundImageLoadForState (UIControlState state);
}

// @interface WebCacheDeprecated (UIButton)
[Category]
[BaseType (typeof(UIButton))]
interface UIButton_WebCacheDeprecated
{
	// -(NSURL *)currentImageURL __attribute__((deprecated("Use `sd_currentImageURL`")));
	[Export ("currentImageURL")]
	[Verify (MethodToProperty)]
	NSURL CurrentImageURL { get; }

	// -(NSURL *)imageURLForState:(UIControlState)state __attribute__((deprecated("Use `sd_imageURLForState:`")));
	[Export ("imageURLForState:")]
	NSURL ImageURLForState (UIControlState state);

	// -(void)setImageWithURL:(NSURL *)url forState:(UIControlState)state __attribute__((deprecated("Method deprecated. Use `sd_setImageWithURL:forState:`")));
	[Export ("setImageWithURL:forState:")]
	void SetImageWithURL (NSURL url, UIControlState state);

	// -(void)setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder __attribute__((deprecated("Method deprecated. Use `sd_setImageWithURL:forState:placeholderImage:`")));
	[Export ("setImageWithURL:forState:placeholderImage:")]
	void SetImageWithURL (NSURL url, UIControlState state, UIImage placeholder);

	// -(void)setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options __attribute__((deprecated("Method deprecated. Use `sd_setImageWithURL:forState:placeholderImage:options:`")));
	[Export ("setImageWithURL:forState:placeholderImage:options:")]
	void SetImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageOptions options);

	// -(void)setImageWithURL:(NSURL *)url forState:(UIControlState)state completed:(SDWebImageCompletedBlock)completedBlock __attribute__((deprecated("Method deprecated. Use `sd_setImageWithURL:forState:completed:`")));
	[Export ("setImageWithURL:forState:completed:")]
	void SetImageWithURL (NSURL url, UIControlState state, SDWebImageCompletedBlock completedBlock);

	// -(void)setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder completed:(SDWebImageCompletedBlock)completedBlock __attribute__((deprecated("Method deprecated. Use `sd_setImageWithURL:forState:placeholderImage:completed:`")));
	[Export ("setImageWithURL:forState:placeholderImage:completed:")]
	void SetImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageCompletedBlock completedBlock);

	// -(void)setImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options completed:(SDWebImageCompletedBlock)completedBlock __attribute__((deprecated("Method deprecated. Use `sd_setImageWithURL:forState:placeholderImage:options:completed:`")));
	[Export ("setImageWithURL:forState:placeholderImage:options:completed:")]
	void SetImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageOptions options, SDWebImageCompletedBlock completedBlock);

	// -(void)setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state __attribute__((deprecated("Method deprecated. Use `sd_setBackgroundImageWithURL:forState:`")));
	[Export ("setBackgroundImageWithURL:forState:")]
	void SetBackgroundImageWithURL (NSURL url, UIControlState state);

	// -(void)setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder __attribute__((deprecated("Method deprecated. Use `sd_setBackgroundImageWithURL:forState:placeholderImage:`")));
	[Export ("setBackgroundImageWithURL:forState:placeholderImage:")]
	void SetBackgroundImageWithURL (NSURL url, UIControlState state, UIImage placeholder);

	// -(void)setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options __attribute__((deprecated("Method deprecated. Use `sd_setBackgroundImageWithURL:forState:placeholderImage:options:`")));
	[Export ("setBackgroundImageWithURL:forState:placeholderImage:options:")]
	void SetBackgroundImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageOptions options);

	// -(void)setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state completed:(SDWebImageCompletedBlock)completedBlock __attribute__((deprecated("Method deprecated. Use `sd_setBackgroundImageWithURL:forState:completed:`")));
	[Export ("setBackgroundImageWithURL:forState:completed:")]
	void SetBackgroundImageWithURL (NSURL url, UIControlState state, SDWebImageCompletedBlock completedBlock);

	// -(void)setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder completed:(SDWebImageCompletedBlock)completedBlock __attribute__((deprecated("Method deprecated. Use `sd_setBackgroundImageWithURL:forState:placeholderImage:completed:`")));
	[Export ("setBackgroundImageWithURL:forState:placeholderImage:completed:")]
	void SetBackgroundImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageCompletedBlock completedBlock);

	// -(void)setBackgroundImageWithURL:(NSURL *)url forState:(UIControlState)state placeholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options completed:(SDWebImageCompletedBlock)completedBlock __attribute__((deprecated("Method deprecated. Use `sd_setBackgroundImageWithURL:forState:placeholderImage:options:completed:`")));
	[Export ("setBackgroundImageWithURL:forState:placeholderImage:options:completed:")]
	void SetBackgroundImageWithURL (NSURL url, UIControlState state, UIImage placeholder, SDWebImageOptions options, SDWebImageCompletedBlock completedBlock);

	// -(void)cancelCurrentImageLoad __attribute__((deprecated("Use `sd_cancelImageLoadForState:`")));
	[Export ("cancelCurrentImageLoad")]
	void CancelCurrentImageLoad ();

	// -(void)cancelBackgroundImageLoadForState:(UIControlState)state __attribute__((deprecated("Use `sd_cancelBackgroundImageLoadForState:`")));
	[Export ("cancelBackgroundImageLoadForState:")]
	void CancelBackgroundImageLoadForState (UIControlState state);
}
