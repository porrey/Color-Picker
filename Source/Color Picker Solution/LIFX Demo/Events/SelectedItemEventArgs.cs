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
