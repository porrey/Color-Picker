using System;
using System.Threading;
using System.Threading.Tasks;
using LifxDemo.Common;
using Windows.UI.Xaml;

namespace LifxDemo.Models
{
	public class ApiRateLimiter : IDisposable
	{
		private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
		private bool _locked = false;

		protected Action Callback { get; set; }
		protected DateTime LastCall { get; set; } = DateTime.MinValue;
		protected Action LastSkippedAction { get; set; }
		protected DispatcherTimer CallbackTimer { get; } = new DispatcherTimer();
		protected TimeSpan CallbackDelay { get; set; }

		public ApiRateLimiter(TimeSpan interval, Action callback, TimeSpan callbackDelay)
		{
			this.Interval = interval;
			this.Callback = callback;
			this.CallbackDelay = callbackDelay;

			this.CallbackTimer.Interval = this.CallbackDelay;
			this.CallbackTimer.Tick += this.CallbackTimer_Tick;
		}

		private void CallbackTimer_Tick(object sender, object e)
		{
			try
			{
				// ***
				// *** Stop the timer, it only needs to fire once.
				// ***
				this.CallbackTimer.Stop();

				// ***
				// *** Execute the last skipped action.
				// ***
				if (this.LastSkippedAction != null)
				{
					Task.Factory.StartNew(this.LastSkippedAction).RunAsync();
				}
			}
			finally
			{
				// ***
				// *** execute the callback.
				// ***
				this.Callback.Invoke();
			}
		}

		public Task Lock()
		{
			_lock.EnterWriteLock();

			try
			{
				_locked = true;
			}
			finally
			{
				_lock.ExitWriteLock();
			}

			return Task.FromResult(0);
		}

		public Task Unlock()
		{
			_lock.EnterWriteLock();

			try
			{
				_locked = false;
			}
			finally
			{
				_lock.ExitWriteLock();
			}

			return Task.FromResult(0);
		}

		public Task<bool> IsLocked()
		{
			bool returnValue = false;

			_lock.EnterReadLock();
			try
			{
				returnValue = _locked;
			}
			finally
			{
				_lock.ExitReadLock();
			}

			return Task.FromResult(returnValue);
		}

		public TimeSpan Interval { get; set; }

		public async void ThrottleMethod(Action action)
		{
			if (!await this.IsLocked())
			{
				System.Diagnostics.Debug.WriteLine("Not Locked");

				// ***
				// *** Must wait at least 50ms between calls per the documentation.
				// ***
				if (DateTime.Now.Subtract(this.LastCall) > this.Interval)
				{
					try
					{
						// ***
						// *** Execute the action.
						// ***
						Task.Factory.StartNew(action).RunAsync();
					}
					finally
					{
						// ***
						// *** Mark the time of the last action.
						// ***
						this.LastCall = DateTime.Now;

						// ***
						// *** Reset the last skipped action so that an older
						// *** action is not executed after a newer action.
						// ***
						this.LastSkippedAction = null;

						if (this.CallbackDelay != TimeSpan.Zero)
						{
							// ***
							// *** Start the timer; each time the time is started it will
							// *** reset. This prevents the timer event from being fired until
							// *** the full interval has elapsed. For example, if the timer
							// *** interval is set to 1 second, and the Start() method is called
							// *** 100 times in a second, the timer event will fire just once.
							// ***
							this.CallbackTimer.Start();
						}
					}
				}
				else
				{
					// ***
					// *** Store the last skipped action.
					// ***
					this.LastSkippedAction = action;
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("LOCKED");
			}
		}

		public void Dispose()
		{
			this.CallbackTimer.Stop();
			this.CallbackTimer.Tick -= this.CallbackTimer_Tick;
		}
	}
}
