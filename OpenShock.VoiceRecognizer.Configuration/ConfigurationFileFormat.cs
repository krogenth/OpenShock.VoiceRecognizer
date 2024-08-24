using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Common.Enums;
using OpenShock.VoiceRecognizer.IO.Json;

namespace OpenShock.VoiceRecognizer.Configuration;

public class ConfigurationFileFormat
{
	public string InputDeviceID { get; set; } = string.Empty;
	public string VoskModelDirectory { get; set; } = string.Empty;
	public int OscListenPort { get; set; } = 0;
	public ShockCollarType CollarType { get; set; } = ShockCollarType.OpenShock;
	public ObservableCollection<WordRecognition> Words { get; set; } = [];
	public string OpenShockAPIKey { get; set; } = string.Empty;
	public Guid OpenShockDeviceID { get; set;} = Guid.Empty;
	public Guid OpenShockShockerID { get; set; } = Guid.Empty;
	public BrowserProxyType BrowserProxyType { get; set; } = BrowserProxyType.Chrome;
	public int BrowserProxyPort { get; set; } = 0;

	public ConfigurationFileFormat() { }

	public ConfigurationFileFormat(ConfigurationState state)
	{
		InputDeviceID = state.Audio.InputDeviceID.Value;
		VoskModelDirectory = state.Vosk.ModelDirectory.Value;
		OscListenPort = state.OSC.ListenPort.Value;
		CollarType = state.Shock.CollarType.Value;
		Words = state.Shock.Words.Value;
		OpenShockAPIKey = state.OpenShock.APIKey.Value;
		OpenShockDeviceID = state.OpenShock.DeviceID.Value;
		OpenShockShockerID = state.OpenShock.ShockerID.Value;
		BrowserProxyType = state.BrowserProxy.Proxy.Value;
		BrowserProxyPort = state.BrowserProxy.ProxyPort.Value;
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
