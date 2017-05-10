using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;

namespace CoolBeans.Pages.Main
{
    public partial class MainPrtPage : ContentPage
    {
        public MainPrtPage()
        {
            InitializeComponent();

            list.SetBinding(ListView.ItemsSourceProperty, new Binding("Notices"));

			var setting = new ToolbarItem{ Name = "设置"};
            setting.SetBinding(ToolbarItem.CommandProperty, "ShowSettingPageCommand");
			ToolbarItems.Add (setting);



        }
    }
}
