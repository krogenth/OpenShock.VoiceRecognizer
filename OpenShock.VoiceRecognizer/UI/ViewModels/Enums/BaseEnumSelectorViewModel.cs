using System;
using System.Collections.Generic;
using OpenShock.VoiceRecognizer.Common.Enums;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Enums;

public abstract class BaseEnumSelectorViewModel<T> : BaseViewModel
	where T : struct, Enum
{
	private int _selectedValueIndex;

	public event EventHandler<EnumChangedEventArgs<T>>? EnumChanged;

	public IList<T> Items { get; set; }
	public string Title { get; }

	public BaseEnumSelectorViewModel(string title, T initialValue)
	{
		Items = EnumExtensions.GetEnumList<T>();
		Title = title;

		SelectedValueIndex = Items.IndexOf(initialValue);
	}

	public int SelectedValueIndex
	{
		get => _selectedValueIndex;
		set
		{
			if (Items.Count == 0)
			{
				return;
			}

			if (value >= Items.Count)
			{
				value = Items.Count - 1;
			}

			if (value < 0)
			{
				value = 0;
			}

			_selectedValueIndex = value;
			OnPropertyChanged(nameof(SelectedValueIndex));
			EnumChanged?.Invoke(this, new EnumChangedEventArgs<T>(Items[value]));
		}
	}

	public T SelectedValue {
		get
		{
			if (SelectedValueIndex < Items.Count && SelectedValueIndex >= 0)
			{
				return Items[SelectedValueIndex];
			}

			return default;
		}
	}
}

public class EnumChangedEventArgs<T>(T value) : EventArgs
	where T : struct, Enum
{
	public T Value { get; set; } = value;
}
