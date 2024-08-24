using System;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsWindowViewModel
{
	public event Action? CloseWindow;

	public SettingsGeneralViewModel GeneralVM { get; }
	public SettingsVoskModelViewModel VoskVM { get; }
	public SettingsZapViewModel ZapVM { get; }
	public SettingsOpenShockViewModel OpenShockVM { get; }

	public SettingsWindowViewModel()
	{
		GeneralVM = new();
		VoskVM = new();
		ZapVM = new();
		OpenShockVM = new();
	}

	public void SaveSettings()
	{
		GeneralVM.SaveToConfigurationState();
		VoskVM.SaveToConfigurationState();
		ZapVM.SaveToConfigurationState();
		OpenShockVM.SaveToConfigurationState();
		ConfigurationState.Instance!.SaveConfigurationStateToFile();
		CloseWindow?.Invoke();
	}
}
