using System;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class NumberInputViewModel : BaseViewModel
{
	private int _value;

	public event EventHandler<NumberValueChangedEventArgs>? NumberValueChanged;

	public NumberInputViewModel(string label, int value)
	{
		Value = value;
		Label = label;
	}

	public int Value
	{
		get => _value;
		set
		{
			_value = value;
			OnPropertyChanged(nameof(Value));
			NumberValueChanged?.Invoke(this, new NumberValueChangedEventArgs(value));
		}
	}

	public string Label { get; }
}

public class NumberValueChangedEventArgs(int value) : EventArgs
{
	public int Value { get; set; } = value;
}
