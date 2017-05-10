using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using Photos;
using UIKit;

namespace MWPhotoBrowserBinding
{
    
    // @interface MWCaptionView : UIToolbar
    [BaseType(typeof(UIToolbar))]
    interface MWCaptionView
    {
        // -(id)initWithPhoto:(id<MWPhoto>)photo;
        [Export("initWithPhoto:")]
        IntPtr Constructor(MWPhoto photo);

        // -(void)setupCaption;
        [Export("setupCaption")]
        void SetupCaption();

        // -(CGSize)sizeThatFits:(CGSize)size;
        [Export("sizeThatFits:")]
        CGSize SizeThatFits(CGSize size);
    }

    [BaseType(typeof(NSObject))]
    public partial interface MWPhoto
    {
        // @required @property (nonatomic, strong) UIImage * underlyingImage;
        [Export("underlyingImage", ArgumentSemantic.Strong)]
        UIImage UnderlyingImage { get; set; }
        
        // @required -(void)loadUnderlyingImageAndNotify;
        [Export("loadUnderlyingImageAndNotify")]
        void LoadUnderlyingImageAndNotify();
        
        // @required -(void)performLoadUnderlyingImageAndNotify;
        [Export("performLoadUnderlyingImageAndNotify")]
        void PerformLoadUnderlyingImageAndNotify();

        // @required -(void)unloadUnderlyingImage;
        [Export("unloadUnderlyingImage")]
        void UnloadUnderlyingImage();

        // @optional -(void)getVideoURL:(void (^)(NSURL *))completion;
        [Export("getVideoURL:")]
        void GetVideoURL(Action<Foundation.NSUrl> completion);

        // @optional -(void)cancelAnyLoading;
        [Export("cancelAnyLoading")]
        void CancelAnyLoading();

        [Notification]
        [Field("MWPHOTO_LOADING_DID_END_NOTIFICATION", "__Internal")]
        NSString MWPhotoEndNotification { get; }

        [Notification]
        [Field("MWPHOTO_PROGRESS_NOTIFICATION", "__Internal")]
        NSString MWPhotoProgressNotification { get; }

        // @property (nonatomic, strong) NSString * caption;
        [Export("caption", ArgumentSemantic.Strong)]
        string Caption { get; set; }

        // @property (nonatomic, strong) NSURL * videoURL;
        [Export("videoURL", ArgumentSemantic.Strong)]
        NSUrl VideoURL { get; set; }

        // @property (nonatomic) BOOL emptyImage;
        [Export("emptyImage")]
        bool EmptyImage { get; set; }

        // @property (nonatomic) BOOL isVideo;
        [Export("isVideo")]
        bool IsVideo { get; set; }

        // +(MWPhoto *)photoWithImage:(UIImage *)image;
        [Static]
        [Export("photoWithImage:")]
        MWPhoto PhotoWithImage(UIImage image);

        // +(MWPhoto *)photoWithURL:(NSURL *)url;
        [Static]
        [Export("photoWithURL:")]
        MWPhoto PhotoWithURL(NSUrl url);

        // +(MWPhoto *)photoWithAsset:(PHAsset *)asset targetSize:(CGSize)targetSize;
        [Static]
        [Export("photoWithAsset:targetSize:")]
        MWPhoto PhotoWithAsset(PHAsset asset, CGSize targetSize);

        // +(MWPhoto *)videoWithURL:(NSURL *)url;
        [Static]
        [Export("videoWithURL:")]
        MWPhoto VideoWithURL(NSUrl url);

        // -(id)initWithImage:(UIImage *)image;
        [Export("initWithImage:")]
        IntPtr Constructor(UIImage image);

        // -(id)initWithURL:(NSURL *)url;
        [Export("initWithURL:")]
        IntPtr Constructor(NSUrl url);

