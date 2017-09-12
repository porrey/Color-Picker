using System;
using Windows.UI.Xaml.Data;

namespace LifxDemo.Converters
{
	public sealed class NullToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			bool returnValue = false;

			if (value != null)
			{
				returnValue = true;
			}

			return returnValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotSupportedException();
		}
	}
}
