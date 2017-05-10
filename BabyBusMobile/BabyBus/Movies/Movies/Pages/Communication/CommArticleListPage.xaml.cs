using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoolBeans.Pages.Communication
{
    public partial class CommArticleListPage : ContentPage
    {
        public CommArticleListPage()
        {
            InitializeComponent();
            list.GroupDisplayBinding = new Binding("DisplayDate");
            list.IsGroupingEnabled = true;
            list.SetBinding(ListView.ItemsSourceProperty, "ArticlesCollection");
            list.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedArticle"));
        }
    }
}
