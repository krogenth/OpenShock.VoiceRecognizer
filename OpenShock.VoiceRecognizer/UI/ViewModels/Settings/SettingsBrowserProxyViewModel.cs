using OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;
using OpenShock.VoiceRecognizer.UI.ViewModels.Enums;
using OpenShock.VoiceRecognizer.Common.Enums;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Settings;

public class SettingsBrowserProxyViewModel : BasedSettingsViewModel
{
	public IntInputViewModel ProxyPortVM { get; }
	public BrowserProxyTypeSelectorViewModel BrowserProxyVM { get; }

	private int _proxyPort;
	private BrowserProxyType _browserProxyType;

	public SettingsBrowserProxyViewModel()
	{
		_proxyPort = ConfigurationState.Instance!.BrowserProxy.ProxyPort.Value;
		_browserProxyType = ConfigurationState.Instance!.BrowserProxy.BrowserProxy.Value;

		ProxyPortVM = new(
			"Proxy Port",
			_proxyPort
		);

		BrowserProxyVM = new(
			"Browser",
			_browserProxyType
		);

		AttachEventHandlers();
	}

	private void AttachEventHandlers()
	{
		ProxyPortVM.ValueChanged += ProxyPortChanged;
		BrowserProxyVM.EnumChanged += BrowserProxyTypeChagned;
	}

	public override void SaveToConfigurationState()
	{
		ConfigurationState.Instance!.BrowserProxy.ProxyPort.Value = _proxyPort;
		ConfigurationState.Instance!.BrowserProxy.BrowserProxy.Value = _browserProxyType;
	}

	private void ProxyPortChanged(object? sender, IntInputViewModel.NumberChangedEventArgs e) =>
		_proxyPort = e.Value;

	private void BrowserProxyTypeChagned(object? sender, EnumChangedEventArgs<BrowserProxyType> e) =>
		_browserProxyType = e.Value;
}
