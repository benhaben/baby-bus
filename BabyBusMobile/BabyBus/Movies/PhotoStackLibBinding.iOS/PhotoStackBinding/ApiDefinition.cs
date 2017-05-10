using System;
using System.Drawing;

using ObjCRuntime;
using Foundation;
using UIKit;

namespace PhotoStackBinding {
    // The first step to creating a binding is to add your native library ("libNativeLibrary.a")
    // to the project by right-clicking (or Control-clicking) the folder containing this source
    // file and clicking "Add files..." and then simply select the native library (or libraries)
    // that you want to bind.
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
    //     void DoSomething (NSObject object, nint index);
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
    [Model]
    [Protocol]
    interface PhotoStackViewDataSource {
        [Abstract]
        [Export("numberOfPhotosInPhotoStackView:")]
        nint NumberOfPhotosInPhotoStackView(PhotoStackView photoStack);

        [Abstract]
        [Export("photoStackView:photoForIndex:")]
        UIImage PhotoStackViewPhotoForIndex(PhotoStackView photoStack, nint index);

        //		[Abstract]
        [Export("photoStackView:photoSizeForIndex")]
        SizeF PhotoStackViewPhotoSizeForIndex(PhotoStackView photoStack, nint index);
    }

    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    interface PhotoStackViewDelegate {
        [Abstract]
        [Export("photoStackView:willStartMovingPhotoAtIndex:")]
        void PhotoStackViewWillStartMovingPhotoAtIndex(PhotoStackView photoStackView, nint index);

        [Abstract]
        [Export("photoStackView:willFlickAwayPhotoFromIndex:toIndex:")]
        void PhotoStackViewWillFlickAwayPhotoFromIndex(PhotoStackView photoStackView, nint fromIndex, nint toIndex);

        [Abstract]
        [Export("photoStackView:didRevealPhotoAtIndex:")]
        void PhotoStackViewDidRevealPhotoAtIndex(PhotoStackView photoStackView, nint index);

        [Abstract]
        [Export("photoStackView:didSelectPhotoAtIndex:")]
        void PhotoStackViewDidSelectPhotoAtIndex(PhotoStackView photoStackView, nint index);
    }

    //	[Protocol]
    //	[DisableDefaultCtor]
    [BaseType(typeof(UIControl))]
    interface PhotoStackView : IUIGestureRecognizerDelegate {
        [Export("dataSource")][NullAllowed]
        NSObject WeakPhotoStackViewDataSourceDelegate { get; set; }

        //		[Wrap ("WeakPhotoStackViewDataSourceDelegate")]
        //		PhotoStackViewDataSource PhotoStackViewDataSourceDelegate  { get; set; }

        [Export("delegate")][NullAllowed]
        NSObject WeakPhotoStackViewDelegate { get; set; }

        //		[Wrap ("WeakPhotoStackViewDelegate")]
        //		PhotoStackViewDelegate PhotoStackViewDelegate  { get; set; }


        //remove borderimage in the file.a
        [Export("borderImage")]
        UIImage BorderImage{ get; set; }

        [Export("showBorder")]
        bool ShowBorder{ get; set; }

        [Export("borderWidth")]
        nfloat BorderWidth{ get; set; }

        [Export("rotationOffset")]
        nfloat RotationOffset{ get; set; }

        [Export("highlightColor")]
        UIColor highlightColor{ get; set; }

        [Export("indexOfTopPhoto")]
        void IndexOfTopPhoto();

        [Export("goToPhotoAtIndex:")]
        void goToPhotoAtIndex(nuint index);

        [Export("hidePhotoAtIndex:")]
        void HidePhotoAtIndex(nuint index);

        [Export("lipToNextPhoto")]
        void FlipToNextPhoto();

        [Export("reloadData")]
        void ReloadData();

        [Export("setPhotoViews:")]
        void SetPhotoViews(NSObject[] photoViews);
    }
}

