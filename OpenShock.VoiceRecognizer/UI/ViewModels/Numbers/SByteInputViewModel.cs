namespace OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

public class SByteInputViewModel(
	string title,
	sbyte initialValue,
	sbyte minValue = sbyte.MinValue,
	sbyte maxValue = sbyte.MaxValue,
	double increment = 1.0d
) : BaseNumberInputViewModel<sbyte>(title, initialValue, minValue, maxValue, increment)
{

}
