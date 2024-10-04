using OpenShock.VoiceRecognizer.Common.Enums;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Enums;

public sealed class BrowserProxyTypeSelectorViewModel(string title, BrowserProxyType initialValue)
	: BaseEnumSelectorViewModel<BrowserProxyType>(title, initialValue)
{
}
