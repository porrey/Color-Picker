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
using System.Collections.Generic;
using Prism.Windows.Navigation;

namespace LifxDemo.Events
{
	public class ApplicationClosingEventArgs
	{
		public ApplicationClosingEventArgs(NavigatingFromEventArgs navigatedToEventArgs, Dictionary<string, object> viewModelState, bool suspending)
		{
			this.NavigatedToEventArgs = navigatedToEventArgs;
			this.ViewModelState = viewModelState;
			this.Suspending = suspending;
		}

		public NavigatingFromEventArgs NavigatedToEventArgs { get; private set; }
		public Dictionary<string, object> ViewModelState { get; private set; }
		public bool Suspending { get; private set; }
	}
}
