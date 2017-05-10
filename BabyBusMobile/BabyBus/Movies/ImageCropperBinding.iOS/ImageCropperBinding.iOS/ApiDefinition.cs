using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace ImageCropperBinding {
    // The first step to creating a binding is to add your native library ("libNativeLibrary.a")
    // to the project by right-clicking (or Control-clicking) the folder containing this source
    // file and clicking "Add files..." and then simply select the native libraryusing     // that you want to bind.
    //
    // When you do that, you'll notice that MonoDevelop generates a code-behind file for each
    // native library which will contain a [LinkWith] attribute. MonoDevelop auto-detects the
    // architectures that the native library supports and fills in that information for you,
    // however, it cannot auto-detect any Frameworks or other system libraries that the
    // native library may depend on, so you'll need to fill in that information yourself.
    //
    // Once you've done that, you're ready to move on to binding the API...
    //
    //
    // Here is where you'd define your API definition for the native Objective-C library.
    //
    // For example, to bind the following Objective-C class:
    //
    //     @interface Widget : NSObject {
    //     }
    //
    // The C# binding would look like this:
    //
    //     [BaseType (typeof (NSObject))]
    //     interface Widget {
    //     }
    //
    // To bind Objective-C properties, such as:
    //
    //     @property (nonatomic, readwrite, assign) CGPoint center;
    //
    // You would add a property definition in the C# interface like so:
    //
    //     [Export ("center")]
    //     PointF Center { get; set; }
    //
    // To bind an Objective-C method, such as:
    //
    //     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
    //
    // You would add a method definition to the C# interface like so:
    //
    //     [Export ("doSomething:atIndex:")]
    //     void DoSomething (NSObject object, int index);
    //
    // Objective-C "constructors" such as:
    //
    //     -(id)initWithElmo:(ElmoMuppet *)elmo;
    //
    // Can be bound as:
    //
    //     [Export ("initWithElmo:")]
    //     IntPtr Constructor (ElmoMuppet elmo);
    //
    // For more information, see http://docs.xamarin.com/ios/advanced_topics/binding_objective-c_libraries
    //

    [BaseType(typeof(NSObject))]
    [Model, Protocol]
    interface VPImageCropperDelegate {

        [Abstract]
        [Export("imageCropper:didFinished:"),EventArgs("ImageCroppered")]
        void ImageCropper(VPImageCropperViewController cropperViewController, UIImage editedImage);

        [Abstract]
        [Export("imageCropperDidCancel:"),EventArgs("ImageCropperCancel")]
        void ImageCropperDidCancel(VPImageCropperViewController cropperViewController);
    }



    [BaseType(typeof(UIViewController))]
    interface VPImageCropperViewController {
        [Export("tag")]
        nint Tag { get; set; }

        [Export("cropFrame")]
        CGRect CropFrame{ get; set; }

        //        [Export ("dataSource")][NullAllowed]
        //        NSObject WeakPhotoStackViewDataSourceDelegate { get; set; }

        //        [Wrap("WeakDelegate")]
        //        VPImageCropperDelegate Delegate { get; set; }

        [Export("delegate")][NullAllowed]
        NSObject WeakVPImageCropperDelegate { get; set; }


        [Export("initWithImage:cropFrame:limitScaleRatio:")]
        IntPtr Constructor(UIImage originalImage, CGRect cropFrame, nint limitRatio);

        //      - (id)initWithImage:(UIImage *)originalImage cropFrame:(CGRect)cropFrame limitScaleRatio:(NSInteger)limitRatio;
        //      [Export ("initWithImage:cropFrame:limitScaleRatio:")]
        //      void InitWithImage (UIImage originalImage, System.Drawing.RectangleF cropFrame, int limitRatio);
    }
}

//Type mappings
//Objective-C type name Xamarin.iOS type
//BOOL, GLboolean   bool
//NSInteger int
//NSUInteger    uint
//CFTimeInterval / NSTimeInterval   double
//NSString (more on binding NSString)   string
//char *    [PlainString] string
//CGRect    System.Drawing.RectangleF
//CGPoint   System.Drawing.PointF
//CGSize    System.Drawing.SizeF
//CGFloat, GLfloat  float
//CoreFoundation types (CF*)    CoreFoundation.CF*
//GLint int
//GLfloat   float
//Foundation types (NS*)    Foundation.NS*
//id    Foundation.NSObject
//NSGlyph   int
//NSSize    System.Drawing.SizeF
//NSTextAlignment   UITextAlignment
//SEL   ObjCRuntime.Selector
//dispatch_queue_t  CoreFoundation.DispatchQueue
//NSGlyph   uint
//
