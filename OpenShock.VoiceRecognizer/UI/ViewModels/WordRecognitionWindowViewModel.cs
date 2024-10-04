using System;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.UI.ViewModels.Enums;
using OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;
using OpenShock.VoiceRecognizer.Utility.Common;

namespace OpenShock.VoiceRecognizer.UI.ViewModels;

public class WordRecognitionWindowViewModel : BaseViewModel
{
	public event Action? CloseWindow;
	public event Action? SaveWord;

	public StringInputViewModel TextInputVM { get; set; }
	public ShockTypeSelectorViewModel ShockTypeSelectorVM { get; set; }
	public FloatInputViewModel MinInitialDelayInputVM { get; set; }
	public FloatInputViewModel MaxInitialDelayInputVM { get; set; }
	public FloatInputViewModel MinDelayInputVM { get; set; }
	public FloatInputViewModel MaxDelayInputVM { get; set; }
	public ByteInputViewModel MinIntensityInputVM { get; set; }
	public ByteInputViewModel MaxIntensityInputVM { get; set; }
	public UShortInputViewModel MinDurationInputVM { get; set; }
	public UShortInputViewModel MaxDurationInputVM { get; set; }
	public DoubleInputViewModel CooldownInputVM { get; set; }

	public string InputText { get; set; } = string.Empty;
	public ShockType ShockType { get; set; } = ShockType.Vibrate;
	public float MinInitialDelay { get; set; } = 0.0f;
	public float MaxInitialDelay { get; set; } = 0.0f;
	public float MinDelay { get; set; } = 0.0f;
	public float MaxDelay { get; set; } = 0.0f;
	public byte MinIntensity { get; set; } = 0;
	public byte MaxIntensity { get; set; } = 0;
	public ushort MinDuration { get; set; } = 0;
	public ushort MaxDuration { get; set; } = 0;
	public double Cooldown { get; set; } = 0.0d;

	public WordRecognitionWindowViewModel()
	{
		TextInputVM = new("Text", InputText);
		ShockTypeSelectorVM = new("Shock Type", ShockType);
		MinInitialDelayInputVM = new("Minimum Initial Delay (seconds)", MinInitialDelay);
		MaxInitialDelayInputVM = new("Maximum Initial Delay (seconds)", MaxInitialDelay);
		MinDelayInputVM = new("Minimum Delay Between (seconds)", MinDelay);
		MaxDelayInputVM = new("Maximum Delay Between (seconds)", MaxDelay);
		MinIntensityInputVM = new("Minimum Intensity (%)", MinIntensity, 0, 100);
		MaxIntensityInputVM = new("Maximum Intensity (%)", MaxIntensity, 0, 100);
		MinDurationInputVM = new("Minimum Duration (ms)", MinDuration);
		MaxDurationInputVM = new("Maximum Duration (ms)", MaxDuration);
		CooldownInputVM = new("Cooldown (ms)", Cooldown, 0, double.MaxValue, 1);

		AttachHandlers();
	}

	public WordRecognitionWindowViewModel(WordRecognition wordRecognition)
	{
		InputText = wordRecognition.Word;
		ShockType = wordRecognition.Type;
		MinInitialDelay = wordRecognition.MinInitialDelay;
		MaxInitialDelay = wordRecognition.MaxInitialDelay;
		MinDelay = wordRecognition.MinDelay;
		MaxDelay = wordRecognition.MaxDelay;
		MinIntensity = wordRecognition.MinIntensity;
		MaxIntensity = wordRecognition.MaxIntensity;
		MinDuration = wordRecognition.MinDuration;
		MaxDuration = wordRecognition.MaxDuration;
		Cooldown = wordRecognition.Cooldown;

		TextInputVM = new("Text", InputText);
		ShockTypeSelectorVM = new("Shock Type", ShockType);
		MinInitialDelayInputVM = new("Minimum Initial Delay (seconds)", MinInitialDelay);
		MaxInitialDelayInputVM = new("Maximum Initial Delay (seconds)", MaxInitialDelay);
		MinDelayInputVM = new("Minimum Delay (seconds)", MinDelay);
		MaxDelayInputVM = new("Maximum Delay (seconds)", MaxDelay);
		MinIntensityInputVM = new("Intensity (%)", MinIntensity, 0, 100);
		MaxIntensityInputVM = new("Intensity (%)", MaxIntensity, 0, 100);
		MinDurationInputVM = new("Minimum Duration (ms)", MinDuration);
		MaxDurationInputVM = new("Maximum Duration (ms)", MaxDuration);
		CooldownInputVM = new("Cooldown (ms)", Cooldown, 0, double.MaxValue, 1);

		AttachHandlers();
	}

