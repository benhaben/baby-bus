using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using CoolBeans.Pages;
using CoolBeans.Services;
using CoolBeans.Utilities;
using Xamarin.Forms;
using CoolBeans.Services.Media;
using Xamarin.Forms.Labs.Controls;

namespace CoolBeans.Controls
{
    public class ImageSelectButton : Button
    {
        private IMediaPicker _mediaPicker;
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }


        public ImageSelectButton(NeedSelectImageContentPage page, int imageWidth = 40, int imageHeight = 40)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;

            var imageButton = this;
            imageButton.Text = " 宝贝照片 ";
            imageButton.VerticalOptions = LayoutOptions.EndAndExpand;
            imageButton.Clicked += async (sender, e) =>
            {
                var action = await page.DisplayActionSheet("请选择", "直接拍照", "取消", "选取照片");
                if (action == "直接拍照")
                {
                    await TakePicture(page);
                }
                else if (action == "选取照片")
                {
                    await SelectPicture(page);
                }
            };

            imageButton.BackgroundColor = Color.Green;
            imageButton.BorderColor = Color.Green;
            imageButton.TextColor = Color.White;
        }
        private void Setup()
        {
            if (_mediaPicker != null)
            {
                return;
            }

            _mediaPicker = DependencyService.Get<IMediaPicker>();
        }

        //TODO: parameter page is ver ugly
        private async Task<SmartFileImageSource> SelectPicture(NeedSelectImageContentPage page)
        {
            Setup();
            SmartFileImageSource ret = null;
            try
            {
                var mediaFile = await this._mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
                {
                    DefaultCamera = CameraDevice.Rear,
                    MaxPixelDimension = 10
                });

                var compress = Mvx.Resolve<IPictureService>();
                string newFilePath = null;
                page.NeedSelectImageViewModel.MemoryStreamOfImage
                    = compress.GetSmallStreamFromFile(mediaFile.Path, ImageWidth, ImageHeight, ref newFilePath);

                page.NeedSelectImageViewModel.TheImageSource
                               = new SmartFileImageSource(mediaFile.Path, newFilePath, newFilePath);

                ret = page.NeedSelectImageViewModel.TheImageSource;
                mediaFile.Dispose();

            }
            catch (System.Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, "[SelectPicture] \t" + ex.Message);
            }
            return ret;
        }

        private NeedSelectImageContentPage _page;
        private async Task<SmartFileImageSource> TakePicture(NeedSelectImageContentPage page)
        {
            Setup();
            _page = page;

            SmartFileImageSource ret = await
                this._mediaPicker.TakePhotoAsync(
                    new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 })
                    .ContinueWith(t =>
                    {
                        SmartFileImageSource imageSource = null;
                        if (t.IsFaulted)
                        {
                            page.NeedSelectImageViewModel.MemoryStreamOfImage = null;
                            page.NeedSelectImageViewModel.TheImageSource = null;
                            Mvx.Trace(MvxTraceLevel.Error, "[TakePicture] \t" + t.Exception.InnerException.ToString());

                        }
                        else if (t.IsCanceled)
                        {
                            page.NeedSelectImageViewModel.TheImageSource = null;
                            page.NeedSelectImageViewModel.MemoryStreamOfImage = null;
                            Mvx.Trace(MvxTraceLevel.Error, "[TakePicture] \t" + "Canceled");
                        }
                        else
                        {
                            var mediaFile = t.Result;
                            var compress = Mvx.Resolve<IPictureService>();
                            string newFilePath = null;
                            page.NeedSelectImageViewModel.MemoryStreamOfImage
                                = compress.GetSmallStreamFromFile(mediaFile.Path, ImageWidth, ImageHeight, ref newFilePath);
                            page.NeedSelectImageViewModel.TheImageSource
                                = new SmartFileImageSource(mediaFile.Path, newFilePath, newFilePath);
                            imageSource = page.NeedSelectImageViewModel.TheImageSource;
                            mediaFile.Dispose();
                        }
                        return imageSource;
                    }, _scheduler);
            return ret;
        }

    }
}
