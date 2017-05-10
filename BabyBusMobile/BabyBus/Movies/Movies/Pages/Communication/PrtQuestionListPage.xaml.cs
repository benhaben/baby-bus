using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;

namespace CoolBeans.Pages.Communication
{
    public partial class PrtQuestionListPage : ContentPage
    {
        public PrtQuestionListPage()
        {
            InitializeComponent();
			listView.SetBinding(ListView.ItemsSourceProperty, "Questions");
            listView.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedQuestion"));
        }
    }
}
