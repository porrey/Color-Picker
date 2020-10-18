using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Porrey.Controls.ColorPicker;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ColorPickerDemo
{
	public partial class MainPage : Page, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = null;

		public MainPage()
		{
			this.InitializeComponent();
			this.ColorPicker.HueChanged += this.ColorPicker_HueChanged;
		}

		private void ColorPicker_HueChanged(object sender, ValueChangedEventArgs<int> e)
		{
			this.PowerSwitch.Hue = e.NewValue;
		}

		protected void RaisedPropertyChangedEvent([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.ColorPicker.ResetColor();
		}

		private void ColorPicker_SelectedColorChanged(object sender, ValueChangedEventArgs<SolidColorBrush> e)
		{
			this.RaisedPropertyChangedEvent(nameof(this.SelectedColor));
			this.RaisedPropertyChangedEvent(nameof(this.Red));
			this.RaisedPropertyChangedEvent(nameof(this.RedPercent));
			this.RaisedPropertyChangedEvent(nameof(this.Green));
			this.RaisedPropertyChangedEvent(nameof(this.GreenPercent));
			this.RaisedPropertyChangedEvent(nameof(this.Blue));
			this.RaisedPropertyChangedEvent(nameof(this.BluePercent));
			this.RaisedPropertyChangedEvent(nameof(this.HueFontColor));
			this.RaisedPropertyChangedEvent(nameof(this.RedFontColor));
			this.RaisedPropertyChangedEvent(nameof(this.GreenFontColor));
			this.RaisedPropertyChangedEvent(nameof(this.BlueFontColor));
		}

		public SolidColorBrush SelectedColor => this.ColorPicker.SelectedColor;
		public SolidColorBrush Red => new SolidColorBrush(Color.FromArgb(255, this.SelectedColor.Color.R, 0, 0));
		public SolidColorBrush Green => this.SelectedColor != null ? new SolidColorBrush(Color.FromArgb(255, 0, this.SelectedColor.Color.G, 0)) : new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
		public SolidColorBrush Blue => this.SelectedColor != null ? new SolidColorBrush(Color.FromArgb(255, 0, 0, this.SelectedColor.Color.B)) : new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

		public double RedPercent => this.Red.Color.R / 255.0;
		public double GreenPercent => this.Green.Color.G / 255.0;
		public double BluePercent => this.Blue.Color.B / 255.0;

		public SolidColorBrush HueFontColor
		{
			get
			{
				Color returnValue = Colors.White;

				// ***
				// *** Get the three color values weighted. The total
				// *** sum of the three colors below will be 255.
				// ***
				double r = this.SelectedColor.Color.R * 0.333;
				double g = this.SelectedColor.Color.G * 0.533;
				double b = this.SelectedColor.Color.B * 0.134;

				// ***
				// *** Color sum: the larger the value the "brighter" the
				// *** color. Inversely, the lower the number the darker
				// *** the color.
				// ***
				double sum = r + g + b;

				// ***
				// *** Calculate the total as a percentage.
				// ***
				double colorMagnitude = sum / 255.0;

				// ***
				// *** If the color is "lighter" use a black
				// *** font color otherwise use white.
				// ***
				if (colorMagnitude > .5)
				{
					returnValue = Colors.Black;
				}
				else
				{
					returnValue = Colors.White;
				}

				return new SolidColorBrush(returnValue);
			}
		}

		public SolidColorBrush RedFontColor
		{
			get
			{
				Color returnValue = Colors.Wheat;

				if (this.RedPercent < .60)
				{
					returnValue = Colors.Black;
				}

				return new SolidColorBrush(returnValue);
			}
		}

		public SolidColorBrush GreenFontColor
		{
			get
			{
				Color returnValue = Colors.Wheat;

				if (this.GreenPercent < .60)
				{
					returnValue = Colors.Black;
				}

				return new SolidColorBrush(returnValue);
			}
		}

		public SolidColorBrush BlueFontColor
		{
			get
			{
				Color returnValue = Colors.Wheat;

				if (this.BluePercent < .60)
				{
					returnValue = Colors.Black;
				}

				return new SolidColorBrush(returnValue);
			}
		}

		public async Task AutoRotate()
		{
			await Task.Delay(2000);

			for (int i = 0; i <= 360; i++)
			{
				this.ColorPicker.Hue = i;
				await Task.Delay(75);
			}
		}
	}
}