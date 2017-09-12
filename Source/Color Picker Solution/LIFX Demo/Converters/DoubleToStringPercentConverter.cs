using System;
using Windows.UI.Xaml.Data;

namespace LifxDemo.Converters
{
	public class DoubleToStringPercentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string returnValue = "0%";

			if (value is double doubleValue)
			{
				if (doubleValue < 0) doubleValue = 0;
				if (doubleValue > 1.0) doubleValue = 1.0;

				returnValue = doubleValue.ToString("0%");
			}

			return returnValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
