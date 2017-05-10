using CoolBeans.ViewModels.SchoolOnLine;
using Xamarin.Forms;

namespace CoolBeans.Pages.SchoolOnLine
{
    public partial class ChildrenPage : MvvmableContentPage
    {
        private readonly ListView listView;
        private readonly ToolbarItem finishBar;

        public ChildrenPage()
        {
            finishBar = new ToolbarItem
            {
                Name = "完成",
            };

            finishBar.SetBinding(ToolbarItem.CommandProperty, new Binding("ReturnCommand"));
            ToolbarItems.Add(finishBar);

            //InitializeComponent ();
            var btnAll = new Button
            {
                BackgroundColor = Color.FromHex("#3d509f"),
                TextColor = Color.FromHex("#ffffff"),
                Text = "全选",
                HeightRequest = 50,
                WidthRequest = 100,
            };
            btnAll.SetBinding(Button.CommandProperty, "SelectAllCommand");
            var btnNone = new Button
            {
                BackgroundColor = Color.Red,
                TextColor = Color.FromHex("#ffffff"),
                Text = "反选",
                HeightRequest = 50,
                WidthRequest = 100,
            };

            var label = new Label();
            
            label.SetBinding(Label.TextProperty, new Binding("SelectedText"));

            btnNone.SetBinding(Button.CommandProperty, "SelectNoneCommand");
            var btnLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };
            btnLayout.Children.Add(btnAll);
            btnLayout.Children.Add(btnNone);


            listView = new ListView
            {
                RowHeight = 40
            };
            listView.ItemTemplate = new DataTemplate(typeof(ChildItemCell));
            listView.SetBinding(ListView.ItemsSourceProperty, "Children");
            listView.ItemSelected += ChildItemSelectd;

            var layout = new StackLayout();
            layout.Children.Add(btnLayout);
            layout.Children.Add(listView);
            layout.VerticalOptions = LayoutOptions.FillAndExpand;
            Content = layout;

            //MessagingCenter.Subscribe<SendNoticeViewModel, ObservableCollection<ChildViewModel>>(this,
            //    Constants.Message_SelectedChildren,
            //    (sender, args) =>
            //    {
            //        if (args != null)
            //        {
            //            children = args;
            //            listView.ItemsSource = children;
            //        }
            //    }, null);
        }

        private void ChildItemSelectd(object sender, SelectedItemChangedEventArgs e)
        {
            var child = (ChildViewModel)e.SelectedItem;
            //TODO 为了通知UI,ChildViewModel继承了ViewModelBase，应该有更好的方法。
            child.IsSelect = !child.IsSelect;
            //((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }


}

