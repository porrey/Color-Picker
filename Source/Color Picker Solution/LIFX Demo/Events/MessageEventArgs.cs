using System;

namespace LifxDemo.Events
{
	public enum MessageEventType
	{
		None,
		Information,
		Warning,
		Error
	}

	public class MessageEventArgs : EventArgs
	{
		public MessageEventArgs(MessageEventType type, string message)
		{
			this.Type = type;
			this.Message = message;
		}

		public MessageEventArgs(Exception ex)
		{
			this.Type = MessageEventType.Error;
			this.Message = ex.Message;
		}

		public string Message { get; private set; }
		public MessageEventType Type { get; private set; }
	}
}
