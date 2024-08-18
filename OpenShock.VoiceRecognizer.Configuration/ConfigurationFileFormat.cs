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
	/*public string OpenShockAPIKey { get; set; } = string.Empty;
	public string OpenShockGroupName { get; set; } = string.Empty;*/
	public string OpenShockSendHost {  get; set; } = string.Empty;
	public int OpenShockSendPort { get; set; } = 0;
	public string OpenShockOscListenIntensityEndpoint { get; set; } = string.Empty;
	public string OpenShockOscSendShockEndpoint { get; set; } = string.Empty;
	public string OpenShockOscSendVibrateEndpoint { get; set; } = string.Empty;
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
		/*OpenShockAPIKey = state.OpenShock.APIKey.Value;
		OpenShockGroupName = state.OpenShock.GroupName.Value;*/
		OpenShockSendHost = state.OpenShock.SendHost.Value;
		OpenShockSendPort = state.OpenShock.SendPort.Value;
		OpenShockOscListenIntensityEndpoint = state.OpenShock.ExternalListenSetIntensityEndpoint;
		OpenShockOscSendShockEndpoint = state.OpenShock.ExternalSendStartShockEndpoint;
		OpenShockOscSendVibrateEndpoint = state.OpenShock.ExternalSendStartShockEndpoint;
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
