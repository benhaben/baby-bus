using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using CoolBeans.ViewModels;
using Xamarin.Forms;

namespace CoolBeans.Pages
{
    public class MvvmableContentPage : ContentPage
    {
        protected  ViewModelBase ViewModel;
        

        public MvvmableContentPage()
        {
            Title = "MvvmableContentPage show";
        }

        protected override void OnAppearing()
        {
            ViewModel = BindingContext as ViewModelBase;
            if (ViewModel != null)
            {
                ViewModel.OnAppearing(this);
            }
            base.OnAppearing();
        }
    }
}
