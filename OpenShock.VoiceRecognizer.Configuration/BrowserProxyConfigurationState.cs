using OpenShock.VoiceRecognizer.Common;
using OpenShock.VoiceRecognizer.Common.Audio;
using OpenShock.VoiceRecognizer.Common.Enums;

namespace OpenShock.VoiceRecognizer.Configuration;

public class BrowserProxyConfigurationState
{
	public ReactiveObject<BrowserProxyType> BrowserProxy { get; set; }
	public ReactiveObject<int> ProxyPort { get; set; }

	public BrowserProxyConfigurationState()
	{
		BrowserProxy = new(BrowserProxyType.Chrome);
		ProxyPort = new(0);
	}

	public void LoadFileConfiguration(ConfigurationFileFormat configurationFileFormat)
	{
		BrowserProxy.Value = configurationFileFormat.BrowserProxyType;
		ProxyPort.Value = configurationFileFormat.BrowserProxyPort;
	}

	public void LoadDefaultConfiguration()
	{
		BrowserProxy.Value = BrowserProxyType.Chrome;
		ProxyPort.Value = 0;
	}
}
