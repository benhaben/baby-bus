using System;
using Xamarin.Forms;

namespace CoolBeans.Pages.GrowMemory
{
    public partial class GrowMemoryPage : ContentPage
    {
        public GrowMemoryPage()
        {
			//System.GC.Collect ();
            InitializeComponent();
            list.GroupDisplayBinding = new Binding("DisplayDate");
            list.IsGroupingEnabled = true;
            list.SetBinding(ListView.ItemsSourceProperty, "ArticlesCollection");
            list.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedArticle"));
        }

        public void Test()
        {
            var list = new ListView
            {
                ItemTemplate = new DataTemplate(typeof (TextCell))
                {
                    Bindings =
                    {
                        {TextCell.TextProperty, new Binding("Title")}
                    }
                },
                GroupDisplayBinding = new Binding("DisplayDate"),
                IsGroupingEnabled = true,
            };
            

            list.SetBinding(ListView.ItemsSourceProperty, "ArticlesCollection");
            list.SetBinding(ListView.SelectedItemProperty, "SelectedArticle");

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {list},
            };
        }
    }

    public class ImageListCell : ViewCell
    {

        public ImageListCell()
        {
			var list = new ListView {
				ItemTemplate = new DataTemplate (typeof(Image)) {
					Bindings = {
						{ Image.SourceProperty, new Binding ("Url") }
					}
				}
			};

			list.SetBinding (ListView.ItemsSourceProperty, "ImageList");
			var viewLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = { list }
			};
			View = viewLayout;
        }


    }
}