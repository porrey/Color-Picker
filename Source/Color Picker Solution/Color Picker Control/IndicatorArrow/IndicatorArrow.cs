using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
			if (this.GetTemplateChild("PART_Triangle") is Polygon triangle)
			{
				this.Triangle = triangle;
			}

			//this.Background = new SolidColorBrush(Colors.Transparent);
			//this.BorderBrush = new SolidColorBrush(Colors.Transparent);
			//this.BorderThickness = new Thickness(0);

			base.OnApplyTemplate();
		}

		private void Selector_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.Triangle.Points.Clear();
			this.Triangle.Points.Add(new Point(0, 0));
			this.Triangle.Points.Add(new Point(this.ActualWidth, 0));
			this.Triangle.Points.Add(new Point(this.ActualWidth / 2.0, this.ActualHeight));
		}

		protected Polygon Triangle { get; set; }
	}
}
