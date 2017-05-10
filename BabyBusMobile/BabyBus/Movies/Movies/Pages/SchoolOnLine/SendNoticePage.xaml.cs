using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CoolBeans.Controls;
using CoolBeans.ViewModels;
using CoolBeans.ViewModels.SchoolOnLine;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;

namespace CoolBeans.Pages.SchoolOnLine
{
    public partial class SendNoticePage : NeedSelectImageContentPage
    {
        public SendNoticePage()
        {
            InitializeComponent();
            Title = "发通知";
            var imageButton = new ImageSelectButton(this, 150, 150);
            imageButton.HorizontalOptions = LayoutOptions.Start;
            imageButton.Text = " + ";
            mediaLayout.Children.Add(imageButton);

            
        }

/*
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewmodel = (SendNoticeViewModel) ViewModel;
            //TODO Picker can't set binding.  
            if (this.noticeType.Items.Count == 0)
            {
                foreach (string type in (viewmodel).NoticeTypeItems)
                {
                    this.noticeType.Items.Add(type);
                }
            }
            //TODO Ugly
            viewmodel.PropertyChanged +=viewmodel_PropertyChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            //TODO Ugly, Fuck!!!
            var viewmodel = (SendNoticeViewModel)ViewModel;
            viewmodel.PropertyChanged -= viewmodel_PropertyChanged;
        }

        private void viewmodel_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var viewmodel = (SendNoticeViewModel) ViewModel;
            if (args.PropertyName == "TheImageSource")
            {   
                var image1 = new Image {Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.Start};
                image1.Source = viewmodel.TheImageSource.Small;
                image1.GestureRecognizers.Add(new TapGestureRecognizer(OnTap));
                imageLayout.Children.Add(image1);
            }
        }

        private void OnTap(View arg1, object arg2)
        {
            DisplayAlert("", "Hahaha", "Cancel");
        }
 * */
    }
}