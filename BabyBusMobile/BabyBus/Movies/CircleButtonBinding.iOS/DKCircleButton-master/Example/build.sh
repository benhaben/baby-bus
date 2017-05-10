
xcodebuild -project CircleButtonDemo.xcodeproj -target CircleButton -sdk iphonesimulator -configuration Release clean build
cp ./build/Release-iphonesimulator/libCircleButton.a ~/Desktop/libCircleButton-i386.a

xcodebuild -project CircleButtonDemo.xcodeproj -target CircleButton -sdk iphoneos -arch armv7 -configuration Release clean build
cp ./build/Release-iphoneos/libCircleButton.a ~/Desktop/libCircleButton-armv7.a

xcodebuild -project CircleButtonDemo.xcodeproj -target CircleButton -sdk iphoneos -arch armv7s -configuration Release clean build
cp ./build/Release-iphoneos/libCircleButton.a ~/Desktop/libCircleButton-armv7s.a

xcodebuild -project CircleButtonDemo.xcodeproj -target CircleButton -sdk iphoneos -arch arm64 -configuration Release clean build
cp ./build/Release-iphoneos/libCircleButton.a ~/Desktop/libCircleButton-arm64.a

lipo -create -output ~/Desktop/libCircleButton.a ~/Desktop/libCircleButton-i386.a ~/Desktop/libCircleButton-armv7.a ~/Desktop/libCircleButton-armv7s.a ~/Desktop/libCircleButton-arm64.a