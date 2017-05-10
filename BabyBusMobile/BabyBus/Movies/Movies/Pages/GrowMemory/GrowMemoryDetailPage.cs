
using System;
using System.Collections.Generic;
using System.Threading;
using CoolBeans.Models.GroupMemory;
using CoolBeans.ViewModels.GrowMemory;
using Xamarin.Forms;

namespace CoolBeans.Pages.GrowMemory
{
    public class GrowMemoryDetailPage : CarouselPage
    {
        public GrowMemoryDetailPage()
        {
			//System.GC.Collect ();
            this.SetBinding(ItemsSourceProperty,"ImageList");

            //this.ItemsSource = new List<ImageUrl>
            //{
            //    new ImageUrl {Url = "http://img4.cache.netease.com/photo/0005/2014-07-21/A1N22H6P0ACR0005.jpg"},
            //    new ImageUrl {Url = "http://img6.cache.netease.com/photo/0005/2014-07-21/900x600_A1N22NJB0ACR0005.jpg"},
            //    new ImageUrl {Url = "http://ww1.sinaimg.cn/bmiddle/68611fd2gw1eik8go4knkj20c807xabd.jpg"}
            //};



            this.ItemTemplate = new DataTemplate(() =>
            {
                return new ImageDetailPage();
            });
            //_article = article;
            //List<ContentPage> pages = new List<ContentPage>();
            //if (!string.IsNullOrEmpty(_article.Image1))
            //{
            //    Children.Add(new ContentPage
            //    {
            //        Content = new Image { Source = ImageSource.FromUri(new Uri(_article.Image1)) }
            //    });
            //}
            //if (!string.IsNullOrEmpty(_article.Image2))
            //{
            //    Children.Add(new ContentPage
            //    {
            //        Content = new Image { Source = ImageSource.FromUri(new Uri(_article.Image2)) }
            //    });
            //}
            //if (!string.IsNullOrEmpty(_article.Image1))
            //{
            //    Children.Add(new ContentPage
            //    {
            //        Content = new Image { Source = ImageSource.FromUri(new Uri(_article.Image3)) }
            //    });
            //}
            //Children.Add(new ContentPage
            //{
            //    Content = new BoxView {Color = new Color(1, 0, 0)},
            //    Title = "Page 1"
            //});
            //Children.Add(new ContentPage
            //{
            //    Content = new BoxView {Color = new Color(0, 1, 0)},
            //    Title = "Page 2"
            //});
            //Children.Add(new ContentPage
            //{
            //    Content = new BoxView {Color = new Color(0, 0, 1)},
            //    Title = "Page 3"
            //});
        }

        public class ImageDetailPage:ContentPage
        {
            public ImageDetailPage()
            {
                Image image = new Image();
				image.Aspect = Aspect.Fill;
				image.VerticalOptions = LayoutOptions.CenterAndExpand;
				image.HorizontalOptions = LayoutOptions.CenterAndExpand;
                image.SetBinding(Image.SourceProperty, "Url");
                this.Content = new StackLayout
                {
                    Children = {image}
                };


            }


        }
    }
}