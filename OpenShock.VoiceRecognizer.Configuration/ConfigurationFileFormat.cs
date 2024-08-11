using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Common.Enums;
using OpenShock.VoiceRecognizer.IO.Json;

namespace OpenShock.VoiceRecognizer.Configuration;

public class ConfigurationFileFormat
{
	public string InputDeviceID { get; set; } = string.Empty;
	public string VoskModelDirectory { get; set; } = string.Empty;
	public int OscListenPort { get; set; } = 0;
	public ShockCollarType CollarType { get; }
	public ObservableCollection<WordRecognition> Words { get; set; } = [];

	public ConfigurationFileFormat() { }

	public ConfigurationFileFormat(ConfigurationState state)
	{
		InputDeviceID = state.Audio.InputDeviceID.Value;
		VoskModelDirectory = state.Vosk.ModelDirectory.Value;
		OscListenPort = state.OSC.ListenPort.Value;
		CollarType = state.Shock.CollarType.Value;
		Words = state.Shock.Words.Value;
	}

	public static bool TryLoad(string filepath, out ConfigurationFileFormat? configurationFileFormat)
	{
		try
		{
			configurationFileFormat = JsonHelper.DeserializeFromFile(
				filepath,
				ConfigurationFileFormatSettings.SerializerContext.ConfigurationFileFormat
			);

			return configurationFileFormat != null;
		}
		catch
		{
			configurationFileFormat = null;
			return false;
		}
	}

	public void SaveFile(string filepath)
	{
		JsonHelper.SerializeToFile(
			filepath,
			this,
			ConfigurationFileFormatSettings.SerializerContext.ConfigurationFileFormat
		);
	}
}
