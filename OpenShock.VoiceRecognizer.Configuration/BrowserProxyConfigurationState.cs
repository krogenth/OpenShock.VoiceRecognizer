using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Common.Audio;
using OpenShock.VoiceRecognizer.Common.Enums;

namespace OpenShock.VoiceRecognizer.Configuration;

public class BrowserProxyConfigurationState
{
	public ReactiveObject<BrowserProxyType> Proxy { get; set; }
	public ReactiveObject<int> ProxyPort { get; set; }

	public BrowserProxyConfigurationState()
	{
		Proxy = new(BrowserProxyType.Chrome);
		ProxyPort = new(0);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat)
	{
		Proxy.Value = configurationFileFormat.BrowserProxyType;
		ProxyPort.Value = configurationFileFormat.BrowserProxyPort;
	}

	public void LoadDefaultConfiguration()
	{
		Proxy.Value = BrowserProxyType.Chrome;
		ProxyPort.Value = 0;
	}
}
