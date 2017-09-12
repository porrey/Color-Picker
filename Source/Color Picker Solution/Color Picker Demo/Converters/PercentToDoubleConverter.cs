using System;
using Windows.UI.Xaml.Data;

namespace ColorPickerDemo.Converters
{
	public class PercentToDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			double returnValue = 0;

			if (value is double doubleValue)
			{
				if (doubleValue < 0) doubleValue = 0;
				if (doubleValue > 100) doubleValue = 100;

				returnValue = doubleValue / 100.0;
			}

			return returnValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			double returnValue = 0;

			if (value is double doubleValue)
			{
				if (doubleValue < 0) doubleValue = 0;
				if (doubleValue > 1.0) doubleValue = 1.0;

				returnValue = 100.0 * doubleValue;
			}

			return returnValue;
		}
	}
}
