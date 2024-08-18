using System;
using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.UI.ViewModels.Enums;
using OpenShock.VoiceRecognizer.Utility.Common;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsZapViewModel : BaseViewModel
{
	private int _selectedWordIndex;
	public ObservableCollection<WordRecognition> Words { get; set; }

	public StringInputViewModel TextInputVM { get; set; }
	public ShockTypeSelectorViewModel ShockTypeSelectorVM { get; set; }
	public NumberInputViewModel MinDelayInputVM { get; set; }
	public NumberInputViewModel MaxDelayInputVM { get; set; }

	public SettingsZapViewModel()
	{
		_selectedWordIndex = -1;
		Words = ConfigurationState.Instance!.Shock.Words.Value;
		InputText = string.Empty;
		ShockType = ShockType.VibrateThenShock;
		MinDelay = 0.0f;
		MaxDelay = 0.0f;

		TextInputVM = new("Text", string.Empty);
		ShockTypeSelectorVM = new("Shock Type", ShockType);
		MinDelayInputVM = new("Minimum Delay (seconds)", 0);
		MaxDelayInputVM = new("Maximum Delay (seconds)", 0);

		AttachHandlers();
	}

	private void AttachHandlers()
	{
		TextInputVM.PropertyChanged += OnTextInputChanged;
		ShockTypeSelectorVM.EnumChanged += OnShockTypeSelected;
		MinDelayInputVM.PropertyChanged += OnMinDelayInputChanged;
		MaxDelayInputVM.PropertyChanged += OnMaxDelayInputChanged;
	}

	private void OnTextInputChanged(object? sender, EventArgs e) =>
		InputText = TextInputVM.Text ?? string.Empty;
	private void OnShockTypeSelected(object? sender, EnumChangedEventArgs<ShockType> e) =>
		ShockType = e.Value;
	private void OnMinDelayInputChanged(object? sender, EventArgs e) =>
		MinDelay = MinDelayInputVM.Value;
	private void OnMaxDelayInputChanged(object? sender, EventArgs e) =>
		MaxDelay = MaxDelayInputVM.Value;

	public int SelectedWordIndex
	{
		get => _selectedWordIndex;
		set
		{
			_selectedWordIndex = value;
			OnPropertyChanged(nameof(SelectedWordIndex));
			OnPropertyChanged(nameof(HasSelectedWord));
		}
	}

	public string InputText { get; set; }
	public ShockType ShockType { get; set; }
	public float MinDelay { get; set; }
	public float MaxDelay { get; set; }

	public bool HasSelectedWord => SelectedWordIndex <= Words.Count && SelectedWordIndex > -1;
}
