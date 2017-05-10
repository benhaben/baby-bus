XBUILD=/Applications/Xcode.app/Contents/Developer/usr/bin/xcodebuild 
PROJECT_ROOT=. 
PROJECT=$(PROJECT_ROOT)/Gigya.xcodeproj 
TARGET=Gigya

all: libGigya.a

libGigya-i386.a: $(XBUILD) -project $(PROJECT) -target $(TARGET) -sdk iphonesimulator -configuration Release clean build -mv $(PROJECT_ROOT)/build/Release-iphonesimulator/lib$(TARGET).a $@

libGigya-armv7.a: $(XBUILD) -project $(PROJECT) -target $(TARGET) -sdk iphoneos -arch armv7 -configuration Release clean build -mv $(PROJECT_ROOT)/build/Release-iphoneos/lib$(TARGET).a $@

libGigya.a: libGigya-i386.a libGigya-armv7.a lipo -create -output $@ $^

clean: -rm -f *.a *.dll