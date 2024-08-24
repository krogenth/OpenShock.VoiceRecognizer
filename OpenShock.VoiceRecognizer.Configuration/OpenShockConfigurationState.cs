using System.Net.Sockets;
using OpenShock.VoiceRecognizer.Common;

namespace OpenShock.VoiceRecognizer.Configuration;

public class OpenShockConfigurationState
{
	public ReactiveObject<string> APIKey { get; private set; }
	public ReactiveObject<Guid> DeviceID { get; private set; }
	public ReactiveObject<Guid> ShockerID { get; private set; }

	public OpenShockConfigurationState()
	{
		APIKey = new(string.Empty);
		DeviceID = new(Guid.Empty);
		ShockerID = new(Guid.Empty);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat)
	{
		APIKey.Value = configurationFileFormat.OpenShockAPIKey;
		DeviceID.Value = configurationFileFormat.OpenShockDeviceID;
		ShockerID.Value = configurationFileFormat.OpenShockShockerID;
	}

	public void LoadDefaultConfiguration()
	{
		APIKey.Value = string.Empty;
		DeviceID.Value = Guid.Empty;
		ShockerID.Value = Guid.Empty;
	}
}
