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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Porrey.Controls.ColorPicker
{
	[TemplateVisualState(Name = "UncheckedNormal", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "UncheckedPointerOver", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "UncheckedPressed", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "UncheckedDisabled", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "CheckedNormal", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "CheckedPointerOver", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "CheckedPressed", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "CheckedDisabled", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "IndeterminateNormal", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "IndeterminatePointerOver", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "IndeterminatePressed", GroupName = "CombinedStates")]
	[TemplateVisualState(Name = "IndeterminateDisabled", GroupName = "CombinedStates")]
	[TemplatePart(Name = "PART_OuterBorder", Type = typeof(Border))]
	[TemplatePart(Name = "PART_Icon", Type = typeof(BitmapIcon))]
	[TemplatePart(Name = "PART_Glow", Type = typeof(Ellipse))]
	public class TogglePowerSwitch : CheckBox
	{
		public TogglePowerSwitch()
		{
			this.DefaultStyleKey = typeof(TogglePowerSwitch);
			this.SizeChanged += this.TogglePowerSwitch_SizeChanged;
		}

		private void TogglePowerSwitch_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this.OuterBorder != null)
			{
				this.OuterBorder.Width = e.NewSize.Width;
				this.OuterBorder.Height = e.NewSize.Height;
				this.OuterBorder.CornerRadius = new CornerRadius(e.NewSize.Width);

				this.SetGlowVerticalOffset();
			}
		}

		public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(int), typeof(TogglePowerSwitch), new PropertyMetadata(0, new PropertyChangedCallback(OnHuePropertyChanged)));
		public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register("Saturation", typeof(double), typeof(TogglePowerSwitch), new PropertyMetadata(1.0, new PropertyChangedCallback(OnSaturationPropertyChanged)));
		public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register("Brightness", typeof(double), typeof(TogglePowerSwitch), new PropertyMetadata(.5, new PropertyChangedCallback(OnBrightnessPropertyChanged)));
		public static readonly DependencyProperty LightColorProperty = DependencyProperty.Register("LightColor", typeof(Color), typeof(TogglePowerSwitch), new PropertyMetadata(Color.FromArgb(255, 255, 0, 0), new PropertyChangedCallback(OnLightColorPropertyChanged)));

		public int Hue
		{
			get
			{
				return (int)this.GetValue(HueProperty);
			}
			set
			{
				this.SetValue(HueProperty, value);
			}
		}

		public double Saturation
		{
			get
			{
				return (double)this.GetValue(SaturationProperty);
			}
			set
			{
				this.SetValue(SaturationProperty, value);
			}
		}

		public double Brightness
		{
			get
			{
				return (double)this.GetValue(BrightnessProperty);
			}
			set
			{
				this.SetValue(BrightnessProperty, value);
			}
		}

		public Color LightColor
		{
			get
			{
				return (Color)this.GetValue(LightColorProperty);
			}
			set
			{
				this.SetValue(LightColorProperty, value);
			}
		}

		private static void OnHuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TogglePowerSwitch instance)
			{
				if (instance.Hue >= 0 && instance.Hue <= 360)
				{
					instance.SetSelectedColor(instance.Hue, instance.Saturation, instance.Brightness);
				}
				else
				{
					throw new ArgumentOutOfRangeException("Hue", "Hue must be a value from 0 to 360.");
				}
			}
		}

		private static void OnSaturationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TogglePowerSwitch instance)
			{
				if (instance.Saturation >= 0 && instance.Saturation <= 1.0)
				{
					instance.SetSelectedColor(instance.Hue, instance.Saturation, instance.Brightness);
				}
				else
				{
					throw new ArgumentOutOfRangeException("Saturation", "Saturation must be a value from 0 to 1.0.");
				}
			}
		}

		private static void OnBrightnessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TogglePowerSwitch instance)
			{
				if (instance.Brightness >= 0 && instance.Brightness <= 1.0)
				{
					if (instance.Glow != null)
					{
						instance.Glow.Opacity = instance.Brightness;
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException("Brightness", "Brightness must be a value from 0 to 1.0.");
				}
			}
		}

		private static void OnLightColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TogglePowerSwitch instance)
			{
				var lightColor = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHsv(instance.LightColor);
				instance.SetSelectedColor((int)(360 * lightColor.H), lightColor.S, lightColor.V);
			}
		}

		protected override void OnApplyTemplate()
		{
			if (this.GetTemplateChild("PART_OuterBorder") is Border border)
			{
				this.OuterBorder = border;
			}

			if (this.GetTemplateChild("PART_Glow") is Ellipse glow)
			{
				this.Glow = glow;
			}

			this.SetSelectedColor(0, 1.0, 1.0);

			base.OnApplyTemplate();
		}

		protected Border OuterBorder { get; set; }
		protected Ellipse Glow { get; set; }

		protected void SetSelectedColor(int hue, double saturation, double brightness)
		{
			if (this.Glow?.Fill is LinearGradientBrush fill)
			{
				fill.GradientStops[4].Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.FromHsv(hue, saturation, brightness, 1.0);
				fill.GradientStops[3].Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.FromHsv(hue, saturation, brightness, 0.8);
				fill.GradientStops[2].Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.FromHsv(hue, saturation, brightness, 0.6);
				fill.GradientStops[1].Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.FromHsv(hue, saturation, brightness, 0.4);
			}
		}

		protected void SetGlowVerticalOffset()
		{
			if (this.Glow?.RenderTransform is CompositeTransform transform)
			{
				transform.TranslateY = .205 * (this.OuterBorder.ActualHeight - this.OuterBorder.BorderThickness.Top);
			}
		}
	}
}
