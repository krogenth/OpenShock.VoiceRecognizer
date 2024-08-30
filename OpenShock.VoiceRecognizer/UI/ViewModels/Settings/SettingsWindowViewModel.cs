using System;
using OpenShock.VoiceRecognizer.Configuration;
using OpenShock.VoiceRecognizer.UI.Windows;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsWindowViewModel(SettingsWindow window)
{
	public event Action? CloseWindow;

	public SettingsGeneralViewModel GeneralVM { get; } = new();
	public SettingsVoskModelViewModel VoskVM { get; } = new();
	public SettingsZapViewModel ZapVM { get; } = new(window);
	public SettingsOpenShockViewModel OpenShockVM { get; } = new();

	public void SaveSettings()
	{
		GeneralVM.SaveToConfigurationState();
		VoskVM.SaveToConfigurationState();
		ZapVM.SaveToConfigurationState();
		OpenShockVM.SaveToConfigurationState();
		ConfigurationState.Instance!.SaveConfigurationStateToFile();
	}

	public void Close() => CloseWindow?.Invoke();
}
