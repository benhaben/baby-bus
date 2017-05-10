using CoolBeans.Services;
using CoolBeans.ViewModels;
using Xamarin.Forms;

namespace CoolBeans.Pages
{
    public class LoginPage : ContentPage
    {
        public LoginPage()
        {
            //BackgroundColor = Helpers.Color.Purple.ToFormsColor();

            var layout = new StackLayout { Padding = 10 };
            var header = new Label
            {
                Text = "欢迎来到贝贝巴士",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = 300
            };
            layout.Children.Add(header);
            /////////////////////////////////////////////////////////////////////////////////
            var nameLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var usernameLabel = new Label
            {
                Text = "用户名",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            var username = new Entry
            {
                Keyboard = Keyboard.Text,
                Placeholder = "请输入用户名",
                WidthRequest = 150,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            nameLayout.Children.Add(usernameLabel);
            nameLayout.Children.Add(username);
            layout.Children.Add(nameLayout);

            /////////////////////////////////////////////////////////////////////////////////
            var passwordLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var passwordLabel = new Label
            {
                Text = "用户名",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center
            };
            var password = new Entry
            {
                Keyboard = Keyboard.Text,
                Placeholder = "请输入密码",
                IsPassword = true
                //WidthRequest = 150
            };
            passwordLayout.Children.Add(passwordLabel);
            passwordLayout.Children.Add(password);
            layout.Children.Add(passwordLayout);
           
            /////////////////////////////////////////////////////////////////////////////////
            var buttonLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var buttonRegist = new Button { Text = "注册", TextColor = Color.White };
            var buttonLogin = new Button { Text = "登陆", TextColor = Color.White };
            buttonLayout.Children.Add(buttonRegist);
            buttonLayout.Children.Add(buttonLogin);
            layout.Children.Add(buttonLayout);

            var backgroundImage = new Image()
            {
                Source = FileImageSource.FromFile("login_background.png")
            };
     
            var absoluteLayout = new AbsoluteLayout
            {
                Children =
                {
                    {backgroundImage, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All},
                    {layout, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All}
                }
            };
            username.SetBinding(Entry.TextProperty, new Binding("Username"));
            password.SetBinding(Entry.TextProperty, new Binding("Password"));
            buttonLogin.SetBinding(Button.CommandProperty, new Binding("LoginCommand"));

            this.Content = absoluteLayout;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            // Fixed in next version of Xamarin.Forms. BindingContext is not properly set on ToolbarItem.
            var aboutItem = new ToolbarItem { Name = "About", BindingContext = BindingContext };
            aboutItem.SetBinding(ToolbarItem.CommandProperty, new Binding("ShowAboutPageCommand"));

            ToolbarItems.Add(aboutItem);
        }
    }
}
