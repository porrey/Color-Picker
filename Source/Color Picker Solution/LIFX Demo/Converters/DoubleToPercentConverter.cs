// Copyright © 2018-2020 Daniel Porrey
//
// This file is part of the Color Picker Control solution.
// 
// Sensor Telemetry is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Sensor Telemetry is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with the Color Picker Control solution. If not, 
// see http://www.gnu.org/licenses/.
//
using System;
using Windows.UI.Xaml.Data;

namespace LifxDemo.Converters
{
	public class DoubleToPercentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
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

		public object ConvertBack(object value, Type targetType, object parameter, string language)
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
	}
}
