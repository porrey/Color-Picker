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
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Porrey.Controls.ColorPicker
{
	public static class GeometryExtensions
	{
		/// <summary>
		/// Gets the point of a FrameworkElement on which the element
		/// rotates.
		/// </summary>
		/// <param name="element">A FrameworkElement object.</param>
		/// <returns>A Point representing the center point on which the object rotates.</returns>
		public static Point Center(this FrameworkElement element)
		{
			return new Point(element.ActualWidth * element.RenderTransformOrigin.X, element.ActualHeight * element.RenderTransformOrigin.Y);
		}

		/// <summary>
		/// Returns a point translated around the specified origin point.
		/// </summary>
		/// <param name="point">The point to be translated.</param>
		/// <param name="origin">The origin point.</param>
		/// <returns>The translated point.</returns>
		public static Point TranslateFromOrigin(this Point point, Point origin)
		{
			return new Point(point.X - origin.X, origin.Y - point.Y);
		}

		/// <summary>
		/// Applies the Math.Atan2 method to a point.
		/// </summary>
		/// <param name="point">The point to which Math.Atan2 is applied.</param>
		/// <returns>The result of the Math.Atan2 method.</returns>
		public static double Atan2(this Point point)
		{
			return Math.Atan2(point.X, point.Y);
		}

		/// <summary>
		/// Converts and angle in Radians to Degrees.
		/// </summary>
		/// <param name="radians">The angle to be converted.</param>
		/// <returns>The angle converted to degrees.</returns>
		public static double ToDegrees(this double radians)
		{
			return radians * 180.0 / Math.PI;
		}

		/// <summary>
		/// Gets the angle of a line segment in Radians.
		/// </summary>
		/// <param name="from">The first point in the line segment.</param>
		/// <param name="to">The second point in the line segment.</param>
		/// <returns>The angle of the line segment in radians.</returns>
		public static double GetAngle(this Point from, Point to)
		{
			double returnValue = 0;

			// ***
			// *** Translate the "to" point using the "from" point as the origin.
			// ***
			Point p = to.TranslateFromOrigin(from);

			// ***
			// *** Use Atan2 to get angles between 0 and 180 (Quadrants I and IV) and
			// *** 0 and -180 (quadrants II and III).
			// ***
			returnValue = p.Atan2();

			return returnValue;
		}

		/// <summary>
		/// Converts an angle in Degrees to a Rotation value that can be
		/// applied to the RotateTransform.
		/// </summary>
		/// <param name="angleDegrees">An angle in degrees.</param>
		/// <returns>A rotation value between 0 and 360 where 0 represents
		/// an item that is not rotated.</returns>
		public static double ToRotation(this double angleDegrees)
		{
			return 360 - angleDegrees;
		}

		public static int GetHueFromRotation(this double rotationDegrees)
		{
			int returnValue = 0;
			double angle = -1 * (rotationDegrees % 360);

			if (angle < 0)
			{
				returnValue = (int)(360 + angle) + 1;
			}
			else
			{
				returnValue = (int)angle + 1;
			}

			return returnValue;
		}

		public static double GetRotationFromHue(this int hue)
		{
			return 360.0 - hue;
		}
	}
}
