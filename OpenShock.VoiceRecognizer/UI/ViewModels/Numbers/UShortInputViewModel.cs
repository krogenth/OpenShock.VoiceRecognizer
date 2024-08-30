namespace OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

public class UShortInputViewModel(
	string title,
	ushort initialValue,
	ushort minValue = ushort.MinValue,
	ushort maxValue = ushort.MaxValue,
	double increment = 1.0d
) : BaseNumberInputViewModel<ushort>(title, initialValue, minValue, maxValue, increment)
{

}
