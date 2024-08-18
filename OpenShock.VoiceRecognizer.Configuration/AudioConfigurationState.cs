using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Common.Audio;

namespace OpenShock.VoiceRecognizer.Configuration;

public class AudioConfigurationState
{
	public ReactiveObject<string> InputDeviceID { get; private set; }

	public AudioConfigurationState()
	{
		InputDeviceID = new(string.Empty);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat) =>
		InputDeviceID.Value = configurationFileFormat.InputDeviceID;

	public void LoadDefaultConfiguration() =>
		InputDeviceID.Value = AudioDevices.GetInputAudioDevices().First().ID;
}