        // -(id)initWithAsset:(PHAsset *)asset targetSize:(C@GSize)targetSize;
        [Export("initWithAsset:targetSize:")]
        IntPtr Constructor(PHAsset asset, CGSize targetSize);

        // -(id)initWithVideoURL:(NSURL *)url;
        [Export("initWithVideoURL:")]
        void InitWithVideoURL(NSUrl url);
    }

    // @protocol MWPhotoBrowserDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface MWPhotoBrowserDelegate
    {
        // @required -(NSUInteger)numberOfPhotosInPhotoBrowser:(MWPhotoBrowser *)photoBrowser;
        [Abstract]
        [Export("numberOfPhotosInPhotoBrowser:"),EventArgs("NumberOfPhotosInPhotoBrowser"),DefaultValue(null)]
        int NumberOfPhotosInPhotoBrowser(MWPhotoBrowser photoBrowser);

        // @required -(id<MWPhoto>)photoBrowser:(MWPhotoBrowser *)photoBrowser photoAtIndex:(NSUInteger)index;
        [Abstract]
        [Export("photoBrowser:photoAtIndex:"),EventArgs("PhotoAtIndex"),DefaultValue(null)]
        MWPhoto PhotoAtIndex(MWPhotoBrowser photoBrowser, uint index);

        // @optional -(id<MWPhoto>)photoBrowser:(MWPhotoBrowser *)photoBrowser thumbPhotoAtIndex:(NSUInteger)index;
        [Export("photoBrowser:thumbPhotoAtIndex:"),EventArgs("ThumbPhotoAtIndex"),DefaultValue(null)]
        MWPhoto ThumbPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index);

