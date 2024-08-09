using OpenShock.VoiceRecognizer.Common;

namespace OpenShock.VoiceRecognizer.Configuration;

public class OSCConfigurationState
{
	public ReactiveObject<int> ListenPort { get; private set; }

	public OSCConfigurationState()
	{
		ListenPort = new(9005);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat) =>
		ListenPort.Value = configurationFileFormat.OscListenPort;

	public void LoadDefaultConfiguration() =>
		ListenPort = new(9005);
}
