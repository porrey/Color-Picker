using System;
using Prism.Events;

namespace LifxDemo.Events
{
	public class ApplicationReadyEvent : PubSubEvent<ApplicationReadyEventArgs> { }
	public class ApplicationClosingEvent : PubSubEvent<ApplicationClosingEventArgs> { }
	public class SelectedItemEvent : PubSubEvent<SelectedItemEventArgs> { }
	public class MessageEvent : PubSubEvent<MessageEventArgs> { }
	public class RefreshEvent : PubSubEvent<RefreshEventArgs> { }
}