        // @optional -(MWCaptionView *)photoBrowser:(MWPhotoBrowser *)photoBrowser captionViewForPhotoAtIndex:(NSUInteger)index;
        [Export("photoBrowser:captionViewForPhotoAtIndex:"),EventArgs("CaptionViewForPhotoAtIndex"),DefaultValue(null)]
        MWCaptionView CaptionViewForPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index);

        // @optional -(NSString *)photoBrowser:(MWPhotoBrowser *)photoBrowser titleForPhotoAtIndex:(NSUInteger)index;
        [Export("photoBrowser:titleForPhotoAtIndex:"),EventArgs("TitleForPhotoAtIndex"),DefaultValue(null)]
        string TitleForPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index);

        // @optional -(void)photoBrowser:(MWPhotoBrowser *)photoBrowser didDisplayPhotoAtIndex:(NSUInteger)index;
        [Export("photoBrowser:didDisplayPhotoAtIndex:"),EventArgs("DidDisplayPhotoAtIndex"),DefaultValue(null)]
        void DidDisplayPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index);

        // @optional -(void)photoBrowser:(MWPhotoBrowser *)photoBrowser actionButtonPressedForPhotoAtIndex:(NSUInteger)index;
        [Export("photoBrowser:actionButtonPressedForPhotoAtIndex:"),EventArgs("ActionButtonPressedForPhotoAtIndex"),DefaultValue(null)]
        void ActionButtonPressedForPhotoAtIndex(MWPhotoBrowser photoBrowser, uint index);


        // @optional -(BOOL)photoBrowser:(MWPhotoBrowser *)photoBrowser isPhotoSelectedAtIndex:(NSUInteger)index;
        [Export("photoBrowser:isPhotoSelectedAtIndex:"),EventArgs("IsPhotoSelectedAtIndex"),DefaultValue(true)]
        bool IsPhotoSelectedAtIndex(MWPhotoBrowser photoBrowser, uint index);

        // @optional -(void)photoBrowser:(MWPhotoBrowser *)photoBrowser photoAtIndex:(NSUInteger)index selectedChanged:(BOOL)selected;
        [Export("photoBrowser:photoAtIndex:selectedChanged:"),EventArgs("PhotoAtIndexSelectedChanged")]
        void PhotoAtIndexSelectedChanged(MWPhotoBrowser photoBrowser, uint index, bool selected);

        [Export("photoBrowserDidFinishModalPresentation:")]
        void PhotoBrowserDidFinishModalPresentation(MWPhotoBrowser photoBrowser);
    }

    // @interface MWPhotoBrowser : UIViewController <UIScrollViewDelegate, UIActionSheetDelegate>
    [BaseType(typeof(UIViewController),
        Delegates = new string [] { "WeakDelegate" },
        Events = new Type [] { typeof(MWPhotoBrowserDelegate) })]
    interface MWPhotoBrowser : IUIScrollViewDelegate, IUIActionSheetDelegate
    {
        [Wrap("WeakDelegate")]
        MWPhotoBrowserDelegate Delegate { get; set; }

        // @property (nonatomic, weak) id<MWPhotoBrowserDelegate> delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @property (nonatomic) BOOL zoomPhotosToFill;
        [Export("zoomPhotosToFill")]
        bool ZoomPhotosToFill { get; set; }

        // @property (nonatomic) BOOL displayNavArrows;
        [Export("displayNavArrows")]
        bool DisplayNavArrows { get; set; }

        // @property (nonatomic) BOOL displayActionButton;
        [Export("displayActionButton")]
        bool DisplayActionButton { get; set; }

        // @property (nonatomic) BOOL displaySelectionButtons;
        [Export("displaySelectionButtons")]
        bool DisplaySelectionButtons { get; set; }

        // @property (nonatomic) BOOL alwaysShowControls;
        [Export("alwaysShowControls")]
        bool AlwaysShowControls { get; set; }

        // @property (nonatomic) BOOL enableGrid;
        [Export("enableGrid")]
        bool EnableGrid { get; set; }

        // @property (nonatomic) BOOL enableSwipeToDismiss;
        [Export("enableSwipeToDismiss")]
        bool EnableSwipeToDismiss { get; set; }

        // @property (nonatomic) BOOL startOnGrid;
        [Export("startOnGrid")]
        bool StartOnGrid { get; set; }

        // @property (nonatomic) BOOL autoPlayOnAppear;
        [Export("autoPlayOnAppear")]
        bool AutoPlayOnAppear { get; set; }

        // @property (nonatomic) NSUInteger delayToHideElements;
        [Export("delayToHideElements", ArgumentSemantic.Assign)]
        nuint DelayToHideElements { get; set; }

        // @property (readonly, nonatomic) NSUInteger currentIndex;
        [Export("currentIndex")]
        nuint CurrentIndex { get; }

        // @property (nonatomic, strong) NSString * customImageSelectedIconName;
        [Export("customImageSelectedIconName", ArgumentSemantic.Strong)]
        string CustomImageSelectedIconName { get; set; }

        // @property (nonatomic, strong) NSString * customImageSelectedSmallIconName;
        [Export("customImageSelectedSmallIconName", ArgumentSemantic.Strong)]
        string CustomImageSelectedSmallIconName { get; set; }

        // -(id)initWithPhotos:(NSArray *)photosArray;
        [Export("initWithPhotos:")]
        IntPtr Constructor(NSObject[] photosArray);

        // -(id)initWithDelegate:(id<MWPhotoBrowserDelegate>)delegate;
        [Export("initWithDelegate:")]
        IntPtr Constructor(MWPhotoBrowserDelegate mwPhotoBrowserDelegate);

        // -(void)reloadData;
        [Export("reloadData")]
        void ReloadData();

        // -(void)setCurrentPhotoIndex:(NSUInteger)index;
        [Export("setCurrentPhotoIndex:")]
        void SetCurrentPhotoIndex(nuint index);

        // -(void)showNextPhotoAnimated:(BOOL)animated;
        [Export("showNextPhotoAnimated:")]
        void ShowNextPhotoAnimated(bool animated);

        // -(void)showPreviousPhotoAnimated:(BOOL)animated;
        [Export("showPreviousPhotoAnimated:")]
        void ShowPreviousPhotoAnimated(bool animated);
    }



}

