using OpenShock.VoiceRecognizer.Utility.Common;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Enums;

public sealed class ShockTypeSelectorViewModel(string title, ShockType initialValue)
	: BaseEnumSelectorViewModel<ShockType>(title, initialValue)
{
}
