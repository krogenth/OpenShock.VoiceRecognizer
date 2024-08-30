using System;
using System.Numerics;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

public abstract class BaseNumberInputViewModel<T>(string title, T initialValue, T minValue, T maxValue, double increment = 1.0d) : BaseViewModel
	where T : IComparable<T>, IEquatable<T>, IMinMaxValue<T>
{
	private T _value = initialValue;
	public event EventHandler<NumberChangedEventArgs>? ValueChanged;

	public string Title { get; } = title;

	public T Value
	{
		get => _value;
		set
		{
			_value = value;
			ValueChanged?.Invoke(this, new NumberChangedEventArgs(value));
		}
	}

	public T MinValue { get; } = minValue;
	public T MaxValue { get; } = maxValue;
	public double Increment { get; } = increment;

	public class NumberChangedEventArgs(T value) : EventArgs
	{
		public T Value { get; set; } = value;
	}

}