namespace SDWebImage
{
    // @protocol SDWebImageManagerDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SDWebImageManagerDelegate
    {
        // @optional -(BOOL)imageManager:(SDWebImageManager *)imageManager shouldDownloadImageForURL:(NSURL *)imageURL;
        [Export("imageManager:shouldDownloadImageForURL:")]
        bool ImageManager(SDWebImageManager imageManager, NSUrl imageURL);

        // @optional -(UIImage *)imageManager:(SDWebImageManager *)imageManager transformDownloadedImage:(UIImage *)image withURL:(NSURL *)imageURL;
        [Export("imageManager:transformDownloadedImage:withURL:")]
        UIImage ImageManager(SDWebImageManager imageManager, UIImage image, NSUrl imageURL);
    }

  
    // typedef void (^SDWebImageCalculateSizeBlock)(NSUIntegerNSUInteger);
    delegate void SDWebImageCalculateSizeBlock(nuint arg0,nuint arg1);

    // typedef NSString * (^SDWebImageCacheKeyFilterBlock)(NSURL *);
    delegate string SDWebImageCacheKeyFilterBlock(NSUrl arg0);

    delegate void SDWebImageCheckCacheCompletionBlock(bool arg0);

    // typedef NSDictionary * (^SDWebImageDownloaderHeadersFilterBlock)(NSURL *NSDictionary *);
    delegate NSDictionary SDWebImageDownloaderHeadersFilterBlock(NSUrl arg0,NSDictionary arg1);

    // typedef void (^SDWebImageCompletionBlock)(UIImage *NSError *SDImageCacheTypeNSURL *);
    delegate void SDWebImageCompletionBlock(UIImage arg0,NSError arg1,SDImageCacheType arg2,NSUrl arg3);

    // typedef void (^SDWebImageDownloaderProgressBlock)(NSIntegerNSInteger);
    delegate void SDWebImageDownloaderProgressBlock(nint arg0,nint arg1);

    // typedef void (^SDWebImageDownloaderCompletedBlock)(UIImage *NSData *NSError *BOOL);
    delegate void SDWebImageDownloaderCompletedBlock(UIImage arg0,NSData arg1,NSError arg2,bool arg3);

    // typedef void (^SDWebImageCompletionWithFinishedBlock)(UIImage *NSError *SDImageCacheTypeBOOLNSURL *);
    delegate void SDWebImageCompletionWithFinishedBlock(UIImage arg0,NSError arg1,SDImageCacheType arg2,bool arg3,NSUrl arg4);

    // typ

    [BaseType(typeof(NSObject))]
    interface SDWebImageDownloader
    {
        [Export("maxConcurrentDownloads", ArgumentSemantic.Assign)]
        int MaxConcurrentDownloads { get; set; }

        [Export("currentDownloadCount")]
        nuint CurrentDownloadCount { get; }

        // @property (assign, nonatomic) NSTimeInterval downloadTimeout;
        [Export("downloadTimeout")]
        double DownloadTimeout { get; set; }

        [Export("executionOrder", ArgumentSemantic.Assign)]
        SDWebImageDownloaderExecutionOrder ExecutionOrder { get; set; }

        [Static, Export("sharedDownloader")]
        SDWebImageDownloader SharedDownloader { get; }

        [Export("username", ArgumentSemantic.Strong)]
        string Username { get; set; }

        // @property (nonatomic, strong) NSString * password;
        [Export("password", ArgumentSemantic.Strong)]
        string Password { get; set; }

        // @property (copy, nonatomic) SDWebImageDownloaderHeadersFilterBlock headersFilter;
        [Export("headersFilter", ArgumentSemantic.Copy)]
        SDWebImageDownloaderHeadersFilterBlock HeadersFilter { get; set; }

