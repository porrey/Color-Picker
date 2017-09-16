using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
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
	[TemplatePart(Name = "PART_LightColor", Type = typeof(Ellipse))]
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
				this.OuterBorder.CornerRadius =new CornerRadius(e.NewSize.Width);
			}
		}

		public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register("Brightness", typeof(double), typeof(TogglePowerSwitch), new PropertyMetadata(.5, new PropertyChangedCallback(OnBrightnessPropertyChanged)));

		private static void OnBrightnessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TogglePowerSwitch instance)
			{
				if (instance.Brightness >= 0 && instance.Brightness <= 1.0)
				{
					if (instance.Brightness == 0)
					{
						instance.Glow.Opacity = 0;
					}
					else
					{
						instance.Glow.Opacity = .5 + (instance.Brightness / 2.0);
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException("Brightness", "Brightness must be a value from 0 to 1.0.");
				}
			}
		}

		public double Brightness
		{
			get
			{
				return (double)GetValue(BrightnessProperty);
			}
			set
			{
				SetValue(BrightnessProperty, value);
			}
		}

		#region Control Event Handlers
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

			base.OnApplyTemplate();
		}
		#endregion

		protected Border OuterBorder { get; set; }
		protected Ellipse Glow { get; set; }
	}
}
