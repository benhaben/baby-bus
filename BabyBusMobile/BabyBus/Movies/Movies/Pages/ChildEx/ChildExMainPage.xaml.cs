using Xamarin.Forms;

namespace CoolBeans.Pages.ChildEx
{
    public partial class ChildExMainPage : CarouselPage
    {
        public ChildExMainPage()
        {
			//InitializeComponent();
			this.SetBinding (ItemsSourceProperty, "ChildExsCollection");
            
            

			this.ItemTemplate = new DataTemplate(() =>
				{
					return new ChildExSubPage();
				});
            
        }
    }
}