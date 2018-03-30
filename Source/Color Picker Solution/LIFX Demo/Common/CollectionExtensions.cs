using System.Collections.Generic;
using System.Linq;

namespace System.Collections.ObjectModel
{
	public static class CollectionExtensions
	{
		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
		{
			ObservableCollection<T> returnValue = new ObservableCollection<T>();

			foreach (var item in items)
			{
				returnValue.Add(item);
			}

			return returnValue;
		}

		public static ObservableCollection<T> ToObservableCollection<T>(this IQueryable<T> items)
		{
			ObservableCollection<T> returnValue = new ObservableCollection<T>();

			foreach (var item in items)
			{
				returnValue.Add(item);
			}

			return returnValue;
		}

		public static void AddRange<T>(this ObservableCollection<T> target, IEnumerable<T> source)
		{
			foreach (var item in source)
			{
				target.Add(item);
			}
		}

		public static void AddRange<T>(this ObservableCollection<T> target, IQueryable<T> source)
		{
			foreach (var item in source)
			{
				target.Add(item);
			}
		}
	}
}
