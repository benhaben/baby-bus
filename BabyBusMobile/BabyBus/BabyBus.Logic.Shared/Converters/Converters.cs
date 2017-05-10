using System;
using System.Globalization;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;


namespace BabyBus.Logic.Shared
{
	public class DateTimeOffsetValueConverter : MvxValueConverter<DateTime,string>
	{

		protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
		{
			return LogicUtils.DateTimeString(value);
		}


	}

	public class IsHaveAnswersValueConverter : MvxValueConverter<bool,string>
	{
		protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value) {
				return "已经回答";
			} else {
				return "未回答";
			}
		}
	}

	public class BirthDayLongDateValueConverter:MvxValueConverter<DateTime,string>
	{
		protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture) {
			return value.ToString("D");
		}
	}

	public class IsAttenceValueConverter : MvxValueConverter<bool,string>
	{
		protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture) {
			if (value) {
				return "本日已经考勤";
			} else {
				return "本日还未进行考勤，请点击上方的按钮开始考勤";
			}
		}
	}

	public class IsAttenceMasterValueConverter : MvxValueConverter<bool,string>
	{
		protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture) {
			if (value) {
				return "已考勤";
			} else {
				return "未考勤";
			}
		}
	}


	public class StringToUriThumbValueConverter : MvxValueConverter<string,Uri>
	{
		protected override Uri Convert(string value, Type targetType, object parameter, CultureInfo culture) {
			if (!string.IsNullOrEmpty(value)) {
				return new Uri(Constants.ThumbServerPath + value + Constants.ThumbRule);
			} else {
				return null;
			}
		}
	}

	public class GenderModel2IdValueConverter : MvxValueConverter<GenderModel,int>
	{
		protected override int Convert(GenderModel value, Type targetType, object parameter, CultureInfo culture) {
			return value.Id;
		}

		protected override GenderModel ConvertBack(int value, Type targetType, object parameter, CultureInfo culture) {
			var genderModel = new GenderModel();
			genderModel.Id = value;
			return genderModel;
		}
	}
}
