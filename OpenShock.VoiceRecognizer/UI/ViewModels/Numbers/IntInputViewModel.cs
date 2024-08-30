namespace OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

public class IntInputViewModel(
	string title,
	int initialValue,
	int minValue = int.MinValue,
	int maxValue = int.MaxValue,
	double increment = 1.0d
) : BaseNumberInputViewModel<int>(title, initialValue, minValue, maxValue, increment)
{

}
