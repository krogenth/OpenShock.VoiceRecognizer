using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Common.Audio;
using OpenShock.VoiceRecognizer.Common.Enums;

namespace OpenShock.VoiceRecognizer.Configuration;

public class GeneralConfigurationState
{
	public ReactiveObject<RecognizerType> Recognizer { get; private set; }
	public ReactiveObject<ShockCollarType> CollarType { get; private set; }
	public ReactiveObject<string> InputDeviceID { get; private set; }

	public GeneralConfigurationState()
	{
		Recognizer = new(RecognizerType.Vosk);
		CollarType = new(ShockCollarType.OpenShock);
		InputDeviceID = new(string.Empty);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat)
	{
		Recognizer.Value = configurationFileFormat.RecognizerType;
		CollarType.Value = configurationFileFormat.CollarType;
		InputDeviceID.Value = configurationFileFormat.InputDeviceID;
	}

	public void LoadDefaultConfiguration()
	{
		CollarType.Value = ShockCollarType.OpenShock;
		InputDeviceID.Value = AudioDevices.GetInputAudioDevices().First().ID;
	}
}
