using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoolBeans.Controls;
using CoolBeans.Pages.Login;
using Xamarin.Forms;

namespace CoolBeans.Pages
{
    public class RegisterDetailPage : NeedSelectImageContentPage
    {
        public Image ChildImage = null;
        public RegisterDetailPage()
        {
            BackgroundImage = "login_background.png";
            //Title = "Name";
            //Icon = (FileImageSource)FileImageSource.FromFile("Icon.png");

            var tableView = new MyTableView("注册详细信息");
            //-------------------------------------------------------------------------

            //TODO:get from server or db later

            var mibaokaLayout = LoginHelper.CreateLabelEntryInStackLayout("密保卡：", "Mibaoka");
            var areaLayout = LoginHelper.CreateLabelPickerInStackLayout("城市：", LoginHelper.GetSupportCitys(), "City",
                "城市");
            var gardenLayout = LoginHelper.CreateLabelPickerInStackLayout("幼儿园：", LoginHelper.GetSupportGardens(),
                "Garden", "幼儿园");
            var classLayout = LoginHelper.CreateLabelPickerInStackLayout("班级：", LoginHelper.GetSupportClassNames(),
                "ClassName", "班级");

            var genderLayout = LoginHelper.CreateLabelPickerInStackLayout("性别：", LoginHelper.GetSupportGender(),
              "Gender", "性别");

            const string sectionTitle = "请填写以下信息";
            tableView.CreateTableSection(sectionTitle);
            tableView.AddStackLayoutInTableSection(sectionTitle, mibaokaLayout);
            tableView.AddStackLayoutInTableSection(sectionTitle, areaLayout);
            tableView.AddStackLayoutInTableSection(sectionTitle, gardenLayout);
            tableView.AddStackLayoutInTableSection(sectionTitle, classLayout);

            tableView.AddStackLayoutInTableSection(sectionTitle, LoginHelper.CreateLabelEntryInStackLayout("姓名：", "Name"));
            tableView.AddStackLayoutInTableSection(sectionTitle, genderLayout);
            tableView.AddStackLayoutInTableSection(sectionTitle, LoginHelper.CreateLabelDatePickerInStackLayout("生日：", "Birthday"));

            ChildImage = new Image
            {
                WidthRequest = 300,
                HeightRequest = 200,
            };
            ChildImage.SetBinding(Image.SourceProperty, "TheImageSource");

            var buttonRegister = new Button
            {
                BorderWidth = 20,
                BorderRadius = 5,
                Text = "注册",
                TextColor = Color.White
            };
            buttonRegister.SetBinding(Button.CommandProperty, new Binding("RegisterCommand"));

            var scrollview = new ScrollView
            {
                HeightRequest = 1024,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = new StackLayout
                {
                    Children =
                    {
                        tableView,
                        LoginHelper.CreatePhotoModuleInStackLayout(this),
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HeightRequest = 50,
                            WidthRequest = 50,
                            Padding = 3,
                            Children =
                            {
                                ChildImage
                            }
                        },
                        buttonRegister
                    }
                },
            };

            this.Content = new StackLayout
            {
                Children = 
                {
                    scrollview
                }
            };

            // Accomodate iPhone status bar.
            Padding = new Thickness(10,
                Device.OnPlatform(20, 0, 0),
                10,
                5);
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