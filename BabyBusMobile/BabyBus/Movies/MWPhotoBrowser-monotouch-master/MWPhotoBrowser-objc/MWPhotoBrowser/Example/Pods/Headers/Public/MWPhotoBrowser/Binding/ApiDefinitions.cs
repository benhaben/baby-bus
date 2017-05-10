using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

// @protocol MWPhoto <NSObject>
[Protocol, Model]
[BaseType (typeof(NSObject))]
interface MWPhoto
{
	// @required @property (nonatomic, strong) UIImage * underlyingImage;
	[Abstract]
	[Export ("underlyingImage", ArgumentSemantic.Strong)]
	UIImage UnderlyingImage { get; set; }

	// @required -(void)loadUnderlyingImageAndNotify;
	[Abstract]
	[Export ("loadUnderlyingImageAndNotify")]
	void LoadUnderlyingImageAndNotify ();

	// @required -(void)performLoadUnderlyingImageAndNotify;
	[Abstract]
	[Export ("performLoadUnderlyingImageAndNotify")]
	void PerformLoadUnderlyingImageAndNotify ();

	// @required -(void)unloadUnderlyingImage;
	[Abstract]
	[Export ("unloadUnderlyingImage")]
	void UnloadUnderlyingImage ();

	// @optional @property (nonatomic) BOOL emptyImage;
	[Export ("emptyImage")]
	bool EmptyImage { get; set; }

	// @optional @property (nonatomic) BOOL isVideo;
	[Export ("isVideo")]
	bool IsVideo { get; set; }

	// @optional -(void)getVideoURL:(void (^)(NSURL *))completion;
	[Export ("getVideoURL:")]
	void GetVideoURL (Action<NSURL> completion);

	// @optional -(NSString *)caption;
	[Export ("caption")]
	[Verify (MethodToProperty)]
	string Caption { get; }

	// @optional -(void)cancelAnyLoading;
	[Export ("cancelAnyLoading")]
	void CancelAnyLoading ();
}

// @interface MWCaptionView : UIToolbar
[BaseType (typeof(UIToolbar))]
interface MWCaptionView
{
	// -(id)initWithPhoto:(id<MWPhoto>)photo;
	[Export ("initWithPhoto:")]
	IntPtr Constructor (MWPhoto photo);

	// -(void)setupCaption;
	[Export ("setupCaption")]
	void SetupCaption ();

	// -(CGSize)sizeThatFits:(CGSize)size;
	[Export ("sizeThatFits:")]
	CGSize SizeThatFits (CGSize size);
}
