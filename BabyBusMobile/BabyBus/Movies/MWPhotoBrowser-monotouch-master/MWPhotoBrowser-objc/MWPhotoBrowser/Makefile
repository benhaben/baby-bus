XBUILD=/Applications/Xcode.app/Contents/Developer/usr/bin/xcodebuild
PROJECT_ROOT=.
PROJECT=$(PROJECT_ROOT)/Example/Pods/Pods.xcodeproj
TARGET=Pods-MWPhotoBrowser_Example
I386=i386
ARMV7=armv7
ARM64=arm64

all: libMWPhotoBrowser.a

libMWPhotoBrowser-$(I386).a:
	$(XBUILD) -project $(PROJECT) -target $(TARGET) -sdk iphonesimulator -configuration Release clean build
	-mv $(PROJECT_ROOT)/Example/build/Release-iphonesimulator/lib$(TARGET).a lib$(TARGET).a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphonesimulator/lib$(TARGET)-DACircularProgress.a lib$(TARGET)-DACircularProgress.a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphonesimulator/lib$(TARGET)-MBProgressHUD.a lib$(TARGET)-MBProgressHUD.a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphonesimulator/lib$(TARGET)-MWPhotoBrowser.a lib$(TARGET)-MWPhotoBrowser.a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphonesimulator/lib$(TARGET)-SDWebImage.a lib$(TARGET)-SDWebImage.a
	lipo lib$(TARGET).a -thin $(I386) -output lib$(TARGET)-$(I386).a
	lipo lib$(TARGET)-DACircularProgress.a -thin $(I386) -output lib$(TARGET)-DACircularProgress-$(I386).a
	lipo lib$(TARGET)-MBProgressHUD.a -thin $(I386) -output lib$(TARGET)-MBProgressHUD-$(I386).a
	lipo lib$(TARGET)-MWPhotoBrowser.a -thin $(I386) -output lib$(TARGET)-MWPhotoBrowser-$(I386).a
	lipo lib$(TARGET)-SDWebImage.a -thin $(I386) -output lib$(TARGET)-SDWebImage-$(I386).a
	ar -x lib$(TARGET)-$(I386).a 
	ar -x lib$(TARGET)-DACircularProgress-$(I386).a 
	ar -x lib$(TARGET)-MBProgressHUD-$(I386).a 
	ar -x lib$(TARGET)-MWPhotoBrowser-$(I386).a 
	ar -x lib$(TARGET)-SDWebImage-$(I386).a
	ar -cru $@ *.o 
	ranlib $@

libMWPhotoBrowser-armv7.a:
	$(XBUILD) -project $(PROJECT) -target $(TARGET) -sdk iphoneos -arch $(ARMV7) -configuration Release clean build
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET).a lib$(TARGET)-$(ARMV7).a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET)-DACircularProgress.a lib$(TARGET)-DACircularProgress-$(ARMV7).a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET)-MBProgressHUD.a lib$(TARGET)-MBProgressHUD-$(ARMV7).a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET)-MWPhotoBrowser.a lib$(TARGET)-MWPhotoBrowser-$(ARMV7).a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET)-SDWebImage.a lib$(TARGET)-SDWebImage-$(ARMV7).a
	ar -x lib$(TARGET)-$(ARMV7).a 
	ar -x lib$(TARGET)-DACircularProgress-$(ARMV7).a 
	ar -x lib$(TARGET)-MBProgressHUD-$(ARMV7).a 
	ar -x lib$(TARGET)-MWPhotoBrowser-$(ARMV7).a 
	ar -x lib$(TARGET)-SDWebImage-$(ARMV7).a
	ar -cru $@ *.o 
	ranlib $@
	
	
libMWPhotoBrowser-arm64.a:
	$(XBUILD) -project $(PROJECT) -target $(TARGET) -sdk iphoneos -arch $(ARM64) -configuration Release clean build
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET).a lib$(TARGET)-$(ARM64).a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET)-DACircularProgress.a lib$(TARGET)-DACircularProgress-$(ARM64).a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET)-MBProgressHUD.a lib$(TARGET)-MBProgressHUD-$(ARM64).a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET)-MWPhotoBrowser.a lib$(TARGET)-MWPhotoBrowser-$(ARM64).a
	-mv $(PROJECT_ROOT)/Example/build/Release-iphoneos/lib$(TARGET)-SDWebImage.a lib$(TARGET)-SDWebImage-$(ARM64).a
	ar -x lib$(TARGET)-$(ARM64).a 
	ar -x lib$(TARGET)-DACircularProgress-$(ARM64).a 
	ar -x lib$(TARGET)-MBProgressHUD-$(ARM64).a 
	ar -x lib$(TARGET)-MWPhotoBrowser-$(ARM64).a 
	ar -x lib$(TARGET)-SDWebImage-$(ARM64).a
	ar -cru $@ *.o 
	ranlib $@

libMWPhotoBrowser.a: libMWPhotoBrowser-i386.a libMWPhotoBrowser-armv7.a libMWPhotoBrowser-arm64.a
	lipo -create -output $@ $^

clean:
	-rm -f *.a *.dll
	-rm -f *.o
	-rm -f __.SYMDEF
	-rm -f "__.SYMDEF SORTED"

