using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;

namespace CoolBeans.Pages.Main
{
	public partial class RePasswordPage : ContentPage
    {
        public RePasswordPage()
        {
            InitializeComponent();

			btnEnter.SetBinding(ImageButton.CommandProperty, new Binding("RePasswordCommand"));
        }
    }
}
