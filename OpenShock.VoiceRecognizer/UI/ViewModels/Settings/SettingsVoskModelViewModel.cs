using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsVoskModelViewModel : BasedSettingsViewModel
{
	private string _selectedModelDirectory = string.Empty;

	public SettingsVoskModelViewModel()
	{
		_selectedModelDirectory = ConfigurationState.Instance!.Vosk.ModelDirectory.Value;
	}

	public override void SaveToConfigurationState()
	{
		ConfigurationState.Instance!.Vosk.ModelDirectory.Value = _selectedModelDirectory;
	}

	public string SelectedModelDirectory
	{
		get => _selectedModelDirectory;
		set
		{
			_selectedModelDirectory = value;
			OnPropertyChanged(nameof(SelectedModelDirectory));
		}
	}
}
