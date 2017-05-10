
xcodebuild -project PhotoStack.xcodeproj -target PhotoStackStaticLib -sdk iphonesimulator -configuration Release clean build
cp ./build/Release-iphonesimulator/libPhotoStackStaticLib.a ~/Desktop/libPhotoStackStaticLib-i386.a

xcodebuild -project PhotoStack.xcodeproj -target PhotoStackStaticLib -sdk iphoneos -arch armv7 -configuration Release clean build
cp ./build/Release-iphoneos/libPhotoStackStaticLib.a ~/Desktop/libPhotoStackStaticLib-armv7.a

xcodebuild -project PhotoStack.xcodeproj -target PhotoStackStaticLib -sdk iphoneos -arch armv7s -configuration Release clean build
cp ./build/Release-iphoneos/libPhotoStackStaticLib.a ~/Desktop/libPhotoStackStaticLib-armv7s.a

xcodebuild -project PhotoStack.xcodeproj -target PhotoStackStaticLib -sdk iphoneos -arch arm64 -configuration Release clean build
cp ./build/Release-iphoneos/libPhotoStackStaticLib.a ~/Desktop/libPhotoStackStaticLib-arm64.a

lipo -create -output ~/Desktop/libPhotoStack.a ~/Desktop/libPhotoStackStaticLib-i386.a ~/Desktop/libPhotoStackStaticLib-armv7.a ~/Desktop/libPhotoStackStaticLib-armv7s.a ~/Desktop/libPhotoStackStaticLib-arm64.a