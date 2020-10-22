// Copyright © 2018-2020 Daniel Porrey
//
// This file is part of the Color Picker Control solution.
// 
// Sensor Telemetry is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Sensor Telemetry is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with the Color Picker Control solution. If not, 
// see http://www.gnu.org/licenses/.
//
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
