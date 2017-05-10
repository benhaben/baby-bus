using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CoolBeans.Pages.GrowMemory
{	
	public partial class CarouselPageDemoPage : CarouselPage
	{	
		public CarouselPageDemoPage ()
		{

			this.ItemsSource = new TempBind[] 
			{
				new TempBind{Title="111",Content="111"},
				new TempBind{Title="222",Content="222"},
				new TempBind{Title="333",Content="333"}
			};

			this.ItemTemplate = new DataTemplate(() => 
				{ 
					return new TempBindPage();
				});
		}
	}

	public class TempBind
	{
		public string Title{ get; set; }
		public string Content{ get; set; }
	}

	public class TempBindPage : ContentPage
	{
		public TempBindPage()
		{
			this.SetBinding (ContentPage.TitleProperty, "Title");
			Label label = new Label ();
			label.SetBinding (Label.TextProperty, "Content");
			this.Content = new StackLayout {
				Children = { label }
			};
		}
	}
}

