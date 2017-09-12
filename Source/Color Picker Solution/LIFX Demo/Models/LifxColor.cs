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
