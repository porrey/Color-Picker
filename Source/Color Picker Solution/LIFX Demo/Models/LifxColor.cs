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
using Microsoft.Toolkit.Uwp;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace LifxDemo.Models
{
	public class Lifx
	{
		public static class Rgb
		{
			public static Color FromLifx(ushort hue, ushort saturation, ushort brightness)
			{
				Color returnValue = Colors.White;

				double h = Lifx.Hue.FromLifx(hue);
				double s = Lifx.Saturation.FromLifx(saturation);
				double v = Lifx.Brightness.FromLifx(brightness);
				returnValue = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.FromHsv(h, s, v);

				return returnValue;
			}

			public static SolidColorBrush CreateSolidColorBrush(ushort hue, ushort saturation, ushort brightness)
			{
				Color color = Lifx.Rgb.FromLifx(hue, saturation, brightness);
				return new SolidColorBrush(color);
			}
		}

		public static class Hue
		{
			public static ushort FromLifx(ushort hue)
			{
				return (ushort)((hue / 65535.0) * 360);
			}

			public static ushort ToLifx(ushort hue)
			{
				return (ushort)(hue / 360.0 * 65535.0);
			}

			public static ushort FromColor(Color color)
			{
				HsvColor hsv = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHsv(color);
				return Lifx.Hue.ToLifx((ushort)hsv.H);
			}
		}

		public static class Brightness
		{
			public static double FromLifx(ushort brightness)
			{
				return (double)(brightness / 65535.0);
			}

			public static ushort ToLifx(double brightness)
			{
				return (ushort)(brightness * 65535.0);
			}

			public static ushort FromColor(Color color)
			{
				HsvColor hsv = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHsv(color);
				return Lifx.Brightness.ToLifx((ushort)hsv.V);
			}
		}

		public static class Saturation
		{
			public static double FromLifx(ushort saturation)
			{
				return (double)(saturation / 65535.0);
			}

			public static ushort ToLifx(double saturation)
			{
				return (ushort)(saturation * 65535.0);
			}

			public static ushort FromColor(Color color)
			{
				HsvColor hsv = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHsv(color);
				return Lifx.Saturation.ToLifx((ushort)hsv.S);
			}
		}
	}
}
