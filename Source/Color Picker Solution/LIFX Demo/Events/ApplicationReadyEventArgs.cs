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
