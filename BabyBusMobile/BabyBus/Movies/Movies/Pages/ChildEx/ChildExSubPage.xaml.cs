using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoolBeans.Pages.ChildEx
{
    public partial class ChildExSubPage : ContentPage
    {
        public ChildExSubPage()
        {
            InitializeComponent();
			this.SetBinding (TitleProperty, "Title");
			list.SetBinding(ListView.ItemsSourceProperty, "ChildExs");
            list.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedChildEx"));
        }
    }
}
