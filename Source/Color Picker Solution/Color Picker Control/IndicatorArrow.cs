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
using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Porrey.Controls.ColorPicker
{
	public class IndicatorArrow : Control
	{
		public static readonly DependencyProperty OutlineBrushProperty = DependencyProperty.Register("OutlineBrush", typeof(SolidColorBrush), typeof(IndicatorArrow), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)), new PropertyChangedCallback(OnOutlineBrushPropertyChanged)));
		public static readonly DependencyProperty OutlineThicknessProperty = DependencyProperty.Register("OutlineThickness", typeof(double), typeof(IndicatorArrow), new PropertyMetadata(3, new PropertyChangedCallback(OnOutlineThicknessPropertyChanged)));

		public event EventHandler<ValueChangedEventArgs<SolidColorBrush>> OutlineBrushChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> OutlineThicknessChanged = null;

		public IndicatorArrow()
		{
			this.DefaultStyleKey = typeof(IndicatorArrow);
			this.SizeChanged += this.IndicatorArrow_SizeChanged;
		}

		public SolidColorBrush OutlineBrush
		{
			get
			{
				return (SolidColorBrush)this.GetValue(OutlineBrushProperty);
			}
			set
			{
				this.SetValue(OutlineBrushProperty, value);
			}
		}

		public double OutlineThickness
		{
			get
			{
				return (double)this.GetValue(OutlineThicknessProperty);
			}
			set
			{
				this.SetValue(OutlineThicknessProperty, value);
			}
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			if (this.GetTemplateChild("PART_Triangle") is Path triangle)
			{
				this.Triangle = triangle;
			}
		}

		private void IndicatorArrow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.Triangle.Stretch = Stretch.Uniform;
			//this.Triangle.Points.Clear();
			//this.Triangle.Points.Add(new Point(0, 0));
			//this.Triangle.Points.Add(new Point(this.ActualWidth, 0));
			//this.Triangle.Points.Add(new Point(this.ActualWidth / 2.0, this.ActualHeight));
		}
	}
}
