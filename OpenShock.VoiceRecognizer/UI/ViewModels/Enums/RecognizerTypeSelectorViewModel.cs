using OpenShock.VoiceRecognizer.Common.Enums;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Enums;

public sealed class RecognizerTypeSelectorViewModel(string title, RecognizerType initialValue)
	: BaseEnumSelectorViewModel<RecognizerType>(title, initialValue)
{
}
