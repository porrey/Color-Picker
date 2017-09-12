using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LifxDemo.Common;
using LifxDemo.Events;
using Prism.Events;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace LifxDemo.ViewModels
{
	public class LifxControllerPageViewModel : LifxViewModelBase
	{
		private SubscriptionToken _token = null;
		private bool _setting = false;

		public LifxControllerPageViewModel()
			: base(true)
		{
			Dictionary<string, ushort> items = new Dictionary<string, ushort>
			{
				{"Ultra Warm", 2500 }, { "Incandescent", 2750}, {"Warm", 3000 }, {"Neutral Warm", 3200 },
				{"Neutral", 3500 }, {"Cool", 4000 }, {"Cool Daylight", 4500 }, {"Soft Daylight", 5000 },
				{"Daylight", 5500 }, {"Noon Daylight", 6000 }, {"Bright Daylight", 6500 }, {"Cloudy Daylight", 7000 },
				{"Blue Daylight", 7500 }, {"Blue Overcast", 8000 }, {"Blue Water", 8500 }, {"Blue Ice", 9000 }
			};


			foreach (KeyValuePair<string, ushort> item in items)
			{
				double h = 33;
				double s = 33.0 * (1 - ((double)item.Value / 9000.0)) / 100.0;
				double b = 1.0;
				Color color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.FromHsv(h, s, b);

				this.WhiteItems.Add(new WhiteItem() { Name = $"{item.Key}", Color = new SolidColorBrush(color), Kelvin = item.Value });
			}
		}

		public ObservableCollection<WhiteItem> WhiteItems { get; } = new ObservableCollection<WhiteItem>();

		protected override async Task OnApplicationReadyEvent(ApplicationReadyEventArgs e)
		{
			this.SelectedItem = null;
			_token = this.EventAggregator.GetEvent<SelectedItemEvent>().Subscribe((args) => this.OnSelectedItemEvent(args));

			await base.OnApplicationReadyEvent(e);
		}

		protected override Task OnApplicationClosingEvent(ApplicationClosingEventArgs e)
		{
			this.SelectedItem = null;

			if (_token != null)
			{
				this.EventAggregator.GetEvent<SelectedItemEvent>().Unsubscribe(_token);
			}

			return base.OnApplicationClosingEvent(e);
		}

		protected async void OnSelectedItemEvent(SelectedItemEventArgs e)
		{
			this.SelectedItem = e.LifxBulb;

			if (this.SelectedItem != null)
			{
				await this.SelectedItem.Update();
			}
		}

		private LifxItem _selectedItem = null;
		public LifxItem SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				this.SetProperty(ref _selectedItem, value);

				if (_selectedItem != null)
				{
					// ***
					// *** The kelvin value on the bulb may not be an exact
					// *** match for an item in the list. Find the closest 
					// *** item.
					// ***
					var closestItem = (from tbl in this.WhiteItems
									   select new
									   {
										   d = Math.Abs(tbl.Kelvin - _selectedItem.Kelvin),
										   k = tbl.Kelvin
									   }).OrderBy(s => s.d).FirstOrDefault();

					// ***
					// *** Select the item.
					// ***
					WhiteItem item = this.WhiteItems.Where(r => r.Kelvin == closestItem.k).SingleOrDefault();

					try
					{
						_setting = true;

						if (item != null)
						{
							this.SelectedWhiteItem = item;
						}
						else
						{
							this.SelectedWhiteItem = null;
						}
					}
					finally
					{
						_setting = false;
					}
				}
			}
		}

		private WhiteItem _selectedWhiteItem = null;
		public WhiteItem SelectedWhiteItem
		{
			get
			{
				return _selectedWhiteItem;
			}
			set
			{
				this.SetProperty(ref _selectedWhiteItem, value);

				if (this.SelectedItem != null && !_setting)
				{
					this.SelectedItem.Kelvin = _selectedWhiteItem.Kelvin;
				}
			}
		}
	}
}
