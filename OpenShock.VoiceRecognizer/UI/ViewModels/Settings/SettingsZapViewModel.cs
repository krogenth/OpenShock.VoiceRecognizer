using System;
using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.UI.ViewModels.Enums;
using OpenShock.VoiceRecognizer.Utility.Common;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsZapViewModel : BasedSettingsViewModel
{
	private int _selectedWordIndex;
	public ObservableCollection<WordRecognition> Words { get; set; }

	public StringInputViewModel TextInputVM { get; set; }
	public ShockTypeSelectorViewModel ShockTypeSelectorVM { get; set; }
	public NumberInputViewModel MinInitialDelayInputVM { get; set; }
	public NumberInputViewModel MaxInitialDelayInputVM { get; set; }
	public NumberInputViewModel MinDelayInputVM { get; set; }
	public NumberInputViewModel MaxDelayInputVM { get; set; }
	public NumberInputViewModel IntensityInputVM { get; set; }
	public NumberInputViewModel MinDurationInputVM { get; set; }
	public NumberInputViewModel MaxDurationInputVM { get; set; }

	public SettingsZapViewModel()
	{
		_selectedWordIndex = -1;
		Words = new(ConfigurationState.Instance!.Shock.Words.Value);

		TextInputVM = new("Text", InputText);
		ShockTypeSelectorVM = new("Shock Type", ShockType);
		MinInitialDelayInputVM = new("Minimum Initial Delay (seconds)", 0);
		MaxInitialDelayInputVM = new("Maximum Initial Delay (seconds)", 0);
		MinDelayInputVM = new("Minimum Delay (seconds)", 0);
		MaxDelayInputVM = new("Maximum Delay (seconds)", 0);
		IntensityInputVM = new("Intensity", Intensity);
		MinDurationInputVM = new("Minimum Duration (seconds)", MinDuration);
		MaxDurationInputVM = new("Maximum Duration (seconds)", MaxDuration);

		AttachHandlers();
	}

	private void AttachHandlers()
	{
		TextInputVM.PropertyChanged += OnTextInputChanged;
		ShockTypeSelectorVM.EnumChanged += OnShockTypeSelected;
		MinInitialDelayInputVM.PropertyChanged += OnMinInitialDelayInputChanged;
		MaxInitialDelayInputVM.PropertyChanged += OnMaxInitialDelayInputChanged;
		MinDelayInputVM.PropertyChanged += OnMinDelayInputChanged;
		MaxDelayInputVM.PropertyChanged += OnMaxDelayInputChanged;
		IntensityInputVM.PropertyChanged += OnIntensityInputChanged;
		MinDurationInputVM.PropertyChanged += OnMinDurationInputChanged;
		MaxDurationInputVM.PropertyChanged += OnMaxDurationInputChanged;
	}

	private void OnTextInputChanged(object? sender, EventArgs e) =>
		InputText = TextInputVM.Text ?? string.Empty;
	private void OnShockTypeSelected(object? sender, EnumChangedEventArgs<ShockType> e) =>
		ShockType = e.Value;
	private void OnMinInitialDelayInputChanged(object? sender, EventArgs e) =>
		MinDelay = MinInitialDelayInputVM.Value;
	private void OnMaxInitialDelayInputChanged(object? sender, EventArgs e) =>
		MaxDelay = MaxInitialDelayInputVM.Value;
	private void OnMinDelayInputChanged(object? sender, EventArgs e) =>
		MinDelay = MinDelayInputVM.Value;
	private void OnMaxDelayInputChanged(object? sender, EventArgs e) =>
		MaxDelay = MaxDelayInputVM.Value;
	private void OnIntensityInputChanged(object? sender, EventArgs e) =>
		MinDelay = IntensityInputVM.Value;
	private void OnMinDurationInputChanged(object? sender, EventArgs e) =>
		MinDelay = MinDurationInputVM.Value;
	private void OnMaxDurationInputChanged(object? sender, EventArgs e) =>
		MaxDelay = MaxDurationInputVM.Value;

	public override void SaveToConfigurationState()
	{
		ConfigurationState.Instance!.Shock.Words.Value = Words;
	}

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

	public string InputText { get; set; } = string.Empty;
	public ShockType ShockType { get; set; } = ShockType.Vibrate;
	public float MinInitialDelay { get; set; } = 0.0f;
	public float MaxInitialDelay { get; set; } = 0.0f;
	public float MinDelay { get; set; } = 0.0f;
	public float MaxDelay { get; set; } = 0.0f;
	public byte Intensity { get; set; } = 0;
	public ushort MinDuration { get; set; } = 0;
	public ushort MaxDuration { get; set; } = 0;

	public bool HasSelectedWord => SelectedWordIndex <= Words.Count && SelectedWordIndex > -1;
}