        [Export("setValue:forHTTPHeaderField:")]
        void SetHTTPHeaderValue(string value, string field);

        [Export("valueForHTTPHeaderField:")]
        string GetHTTPHeaderValue(string field);

        // -(id<SDWebImageOperation>)downloadImageWithURL:(NSURL *)url options:(SDWebImageDownloaderOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageDownloaderCompletedBlock)completedBlock;
        [Export("downloadImageWithURL:options:progress:completed:")]
        SDWebImageOperation DownloadImageWithURL(NSUrl url, SDWebImageDownloaderOptions options, SDWebImageDownloaderProgressBlock progressBlock, SDWebImageDownloaderCompletedBlock completedBlock);

        [Export("setSuspended:")]
        void SetSuspended(bool suspended);
    }

    // @protocol SDWebImageOperation <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface SDWebImageOperation
    {
        // @required -(void)cancel;
        [Abstract]
        [Export("cancel")]
        void Cancel();
    }


    //    interface ISDWebImageOperation {
    //    }

    [Category]
    [BaseType(typeof(UIImageView))]
    interface UIImageView_WebCache
    {
        #region UIImageView

        [Static]
        [Export("sd_imageURL")]
        NSUrl ImageURL { get; }

        [Export("sd_setImageWithURL:")]
        void SetImage(NSUrl url);

        [Export("sd_setImageWithURL:placeholderImage:")]
        void SetImage(NSUrl url, [NullAllowed] UIImage placeholder);

        [Export("sd_setImageWithURL:placeholderImage:options:")]
        void SetImage(NSUrl url, [NullAllowed] UIImage placeholder, SDWebImageOptions options);

        [Export("sd_setImageWithURL:completed:")]
        void SetImage(NSUrl url, SDWebImageCompletionBlock completedBlock);

        [Export("sd_setImageWithURL:placeholderImage:completed:")]
        void SetImage(NSUrl url, [NullAllowed] UIImage placeholder, [NullAllowed] SDWebImageCompletionBlock completedBlock);

        [Export("sd_setImageWithURL:placeholderImage:options:completed:")]
        void SetImage(NSUrl url, [NullAllowed] UIImage placeholder, SDWebImageOptions options, [NullAllowed] SDWebImageCompletionBlock completedBlock);

        [Export("sd_setImageWithURL:placeholderImage:options:progress:completed:")]
        void SetImage(NSUrl url, [NullAllowed] UIImage placeholder, SDWebImageOptions options, [NullAllowed] SDWebImageDownloaderProgressBlock progress, [NullAllowed] SDWebImageCompletionBlock completedBlock);

        // -(void)sd_setImageWithPreviousCachedImageWithURL:(NSURL *)url andPlaceholderImage:(UIImage *)placeholder options:(SDWebImageOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageCompletionBlock)completedBlock;
        [Export("sd_setImageWithPreviousCachedImageWithURL:andPlaceholderImage:options:progress:completed:")]
        void Sd_setImageWithPreviousCachedImageWithURL(NSUrl url, UIImage placeholder, SDWebImageOptions options, SDWebImageDownloaderProgressBlock progressBlock, SDWebImageCompletionBlock completedBlock);


        [Export("sd_setAnimationImagesWithURLs:")]
        void SetAnimationImages(NSUrl[] urls);

        [Export("sd_cancelCurrentImageLoad")]
        void CancelCurrentImageLoad();

        [Export("sd_cancelCurrentAnimationImagesLoad")]
        void CancelCurrentArrayLoad();

        #endregion
    }

    [Category]
    [BaseType(typeof(UIButton))]
    interface UIButton_WebCache
    {
        #region UIButton

        [Static]
        [Export("sd_currentImageURL")]
        NSUrl Sd_currentImageURL { get; }

        // -(NSURL *)sd_imageURLForState:(UIControlState)state;
        [Export("sd_imageURLForState:")]
        NSUrl Sd_imageURLForState(UIControlState state);

