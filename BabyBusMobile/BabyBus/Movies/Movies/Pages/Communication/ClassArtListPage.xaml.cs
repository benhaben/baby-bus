using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoolBeans.Pages.Communication
{
    public partial class ClassArtListPage
    {
        public ClassArtListPage()
        {
            InitializeComponent();
            InitializeComponent();
            this.SetValue(TitleProperty, "班级通知");

            listView.GroupDisplayBinding  = new Binding("DisplayDate");
            listView.IsGroupingEnabled = true;
            listView.SetBinding(ListView.ItemsSourceProperty, "ArticlesCollection");
            listView.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedArticle"));
        }
    }
}
