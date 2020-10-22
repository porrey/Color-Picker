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
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using LifxDemo.Common;
using LifxDemo.Events;
using LifxNet;
using Windows.UI.Core;

namespace LifxDemo.ViewModels
{
	public class LifxListPageViewModel : LifxViewModelBase
	{
		public LifxListPageViewModel()
			: base(true)
		{
		}

		public ObservableCollection<LifxItem> Items { get; } = new ObservableCollection<LifxItem>();
		protected LifxClient LifxClient { get; set; }

		protected async override Task OnApplicationReadyEvent(ApplicationReadyEventArgs e)
		{
			try
			{
				// ***
				// *** Subscribe to refresh events.
				// ***
				this.EventAggregator.GetEvent<RefreshEvent>().Subscribe((args) => this.OnRefreshEvent(args));

				// ***
				// *** Set the dispatcher.
				// ***
				this.Dispatcher = e.Dispatcher;

				// ***
				// *** Initialize the LIFX client.
				// ***
				await this.InitializeLifxClient();
			}
			catch (Exception ex)
			{
				this.EventAggregator.GetEvent<MessageEvent>().Publish(new MessageEventArgs(ex));
			}
		}

		protected override async Task OnApplicationClosingEvent(ApplicationClosingEventArgs e)
		{
			// ***
			// *** Release the LFIX client.
			// ***
			await this.DeinitializeLifxClient();
		}

		protected async Task InitializeLifxClient()
		{
			this.EventAggregator.GetEvent<MessageEvent>().Publish(new MessageEventArgs(MessageEventType.Information, "Ready."));

			try
			{
				// ***
				// *** Configure the LIFX client.
				// ***
				this.LifxClient = await LifxClient.CreateAsync();
				this.LifxClient.DeviceDiscovered += this.LifxClient_DeviceDiscovered;
				this.LifxClient.DeviceLost += this.LifxClient_DeviceLost;
				this.LifxClient.StartDeviceDiscovery();
			}
			catch (SocketException)
			{
				string message = this.ResourceLoader.GetString(MagicString.Resource.LifxApplicationError);
				this.EventAggregator.GetEvent<MessageEvent>().Publish(new MessageEventArgs(MessageEventType.Error, message));
			}
			catch (Exception ex)
			{
				this.EventAggregator.GetEvent<MessageEvent>().Publish(new MessageEventArgs(MessageEventType.Error, ex.Message));
			}
		}

		protected Task DeinitializeLifxClient()
		{
			// ***
			// *** Dispose the current items.
			// ***
			foreach (LifxItem item in this.Items)
			{
				item.Dispose();
			}

			// ***
			// *** Clear the list.
			// ***
			this.Items.Clear();

			// ***
			// *** Release the client.
			// ***
			if (this.LifxClient != null)
			{
				this.LifxClient.StopDeviceDiscovery();
				this.LifxClient.DeviceDiscovered -= this.LifxClient_DeviceDiscovered;
				this.LifxClient.DeviceLost -= this.LifxClient_DeviceLost;
				this.LifxClient.Dispose();
				this.LifxClient = null;
			}

			return Task.FromResult(0);
		}

		private async void LifxClient_DeviceDiscovered(object sender, LifxClient.DeviceDiscoveryEventArgs e)
		{
			await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				if (e.Device is LightBulb bulb)
				{
					LifxItem lifxBulb = new LifxItem(this.LifxClient, bulb);

					if (!this.Items.Contains(lifxBulb))
					{
						this.Items.Add(lifxBulb);
					}
				}
			});
		}

		private async void LifxClient_DeviceLost(object sender, LifxClient.DeviceDiscoveryEventArgs e)
		{
			await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				if (e.Device is LightBulb bulb)
				{
					LifxItem item = this.Items.Where(t => t.HostName == bulb.HostName).SingleOrDefault();
					
					if (item != null)
					{
						item.Dispose();
						this.Items.Remove(item);
					}
				}
			});
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
				this.EventAggregator.GetEvent<SelectedItemEvent>().Publish(new SelectedItemEventArgs(value));
			}
		}

		protected async void OnRefreshEvent(RefreshEventArgs e)
		{
			if (e.Rediscover)
			{
				await this.DeinitializeLifxClient();
				await Task.Delay(1000);
				await this.InitializeLifxClient();
			}
			else
			{
				foreach (LifxItem item in this.Items)
				{
					item.Update().RunAsync();
				}
			}
		}
	}
}