        [Export("sd_setImageWithURL:forState:")]
        void SetImage(NSUrl url, UIControlState state);

        [Export("sd_setImageWithURL:forState:placeholderImage:")]
        void SetImage(NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder);

        [Export("sd_setImageWithURL:forState:placeholderImage:options:")]
        void SetImage(NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, SDWebImageOptions options);

        [Export("sd_setImageWithURL:forState:completed:")]
        void SetImage(NSUrl url, UIControlState state, [NullAllowed] SDWebImageCompletionBlock completedBlock);

        [Export("sd_setImageWithURL:forState:placeholderImage:completed:")]
        void SetImage(NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, [NullAllowed] SDWebImageCompletionBlock completedBlock);

        [Export("sd_setImageWithURL:forState:placeholderImage:options:completed:")]
        void SetImage(NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, SDWebImageOptions options, [NullAllowed] SDWebImageCompletionBlock completedBlock);

        [Export("sd_setBackgroundImageWithURL:forState:")]
        void SetBackgroundImage(NSUrl url, UIControlState state);

        [Export("sd_setBackgroundImageWithURL:forState:placeholderImage:")]
        void SetBackgroundImage(NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder);

        [Export("sd_setBackgroundImageWithURL:forState:placeholderImage:options:")]
        void SetBackgroundImage(NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, SDWebImageOptions options);

        [Export("sd_setBackgroundImageWithURL:forState:completed:")]
        void SetBackgroundImage(NSUrl url, UIControlState state, [NullAllowed] SDWebImageCompletionBlock completedBlock);

        [Export("sd_setBackgroundImageWithURL:forState:placeholderImage:completed:")]
        void SetBackgroundImage(NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, [NullAllowed] SDWebImageCompletionBlock completedBlock);

        [Export("sd_setBackgroundImageWithURL:forState:placeholderImage:options:completed:")]
        void SetBackgroundImage(NSUrl url, UIControlState state, [NullAllowed] UIImage placeholder, SDWebImageOptions options, [NullAllowed] SDWebImageCompletionBlock completedBlock);

        // -(void)sd_cancelImageLoadForState:(UIControlState)state;
        [Export("sd_cancelImageLoadForState:")]
        void Sd_cancelImageLoadForState(UIControlState state);

        // -(void)sd_cancelBackgroundImageLoadForState:(UIControlState)state;
        [Export("sd_cancelBackgroundImageLoadForState:")]
        void Sd_cancelBackgroundImageLoadForState(UIControlState state);

        #endregion
    }

    [BaseType(typeof(NSObject))]
    interface SDWebImageManager
    {
        [Wrap("WeakDelegate")]
        SDWebImageManagerDelegate Delegate { get; set; }

        // @property (nonatomic, weak) id<SDWebImageManagerDelegate> delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @property (readonly, nonatomic, strong) SDImageCache * imageCache;
        [Export("imageCache", ArgumentSemantic.Strong)]
        SDImageCache ImageCache { get; }

        // @property (readonly, nonatomic, strong) SDWebImageDownloader * imageDownloader;
        [Export("imageDownloader", ArgumentSemantic.Strong)]
        SDWebImageDownloader ImageDownloader { get; }

        // @property (copy) SDWebImageCacheKeyFilterBlock cacheKeyFilter;
        //        [Export("cacheKeyFilter", ArgumentSemantic.Copy)]
        //        SDWebImageCacheKeyFilterBlock CacheKeyFilter { get; set; }

        // +(SDWebImageManager *)sharedManager;
        [Static]
        [Export("sharedManager")]
        SDWebImageManager SharedManager { get; }

