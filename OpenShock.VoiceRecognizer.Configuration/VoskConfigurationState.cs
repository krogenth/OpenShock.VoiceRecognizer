using OpenShock.VoiceRecognizer.Common;

namespace OpenShock.VoiceRecognizer.Configuration;

public class VoskConfigurationState
{
	public ReactiveObject<string> ModelDirectory { get; private set; }

	public VoskConfigurationState()
	{
		ModelDirectory = new(string.Empty);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat) =>
		ModelDirectory.Value = configurationFileFormat.VoskModelDirectory;

	public void LoadDefaultConfiguration() =>
		ModelDirectory = new(AppDomain.CurrentDomain.BaseDirectory);
}