	private void AttachHandlers()
	{
		TextInputVM.PropertyChanged += OnTextInputChanged;
		ShockTypeSelectorVM.EnumChanged += OnShockTypeSelected;
		MinInitialDelayInputVM.ValueChanged += OnMinInitialDelayInputChanged;
		MaxInitialDelayInputVM.ValueChanged += OnMaxInitialDelayInputChanged;
		MinDelayInputVM.ValueChanged += OnMinDelayInputChanged;
		MaxDelayInputVM.ValueChanged += OnMaxDelayInputChanged;
		MinIntensityInputVM.ValueChanged += OnMinIntensityInputChanged;
		MaxIntensityInputVM.ValueChanged += OnMaxIntensityInputChanged;
		MinDurationInputVM.ValueChanged += OnMinDurationInputChanged;
		MaxDurationInputVM.ValueChanged += OnMaxDurationInputChanged;
		CooldownInputVM.ValueChanged += OnCooldownInputChanged;
	}

	private void OnTextInputChanged(object? sender, EventArgs e) =>
		InputText = TextInputVM.Text ?? string.Empty;
	private void OnShockTypeSelected(object? sender, EnumChangedEventArgs<ShockType> e) =>
		ShockType = e.Value;
	private void OnMinInitialDelayInputChanged(object? sender, BaseNumberInputViewModel<float>.NumberChangedEventArgs e) =>
		MinInitialDelay = MinInitialDelayInputVM.Value;
	private void OnMaxInitialDelayInputChanged(object? sender, BaseNumberInputViewModel<float>.NumberChangedEventArgs e) =>
		MaxInitialDelay = MaxInitialDelayInputVM.Value;
	private void OnMinDelayInputChanged(object? sender, BaseNumberInputViewModel<float>.NumberChangedEventArgs e) =>
		MinDelay = MinDelayInputVM.Value;
	private void OnMaxDelayInputChanged(object? sender, BaseNumberInputViewModel<float>.NumberChangedEventArgs e) =>
		MaxDelay = MaxDelayInputVM.Value;
	private void OnMinIntensityInputChanged(object? sender, BaseNumberInputViewModel<byte>.NumberChangedEventArgs e) =>
		MinIntensity = MinIntensityInputVM.Value;
	private void OnMaxIntensityInputChanged(object? sender, BaseNumberInputViewModel<byte>.NumberChangedEventArgs e) =>
		MaxIntensity = MaxIntensityInputVM.Value;
	private void OnMinDurationInputChanged(object? sender, BaseNumberInputViewModel<ushort>.NumberChangedEventArgs e) =>
		MinDuration = MinDurationInputVM.Value;
	private void OnMaxDurationInputChanged(object? sender, BaseNumberInputViewModel<ushort>.NumberChangedEventArgs e) =>
		MaxDuration = MaxDurationInputVM.Value;
	private void OnCooldownInputChanged(object? sender, BaseNumberInputViewModel<double>.NumberChangedEventArgs e) =>
		Cooldown = CooldownInputVM.Value;

	public void Save()
	{
		SaveWord?.Invoke();
		Close();
	}

	public void Close() => CloseWindow?.Invoke();
}