        // -(id<SDWebImageOperation>)downloadImageWithURL:(NSURL *)url options:(SDWebImageOptions)options progress:(SDWebImageDownloaderProgressBlock)progressBlock completed:(SDWebImageCompletionWithFinishedBlock)completedBlock;
        [Export("downloadImageWithURL:options:progress:completed:")]
        SDWebImageOperation DownloadImageWithURL(NSUrl url, SDWebImageOptions options, SDWebImageDownloaderProgressBlock progressBlock, SDWebImageCompletionWithFinishedBlock completedBlock);

        // -(void)saveImageToCache:(UIImage *)image forURL:(NSURL *)url;
        [Export("saveImageToCache:forURL:")]
        void SaveImageToCache(UIImage image, NSUrl url);

        [Export("cancelAll")]
        void CancelAll();

        [Export("isRunning")]
        bool IsRunning();

        // -(BOOL)cachedImageExistsForURL:(NSURL *)url;
        [Export("cachedImageExistsForURL:")]
        bool CachedImageExistsForURL(NSUrl url);

        // -(BOOL)diskImageExistsForURL:(NSURL *)url;
        [Export("diskImageExistsForURL:")]
        bool DiskImageExistsForURL(NSUrl url);

        // -(void)cachedImageExistsForURL:(NSURL *)url completion:(SDWebImageCheckCacheCompletionBlock)completionBlock;
        [Export("cachedImageExistsForURL:completion:")]
        void CachedImageExistsForURL(NSUrl url, SDWebImageCheckCacheCompletionBlock completionBlock);

        // -(void)diskImageExistsForURL:(NSURL *)url completion:(SDWebImageCheckCacheCompletionBlock)completionBlock;
        [Export("diskImageExistsForURL:completion:")]
        void DiskImageExistsForURL(NSUrl url, SDWebImageCheckCacheCompletionBlock completionBlock);

        // -(NSString *)cacheKeyForURL:(NSURL *)url;
        [Export("cacheKeyForURL:")]
        string CacheKeyForURL(NSUrl url);


    }

    // typedef void (^SDWebImageQueryCompletedBlock)(UIImage *SDImageCacheType);
    delegate void SDWebImageQueryCompletedBlock(UIImage arg0,SDImageCacheType arg1);
    delegate void SDWebImageNoParamsBlock();

    [BaseType(typeof(NSObject))]
    interface SDImageCache
    {
        // @property (assign, nonatomic) NSUInteger maxMemoryCost;
        [Export("maxMemoryCost", ArgumentSemantic.Assign)]
        nuint MaxMemoryCost { get; set; }

        // @property (assign, nonatomic) NSInteger maxCacheAge;
        [Export("maxCacheAge", ArgumentSemantic.Assign)]
        nint MaxCacheAge { get; set; }

        // @property (assign, nonatomic) NSUInteger maxCacheSize;
        [Export("maxCacheSize", ArgumentSemantic.Assign)]
        nuint MaxCacheSize { get; set; }

        // +(SDImageCache *)sharedImageCache;
        [Static]
        [Export("sharedImageCache")]
        SDImageCache SharedImageCache { get; }

        // -(id)initWithNamespace:(NSString *)ns;
        [Export("initWithNamespace:")]
        IntPtr Constructor(string ns);

        // -(void)addReadOnlyCachePath:(NSString *)path;
        [Export("addReadOnlyCachePath:")]
        void AddReadOnlyCachePath(string path);

        // -(void)storeImage:(UIImage *)image forKey:(NSString *)key;
        [Export("storeImage:forKey:")]
        void StoreImage(UIImage image, string key);

        // -(void)storeImage:(UIImage *)image forKey:(NSString *)key toDisk:(BOOL)toDisk;
        [Export("storeImage:forKey:toDisk:")]
        void StoreImage(UIImage image, string key, bool toDisk);

        // -(void)storeImage:(UIImage *)image recalculateFromImage:(BOOL)recalculate imageData:(NSData *)imageData forKey:(NSString *)key toDisk:(BOOL)toDisk;
        [Export("storeImage:recalculateFromImage:imageData:forKey:toDisk:")]
        void StoreImage(UIImage image, bool recalculate, NSData imageData, string key, bool toDisk);

