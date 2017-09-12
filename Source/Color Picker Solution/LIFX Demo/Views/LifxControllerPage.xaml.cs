using System.Linq;
using Windows.UI.Xaml.Controls;

namespace LifxDemo.Views
{
	public sealed partial class LifxControllerPage : Page
	{
		public LifxControllerPage()
		{
			this.InitializeComponent();
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			object item = e.AddedItems.FirstOrDefault();

			if (item != null)
			{
				if (sender is ListView lv)
				{
					lv.ScrollIntoView(item);
				}
			}
		}
	}
}
