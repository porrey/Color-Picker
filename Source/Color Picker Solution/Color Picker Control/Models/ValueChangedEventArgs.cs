using System;

namespace Porrey.Controls.ColorPicker
{
	public class ValueChangedEventArgs<T> : EventArgs
	{
		public ValueChangedEventArgs(T previousValue, T newValue)
		{
			this.PreviousValue = previousValue;
			this.NewValue = newValue;
		}

		public T PreviousValue { get; private set; }
		public T NewValue { get; private set; }
	}
}