        // -(NSOperation *)queryDiskCacheForKey:(NSString *)key done:(SDWebImageQueryCompletedBlock)doneBlock;
        [Export("queryDiskCacheForKey:done:")]
        NSOperation QueryDiskCacheForKey(string key, SDWebImageQueryCompletedBlock doneBlock);

        // -(UIImage *)imageFromMemoryCacheForKey:(NSString *)key;
        [Export("imageFromMemoryCacheForKey:")]
        UIImage ImageFromMemoryCacheForKey(string key);

        // -(UIImage *)imageFromDiskCacheForKey:(NSString *)key;
        [Export("imageFromDiskCacheForKey:")]
        UIImage ImageFromDiskCacheForKey(string key);

        // -(void)removeImageForKey:(NSString *)key;
        [Export("removeImageForKey:")]
        void RemoveImageForKey(string key);

        // -(void)removeImageForKey:(NSString *)key withCompletion:(SDWebImageNoParamsBlock)completion;
        [Export("removeImageForKey:withCompletion:")]
        void RemoveImageForKey(string key, SDWebImageNoParamsBlock completion);

        // -(void)removeImageForKey:(NSString *)key fromDisk:(BOOL)fromDisk;
        [Export("removeImageForKey:fromDisk:")]
        void RemoveImageForKey(string key, bool fromDisk);

        // -(void)removeImageForKey:(NSString *)key fromDisk:(BOOL)fromDisk withCompletion:(SDWebImageNoParamsBlock)completion;
        [Export("removeImageForKey:fromDisk:withCompletion:")]
        void RemoveImageForKey(string key, bool fromDisk, SDWebImageNoParamsBlock completion);

        // -(void)clearMemory;
        [Export("clearMemory")]
        void ClearMemory();

        // -(void)clearDiskOnCompletion:(SDWebImageNoParamsBlock)completion;
        [Export("clearDiskOnCompletion:")]
        void ClearDiskOnCompletion(SDWebImageNoParamsBlock completion);

        // -(void)clearDisk;
        [Export("clearDisk")]
        void ClearDisk();

        // -(void)cleanDiskWithCompletionBlock:(SDWebImageNoParamsBlock)completionBlock;
        [Export("cleanDiskWithCompletionBlock:")]
        void CleanDiskWithCompletionBlock(SDWebImageNoParamsBlock completionBlock);

        // -(void)cleanDisk;
        [Export("cleanDisk")]
        void CleanDisk();

        // -(NSUInteger)getSize;
        [Export("getSize")]
        nuint Size { get; }

        // -(NSUInteger)getDiskCount;
        [Export("getDiskCount")]
        nuint DiskCount { get; }

        // -(void)calculateSizeWithCompletionBlock:(SDWebImageCalculateSizeBlock)completionBlock;
        [Export("calculateSizeWithCompletionBlock:")]
        void CalculateSizeWithCompletionBlock(SDWebImageCalculateSizeBlock completionBlock);

        // -(void)diskImageExistsWithKey:(NSString *)key completion:(SDWebImageCheckCacheCompletionBlock)completionBlock;
        [Export("diskImageExistsWithKey:completion:")]
        void DiskImageExistsWithKey(string key, SDWebImageCheckCacheCompletionBlock completionBlock);

        // -(BOOL)diskImageExistsWithKey:(NSString *)key;
        [Export("diskImageExistsWithKey:")]
        bool DiskImageExistsWithKey(string key);

        // -(NSString *)cachePathForKey:(NSString *)key inPath:(NSString *)path;
        [Export("cachePathForKey:inPath:")]
        string CachePathForKey(string key, string path);

        // -(NSString *)defaultCachePathForKey:(NSString *)key;
        [Export("defaultCachePathForKey:")]
        string DefaultCachePathForKey(string key);
    }
}
