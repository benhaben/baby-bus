using CoolBeans.Controls;
using CoolBeans.Pages.Login;
using CoolBeans.Services;
using CoolBeans.ViewModels;
using Xamarin.Forms;

namespace CoolBeans.Pages
{
    public class RegisterPage : MvvmableContentPage
    {
        public RegisterPage()
        {
            BackgroundImage = "login_background.png";

            var phoneLayout = LoginHelper.
                CreateLabelEntryInStackLayout("电话号码：", "Phone");

            var passwordLayout = LoginHelper.
               CreateLabelEntryInStackLayout("密码：", "Password", true);

            var comfirmPasswordLayout = LoginHelper.
              CreateLabelEntryInStackLayout("确认密码：", "ComfirmedPassword", true);


            var tableView = new MyTableView("注册");
            const string sectionTitle = "请填写以下信息";

            tableView.CreateTableSection(sectionTitle);
            tableView.AddStackLayoutInTableSection(sectionTitle, phoneLayout);
            tableView.AddStackLayoutInTableSection(sectionTitle, passwordLayout);
            tableView.AddStackLayoutInTableSection(sectionTitle, comfirmPasswordLayout);

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
            var buttonRegister = new Button
            {
                BorderWidth = 20,
                BorderRadius = 5,
                Text = "注册",
                TextColor = Color.White,
                IsEnabled = true
            };
            var info = new Label
              {
                  Text = "欢迎来到贝贝巴士",
                  Font = Font.SystemFontOfSize(NamedSize.Medium),
                  TextColor = Color.Red,
                  VerticalOptions = LayoutOptions.StartAndExpand,
                  XAlign = TextAlignment.Center,
                  YAlign = TextAlignment.Center,
              };

            // Build the page.
            this.Content = new StackLayout
            {
                Children = 
                {
                    //header,
                    tableView,
                    info,
                    buttonRegister
                }
            };

            info.SetBinding(Label.TextProperty, new Binding("Information"));
            buttonRegister.SetBinding(Button.CommandProperty, new Binding("DetialRegisterCommand"));
        }

        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();

        //    // Fixed in next version of Xamarin.Forms. BindingContext is not properly set on ToolbarItem.
        //    var aboutItem = new ToolbarItem { Name = "About", BindingContext = BindingContext };
        //    aboutItem.SetBinding(ToolbarItem.CommandProperty, new Binding("ShowAboutPageCommand"));

        //    ToolbarItems.Add(aboutItem);
        //}
    }
}
