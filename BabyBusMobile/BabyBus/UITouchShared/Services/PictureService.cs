using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Foundation;
using UIKit;
using Utilities.Touch;
using Environment = System.Environment;
using File = System.IO.File;
using Path = System.IO.Path;
using SDWebImage;
using AssetsLibrary;

namespace BabyBus.iOS
{
    public class PictureService : IPictureService
    {
        public string SaveImageInFile(string fileName, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public MemoryStream GetStreamFromFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void LoadIamgeFromSource(string fileName, Action<Stream> iamgeAvailable, string path = "http://babybus.emolbase.com/")
        {
            throw new NotImplementedException();
        }

        public virtual string GetMd5String(byte[] bytes)
        {
            var s = UtilitiesTouch.GetMd5String(bytes);
            return s;
        }


        public virtual Task<Byte[]>  GetImageBytesFromFile(string fileName)
        {
            var tcs = new TaskCompletionSource<Byte[]>();

            try
            {
                
                ALAssetsLibrary al = new ALAssetsLibrary();
                al.AssetForUrl(NSUrl.FromString(fileName), (ALAsset alAsset) =>
                    {
                        NSObject obj = new NSObject();
                        obj.InvokeOnMainThread(() =>
                            {
                                ALAssetRepresentation rep = alAsset.DefaultRepresentation;
                                var cgiImage = rep.GetFullScreenImage();
                                var image = UIImage.FromImage(cgiImage);
                                var smallImage = image.ImageByScalingToMaxSize(320, 2);
                                using (NSData data = smallImage.AsPNG())
                                {
                                    var byteArray = new byte[data.Length];
                                    Marshal.Copy(data.Bytes, byteArray, 0, Convert.ToInt32(data.Length));
                                    tcs.TrySetResult(byteArray);
                                }
                            });
                    }, (NSError error) =>
                    {
                        tcs.TrySetException(new NSErrorException(error));
                        MvxTrace.Trace(MvxTraceLevel.Error, "Photo from asset library error:", error.LocalizedFailureReason);
                    });
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
                MvxTrace.Trace(MvxTraceLevel.Error, "Photo from asset library error:", ex.Message);
            }
            return tcs.Task;
        }
    }
    //class
}

