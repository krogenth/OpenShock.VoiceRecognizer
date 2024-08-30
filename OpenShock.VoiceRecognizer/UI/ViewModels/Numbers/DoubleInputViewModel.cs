namespace OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

public class DoubleInputViewModel(
	string title,
	double initialValue,
	double minValue = double.MinValue,
	double maxValue = double.MaxValue,
	double increment = 0.1d
) : BaseNumberInputViewModel<double>(title, initialValue, minValue, maxValue, increment)
{

}
