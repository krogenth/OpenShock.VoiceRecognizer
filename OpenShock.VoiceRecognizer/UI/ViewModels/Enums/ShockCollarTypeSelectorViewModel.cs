using OpenShock.VoiceRecognizer.Common.Enums;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Enums;

public sealed class ShockCollarTypeSelectorViewModel(string title, ShockCollarType initialValue)
: BaseEnumSelectorViewModel<ShockCollarType>(title, initialValue)
{
}
