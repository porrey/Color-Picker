﻿// Copyright © 2018-2022 Daniel Porrey
//
// This file is part of the Color Picker Control solution.
// 
// Color Picker Control is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Color Picker Control is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with the Color Picker Control solution. If not, 
// see http://www.gnu.org/licenses/.
//
using LifxDemo.ViewModels;
using System;

namespace LifxDemo.Events
{
	public class SelectedItemEventArgs : EventArgs
	{
		public SelectedItemEventArgs(LifxItem lifxBulb)
		{
			this.LifxBulb = lifxBulb;
		}

		public LifxItem LifxBulb { get; private set; }
	}
}
