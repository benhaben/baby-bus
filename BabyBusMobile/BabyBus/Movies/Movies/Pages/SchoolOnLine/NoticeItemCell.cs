using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace CoolBeans.Pages.SchoolOnLine
{
    public class NoticeItemCell : ViewCell
    {
        Label title, label;
		StackLayout layout;
        public NoticeItemCell()
		{

			title = new Label {
				YAlign = TextAlignment.Center
			};
            title.SetBinding(Label.TextProperty, "NoticeType");

			label = new Label {
				YAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize(10)
			};
            label.SetBinding(Label.TextProperty, "Content");

			var fav = new Image {
                Source = FileImageSource.FromFile("Icon.png"),
			};
			var btnabstr = new Button {
				Text = "+",
				HorizontalOptions=LayoutOptions.Start,
				BackgroundColor = Color.Green,
			};
			btnabstr.SetBinding (Button.CommandProperty, "VisiableAbstractCommand");
            //var abstr = new Editor();
            //abstr.SetBinding(Editor.TextProperty, "Content");
            //abstr.SetBinding(Editor.IsVisibleProperty, "IsShowAbstract");

            var test = new Label
            {
                YAlign = TextAlignment.Center
            };
            test.SetBinding(Label.TextProperty, "Content");

			var text = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness(0, 0, 0, 0),
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Children = {title, label}
			};

			layout = new StackLayout {
				Padding = new Thickness(20, 0, 0, 0),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = { fav,text,btnabstr}
			};
            var ablayout = new StackLayout
            {
                Padding = new Thickness(20, 0, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = { test,layout}
            };
            View = ablayout;
		}

        protected override void OnBindingContextChanged()
        {
            // Fixme : this is happening because the View.Parent is getting 
            // set after the Cell gets the binding context set on it. Then it is inheriting
            // the parents binding context.
            View.BindingContext = BindingContext;
            base.OnBindingContextChanged();
        }
    }
}
