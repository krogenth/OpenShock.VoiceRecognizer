namespace OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

public class ShortInputViewModel(
	string title,
	short initialValue,
	short minValue = short.MinValue,
	short maxValue = short.MaxValue,
	double increment = 1.0d
) : BaseNumberInputViewModel<short>(title, initialValue, minValue, maxValue, increment)
{

}
