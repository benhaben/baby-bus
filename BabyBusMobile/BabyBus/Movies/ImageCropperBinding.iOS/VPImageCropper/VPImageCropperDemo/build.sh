
xcodebuild -project VPImageCropperDemo.xcodeproj -target ImageCropper -sdk iphonesimulator -configuration Release clean build
cp ./build/Release-iphonesimulator/libImageCropper.a ~/Desktop/libImageCropper-i386.a

xcodebuild -project VPImageCropperDemo.xcodeproj -target ImageCropper -sdk iphoneos -arch armv7 -configuration Release clean build
cp ./build/Release-iphoneos/libImageCropper.a ~/Desktop/libImageCropper-armv7.a

xcodebuild -project VPImageCropperDemo.xcodeproj -target ImageCropper -sdk iphoneos -arch armv7s -configuration Release clean build
cp ./build/Release-iphoneos/libImageCropper.a ~/Desktop/libImageCropper-armv7s.a

xcodebuild -project VPImageCropperDemo.xcodeproj -target ImageCropper -sdk iphoneos -arch arm64 -configuration Release clean build
cp ./build/Release-iphoneos/libImageCropper.a ~/Desktop/libImageCropper-arm64.a

lipo -create -output ~/Desktop/libImageCropper.a ~/Desktop/libImageCropper-i386.a ~/Desktop/libImageCropper-armv7.a ~/Desktop/libImageCropper-armv7s.a ~/Desktop/libImageCropper-arm64.a