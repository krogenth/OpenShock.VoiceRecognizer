namespace OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

public class UIntInputViewModel(
	string title,
	uint initialValue,
	uint minValue = uint.MinValue,
	uint maxValue = uint.MaxValue,
	double increment = 1.0d
) : BaseNumberInputViewModel<uint>(title, initialValue, minValue, maxValue, increment)
{

}
