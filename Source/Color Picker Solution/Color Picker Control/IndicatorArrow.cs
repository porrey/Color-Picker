// Copyright © 2018 Daniel Porrey
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
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Porrey.Controls.ColorPicker
{
	public class IndicatorArrow : Control
	{
		public IndicatorArrow()
		{
			this.DefaultStyleKey = typeof(IndicatorArrow);
			this.SizeChanged += this.Selector_SizeChanged;
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (this.GetTemplateChild("PART_Triangle") is Path triangle)
			{
				this.Triangle = triangle;
			}
		}

		private void Selector_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.Triangle.Stretch = Stretch.Uniform;
			//this.Triangle.Points.Clear();
			//this.Triangle.Points.Add(new Point(0, 0));
			//this.Triangle.Points.Add(new Point(this.ActualWidth, 0));
			//this.Triangle.Points.Add(new Point(this.ActualWidth / 2.0, this.ActualHeight));
		}

		protected Path Triangle { get; set; }
	}
}
