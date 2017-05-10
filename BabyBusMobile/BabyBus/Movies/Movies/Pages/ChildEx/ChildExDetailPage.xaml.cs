using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoolBeans.Pages.ChildEx
{
    public partial class ChildExDetailPage : ContentPage
    {
        public ChildExDetailPage()
        {
            //InitializeComponent();

            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            var innerStack = new StackLayout
            {
                Spacing = 10,
                Orientation = StackOrientation.Vertical
            };

            //var type = new Label
            //{
            //    Font = Font.SystemFontOfSize(NamedSize.Small),
            //    HorizontalOptions = LayoutOptions.Center,
            //    Text = "幼教经验Test",
            //    TextColor = Color.Blue
            //};

            var title = new Label
            {
                Font = Font.SystemFontOfSize(NamedSize.Small),
                HorizontalOptions = LayoutOptions.Center,
            };

            var date = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand 
            };

            var content = new Label
            {
                HorizontalOptions = LayoutOptions.Center
            };

            //innerStack.Children.Add(type);
            innerStack.Children.Add(title);
            innerStack.Children.Add(date);
            innerStack.Children.Add(content);

            title.SetBinding(Label.TextProperty,new Binding("ChildEx.Title"));
            date.SetBinding(Label.TextProperty, new Binding("InDate"));
            content.SetBinding(Label.TextProperty, new Binding("Content"));

            var scrollView = new ScrollView { Content = innerStack };

            Content = scrollView;
        }
    }
}
