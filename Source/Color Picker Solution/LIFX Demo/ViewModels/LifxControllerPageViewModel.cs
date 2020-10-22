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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
			WhiteItem[] whites = new WhiteItem[]
			{
				new WhiteItem() { Name = "Ultra Warm", Kelvin = 2500, Color = new SolidColorBrush(Color.FromArgb(255, 255, 222, 184)) },
				new WhiteItem() { Name = "Incandescent", Kelvin = 2750, Color = new SolidColorBrush(Color.FromArgb(255, 255, 225, 184))},
				new WhiteItem() { Name = "Warm", Kelvin = 3000 , Color = new SolidColorBrush(Color.FromArgb(255, 255, 228, 194))},
				new WhiteItem() { Name = "Neutral Warm", Kelvin = 3200, Color = new SolidColorBrush(Color.FromArgb(255, 254, 229, 198)) },
				new WhiteItem() { Name = "Neutral", Kelvin = 3500, Color = new SolidColorBrush(Color.FromArgb(255, 253, 229, 201)) },
				new WhiteItem() { Name = "Cool", Kelvin = 4000, Color = new SolidColorBrush(Color.FromArgb(255, 255, 235, 210)) },
				new WhiteItem() { Name = "Cool Daylight", Kelvin = 4500, Color = new SolidColorBrush(Color.FromArgb(255, 255, 239, 217)) },
				new WhiteItem() { Name = "Soft Daylight", Kelvin = 5000, Color = new SolidColorBrush(Color.FromArgb(255, 254, 240, 220)) },
				new WhiteItem() { Name = "Daylight", Kelvin = 5500, Color = new SolidColorBrush(Color.FromArgb(255, 253, 240, 225)) },
				new WhiteItem() { Name = "Noon Daylight", Kelvin = 6000, Color = new SolidColorBrush(Color.FromArgb(255, 249, 242, 230)) },
				new WhiteItem() { Name = "Bright Daylight", Kelvin = 6500, Color = new SolidColorBrush(Color.FromArgb(255, 246, 242, 235)) },
				new WhiteItem() { Name = "Cloudy Daylight", Kelvin = 7000, Color = new SolidColorBrush(Color.FromArgb(255, 242, 240, 237)) },
				new WhiteItem() { Name = "Blue Daylight", Kelvin = 7500, Color = new SolidColorBrush(Color.FromArgb(255, 236, 237, 238)) },
				new WhiteItem() { Name = "Blue Overcast", Kelvin = 8000, Color = new SolidColorBrush(Color.FromArgb(255, 237, 241, 246)) },
				new WhiteItem() { Name = "Blue Water", Kelvin = 8500, Color = new SolidColorBrush(Color.FromArgb(255, 235, 242, 249)) },
				new WhiteItem() { Name = "Blue Ice", Kelvin = 9000, Color = new SolidColorBrush(Color.FromArgb(255, 235, 244, 253)) }
			};

			this.WhiteItems.AddRange(whites);
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
					this.SelectedItem.Saturation = 0;
					this.SelectedItem.Kelvin = _selectedWhiteItem.Kelvin;
				}
			}
		}
	}
}
