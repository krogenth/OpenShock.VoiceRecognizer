using OpenShock.VoiceRecognizer.Common.Enums;

namespace OpenShock.VoiceRecognizer.UI.ViewModels.Enums;

public sealed class AudioDeviceTypeSelectorViewModel(string title, AudioDeviceType initialValue)
	: BaseEnumSelectorViewModel<AudioDeviceType>(title, initialValue)
{
}
