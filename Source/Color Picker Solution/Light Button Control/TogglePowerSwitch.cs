using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Porrey.Controls.LightButton
{
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplatePart(Name = "PART_OuterBorder", Type = typeof(Border))]
	[TemplatePart(Name = "PART_LightColor", Type = typeof(Ellipse))]
	[TemplatePart(Name = "PART_Icon", Type = typeof(BitmapIcon))]
	[TemplatePart(Name = "PART_Glow", Type = typeof(Ellipse))]
	public class TogglePowerSwitch : CheckBox
	{
		public TogglePowerSwitch()
		{
			this.DefaultStyleKey = typeof(TogglePowerSwitch);
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
						instance.GlowTransform.ScaleX = 0;
						instance.GlowTransform.ScaleY = 0;
					}
					else
					{
						instance.GlowTransform.ScaleX = .5 + (instance.Brightness / 2.0);
						instance.GlowTransform.ScaleY = .5 + (instance.Brightness / 2.0);
					}
					//instance.RaiseBrightnessChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
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
			if (this.GetTemplateChild("PART_GlowTransform") is ScaleTransform transform)
			{
				this.GlowTransform = transform;
			}

			base.OnApplyTemplate();
		}
		#endregion

		protected ScaleTransform GlowTransform { get; set; }
	}
}
