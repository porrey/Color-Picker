using System;

namespace LifxDemo.Events
{
	public class RefreshEventArgs : EventArgs
	{
		public RefreshEventArgs(bool rediscover)
		{
			this.Rediscover = rediscover;
		}

		public bool Rediscover { get; private set; }
	}
}