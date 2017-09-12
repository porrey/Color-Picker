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
