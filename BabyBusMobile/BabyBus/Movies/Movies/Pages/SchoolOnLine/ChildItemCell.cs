using System;
using Xamarin.Forms;

namespace CoolBeans
{
	public class ChildItemCell :ViewCell
	{
		public ChildItemCell ()
		{
			var label = new Label {
				YAlign = TextAlignment.Center
			};
			var image = new Image {
				Source = FileImageSource.FromFile ("empty_contact.jpg"), 
				VerticalOptions = LayoutOptions.Start,
				WidthRequest=40,
				HeightRequest=40
			};
			label.SetBinding (Label.TextProperty, "Name");

			var tick = new Image {
				Source = FileImageSource.FromFile ("check.png"),
			};
			tick.SetBinding (Image.IsVisibleProperty, "IsSelect");

			var layout = new StackLayout {
				Padding = new Thickness(20, 0, 0, 0),
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				Children = {image,label, tick}
			};
			View = layout;
		}

		protected override void OnBindingContextChanged ()
		{
			// Fixme : this is happening because the View.Parent is getting 
			// set after the Cell gets the binding context set on it. Then it is inheriting
			// the parents binding context.
			View.BindingContext = BindingContext;
			base.OnBindingContextChanged ();
		}
	}
}

