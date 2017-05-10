using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBeans.Controls;
using Xamarin.Forms;

namespace CoolBeans.Pages.SchoolOnLine
{
    public partial class NoticeListPage : MvvmableContentPage
    {
        public NoticeListPage()
        {
            //InitializeComponent();

            var listView = new ListView();

            listView.ItemTemplate = new DataTemplate(typeof(NoticeItemCell));
            listView.SetBinding(ListView.ItemsSourceProperty,"Notices");
			listView.ItemSelected += (sender, e) => {
			};

            var layout = new StackLayout();
            layout.Children.Add(listView);
            Content = layout;
        }
    }
}
