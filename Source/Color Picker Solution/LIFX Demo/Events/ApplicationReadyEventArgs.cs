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
using System.Collections.Generic;
using Prism.Windows.Navigation;
using Windows.UI.Core;

namespace LifxDemo.Events
{
	public class ApplicationReadyEventArgs
    {
		public ApplicationReadyEventArgs(CoreDispatcher dispatcher, NavigatedToEventArgs navigatedToEventArgs, Dictionary<string, object> viewModelState)
		{
			this.Dispatcher = dispatcher;
			this.NavigatedToEventArgs = navigatedToEventArgs;
			this.ViewModelState = viewModelState;
		}

		public CoreDispatcher Dispatcher { get; private set; }
		public NavigatedToEventArgs NavigatedToEventArgs { get; private set; }
		public Dictionary<string, object> ViewModelState { get; private set; }
	}
}
