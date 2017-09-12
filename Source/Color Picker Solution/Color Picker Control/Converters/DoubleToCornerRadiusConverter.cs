using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Porrey.Controls.ColorPicker.Converters
{
	public class DoubleToCornerRadiusConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			CornerRadius returnValue = new CornerRadius(0);

			if (value is double doubleValue)
			{
				if (!double.IsNaN(doubleValue))
				{
					returnValue = new CornerRadius(doubleValue);
				}
			}

			return returnValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
