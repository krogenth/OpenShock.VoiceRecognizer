using System;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsWindowViewModel
{
	public event Action? CloseWindow;

	public SettingsGeneralViewModel GeneralVM { get; }
	public SettingsVoskModelViewModel VoskVM { get; }

	public SettingsWindowViewModel()
	{
		GeneralVM = new();
		VoskVM = new();
	}

	public void SaveSettings()
	{
		ConfigurationState.Instance!.SaveConfigurationStateToFile();
		CloseWindow?.Invoke();
	}
}
