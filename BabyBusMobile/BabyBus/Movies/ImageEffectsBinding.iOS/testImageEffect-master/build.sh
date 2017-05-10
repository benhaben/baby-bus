
xcodebuild -project testImageEffect.xcodeproj -target ImageEffect -sdk iphonesimulator -configuration Release clean build
cp ./build/Release-iphonesimulator/libimageEffect.a ~/Desktop/libimageEffect-i386.a

xcodebuild -project testImageEffect.xcodeproj -target ImageEffect -sdk iphoneos -arch armv7 -configuration Release clean build
cp ./build/Release-iphoneos/libimageEffect.a ~/Desktop/libimageEffect-armv7.a

xcodebuild -project testImageEffect.xcodeproj -target ImageEffect -sdk iphoneos -arch armv7s -configuration Release clean build
cp ./build/Release-iphoneos/libimageEffect.a ~/Desktop/libimageEffect-armv7s.a

xcodebuild -project testImageEffect.xcodeproj -target ImageEffect -sdk iphoneos -arch arm64 -configuration Release clean build
cp ./build/Release-iphoneos/libimageEffect.a ~/Desktop/libimageEffect-arm64.a

lipo -create -output ~/Desktop/libImageEffect.a ~/Desktop/libimageEffect-i386.a ~/Desktop/libimageEffect-armv7.a ~/Desktop/libimageEffect-armv7s.a ~/Desktop/libimageEffect-arm64.a