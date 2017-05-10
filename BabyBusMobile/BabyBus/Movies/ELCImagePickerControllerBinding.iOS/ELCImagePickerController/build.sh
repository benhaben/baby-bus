
xcodebuild -project ELCImagePickerDemo.xcodeproj -target ELCImagePicker -sdk iphonesimulator -configuration Release clean build
cp ./build/Release-iphonesimulator/libELCImagePicker.a ~/Desktop/libELCImagePicker-i386.a

xcodebuild -project ELCImagePickerDemo.xcodeproj -target ELCImagePicker -sdk iphoneos -arch armv7 -configuration Release clean build
cp ./build/Release-iphoneos/libELCImagePicker.a ~/Desktop/libELCImagePicker-armv7.a

xcodebuild -project ELCImagePickerDemo.xcodeproj -target ELCImagePicker -sdk iphoneos -arch armv7s -configuration Release clean build
cp ./build/Release-iphoneos/libELCImagePicker.a ~/Desktop/libELCImagePicker-armv7s.a

xcodebuild -project ELCImagePickerDemo.xcodeproj -target ELCImagePicker -sdk iphoneos -arch arm64 -configuration Release clean build
cp ./build/Release-iphoneos/libELCImagePicker.a ~/Desktop/libELCImagePicker-arm64.a

lipo -create -output ~/Desktop/libELCImagePicker.a ~/Desktop/libELCImagePicker-i386.a ~/Desktop/libELCImagePicker-armv7.a ~/Desktop/libELCImagePicker-armv7s.a ~/Desktop/libELCImagePicker-arm64.a