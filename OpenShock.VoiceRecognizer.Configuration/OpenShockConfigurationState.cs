using OpenShock.VoiceRecognizer.Common;

namespace OpenShock.VoiceRecognizer.Configuration;

public class OpenShockConfigurationState
{
	/*public ReactiveObject<string> APIKey { get; private set; }
	public ReactiveObject<string> GroupName { get; private set; }*/
	public ReactiveObject<string> SendHost { get; private set; }
	public ReactiveObject<int> SendPort { get; private set; }

	public ReactiveObject<string> ExternalListenSetIntensityEndpoint { get; set; }
	public ReactiveObject<string> ExternalSendStartShockEndpoint { get; set; }
	public ReactiveObject<string> ExternalSendStartVibrationEndpoint { get; set; }

	public OpenShockConfigurationState()
	{
		/*APIKey = new(string.Empty);
		GroupName = new(string.Empty);*/
		SendHost = new(string.Empty);
		SendPort = new(9006);
		ExternalListenSetIntensityEndpoint = new(string.Empty);
		ExternalSendStartShockEndpoint = new(string.Empty);
		ExternalSendStartVibrationEndpoint = new(string.Empty);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat)
	{
		/*APIKey.Value = configurationFileFormat.OpenShockAPIKey;
		GroupName.Value = configurationFileFormat.OpenShockGroupName;*/
		SendHost.Value = configurationFileFormat.OpenShockSendHost;
		SendPort.Value = configurationFileFormat.OpenShockSendPort;
		ExternalListenSetIntensityEndpoint.Value = configurationFileFormat.OpenShockOscListenIntensityEndpoint;
		ExternalSendStartShockEndpoint.Value = configurationFileFormat.OpenShockOscSendShockEndpoint;
		ExternalSendStartVibrationEndpoint.Value = configurationFileFormat.OpenShockOscSendVibrateEndpoint;
	}

	public void LoadDefaultConfiguration()
	{
		/*APIKey.Value = string.Empty;
		GroupName.Value = string.Empty;*/
		SendHost.Value = "127.0.0.1";
		SendPort.Value = 0;
		ExternalListenSetIntensityEndpoint.Value = string.Empty;
		ExternalSendStartShockEndpoint.Value = string.Empty;
		ExternalSendStartVibrationEndpoint.Value = string.Empty;
	}
}
