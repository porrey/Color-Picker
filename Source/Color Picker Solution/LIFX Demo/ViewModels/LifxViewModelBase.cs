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
using System.Threading.Tasks;
using LifxDemo.Events;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace LifxDemo.ViewModels
{
	public abstract class LifxViewModelBase : ViewModelBase
	{
		private SubscriptionToken _token1 = null;
		private SubscriptionToken _token2 = null;

		public LifxViewModelBase(bool attachApplicationEvents)
		{
			this.OnRegisterCommands();

			if (attachApplicationEvents)
			{
				_token1 = this.EventAggregator.GetEvent<ApplicationReadyEvent>().Subscribe((e) => this.OnApplicationReadyEvent(e), ThreadOption.UIThread);
				_token2 = this.EventAggregator.GetEvent<ApplicationClosingEvent>().Subscribe((e) => this.OnApplicationClosingEvent(e), ThreadOption.UIThread);
			}
		}

		protected CoreDispatcher Dispatcher { get; set; }
		protected IEventAggregator EventAggregator => ServiceLocator.Current.GetInstance<IEventAggregator>();
		protected IResourceLoader ResourceLoader => ServiceLocator.Current.GetInstance<IResourceLoader>();

		private bool _isActive = false;
		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				this.SetProperty(ref _isActive, value);
				this.RaisePropertyChanged(nameof(this.NotIsActive));
			}
		}

		public bool NotIsActive
		{
			get
			{
				return !this.IsActive;
			}
		}

		public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
		{
			base.OnNavigatedTo(e, viewModelState);

			// ***
			// *** Get and set the dispatcher.
			// ***
			this.Dispatcher = Window.Current.Dispatcher;

			// ***
			// *** Publish this event for view models that are attached to a
			// *** control and do not receive the normal OnNavigatedTo event.
			// ***
			this.EventAggregator.GetEvent<ApplicationReadyEvent>().Publish(new ApplicationReadyEventArgs(this.Dispatcher, e, viewModelState));
		}

		public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
		{
			base.OnNavigatingFrom(e, viewModelState, suspending);

			// ***
			// *** Publish this event for view models that are attached to a
			// *** control and do not receive the normal OnNavigatingFrom event.
			// ***
			this.EventAggregator.GetEvent<ApplicationClosingEvent>().Publish(new ApplicationClosingEventArgs(e, viewModelState, suspending));

			if (_token1 != null)
			{
				this.EventAggregator.GetEvent<ApplicationReadyEvent>().Unsubscribe(_token1);
			}

			if (_token2 != null)
			{
				this.EventAggregator.GetEvent<ApplicationClosingEvent>().Unsubscribe(_token2);
			}
		}

		protected virtual Task OnApplicationReadyEvent(ApplicationReadyEventArgs e)
		{
			return Task.FromResult(0);
		}

		protected virtual Task OnApplicationClosingEvent(ApplicationClosingEventArgs e)
		{
			return Task.FromResult(0);
		}

		protected IList<DelegateCommandBase> Commands { get; private set; } = new List<DelegateCommandBase>();

		protected DelegateCommand<T> RegisterCommand<T>(Action<T> executeMethod)
		{
			DelegateCommand<T> command = new DelegateCommand<T>(executeMethod);
			this.Commands.Add(command);
			return command;
		}

		protected DelegateCommand<T> RegisterCommand<T>(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
		{
			DelegateCommand<T> command = new DelegateCommand<T>(executeMethod, canExecuteMethod);
			this.Commands.Add(command);
			return command;
		}

		protected DelegateCommand RegisterCommand(Action executeMethod)
		{
			DelegateCommand command = new DelegateCommand(executeMethod);
			this.Commands.Add(command);
			return command;
		}

		protected DelegateCommand RegisterCommand(Action executeMethod, Func<bool> canExecuteMethod)
		{
			DelegateCommand command = new DelegateCommand(executeMethod, canExecuteMethod);
			this.Commands.Add(command);
			return command;
		}

		protected Task RefreshCommands()
		{
			foreach (DelegateCommandBase command in this.Commands)
			{
				command.RaiseCanExecuteChanged();
			}

			return Task.FromResult(0);
		}

		protected virtual void OnRegisterCommands()
		{
		}
	}
}
