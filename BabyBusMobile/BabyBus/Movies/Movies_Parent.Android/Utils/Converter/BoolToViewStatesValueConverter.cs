using System;
using System.Globalization;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;

namespace BabyBus.Droid
{
	public class BoolToViewStatesValueConverter : MvxValueConverter<bool, ViewStates>
	{
		protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture) {
			return value ? ViewStates.Visible : ViewStates.Invisible;
		}
	}
}