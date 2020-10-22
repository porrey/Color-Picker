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
using System.Threading.Tasks;
using LifxDemo.Common;
using LifxDemo.Models;
using LifxNet;
using Prism.Mvvm;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LifxDemo.ViewModels
{
	public class LifxItem : BindableBase, IDisposable
	{
		protected ApiRateLimiter Limiter { get; set; }

		public LifxItem(LifxClient client, LightBulb lightBulb)
		{
			this.Client = client;
			this.LightBulb = lightBulb;
			this.Limiter = new ApiRateLimiter(TimeSpan.FromMilliseconds(50), this.ApiLimiterCallback, TimeSpan.Zero);

			this.Update().RunAsync();
		}

		private async void ApiLimiterCallback()
		{
			await this.Update();
		}

		protected LifxClient Client { get; set; }
		protected LightBulb LightBulb { get; set; }

		public string HostName => this.LightBulb?.HostName;
		public string MacAddress => this.LightBulb?.MacAddressName.ToLower();

		private string _name = "Unknown";
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				this.SetProperty(ref _name, value);
			}
		}

		private SolidColorBrush _lightColor = new SolidColorBrush(Colors.White);
		public SolidColorBrush LightColor
		{
			get
			{
				return _lightColor;
			}
			set
			{
				this.SetProperty(ref _lightColor, value);
			}
		}

		private bool _isOn = false;
		public bool IsOn
		{
			get
			{
				return _isOn;
			}
			set
			{
				this.SetProperty(ref _isOn, value);
				this.Limiter.ThrottleMethod(async () => { await this.SetPower(value); });
			}
		}

		private ushort _hue = 0;
		public ushort Hue
		{
			get
			{
				return _hue;
			}
			set
			{
				this.SetProperty(ref _hue, value);
				this.Limiter.ThrottleMethod(async () => { await this.SetHue(_hue); });
				this.SetColor();
			}
		}

		private double _saturation = 0;
		public double Saturation
		{
			get
			{
				return _saturation;
			}
			set
			{
				this.SetProperty(ref _saturation, value);
				this.Limiter.ThrottleMethod(async () => { await this.SetSaturation(_saturation); });
				this.SetColor();
			}
		}

		private double _brightness = 0;
		public double Brightness
		{
			get
			{
				return _brightness;
			}
			set
			{
				this.SetProperty(ref _brightness, value);
				this.Limiter.ThrottleMethod(async () => { await this.SetBrightness(_brightness); });
				this.SetColor();
			}
		}

		private ushort _kelvin = 0;
		public ushort Kelvin
		{
			get
			{
				return _kelvin;
			}
			set
			{
				this.SetProperty(ref _kelvin, value);
				this.SetKelvin(_kelvin);
				this.SetColor();
			}
		}

		public async Task Update()
		{
			try
			{
				// ***
				// *** Locking the API limiter prevents all calls
				// *** from being made.
				// ***
				await this.Limiter.Lock();

				if (this.Client != null)
				{
					LightStateResponse state = await this.Client.GetLightStateAsync(this.LightBulb);

					if (state != null)
					{
						// ***
						// *** Since this is a view model, it is possible that the bindable properties such
						// *** as LightColor are connected to the UI. For that reason, only update them on
						// *** the UI thread.
						// ***
						await ((DispatchedHandler)(() =>
						{
							this.Name = state.Label;
							this.IsOn = state.IsOn;
							this.Hue = Lifx.Hue.FromLifx(state.Hue);
							this.Saturation = Lifx.Saturation.FromLifx(state.Saturation);
							this.Brightness = Lifx.Brightness.FromLifx(state.Brightness);
							this.Kelvin = state.Kelvin;
							this.LightColor = Lifx.Rgb.CreateSolidColorBrush(state.Hue, state.Saturation, state.Brightness);
						})).RunOnUiThread();
					}
				}
			}
			finally
			{
				// ***
				// *** Unlock the API limiter.
				// ***
				await this.Limiter.Unlock();
			}
		}

		public async Task SetPower(bool on)
		{
			if (this.Client != null)
			{
				await this.Client.SetDevicePowerStateAsync(this.LightBulb, on);
			}
		}

		public async Task SetHue(ushort hue)
		{
			ushort h = Lifx.Hue.ToLifx(hue);
			ushort s = Lifx.Saturation.ToLifx(this.Saturation);
			ushort b = Lifx.Brightness.ToLifx(this.Brightness);

			await this.Client.SetColorAsync(this.LightBulb, h, s, b, this.Kelvin, TimeSpan.Zero);
		}

		public async Task SetSaturation(double saturation)
		{
			ushort h = Lifx.Hue.ToLifx(this.Hue);
			ushort s = Lifx.Saturation.ToLifx(saturation);
			ushort b = Lifx.Brightness.ToLifx(this.Brightness);

			await this.Client.SetColorAsync(this.LightBulb, h, s, b, this.Kelvin, TimeSpan.Zero);
		}

		public async Task SetBrightness(double brightness)
		{
			ushort h = Lifx.Hue.ToLifx(this.Hue);
			ushort s = Lifx.Saturation.ToLifx(this.Saturation);
			ushort b = Lifx.Brightness.ToLifx(brightness);

			await this.Client.SetColorAsync(this.LightBulb, h, s, b, this.Kelvin, TimeSpan.Zero);
		}

		public Task SetKelvin(ushort kelvin)
		{
			ushort h = Lifx.Hue.ToLifx(this.Hue);
			ushort s = Lifx.Saturation.ToLifx(this.Saturation);
			ushort b = Lifx.Brightness.ToLifx(this.Brightness);

			this.Client.SetColorAsync(this.LightBulb, h, s, b, kelvin, TimeSpan.Zero);
			return Task.FromResult(0);
		}

		public void Dispose()
		{
			//this.UpdateStateTimer.Tick -= this.UpdateStateTimer_Tick;

			if (this.Limiter != null)
			{
				this.Limiter.Dispose();
				this.Limiter = null;
			}

			if (this.Client != null)
			{
				// ***
				// *** Do not dispose the client here, it is shared and 
				// *** will be disposed by the owner.
				// ***
				this.Client = null;
			}

			if (this.LightBulb != null)
			{
				this.LightBulb = null;
			}
		}

		protected async void SetColor()
		{
			ushort h = Lifx.Hue.ToLifx(this.Hue);
			ushort s = Lifx.Saturation.ToLifx(this.Saturation);
			ushort b = Lifx.Brightness.ToLifx(this.Brightness);

			await ((DispatchedHandler)(() =>
			{
				this.LightColor = Lifx.Rgb.CreateSolidColorBrush(h, s, b);
			})).RunOnUiThread();
		}
	}
}
