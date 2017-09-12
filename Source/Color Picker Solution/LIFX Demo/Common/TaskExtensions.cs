using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace LifxDemo.Common
{
	/// <summary>
	/// Defines extension methods for objects of type Task<typeparamref name="T"/>
	/// </summary>
	public static class MyTaskExtensions
	{
		/// <summary>
		/// Waits for an async method to complete and returns the result.
		/// </summary>
		/// <typeparam name="T">The type of the result.</typeparam>
		/// <param name="task">The Task object that is awaited.</param>
		/// <returns>Returns the result of the completed task.</returns>
		public static T WaitForResult<T>(this Task<T> task)
		{
			task.Wait();
			return task.Result;
		}

		public static void RunAsync(this Task task)
		{
			task.ContinueWith(t => { if (t.Exception != null) throw t.Exception; });
		}

		public static async Task RunOnUiThread(this DispatchedHandler agileCallback)
		{
			CoreApplicationView view = CoreApplication.MainView; ;

			if (view == null)
			{
				try
				{
					view = CoreApplication.GetCurrentView();
				}
				catch
				{
					view = null;
				}
			}

			if (view != null)
			{
				if (view.CoreWindow != null)
				{
					if (view.CoreWindow.Dispatcher != null)
					{
						await view.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, agileCallback);
					}
					else
					{
						// ***
						// *** Run on the current thread since there is no dispatcher.
						// ***
						agileCallback.Invoke();
					}
				}
				else
				{
					// ***
					// *** Run on the current thread since there is no core window.
					// ***
					agileCallback.Invoke();
				}
			}
			else
			{
				// ***
				// *** Run on the current thread since there is no current view.
				// ***
				agileCallback.Invoke();
			}
		}
	}
}
