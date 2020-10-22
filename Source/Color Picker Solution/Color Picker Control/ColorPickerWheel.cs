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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Porrey.Controls.ColorPicker
{
	//[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	//[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	//[TemplateVisualState(Name = "PointerOver", GroupName = "CommonStates")]
	//[TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
	//[TemplatePart(Name = "PART_Rotary", Type = typeof(Border))]
	//[TemplatePart(Name = "PART_Center", Type = typeof(Border))]
	//[TemplatePart(Name = "PART_Indicator", Type = typeof(ContentPresenter))]
	//[TemplatePart(Name = "PART_Content", Type = typeof(ContentPresenter))]
	//[ContentProperty(Name = "Content")]
	public class ColorPickerWheel : ContentControl
	{
		private bool _pointerEntered = false;

		public ColorPickerWheel()
		{
			this.DefaultStyleKey = typeof(ColorPickerWheel);
			this.IsEnabledChanged += this.HueColorPicker_IsEnabledChanged;
			this.SizeChanged += this.ColorPickerWheel_SizeChanged;
		}

		#region Sizing
		private void ColorPickerWheel_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.ActualOuterDiameter = this.GetOuterDiameter(e.NewSize);
			this.ActualInnerDiameter = this.GetInnerDiameter(this.ActualOuterDiameter);
			this.ActualOuterRadius = (this.ActualOuterDiameter - this.ActualInnerDiameter) / 2.0;

			this.ActualTopBottomGap = (e.NewSize.Height - this.ActualOuterDiameter) / 2.0;
			this.ActualLeftRightGap = (e.NewSize.Width - this.ActualOuterDiameter) / 2.0;

			if (this.GetTemplateChild("PART_Indicator") is ContentPresenter cp)
			{
				if (cp.RenderTransform is CompositeTransform transform)
				{
					transform.TranslateY = this.ActualTopBottomGap + this.IndicatorOffset;
					transform.ScaleX = this.IndicatorScale;
					transform.ScaleY = this.IndicatorScale;
				}
			}
		}

		protected double GetOuterDiameter(Size containerSize)
		{
			double returnValue = 0;

			if (containerSize.Width < containerSize.Height)
			{
				returnValue = containerSize.Width;
			}
			else
			{
				returnValue = containerSize.Height;
			}

			return returnValue;
		}

		protected double GetInnerDiameter(double outerDiameter)
		{
			double innerDiameter = (this.InnerDiameter * outerDiameter) - this.BorderThickness.Left;
			return innerDiameter > 0 ? innerDiameter : 0;
		}

		protected Size GetOuterDiameterSize(Size availableSize)
		{
			Size returnVaule = new Size();

			double outerDiameter = this.GetOuterDiameter(availableSize);
			returnVaule.Width = outerDiameter;
			returnVaule.Height = outerDiameter;

			return returnVaule;
		}
		#endregion

		#region Dependency Properties
		public static readonly DependencyProperty ActualOuterDiameterProperty = DependencyProperty.Register("ActualOuterDiameter", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(0, new PropertyChangedCallback(OnActualOuterDiameterPropertyChanged)));
		public static readonly DependencyProperty InnerDiameterProperty = DependencyProperty.Register("InnerDiameter", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(.55, new PropertyChangedCallback(OnInnerDiameterPropertyChanged)));
		public static readonly DependencyProperty ActualInnerDiameterProperty = DependencyProperty.Register("ActualInnerDiameter", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(0, new PropertyChangedCallback(OnActualInnerDiameterPropertyChanged)));
		public static readonly DependencyProperty ActualOuterRadiusProperty = DependencyProperty.Register("ActualOuterRadius", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(0, new PropertyChangedCallback(OnActualOuterRadiusPropertyChanged)));
		public static readonly DependencyProperty ActualTopBottomGapProperty = DependencyProperty.Register("ActualTopBottomGap", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(0, new PropertyChangedCallback(OnActualOuterRadiusPropertyChanged)));
		public static readonly DependencyProperty ActualLeftRightGapProperty = DependencyProperty.Register("ActualLeftRightGap", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(0, new PropertyChangedCallback(OnActualOuterRadiusPropertyChanged)));
		public static readonly DependencyProperty IndicatorOffsetProperty = DependencyProperty.Register("IndicatorOffset", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(0, new PropertyChangedCallback(OnIndicatorOffsetPropertyChanged)));
		public static readonly DependencyProperty IndicatorProperty = DependencyProperty.Register("Indicator", typeof(object), typeof(ColorPickerWheel), new PropertyMetadata(null, new PropertyChangedCallback(OnIndicatorPropertyChanged)));
		public static readonly DependencyProperty IndicatorScaleProperty = DependencyProperty.Register("IndicatorScale", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(1.0, new PropertyChangedCallback(OnIndicatorScalePropertyChanged)));
		public static readonly DependencyProperty RotationProperty = DependencyProperty.Register("Rotation", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(0.0, new PropertyChangedCallback(OnRotationPropertyChanged)));
		public static readonly DependencyProperty HueProperty = DependencyProperty.Register("Hue", typeof(int), typeof(ColorPickerWheel), new PropertyMetadata(0, new PropertyChangedCallback(OnHuePropertyChanged)));
		public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register("Saturation", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(1.0, new PropertyChangedCallback(OnSaturationPropertyChanged)));
		public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register("Brightness", typeof(double), typeof(ColorPickerWheel), new PropertyMetadata(.5, new PropertyChangedCallback(OnBrightnessPropertyChanged)));
		public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(SolidColorBrush), typeof(ColorPickerWheel), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)), new PropertyChangedCallback(OnSelectedColorPropertyChanged)));
		public static readonly DependencyProperty IsInertiaEnabledProperty = DependencyProperty.Register("IsInertiaEnabled", typeof(bool), typeof(ColorPickerWheel), new PropertyMetadata(true, new PropertyChangedCallback(OnIsInertiaEnabledPropertyChanged)));
		#endregion

		#region Public Events
		public event EventHandler<ValueChangedEventArgs<double>> ActualOuterDiameterChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> InnerDiameterChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> ActualInnerDiameterChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> ActualOuterRadiusChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> ActualTopBottomGapChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> ActualLeftRightGapChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> IndicatorOffsetChanged = null;
		public event EventHandler<ValueChangedEventArgs<object>> IndicatorChanged = null;
		public event EventHandler<ValueChangedEventArgs<object>> IndicatorScaleChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> RotationChanged = null;
		public event EventHandler<ValueChangedEventArgs<int>> HueChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> SaturationChanged = null;
		public event EventHandler<ValueChangedEventArgs<double>> BrightnessChanged = null;
		public event EventHandler<ValueChangedEventArgs<SolidColorBrush>> SelectedColorChanged = null;
		public event EventHandler<ValueChangedEventArgs<bool>> IsInertiaEnabledChanged = null;

		protected virtual void RaiseActualOuterDiameterChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnActualOuterDiameterChangedEvent(this, e);
			this.ActualOuterDiameterChanged?.Invoke(this, e);
		}

		protected virtual void OnActualOuterDiameterChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseInnerDiameterChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnInnerDiameterChangedEvent(this, e);
			this.InnerDiameterChanged?.Invoke(this, e);
		}

		protected virtual void OnInnerDiameterChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseActualInnerDiameterChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnActualInnerDiameterChangedEvent(this, e);
			this.ActualInnerDiameterChanged?.Invoke(this, e);
		}

		protected virtual void OnActualInnerDiameterChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseActualOuterRadiusChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnActualOuterRadiusChangedEvent(this, e);
			this.ActualOuterRadiusChanged?.Invoke(this, e);
		}

		protected virtual void OnActualOuterRadiusChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseActualTopBottomGapChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnActualTopBottomGapChangedEvent(this, e);
			this.ActualTopBottomGapChanged?.Invoke(this, e);
		}

		protected virtual void OnActualTopBottomGapChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseActualLeftRightGapChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnActualLeftRightGapChangedEvent(this, e);
			this.ActualLeftRightGapChanged?.Invoke(this, e);
		}

		protected virtual void OnActualLeftRightGapChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseIndicatorOffsetChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnIndicatorOffsetChangedEvent(this, e);
			this.IndicatorOffsetChanged?.Invoke(this, e);
		}

		protected virtual void OnIndicatorOffsetChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseIndicatorChangedEvent(object previousValue, object newValue)
		{
			ValueChangedEventArgs<object> e = new ValueChangedEventArgs<object>(previousValue, newValue);
			this.OnIndicatorChangedEvent(this, e);
			this.IndicatorChanged?.Invoke(this, e);
		}

		protected virtual void OnIndicatorChangedEvent(object sender, ValueChangedEventArgs<object> e)
		{
		}

		protected virtual void RaiseIndicatorScaleChangedEvent(object previousValue, object newValue)
		{
			ValueChangedEventArgs<object> e = new ValueChangedEventArgs<object>(previousValue, newValue);
			this.OnIndicatorScaleChangedEvent(this, e);
			this.IndicatorScaleChanged?.Invoke(this, e);
		}

		protected virtual void OnIndicatorScaleChangedEvent(object sender, ValueChangedEventArgs<object> e)
		{
		}

		protected virtual void RaiseRotationChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnRotationChangedEvent(this, e);
			this.RotationChanged?.Invoke(this, e);
		}

		protected virtual void OnRotationChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseHueChangedEvent(int previousValue, int newValue)
		{
			ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(previousValue, newValue);
			this.OnHueChangedEvent(this, e);
			this.HueChanged?.Invoke(this, e);
		}

		protected virtual void OnHueChangedEvent(object sender, ValueChangedEventArgs<int> e)
		{
		}

		protected virtual void RaiseSaturationChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnSaturationChangedEvent(this, e);
			this.SaturationChanged?.Invoke(this, e);
		}

		protected virtual void OnSaturationChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseBrightnessChangedEvent(double previousValue, double newValue)
		{
			ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(previousValue, newValue);
			this.OnBrightnessChangedEvent(this, e);
			this.BrightnessChanged?.Invoke(this, e);
		}

		protected virtual void OnBrightnessChangedEvent(object sender, ValueChangedEventArgs<double> e)
		{
		}

		protected virtual void RaiseSelectedColorChangedEvent(SolidColorBrush previousValue, SolidColorBrush newValue)
		{
			ValueChangedEventArgs<SolidColorBrush> e = new ValueChangedEventArgs<SolidColorBrush>(previousValue, newValue);
			this.OnSelectedColorChangedEvent(this, e);
			this.SelectedColorChanged?.Invoke(this, e);
		}

		protected virtual void OnSelectedColorChangedEvent(object sender, ValueChangedEventArgs<SolidColorBrush> e)
		{
		}

		protected virtual void RaiseIsInertiaEnabledChangedEvent(bool previousValue, bool newValue)
		{
			ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(previousValue, newValue);
			this.OnIsInertiaEnabledChangedEvent(this, e);
			this.IsInertiaEnabledChanged?.Invoke(this, e);
		}

		protected virtual void OnIsInertiaEnabledChangedEvent(object sender, ValueChangedEventArgs<bool> e)
		{
		}
		#endregion

		#region Dependency Property Change Callbacks
		private static void OnActualOuterDiameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.RaiseInnerDiameterChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
			}
		}

		private static void OnInnerDiameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				if (instance.InnerDiameter >= 0 && instance.InnerDiameter <= 1.0)
				{
					instance.RaiseInnerDiameterChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
				}
				else
				{
					throw new ArgumentOutOfRangeException("InnerDiameter", "Inner Diameter must be a value between 0 and 1.0.");
				}
			}
		}

		private static void OnActualInnerDiameterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.RaiseActualInnerDiameterChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
			}
		}

		private static void OnActualOuterRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.RaiseActualOuterRadiusChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
			}
		}

		private static void OnActualTopBottomGapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.RaiseActualTopBottomGapChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
			}
		}

		private static void OnActualLeftRightGapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.RaiseActualLeftRightGapChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
			}
		}

		private static void OnIndicatorOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.RaiseIndicatorOffsetChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
			}
		}

		private static void OnIndicatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.RaiseIndicatorChangedEvent(e.OldValue, e.NewValue);
			}
		}

		private static void OnIndicatorScalePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.RaiseIndicatorScaleChangedEvent(e.OldValue, e.NewValue);
			}
		}

		private static void OnRotationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				double rotation = Convert.ToDouble(e.NewValue);
				instance.RotaryCompositeTransform.Rotation = rotation % 360;
				instance.Hue = rotation.GetHueFromRotation();

				instance.RaiseRotationChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
			}
		}

		private static void OnHuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				if (instance.Hue >= 0 && instance.Hue <= 360)
				{
					if (instance.RotaryCompositeTransform != null)
					{
						instance.RotaryCompositeTransform.Rotation = instance.Hue.GetRotationFromHue();
						instance.ApplySelectedColor();
						instance.RaiseHueChangedEvent(Convert.ToInt32(e.OldValue), Convert.ToInt32(e.NewValue));
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException("Hue", "Hue must be a value from 0 to 360.");
				}
			}
		}

		private static void OnSaturationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				if (instance.Saturation >= 0 && instance.Saturation <= 1.0)
				{
					instance.ApplySelectedColor();
					instance.RaiseSaturationChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
				}
				else
				{
					throw new ArgumentOutOfRangeException("Saturation", "Saturation must be a value from 0 to 1.0.");
				}
			}
		}

		private static void OnBrightnessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				if (instance.Brightness >= 0 && instance.Brightness <= 1.0)
				{
					instance.ApplySelectedColor();
					instance.RaiseBrightnessChangedEvent(Convert.ToDouble(e.OldValue), Convert.ToDouble(e.NewValue));
				}
				else
				{
					throw new ArgumentOutOfRangeException("Brightness", "Brightness must be a value from 0 to 1.0.");
				}
			}
		}

		private static void OnIsInertiaEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.SetManipulationMode();
				instance.RaiseIsInertiaEnabledChangedEvent(Convert.ToBoolean(e.OldValue), Convert.ToBoolean(e.NewValue));
			}
		}

		private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ColorPickerWheel instance)
			{
				instance.RaiseSelectedColorChangedEvent((SolidColorBrush)e.OldValue, (SolidColorBrush)e.NewValue);
			}
		}
		#endregion

		#region Control Event Handlers
		protected override void OnApplyTemplate()
		{
			if (this.GetTemplateChild("PART_Rotary") is Border rotary)
			{
				this.Rotary = rotary;

				this.Rotary.ManipulationStarting += this.Rotary_ManipulationStarting;
				this.Rotary.ManipulationStarted += this.Rotary_ManipulationStarted;
				this.Rotary.ManipulationDelta += this.Rotary_ManipulationDelta;
				this.Rotary.Tapped += this.Rotary_Tapped;

				this.Rotary.PointerEntered += this.Rotary_PointerEntered;
				this.Rotary.PointerPressed += this.Rotary_PointerPressed;
				this.Rotary.PointerExited += this.Rotary_PointerExited;
				this.Rotary.PointerReleased += this.Rotary_PointerReleased;

				this.SetManipulationMode();

				this.RotaryCompositeTransform = ((CompositeTransform)this.Rotary.RenderTransform);
				this.RotaryCompositeTransform.Rotation = this.Hue.GetRotationFromHue();
			}

			// ***
			// *** Create the default indicator control.
			// ***
			if (this.Indicator == null)
			{
				this.Indicator = new IndicatorArrow();
			}

			// ***
			// *** Set initial state
			// ***
			if (this.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Normal", true);
			}
			else
			{
				VisualStateManager.GoToState(this, "Disabled", true);
			}

			// ***
			// *** Initialize the selected color.
			// ***
			this.ApplySelectedColor();

			base.OnApplyTemplate();
		}

		private void Rotary_PointerEntered(object sender, PointerRoutedEventArgs e)
		{
			_pointerEntered = true;
			VisualStateManager.GoToState(this, "PointerOver", true);
			base.OnPointerEntered(e);
		}

		private void Rotary_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "Pressed", true);
			base.OnPointerPressed(e);
		}

		private void Rotary_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			if (_pointerEntered)
			{
				VisualStateManager.GoToState(this, "PointerOver", true);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", true);
			}

			base.OnPointerReleased(e);
		}

		private void Rotary_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			_pointerEntered = false;
			VisualStateManager.GoToState(this, "Normal", true);
			base.OnPointerExited(e);
		}

		private void Rotary_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
		{
			FrameworkElement element = sender as FrameworkElement;
			e.Pivot = new ManipulationPivot(element.Center(), element.ActualWidth / 2.0);
		}

		private void Rotary_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		{
			this.Rotation += e.Cumulative.Rotation;
			e.Handled = true;
		}

		private void Rotary_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			this.Rotation += e.Delta.Rotation;
			e.Handled = true;
		}

		private void Rotary_Tapped(object sender, TappedRoutedEventArgs e)
		{
			FrameworkElement element = sender as FrameworkElement;

			Point point = e.GetPosition(element);
			Point center = element.Center();

			// ***
			// *** The rotation angle is the opposite of the angle as
			// *** calculated in the euclidean space.
			// ***
			double rotation = center.GetAngle(point).ToDegrees().ToRotation();

			this.Rotation = rotation;
		}

		private void Center_Tapped(object sender, TappedRoutedEventArgs e)
		{
			// ***
			// *** Don't allow tapping the center to change the color.
			// ***
			e.Handled = true;
		}

		private void HueColorPicker_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is bool enabled)
			{
				if (enabled)
				{
					VisualStateManager.GoToState(this, "Normal", true);
				}
				else
				{
					VisualStateManager.GoToState(this, "Disabled", true);
				}
			}
		}
		#endregion

		#region Internal Methods & Properties
		protected ManipulationModes ManipulationWithInertia { get; } = ManipulationModes.TranslateInertia | ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Rotate | ManipulationModes.RotateInertia;
		protected ManipulationModes ManipulationWithoutInertia { get; } = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Rotate;

		protected Border Rotary { get; set; }
		protected CompositeTransform RotaryCompositeTransform { get; set; }

		protected void SetManipulationMode()
		{
			if (this.IsInertiaEnabled)
			{
				this.Rotary.ManipulationMode = this.ManipulationWithInertia;
			}
			else
			{
				this.Rotary.ManipulationMode = this.ManipulationWithoutInertia;
			}
		}

		protected void ApplySelectedColor()
		{
			this.SelectedColor = new SolidColorBrush(Microsoft.Toolkit.Uwp.Helpers.ColorHelper.FromHsv(this.Hue, this.Saturation, this.Brightness));
		}
		#endregion

		#region Public Methods & Properties
		public double ActualOuterDiameter
		{
			get
			{
				return (double)this.GetValue(ActualOuterDiameterProperty);
			}
			protected set
			{
				this.SetValue(ActualOuterDiameterProperty, value);
			}
		}

		public double InnerDiameter
		{
			get
			{
				return (double)this.GetValue(InnerDiameterProperty);
			}
			set
			{
				this.SetValue(InnerDiameterProperty, value);
			}
		}

		public double ActualInnerDiameter
		{
			get
			{
				return (double)this.GetValue(ActualInnerDiameterProperty);
			}
			protected set
			{
				this.SetValue(ActualInnerDiameterProperty, value);
			}
		}

		public double ActualOuterRadius
		{
			get
			{
				return (double)this.GetValue(ActualOuterRadiusProperty);
			}
			protected set
			{
				this.SetValue(ActualOuterRadiusProperty, value);
			}
		}

		public double ActualTopBottomGap
		{
			get
			{
				return (double)this.GetValue(ActualTopBottomGapProperty);
			}
			protected set
			{
				this.SetValue(ActualTopBottomGapProperty, value);
			}
		}

		public double ActualLeftRightGap
		{
			get
			{
				return (double)this.GetValue(ActualLeftRightGapProperty);
			}
			protected set
			{
				this.SetValue(ActualLeftRightGapProperty, value);
			}
		}

		public new double Rotation
		{
			get
			{
				return (double)this.GetValue(RotationProperty);
			}
			set
			{
				this.SetValue(RotationProperty, value);
			}
		}

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

		public SolidColorBrush SelectedColor
		{
			get
			{
				return (SolidColorBrush)this.GetValue(SelectedColorProperty);
			}
			private set
			{
				this.SetValue(SelectedColorProperty, value);
			}
		}

		public double IndicatorOffset
		{
			get
			{
				return (double)this.GetValue(IndicatorOffsetProperty);
			}
			set
			{
				this.SetValue(IndicatorOffsetProperty, value);
			}
		}

		public object Indicator
		{
			get
			{
				return this.GetValue(IndicatorProperty);
			}
			set
			{
				this.SetValue(IndicatorProperty, value);
			}
		}

		public double IndicatorScale
		{
			get
			{
				return (double)this.GetValue(IndicatorScaleProperty);
			}
			set
			{
				this.SetValue(IndicatorScaleProperty, value);
			}
		}

		public bool IsInertiaEnabled
		{
			get
			{
				return (bool)this.GetValue(IsInertiaEnabledProperty);
			}
			set
			{
				this.SetValue(IsInertiaEnabledProperty, value);
			}
		}

		public void SetSelectedColor(Color color)
		{
			Microsoft.Toolkit.Uwp.HsvColor hsv = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToHsv(color);
			this.Hue = (int)hsv.H;
			this.Saturation = hsv.S > 1.0 ? 1.0 : hsv.S < 0.0 ? 0 : hsv.S;
			this.Brightness = hsv.V > 1.0 ? 1.0 : hsv.V < 0.0 ? 0 : hsv.V;
		}

		public void ResetColor()
		{
			this.Hue = 0;
			this.Saturation = 1.0;
			this.Brightness = 1.0;
		}
		#endregion
	}
}
