// Copyright © 2018-2022 Daniel Porrey
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
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.ObjectModel
{
	public static class CollectionExtensions
	{
		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
		{
			ObservableCollection<T> returnValue = new ObservableCollection<T>();

			foreach (T item in items)
			{
				returnValue.Add(item);
			}

			return returnValue;
		}

		public static ObservableCollection<T> ToObservableCollection<T>(this IQueryable<T> items)
		{
			ObservableCollection<T> returnValue = new ObservableCollection<T>();

			foreach (T item in items)
			{
				returnValue.Add(item);
			}

			return returnValue;
		}

		public static void AddRange<T>(this ObservableCollection<T> target, IEnumerable<T> source)
		{
			foreach (T item in source)
			{
				target.Add(item);
			}
		}

		public static void AddRange<T>(this ObservableCollection<T> target, IQueryable<T> source)
		{
			foreach (T item in source)
			{
				target.Add(item);
			}
		}
	}
}
