using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using CoolBeans.Controls;
using CoolBeans.Services;
using CoolBeans.ViewModels;
using Xamarin.Forms;

namespace CoolBeans.Pages
{
    public class LoginPage : MvvmableContentPage
    {
        public LoginPage()
        {
            BackgroundImage = "login_background.png";

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            var layout = new StackLayout { Padding = 25 };

            var label = new Label
            {
                Text = "欢迎来到贝贝巴士",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                XAlign = TextAlignment.Center, // Center the text in the blue box.
                YAlign = TextAlignment.Center, // Center the text in the blue box.
            };

            layout.Children.Add(label);

			var info = new Label
			{
				Text = "欢迎来到贝贝巴士",
				Font = Font.SystemFontOfSize(NamedSize.Small),
				TextColor = Color.Red,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
			};
			layout.Children.Add (info);

            var username = new Entry() { Placeholder = "用户名(注册时的电话号码)" };
            layout.Children.Add(username);

            var password = new Entry
            {
                Placeholder = "密码",
                IsPassword = true
            };
            layout.Children.Add(password);

            var buttonLogin = new MyButton
            {
                Text = "登陆",
                TextColor = Color.White,
            };
            var buttonRegister = new MyButton
            {
                BorderWidth = 20,
                BorderRadius = 5, 
                Text = "注册", 
                TextColor = Color.White
            };
            
            //var activityIndicator = new ActivityIndicator
            //{
            //    IsEnabled = true,
            //    IsRunning = false,
            //    IsVisible = true,
            //    HeightRequest = 40,
            //    WidthRequest = 40
            //};
            //layout.Children.Add(activityIndicator);
            layout.Children.Add(buttonLogin);
            layout.Children.Add(buttonRegister);

            Content = layout;

            //activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsRunning"));
            username.SetBinding(Entry.TextProperty, new Binding("Username"));
            password.SetBinding(Entry.TextProperty, new Binding("Password"));
            buttonLogin.SetBinding(Button.CommandProperty, new Binding("LoginCommand"));
            buttonRegister.SetBinding(Button.CommandProperty, new Binding("GotoRegisterCommand"));
            info.SetBinding(Label.TextProperty, new Binding("Information"));
            //ToolbarItems.Clear();
            //ToolbarItems = null;
        }
        protected override void OnAppearing()
        {
            var aboutItem = new ToolbarItem { Name = "About", BindingContext = BindingContext };
            aboutItem.SetBinding(ToolbarItem.CommandProperty, new Binding("ShowAboutPageCommand"));
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            this.ToolbarItems.Clear();
            base.OnDisappearing();
        }
        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();

        //    // Fixed in next version of Xamarin.Forms. BindingContext is not properly set on ToolbarItem.
        //    var aboutItem = new ToolbarItem { Name = "About", BindingContext = BindingContext };
        //    aboutItem.SetBinding(ToolbarItem.CommandProperty, new Binding("ShowAboutPageCommand"));

        //    ToolbarItems.Add(aboutItem);
        //    foreach (var toolbarItem in ToolbarItems)
        //    {
        //        string output = toolbarItem.ToString() + "\tname:" + toolbarItem.Name;
        //        Mvx.Trace(MvxTraceLevel.Diagnostic, "toolbaritem", output);
        //    }
        //}
    }
}
